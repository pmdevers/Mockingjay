using Mockingjay.Common.Handling;
using Mockingjay.Features;
using MockingjayApp.Dialogs;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MockingjayApp
{
    public partial class Main : Form
    {
        private readonly ICommandProcessor _processor;
        private readonly IServiceProvider _services;

        public Main(ICommandProcessor processor, IServiceProvider services)
        {
            InitializeComponent();
            _processor = processor;
            _services = services;
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            await ReloadAsync().ConfigureAwait(false);
            timer1.Start();
        }

        private async Task ReloadAsync()
        {
            listView1.Items.Clear();
            var command = new GetEndpointsCommand {  Page = 1, ItemsPerPage = 25 };
            var results = await _processor.SendAsync<GetEndpointsCommand, GetEndpointsResponse>(command);

            foreach (var endpoint in results.Items)
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

        }

        private async void toolStripButton1_Click(object sender, EventArgs e)
        {
            var dialog = _services.GetService<AddEndpointDialog>();
            dialog.ShowDialog(this);
            await ReloadAsync().ConfigureAwait(false);
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            await ReloadAsync().ConfigureAwait(false);
        }
    }
}
