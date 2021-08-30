using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MockingjayApp.Controls
{
    public class IntTextBox : TextBox
    {
        protected override void OnTextChanged(EventArgs e)
        {
            if (!Regex.IsMatch(Text, "^[0-9]"))
            {
                Text = string.Empty;
            }

            base.OnTextChanged(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
