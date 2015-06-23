namespace PNSDraw
{
    partial class SolutionSettingsWindow
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
            this.button_set = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_im = new System.Windows.Forms.ComboBox();
            this.cb_io = new System.Windows.Forms.ComboBox();
            this.cb_ie = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pb_i = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cb_em = new System.Windows.Forms.ComboBox();
            this.cb_eo = new System.Windows.Forms.ComboBox();
            this.cb_ee = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pb_e = new System.Windows.Forms.PictureBox();
            this.cb_ev = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_i)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_e)).BeginInit();
            this.SuspendLayout();
            // 
            // button_set
            // 
            this.button_set.Location = new System.Drawing.Point(95, 255);
            this.button_set.Name = "button_set";
            this.button_set.Size = new System.Drawing.Size(120, 40);
            this.button_set.TabIndex = 0;
            this.button_set.Text = "Set";
            this.button_set.UseVisualStyleBackColor = true;
            this.button_set.Click += new System.EventHandler(this.button_set_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(430, 255);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(120, 40);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 217);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Included Materials";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.75439F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.24561F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cb_im, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cb_io, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cb_ie, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.pb_i, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(285, 120);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Materials Text:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "Operating Units Text:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "Flows Text:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cb_im
            // 
            this.cb_im.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_im.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_im.FormattingEnabled = true;
            this.cb_im.Items.AddRange(new object[] {
            "None",
            "Original Value",
            "Solution Value"});
            this.cb_im.Location = new System.Drawing.Point(122, 3);
            this.cb_im.Name = "cb_im";
            this.cb_im.Size = new System.Drawing.Size(160, 21);
            this.cb_im.TabIndex = 3;
            // 
            // cb_io
            // 
            this.cb_io.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_io.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_io.FormattingEnabled = true;
            this.cb_io.Items.AddRange(new object[] {
            "None",
            "Original Value",
            "Solution Value"});
            this.cb_io.Location = new System.Drawing.Point(122, 33);
            this.cb_io.Name = "cb_io";
            this.cb_io.Size = new System.Drawing.Size(160, 21);
            this.cb_io.TabIndex = 4;
            // 
            // cb_ie
            // 
            this.cb_ie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_ie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ie.FormattingEnabled = true;
            this.cb_ie.Items.AddRange(new object[] {
            "None",
            "Original Value",
            "Solution Value"});
            this.cb_ie.Location = new System.Drawing.Point(122, 63);
            this.cb_ie.Name = "cb_ie";
            this.cb_ie.Size = new System.Drawing.Size(160, 21);
            this.cb_ie.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 30);
            this.label7.TabIndex = 6;
            this.label7.Text = "Color:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pb_i
            // 
            this.pb_i.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_i.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_i.Location = new System.Drawing.Point(122, 93);
            this.pb_i.Name = "pb_i";
            this.pb_i.Size = new System.Drawing.Size(160, 24);
            this.pb_i.TabIndex = 7;
            this.pb_i.TabStop = false;
            this.pb_i.Click += new System.EventHandler(this.pb_i_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Location = new System.Drawing.Point(341, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 217);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Excluded Materials";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.75439F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.24561F));
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cb_em, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cb_eo, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.cb_ee, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.pb_e, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.cb_ev, 1, 4);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(285, 150);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 30);
            this.label4.TabIndex = 0;
            this.label4.Text = "Materials Text:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 30);
            this.label5.TabIndex = 1;
            this.label5.Text = "Operating Units Text:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 30);
            this.label6.TabIndex = 2;
            this.label6.Text = "Flows Text:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cb_em
            // 
            this.cb_em.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_em.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_em.FormattingEnabled = true;
            this.cb_em.Items.AddRange(new object[] {
            "None",
            "Original Value"});
            this.cb_em.Location = new System.Drawing.Point(122, 3);
            this.cb_em.Name = "cb_em";
            this.cb_em.Size = new System.Drawing.Size(160, 21);
            this.cb_em.TabIndex = 3;
            // 
            // cb_eo
            // 
            this.cb_eo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_eo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_eo.FormattingEnabled = true;
            this.cb_eo.Items.AddRange(new object[] {
            "None",
            "Original Value"});
            this.cb_eo.Location = new System.Drawing.Point(122, 33);
            this.cb_eo.Name = "cb_eo";
            this.cb_eo.Size = new System.Drawing.Size(160, 21);
            this.cb_eo.TabIndex = 4;
            // 
            // cb_ee
            // 
            this.cb_ee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_ee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ee.FormattingEnabled = true;
            this.cb_ee.Items.AddRange(new object[] {
            "None",
            "Original Value"});
            this.cb_ee.Location = new System.Drawing.Point(122, 63);
            this.cb_ee.Name = "cb_ee";
            this.cb_ee.Size = new System.Drawing.Size(160, 21);
            this.cb_ee.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 30);
            this.label8.TabIndex = 6;
            this.label8.Text = "Color:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 30);
            this.label9.TabIndex = 7;
            this.label9.Text = "Visibility:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pb_e
            // 
            this.pb_e.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_e.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_e.Location = new System.Drawing.Point(122, 93);
            this.pb_e.Name = "pb_e";
            this.pb_e.Size = new System.Drawing.Size(160, 24);
            this.pb_e.TabIndex = 8;
            this.pb_e.TabStop = false;
            this.pb_e.Click += new System.EventHandler(this.pb_e_Click);
            // 
            // cb_ev
            // 
            this.cb_ev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_ev.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ev.FormattingEnabled = true;
            this.cb_ev.Items.AddRange(new object[] {
            "Hide",
            "Show"});
            this.cb_ev.Location = new System.Drawing.Point(122, 123);
            this.cb_ev.Name = "cb_ev";
            this.cb_ev.Size = new System.Drawing.Size(160, 21);
            this.cb_ev.TabIndex = 9;
            this.cb_ev.SelectedIndexChanged += new System.EventHandler(this.cb_ev_SelectedIndexChanged);
            // 
            // SolutionSettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 302);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_set);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(670, 341);
            this.MinimumSize = new System.Drawing.Size(670, 341);
            this.Name = "SolutionSettingsWindow";
            this.Text = "SolutionSettingsWindow";
            this.Load += new System.EventHandler(this.SolutionSettingsWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_i)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_e)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_set;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_im;
        private System.Windows.Forms.ComboBox cb_io;
        private System.Windows.Forms.ComboBox cb_ie;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cb_em;
        private System.Windows.Forms.ComboBox cb_eo;
        private System.Windows.Forms.ComboBox cb_ee;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pb_i;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pb_e;
        private System.Windows.Forms.ComboBox cb_ev;
    }
}