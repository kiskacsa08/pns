namespace PNSDraw
{
    partial class SolverSettingsDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbMoneyUnit = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbTimeUnit = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbMassUnit = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.numOUPropCost = new System.Windows.Forms.NumericUpDown();
            this.numOUFixCost = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numOUCapacityUpper = new System.Windows.Forms.NumericUpDown();
            this.numOUCapacityLower = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numMatPrice = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numMatFlowRateUpper = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numMatFlowRateLower = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbDefMat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numFlowRate = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOUPropCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOUFixCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOUCapacityUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOUCapacityLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatFlowRateUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatFlowRateLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFlowRate)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 413);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(409, 46);
            this.panel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(322, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(241, 11);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(144, 11);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(409, 413);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.cmbMoneyUnit);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.cmbTimeUnit);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.cmbMassUnit);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(401, 387);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Measurement Units";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(219, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Set measurement units for the actual problem";
            // 
            // cmbMoneyUnit
            // 
            this.cmbMoneyUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMoneyUnit.FormattingEnabled = true;
            this.cmbMoneyUnit.Items.AddRange(new object[] {
            "EUR",
            "HUF"});
            this.cmbMoneyUnit.Location = new System.Drawing.Point(87, 163);
            this.cmbMoneyUnit.Name = "cmbMoneyUnit";
            this.cmbMoneyUnit.Size = new System.Drawing.Size(121, 21);
            this.cmbMoneyUnit.TabIndex = 5;
            this.cmbMoneyUnit.SelectedIndexChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Money unit";
            // 
            // cmbTimeUnit
            // 
            this.cmbTimeUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeUnit.FormattingEnabled = true;
            this.cmbTimeUnit.Items.AddRange(new object[] {
            "second",
            "hour",
            "day",
            "week",
            "month",
            "year"});
            this.cmbTimeUnit.Location = new System.Drawing.Point(87, 97);
            this.cmbTimeUnit.Name = "cmbTimeUnit";
            this.cmbTimeUnit.Size = new System.Drawing.Size(121, 21);
            this.cmbTimeUnit.TabIndex = 3;
            this.cmbTimeUnit.SelectedIndexChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Time unit";
            // 
            // cmbMassUnit
            // 
            this.cmbMassUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMassUnit.FormattingEnabled = true;
            this.cmbMassUnit.Items.AddRange(new object[] {
            "gram",
            "kilogram",
            "ton"});
            this.cmbMassUnit.Location = new System.Drawing.Point(87, 40);
            this.cmbMassUnit.Name = "cmbMassUnit";
            this.cmbMassUnit.Size = new System.Drawing.Size(121, 21);
            this.cmbMassUnit.TabIndex = 1;
            this.cmbMassUnit.SelectedIndexChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mass unit";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.numFlowRate);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.numOUPropCost);
            this.tabPage2.Controls.Add(this.numOUFixCost);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.numOUCapacityUpper);
            this.tabPage2.Controls.Add(this.numOUCapacityLower);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.numMatPrice);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.numMatFlowRateUpper);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.numMatFlowRateLower);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.cmbDefMat);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(401, 387);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Default Values";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(19, 289);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(154, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Operating unit proportional cost";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 253);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(109, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Operating unit fix cost";
            // 
            // numOUPropCost
            // 
            this.numOUPropCost.DecimalPlaces = 2;
            this.numOUPropCost.Location = new System.Drawing.Point(228, 287);
            this.numOUPropCost.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numOUPropCost.Name = "numOUPropCost";
            this.numOUPropCost.Size = new System.Drawing.Size(120, 20);
            this.numOUPropCost.TabIndex = 15;
            this.numOUPropCost.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // numOUFixCost
            // 
            this.numOUFixCost.DecimalPlaces = 2;
            this.numOUFixCost.Location = new System.Drawing.Point(227, 251);
            this.numOUFixCost.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numOUFixCost.Name = "numOUFixCost";
            this.numOUFixCost.Size = new System.Drawing.Size(120, 20);
            this.numOUFixCost.TabIndex = 14;
            this.numOUFixCost.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 219);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(179, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Operating unit capacity upper bound";
            // 
            // numOUCapacityUpper
            // 
            this.numOUCapacityUpper.DecimalPlaces = 2;
            this.numOUCapacityUpper.Location = new System.Drawing.Point(227, 217);
            this.numOUCapacityUpper.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numOUCapacityUpper.Name = "numOUCapacityUpper";
            this.numOUCapacityUpper.Size = new System.Drawing.Size(120, 20);
            this.numOUCapacityUpper.TabIndex = 11;
            this.numOUCapacityUpper.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // numOUCapacityLower
            // 
            this.numOUCapacityLower.DecimalPlaces = 2;
            this.numOUCapacityLower.Location = new System.Drawing.Point(226, 181);
            this.numOUCapacityLower.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numOUCapacityLower.Name = "numOUCapacityLower";
            this.numOUCapacityLower.Size = new System.Drawing.Size(120, 20);
            this.numOUCapacityLower.TabIndex = 10;
            this.numOUCapacityLower.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 183);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(177, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Operating unit capacity lower bound";
            // 
            // numMatPrice
            // 
            this.numMatPrice.DecimalPlaces = 2;
            this.numMatPrice.Location = new System.Drawing.Point(226, 147);
            this.numMatPrice.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numMatPrice.Name = "numMatPrice";
            this.numMatPrice.Size = new System.Drawing.Size(120, 20);
            this.numMatPrice.TabIndex = 7;
            this.numMatPrice.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 147);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Material price";
            // 
            // numMatFlowRateUpper
            // 
            this.numMatFlowRateUpper.DecimalPlaces = 2;
            this.numMatFlowRateUpper.Location = new System.Drawing.Point(226, 114);
            this.numMatFlowRateUpper.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numMatFlowRateUpper.Name = "numMatFlowRateUpper";
            this.numMatFlowRateUpper.Size = new System.Drawing.Size(120, 20);
            this.numMatFlowRateUpper.TabIndex = 5;
            this.numMatFlowRateUpper.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(150, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Material flow rate upper bound";
            // 
            // numMatFlowRateLower
            // 
            this.numMatFlowRateLower.DecimalPlaces = 2;
            this.numMatFlowRateLower.Location = new System.Drawing.Point(227, 75);
            this.numMatFlowRateLower.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numMatFlowRateLower.Name = "numMatFlowRateLower";
            this.numMatFlowRateLower.Size = new System.Drawing.Size(120, 20);
            this.numMatFlowRateLower.TabIndex = 3;
            this.numMatFlowRateLower.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(148, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Material flow rate lower bound";
            // 
            // cmbDefMat
            // 
            this.cmbDefMat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefMat.FormattingEnabled = true;
            this.cmbDefMat.Items.AddRange(new object[] {
            "Raw",
            "Intermediate",
            "Product"});
            this.cmbDefMat.Location = new System.Drawing.Point(226, 37);
            this.cmbDefMat.Name = "cmbDefMat";
            this.cmbDefMat.Size = new System.Drawing.Size(121, 21);
            this.cmbDefMat.TabIndex = 1;
            this.cmbDefMat.SelectedIndexChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Default material type";
            // 
            // numFlowRate
            // 
            this.numFlowRate.DecimalPlaces = 2;
            this.numFlowRate.Location = new System.Drawing.Point(229, 323);
            this.numFlowRate.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numFlowRate.Name = "numFlowRate";
            this.numFlowRate.Size = new System.Drawing.Size(120, 20);
            this.numFlowRate.TabIndex = 19;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(19, 325);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 13);
            this.label13.TabIndex = 20;
            this.label13.Text = "Flow rate";
            // 
            // SolverSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 459);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SolverSettingsDialog";
            this.Text = "Solver Settings";
            this.Load += new System.EventHandler(this.SolverSettingsDialog_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOUPropCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOUFixCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOUCapacityUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOUCapacityLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatFlowRateUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatFlowRateLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFlowRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cmbTimeUnit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbMassUnit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbMoneyUnit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDefMat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numMatFlowRateUpper;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numMatFlowRateLower;
        private System.Windows.Forms.NumericUpDown numMatPrice;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numOUPropCost;
        private System.Windows.Forms.NumericUpDown numOUFixCost;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numOUCapacityUpper;
        private System.Windows.Forms.NumericUpDown numOUCapacityLower;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numFlowRate;
        private System.Windows.Forms.Label label13;
    }
}