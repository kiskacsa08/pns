namespace Pns.Dialogs
{
    partial class DefaultValue
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
            this.tableLayoutPanelDefaultValue = new System.Windows.Forms.TableLayoutPanel();
            this.propertyGridDefaultValues = new System.Windows.Forms.PropertyGrid();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDefaults = new System.Windows.Forms.Button();
            this.tableLayoutPanelDefaultValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelDefaultValue
            // 
            this.tableLayoutPanelDefaultValue.ColumnCount = 3;
            this.tableLayoutPanelDefaultValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelDefaultValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelDefaultValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelDefaultValue.Controls.Add(this.propertyGridDefaultValues, 0, 0);
            this.tableLayoutPanelDefaultValue.Controls.Add(this.buttonUpdate, 0, 1);
            this.tableLayoutPanelDefaultValue.Controls.Add(this.buttonCancel, 1, 1);
            this.tableLayoutPanelDefaultValue.Controls.Add(this.buttonDefaults, 2, 1);
            this.tableLayoutPanelDefaultValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDefaultValue.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDefaultValue.Name = "tableLayoutPanelDefaultValue";
            this.tableLayoutPanelDefaultValue.RowCount = 2;
            this.tableLayoutPanelDefaultValue.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDefaultValue.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelDefaultValue.Size = new System.Drawing.Size(503, 396);
            this.tableLayoutPanelDefaultValue.TabIndex = 0;
            // 
            // propertyGridDefaultValues
            // 
            this.tableLayoutPanelDefaultValue.SetColumnSpan(this.propertyGridDefaultValues, 3);
            this.propertyGridDefaultValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridDefaultValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGridDefaultValues.Location = new System.Drawing.Point(3, 3);
            this.propertyGridDefaultValues.Name = "propertyGridDefaultValues";
            this.propertyGridDefaultValues.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGridDefaultValues.Size = new System.Drawing.Size(497, 362);
            this.propertyGridDefaultValues.TabIndex = 0;
            this.propertyGridDefaultValues.ToolbarVisible = false;
            this.propertyGridDefaultValues.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridDefaultValues_PropertyValueChanged);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUpdate.Location = new System.Drawing.Point(3, 371);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(161, 22);
            this.buttonUpdate.TabIndex = 1;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(170, 371);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(161, 22);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDefaults
            // 
            this.buttonDefaults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDefaults.Location = new System.Drawing.Point(337, 371);
            this.buttonDefaults.Name = "buttonDefaults";
            this.buttonDefaults.Size = new System.Drawing.Size(163, 22);
            this.buttonDefaults.TabIndex = 3;
            this.buttonDefaults.Text = "Reload defaults";
            this.buttonDefaults.UseVisualStyleBackColor = true;
            this.buttonDefaults.Click += new System.EventHandler(this.buttonDefaults_Click);
            // 
            // DefaultValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 396);
            this.Controls.Add(this.tableLayoutPanelDefaultValue);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefaultValue";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Default Values";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DefaultValue_FormClosing);
            this.tableLayoutPanelDefaultValue.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDefaultValue;
        private System.Windows.Forms.PropertyGrid propertyGridDefaultValues;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDefaults;

    }
}