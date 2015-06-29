namespace PNSDraw
{
    partial class ProblemSettingsDialog
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
            this.numPayoutPeriod = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.numWorkingHour = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbQuantity = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbMoneyUnit = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbTimeUnit = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDefUnit = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.numIProp = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.numIFixed = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.numOProp = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.numOFixed = new System.Windows.Forms.NumericUpDown();
            this.numFlowRate = new System.Windows.Forms.NumericUpDown();
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
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPayoutPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWorkingHour)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIProp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIFixed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOProp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOFixed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFlowRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOUCapacityUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOUCapacityLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatFlowRateUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatFlowRateLower)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 482);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 46);
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
            this.tabControl1.Size = new System.Drawing.Size(452, 482);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.numPayoutPeriod);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.numWorkingHour);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.cmbQuantity);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.cmbMoneyUnit);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.cmbTimeUnit);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.cmbDefUnit);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(444, 456);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Measurement Units";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // numPayoutPeriod
            // 
            this.numPayoutPeriod.Location = new System.Drawing.Point(183, 371);
            this.numPayoutPeriod.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numPayoutPeriod.Name = "numPayoutPeriod";
            this.numPayoutPeriod.Size = new System.Drawing.Size(120, 20);
            this.numPayoutPeriod.TabIndex = 14;
            this.numPayoutPeriod.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(22, 373);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(137, 13);
            this.label13.TabIndex = 13;
            this.label13.Text = "Default payout period (year)";
            // 
            // numWorkingHour
            // 
            this.numWorkingHour.Location = new System.Drawing.Point(183, 305);
            this.numWorkingHour.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numWorkingHour.Name = "numWorkingHour";
            this.numWorkingHour.Size = new System.Drawing.Size(120, 20);
            this.numWorkingHour.TabIndex = 10;
            this.numWorkingHour.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 307);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(146, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Default working hour per year";
            // 
            // cmbQuantity
            // 
            this.cmbQuantity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQuantity.FormattingEnabled = true;
            this.cmbQuantity.Items.AddRange(new object[] {
            "Mass",
            "Volume",
            "Amount of substance",
            "Energy, work, heat",
            "Length",
            "Electric current",
            "Area",
            "Speed",
            "Acceleration",
            "Mass density",
            "Thermodinamic temperature",
            "Luminous intensity",
            "Concentration",
            "Force",
            "Pressure",
            "Power",
            "Capacity"});
            this.cmbQuantity.Location = new System.Drawing.Point(183, 53);
            this.cmbQuantity.Name = "cmbQuantity";
            this.cmbQuantity.Size = new System.Drawing.Size(185, 21);
            this.cmbQuantity.TabIndex = 8;
            this.cmbQuantity.SelectionChangeCommitted += new System.EventHandler(this.cmbQuantity_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(22, 56);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Quantity type";
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
            "HUF",
            "USD"});
            this.cmbMoneyUnit.Location = new System.Drawing.Point(183, 238);
            this.cmbMoneyUnit.Name = "cmbMoneyUnit";
            this.cmbMoneyUnit.Size = new System.Drawing.Size(185, 21);
            this.cmbMoneyUnit.TabIndex = 5;
            this.cmbMoneyUnit.SelectedIndexChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 241);
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
            this.cmbTimeUnit.Location = new System.Drawing.Point(183, 172);
            this.cmbTimeUnit.Name = "cmbTimeUnit";
            this.cmbTimeUnit.Size = new System.Drawing.Size(185, 21);
            this.cmbTimeUnit.TabIndex = 3;
            this.cmbTimeUnit.SelectedIndexChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Time unit";
            // 
            // cmbDefUnit
            // 
            this.cmbDefUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefUnit.FormattingEnabled = true;
            this.cmbDefUnit.Items.AddRange(new object[] {
            "gram",
            "kilogram",
            "ton"});
            this.cmbDefUnit.Location = new System.Drawing.Point(183, 114);
            this.cmbDefUnit.Name = "cmbDefUnit";
            this.cmbDefUnit.Size = new System.Drawing.Size(185, 21);
            this.cmbDefUnit.TabIndex = 1;
            this.cmbDefUnit.SelectionChangeCommitted += new System.EventHandler(this.cmbDefUnit_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default unit";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label18);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.numIProp);
            this.tabPage2.Controls.Add(this.label17);
            this.tabPage2.Controls.Add(this.numIFixed);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.numOProp);
            this.tabPage2.Controls.Add(this.label15);
            this.tabPage2.Controls.Add(this.numOFixed);
            this.tabPage2.Controls.Add(this.numFlowRate);
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
            this.tabPage2.Size = new System.Drawing.Size(444, 456);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Default Values";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(19, 399);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(50, 13);
            this.label18.TabIndex = 38;
            this.label18.Text = "Flow rate";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(19, 363);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(194, 13);
            this.label16.TabIndex = 32;
            this.label16.Text = "Investment cost proportionality constant";
            // 
            // numIProp
            // 
            this.numIProp.DecimalPlaces = 2;
            this.numIProp.Location = new System.Drawing.Point(229, 361);
            this.numIProp.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numIProp.Name = "numIProp";
            this.numIProp.Size = new System.Drawing.Size(120, 20);
            this.numIProp.TabIndex = 31;
            this.numIProp.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(19, 327);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(143, 13);
            this.label17.TabIndex = 30;
            this.label17.Text = "Investment cost fixed charge";
            // 
            // numIFixed
            // 
            this.numIFixed.DecimalPlaces = 2;
            this.numIFixed.Location = new System.Drawing.Point(228, 325);
            this.numIFixed.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numIFixed.Name = "numIFixed";
            this.numIFixed.Size = new System.Drawing.Size(120, 20);
            this.numIFixed.TabIndex = 28;
            this.numIFixed.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(19, 291);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(188, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "Operating cost proportionality constant";
            // 
            // numOProp
            // 
            this.numOProp.DecimalPlaces = 2;
            this.numOProp.Location = new System.Drawing.Point(229, 289);
            this.numOProp.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numOProp.Name = "numOProp";
            this.numOProp.Size = new System.Drawing.Size(120, 20);
            this.numOProp.TabIndex = 25;
            this.numOProp.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(19, 255);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(137, 13);
            this.label15.TabIndex = 24;
            this.label15.Text = "Operating cost fixed charge";
            // 
            // numOFixed
            // 
            this.numOFixed.DecimalPlaces = 2;
            this.numOFixed.Location = new System.Drawing.Point(228, 253);
            this.numOFixed.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numOFixed.Name = "numOFixed";
            this.numOFixed.Size = new System.Drawing.Size(120, 20);
            this.numOFixed.TabIndex = 22;
            this.numOFixed.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
            // 
            // numFlowRate
            // 
            this.numFlowRate.DecimalPlaces = 2;
            this.numFlowRate.Location = new System.Drawing.Point(229, 397);
            this.numFlowRate.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numFlowRate.Name = "numFlowRate";
            this.numFlowRate.Size = new System.Drawing.Size(120, 20);
            this.numFlowRate.TabIndex = 19;
            this.numFlowRate.ValueChanged += new System.EventHandler(this.cmbDefMat_SelectedIndexChanged);
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
            // ProblemSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 528);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ProblemSettingsDialog";
            this.Text = "Problem Settings";
            this.Load += new System.EventHandler(this.SolverSettingsDialog_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPayoutPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWorkingHour)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIProp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIFixed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOProp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOFixed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFlowRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOUCapacityUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOUCapacityLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatFlowRateUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMatFlowRateLower)).EndInit();
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
        private System.Windows.Forms.ComboBox cmbDefUnit;
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
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numOUCapacityUpper;
        private System.Windows.Forms.NumericUpDown numOUCapacityLower;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numFlowRate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown numOProp;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numOFixed;
        private System.Windows.Forms.ComboBox cmbQuantity;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown numIProp;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown numIFixed;
        private System.Windows.Forms.NumericUpDown numPayoutPeriod;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numWorkingHour;
        private System.Windows.Forms.Label label12;
    }
}