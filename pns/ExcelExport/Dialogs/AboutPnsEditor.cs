using System;
using System.Windows.Forms;

namespace Pns.Dialogs
{
    partial class AboutPnsEditor : Form
    {
        public AboutPnsEditor() { InitializeComponent(); }
        private void okButton_Click(object sender, EventArgs e) { Close(); }
    }
}
