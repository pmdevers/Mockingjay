using Mockingjay.Common.Http;
using Mockingjay.Features.AddEndpoint;
using System;

using System.Threading.Tasks;
using System.Windows.Forms;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForMockEndpoint>;

namespace mockingjay
{
    public partial class Main : Form
    {
        public Main(MockingjayClient client)
        {
            InitializeComponent();
            Client = client;
        }

        public MockingjayClient Client { get; }

        private async void Main_Load(object sender, EventArgs e)
        {

            Task<EndpointId> task = Task.Run(() => Client.AddEndpointAsync(new AddEndpointCommand()));

            var result = await task;
            

            label1.Text = result.ToString();
        }
    }
}
