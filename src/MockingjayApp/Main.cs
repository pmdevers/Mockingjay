using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Mockingjay.Common.Handling;
using Mockingjay.Features;
using MockingjayApp.Dialogs;
using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace MockingjayApp
{
    public partial class Main : Form
    {
        private readonly ICommandProcessor _processor;
        private readonly IServiceProvider _services;
        private StringWriter _writer = new StringWriter();
        private EndpointId _selectedEndpoint;

        public Main(ICommandProcessor processor, IServiceProvider services)
        {
            InitializeComponent();
            _processor = processor;
            _services = services;

            Program.Messages.NewLogHandler += Messages_NewLogHandler;
        }

        private void Messages_NewLogHandler(object sender, EventArgs e)
        {
            var log = (LogEventArgs)e;
            _writer.WriteLine(log.Log.RenderMessage());
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            await ReloadAsync();
            timer1.Start();
        }

        private async Task ReloadAsync()
        {
            listView1.Items.Clear();
            var command = new GetEndpointsCommand();
            var results = await _processor.SendAsync<GetEndpointsCommand, GetEndpointsResponse>(command);

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
            if (listView1.SelectedIndices.Count > 0)
            {
                _selectedEndpoint = (EndpointId)listView1.Items[listView1.SelectedIndices[0]].Tag;
            }
            else
            {
                _selectedEndpoint = EndpointId.Empty;
            }

            btnDelete.Visible = listView1.SelectedIndices.Count > 0;
            btnEdit.Visible = listView1.SelectedIndices.Count > 0;
        }

        private async void toolStripButton1_Click(object sender, EventArgs e)
        {
            var dialog = _services.GetService<AddEndpointDialog>();
            dialog.Text = "Add new endpoint";
            dialog.EndpointId = EndpointId.Empty;
            dialog.ShowDialog(this);
            await ReloadAsync();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            await RefreshUI();
        }

        private async Task RefreshUI()
        {
            txtLog.Text = _writer.ToString();
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();

            await ReloadAsync();

            if (listView1.Items.Count > 0 && !_selectedEndpoint.IsEmpty())
            {
                var index = listView1.Items.IndexOfKey(_selectedEndpoint.ToString());
                if (index >= 0)
                {
                    listView1.SelectedIndices.Add(index);
                }
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Weet je het zeker?", "Delete endpoint", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                await _processor.SendAsync(new DeleteEndpointCommand { EndpointId = _selectedEndpoint });
                await ReloadAsync();
            }
        }

        private void btnClearlogs_Click(object sender, EventArgs e)
        {
            _writer = new StringWriter();
            txtLog.Text = string.Empty;
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            var dialog = _services.GetService<AddEndpointDialog>();
            dialog.EndpointId = _selectedEndpoint;
            dialog.Text = "Edit endpoint";
            dialog.ShowDialog(this);
            await ReloadAsync();
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
    }
}
