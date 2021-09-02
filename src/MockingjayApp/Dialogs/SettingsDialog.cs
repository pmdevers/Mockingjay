using Mockingjay.Common.Handling;
using Mockingjay.Features;
using System;
using System.Windows.Forms;

namespace MockingjayApp.Dialogs
{
    public partial class SettingsDialog : Form
    {
        private readonly ICommandProcessor _processor;
        private readonly Settings _settings;

        public SettingsDialog(ICommandProcessor processor, Settings settings)
        {
            InitializeComponent();
            _processor = processor;
            _settings = settings;
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
            txtBootstrapServers.Text = _settings.BootstrapServices;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            _processor.Send(new SaveSettingsCommand {
               BootstrapServices = txtBootstrapServers.Text,
            });
            Close();
        }

        private void btnCloseSettings_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
