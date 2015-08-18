/* Copyright 2015 Department of Computer Science and Systems Technology, University of Pannonia

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. 
*/

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

using PNSDraw.Configuration;

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
            txtFolder.Text = Config.Instance.SolverSettings.OfflineSolverTempFolder;
            numLimit.Value = (decimal)Config.Instance.SolverSettings.SolutionLimit;
            numProcesses.Value = (decimal)Config.Instance.SolverSettings.NumberOfSolverProccess;
            chkOnline.Checked = Config.Instance.SolverSettings.IsOnlineSolver;
            chkKeep.Checked = Config.Instance.SolverSettings.IsKeepTempFiles;
            txtHost.Text = Config.Instance.SolverSettings.OnlineSolverHost;
            numPort.Value = (decimal)Config.Instance.SolverSettings.OnlineSolverPort;
            if (Config.Instance.SolverSettings.IsOnlineSolver == false)
            {
                numProcesses.Enabled = false;
                txtHost.Enabled = false;
                numPort.Enabled = false;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Config.Instance.SolverSettings.OfflineSolverTempFolder = txtFolder.Text;
            Config.Instance.SolverSettings.SolutionLimit = (int)numLimit.Value;
            Config.Instance.SolverSettings.IsOnlineSolver = chkOnline.Checked;
            Config.Instance.SolverSettings.IsKeepTempFiles = chkKeep.Checked;
            if (chkOnline.Checked)
            {
                Config.Instance.SolverSettings.NumberOfSolverProccess = (int)numProcesses.Value;
                if (PingHost(txtHost.Text, (int)numPort.Value))
                {
                    Config.Instance.SolverSettings.OnlineSolverHost = txtHost.Text;
                    Config.Instance.SolverSettings.OnlineSolverPort = (int)numPort.Value;
                }
                else
                {
                    MessageBox.Show("Error pinging host:'" + txtHost.Text + ":" + numPort.Value.ToString() + "'");
                    Config.Instance.SolverSettings.IsOnlineSolver = false;
                }
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
            
        }

        public static bool PingHost(string _HostURI, int _PortNumber)
        {
            try
            {
                PNSDraw.online.SolverSocket sock = new PNSDraw.online.SolverSocket();
                return sock.Ping(_HostURI, _PortNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private void chkOnline_CheckedChanged(object sender, EventArgs e)
        {
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

        private void chkKeep_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkOnline_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Utils.Instance.CheckLogin())
            {
                chkOnline.Checked = false;
                return;
            }
        }
    }
}
