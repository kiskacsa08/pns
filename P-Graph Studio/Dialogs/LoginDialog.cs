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
    public partial class LoginDialog : Form
    {
        public bool LoginSuccess { get; private set; }
        public bool RegisterSuccess { get; private set; }
        //public string Username { get; private set; }

        public LoginDialog()
        {
            LoginSuccess = false;
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            WebserverHttpClient client = new WebserverHttpClient();

            if (!client.Check())
            {
                MessageBox.Show("Server not found or your computer is not connected to the Internet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoginSuccess = client.Login(username, password);

            if (LoginSuccess)
            {
                Config.Instance.Login.Username = username;
                Close();
            }
            else
            {
                MessageBox.Show("Incorrect username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void registerUpButton_Click(object sender, EventArgs e)
        {
            string username = upUsernameTextBox.Text;
            string password = upPassTextBox.Text;
            string confirmPassword = upPassConfirmTextBox.Text;
            string email = upEmailTextBox.Text;

            if (username.Length == 0)
            {
                MessageBox.Show("Please type an username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RegisterSuccess = false;
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("E-mail address is invalid, please type valid address!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RegisterSuccess = false;
                return;
            }

            if (password.Length == 0)
            {
                MessageBox.Show("Please type a password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RegisterSuccess = false;
                return;
            }

            if (!password.Equals(confirmPassword))
            {
                MessageBox.Show("The password and confirmation password do not match, please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RegisterSuccess = false;
                return;
            }

            WebserverHttpClient client = new WebserverHttpClient();

            if (!client.Check())
            {
                MessageBox.Show("Server not found or your computer is not connected to the Internet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RegisterSuccess = client.Register(username, password, email);

            if (RegisterSuccess)
            {
                upPassTextBox.Text = "";
                upPassConfirmTextBox.Text = "";
                bool valid = false;
                while (!valid)
                {
                    InputBox.SetLanguage(InputBox.Language.English);
                    DialogResult dresult = InputBox.ShowDialog(
                        "Please enter the validation code that was sent to you on your e-mail address!",
                        "Validation",
                        InputBox.Icon.Question,
                        InputBox.Buttons.Ok,
                        InputBox.Type.TextBox
                    );

                    if (client.Validate(username, InputBox.ResultValue))
                    {
                        MessageBox.Show("Registration success, please log in!");
                        valid = true;

                        upUsernameTextBox.Text = "";
                        upEmailTextBox.Text = "";

                        usernameTextBox.Text = username;
                        passwordTextBox.Select();
                    }
                    else
                    {
                        MessageBox.Show("The validation code is not valid, please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("This username or email is exist, please choose another!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void loginPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                loginButton_Click(sender, e);
            }
            else if(e.KeyCode == Keys.Escape)
            {
                cancelButton_Click(sender, e);
            }
        }

        private void registerPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                registerUpButton_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                cancelButton_Click(sender, e);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void forgotPasswordButton_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Enabled = false;
            InputBox.SetLanguage(InputBox.Language.English);
            DialogResult dresult = InputBox.ShowDialog(
                        "Please type your username and click OK!",
                        "Validation",
                        InputBox.Icon.Information,
                        InputBox.Buttons.OkCancel,
                        InputBox.Type.TextBox
                    );

            if (dresult == DialogResult.OK)
            {
                WebserverHttpClient client = new WebserverHttpClient();
                if (client.ForgotPassword(InputBox.ResultValue))
                {
                    MessageBox.Show(
                        "The new password has been sent to your email address.\nAfter logging in, you need to change your password!",
                        "Password change",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            this.Enabled = true;
        }
    }
}
