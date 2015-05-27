namespace Pns.Dialogs
{
    partial class SolverDialog
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
            this.buttonAbort = new System.Windows.Forms.Button();
            this.richTextBoxSolverOutput = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // buttonAbort
            // 
            this.buttonAbort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonAbort.Location = new System.Drawing.Point(436, 274);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(75, 23);
            this.buttonAbort.TabIndex = 1;
            this.buttonAbort.Text = "Abort";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // richTextBoxSolverOutput
            // 
            this.richTextBoxSolverOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxSolverOutput.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxSolverOutput.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxSolverOutput.Name = "richTextBoxSolverOutput";
            this.richTextBoxSolverOutput.ReadOnly = true;
            this.richTextBoxSolverOutput.Size = new System.Drawing.Size(499, 256);
            this.richTextBoxSolverOutput.TabIndex = 2;
            this.richTextBoxSolverOutput.Text = "Solver is running...\n";
            // 
            // Solver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.CancelButton = this.buttonAbort;
            this.ClientSize = new System.Drawing.Size(523, 309);
            this.Controls.Add(this.richTextBoxSolverOutput);
            this.Controls.Add(this.buttonAbort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Solver";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Solver status";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Solver_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAbort;
        private System.Windows.Forms.RichTextBox richTextBoxSolverOutput;
    }
}