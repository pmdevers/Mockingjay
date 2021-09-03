using Mockingjay.Common.Handling;
using Mockingjay.Common.Identifiers;
using Mockingjay.Features;
using Mockingjay.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MockingjayApp.Dialogs
{
    public partial class AddTopicDialog : Form
    {
        private readonly ICommandProcessor _proccessor;

        public AddTopicDialog(ICommandProcessor proccessor)
        {
            InitializeComponent();
            _proccessor = proccessor;
        }

        public Id<ForTopic> TopicId { get; internal set; }

        private void AddTopicDialog_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TopicId = _proccessor.Send<CreateTopicCommand, Id<ForTopic>>(new CreateTopicCommand {
                From = txtFrom.Text,
                To = txtTo.Text,
                Content = txtContent.Text,
                });
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            TopicId = Id<ForTopic>.Empty;
            Close();
        }
    }
}
