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

namespace PNSDraw
{
    partial class LoginDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.loginGroupBox = new System.Windows.Forms.GroupBox();
            this.forgotPasswordButton = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.registerGroupBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.registerUpButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cancelButton2 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.upUsernameTextBox = new System.Windows.Forms.TextBox();
            this.upEmailTextBox = new System.Windows.Forms.TextBox();
            this.upPassConfirmTextBox = new System.Windows.Forms.TextBox();
            this.upPassTextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.loginGroupBox.SuspendLayout();
            this.registerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.loginGroupBox);
            this.panel1.Controls.Add(this.registerGroupBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(621, 221);
            this.panel1.TabIndex = 0;
            // 
            // loginGroupBox
            // 
            this.loginGroupBox.Controls.Add(this.forgotPasswordButton);
            this.loginGroupBox.Controls.Add(this.label2);
            this.loginGroupBox.Controls.Add(this.label1);
            this.loginGroupBox.Controls.Add(this.loginButton);
            this.loginGroupBox.Controls.Add(this.label3);
            this.loginGroupBox.Controls.Add(this.cancelButton);
            this.loginGroupBox.Controls.Add(this.usernameTextBox);
            this.loginGroupBox.Controls.Add(this.passwordTextBox);
            this.loginGroupBox.Location = new System.Drawing.Point(12, 12);
            this.loginGroupBox.Name = "loginGroupBox";
            this.loginGroupBox.Size = new System.Drawing.Size(295, 194);
            this.loginGroupBox.TabIndex = 0;
            this.loginGroupBox.TabStop = false;
            this.loginGroupBox.Text = "Login";
            // 
            // forgotPasswordButton
            // 
            this.forgotPasswordButton.AutoSize = true;
            this.forgotPasswordButton.Location = new System.Drawing.Point(6, 166);
            this.forgotPasswordButton.Name = "forgotPasswordButton";
            this.forgotPasswordButton.Size = new System.Drawing.Size(104, 13);
            this.forgotPasswordButton.TabIndex = 5;
            this.forgotPasswordButton.TabStop = true;
            this.forgotPasswordButton.Text = "I forgot my password";
            this.forgotPasswordButton.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.forgotPasswordButton_LinkClicked);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 80);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.MaximumSize = new System.Drawing.Size(105, 25);
            this.label2.MinimumSize = new System.Drawing.Size(105, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Username:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 0, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please login by entering your username and password!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(120, 161);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 3;
            this.loginButton.Text = "Log in";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 107);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.MaximumSize = new System.Drawing.Size(105, 25);
            this.label3.MinimumSize = new System.Drawing.Size(105, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Password:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(209, 161);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.usernameTextBox.Location = new System.Drawing.Point(121, 85);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(164, 20);
            this.usernameTextBox.TabIndex = 1;
            this.usernameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loginPanel_KeyDown);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(120, 110);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '•';
            this.passwordTextBox.Size = new System.Drawing.Size(164, 20);
            this.passwordTextBox.TabIndex = 2;
            this.passwordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loginPanel_KeyDown);
            // 
            // registerGroupBox
            // 
            this.registerGroupBox.Controls.Add(this.label4);
            this.registerGroupBox.Controls.Add(this.label5);
            this.registerGroupBox.Controls.Add(this.registerUpButton);
            this.registerGroupBox.Controls.Add(this.label6);
            this.registerGroupBox.Controls.Add(this.label8);
            this.registerGroupBox.Controls.Add(this.cancelButton2);
            this.registerGroupBox.Controls.Add(this.label10);
            this.registerGroupBox.Controls.Add(this.upUsernameTextBox);
            this.registerGroupBox.Controls.Add(this.upEmailTextBox);
            this.registerGroupBox.Controls.Add(this.upPassConfirmTextBox);
            this.registerGroupBox.Controls.Add(this.upPassTextBox);
            this.registerGroupBox.Location = new System.Drawing.Point(313, 12);
            this.registerGroupBox.Name = "registerGroupBox";
            this.registerGroupBox.Size = new System.Drawing.Size(296, 194);
            this.registerGroupBox.TabIndex = 0;
            this.registerGroupBox.TabStop = false;
            this.registerGroupBox.Text = "Register";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(278, 36);
            this.label4.TabIndex = 0;
            this.label4.Text = "If you do not have an account, please register!";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 55);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.MaximumSize = new System.Drawing.Size(105, 25);
            this.label5.MinimumSize = new System.Drawing.Size(105, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 25);
            this.label5.TabIndex = 0;
            this.label5.Text = "Username:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // registerUpButton
            // 
            this.registerUpButton.Location = new System.Drawing.Point(120, 161);
            this.registerUpButton.Name = "registerUpButton";
            this.registerUpButton.Size = new System.Drawing.Size(75, 23);
            this.registerUpButton.TabIndex = 9;
            this.registerUpButton.Text = "Register";
            this.registerUpButton.UseVisualStyleBackColor = true;
            this.registerUpButton.Click += new System.EventHandler(this.registerUpButton_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 80);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.MaximumSize = new System.Drawing.Size(105, 25);
            this.label6.MinimumSize = new System.Drawing.Size(105, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 25);
            this.label6.TabIndex = 0;
            this.label6.Text = "E-mail address:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 105);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.MaximumSize = new System.Drawing.Size(105, 25);
            this.label8.MinimumSize = new System.Drawing.Size(105, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 25);
            this.label8.TabIndex = 0;
            this.label8.Text = "Password:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cancelButton2
            // 
            this.cancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton2.Location = new System.Drawing.Point(211, 161);
            this.cancelButton2.Name = "cancelButton2";
            this.cancelButton2.Size = new System.Drawing.Size(75, 23);
            this.cancelButton2.TabIndex = 10;
            this.cancelButton2.Text = "Cancel";
            this.cancelButton2.UseVisualStyleBackColor = true;
            this.cancelButton2.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(8, 130);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label10.MaximumSize = new System.Drawing.Size(105, 25);
            this.label10.MinimumSize = new System.Drawing.Size(105, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 25);
            this.label10.TabIndex = 0;
            this.label10.Text = "Confirm password:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // upUsernameTextBox
            // 
            this.upUsernameTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.upUsernameTextBox.Location = new System.Drawing.Point(120, 60);
            this.upUsernameTextBox.Name = "upUsernameTextBox";
            this.upUsernameTextBox.Size = new System.Drawing.Size(164, 20);
            this.upUsernameTextBox.TabIndex = 5;
            this.upUsernameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.registerPanel_KeyDown);
            // 
            // upEmailTextBox
            // 
            this.upEmailTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.upEmailTextBox.Location = new System.Drawing.Point(120, 85);
            this.upEmailTextBox.Name = "upEmailTextBox";
            this.upEmailTextBox.Size = new System.Drawing.Size(164, 20);
            this.upEmailTextBox.TabIndex = 6;
            this.upEmailTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.registerPanel_KeyDown);
            // 
            // upPassConfirmTextBox
            // 
            this.upPassConfirmTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.upPassConfirmTextBox.Location = new System.Drawing.Point(120, 135);
            this.upPassConfirmTextBox.Name = "upPassConfirmTextBox";
            this.upPassConfirmTextBox.PasswordChar = '•';
            this.upPassConfirmTextBox.Size = new System.Drawing.Size(164, 20);
            this.upPassConfirmTextBox.TabIndex = 8;
            this.upPassConfirmTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.registerPanel_KeyDown);
            // 
            // upPassTextBox
            // 
            this.upPassTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.upPassTextBox.Location = new System.Drawing.Point(120, 110);
            this.upPassTextBox.Name = "upPassTextBox";
            this.upPassTextBox.PasswordChar = '•';
            this.upPassTextBox.Size = new System.Drawing.Size(164, 20);
            this.upPassTextBox.TabIndex = 7;
            this.upPassTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.registerPanel_KeyDown);
            // 
            // LoginDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(621, 221);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LoginDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User";
            this.panel1.ResumeLayout(false);
            this.loginGroupBox.ResumeLayout(false);
            this.loginGroupBox.PerformLayout();
            this.registerGroupBox.ResumeLayout(false);
            this.registerGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox upPassTextBox;
        private System.Windows.Forms.TextBox upEmailTextBox;
        private System.Windows.Forms.TextBox upUsernameTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox upPassConfirmTextBox;
        private System.Windows.Forms.Button registerUpButton;
        private System.Windows.Forms.Button cancelButton2;
        private System.Windows.Forms.GroupBox loginGroupBox;
        private System.Windows.Forms.GroupBox registerGroupBox;
        private System.Windows.Forms.LinkLabel forgotPasswordButton;

    }
}