using Mockingjay.Common.Handling;
using Mockingjay.Features;
using MockingjayApp.Dialogs;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows.Forms;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;


namespace MockingjayApp
{
    public partial class Main : Form
    {
        private readonly ICommandProcessor _processor;
        private readonly IServiceProvider _services;
        private EndpointId _selectedEndpoint;

        public Main(ICommandProcessor processor, IServiceProvider services)
        {
            InitializeComponent();
            _processor = processor;
            _services = services;
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

                item.Tag = endpoint.Id;
                item.SubItems.Add(endpoint.Method);
                item.SubItems.Add(endpoint.Path);
                item.SubItems.Add(endpoint.ContentType);
                item.SubItems.Add(endpoint.TotalRequest.ToString());
                listView1.Items.Add(item);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedIndices.Count > 0)
            {
                _selectedEndpoint = (EndpointId)listView1.Items[listView1.SelectedIndices[0]].Tag;
            }
            else
            {
                _selectedEndpoint = EndpointId.Empty;
            }
           
            btnDelete.Visible = listView1.SelectedIndices.Count > 0;
        }

        private async void toolStripButton1_Click(object sender, EventArgs e)
        {
            var dialog = _services.GetService<AddEndpointDialog>();
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.ShowDialog(this);
            await ReloadAsync();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtLog.Text = Program.Messages.ToString();
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(this, "Weet je het zeker?", "Delete endpoint", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                await _processor.SendAsync(new DeleteEndpointCommand { EndpointId = _selectedEndpoint });
                await ReloadAsync();
            }
        }
    }
}
