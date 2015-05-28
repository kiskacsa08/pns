namespace Pns.Dialogs
{
    partial class DefaultMU
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
            System.Windows.Forms.TableLayoutPanel tableLayoutPanelDefaultMU;
            this.propertyGridDefaultMU = new System.Windows.Forms.PropertyGrid();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDefault = new System.Windows.Forms.Button();
            tableLayoutPanelDefaultMU = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanelDefaultMU.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelDefaultMU
            // 
            tableLayoutPanelDefaultMU.ColumnCount = 3;
            tableLayoutPanelDefaultMU.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanelDefaultMU.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanelDefaultMU.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanelDefaultMU.Controls.Add(this.propertyGridDefaultMU, 0, 0);
            tableLayoutPanelDefaultMU.Controls.Add(this.buttonUpdate, 0, 1);
            tableLayoutPanelDefaultMU.Controls.Add(this.buttonCancel, 1, 1);
            tableLayoutPanelDefaultMU.Controls.Add(this.buttonDefault, 2, 1);
            tableLayoutPanelDefaultMU.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelDefaultMU.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelDefaultMU.Name = "tableLayoutPanelDefaultMU";
            tableLayoutPanelDefaultMU.RowCount = 2;
            tableLayoutPanelDefaultMU.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelDefaultMU.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanelDefaultMU.Size = new System.Drawing.Size(436, 526);
            tableLayoutPanelDefaultMU.TabIndex = 0;
            // 
            // propertyGridDefaultMU
            // 
            tableLayoutPanelDefaultMU.SetColumnSpan(this.propertyGridDefaultMU, 3);
            this.propertyGridDefaultMU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridDefaultMU.HelpBackColor = System.Drawing.SystemColors.Info;
            this.propertyGridDefaultMU.Location = new System.Drawing.Point(3, 3);
            this.propertyGridDefaultMU.Name = "propertyGridDefaultMU";
            this.propertyGridDefaultMU.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGridDefaultMU.Size = new System.Drawing.Size(430, 492);
            this.propertyGridDefaultMU.TabIndex = 0;
            this.propertyGridDefaultMU.ToolbarVisible = false;
            this.propertyGridDefaultMU.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridDefaultMU_PropertyValueChanged);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUpdate.Location = new System.Drawing.Point(3, 501);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(139, 22);
            this.buttonUpdate.TabIndex = 1;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(148, 501);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(139, 22);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDefault
            // 
            this.buttonDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDefault.Location = new System.Drawing.Point(293, 501);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(140, 22);
            this.buttonDefault.TabIndex = 3;
            this.buttonDefault.Text = "Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // DefaultMU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 526);
            this.Controls.Add(tableLayoutPanelDefaultMU);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefaultMU";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Default Measurement Units";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DefaultMU_FormClosing);
            tableLayoutPanelDefaultMU.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.PropertyGrid propertyGridDefaultMU;
        internal System.Windows.Forms.Button buttonUpdate;
        internal System.Windows.Forms.Button buttonCancel;
        internal System.Windows.Forms.Button buttonDefault;

    }
}