/* Copyright 2015 Department of Computer Science and Systems Technology, University of Pannonia

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Globalization;
using PNSDraw.online;
using System.Diagnostics;
using PNSDraw.Excel_export;
using PNSDraw.ZIMPL_export;
using System.Drawing.Printing;
using PNSDraw.Dialogs;

using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;

using PNSDraw.Configuration;
using System.Deployment.Application;

namespace PNSDraw
{
    public partial class Form1 : Form
    {
        #region Enums
        enum CanvasMode { Pointer, Link, Raw, Intermediate, Product, OperatingUnit }

        enum ObjectType { Raw, Intermediate, Product, OperatingUnit }
        public enum ExcelExportType { graph_export, brief_export, brief_view, detailed_export, detailed_view, export_summary_of_results, view_summary_of_results }

        public enum ExportExtensions { JPG, PNG, SVG, XLS, ZIMPL }
        
        #endregion

        #region Private variables
        string OpenedFilename = "";

        PGraph Graph;

        string LastGraphXML = "";

        string LockedGraphXML = "";

        bool solutionmode = false;
        bool printviewmode = false;

        ContextMenuStrip contMenu;

        PleaseWaitDialog pwd;
        string algorithm;
        bool isProblemExists;
        string inPath;
        string outPath;

        Solver solver;

        bool LockedMode
        {
            get
            {
                return solutionmode || printviewmode;
            }
        }

        string CurrentFile = "";

        CanvasMode canvasstate = CanvasMode.Pointer;
        #endregion

        #region Constructors
        public Form1(string openedFilename)
        {
            OpenedFilename = openedFilename;
            InitializeComponent();
            Graph = new PGraph();
            pnsCanvas1.GraphicsStructure = Graph;
            isProblemExists = false;
            //UpdateListOfMUs();
            UpdateListOfQuantities();
            UpdateListOfPriceMUs();
        }
        #endregion

        #region Form load and close methods
        private void Form1_Load(object sender, EventArgs e)
        {
            pnsCanvas1.Objects = Graph.GetObjectList();
            pnsCanvas1.NewItem += new Canvas.CanvasEventHandler(pnsCanvas1_NewItem);
            pnsCanvas1.ClickItem += new Canvas.CanvasEventHandler(pnsCanvas1_ClickItem);
            pnsCanvas1.NewConnector += new Canvas.CanvasEventHandler(pnsCanvas1_NewConnector);
            pnsCanvas1.RemoveObjects += new Canvas.CanvasEventHandler(pnsCanvas1_RemoveObjects);
            pnsCanvas1.SelectionChanged += new Canvas.CanvasEventHandler(pnsCanvas1_SelectionChanged);
            pnsCanvas1.DataChanged += new Canvas.CanvasEventHandler(pnsCanvas1_DataChanged);
            pnsCanvas1.EditItem += new Canvas.CanvasEventHandler(pnsCanvas1_EditItem);
            pnsCanvas1.ViewChanged += new Canvas.CanvasEventHandler(pnsCanvas1_ViewChanged);
            pnsCanvas1.Copy += new Canvas.CanvasEventHandler(pnsCanvas1_Copy);
            pnsCanvas1.Paste += new Canvas.CanvasEventHandler(pnsCanvas1_Paste);
            pnsCanvas1.Duplicate += new Canvas.CanvasEventHandler(pnsCanvas1_Duplicate);
            leftToolStripMenuItem.Checked = true;

            CreateUndo();
            LastGraphXML = Graph.ExportToXML();
            CurrentFile = "";
            UpdateTitle();
            ResetViewList();
            toolStripComboBox2.SelectedIndex = 0;
            RefreshTreeViews();

            string tempfolder = System.IO.Path.GetTempPath();

            Config.Instance.Load(tempfolder + "/quicksave_config.xml");
            if (Config.Instance.Login.IsLoggedIn)
            {
                SignInButton.Text = Config.Instance.Login.Username + " / Logout";
            }


            if (OpenedFilename == "")
            {
                OnLoad(tempfolder + "/quicksave_graph.xml", false);
            }
            else
            {
                OnLoad(OpenedFilename, true);
            }
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;

            Utils.Instance.LoginChanged += new EventHandler(SignInButton_Click);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool process = true;
            if (CheckModification())
            {                
                DialogResult dr = MessageBox.Show(
                    "The graph has been changed. Do you want to save the changes?",
                    "Exit",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                    );

                switch (dr)
                {
                    case DialogResult.Yes:
                        OnSave();
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        process = false;
                        break;
                }
            }

            if (process)
            {
                string tempfolder = System.IO.Path.GetTempPath();
                Config.Instance.Save(tempfolder + "/quicksave_config.xml");
                CurrentFile = tempfolder + "/quicksave_graph.xml";
                OnSave();
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

        private void OnLoad(string filename, bool isOFD = true)
        {
            try
            {
                FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file, Encoding.UTF8);

                string xml = sr.ReadToEnd();
                Graph.Reset();
                pnsCanvas1.Reset();
                pnsCanvas1.GridSize = Config.Instance.GraphSettings.GridSize;

                string extension = Path.GetExtension(file.Name);

                if (extension.Equals(".xml") || extension.Equals(".pgsx"))
                {
                    Graph.ImportFromXML(xml);
                }
                else if (extension.Equals(".pns"))
                {
                    Graph.ImportFromPNS(xml);
                    Graph.GenerateLayout();
                }

                RefreshMinimap(); // Azert kell, mert a TextObjectek rajzolaskor szamoljak ki a meretuket
                ZoomToFit();
                pnsCanvas1.Refresh();
                LastGraphXML = Graph.ExportToXML();
                if (isOFD)
                {
                    CurrentFile = filename;
                }                
                CreateUndo();
                UpdateTitle();

                UpdateSolutionsTab();
                tabControl1.SelectedTab = tabPage1;
                ResetViewList();
                RefreshTreeViews();
                sr.Close();
                file.Close();
                isProblemExists = true;
            }
            catch (Exception ex)
            {
                if (isOFD)
                {
                    MessageBox.Show(ex.ToString());
                }
                else
                {
                    Console.WriteLine(ex.ToString());
                }                
            }
        }

        private void ResetViewList()
        {
            printviewmode = false;
            solutionmode = false;
            UpdateViewList();
        }

        private void DoUndo()
        {
            if (solutionmode)
            {
                return;
            }

            redoToolStripMenuItem.Enabled = true;
            if (Graph.DoUndo() < 2)
            {
                undoToolStripMenuItem.Enabled = false;
            }
            pnsCanvas1.GridSize = Config.Instance.GraphSettings.GridSize;
            pnsCanvas1.Refresh();
            UpdateTitle();
            RefreshTreeViews();
            RefreshMinimap();
            propertyGrid1.SelectedObject = null;
        }

        private void DoRedo()
        {
            if (solutionmode)
            {
                return;
            }
            undoToolStripMenuItem.Enabled = true;
            if (Graph.DoRedo() < 1)
            {
                redoToolStripMenuItem.Enabled = false;
            }
            pnsCanvas1.GridSize = Config.Instance.GraphSettings.GridSize;
            pnsCanvas1.Refresh();
            UpdateTitle();
            RefreshTreeViews();
            RefreshMinimap();
            propertyGrid1.SelectedObject = null;
        }

        private void CreateUndo()
        {
            Graph.CreateUndo();
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = false;
            UpdateTitle();
            RefreshMinimap();
        }

        private bool CheckModification()
        {
            string currentxml = Graph.ExportToXML();
            if (currentxml != LastGraphXML)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ZoomToFit()
        {   
            pnsCanvas1.ZoomToFit();
        }

        private void OnSave()
        {
            if (CurrentFile == "")
            {
                OnSaveAs();
                return;
            }
            try
            {
                FileStream file = new FileStream(CurrentFile, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(file);
                string xml = "";
                if (LockedMode)
                {
                    xml = LockedGraphXML;
                }
                else
                {
                    xml = Graph.ExportToXML();
                }
                sw.Write(xml);
                sw.Close();
                file.Close();
                LastGraphXML = xml;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            UpdateTitle();
        }

        private void OnSaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "pgsx file|*.pgsx|xml file|*.xml";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(file);
                    string xml = "";
                    if (LockedMode)
                    {
                        xml = LockedGraphXML;
                    }
                    else
                    {
                        xml = Graph.ExportToXML();
                    }
                    sw.Write(xml);
                    sw.Close();
                    file.Close();
                    LastGraphXML = xml;

                    CurrentFile = sfd.FileName;
                    UpdateTitle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            string name = "untitled";
            if (CurrentFile != "")
            {
                name = Path.GetFileName(CurrentFile);
            }
            if (CheckModification())
            {
                name = "*" + name;
            }
            string online;
            if (Config.Instance.SolverSettings.IsOnlineSolver)
            {
                online = "Online";
            }
            else
            {
                online = "Offline";
            }

            Text = "P-Graph Studio (" + name + ")" + " Solver: " + online;
        }

        private void RefreshTreeViews()
        {
            treeMaterials.BeginUpdate();
            treeOpUnits.BeginUpdate();

            treeMaterials.Nodes.Clear();
            treeOpUnits.Nodes.Clear();

            treeMaterials.Nodes.Add("Materials");
            treeMaterials.Nodes[0].Nodes.Add("Raw Materials");
            treeMaterials.Nodes[0].Nodes.Add("Intermediates");
            treeMaterials.Nodes[0].Nodes.Add("Products");
            treeOpUnits.Nodes.Add("Operating Units");
            contMenu = new ContextMenuStrip();
            ToolStripMenuItem rename = new ToolStripMenuItem("Rename");
            rename.Click += rename_Click;
            contMenu.Items.Add(rename);

            foreach (Material mat in Graph.Materials)
            {
                TreeNode tn = new TreeNode();
                tn.ContextMenuStrip = contMenu;

                tn.Name = mat.Name;
                tn.Text = mat.Name;
                tn.Nodes.Add("Price: " + mat.PriceProp.Value + " " + mat.PriceProp.MU);
                tn.Nodes.Add("Req. flow: " + mat.ReqFlowProp.Value + " " + mat.ReqFlowProp.MU);
                tn.Nodes.Add("Max. flow: " + mat.MaxFlowProp.Value + " " + mat.MaxFlowProp.MU);

                switch (mat.Type)
                {
                    case MaterialTypes.Raw:
                        treeMaterials.Nodes[0].Nodes[0].Nodes.Add(tn);
                        break;
                    case MaterialTypes.Intermediate:
                        treeMaterials.Nodes[0].Nodes[1].Nodes.Add(tn);
                        break;
                    case MaterialTypes.Product:
                        treeMaterials.Nodes[0].Nodes[2].Nodes.Add(tn);
                        break;
                    default:
                        break;
                }
            }

            int i = 0;
            foreach (OperatingUnit ou in Graph.OperatingUnits)
            {
                TreeNode tn2 = new TreeNode();
                tn2.ContextMenuStrip = contMenu;
                tn2.Name = ou.Name;
                tn2.Text = ou.Name;

                tn2.Nodes.Add("Cap. lower bound: " + ou.CapacityLowerProp.Value + " " + ou.CapacityLowerProp.MU);
                tn2.Nodes.Add("Cap. upper bound: " + ou.CapacityUpperProp.Value + " " + ou.CapacityUpperProp.MU);
                tn2.Nodes.Add("Inv. cost - fix: " + ou.InvestmentCostFixProp.Value + " " + ou.InvestmentCostFixProp.MU);
                tn2.Nodes.Add("Inv. cost - prop.: " + ou.InvestmentCostPropProp.Value + " " + ou.InvestmentCostPropProp.MU);
                tn2.Nodes.Add("Oper. cost - fix: " + ou.OperatingCostFixProp.Value + " " + ou.OperatingCostFixProp.MU);
                tn2.Nodes.Add("Oper. cost - prop.: " + ou.OperatingCostPropProp.Value + " " + ou.OperatingCostPropProp.MU);
                tn2.Nodes.Add("Payout period: " + ou.PayoutPeriodProp.Value + " " + ou.PayoutPeriodProp.MU);
                tn2.Nodes.Add("Working hour/year: " + ou.WorkingHourProp.Value + " " + ou.WorkingHourProp.MU);

                treeOpUnits.Nodes[0].Nodes.Add(tn2);
                treeOpUnits.Nodes[0].Nodes[i].Nodes.Add("Input materials");
                treeOpUnits.Nodes[0].Nodes[i].Nodes.Add("Output materials");

                foreach (Canvas.IConnectableObject obj in ou.connectedobjects)
                {
                    treeOpUnits.Nodes[0].Nodes[i].Nodes[8].Nodes.Add(((Material)obj).Name);
                }

                foreach (Material mat in Graph.Materials)
                {
                    foreach (Canvas.IConnectableObject obj in mat.connectedobjects)
                    {
                        OperatingUnit act = (OperatingUnit)obj;
                        if (act.Name == ou.Name)
                        {
                            treeOpUnits.Nodes[0].Nodes[i].Nodes[9].Nodes.Add(mat.Name);
                        }
                    }
                }
                i++;
            }

            treeOpUnits.ExpandAll();

            for (int j = 0; j < i; j++)
            {
                if (i > 0)
                {
                    treeOpUnits.Nodes[0].Nodes[j].Collapse();
                }
            }

            treeMaterials.ExpandAll();

            for (int k = 0; k < treeMaterials.Nodes[0].Nodes.Count; k++)
            {
                for(int j=0;j<treeMaterials.Nodes[0].Nodes[k].Nodes.Count;j++){
                    treeMaterials.Nodes[0].Nodes[k].Nodes[j].Collapse();
                }
                
            }

            treeMaterials.Update();
            treeOpUnits.Update();
            treeMaterials.EndUpdate();
            treeOpUnits.EndUpdate();
        }

        void rename_Click(object sender, EventArgs e)
        {
            string senderObject;
            string newName;

            if (contMenu.SourceControl.Name == "treeMaterials")
            {
                senderObject = treeMaterials.SelectedNode.Name;
            }
            else
            {
                senderObject = treeOpUnits.SelectedNode.Name;
            }

            RenameDialog renameDialog = new RenameDialog();
            if (renameDialog.ShowDialog(this) == DialogResult.OK)
            {
                newName = renameDialog.textBox1.Text;
            }
            else
            {
                newName = senderObject;
            }

            if (contMenu.SourceControl.Name == "treeMaterials")
            {
                foreach (Material mat in Graph.Materials)
                {
                    if (mat.Name == senderObject)
                    {
                        mat.Name = newName;
                        break;
                    }
                }
            }
            else
            {
                foreach (OperatingUnit ou in Graph.OperatingUnits)
                {
                    if (ou.Name == senderObject)
                    {
                        ou.Name = newName;
                        break;
                    }
                }
            }

            pnsCanvas1.Refresh();
            RefreshTreeViews();
            CreateUndo();
        }

        private void button_rawmaterial_MouseDown(object sender, MouseEventArgs e)
        {
            Material m = new Material(Graph);
            m.Type = MaterialTypes.Raw;
            this.DoDragDrop(m, DragDropEffects.Move);
            pnsCanvas1.Focus();
            isProblemExists = true;
        }

        private void button_intermediatematerial_MouseDown(object sender, MouseEventArgs e)
        {
            Material m = new Material(Graph);
            m.Type = MaterialTypes.Intermediate;
            this.DoDragDrop(m, DragDropEffects.Move);
            pnsCanvas1.Focus();
            isProblemExists = true;
        }

        private void button_productmaterial_MouseDown(object sender, MouseEventArgs e)
        {
            Material m = new Material(Graph);
            m.Type = MaterialTypes.Product;
            this.DoDragDrop(m, DragDropEffects.Move);
            pnsCanvas1.Focus();
            isProblemExists = true;
        }

        private void button_operatingunit_MouseDown(object sender, MouseEventArgs e)
        {
            OperatingUnit op = new OperatingUnit(Graph);
            this.DoDragDrop(op, DragDropEffects.Move);
            pnsCanvas1.Focus();
            isProblemExists = true;
        }

        private void SetState(CanvasMode state)
        {
            pnsCanvas1.AddObjectMode = false;
            pnsCanvas1.ConnectorMode = false;

            canvasstate = state;

            switch (state)
            {
                case CanvasMode.Pointer:
                    drawingmode_menuitem.Image = drawingmode_pointer.Image;
                    pnsCanvas1.AddCursor = Cursors.Arrow;
                    break;
                case CanvasMode.Link:
                    drawingmode_menuitem.Image = drawingmode_link.Image;
                    pnsCanvas1.AddCursor = Cursors.Arrow;
                    pnsCanvas1.ConnectorMode = true;
                    break;
                case CanvasMode.Raw:
                    drawingmode_menuitem.Image = drawingmode_raw.Image;
                    Bitmap br = new Bitmap(drawingmode_raw.Image);
                    pnsCanvas1.AddCursor = new Cursor(br.GetHicon());
                    pnsCanvas1.AddObjectMode = true;
                    break;
                case CanvasMode.Intermediate:
                    drawingmode_menuitem.Image = drawingmode_intermediate.Image;
                    Bitmap bi = new Bitmap(drawingmode_intermediate.Image);
                    pnsCanvas1.AddCursor = new Cursor(bi.GetHicon());
                    pnsCanvas1.AddObjectMode = true;
                    break;
                case CanvasMode.Product:
                    drawingmode_menuitem.Image = drawingmode_product.Image;
                    Bitmap bp = new Bitmap(drawingmode_product.Image);
                    pnsCanvas1.AddCursor = new Cursor(bp.GetHicon());
                    pnsCanvas1.AddObjectMode = true;
                    break;
                case CanvasMode.OperatingUnit:
                    drawingmode_menuitem.Image = drawingmode_operatingunit.Image;
                    Bitmap bo = new Bitmap(drawingmode_operatingunit.Image);
                    pnsCanvas1.AddCursor = new Cursor(bo.GetHicon());
                    pnsCanvas1.AddObjectMode = true;
                    break;
            }
        }

        private void pointerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetState(CanvasMode.Pointer);
        }

        private void drawingmode_link_Click(object sender, EventArgs e)
        {
            SetState(CanvasMode.Link);
        }

        private void drawingmode_raw_Click(object sender, EventArgs e)
        {
            SetState(CanvasMode.Raw);
        }

        private void drawingmode_intermediate_Click(object sender, EventArgs e)
        {
            SetState(CanvasMode.Intermediate);
        }

        private void drawingmode_product_Click(object sender, EventArgs e)
        {
            SetState(CanvasMode.Product);
        }

        private void drawingmode_operatingunit_Click(object sender, EventArgs e)
        {
            SetState(CanvasMode.OperatingUnit);
        }

        private void button_zoomin_Click(object sender, EventArgs e)
        {
            pnsCanvas1.ZoomIn();
        }

        private void button_zoomout_Click(object sender, EventArgs e)
        {
            pnsCanvas1.ZoomOut();
        }

        private void button_snapobjects_Click(object sender, EventArgs e)
        {
            pnsCanvas1.SnapSelectedObjectsToGrid();
        }

        public void RefreshMinimap()
        {
            Bitmap bmp = new Bitmap(pictureBox_minimap.Width, pictureBox_minimap.Height);
            Graphics g = Graphics.FromImage(bmp);

            pnsCanvas1.Export(g, new Size(pictureBox_minimap.Width, pictureBox_minimap.Height));


            Rectangle vrect = pnsCanvas1.GetCurrentViewBoundary();

            Point tl = pnsCanvas1.TranslateCanvasToBoundary(pictureBox_minimap.ClientRectangle, vrect.Location);
            Point br = pnsCanvas1.TranslateCanvasToBoundary(pictureBox_minimap.ClientRectangle, vrect.Location + vrect.Size);

            Rectangle view = new Rectangle(tl.X, tl.Y, br.X - tl.X, br.Y - tl.Y);

            
            g.FillRectangle(new SolidBrush(Color.FromArgb(30,Color.Navy)), view);
            g.DrawRectangle(new Pen(Color.FromArgb(150, Color.DarkBlue), 1), view);


            pictureBox_minimap.Image = bmp;
            pictureBox_minimap.Refresh();
        }


        private void pictureBox_minimap_SizeChanged(object sender, EventArgs e)
        {
            RefreshMinimap();
        }

        private void pictureBox_minimap_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point center = pnsCanvas1.TranslateBoundaryToCanvas(pictureBox_minimap.ClientRectangle, e.Location);
                pnsCanvas1.SetViewCenter(center);
            }
        }

        private void pictureBox_minimap_MouseDown(object sender, MouseEventArgs e)
        {
            Point center = pnsCanvas1.TranslateBoundaryToCanvas(pictureBox_minimap.ClientRectangle, e.Location);
            pnsCanvas1.SetViewCenter(center);
        }

        public void UpdateViewList()
        {
            solutionComboBox.Items.Clear();

            IndexedComboBoxItem cbi;
            
            cbi = new IndexedComboBoxItem(-2, "Original Problem");
            solutionComboBox.Items.Add(cbi);
            cbi = new IndexedComboBoxItem(-3, "Print View");
            solutionComboBox.Items.Add(cbi);

            int solutioncount = Graph.SolutionCount;

            if (solutioncount > 0)
            {
                cbi = new IndexedComboBoxItem(-1, "-------------");
                solutionComboBox.Items.Add(cbi);

                for (int i = 0; i < solutioncount; i++)
                {
                    cbi = new IndexedComboBoxItem(i, Graph.GetSolutionTitle(i));
                    solutionComboBox.Items.Add(cbi);
                }
            }
            solutionComboBox.SelectedIndex = 0;
        }

        class IndexedComboBoxItem
        {
            public int Index = 0;
            public string Text = "";

            public IndexedComboBoxItem(int index, string text)
            {
                Index = index;
                Text = text;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        private void solutionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndexedComboBoxItem cbi = solutionComboBox.SelectedItem as IndexedComboBoxItem;
            if (cbi != null)
            {
                if (cbi.Index >= 0)
                {
                    if (LockedMode == false)
                    {
                        LockedGraphXML = Graph.ExportToXML();
                    }
                    solutionmode = true;
                    printviewmode = false;
                    Graph.ImportFromXML(LockedGraphXML);
                    Graph.PaintSolution(cbi.Index);
                    pnsCanvas1.Refresh();
                    RefreshMinimap();
                    pnsCanvas1.Locked = true;
                    propertyGrid1.ReadOnly = true;
                }
                else if (cbi.Index == -1)
                {
                    if (solutionmode)
                    {
                        solutionComboBox.SelectedIndex = 1;
                    }
                    else if (Graph.SolutionCount>0)
                    {
                        solutionComboBox.SelectedIndex = 3;
                    }
                }
                else if (cbi.Index == -3)
                {
                    if (LockedMode == false)
                    {
                        LockedGraphXML = Graph.ExportToXML();
                    }
                    printviewmode = true;
                    solutionmode = false;
                    Graph.ImportFromXML(LockedGraphXML);
                    Graph.PaintPrintView();
                    pnsCanvas1.Refresh();
                    RefreshMinimap();
                    pnsCanvas1.Locked = true;
                    propertyGrid1.ReadOnly = true;
                }
                else
                {
                    if (LockedMode == true)
                    {
                        Graph.ImportFromXML(LockedGraphXML);
                        solutionmode = false;
                        printviewmode = false;
                    }
                    pnsCanvas1.Locked = false;
                    propertyGrid1.ReadOnly = false;
                    pnsCanvas1.Refresh();
                    RefreshMinimap();
                }
            }
            if (cmbSolutions.Items.Count > 0 && solutionComboBox.SelectedIndex >= 3)
            {
                cmbSolutions.SelectedIndex = solutionComboBox.SelectedIndex - 3;
            }
        }

        private void DoCopy()
        {
            if (LockedMode == false)
            {
                Graph.DoCopy();
                paste_toolStripMenuItem.Enabled = true;
            }
        }

        private void DoPaste()
        {
            if (LockedMode == false)
            {
                Graph.DoPaste();
                RefreshMinimap();
                pnsCanvas1.Refresh();
                RefreshTreeViews();
                UpdateTitle();
                CreateUndo();
                propertyGrid1.SelectedObject = null;
            }
        }

        private void DoDuplicate()
        {
            if (LockedMode == false)
            {
                DoCopy();
                DoPaste();
            }
        }

        private void cancelAsyncButton_Click(object sender, EventArgs e)
        {
            if (Config.Instance.SolverSettings.IsOnlineSolver)
            {
                if (backgroundWorkerOnline.WorkerSupportsCancellation == true)
                {
                    backgroundWorkerOnline.CancelAsync();
                    solver.Stop();
                }
            }
            else
            {
                if (backgroundWorkerOffline.WorkerSupportsCancellation == true)
                {
                    backgroundWorkerOffline.CancelAsync();
                    foreach (var process in Process.GetProcessesByName("pns_depth"))
                    {
                        process.Kill();
                    }
                }
            }
            pwd.Close();
        }

        private void UpdateSolutionsTab()
        {
            cmbSolutions.Items.Clear();
            treeSolution.Nodes.Clear();
            foreach (Solution sol in Graph.Solutions)
            {
                cmbSolutions.Items.Add(sol.Title + ": Total cost: " + sol.OptimalValue + " " + Config.Instance.Quantity.money_mu + "/" + Config.Instance.Quantity.time_mu);
            }
            if (cmbSolutions.Items.Count != 0)
            {
                cmbSolutions.SelectedIndex = 0;
            }
        }

        private void UpdateSolutionTreeView(int solution)
        {
            treeSolution.Nodes.Clear();
            Solution sol = Graph.Solutions[solution];
            //----------Materials ág---------------
            treeSolution.Nodes.Add("Materials");
            int i = 0;
            Console.WriteLine("Materials number: " + sol.Materials.Count);
            //TODO a double helyett MaterialProperty lett a párnak az értéke
            foreach (KeyValuePair<string, MaterialProperty> mat in sol.Materials)
            {
                //TODO hozzáadtam a költségét is a materialnak
                treeSolution.Nodes[0].Nodes.Add(mat.Key + ": " + mat.Value.Flow + " " + Config.Instance.Quantity.mass_mu + "/" + Config.Instance.Quantity.time_mu +
                    ", Cost: " + mat.Value.Cost + " " + Config.Instance.Quantity.money_mu + "/" + Config.Instance.Quantity.time_mu);
                Material m = Graph.GetMaterialByName(mat.Key);
                if (m != null)
                {
                    treeSolution.Nodes[0].Nodes[i].Nodes.Add("Type: " + m.TypeProp);
                    //TODO javítottam az értékek kiírását a fában, hogy helyesen írja ki, és csak akkor, ha nem -1 az értéke a tulajdonságnak
                    if(m.PriceProp.Value != m.ParameterList["price"].NonValue)
                    {
                        double price = MUs.UnitConvert(m.PriceProp.MU, Config.Instance.Quantity.money_mu.ToString(), m.PriceProp.Value, "currency");
                        treeSolution.Nodes[0].Nodes[i].Nodes.Add("Price: " + Math.Round(price, 2) + " " + Config.Instance.Quantity.money_mu + "/" + Config.Instance.Quantity.time_mu);
                    }
                    if (m.ReqFlowProp.Value != m.ParameterList["reqflow"].NonValue)
                    {
                        double reqflow = MUs.UnitConvert(m.ReqFlowProp.MU, Config.Instance.Quantity.mass_mu.ToString(), m.ReqFlowProp.Value, "mass");
                        treeSolution.Nodes[0].Nodes[i].Nodes.Add("Min: " + Math.Round(reqflow, 4) + " " + Config.Instance.Quantity.mass_mu + "/" + Config.Instance.Quantity.time_mu);
                    }
                    if (m.MaxFlowProp.Value != m.ParameterList["maxflow"].NonValue)
                    {
                        double maxflow = MUs.UnitConvert(m.MaxFlowProp.MU, Config.Instance.Quantity.mass_mu.ToString(), m.MaxFlowProp.Value, "mass");
                        treeSolution.Nodes[0].Nodes[i].Nodes.Add("Max: " + Math.Round(maxflow, 4) + " " + Config.Instance.Quantity.mass_mu + "/" + Config.Instance.Quantity.time_mu);
                    }
                    //TODO hozzáadtam a címkéjét a materialnak, így könnyebb visszanézni a gráfban
                    treeSolution.Nodes[0].Nodes[i].Nodes.Add("Title: " + m.DisplayedText);

                }
                i++;
            }
            //-------------------------------------

            //----------Operating Units ág---------
            treeSolution.Nodes.Add("Operating Units");
            i = 0;
            foreach (KeyValuePair<string, OperatingUnitProperty> ou in sol.OperatingUnits)
            {
                //TODO hozzáadtam ,hogy írja ki a költséget is
                treeSolution.Nodes[1].Nodes.Add(ou.Key + ": " + ou.Value.Size + " " + Config.Instance.Quantity.mass_mu + "/" + Config.Instance.Quantity.time_mu +
                    ", Cost: " + ou.Value.Cost + " " + Config.Instance.Quantity.money_mu + "/" + Config.Instance.Quantity.time_mu);
                OperatingUnit opUnit = Graph.GetOperatingUnitByName(ou.Key);
                if (opUnit != null)
                {
                    if (opUnit.CapacityLowerProp.Value != opUnit.ParameterList["caplower"].NonValue)
                    {
                        double caplower = MUs.UnitConvert(opUnit.CapacityLowerProp.MU, Config.Instance.Quantity.mass_mu.ToString(), opUnit.CapacityLowerProp.Value, "mass");
                        treeSolution.Nodes[1].Nodes[i].Nodes.Add("Capacity lower bound: " + Math.Round(caplower, 4) + " " + Config.Instance.Quantity.mass_mu + "/" + Config.Instance.Quantity.time_mu);
                    }
                    if (opUnit.CapacityUpperProp.Value != opUnit.ParameterList["capupper"].NonValue)
                    {
                        double capupper = MUs.UnitConvert(opUnit.CapacityUpperProp.MU, Config.Instance.Quantity.mass_mu.ToString(), opUnit.CapacityUpperProp.Value, "mass");
                        treeSolution.Nodes[1].Nodes[i].Nodes.Add("Capacity upper bound: " + Math.Round(capupper, 4) + " " + Config.Instance.Quantity.mass_mu + "/" + Config.Instance.Quantity.time_mu);
                    }
                    if (opUnit.InvestmentCostFixProp.Value != opUnit.ParameterList["investcostfix"].NonValue)
                    {
                        double investfix = MUs.UnitConvert(opUnit.InvestmentCostFixProp.MU, Config.Instance.Quantity.money_mu.ToString(), opUnit.InvestmentCostFixProp.Value, "currency");
                        treeSolution.Nodes[1].Nodes[i].Nodes.Add("Investment fix cost: " + Math.Round(investfix, 2) + " " + Config.Instance.Quantity.money_mu + "/" + Config.Instance.Quantity.time_mu);
                    }
                    if (opUnit.InvestmentCostPropProp.Value != opUnit.ParameterList["investcostprop"].NonValue)
                    {
                        double investprop = MUs.UnitConvert(opUnit.InvestmentCostPropProp.MU, Config.Instance.Quantity.money_mu.ToString(), opUnit.InvestmentCostPropProp.Value, "currency");
                        treeSolution.Nodes[1].Nodes[i].Nodes.Add("Investment proportional cost: " + Math.Round(investprop, 2) + " " + Config.Instance.Quantity.money_mu + "/" + Config.Instance.Quantity.time_mu);
                    }
                    if (opUnit.OperatingCostFixProp.Value != opUnit.ParameterList["opercostfix"].NonValue)
                    {
                        double operfix = MUs.UnitConvert(opUnit.OperatingCostFixProp.MU, Config.Instance.Quantity.money_mu.ToString(), opUnit.OperatingCostFixProp.Value, "currency");
                        treeSolution.Nodes[1].Nodes[i].Nodes.Add("Operating fix cost: " + Math.Round(operfix, 2) + " " + Config.Instance.Quantity.money_mu + "/" + Config.Instance.Quantity.time_mu);
                    }
                    if (opUnit.OperatingCostPropProp.Value != opUnit.ParameterList["opercostprop"].NonValue)
                    {
                        double operprop = MUs.UnitConvert(opUnit.OperatingCostPropProp.MU, Config.Instance.Quantity.money_mu.ToString(), opUnit.OperatingCostPropProp.Value, "currency");
                        treeSolution.Nodes[1].Nodes[i].Nodes.Add("Operating proportional cost: " + Math.Round(operprop, 2) + " " + Config.Instance.Quantity.money_mu + "/" + Config.Instance.Quantity.time_mu);
                    }

                    //TODO ezt javítottam, a megoldásból jönnek ezek az adatok, mert lehet akár kieső node is a megoldásnál
                    treeSolution.Nodes[1].Nodes[i].Nodes.Add("Input materials");
                    treeSolution.Nodes[1].Nodes[i].Nodes.Add("Output materials");

                    foreach (MaterialProperty m in ou.Value.Input)
                    {
                        treeSolution.Nodes[1].Nodes[i].Nodes[treeSolution.Nodes[1].Nodes[i].Nodes.Count - 2].Nodes.Add(m.Name + ": " + Math.Round(m.Flow, 4) + " " + Config.Instance.Quantity.mass_mu + "/" + Config.Instance.Quantity.time_mu);
                    }

                    foreach (MaterialProperty m in ou.Value.Output)
                    {
                        treeSolution.Nodes[1].Nodes[i].Nodes[treeSolution.Nodes[1].Nodes[i].Nodes.Count - 1].Nodes.Add(m.Name + ": " + Math.Round(m.Flow, 4) + " " + Config.Instance.Quantity.mass_mu + "/" + Config.Instance.Quantity.time_mu);
                    }
                }
                i++;
            }

            treeSolution.Nodes[0].Expand();
            treeSolution.Nodes[1].Expand();
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyValue < 48 || e.KeyValue > 57) && e.KeyValue != 8)
            {
                e.SuppressKeyPress = true;
            }
            else
            {
                e.SuppressKeyPress = false;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ProblemSettingsDialog ssd = new ProblemSettingsDialog();
            ssd.ShowDialog();
        }

        private void backgroundWorkerOffline_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            Problem p = new Problem(algorithm, Graph, 0, Config.Instance.SolverSettings.SolutionLimit);
            FileConnector.ProblemToSolverInput(p, p.name);
            inPath = Config.Instance.SolverSettings.OfflineSolverTempFolder + p.name + ".in";
            outPath = Config.Instance.SolverSettings.OfflineSolverTempFolder + p.name + ".out";
            Console.WriteLine("InPath: " + inPath);
            string arguments = algorithm + " \"" + inPath + "\" " + "\"" + outPath + "\" " + Config.Instance.SolverSettings.SolutionLimit.ToString();

            worker.ReportProgress(10);
            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.WorkingDirectory = Path.GetTempPath();
            pInfo.RedirectStandardError = true;
            pInfo.RedirectStandardOutput = true;
            pInfo.UseShellExecute = false;
            pInfo.Arguments = arguments;
            pInfo.FileName = "Solver/pns_depth.exe";
            pInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pInfo.CreateNoWindow = true;
            int exitCode = 1;

            Process solver = new Process();
            try
            {
                solver.StartInfo = pInfo;
                solver.Start();
                solver.WaitForExit();
                exitCode = solver.ExitCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            worker.ReportProgress(70);

            if (exitCode == 0)
            {
                try
                {
                    FileStream file = new FileStream(outPath, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(file);
                    string solution = sr.ReadToEnd();
                    FileConnector.ParseSolution(solution, Graph);
                    sr.Close();
                    file.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            worker.ReportProgress(100);
        }

        private void backgroundWorkerOffline_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelResult.Text = (e.ProgressPercentage.ToString() + "%");
            pwd.Message = "In progress, please wait... " + e.ProgressPercentage.ToString() + "%";
            pwd.ProgressMessage = "Creating .in file...";
            if (e.ProgressPercentage == 10)
            {
                pwd.ProgressMessage = "Solver is working...";
            }
            if (e.ProgressPercentage == 70)
            {
                pwd.ProgressMessage = "Reading from .out file...";
            }
            pwd.ProgressValue = e.ProgressPercentage;
        }

        private void backgroundWorkerOffline_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                labelResult.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                labelResult.Text = "Error: " + e.Error.Message;
            }
            else
            {
                labelResult.Text = "Done!";
                UpdateViewList();
                UpdateSolutionsTab();
                tabControl1.SelectedTab = tabPage3;

                if (Graph.SolutionCount > 0)
                {
                    solutionComboBox.Visible = true;
                }
            }
            pwd.Close();

            if (!Config.Instance.SolverSettings.IsKeepTempFiles)
            {
                File.Delete(inPath);
                File.Delete(outPath);
            }
            
        }

        public void ExportToExcel(object sender, string filename, ExcelExportType t_export_type)
        {
            if (t_export_type == ExcelExportType.graph_export)
            {
                ProblemToExcel.PNSProblemToExcel(false, filename, Graph);
                return;
            }

            if (cmbSolutions.SelectedIndex != -1)
            {
                ResultExcelExport t_export = new ResultExcelExport(Graph.Solutions[cmbSolutions.SelectedIndex]);
                ResultsSummaryExcelExport t_summary = new ResultsSummaryExcelExport(Graph.Materials, Graph.OperatingUnits, Graph.Solutions);
                switch (t_export_type)
                {
                    case ExcelExportType.brief_export:
                        t_export.ResultToExcel(true, false, filename, Graph);
                        break;
                    case ExcelExportType.brief_view:
                        t_export.ResultToExcel(true, true, filename, Graph);
                        break;
                    case ExcelExportType.detailed_export:
                        t_export.ResultToExcel(false, false, filename, Graph);
                        break;
                    case ExcelExportType.detailed_view:
                        t_export.ResultToExcel(false, true, filename, Graph);
                        break;
                    case ExcelExportType.export_summary_of_results:
                        t_summary.ResultsToExcel(false, filename);
                        break;
                    case ExcelExportType.view_summary_of_results:
                        t_summary.ResultsToExcel(true, filename);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Not selected solution, please select one!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void menuBriefExport_Click(object sender, EventArgs e)
        {
            //ExportToExcel(sender, ExcelExportType.brief_export);
        }

        private void menuBriefView_Click(object sender, EventArgs e)
        {
           // ExportToExcel(sender, ExcelExportType.brief_view);
        }

        private void menuDetailedExport_Click(object sender, EventArgs e)
        {
           // ExportToExcel(sender, ExcelExportType.detailed_export);
        }

        private void menuDetailedView_Click(object sender, EventArgs e)
        {
           // ExportToExcel(sender, ExcelExportType.detailed_view);
        }

        private void menuSummaryExport_Click(object sender, EventArgs e)
        {
           // ExportToExcel(sender, ExcelExportType.export_summary_of_results);
        }

        private void menuSummaryView_Click(object sender, EventArgs e)
        {
           // ExportToExcel(sender, ExcelExportType.view_summary_of_results);
        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle rect = pnsCanvas1.GetCanvasBoundary();
            Point p = new Point(rect.Size.Width-1, rect.Size.Height-1);
            Size s = new Size(rect.Size.Width, rect.Size.Height);
            Bitmap bmp = new Bitmap(s.Width, s.Height);
            Graphics g = Graphics.FromImage(bmp);
            pnsCanvas1.Export(g, s);
            
            e.Graphics.DrawImage(bmp, e.MarginBounds);
        }

        private void CheckMenuItem(ToolStripMenuItem mnu, ToolStripMenuItem checked_item)
        {
            foreach (ToolStripItem item in mnu.DropDownItems)
            {
                if (item is ToolStripMenuItem)
                {
                    ToolStripMenuItem menu_item = item as ToolStripMenuItem;
                    menu_item.Checked = (menu_item == checked_item);
                }
            }
        }

        private void backgroundWorkerOnline_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            Problem problem = new Problem(algorithm, Graph, Config.Instance.SolverSettings.NumberOfSolverProccess, Config.Instance.SolverSettings.SolutionLimit);
            solver = new Solver(problem);
            problem = solver.Run(worker);
        }

        private void backgroundWorkerOnline_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelResult.Text = (e.ProgressPercentage.ToString() + "%");
            pwd.Message = "In progress, please wait... " + e.ProgressPercentage.ToString() + "%";
            pwd.ProgressValue = e.ProgressPercentage;
            pwd.ProgressMessage = "Connecting to server...";
            if (e.ProgressPercentage == 10)
            {
                pwd.ProgressMessage = "Sending problem to server...";
            }
            if (e.ProgressPercentage == 80)
            {
                pwd.ProgressMessage = "Retrieving solution...";
            }
        }

        private void backgroundWorkerOnline_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                labelResult.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                labelResult.Text = "Error: " + e.Error.Message;
            }
            else
            {
                labelResult.Text = "Done!";
                UpdateViewList();
                UpdateSolutionsTab();
                tabControl1.SelectedTab = tabPage3;

                if (Graph.SolutionCount > 0)
                {
                    solutionComboBox.Visible = true;
                }
            }
            pwd.Close();
        }

        private void UpdateListOfQuantities()
        {
            QuantityTypes.listOfQuantities = new string[Quantity.quantities.Count];
            Quantity.quantities.Keys.CopyTo(QuantityTypes.listOfQuantities, 0);
        }

        private void UpdateListOfPriceMUs()
        {
            PriceMUs.listOfPriceMUs = new string[Enum.GetNames(typeof(Config.MoneyUnit)).Length];
            PriceMUs.listOfPriceMUs = Enum.GetNames(typeof(Config.MoneyUnit));
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Graph.GenerateLayout();
            pnsCanvas1.Refresh();
            RefreshMinimap();
            CreateUndo();
        }

        private void pinSelectedButton_Click(object sender, EventArgs e)
        {
            pnsCanvas1.PinSelectedObjects();
            CreateUndo();
        }

        private void pinUnselectButton_Click(object sender, EventArgs e)
        {
            pnsCanvas1.UnPinSelectedObjects();
            CreateUndo();
        }

        private void SignInButton_Click(object sender, EventArgs e)
        {
            if (!Config.Instance.Login.IsLoggedIn)
            {
                LoginDialog sd = new LoginDialog();
                sd.ShowDialog();

                Config.Instance.Login.IsLoggedIn = sd.LoginSuccess;

                if (sd.LoginSuccess)
                {
                    SignInButton.Text = Config.Instance.Login.Username + " / Logout";
                }
            }
            else
            {
                UserDialog ud = new UserDialog(Config.Instance.Login.Username);
                ud.ShowDialog();

                if (ud.LogoutSuccess)
                {
                    SignInButton.Text = "Login/Register";
                    UpdateAlgorithmComboBox();
                    MessageBox.Show("Logout success!\nGood bye!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            propertyGrid1.Refresh();
            pnsCanvas1.Refresh();
            RefreshTreeViews();
            CreateUndo();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.Instance.CheckLogin())
            {
                return;
            }

            bool process = true;
            if (CheckModification())
            {

                DialogResult dr = MessageBox.Show(
                    "The graph has been changed. Do you want to save the changes?",
                    "Open",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                    );

                switch (dr)
                {
                    case DialogResult.Yes:
                        OnSave();
                        break;
                    case DialogResult.No:

                        break;
                    default:
                        process = false;
                        break;
                }
            }

            if (process)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = false;
                ofd.Filter = "pns file|*.pns";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    OnLoad(ofd.FileName);
                }
                redoToolStripMenuItem.Enabled = false;
                undoToolStripMenuItem.Enabled = false;
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InstallUpdateSyncWithInfo();
        }

        private void InstallUpdateSyncWithInfo()
        {
            UpdateCheckInfo info = null;

            ApplicationDeployment ad;

            try
            {
                ad = ApplicationDeployment.CurrentDeployment;
                info = ad.CheckForDetailedUpdate();
            }
            catch (DeploymentDownloadException dde)
            {
                MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                return;
            }
            catch (InvalidDeploymentException ide)
            {
                MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                return;
            }
            catch (InvalidOperationException ioe)
            {
                MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                return;
            }

            if (info.UpdateAvailable)
            {
                Boolean doUpdate = true;

                if (!info.IsUpdateRequired)
                {
                    DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                    if (!(DialogResult.OK == dr))
                    {
                        doUpdate = false;
                    }
                }
                else
                {
                    // Display a message that the app MUST reboot. Display the minimum required version.
                    MessageBox.Show("This application has detected a mandatory update from your current " +
                        "version to version " + info.MinimumRequiredVersion.ToString() +
                        ". The application will now install the update and restart.",
                        "Update Available", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                if (doUpdate)
                {
                    try
                    {
                        ad.Update();
                        MessageBox.Show("The application has been upgraded, and will now restart.");
                        Application.Restart();
                    }
                    catch (DeploymentDownloadException dde)
                    {
                        MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("This application is up to date.", "Up to date", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /***** SANDBOX *****/
    }
}