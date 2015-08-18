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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MsgBox;
using PNSDraw.Configuration;

namespace PNSDraw
{
    public partial class UserDialog : Form
    {
        public bool LoginSuccess { get; private set; }
        public bool LogoutSuccess { get; private set; }
        public string Username { get; private set; }

        public UserDialog(string username)
        {
            LoginSuccess = false;
            LogoutSuccess = false;

            InitializeComponent();

            chUsernameTextBox.Text = username;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            string username = Config.Instance.Login.Username;
            string email = Config.Instance.Login.Email;
            string current = chCurrentTextBox.Text;
            string newPwd = chNewTextBox.Text;
            string newPwdConf = chConfirmTextBox.Text;

            if (current.Length == 0)
            {
                MessageBox.Show("Please type your current password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPwd.Length == 0)
            {
                MessageBox.Show("Please type a new password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!newPwd.Equals(newPwdConf))
            {
                MessageBox.Show("The new password and confirmation password do not match, please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            WebserverHttpClient client = new WebserverHttpClient();
            if (client.ChangePassword(username, email, current, newPwd))
            {
                MessageBox.Show(
                    "Your password has been changed!",
                    "Password change",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    "Failed to change your password!",
                    "Password change",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            LoginSuccess = true;
            Username = username;
            Close();
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            Config.Instance.Login.IsLoggedIn = false;
            Config.Instance.Login.Email = "";
            Config.Instance.Login.Username = "";

            Config.Instance.SolverSettings.IsOnlineSolver = false;

            LogoutSuccess = true;
            Close();
        }

        private void chPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                changeButton_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                cancelButton_Click(sender, e);
            }
        }
    }
}
