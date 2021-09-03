using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;
using Mockingjay.Features;
using MockingjayApp.Dialogs;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;
using TopicId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForTopic>;

namespace MockingjayApp
{
    public partial class Main : Form
    {
        private readonly ICommandProcessor _processor;
        private readonly IServiceProvider _services;
        private readonly ILogger _logger;
        private readonly Settings _settings;
        private StringWriter _writer = new StringWriter();
        private EndpointId _selectedEndpoint;
        private Thread _kafkaConsumer;
        volatile bool _kafkaConsumerRunning = true;
        private TopicId _selectedTopic;

        public Main(ICommandProcessor processor, IServiceProvider services, ILogger<Main> logger, Settings settings)
        {
            InitializeComponent();
            _processor = processor;
            _services = services;
            _logger = logger;
            _settings = settings;

            Program.Messages.NewLogHandler += Messages_NewLogHandler;
        }

        private void Messages_NewLogHandler(object sender, EventArgs e)
        {
            var log = (LogEventArgs)e;
            _writer.WriteLine(log.Log.RenderMessage());
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ReloadEndpoints();
            ReloadTopics();
            timer1.Start();
        }

        private void ReloadTopics()
        {
            listView2.Items.Clear();
            var results = _processor.Send<GetAllTopicsCommand, IEnumerable<Topic>>(new GetAllTopicsCommand());

            foreach (var topic in results)
            {
                var item = new ListViewItem();

                item.Name = topic.Id.ToString();
                item.Tag = topic.Id;
                item.ImageIndex = 1;
                item.SubItems.Add(topic.From);
                item.SubItems.Add(topic.To);
                //item.SubItems.Add(topic.TotalRequests);
                //item.SubItems.Add($"({(int)topic.StatusCode}) {topic.StatusCode}");
                //item.SubItems.Add(topic.TotalRequest.ToString());
                listView2.Items.Add(item);
            }
        }

        private void ReloadEndpoints()
        {
            listView1.Items.Clear();
            var results = _processor.Send<GetEndpointsCommand, GetEndpointsResponse>(new GetEndpointsCommand());

            foreach (var endpoint in results)
            {
                var item = new ListViewItem();

                item.Name = endpoint.Id.ToString();
                item.Tag = endpoint.Id;
                item.ImageIndex = 0;
                item.SubItems.Add(endpoint.Method);
                item.SubItems.Add(endpoint.Path);
                item.SubItems.Add(endpoint.ContentType);
                item.SubItems.Add($"({(int)endpoint.StatusCode}) {endpoint.StatusCode}");
                item.SubItems.Add(endpoint.TotalRequest.ToString());
                listView1.Items.Add(item);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedEndpoint = listView1.SelectedIndices.Count > 0
                ? (EndpointId)listView1.Items[listView1.SelectedIndices[0]].Tag
                : EndpointId.Empty;

            btnDelete.Visible = listView1.SelectedIndices.Count > 0;
            btnEdit.Visible = listView1.SelectedIndices.Count > 0;
        }

        private void btnAddEndpoint_Click(object sender, EventArgs e)
        {
            var dialog = _services.GetService<AddEndpointDialog>();
            dialog.Text = "Add new endpoint";
            dialog.EndpointId = EndpointId.Empty;
            dialog.ShowDialog(this);
            ReloadEndpoints();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtLog.Text = _writer.ToString();
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();

            listView1.Items.Clear();
            var results = _processor
                 .Send<GetEndpointsCommand, GetEndpointsResponse>(
                        new GetEndpointsCommand());

            foreach (var endpoint in results)
            {
                var item = new ListViewItem();

                item.Name = endpoint.Id.ToString();
                item.Tag = endpoint.Id;
                item.ImageIndex = 0;
                item.SubItems.Add(endpoint.Method);
                item.SubItems.Add(endpoint.Path);
                item.SubItems.Add(endpoint.ContentType);
                item.SubItems.Add($"({(int)endpoint.StatusCode}) {endpoint.StatusCode}");
                item.SubItems.Add(endpoint.TotalRequest.ToString());
                listView1.Items.Add(item);
            }

            if (listView1.Items.Count > 0 && !_selectedEndpoint.IsEmpty())
            {
                var index = listView1.Items.IndexOfKey(_selectedEndpoint.ToString());
                if (index >= 0)
                {
                    listView1.SelectedIndices.Add(index);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Weet je het zeker?", "Delete endpoint", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _processor.Send(new DeleteEndpointCommand { EndpointId = _selectedEndpoint });
                ReloadEndpoints();
            }
        }

        private void btnClearlogs_Click(object sender, EventArgs e)
        {
            _writer = new StringWriter();
            txtLog.Text = string.Empty;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var dialog = _services.GetService<AddEndpointDialog>();
            dialog.EndpointId = _selectedEndpoint;
            dialog.Text = "Edit endpoint";
            dialog.ShowDialog(this);
            ReloadEndpoints();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listView1.Items.IndexOfKey(_selectedEndpoint.ToString());
            var item = listView1.Items[index];
            var path = item.SubItems[2].Text;

            try
            {
                Process.Start(new ProcessStartInfo("http://localhost:5050" + path) { UseShellExecute = true });
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
                    MessageBox.Show(noBrowser.Message);
                }
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Import from file.",
                FileName = EndpointDatafile.Filename,
                Filter = EndpointDatafile.Filter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };

            if (fileDialog.ShowDialog(this) == DialogResult.OK)
            {
                timer1.Stop();
                var bytes = File.ReadAllBytes(fileDialog.FileName);
                _processor.Send(new ImportCommand { Bytes = bytes });
                timer1.Start();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileDialog = new SaveFileDialog
            {
                Title = "Export to file",
                FileName = EndpointDatafile.Filename,
                Filter = EndpointDatafile.Filter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };

            if (fileDialog.ShowDialog(this) == DialogResult.OK)
            {
                var data = _processor.Send<ExportCommand, byte[]>(new ExportCommand());
                File.WriteAllBytes(fileDialog.FileName, data);
                _logger.LogInformation("Datafile exported!");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnResetRequests_Click(object sender, EventArgs e)
        {
            _processor.Send(new ResetRequestsCommand());
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var settings = _services.GetService<SettingsDialog>();
            settings.ShowDialog(this);
        }

        public void KafkaLog(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(KafkaLog), new object[] { value });
                return;
            }
            txtKafkaLog.Text += value + "\r\n";
        }

        private void UpdateKafka(object obj)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServices,
                GroupId = _settings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true,
            };

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _settings.BootstrapServices,
            };

            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            var producer = new ProducerBuilder<Ignore, string>(producerConfig)
                .SetKeySerializer(new StringSerializer())
                .SetValueSerializer(new StringSerializer())
                .Build();
            var topics = _processor.Send<GetAllTopicsCommand, IEnumerable<Topic>>(new GetAllTopicsCommand());
            if(!topics.Any())
            {
                btnKafkaStop_Click(this, EventArgs.Empty);
                KafkaLog("No topics to consume.");
                return;
            }
            consumer.Subscribe(topics.Select(x => x.From).Distinct());

            KafkaLog($"Kafka Consumer started! {_settings.BootstrapServices}");
            KafkaLog(string.Join(", ", topics.Select(x => x.From).Distinct()));

            while (_kafkaConsumerRunning)
            {
                try
                {
                    var result = consumer.Consume(1000);
                    if(result != null)
                    {
                        KafkaLog($"Messge Recieved: From: {result.Topic}");

                        var candidateTopics = topics.Where(x => x.From == result.Topic);
                        foreach (Topic toTopic in candidateTopics)
                        {
                            KafkaLog($"Produce message in: {toTopic.To}");
                            producer.Produce(toTopic.To, new Message<Ignore, string> { Value = toTopic.Content });
                        }
                    }
                }
                catch (KafkaException e)
                {
                    KafkaLog(e.Error.Reason);
                    btnKafkaStop_Click(this, EventArgs.Empty);
                }
            }

            KafkaLog("Kafka Consumer stoped!");

            consumer.Close();
        }

        private void btnKafkaStop_Click(object sender, EventArgs e)
        {
            _kafkaConsumerRunning = false;
            kafkaButtons.Items[1].Visible = !(kafkaButtons.Items[0].Visible = true);
        }

        private void btnKafkaStart_Click(object sender, EventArgs e)
        {
            _kafkaConsumer = new Thread(new ParameterizedThreadStart(UpdateKafka));
            _kafkaConsumerRunning = true;
            _kafkaConsumer.Start();
            kafkaButtons.Items[0].Visible = !(kafkaButtons.Items[1].Visible = true);
        }

        private void txtKafkaLog_TextChanged(object sender, EventArgs e)
        {
            txtKafkaLog.SelectionStart = txtKafkaLog.Text.Length;
            txtKafkaLog.ScrollToCaret();
        }

        private void btnAddTopic_Click(object sender, EventArgs e)
        {
            var dialog = _services.GetService<AddTopicDialog>();
            dialog.Text = "Add new topic";
            dialog.TopicId = TopicId.Empty;
            dialog.ShowDialog(this);
            ReloadTopics();
            btnKafkaStop_Click(sender, e);
            btnKafkaStart_Click(sender, e);
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedTopic = listView2.SelectedIndices.Count > 0
                ? (TopicId)listView2.Items[listView2.SelectedIndices[0]].Tag
                : TopicId.Empty;

            btnDeleteTopic.Visible = listView2.SelectedIndices.Count > 0;
            //btnEdit.Visible = listView2.SelectedIndices.Count > 0;
        }

        private void btnDeleteTopic_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Weet je het zeker?", "Delete endpoint", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _processor.Send(new DeleteTopicCommand { TopicId = _selectedTopic });
                ReloadTopics();
            }
        }
    }
}
