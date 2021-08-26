using Mockingjay.Common.Http;
using Mockingjay.Entities;
using Mockingjay.Features.AddEndpoint;
using Mockingjay.Features.GetEndpoint;
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

            Task<EndpointId> task = Task.Run(() => Client.AddEndpointAsync(new AddEndpointCommand {
                    Path = "/test",
                    Method= "GET",
                    ContentType= "application/json",
                    Response = "Test Test",
                    StatusCode = 201
                }));

            var result = await task;
            
            label1.Text = result.ToString();

            Task<EndpointInformation> task1 = Task.Run(() => Client.GetEndpointByRequest(new GetEndpointCommand { Path = "/test", Method = "GET" }));

            var result2 = (await task1);
            textBox1.Text = result2.Response;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
