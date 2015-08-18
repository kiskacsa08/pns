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
    partial class UserDialog
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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chConfirmTextBox = new System.Windows.Forms.TextBox();
            this.chCurrentTextBox = new System.Windows.Forms.TextBox();
            this.chNewTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.chUsernameTextBox = new System.Windows.Forms.TextBox();
            this.registerGroupBox = new System.Windows.Forms.GroupBox();
            this.logoutButton = new System.Windows.Forms.Button();
            this.cancelButton2 = new System.Windows.Forms.Button();
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
            this.loginGroupBox.Controls.Add(this.label4);
            this.loginGroupBox.Controls.Add(this.label5);
            this.loginGroupBox.Controls.Add(this.chConfirmTextBox);
            this.loginGroupBox.Controls.Add(this.chCurrentTextBox);
            this.loginGroupBox.Controls.Add(this.chNewTextBox);
            this.loginGroupBox.Controls.Add(this.label2);
            this.loginGroupBox.Controls.Add(this.label1);
            this.loginGroupBox.Controls.Add(this.loginButton);
            this.loginGroupBox.Controls.Add(this.label3);
            this.loginGroupBox.Controls.Add(this.cancelButton);
            this.loginGroupBox.Controls.Add(this.chUsernameTextBox);
            this.loginGroupBox.Location = new System.Drawing.Point(12, 12);
            this.loginGroupBox.Name = "loginGroupBox";
            this.loginGroupBox.Size = new System.Drawing.Size(295, 194);
            this.loginGroupBox.TabIndex = 0;
            this.loginGroupBox.TabStop = false;
            this.loginGroupBox.Text = "Change Password";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 106);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.MaximumSize = new System.Drawing.Size(105, 25);
            this.label4.MinimumSize = new System.Drawing.Size(105, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "New password:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 132);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.MaximumSize = new System.Drawing.Size(105, 25);
            this.label5.MinimumSize = new System.Drawing.Size(105, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 25);
            this.label5.TabIndex = 0;
            this.label5.Text = "Confirm password:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chConfirmTextBox
            // 
            this.chConfirmTextBox.Location = new System.Drawing.Point(125, 135);
            this.chConfirmTextBox.Name = "chConfirmTextBox";
            this.chConfirmTextBox.PasswordChar = '•';
            this.chConfirmTextBox.Size = new System.Drawing.Size(164, 20);
            this.chConfirmTextBox.TabIndex = 4;
            this.chConfirmTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chPanel_KeyDown);
            // 
            // chCurrentTextBox
            // 
            this.chCurrentTextBox.Location = new System.Drawing.Point(125, 83);
            this.chCurrentTextBox.Name = "chCurrentTextBox";
            this.chCurrentTextBox.PasswordChar = '•';
            this.chCurrentTextBox.Size = new System.Drawing.Size(164, 20);
            this.chCurrentTextBox.TabIndex = 2;
            this.chCurrentTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chPanel_KeyDown);
            // 
            // chNewTextBox
            // 
            this.chNewTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.chNewTextBox.Location = new System.Drawing.Point(125, 109);
            this.chNewTextBox.Name = "chNewTextBox";
            this.chNewTextBox.PasswordChar = '•';
            this.chNewTextBox.Size = new System.Drawing.Size(164, 20);
            this.chNewTextBox.TabIndex = 3;
            this.chNewTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chPanel_KeyDown);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 54);
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
            this.label1.Text = "Please your current and your new password!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(125, 161);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 5;
            this.loginButton.Text = "Change";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.changeButton_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 80);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.MaximumSize = new System.Drawing.Size(105, 25);
            this.label3.MinimumSize = new System.Drawing.Size(105, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Current password:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(214, 161);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // chUsernameTextBox
            // 
            this.chUsernameTextBox.Enabled = false;
            this.chUsernameTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.chUsernameTextBox.Location = new System.Drawing.Point(125, 57);
            this.chUsernameTextBox.Name = "chUsernameTextBox";
            this.chUsernameTextBox.Size = new System.Drawing.Size(164, 20);
            this.chUsernameTextBox.TabIndex = 1;
            this.chUsernameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chPanel_KeyDown);
            // 
            // registerGroupBox
            // 
            this.registerGroupBox.Controls.Add(this.logoutButton);
            this.registerGroupBox.Controls.Add(this.cancelButton2);
            this.registerGroupBox.Location = new System.Drawing.Point(313, 12);
            this.registerGroupBox.Name = "registerGroupBox";
            this.registerGroupBox.Size = new System.Drawing.Size(296, 194);
            this.registerGroupBox.TabIndex = 0;
            this.registerGroupBox.TabStop = false;
            this.registerGroupBox.Text = "Register";
            // 
            // logoutButton
            // 
            this.logoutButton.Location = new System.Drawing.Point(125, 161);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(75, 23);
            this.logoutButton.TabIndex = 7;
            this.logoutButton.Text = "Log out";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // cancelButton2
            // 
            this.cancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton2.Location = new System.Drawing.Point(215, 161);
            this.cancelButton2.Name = "cancelButton2";
            this.cancelButton2.Size = new System.Drawing.Size(75, 23);
            this.cancelButton2.TabIndex = 8;
            this.cancelButton2.Text = "Cancel";
            this.cancelButton2.UseVisualStyleBackColor = true;
            this.cancelButton2.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // UserDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(621, 221);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UserDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User";
            this.panel1.ResumeLayout(false);
            this.loginGroupBox.ResumeLayout(false);
            this.loginGroupBox.PerformLayout();
            this.registerGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox loginGroupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox chUsernameTextBox;
        private System.Windows.Forms.TextBox chCurrentTextBox;
        private System.Windows.Forms.GroupBox registerGroupBox;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.Button cancelButton2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox chNewTextBox;
        private System.Windows.Forms.TextBox chConfirmTextBox;

    }
}