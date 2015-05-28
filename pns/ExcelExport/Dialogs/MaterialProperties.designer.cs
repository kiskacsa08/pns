namespace Pns.Dialogs
{
    partial class MaterialPanel
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
            this.tableLayoutPanelMaterial = new System.Windows.Forms.TableLayoutPanel();
            this.labelMaterial = new System.Windows.Forms.Label();
            this.propertyGridMaterial = new System.Windows.Forms.PropertyGrid();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.checkBoxAutoConvert = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanelMaterial.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMaterial
            // 
            this.tableLayoutPanelMaterial.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanelMaterial.ColumnCount = 3;
            this.tableLayoutPanelMaterial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelMaterial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelMaterial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelMaterial.Controls.Add(this.labelMaterial, 0, 0);
            this.tableLayoutPanelMaterial.Controls.Add(this.propertyGridMaterial, 0, 1);
            this.tableLayoutPanelMaterial.Controls.Add(this.buttonUpdate, 0, 3);
            this.tableLayoutPanelMaterial.Controls.Add(this.buttonCancel, 1, 3);
            this.tableLayoutPanelMaterial.Controls.Add(this.buttonDelete, 2, 3);
            this.tableLayoutPanelMaterial.Controls.Add(this.checkBoxAutoConvert, 0, 2);
            this.tableLayoutPanelMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMaterial.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMaterial.Name = "tableLayoutPanelMaterial";
            this.tableLayoutPanelMaterial.RowCount = 4;
            this.tableLayoutPanelMaterial.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanelMaterial.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMaterial.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelMaterial.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelMaterial.Size = new System.Drawing.Size(305, 319);
            this.tableLayoutPanelMaterial.TabIndex = 0;
            this.tableLayoutPanelMaterial.Click += new System.EventHandler(this.tableLayoutPanelMaterial_Click);
            // 
            // labelMaterial
            // 
            this.labelMaterial.AutoSize = true;
            this.labelMaterial.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelMaterial.SetColumnSpan(this.labelMaterial, 3);
            this.labelMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaterial.Location = new System.Drawing.Point(0, 0);
            this.labelMaterial.Margin = new System.Windows.Forms.Padding(0);
            this.labelMaterial.Name = "labelMaterial";
            this.labelMaterial.Size = new System.Drawing.Size(305, 18);
            this.labelMaterial.TabIndex = 0;
            this.labelMaterial.Text = "Material properties";
            this.labelMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMaterial.Click += new System.EventHandler(this.labelMaterial_Click);
            // 
            // propertyGridMaterial
            // 
            this.propertyGridMaterial.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanelMaterial.SetColumnSpan(this.propertyGridMaterial, 3);
            this.propertyGridMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridMaterial.HelpBackColor = System.Drawing.SystemColors.Info;
            this.propertyGridMaterial.Location = new System.Drawing.Point(3, 21);
            this.propertyGridMaterial.Name = "propertyGridMaterial";
            this.propertyGridMaterial.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGridMaterial.Size = new System.Drawing.Size(299, 239);
            this.propertyGridMaterial.TabIndex = 1;
            this.propertyGridMaterial.ToolbarVisible = false;
            this.propertyGridMaterial.Click += new System.EventHandler(this.propertyGridMaterial_Click);
            this.propertyGridMaterial.Leave += new System.EventHandler(this.propertyGridMaterial_Leave);
            this.propertyGridMaterial.Enter += new System.EventHandler(this.propertyGridMaterial_Enter);
            this.propertyGridMaterial.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridMaterial_PropertyValueChanged);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUpdate.Location = new System.Drawing.Point(3, 294);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(95, 22);
            this.buttonUpdate.TabIndex = 3;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(104, 294);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(95, 22);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDelete.Location = new System.Drawing.Point(205, 294);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(97, 22);
            this.buttonDelete.TabIndex = 5;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // checkBoxAutoConvert
            // 
            this.checkBoxAutoConvert.AutoSize = true;
            this.tableLayoutPanelMaterial.SetColumnSpan(this.checkBoxAutoConvert, 3);
            this.checkBoxAutoConvert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxAutoConvert.Location = new System.Drawing.Point(20, 266);
            this.checkBoxAutoConvert.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.checkBoxAutoConvert.Name = "checkBoxAutoConvert";
            this.checkBoxAutoConvert.Size = new System.Drawing.Size(282, 22);
            this.checkBoxAutoConvert.TabIndex = 2;
            this.checkBoxAutoConvert.Text = "Convert values automatically";
            this.checkBoxAutoConvert.UseVisualStyleBackColor = true;
            this.checkBoxAutoConvert.CheckedChanged += new System.EventHandler(this.checkBoxAutoConvert_CheckedChanged);
            // 
            // MaterialPanel
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanelMaterial);
            this.MinimumSize = new System.Drawing.Size(305, 276);
            this.Name = "MaterialPanel";
            this.Size = new System.Drawing.Size(305, 319);
            this.Click += new System.EventHandler(this.MaterialPanel_Click);
            this.tableLayoutPanelMaterial.ResumeLayout(false);
            this.tableLayoutPanelMaterial.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMaterial;
        private System.Windows.Forms.Label labelMaterial;
        private System.Windows.Forms.PropertyGrid propertyGridMaterial;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox checkBoxAutoConvert;
    }
}
