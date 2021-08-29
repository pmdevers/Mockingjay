using Mockingjay.Common.Handling;
using Mockingjay.Features;
using System;
using System.Net;
using System.Windows.Forms;

using EndpointId = Mockingjay.Common.Identifiers.Id<Mockingjay.ValueObjects.ForEndpoint>;


namespace MockingjayApp.Dialogs
{
    public partial class AddEndpointDialog : Form
    {
        private readonly ICommandProcessor _processor;

        public AddEndpointDialog(ICommandProcessor processor)
        {
            InitializeComponent();
            _processor = processor;
            Method.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var endpointId = await _processor.SendAsync<AddEndpointCommand, EndpointId>(new AddEndpointCommand()
            {
                Method = Method.Items[Method.SelectedIndex].ToString(),
                Path = Endpoint.Text,
                ContentType = contentType.Text,
                Content = content.Text,
                StatusCode = (HttpStatusCode)int.Parse(statusCode.Text)
            });

            this.Close();
        }

        private void AddEndpointDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
