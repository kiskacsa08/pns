namespace PNSDraw
{
    partial class SettingsWindow
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_gridsize = new System.Windows.Forms.TextBox();
            this.textBox_fontsize = new System.Windows.Forms.TextBox();
            this.button_reset = new System.Windows.Forms.Button();
            this.button_set = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_ft = new System.Windows.Forms.CheckBox();
            this.cb_ot = new System.Windows.Forms.CheckBox();
            this.cb_mt = new System.Windows.Forms.CheckBox();
            this.button_cancel = new System.Windows.Forms.Button();
            this.cb_sc = new System.Windows.Forms.CheckBox();
            this.cb_sp = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.24176F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.75824F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox_gridsize, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox_fontsize, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(193, 52);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default Font Size:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Grid Distance:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_gridsize
            // 
            this.textBox_gridsize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_gridsize.Location = new System.Drawing.Point(115, 29);
            this.textBox_gridsize.Name = "textBox_gridsize";
            this.textBox_gridsize.Size = new System.Drawing.Size(75, 20);
            this.textBox_gridsize.TabIndex = 3;
            this.textBox_gridsize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_fontsize
            // 
            this.textBox_fontsize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_fontsize.Location = new System.Drawing.Point(115, 3);
            this.textBox_fontsize.Name = "textBox_fontsize";
            this.textBox_fontsize.Size = new System.Drawing.Size(75, 20);
            this.textBox_fontsize.TabIndex = 4;
            this.textBox_fontsize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(60, 77);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(75, 23);
            this.button_reset.TabIndex = 1;
            this.button_reset.Text = "Reset";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_set
            // 
            this.button_set.Location = new System.Drawing.Point(58, 175);
            this.button_set.Name = "button_set";
            this.button_set.Size = new System.Drawing.Size(120, 40);
            this.button_set.TabIndex = 2;
            this.button_set.Text = "Set";
            this.button_set.UseVisualStyleBackColor = true;
            this.button_set.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Controls.Add(this.button_reset);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 148);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_sp);
            this.groupBox2.Controls.Add(this.cb_sc);
            this.groupBox2.Controls.Add(this.cb_ft);
            this.groupBox2.Controls.Add(this.cb_ot);
            this.groupBox2.Controls.Add(this.cb_mt);
            this.groupBox2.Location = new System.Drawing.Point(229, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(191, 148);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Print View";
            // 
            // cb_ft
            // 
            this.cb_ft.AutoSize = true;
            this.cb_ft.Location = new System.Drawing.Point(20, 72);
            this.cb_ft.Name = "cb_ft";
            this.cb_ft.Size = new System.Drawing.Size(107, 17);
            this.cb_ft.TabIndex = 2;
            this.cb_ft.Text = "Show Flows Text";
            this.cb_ft.UseVisualStyleBackColor = true;
            // 
            // cb_ot
            // 
            this.cb_ot.AutoSize = true;
            this.cb_ot.Location = new System.Drawing.Point(20, 48);
            this.cb_ot.Name = "cb_ot";
            this.cb_ot.Size = new System.Drawing.Size(153, 17);
            this.cb_ot.TabIndex = 1;
            this.cb_ot.Text = "Show Operating Units Text";
            this.cb_ot.UseVisualStyleBackColor = true;
            // 
            // cb_mt
            // 
            this.cb_mt.AutoSize = true;
            this.cb_mt.Location = new System.Drawing.Point(20, 24);
            this.cb_mt.Name = "cb_mt";
            this.cb_mt.Size = new System.Drawing.Size(122, 17);
            this.cb_mt.TabIndex = 0;
            this.cb_mt.Text = "Show Materials Text";
            this.cb_mt.UseVisualStyleBackColor = true;
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(265, 175);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(120, 40);
            this.button_cancel.TabIndex = 5;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // cb_sc
            // 
            this.cb_sc.AutoSize = true;
            this.cb_sc.Location = new System.Drawing.Point(20, 95);
            this.cb_sc.Name = "cb_sc";
            this.cb_sc.Size = new System.Drawing.Size(105, 17);
            this.cb_sc.TabIndex = 3;
            this.cb_sc.Text = "Show Comments";
            this.cb_sc.UseVisualStyleBackColor = true;
            // 
            // cb_sp
            // 
            this.cb_sp.AutoSize = true;
            this.cb_sp.Location = new System.Drawing.Point(20, 118);
            this.cb_sp.Name = "cb_sp";
            this.cb_sp.Size = new System.Drawing.Size(109, 17);
            this.cb_sp.TabIndex = 4;
            this.cb_sp.Text = "Show Parameters";
            this.cb_sp.UseVisualStyleBackColor = true;
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 227);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_set);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(438, 255);
            this.MinimumSize = new System.Drawing.Size(438, 255);
            this.Name = "SettingsWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsWindow_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_gridsize;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Button button_set;
        private System.Windows.Forms.TextBox textBox_fontsize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cb_ft;
        private System.Windows.Forms.CheckBox cb_ot;
        private System.Windows.Forms.CheckBox cb_mt;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.CheckBox cb_sp;
        private System.Windows.Forms.CheckBox cb_sc;

    }
}