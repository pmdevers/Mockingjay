using mockingjay.Dialogs;
using Mockingjay.Common.Http;
using Mockingjay.Features.GetEndpoints;
using System;

using System.Threading.Tasks;
using System.Windows.Forms;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace mockingjay
{
    public partial class Main : Form
    {
        
        private readonly AddEndpointDialog _endpointDialog;
        private int _page = 1;
        private int _itemsPerPage = 50;

        public Main(MockingjayClient client, AddEndpointDialog endpointDialog)
        {
            
            InitializeComponent();
            Client = client;
            _endpointDialog = endpointDialog;
        }

        public MockingjayClient Client { get; }

        private async void Main_Load(object sender, EventArgs e)
        {          
            await ReloadAsync();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _endpointDialog.ShowDialog(this);
        }

        private async void toolStripButton2_Click(object sender, EventArgs e)
        {
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            listView1.Items.Clear();
            var results = await Client.GetEndpointsAsync(_page, _itemsPerPage);

            foreach (var endpoint in results.Items)
            {
                var item = new ListViewItem();

                item.Tag = endpoint.Id;
                item.SubItems.Add(endpoint.Method);
                item.SubItems.Add(endpoint.Path);
                item.SubItems.Add(endpoint.ContentType);
                listView1.Items.Add(item);
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
