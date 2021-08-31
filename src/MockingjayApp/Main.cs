using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mockingjay.Common.Handling;
using Mockingjay.Common.Storage;
using Mockingjay.Features;
using MockingjayApp.Dialogs;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace MockingjayApp
{
    public partial class Main : Form
    {
        private readonly ICommandProcessor _processor;
        private readonly IServiceProvider _services;
        private readonly ILogger _logger;
        private StringWriter _writer = new StringWriter();
        private EndpointId _selectedEndpoint;

        public Main(ICommandProcessor processor, IServiceProvider services, ILogger<Main> logger)
        {
            InitializeComponent();
            _processor = processor;
            _services = services;
            _logger = logger;
            Program.Messages.NewLogHandler += Messages_NewLogHandler;
        }

        private void Messages_NewLogHandler(object sender, EventArgs e)
        {
            var log = (LogEventArgs)e;
            _writer.WriteLine(log.Log.RenderMessage());
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Reload();
            timer1.Start();
        }

        private void Reload()
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var dialog = _services.GetService<AddEndpointDialog>();
            dialog.Text = "Add new endpoint";
            dialog.EndpointId = EndpointId.Empty;
            dialog.ShowDialog(this);
            Reload();
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
                Reload();
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
            Reload();
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
    }
}
