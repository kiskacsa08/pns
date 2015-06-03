using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNSDraw
{
    public partial class KeepFilesDialog : Form
    {
        public KeepFilesDialog(string inFile, string outFile)
        {
            InitializeComponent();
            lblInFileName.Text = inFile;
            lblOutFileName.Text = outFile;
        }
    }
}
