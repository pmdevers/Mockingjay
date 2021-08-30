using Mockingjay.Common.Handling;
using Mockingjay.Features;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;

namespace MockingjayApp.Dialogs
{
    public partial class AddEndpointDialog : Form
    {
        private readonly ICommandProcessor _processor;

        public EndpointId EndpointId { get; set; } = EndpointId.Empty;

        public AddEndpointDialog(ICommandProcessor processor)
        {
            InitializeComponent();
            _processor = processor;
            Method.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            EndpointId = EndpointId == EndpointId.Empty
                ? await _processor.SendAsync<AddEndpointCommand, EndpointId>(new AddEndpointCommand
                {
                    Method = Method.Items[Method.SelectedIndex].ToString(),
                    Path = Endpoint.Text,
                    ContentType = contentType.Text,
                    Content = content.Text,
                    StatusCode = (HttpStatusCode)cbStatusCode.SelectedValue,
                })
                : await _processor.SendAsync<UpdateEndpointCommand, EndpointId>(new UpdateEndpointCommand
                {
                    EndpointId = EndpointId,
                    Method = Method.Items[Method.SelectedIndex].ToString(),
                    Path = Endpoint.Text,
                    ContentType = contentType.Text,
                    Content = content.Text,
                    StatusCode = (HttpStatusCode)cbStatusCode.SelectedValue,
                });
            Close();
        }

        private async void AddEndpointDialog_Load(object sender, EventArgs e)
        {
            cbStatusCode.DataSource = Enum.GetValues<HttpStatusCode>().Distinct().Select(x => new { key = x, value = $"({(int)x}) {x}" }).ToList();
            cbStatusCode.DisplayMember = "value";
            cbStatusCode.ValueMember = "key";
            cbStatusCode.SelectedValue = HttpStatusCode.OK;

            if (!EndpointId.IsEmpty())
            {
                await LoadEndpoint(EndpointId);
            }
        }

        private async Task LoadEndpoint(EndpointId endpointId)
        {
            var endpoint = await _processor.SendAsync<GetEndpointByIdCommand, Endpoint>(new GetEndpointByIdCommand { EndpointId = endpointId });

            EndpointId = endpointId;
            Method.SelectedIndex = Method.FindString(endpoint.Method);
            Endpoint.Text = endpoint.Path;
            contentType.Text = endpoint.ContentType;
            content.Text = endpoint.Content;
            cbStatusCode.SelectedValue = endpoint.StatusCode;
        }
    }
}
