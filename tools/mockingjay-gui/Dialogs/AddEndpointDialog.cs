using Mockingjay.Common.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mockingjay.Dialogs
{
    public partial class AddEndpointDialog : Form
    {
        private readonly MockingjayClient _client;

        public AddEndpointDialog(MockingjayClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
