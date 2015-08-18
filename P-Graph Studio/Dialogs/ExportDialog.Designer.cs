namespace PNSDraw.Dialogs
{
    partial class ExportDialog
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.sizeGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.otherSizeHeight = new System.Windows.Forms.NumericUpDown();
            this.otherSizeWidth = new System.Windows.Forms.NumericUpDown();
            this.otherRadioButton = new System.Windows.Forms.RadioButton();
            this.largeRadioButton = new System.Windows.Forms.RadioButton();
            this.smallRadioButton = new System.Windows.Forms.RadioButton();
            this.mediumRadioButton = new System.Windows.Forms.RadioButton();
            this.formatGroupBox = new System.Windows.Forms.GroupBox();
            this.zimplRadionButton = new System.Windows.Forms.RadioButton();
            this.excelRadioButton = new System.Windows.Forms.RadioButton();
            this.svgRadionButton = new System.Windows.Forms.RadioButton();
            this.pngRadioButton = new System.Windows.Forms.RadioButton();
            this.jpgRadionButton = new System.Windows.Forms.RadioButton();
            this.excelGroupBox = new System.Windows.Forms.GroupBox();
            this.excelReviewButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.excelDetailedRadioButton = new System.Windows.Forms.RadioButton();
            this.excelSummaryRadioButton = new System.Windows.Forms.RadioButton();
            this.excelProblemRadioButton = new System.Windows.Forms.RadioButton();
            this.excelBriefRadioButton = new System.Windows.Forms.RadioButton();
            this.sizeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.otherSizeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherSizeWidth)).BeginInit();
            this.formatGroupBox.SuspendLayout();
            this.excelGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(349, 158);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(268, 158);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(75, 23);
            this.exportButton.TabIndex = 1;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // sizeGroupBox
            // 
            this.sizeGroupBox.Controls.Add(this.label1);
            this.sizeGroupBox.Controls.Add(this.otherSizeHeight);
            this.sizeGroupBox.Controls.Add(this.otherSizeWidth);
            this.sizeGroupBox.Controls.Add(this.otherRadioButton);
            this.sizeGroupBox.Controls.Add(this.largeRadioButton);
            this.sizeGroupBox.Controls.Add(this.smallRadioButton);
            this.sizeGroupBox.Controls.Add(this.mediumRadioButton);
            this.sizeGroupBox.Location = new System.Drawing.Point(224, 12);
            this.sizeGroupBox.Name = "sizeGroupBox";
            this.sizeGroupBox.Size = new System.Drawing.Size(200, 140);
            this.sizeGroupBox.TabIndex = 5;
            this.sizeGroupBox.TabStop = false;
            this.sizeGroupBox.Text = "Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(116, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "×";
            // 
            // otherSizeHeight
            // 
            this.otherSizeHeight.Location = new System.Drawing.Point(135, 90);
            this.otherSizeHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.otherSizeHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.otherSizeHeight.Name = "otherSizeHeight";
            this.otherSizeHeight.Size = new System.Drawing.Size(47, 20);
            this.otherSizeHeight.TabIndex = 13;
            this.otherSizeHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // otherSizeWidth
            // 
            this.otherSizeWidth.Location = new System.Drawing.Point(63, 90);
            this.otherSizeWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.otherSizeWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.otherSizeWidth.Name = "otherSizeWidth";
            this.otherSizeWidth.Size = new System.Drawing.Size(47, 20);
            this.otherSizeWidth.TabIndex = 12;
            this.otherSizeWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // otherRadioButton
            // 
            this.otherRadioButton.AutoSize = true;
            this.otherRadioButton.Location = new System.Drawing.Point(6, 90);
            this.otherRadioButton.Name = "otherRadioButton";
            this.otherRadioButton.Size = new System.Drawing.Size(51, 17);
            this.otherRadioButton.TabIndex = 11;
            this.otherRadioButton.TabStop = true;
            this.otherRadioButton.Text = "Other";
            this.otherRadioButton.UseVisualStyleBackColor = true;
            // 
            // largeRadioButton
            // 
            this.largeRadioButton.AutoSize = true;
            this.largeRadioButton.Location = new System.Drawing.Point(6, 67);
            this.largeRadioButton.Name = "largeRadioButton";
            this.largeRadioButton.Size = new System.Drawing.Size(52, 17);
            this.largeRadioButton.TabIndex = 10;
            this.largeRadioButton.TabStop = true;
            this.largeRadioButton.Text = "Large";
            this.largeRadioButton.UseVisualStyleBackColor = true;
            // 
            // smallRadioButton
            // 
            this.smallRadioButton.AutoSize = true;
            this.smallRadioButton.Location = new System.Drawing.Point(6, 19);
            this.smallRadioButton.Name = "smallRadioButton";
            this.smallRadioButton.Size = new System.Drawing.Size(50, 17);
            this.smallRadioButton.TabIndex = 8;
            this.smallRadioButton.TabStop = true;
            this.smallRadioButton.Text = "Small";
            this.smallRadioButton.UseVisualStyleBackColor = true;
            // 
            // mediumRadioButton
            // 
            this.mediumRadioButton.AutoSize = true;
            this.mediumRadioButton.Location = new System.Drawing.Point(6, 43);
            this.mediumRadioButton.Name = "mediumRadioButton";
            this.mediumRadioButton.Size = new System.Drawing.Size(62, 17);
            this.mediumRadioButton.TabIndex = 9;
            this.mediumRadioButton.TabStop = true;
            this.mediumRadioButton.Text = "Medium";
            this.mediumRadioButton.UseVisualStyleBackColor = true;
            // 
            // formatGroupBox
            // 
            this.formatGroupBox.Controls.Add(this.zimplRadionButton);
            this.formatGroupBox.Controls.Add(this.excelRadioButton);
            this.formatGroupBox.Controls.Add(this.svgRadionButton);
            this.formatGroupBox.Controls.Add(this.pngRadioButton);
            this.formatGroupBox.Controls.Add(this.jpgRadionButton);
            this.formatGroupBox.Location = new System.Drawing.Point(12, 12);
            this.formatGroupBox.Name = "formatGroupBox";
            this.formatGroupBox.Size = new System.Drawing.Size(200, 140);
            this.formatGroupBox.TabIndex = 4;
            this.formatGroupBox.TabStop = false;
            this.formatGroupBox.Text = "Format";
            // 
            // zimplRadionButton
            // 
            this.zimplRadionButton.AutoSize = true;
            this.zimplRadionButton.Location = new System.Drawing.Point(6, 113);
            this.zimplRadionButton.Name = "zimplRadionButton";
            this.zimplRadionButton.Size = new System.Drawing.Size(57, 17);
            this.zimplRadionButton.TabIndex = 7;
            this.zimplRadionButton.TabStop = true;
            this.zimplRadionButton.Text = "ZIMPL";
            this.zimplRadionButton.UseVisualStyleBackColor = true;
            this.zimplRadionButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.formatRadio_CheckedChange);
            // 
            // excelRadioButton
            // 
            this.excelRadioButton.AutoSize = true;
            this.excelRadioButton.Location = new System.Drawing.Point(6, 90);
            this.excelRadioButton.Name = "excelRadioButton";
            this.excelRadioButton.Size = new System.Drawing.Size(51, 17);
            this.excelRadioButton.TabIndex = 6;
            this.excelRadioButton.TabStop = true;
            this.excelRadioButton.Text = "Excel";
            this.excelRadioButton.UseVisualStyleBackColor = true;
            this.excelRadioButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.formatRadio_CheckedChange);
            // 
            // svgRadionButton
            // 
            this.svgRadionButton.AutoSize = true;
            this.svgRadionButton.Location = new System.Drawing.Point(6, 67);
            this.svgRadionButton.Name = "svgRadionButton";
            this.svgRadionButton.Size = new System.Drawing.Size(47, 17);
            this.svgRadionButton.TabIndex = 5;
            this.svgRadionButton.TabStop = true;
            this.svgRadionButton.Text = "SVG";
            this.svgRadionButton.UseVisualStyleBackColor = true;
            this.svgRadionButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.formatRadio_CheckedChange);
            // 
            // pngRadioButton
            // 
            this.pngRadioButton.AutoSize = true;
            this.pngRadioButton.Location = new System.Drawing.Point(6, 43);
            this.pngRadioButton.Name = "pngRadioButton";
            this.pngRadioButton.Size = new System.Drawing.Size(48, 17);
            this.pngRadioButton.TabIndex = 4;
            this.pngRadioButton.TabStop = true;
            this.pngRadioButton.Text = "PNG";
            this.pngRadioButton.UseVisualStyleBackColor = true;
            this.pngRadioButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.formatRadio_CheckedChange);
            // 
            // jpgRadionButton
            // 
            this.jpgRadionButton.AutoSize = true;
            this.jpgRadionButton.Location = new System.Drawing.Point(6, 19);
            this.jpgRadionButton.Name = "jpgRadionButton";
            this.jpgRadionButton.Size = new System.Drawing.Size(78, 17);
            this.jpgRadionButton.TabIndex = 3;
            this.jpgRadionButton.TabStop = true;
            this.jpgRadionButton.Text = "JPG, JPEG";
            this.jpgRadionButton.UseVisualStyleBackColor = true;
            this.jpgRadionButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.formatRadio_CheckedChange);
            // 
            // excelGroupBox
            // 
            this.excelGroupBox.Controls.Add(this.excelReviewButton);
            this.excelGroupBox.Controls.Add(this.label3);
            this.excelGroupBox.Controls.Add(this.label2);
            this.excelGroupBox.Controls.Add(this.excelDetailedRadioButton);
            this.excelGroupBox.Controls.Add(this.excelSummaryRadioButton);
            this.excelGroupBox.Controls.Add(this.excelProblemRadioButton);
            this.excelGroupBox.Controls.Add(this.excelBriefRadioButton);
            this.excelGroupBox.Location = new System.Drawing.Point(224, 12);
            this.excelGroupBox.Name = "excelGroupBox";
            this.excelGroupBox.Size = new System.Drawing.Size(200, 140);
            this.excelGroupBox.TabIndex = 5;
            this.excelGroupBox.TabStop = false;
            this.excelGroupBox.Text = "Excel";
            this.excelGroupBox.Visible = false;
            // 
            // excelReviewButton
            // 
            this.excelReviewButton.Location = new System.Drawing.Point(119, 52);
            this.excelReviewButton.Name = "excelReviewButton";
            this.excelReviewButton.Size = new System.Drawing.Size(75, 23);
            this.excelReviewButton.TabIndex = 12;
            this.excelReviewButton.Text = "Review";
            this.excelReviewButton.UseVisualStyleBackColor = true;
            this.excelReviewButton.Click += new System.EventHandler(this.excelReviewButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Problem";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Solution";
            // 
            // excelDetailedRadioButton
            // 
            this.excelDetailedRadioButton.AutoSize = true;
            this.excelDetailedRadioButton.Location = new System.Drawing.Point(9, 91);
            this.excelDetailedRadioButton.Name = "excelDetailedRadioButton";
            this.excelDetailedRadioButton.Size = new System.Drawing.Size(64, 17);
            this.excelDetailedRadioButton.TabIndex = 10;
            this.excelDetailedRadioButton.TabStop = true;
            this.excelDetailedRadioButton.Text = "Detailed";
            this.excelDetailedRadioButton.UseVisualStyleBackColor = true;
            // 
            // excelSummaryRadioButton
            // 
            this.excelSummaryRadioButton.AutoSize = true;
            this.excelSummaryRadioButton.Location = new System.Drawing.Point(9, 114);
            this.excelSummaryRadioButton.Name = "excelSummaryRadioButton";
            this.excelSummaryRadioButton.Size = new System.Drawing.Size(131, 17);
            this.excelSummaryRadioButton.TabIndex = 11;
            this.excelSummaryRadioButton.TabStop = true;
            this.excelSummaryRadioButton.Text = "Summary (all solutions)";
            this.excelSummaryRadioButton.UseVisualStyleBackColor = true;
            // 
            // excelProblemRadioButton
            // 
            this.excelProblemRadioButton.AutoSize = true;
            this.excelProblemRadioButton.Location = new System.Drawing.Point(9, 32);
            this.excelProblemRadioButton.Name = "excelProblemRadioButton";
            this.excelProblemRadioButton.Size = new System.Drawing.Size(63, 17);
            this.excelProblemRadioButton.TabIndex = 8;
            this.excelProblemRadioButton.TabStop = true;
            this.excelProblemRadioButton.Text = "Problem";
            this.excelProblemRadioButton.UseVisualStyleBackColor = true;
            // 
            // excelBriefRadioButton
            // 
            this.excelBriefRadioButton.AutoSize = true;
            this.excelBriefRadioButton.Location = new System.Drawing.Point(9, 68);
            this.excelBriefRadioButton.Name = "excelBriefRadioButton";
            this.excelBriefRadioButton.Size = new System.Drawing.Size(46, 17);
            this.excelBriefRadioButton.TabIndex = 9;
            this.excelBriefRadioButton.TabStop = true;
            this.excelBriefRadioButton.Text = "Brief";
            this.excelBriefRadioButton.UseVisualStyleBackColor = true;
            // 
            // ExportDialog
            // 
            this.AcceptButton = this.exportButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(436, 191);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.excelGroupBox);
            this.Controls.Add(this.sizeGroupBox);
            this.Controls.Add(this.formatGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export...";
            this.sizeGroupBox.ResumeLayout(false);
            this.sizeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.otherSizeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherSizeWidth)).EndInit();
            this.formatGroupBox.ResumeLayout(false);
            this.formatGroupBox.PerformLayout();
            this.excelGroupBox.ResumeLayout(false);
            this.excelGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.GroupBox sizeGroupBox;
        private System.Windows.Forms.GroupBox formatGroupBox;
        private System.Windows.Forms.RadioButton svgRadionButton;
        private System.Windows.Forms.RadioButton pngRadioButton;
        private System.Windows.Forms.RadioButton jpgRadionButton;
        private System.Windows.Forms.RadioButton largeRadioButton;
        private System.Windows.Forms.RadioButton smallRadioButton;
        private System.Windows.Forms.RadioButton mediumRadioButton;
        private System.Windows.Forms.NumericUpDown otherSizeWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton otherRadioButton;
        private System.Windows.Forms.NumericUpDown otherSizeHeight;
        private System.Windows.Forms.RadioButton zimplRadionButton;
        private System.Windows.Forms.RadioButton excelRadioButton;
        private System.Windows.Forms.GroupBox excelGroupBox;
        private System.Windows.Forms.RadioButton excelDetailedRadioButton;
        private System.Windows.Forms.RadioButton excelProblemRadioButton;
        private System.Windows.Forms.RadioButton excelBriefRadioButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton excelSummaryRadioButton;
        private System.Windows.Forms.Button excelReviewButton;
    }
}