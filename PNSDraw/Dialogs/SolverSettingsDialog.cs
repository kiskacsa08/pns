using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNSDraw.Dialogs
{
    public partial class SolverSettingsDialog : Form
    {
        public SolverSettingsDialog()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (hasWriteAccessToFolder(folderBrowserDialog1.SelectedPath))
                {
                    txtFolder.Text = folderBrowserDialog1.SelectedPath + "\\";
                }
                else
                {
                    MessageBox.Show("You do not have permission to write in this folder!");
                }
            }
        }

        private void SolverSettingsDialog_Load(object sender, EventArgs e)
        {
            txtFolder.Text = Default.tempFolder;
            numLimit.Value = (decimal)Default.limit;
            numProcesses.Value = (decimal)Default.processes;
            chkOnline.Checked = Default.online;
            txtHost.Text = Default.host;
            numPort.Value = (decimal)Default.port;
            if (Default.online == false)
            {
                numProcesses.Enabled = false;
                txtHost.Enabled = false;
                numPort.Enabled = false;
            }
            btnApply.Enabled = false;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Default.tempFolder = txtFolder.Text;
            Default.limit = (int)numLimit.Value;
            Default.online = chkOnline.Checked;
            if (chkOnline.Checked)
            {
                Default.processes = (int)numProcesses.Value;
                if (PingHost(txtHost.Text, (int)numPort.Value))
                {
                    Default.host = txtHost.Text;
                    Default.port = (int)numPort.Value;
                    btnApply.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Error pinging host:'" + txtHost.Text + ":" + numPort.Value.ToString() + "'");
                }
            }
            else
            {
                btnApply.Enabled = false;
            }
        }

        private bool hasWriteAccessToFolder(string folderPath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private void numLimit_ValueChanged(object sender, EventArgs e)
        {
            if (btnApply.Enabled == false)
            {
                btnApply.Enabled = true;
            }
        }

        public static bool PingHost(string _HostURI, int _PortNumber)
        {
            try
            {
                TcpClient client = new TcpClient(_HostURI, _PortNumber);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void chkOnline_CheckedChanged(object sender, EventArgs e)
        {
            if (btnApply.Enabled == false)
            {
                btnApply.Enabled = true;
            }
            if (chkOnline.Checked == false)
            {
                numProcesses.Enabled = false;
                txtHost.Enabled = false;
                numPort.Enabled = false;
            }
            else
            {
                numProcesses.Enabled = true;
                txtHost.Enabled = true;
                numPort.Enabled = true;
            }
        }
    }
}
