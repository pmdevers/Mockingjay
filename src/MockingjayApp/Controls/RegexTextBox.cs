using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MockingjayApp.Controls
{
	public class IntTextBox : TextBox
	{
		protected override void OnTextChanged(EventArgs e)
		{
			if(!Regex.IsMatch(Text, "^[0-9]"))
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
