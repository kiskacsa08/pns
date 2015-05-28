namespace Pns.Dialogs
{
    partial class MaterialQuantityTypeChanged
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
            this.radioButtonDefaultMU = new System.Windows.Forms.RadioButton();
            this.groupBoxFlowMUSettings = new System.Windows.Forms.GroupBox();
            this.radioButtonMaterialMU = new System.Windows.Forms.RadioButton();
            this.groupBoxFlowValueSettings = new System.Windows.Forms.GroupBox();
            this.radioButtonSetToUserActionNeededState = new System.Windows.Forms.RadioButton();
            this.radioButtonSetToDefaultFlowRate = new System.Windows.Forms.RadioButton();
            this.radioButtonKeepCurrentValue = new System.Windows.Forms.RadioButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxFlowMUSettings.SuspendLayout();
            this.groupBoxFlowValueSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonDefaultMU
            // 
            this.radioButtonDefaultMU.AutoSize = true;
            this.radioButtonDefaultMU.Location = new System.Drawing.Point(6, 42);
            this.radioButtonDefaultMU.Name = "radioButtonDefaultMU";
            this.radioButtonDefaultMU.Size = new System.Drawing.Size(174, 17);
            this.radioButtonDefaultMU.TabIndex = 0;
            this.radioButtonDefaultMU.TabStop = true;
            this.radioButtonDefaultMU.Text = "Set to default measurement unit";
            this.radioButtonDefaultMU.UseVisualStyleBackColor = true;
            // 
            // groupBoxFlowMUSettings
            // 
            this.groupBoxFlowMUSettings.Controls.Add(this.radioButtonMaterialMU);
            this.groupBoxFlowMUSettings.Controls.Add(this.radioButtonDefaultMU);
            this.groupBoxFlowMUSettings.Location = new System.Drawing.Point(12, 12);
            this.groupBoxFlowMUSettings.Name = "groupBoxFlowMUSettings";
            this.groupBoxFlowMUSettings.Size = new System.Drawing.Size(296, 67);
            this.groupBoxFlowMUSettings.TabIndex = 1;
            this.groupBoxFlowMUSettings.TabStop = false;
            this.groupBoxFlowMUSettings.Text = "Measurement unit settings of concerned flows";
            // 
            // radioButtonMaterialMU
            // 
            this.radioButtonMaterialMU.AutoSize = true;
            this.radioButtonMaterialMU.Location = new System.Drawing.Point(6, 19);
            this.radioButtonMaterialMU.Name = "radioButtonMaterialMU";
            this.radioButtonMaterialMU.Size = new System.Drawing.Size(273, 17);
            this.radioButtonMaterialMU.TabIndex = 1;
            this.radioButtonMaterialMU.TabStop = true;
            this.radioButtonMaterialMU.Text = "Set to measurement unit of the material minimum flow";
            this.radioButtonMaterialMU.UseVisualStyleBackColor = true;
            // 
            // groupBoxFlowValueSettings
            // 
            this.groupBoxFlowValueSettings.Controls.Add(this.radioButtonSetToUserActionNeededState);
            this.groupBoxFlowValueSettings.Controls.Add(this.radioButtonSetToDefaultFlowRate);
            this.groupBoxFlowValueSettings.Controls.Add(this.radioButtonKeepCurrentValue);
            this.groupBoxFlowValueSettings.Location = new System.Drawing.Point(12, 85);
            this.groupBoxFlowValueSettings.Name = "groupBoxFlowValueSettings";
            this.groupBoxFlowValueSettings.Size = new System.Drawing.Size(296, 90);
            this.groupBoxFlowValueSettings.TabIndex = 2;
            this.groupBoxFlowValueSettings.TabStop = false;
            this.groupBoxFlowValueSettings.Text = "Value settings of concerned flows";
            // 
            // radioButtonSetToUserActionNeededState
            // 
            this.radioButtonSetToUserActionNeededState.AutoSize = true;
            this.radioButtonSetToUserActionNeededState.Location = new System.Drawing.Point(6, 67);
            this.radioButtonSetToUserActionNeededState.Name = "radioButtonSetToUserActionNeededState";
            this.radioButtonSetToUserActionNeededState.Size = new System.Drawing.Size(266, 17);
            this.radioButtonSetToUserActionNeededState.TabIndex = 2;
            this.radioButtonSetToUserActionNeededState.TabStop = true;
            this.radioButtonSetToUserActionNeededState.Text = "Set concerned flows to \"User action needed\" state";
            this.radioButtonSetToUserActionNeededState.UseVisualStyleBackColor = true;
            // 
            // radioButtonSetToDefaultFlowRate
            // 
            this.radioButtonSetToDefaultFlowRate.AutoSize = true;
            this.radioButtonSetToDefaultFlowRate.Location = new System.Drawing.Point(6, 44);
            this.radioButtonSetToDefaultFlowRate.Name = "radioButtonSetToDefaultFlowRate";
            this.radioButtonSetToDefaultFlowRate.Size = new System.Drawing.Size(238, 17);
            this.radioButtonSetToDefaultFlowRate.TabIndex = 1;
            this.radioButtonSetToDefaultFlowRate.TabStop = true;
            this.radioButtonSetToDefaultFlowRate.Text = "Set concerned flows to default flowrate value";
            this.radioButtonSetToDefaultFlowRate.UseVisualStyleBackColor = true;
            // 
            // radioButtonKeepCurrentValue
            // 
            this.radioButtonKeepCurrentValue.AutoSize = true;
            this.radioButtonKeepCurrentValue.Location = new System.Drawing.Point(6, 21);
            this.radioButtonKeepCurrentValue.Name = "radioButtonKeepCurrentValue";
            this.radioButtonKeepCurrentValue.Size = new System.Drawing.Size(120, 17);
            this.radioButtonKeepCurrentValue.TabIndex = 0;
            this.radioButtonKeepCurrentValue.TabStop = true;
            this.radioButtonKeepCurrentValue.Text = "Keep current values";
            this.radioButtonKeepCurrentValue.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(58, 203);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(187, 203);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // MaterialQuantityTypeChanged
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(320, 238);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxFlowValueSettings);
            this.Controls.Add(this.groupBoxFlowMUSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaterialQuantityTypeChanged";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Material quantity type changed";
            this.groupBoxFlowMUSettings.ResumeLayout(false);
            this.groupBoxFlowMUSettings.PerformLayout();
            this.groupBoxFlowValueSettings.ResumeLayout(false);
            this.groupBoxFlowValueSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonDefaultMU;
        private System.Windows.Forms.GroupBox groupBoxFlowMUSettings;
        private System.Windows.Forms.RadioButton radioButtonMaterialMU;
        private System.Windows.Forms.GroupBox groupBoxFlowValueSettings;
        private System.Windows.Forms.RadioButton radioButtonSetToUserActionNeededState;
        private System.Windows.Forms.RadioButton radioButtonSetToDefaultFlowRate;
        private System.Windows.Forms.RadioButton radioButtonKeepCurrentValue;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}