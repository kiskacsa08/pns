namespace Pns.Dialogs
{
    partial class OperatingUnitPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TableLayoutPanel tableLayoutOperatingUnit;
            this.labelOperatingUnit = new System.Windows.Forms.Label();
            this.propertyGridOperatingUnit = new System.Windows.Forms.PropertyGrid();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.checkBoxAutoConvert = new System.Windows.Forms.CheckBox();
            tableLayoutOperatingUnit = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutOperatingUnit.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutOperatingUnit
            // 
            tableLayoutOperatingUnit.BackColor = System.Drawing.SystemColors.Control;
            tableLayoutOperatingUnit.ColumnCount = 3;
            tableLayoutOperatingUnit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutOperatingUnit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutOperatingUnit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutOperatingUnit.Controls.Add(this.labelOperatingUnit, 0, 0);
            tableLayoutOperatingUnit.Controls.Add(this.propertyGridOperatingUnit, 0, 1);
            tableLayoutOperatingUnit.Controls.Add(this.buttonUpdate, 0, 3);
            tableLayoutOperatingUnit.Controls.Add(this.buttonCancel, 1, 3);
            tableLayoutOperatingUnit.Controls.Add(this.buttonDelete, 2, 3);
            tableLayoutOperatingUnit.Controls.Add(this.checkBoxAutoConvert, 0, 2);
            tableLayoutOperatingUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutOperatingUnit.Location = new System.Drawing.Point(0, 0);
            tableLayoutOperatingUnit.Name = "tableLayoutOperatingUnit";
            tableLayoutOperatingUnit.RowCount = 4;
            tableLayoutOperatingUnit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            tableLayoutOperatingUnit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutOperatingUnit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutOperatingUnit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutOperatingUnit.Size = new System.Drawing.Size(355, 471);
            tableLayoutOperatingUnit.TabIndex = 0;
            tableLayoutOperatingUnit.Click += new System.EventHandler(this.tableLayoutOperatingUnit_Click);
            // 
            // labelOperatingUnit
            // 
            this.labelOperatingUnit.AutoSize = true;
            tableLayoutOperatingUnit.SetColumnSpan(this.labelOperatingUnit, 3);
            this.labelOperatingUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOperatingUnit.Location = new System.Drawing.Point(0, 0);
            this.labelOperatingUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelOperatingUnit.Name = "labelOperatingUnit";
            this.labelOperatingUnit.Size = new System.Drawing.Size(355, 18);
            this.labelOperatingUnit.TabIndex = 0;
            this.labelOperatingUnit.Text = "Operating Unit properties";
            this.labelOperatingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelOperatingUnit.Click += new System.EventHandler(this.labelOperatingUnit_Click);
            // 
            // propertyGridOperatingUnit
            // 
            tableLayoutOperatingUnit.SetColumnSpan(this.propertyGridOperatingUnit, 3);
            this.propertyGridOperatingUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridOperatingUnit.HelpBackColor = System.Drawing.SystemColors.Info;
            this.propertyGridOperatingUnit.Location = new System.Drawing.Point(3, 21);
            this.propertyGridOperatingUnit.Name = "propertyGridOperatingUnit";
            this.propertyGridOperatingUnit.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGridOperatingUnit.Size = new System.Drawing.Size(349, 391);
            this.propertyGridOperatingUnit.TabIndex = 1;
            this.propertyGridOperatingUnit.ToolbarVisible = false;
            this.propertyGridOperatingUnit.Click += new System.EventHandler(this.propertyGridOperatingUnit_Click);
            this.propertyGridOperatingUnit.Leave += new System.EventHandler(this.propertyGridOperatingUnit_Leave);
            this.propertyGridOperatingUnit.Enter += new System.EventHandler(this.propertyGridOperatingUnit_Enter);
            this.propertyGridOperatingUnit.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridOperatingUnit_PropertyValueChanged);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUpdate.Location = new System.Drawing.Point(3, 446);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(112, 22);
            this.buttonUpdate.TabIndex = 3;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(121, 446);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(112, 22);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDelete.Location = new System.Drawing.Point(239, 446);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(113, 22);
            this.buttonDelete.TabIndex = 5;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // checkBoxAutoConvert
            // 
            this.checkBoxAutoConvert.AutoSize = true;
            tableLayoutOperatingUnit.SetColumnSpan(this.checkBoxAutoConvert, 3);
            this.checkBoxAutoConvert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxAutoConvert.Location = new System.Drawing.Point(20, 418);
            this.checkBoxAutoConvert.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.checkBoxAutoConvert.Name = "checkBoxAutoConvert";
            this.checkBoxAutoConvert.Size = new System.Drawing.Size(332, 22);
            this.checkBoxAutoConvert.TabIndex = 2;
            this.checkBoxAutoConvert.Text = "Convert values automatically";
            this.checkBoxAutoConvert.UseVisualStyleBackColor = true;
            this.checkBoxAutoConvert.CheckedChanged += new System.EventHandler(this.checkBoxAutoConvert_CheckedChanged);
            // 
            // OperatingUnitPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(tableLayoutOperatingUnit);
            this.MinimumSize = new System.Drawing.Size(355, 348);
            this.Name = "OperatingUnitPanel";
            this.Size = new System.Drawing.Size(355, 471);
            this.Click += new System.EventHandler(this.OperatingUnitPanel_Click);
            tableLayoutOperatingUnit.ResumeLayout(false);
            tableLayoutOperatingUnit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label labelOperatingUnit;
        private System.Windows.Forms.PropertyGrid propertyGridOperatingUnit;
        private System.Windows.Forms.CheckBox checkBoxAutoConvert;

    }
}
