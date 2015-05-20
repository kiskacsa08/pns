﻿namespace PNSDraw
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Raw Materials");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Intermediates");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Products");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Materials", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Operating Units");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallPNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumPNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largePNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToSVGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToPNSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.copy_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paste_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicate_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.showGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.snapToGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.lockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solutionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadASolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.drawingmode_menuitem = new System.Windows.Forms.ToolStripDropDownButton();
            this.drawingmode_pointer = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingmode_link = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingmode_raw = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingmode_intermediate = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingmode_product = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingmode_operatingunit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.button_rawmaterial = new System.Windows.Forms.ToolStripButton();
            this.button_intermediatematerial = new System.Windows.Forms.ToolStripButton();
            this.button_productmaterial = new System.Windows.Forms.ToolStripButton();
            this.button_operatingunit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.button_zoomin = new System.Windows.Forms.ToolStripButton();
            this.button_zoomout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.button_snapobjects = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox_minimap = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox2 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeMaterials = new System.Windows.Forms.TreeView();
            this.treeOpUnits = new System.Windows.Forms.TreeView();
            this.pnsCanvas1 = new PNSDraw.Canvas.PNSCanvas();
            this.propertyGrid1 = new PNSDraw.MyPropertyGrid();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_minimap)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AllowMerge = false;
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.solutionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(2);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1312, 24);
            this.menuStrip1.Stretch = false;
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator3,
            this.exportToolStripMenuItem,
            this.exportToSVGToolStripMenuItem,
            this.exportToPNSToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(145, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smallPNGToolStripMenuItem,
            this.mediumPNGToolStripMenuItem,
            this.largePNGToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exportToolStripMenuItem.Text = "Export to PNG";
            this.exportToolStripMenuItem.DropDownOpened += new System.EventHandler(this.exportToolStripMenuItem_DropDownOpened);
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // smallPNGToolStripMenuItem
            // 
            this.smallPNGToolStripMenuItem.Name = "smallPNGToolStripMenuItem";
            this.smallPNGToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.smallPNGToolStripMenuItem.Text = "Small";
            this.smallPNGToolStripMenuItem.Click += new System.EventHandler(this.smallPNGToolStripMenuItem_Click);
            // 
            // mediumPNGToolStripMenuItem
            // 
            this.mediumPNGToolStripMenuItem.Name = "mediumPNGToolStripMenuItem";
            this.mediumPNGToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.mediumPNGToolStripMenuItem.Text = "Medium";
            this.mediumPNGToolStripMenuItem.Click += new System.EventHandler(this.mediumPNGToolStripMenuItem_Click);
            // 
            // largePNGToolStripMenuItem
            // 
            this.largePNGToolStripMenuItem.Name = "largePNGToolStripMenuItem";
            this.largePNGToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.largePNGToolStripMenuItem.Text = "Large";
            this.largePNGToolStripMenuItem.Click += new System.EventHandler(this.largePNGToolStripMenuItem_Click);
            // 
            // exportToSVGToolStripMenuItem
            // 
            this.exportToSVGToolStripMenuItem.Name = "exportToSVGToolStripMenuItem";
            this.exportToSVGToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exportToSVGToolStripMenuItem.Text = "Export to SVG";
            this.exportToSVGToolStripMenuItem.Click += new System.EventHandler(this.exportToSVGToolStripMenuItem_Click);
            // 
            // exportToPNSToolStripMenuItem
            // 
            this.exportToPNSToolStripMenuItem.Name = "exportToPNSToolStripMenuItem";
            this.exportToPNSToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exportToPNSToolStripMenuItem.Text = "Export to PNS";
            this.exportToPNSToolStripMenuItem.Click += new System.EventHandler(this.exportToPNSToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator11,
            this.copy_toolStripMenuItem,
            this.paste_toolStripMenuItem,
            this.duplicate_toolStripMenuItem,
            this.toolStripSeparator9,
            this.settingsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(121, 6);
            // 
            // copy_toolStripMenuItem
            // 
            this.copy_toolStripMenuItem.Enabled = false;
            this.copy_toolStripMenuItem.Name = "copy_toolStripMenuItem";
            this.copy_toolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.copy_toolStripMenuItem.Text = "Copy";
            this.copy_toolStripMenuItem.Click += new System.EventHandler(this.copy_toolStripMenuItem_Click);
            // 
            // paste_toolStripMenuItem
            // 
            this.paste_toolStripMenuItem.Enabled = false;
            this.paste_toolStripMenuItem.Name = "paste_toolStripMenuItem";
            this.paste_toolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.paste_toolStripMenuItem.Text = "Paste";
            this.paste_toolStripMenuItem.Click += new System.EventHandler(this.paste_toolStripMenuItem_Click);
            // 
            // duplicate_toolStripMenuItem
            // 
            this.duplicate_toolStripMenuItem.Enabled = false;
            this.duplicate_toolStripMenuItem.Name = "duplicate_toolStripMenuItem";
            this.duplicate_toolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.duplicate_toolStripMenuItem.Text = "Duplicate";
            this.duplicate_toolStripMenuItem.Click += new System.EventHandler(this.duplicate_toolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(121, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem,
            this.toolStripSeparator7,
            this.showGridToolStripMenuItem,
            this.snapToGridToolStripMenuItem,
            this.toolStripSeparator8,
            this.lockToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.zoomInToolStripMenuItem.Text = "Zoom In";
            this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.zoomOutToolStripMenuItem.Text = "Zoom Out";
            this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(136, 6);
            // 
            // showGridToolStripMenuItem
            // 
            this.showGridToolStripMenuItem.Checked = true;
            this.showGridToolStripMenuItem.CheckOnClick = true;
            this.showGridToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showGridToolStripMenuItem.Name = "showGridToolStripMenuItem";
            this.showGridToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.showGridToolStripMenuItem.Text = "Show Grid";
            this.showGridToolStripMenuItem.Click += new System.EventHandler(this.showGridToolStripMenuItem_Click);
            // 
            // snapToGridToolStripMenuItem
            // 
            this.snapToGridToolStripMenuItem.Checked = true;
            this.snapToGridToolStripMenuItem.CheckOnClick = true;
            this.snapToGridToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.snapToGridToolStripMenuItem.Name = "snapToGridToolStripMenuItem";
            this.snapToGridToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.snapToGridToolStripMenuItem.Text = "Snap to Grid";
            this.snapToGridToolStripMenuItem.Click += new System.EventHandler(this.snapToGridToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(136, 6);
            // 
            // lockToolStripMenuItem
            // 
            this.lockToolStripMenuItem.Name = "lockToolStripMenuItem";
            this.lockToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.lockToolStripMenuItem.Text = "Lock";
            this.lockToolStripMenuItem.Click += new System.EventHandler(this.lockToolStripMenuItem_Click);
            // 
            // solutionsToolStripMenuItem
            // 
            this.solutionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadASolutionToolStripMenuItem,
            this.settingsToolStripMenuItem1});
            this.solutionsToolStripMenuItem.Name = "solutionsToolStripMenuItem";
            this.solutionsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.solutionsToolStripMenuItem.Text = "Solutions";
            // 
            // loadASolutionToolStripMenuItem
            // 
            this.loadASolutionToolStripMenuItem.Name = "loadASolutionToolStripMenuItem";
            this.loadASolutionToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.loadASolutionToolStripMenuItem.Text = "Load a solution";
            this.loadASolutionToolStripMenuItem.Click += new System.EventHandler(this.loadASolutionToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem1
            // 
            this.settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            this.settingsToolStripMenuItem1.Size = new System.Drawing.Size(155, 22);
            this.settingsToolStripMenuItem1.Text = "Settings";
            this.settingsToolStripMenuItem1.Click += new System.EventHandler(this.settingsToolStripMenuItem1_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.aboutToolStripMenuItem.Text = "About PNS Draw";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawingmode_menuitem,
            this.toolStripSeparator4,
            this.button_rawmaterial,
            this.button_intermediatematerial,
            this.button_productmaterial,
            this.button_operatingunit,
            this.toolStripSeparator5,
            this.button_zoomin,
            this.button_zoomout,
            this.toolStripSeparator6,
            this.button_snapobjects,
            this.toolStripSeparator10,
            this.toolStripComboBox1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 37);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(932, 37);
            this.toolStrip1.TabIndex = 2;
            // 
            // drawingmode_menuitem
            // 
            this.drawingmode_menuitem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.drawingmode_menuitem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawingmode_pointer,
            this.drawingmode_link,
            this.drawingmode_raw,
            this.drawingmode_intermediate,
            this.drawingmode_product,
            this.drawingmode_operatingunit});
            this.drawingmode_menuitem.Image = ((System.Drawing.Image)(resources.GetObject("drawingmode_menuitem.Image")));
            this.drawingmode_menuitem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.drawingmode_menuitem.Name = "drawingmode_menuitem";
            this.drawingmode_menuitem.Size = new System.Drawing.Size(41, 34);
            this.drawingmode_menuitem.Text = "Drawing Mode";
            // 
            // drawingmode_pointer
            // 
            this.drawingmode_pointer.Image = ((System.Drawing.Image)(resources.GetObject("drawingmode_pointer.Image")));
            this.drawingmode_pointer.Name = "drawingmode_pointer";
            this.drawingmode_pointer.Size = new System.Drawing.Size(187, 22);
            this.drawingmode_pointer.Text = "Pointer";
            this.drawingmode_pointer.Click += new System.EventHandler(this.pointerToolStripMenuItem_Click);
            // 
            // drawingmode_link
            // 
            this.drawingmode_link.Image = ((System.Drawing.Image)(resources.GetObject("drawingmode_link.Image")));
            this.drawingmode_link.Name = "drawingmode_link";
            this.drawingmode_link.Size = new System.Drawing.Size(187, 22);
            this.drawingmode_link.Text = "Link";
            this.drawingmode_link.Click += new System.EventHandler(this.drawingmode_link_Click);
            // 
            // drawingmode_raw
            // 
            this.drawingmode_raw.Image = ((System.Drawing.Image)(resources.GetObject("drawingmode_raw.Image")));
            this.drawingmode_raw.Name = "drawingmode_raw";
            this.drawingmode_raw.Size = new System.Drawing.Size(187, 22);
            this.drawingmode_raw.Text = "Raw Material";
            this.drawingmode_raw.Click += new System.EventHandler(this.drawingmode_raw_Click);
            // 
            // drawingmode_intermediate
            // 
            this.drawingmode_intermediate.Image = ((System.Drawing.Image)(resources.GetObject("drawingmode_intermediate.Image")));
            this.drawingmode_intermediate.Name = "drawingmode_intermediate";
            this.drawingmode_intermediate.Size = new System.Drawing.Size(187, 22);
            this.drawingmode_intermediate.Text = "Intermediate Material";
            this.drawingmode_intermediate.Click += new System.EventHandler(this.drawingmode_intermediate_Click);
            // 
            // drawingmode_product
            // 
            this.drawingmode_product.Image = ((System.Drawing.Image)(resources.GetObject("drawingmode_product.Image")));
            this.drawingmode_product.Name = "drawingmode_product";
            this.drawingmode_product.Size = new System.Drawing.Size(187, 22);
            this.drawingmode_product.Text = "Product";
            this.drawingmode_product.Click += new System.EventHandler(this.drawingmode_product_Click);
            // 
            // drawingmode_operatingunit
            // 
            this.drawingmode_operatingunit.Image = ((System.Drawing.Image)(resources.GetObject("drawingmode_operatingunit.Image")));
            this.drawingmode_operatingunit.Name = "drawingmode_operatingunit";
            this.drawingmode_operatingunit.Size = new System.Drawing.Size(187, 22);
            this.drawingmode_operatingunit.Text = "OperatingUnit";
            this.drawingmode_operatingunit.Click += new System.EventHandler(this.drawingmode_operatingunit_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 37);
            // 
            // button_rawmaterial
            // 
            this.button_rawmaterial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_rawmaterial.Image = ((System.Drawing.Image)(resources.GetObject("button_rawmaterial.Image")));
            this.button_rawmaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_rawmaterial.Margin = new System.Windows.Forms.Padding(2);
            this.button_rawmaterial.Name = "button_rawmaterial";
            this.button_rawmaterial.Padding = new System.Windows.Forms.Padding(2);
            this.button_rawmaterial.Size = new System.Drawing.Size(36, 33);
            this.button_rawmaterial.Text = "Drag and Drop Raw Material";
            this.button_rawmaterial.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_rawmaterial_MouseDown);
            // 
            // button_intermediatematerial
            // 
            this.button_intermediatematerial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_intermediatematerial.Image = ((System.Drawing.Image)(resources.GetObject("button_intermediatematerial.Image")));
            this.button_intermediatematerial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_intermediatematerial.Margin = new System.Windows.Forms.Padding(2);
            this.button_intermediatematerial.Name = "button_intermediatematerial";
            this.button_intermediatematerial.Padding = new System.Windows.Forms.Padding(2);
            this.button_intermediatematerial.Size = new System.Drawing.Size(36, 33);
            this.button_intermediatematerial.Text = "Drag and Drop Intermediate Material";
            this.button_intermediatematerial.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_intermediatematerial_MouseDown);
            // 
            // button_productmaterial
            // 
            this.button_productmaterial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_productmaterial.Image = ((System.Drawing.Image)(resources.GetObject("button_productmaterial.Image")));
            this.button_productmaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_productmaterial.Margin = new System.Windows.Forms.Padding(2);
            this.button_productmaterial.Name = "button_productmaterial";
            this.button_productmaterial.Padding = new System.Windows.Forms.Padding(2);
            this.button_productmaterial.Size = new System.Drawing.Size(36, 33);
            this.button_productmaterial.Text = "Drag and Drop Product Material";
            this.button_productmaterial.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_productmaterial_MouseDown);
            // 
            // button_operatingunit
            // 
            this.button_operatingunit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_operatingunit.Image = ((System.Drawing.Image)(resources.GetObject("button_operatingunit.Image")));
            this.button_operatingunit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_operatingunit.Margin = new System.Windows.Forms.Padding(2);
            this.button_operatingunit.Name = "button_operatingunit";
            this.button_operatingunit.Padding = new System.Windows.Forms.Padding(2);
            this.button_operatingunit.Size = new System.Drawing.Size(36, 33);
            this.button_operatingunit.Text = "Drag and Drop Operating Unit";
            this.button_operatingunit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_operatingunit_MouseDown);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 37);
            // 
            // button_zoomin
            // 
            this.button_zoomin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_zoomin.Image = ((System.Drawing.Image)(resources.GetObject("button_zoomin.Image")));
            this.button_zoomin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_zoomin.Margin = new System.Windows.Forms.Padding(2);
            this.button_zoomin.Name = "button_zoomin";
            this.button_zoomin.Size = new System.Drawing.Size(32, 33);
            this.button_zoomin.Text = "Zoom In";
            this.button_zoomin.Click += new System.EventHandler(this.button_zoomin_Click);
            // 
            // button_zoomout
            // 
            this.button_zoomout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_zoomout.Image = ((System.Drawing.Image)(resources.GetObject("button_zoomout.Image")));
            this.button_zoomout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_zoomout.Margin = new System.Windows.Forms.Padding(2);
            this.button_zoomout.Name = "button_zoomout";
            this.button_zoomout.Size = new System.Drawing.Size(32, 33);
            this.button_zoomout.Text = "Zoom Out";
            this.button_zoomout.Click += new System.EventHandler(this.button_zoomout_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 37);
            // 
            // button_snapobjects
            // 
            this.button_snapobjects.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_snapobjects.Image = ((System.Drawing.Image)(resources.GetObject("button_snapobjects.Image")));
            this.button_snapobjects.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_snapobjects.Margin = new System.Windows.Forms.Padding(2);
            this.button_snapobjects.Name = "button_snapobjects";
            this.button_snapobjects.Size = new System.Drawing.Size(32, 33);
            this.button_snapobjects.Text = "Snap Selected Objects to Grid";
            this.button_snapobjects.Click += new System.EventHandler(this.button_snapobjects_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 37);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(160, 33);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.pnsCanvas1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(374, 24);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(938, 603);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox_minimap);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 382);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 189);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Quick View";
            // 
            // pictureBox_minimap
            // 
            this.pictureBox_minimap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_minimap.Location = new System.Drawing.Point(3, 16);
            this.pictureBox_minimap.Name = "pictureBox_minimap";
            this.pictureBox_minimap.Size = new System.Drawing.Size(354, 170);
            this.pictureBox_minimap.TabIndex = 0;
            this.pictureBox_minimap.TabStop = false;
            this.pictureBox_minimap.SizeChanged += new System.EventHandler(this.pictureBox_minimap_SizeChanged);
            this.pictureBox_minimap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_minimap_MouseDown);
            this.pictureBox_minimap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_minimap_MouseMove);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.propertyGrid1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 571);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Object Properties";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(360, 571);
            this.panel1.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(374, 603);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(366, 577);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Object Properties";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(366, 577);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "TreeView";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(366, 577);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Solutions";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator12,
            this.toolStripLabel1,
            this.toolStripComboBox2,
            this.toolStripSeparator13,
            this.toolStripLabel2,
            this.toolStripTextBox1,
            this.toolStripSeparator14,
            this.toolStripLabel3,
            this.toolStripTextBox2,
            this.toolStripButton2});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(932, 37);
            this.toolStrip2.TabIndex = 6;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Checked = true;
            this.toolStripButton1.CheckOnClick = true;
            this.toolStripButton1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(100, 34);
            this.toolStripButton1.Text = "Use online solver";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 37);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(61, 34);
            this.toolStripLabel1.Text = "Algorithm";
            // 
            // toolStripComboBox2
            // 
            this.toolStripComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox2.Items.AddRange(new object[] {
            "ABB",
            "SSG"});
            this.toolStripComboBox2.Name = "toolStripComboBox2";
            this.toolStripComboBox2.Size = new System.Drawing.Size(121, 37);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 37);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(119, 34);
            this.toolStripLabel2.Text = "Number of processes";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 37);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 37);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(78, 34);
            this.toolStripLabel3.Text = "Solution limit";
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(100, 37);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(103, 34);
            this.toolStripButton2.Text = "Solve problem";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(932, 74);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeMaterials);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.treeOpUnits);
            this.splitContainer1.Size = new System.Drawing.Size(360, 571);
            this.splitContainer1.SplitterDistance = 166;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeMaterials
            // 
            this.treeMaterials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeMaterials.Location = new System.Drawing.Point(0, 0);
            this.treeMaterials.Name = "treeMaterials";
            treeNode1.Name = "raw_materials";
            treeNode1.Text = "Raw Materials";
            treeNode2.Name = "intermediates";
            treeNode2.Text = "Intermediates";
            treeNode3.Name = "products";
            treeNode3.Text = "Products";
            treeNode4.Name = "materials";
            treeNode4.Text = "Materials";
            this.treeMaterials.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
            this.treeMaterials.Size = new System.Drawing.Size(166, 571);
            this.treeMaterials.TabIndex = 0;
            this.treeMaterials.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeMaterials_NodeMouseClick);
            // 
            // treeOpUnits
            // 
            this.treeOpUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeOpUnits.Location = new System.Drawing.Point(0, 0);
            this.treeOpUnits.Name = "treeOpUnits";
            treeNode5.Name = "op_units";
            treeNode5.Text = "Operating Units";
            this.treeOpUnits.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.treeOpUnits.Size = new System.Drawing.Size(190, 571);
            this.treeOpUnits.TabIndex = 0;
            this.treeOpUnits.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeMaterials_NodeMouseClick);
            // 
            // pnsCanvas1
            // 
            this.pnsCanvas1.AddObjectMode = false;
            this.pnsCanvas1.AllowDrop = true;
            this.pnsCanvas1.BackColor = System.Drawing.Color.White;
            this.pnsCanvas1.ConnectorMode = false;
            this.pnsCanvas1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnsCanvas1.GridSize = 300;
            this.pnsCanvas1.Location = new System.Drawing.Point(3, 83);
            this.pnsCanvas1.Name = "pnsCanvas1";
            this.pnsCanvas1.ShowGrid = true;
            this.pnsCanvas1.Size = new System.Drawing.Size(932, 492);
            this.pnsCanvas1.SnapToGrid = true;
            this.pnsCanvas1.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 16);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.ReadOnly = false;
            this.propertyGrid1.Size = new System.Drawing.Size(354, 552);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 627);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "Form1";
            this.Text = "PNS Draw";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_minimap)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem snapToGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToSVGToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exportToPNSToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStripButton button_rawmaterial;
        private System.Windows.Forms.ToolStripButton button_intermediatematerial;
        private System.Windows.Forms.ToolStripButton button_productmaterial;
        private System.Windows.Forms.ToolStripButton button_operatingunit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripDropDownButton drawingmode_menuitem;
        private System.Windows.Forms.ToolStripMenuItem drawingmode_pointer;
        private System.Windows.Forms.ToolStripMenuItem drawingmode_link;
        private System.Windows.Forms.ToolStripMenuItem drawingmode_raw;
        private System.Windows.Forms.ToolStripMenuItem drawingmode_intermediate;
        private System.Windows.Forms.ToolStripMenuItem drawingmode_product;
        private System.Windows.Forms.ToolStripMenuItem drawingmode_operatingunit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton button_zoomin;
        private System.Windows.Forms.ToolStripButton button_zoomout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton button_snapobjects;
        private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem lockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallPNGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediumPNGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem largePNGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solutionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadASolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem copy_toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem paste_toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicate_toolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox_minimap;
        private System.Windows.Forms.GroupBox groupBox1;
        private MyPropertyGrid propertyGrid1;
        private System.Windows.Forms.TabPage tabPage2;
        private Canvas.PNSCanvas pnsCanvas1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeMaterials;
        private System.Windows.Forms.TreeView treeOpUnits;

    }
}
