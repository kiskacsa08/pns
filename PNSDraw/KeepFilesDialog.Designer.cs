namespace PNSDraw
{
    partial class KeepFilesDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblInFileName = new System.Windows.Forms.Label();
            this.lblOutFileName = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Do you want to keep the temporary files?";
            // 
            // lblInFileName
            // 
            this.lblInFileName.AutoSize = true;
            this.lblInFileName.Location = new System.Drawing.Point(13, 38);
            this.lblInFileName.Name = "lblInFileName";
            this.lblInFileName.Size = new System.Drawing.Size(35, 13);
            this.lblInFileName.TabIndex = 1;
            this.lblInFileName.Text = "label2";
            // 
            // lblOutFileName
            // 
            this.lblOutFileName.AutoSize = true;
            this.lblOutFileName.Location = new System.Drawing.Point(13, 65);
            this.lblOutFileName.Name = "lblOutFileName";
            this.lblOutFileName.Size = new System.Drawing.Size(35, 13);
            this.lblOutFileName.TabIndex = 2;
            this.lblOutFileName.Text = "label3";
            // 
            // btnYes
            // 
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.Location = new System.Drawing.Point(49, 105);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 3;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            // 
            // btnNo
            // 
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Location = new System.Drawing.Point(159, 105);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 4;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = true;
            // 
            // KeepFilesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 147);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblOutFileName);
            this.Controls.Add(this.lblInFileName);
            this.Controls.Add(this.label1);
            this.Name = "KeepFilesDialog";
            this.Text = "Do you want to keep the files?";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblInFileName;
        private System.Windows.Forms.Label lblOutFileName;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
    }
}