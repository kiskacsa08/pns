using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
//using PNSDraw.PnsSolver;
using PNSDraw.Entities;
using System.Threading;
using System.Globalization;
using Pns.PnsSolver;

namespace PNSDraw
{
    public partial class Form1 : Form
    {
        enum CanvasMode { Pointer, Link, Raw, Intermediate, Product, OperatingUnit }

        enum ObjectType { Raw, Intermediate, Product, OperatingUnit }

        PGraph Graph;

        string LastGraphXML = "";

        string LockedGraphXML = "";

        bool solutionmode = false;
        bool printviewmode = false;

        ContextMenuStrip contMenu;

        bool LockedMode
        {
            get
            {
                return solutionmode || printviewmode;
            }
        }

        string CurrentFile = "";

        CanvasMode canvasstate = CanvasMode.Pointer;

        public Form1()
        {
            InitializeComponent();
            Graph = new PGraph();
            pnsCanvas1.GraphicsStructure = Graph;
            
        }

        void pnsCanvas1_DataChanged(object sender, Canvas.CanvasEventArgs e)
        {
            propertyGrid1.Refresh();
            RefreshTreeViews();
            CreateUndo();
        }

        void pnsCanvas1_SelectionChanged(object sender, Canvas.CanvasEventArgs e)
        {
            List<Canvas.IGraphicsObject> selection = e.Data as List<Canvas.IGraphicsObject>;
            if (selection != null && selection.Count > 0)
            {
                copy_toolStripMenuItem.Enabled = true;
                duplicate_toolStripMenuItem.Enabled = true;
            }
            else
            {
                copy_toolStripMenuItem.Enabled = false;
                duplicate_toolStripMenuItem.Enabled = false;
            }
            if (selection != null && selection.Count == 1)
            {
                if (selection[0] is EdgeNode)
                {
                    propertyGrid1.SelectedObject = selection[0];
                }
                else
                {
                    propertyGrid1.SelectedObject = selection[0].GetParentObject();
                }
            }
            else
            {
                propertyGrid1.SelectedObject = null;
            }
            propertyGrid1.Refresh();
        }

        void pnsCanvas1_RemoveObjects(object sender, Canvas.CanvasEventArgs e)
        {

            List<Canvas.IGraphicsObject> toremove = e.Data as List<Canvas.IGraphicsObject>;
            if (toremove!=null)
            {
                DialogResult res = MessageBox.Show("Do you want to remove the selected objects?", "Confirm", MessageBoxButtons.YesNo);
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    Graph.RemoveObjects(toremove);
                    RefreshTreeViews();
                }
            }
            CreateUndo();
        }

        

        void pnsCanvas1_NewConnector(object sender, Canvas.CanvasEventArgs e)
        {
            PNSDraw.Edge edge = new PNSDraw.Edge(Graph);
            Canvas.ObjectConnector conn = e.Data as Canvas.ObjectConnector;
            if (conn == null)
            {
                return;
            }
            edge.begin = conn.Begin;
            edge.end = conn.End;
            pnsCanvas1.AddObject(edge);
            edge.end.AddConnection(edge.begin);
            e.Handled = true;
            Graph.AddEdge(edge);
        }

        void pnsCanvas1_ClickItem(object sender, Canvas.CanvasEventArgs e)
        {
            Canvas.IGraphicsObject obj = e.Data as Canvas.IGraphicsObject;

            if (obj!=null)
            {
                if (obj.IsPartialObject() && (obj is EdgeNode) == false)
                {
                    propertyGrid1.SelectedObject = obj.GetParentObject();
                    
                    copy_toolStripMenuItem.Enabled = false;
                    duplicate_toolStripMenuItem.Enabled = false;
                }
                else
                {
                    propertyGrid1.SelectedObject = obj;

                    copy_toolStripMenuItem.Enabled = true;
                    duplicate_toolStripMenuItem.Enabled = true;
                }
            }
            else
            {
                propertyGrid1.SelectedObject = null;
            }
        }

        void pnsCanvas1_NewItem(object sender, Canvas.CanvasEventArgs e)
        {
            switch (canvasstate)
            {
                case CanvasMode.Raw:
                    Material mr = new Material(Graph);
                    mr.Type = Globals.MaterialTypes.Raw;
                    e.Data = mr;
                    Graph.AddMaterial(mr);

                    break;
                case CanvasMode.Intermediate:
                    Material mi = new Material(Graph);
                    mi.Type = Globals.MaterialTypes.Intermediate;
                    e.Data = mi;
                    Graph.AddMaterial(mi);

                    break;
                case CanvasMode.Product:
                    Material mp = new Material(Graph);
                    mp.Type = Globals.MaterialTypes.Product;
                    e.Data = mp;
                    Graph.AddMaterial(mp);

                    break;
                case CanvasMode.OperatingUnit:
                    OperatingUnit ou = new OperatingUnit(Graph);
                    e.Data = ou;
                    Graph.AddOperatingUnit(ou);

                    break;
                default:
                    break;
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            pnsCanvas1.Refresh();
            RefreshTreeViews();
            CreateUndo();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool process = true;
            if (CheckModification())
            {
                DialogResult dr = MessageBox.Show("The graph has been changed. Do you want to continue?", "New", MessageBoxButtons.YesNo);
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    process = false;
                }
            }
            if (process == false)
            {
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "xml file|*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream file = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(file);
                    
                    string xml = sr.ReadToEnd();
                    Graph.Reset();
                    Graph.ImportFromXML(xml);
                    
                    pnsCanvas1.Reset();
                    pnsCanvas1.GridSize = Globals.GridSize;
                    RefreshMinimap(); // Azert kell, mert a TextObjectek rajzolaskor szamoljak ki a meretuket
                    ZoomToFit();
                    pnsCanvas1.Refresh();
                    LastGraphXML = Graph.ExportToXML();
                    CurrentFile = ofd.FileName;
                    CreateUndo();
                    UpdateTitle();

                    

                    ResetViewList();
                    RefreshTreeViews();
                    sr.Close();
                    file.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            redoToolStripMenuItem.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
        }

        private void ResetViewList()
        {
            printviewmode = false;
            solutionmode = false;
            UpdateViewList();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnSave();
        }

        private void showGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnsCanvas1.ShowGrid = ((ToolStripMenuItem)sender).Checked;
        }

        private void snapToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnsCanvas1.SnapToGrid = ((ToolStripMenuItem)sender).Checked;
        }

        private void DoUndo()
        {
            if (solutionmode)
            {
                return;
            }
            redoToolStripMenuItem.Enabled = Graph.DoUndo();
            undoToolStripMenuItem.Enabled = false;
            pnsCanvas1.GridSize = Globals.GridSize;
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
            undoToolStripMenuItem.Enabled = Graph.DoRedo();
            redoToolStripMenuItem.Enabled = false;
            pnsCanvas1.GridSize = Globals.GridSize;
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

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoUndo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoRedo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pnsCanvas1.Objects = Graph.GetObjectList();
            pnsCanvas1.NewItem += new Canvas.CanvasEventHandler(pnsCanvas1_NewItem);
            pnsCanvas1.ClickItem += new Canvas.CanvasEventHandler(pnsCanvas1_ClickItem);
            pnsCanvas1.NewConnector += new Canvas.CanvasEventHandler(pnsCanvas1_NewConnector);
            pnsCanvas1.RemoveObjects += new Canvas.CanvasEventHandler(pnsCanvas1_RemoveObjects);
            pnsCanvas1.SelectionChanged += new Canvas.CanvasEventHandler(pnsCanvas1_SelectionChanged);
            pnsCanvas1.DataChanged += new Canvas.CanvasEventHandler(pnsCanvas1_DataChanged);
            pnsCanvas1.KeyDown += new KeyEventHandler(Form1_KeyDown);
            pnsCanvas1.EditItem += new Canvas.CanvasEventHandler(pnsCanvas1_EditItem);
            pnsCanvas1.ViewChanged += new Canvas.CanvasEventHandler(pnsCanvas1_ViewChanged);
            pnsCanvas1.Copy += new Canvas.CanvasEventHandler(pnsCanvas1_Copy);
            pnsCanvas1.Paste += new Canvas.CanvasEventHandler(pnsCanvas1_Paste);
            pnsCanvas1.Duplicate += new Canvas.CanvasEventHandler(pnsCanvas1_Duplicate);

            CreateUndo();
            LastGraphXML = Graph.ExportToXML();
            CurrentFile = "";
            UpdateTitle();
            ResetViewList();
            toolStripComboBox2.SelectedIndex = 0;
            RefreshTreeViews();
        }

        void pnsCanvas1_Duplicate(object sender, Canvas.CanvasEventArgs e)
        {
            DoDuplicate();
        }

        void pnsCanvas1_Paste(object sender, Canvas.CanvasEventArgs e)
        {
            DoPaste();
        }

        void pnsCanvas1_Copy(object sender, Canvas.CanvasEventArgs e)
        {
            DoCopy();
        }

        void pnsCanvas1_ViewChanged(object sender, Canvas.CanvasEventArgs e)
        {
            RefreshMinimap();
        }

        void pnsCanvas1_EditItem(object sender, Canvas.CanvasEventArgs e)
        {
            propertyGrid1.Focus();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z && e.Control)
            {
                DoUndo();
            }
            else if (e.KeyCode == Keys.Y && e.Control)
            {
                DoRedo();
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                OnSave();
            }
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ZoomToFit()
        {   
            pnsCanvas1.ZoomToFit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool process = true;
            if (CheckModification())
            {
                DialogResult dr = MessageBox.Show("The graph has been changed. Do you want to continue?", "New", MessageBoxButtons.YesNo);
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    process = false;
                }
            }
            if (process == false)
            {
                return;
            }
            Graph.Reset();
            pnsCanvas1.Reset();
            pnsCanvas1.GridSize = Globals.GridSize;
            pnsCanvas1.Refresh();
            LastGraphXML = Graph.ExportToXML();
            ResetViewList();
            CurrentFile = "";
            CreateUndo();
            UpdateTitle();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool exit = true;
            if (CheckModification())
            {
                DialogResult dr = MessageBox.Show("The graph has been changed. Do you want to quit?", "Exit", MessageBoxButtons.YesNo);
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    exit = false;
                }
            }
            if (exit == false)
            {
                e.Cancel = true;
            }
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
            sfd.Filter = "xml file|*.xml";
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

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnSaveAs();
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
            Text = "PNS Draw (" + name + ")";
        }

        private void exportToSVGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "svg file|*.svg";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(file);
                    string xml = Graph.ExportToSVG();
                    sw.Write(xml);
                    sw.Close();
                    file.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void exportToPNSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "pns file|*.pns";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(file, Encoding.Unicode);
                    string xml = Graph.ExportToPNS();
                    sw.Write(xml);
                    sw.Close();
                    file.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
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
                switch (mat.Type)
                {
                    case Globals.MaterialTypes.Raw:
                        tn.Name = mat.Name;
                        tn.Text = mat.Name;
                        treeMaterials.Nodes[0].Nodes[0].Nodes.Add(tn);
                        break;
                    case Globals.MaterialTypes.Intermediate:
                        tn.Name = mat.Name;
                        tn.Text = mat.Name;
                        treeMaterials.Nodes[0].Nodes[1].Nodes.Add(tn);
                        break;
                    case Globals.MaterialTypes.Product:
                        tn.Name = mat.Name;
                        tn.Text = mat.Name;
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
                treeOpUnits.Nodes[0].Nodes.Add(tn2);
                treeOpUnits.Nodes[0].Nodes[i].Nodes.Add("Input materials");
                treeOpUnits.Nodes[0].Nodes[i].Nodes.Add("Output materials");
                foreach (Canvas.IConnectableObject obj in ou.connectedobjects)
                {
                    treeOpUnits.Nodes[0].Nodes[i].Nodes[0].Nodes.Add(((Material)obj).Name);
                }

                foreach (Material mat in Graph.Materials)
                {
                    foreach (Canvas.IConnectableObject obj in mat.connectedobjects)
                    {
                        OperatingUnit act = (OperatingUnit)obj;
                        if (act.Name == ou.Name)
                        {
                            treeOpUnits.Nodes[0].Nodes[i].Nodes[1].Nodes.Add(mat.Name);
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
            m.Type = Globals.MaterialTypes.Raw;
            this.DoDragDrop(m, DragDropEffects.Move);
            pnsCanvas1.Focus();
        }

        private void button_intermediatematerial_MouseDown(object sender, MouseEventArgs e)
        {
            Material m = new Material(Graph);
            m.Type = Globals.MaterialTypes.Intermediate;
            this.DoDragDrop(m, DragDropEffects.Move);
            pnsCanvas1.Focus();
        }

        private void button_productmaterial_MouseDown(object sender, MouseEventArgs e)
        {
            Material m = new Material(Graph);
            m.Type = Globals.MaterialTypes.Product;
            this.DoDragDrop(m, DragDropEffects.Move);
            pnsCanvas1.Focus();
        }

        private void button_operatingunit_MouseDown(object sender, MouseEventArgs e)
        {
            OperatingUnit op = new OperatingUnit(Graph);
            this.DoDragDrop(op, DragDropEffects.Move);
            pnsCanvas1.Focus();
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

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnsCanvas1.ZoomIn();
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnsCanvas1.ZoomOut();
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

        private void lockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnsCanvas1.Locked = !pnsCanvas1.Locked;
            lockToolStripMenuItem.Checked = pnsCanvas1.Locked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();

            sw.ShowDialog();

            if (sw.Changed)
            {
                if (LockedMode)
                {
                    toolStripComboBox1_SelectedIndexChanged(this, EventArgs.Empty);
                }
                else
                {
                    pnsCanvas1.GridSize = Globals.GridSize;
                    pnsCanvas1.Refresh();
                    propertyGrid1.Refresh();
                }   
            }
        }

        private void smallPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG file|*.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Rectangle rect = pnsCanvas1.GetCanvasBoundary();
                Size s = new Size(rect.Size.Width / 4, rect.Size.Height/4);
                Bitmap bmp = new Bitmap(s.Width, s.Height);
                Graphics g = Graphics.FromImage(bmp);
                pnsCanvas1.Export(g, s);
                bmp.Save(sfd.FileName);
            }
        }

        private void mediumPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG file|*.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Rectangle rect = pnsCanvas1.GetCanvasBoundary();
                Size s = new Size(rect.Size.Width / 2, rect.Size.Height / 2);
                Bitmap bmp = new Bitmap(s.Width, s.Height);
                Graphics g = Graphics.FromImage(bmp);
                pnsCanvas1.Export(g, s);
                bmp.Save(sfd.FileName);
            }
        }

        private void largePNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG file|*.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Rectangle rect = pnsCanvas1.GetCanvasBoundary();
                Size s = new Size(rect.Size.Width, rect.Size.Height);
                Bitmap bmp = new Bitmap(s.Width, s.Height);
                Graphics g = Graphics.FromImage(bmp);
                pnsCanvas1.Export(g, s);
                bmp.Save(sfd.FileName);
            }
        }
        
        private void exportToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            int sizelimit = 6000 * 6000;

            Rectangle rect = pnsCanvas1.GetCanvasBoundary();
            Size s = new Size(rect.Size.Width, rect.Size.Height);
            largePNGToolStripMenuItem.Text = "Large (" + s.Width + "x" + s.Height + ")";
            mediumPNGToolStripMenuItem.Text = "Medium (" + (s.Width / 2) + "x" + (s.Height / 2) + ")";
            smallPNGToolStripMenuItem.Text = "Small (" + (s.Width / 4) + "x" + (s.Height / 4) + ")";

            int bmpsize = s.Height * s.Width;

            if (bmpsize > sizelimit)
            {
                largePNGToolStripMenuItem.Enabled = false;
            }
            else
            {
                largePNGToolStripMenuItem.Enabled = true;
            }

            if (bmpsize / 4 > sizelimit)
            {
                mediumPNGToolStripMenuItem.Enabled = false;
            }
            else
            {
                mediumPNGToolStripMenuItem.Enabled = true;
            }

            if (bmpsize / 16 > sizelimit)
            {
                smallPNGToolStripMenuItem.Enabled = false;
            }
            else
            {
                smallPNGToolStripMenuItem.Enabled = true;
            }
        }



        private void loadASolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "all file|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream file = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(file);

                    string solution = sr.ReadToEnd();

                    Graph.ParseSolution(solution);
                    UpdateViewList();

                    if (Graph.SolutionCount > 0)
                    {
                        toolStripComboBox1.Visible = true;
                    }
                    

                    sr.Close();
                    file.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Bad solution file format.");
                }
            }
            redoToolStripMenuItem.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
        }


        public void UpdateViewList()
        {
            toolStripComboBox1.Items.Clear();

            IndexedComboBoxItem cbi;
            
            cbi = new IndexedComboBoxItem(-2, "Original Problem");
            toolStripComboBox1.Items.Add(cbi);
            cbi = new IndexedComboBoxItem(-3, "Print View");
            toolStripComboBox1.Items.Add(cbi);

            int solutioncount = Graph.SolutionCount;

            if (solutioncount > 0)
            {
                cbi = new IndexedComboBoxItem(-1, "-------------");
                toolStripComboBox1.Items.Add(cbi);

                for (int i = 0; i < solutioncount; i++)
                {
                    cbi = new IndexedComboBoxItem(i, Graph.GetSolutionTitle(i));
                    toolStripComboBox1.Items.Add(cbi);
                }
            }
            toolStripComboBox1.SelectedIndex = 0;
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

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndexedComboBoxItem cbi = toolStripComboBox1.SelectedItem as IndexedComboBoxItem;
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
                        toolStripComboBox1.SelectedIndex = 1;
                    }
                    else if (Graph.SolutionCount>0)
                    {
                        toolStripComboBox1.SelectedIndex = 3;
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
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SolutionSettingsWindow sw = new SolutionSettingsWindow();

            sw.ShowDialog();

            if (sw.Changed)
            {
                toolStripComboBox1_SelectedIndexChanged(this, EventArgs.Empty);
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

        private void copy_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoCopy();
        }

        private void paste_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoPaste();
        }

        private void duplicate_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoDuplicate();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void treeMaterials_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                e.Node.TreeView.SelectedNode = e.Node;
            }
        }

        private void toolStripButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripButton3.Checked)
            {
                Image check = PNSDraw.Properties.Resources.green_check;
                toolStripButton3.Image = check;
                toolStripTextBox1.Enabled = true;
                toolStripTextBox2.Enabled = true;
                toolStripComboBox2.Items.Clear();
                toolStripComboBox2.Items.Add("ABB");
                toolStripComboBox2.Items.Add("SSG");
                toolStripComboBox2.SelectedIndex = 0;
            }
            else
            {
                Image check = PNSDraw.Properties.Resources.red_check;
                toolStripButton3.Image = check;
                toolStripTextBox1.Enabled = false;
                toolStripTextBox2.Enabled = false;
                toolStripComboBox2.Items.Clear();
                toolStripComboBox2.Items.Add("ABB");
                toolStripComboBox2.Items.Add("SSG");
                toolStripComboBox2.Items.Add("SSG+LP");
                toolStripComboBox2.Items.Add("MSG");
                toolStripComboBox2.SelectedIndex = 0;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string algorithm;
            switch (toolStripComboBox2.SelectedIndex)
            {
                case 0:
                    algorithm = Solver_keys._KEY_ABB;
                    break;
                case 1:
                    algorithm = Solver_keys._KEY_SSG;
                    break;
                case 2:
                    algorithm = Solver_keys._KEY_SSGLP;
                    break;
                case 3:
                    algorithm = Solver_keys._KEY_MSG;
                    break;
                default:
                    algorithm = "";
                    break;
            }

            if (toolStripButton3.Checked)
            {
                if (toolStripTextBox1.Text == "" || toolStripTextBox2.Text == "")
                {
                    MessageBox.Show("Please fill all fields!");
                }
                else
                {
                    int processes = int.Parse(toolStripTextBox1.Text);
                    int limit = int.Parse(toolStripTextBox2.Text);
                    //string algorithm tartalmazza az algoritmust (feljebb), int processes a folyamatok számát, int limit a megoldások limitjét
                    // TODO: Ide jön az online megoldó hívása
                    MessageBox.Show("Online");
                }
            }
            else
            {
                MessageBox.Show("Offline");
                Console.WriteLine(Adapter.StartSolver(algorithm, Graph));
            }
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue < 48 || e.KeyValue > 57)
            {
                e.SuppressKeyPress = true;
            }
            else
            {
                e.SuppressKeyPress = false;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MutualExclusionDialog med = new MutualExclusionDialog(Graph);
            med.ShowDialog();
        }



        //-------------------------------------------------------------------SOLVER-------------------------------------------------------------------------------------------

        //Solutions m_solutions;
        //StringBuilder m_result_string;
        //bool m_result;
        //private void StartSolver(string t_method)
        //{
        //    m_solutions = new Solutions();
        //    m_result_string = new StringBuilder(1000000);
        //    Thread t_solver = new Thread(new ParameterizedThreadStart(SolverThread));
        //}

        //private void SolverThread(object t_method)
        //{
        //    m_result = false;
        //    m_result = CallSolver((string)t_method);
        //}


        //bool CallSolver(string t_method)
        //{
        //    ProblemToSolverInput();
        //    ProcessStartInfo t_info = new ProcessStartInfo();
        //    t_info.WorkingDirectory = System.Windows.Forms.Application.StartupPath;
        //    t_info.FileName = Solver_keys.solver;
        //    t_info.Arguments = t_method +
        //        " \"" + m_filename + Solver_keys._SOLVER_INPUT_EXTENSION + "\" " +
        //        "\"" + m_filename + Solver_keys._SOLVER_ROW_OUTPUT_EXTENSION + "\" " +
        //        DefaultMUsAndValues.DefaultValues.max_solutions;
        //    t_info.RedirectStandardError = true;
        //    t_info.RedirectStandardOutput = true;
        //    t_info.CreateNoWindow = true;
        //    t_info.UseShellExecute = false;
        //    t_info.WindowStyle = ProcessWindowStyle.Hidden;
        //    Process t_proc = new Process();
        //    try
        //    {
        //        t_proc.StartInfo = t_info;
        //        t_proc.OutputDataReceived += new DataReceivedEventHandler(t_proc_OutputDataReceived);
        //        t_proc.Start();
        //        m_solver_dlg.SolverProcess = t_proc;
        //        t_proc.BeginOutputReadLine();
        //        t_proc.WaitForExit();
        //    }
        //    catch (Exception ex)
        //    {
        //        m_result_string.AppendLine(ex.ToString());
        //        return false;
        //    }
        //    if (t_proc.ExitCode != 0)
        //    {
        //        string t_error = t_proc.StandardError.ReadToEnd();
        //        if (t_error == "") m_result_string.AppendLine("Solver error!\nExit code: " + t_proc.ExitCode);
        //        else m_result_string.AppendLine(t_error);
        //        return false;
        //    }
        //    m_timer.Stop();
        //    m_startpos = 0;
        //    FileStream fs = new FileStream(m_filename + Solver_keys._SOLVER_ROW_OUTPUT_EXTENSION, FileMode.Open);
        //    StreamReader sr = new StreamReader(fs);
        //    m_result_string = new StringBuilder();
        //    while (!sr.EndOfStream &&
        //        (m_result_string.Length < Solver_keys._SOLVER_ROW_OUTPUT_MAX_DISPLAYED_SIZE ||
        //        t_method == Solver_keys._KEY_ABB || t_method == Solver_keys._KEY_SSGLP)) m_result_string.AppendLine(sr.ReadLine());
        //    if (!sr.EndOfStream) m_result_string.AppendLine("...");
        //    sr.Close();
        //    fs.Close();
        //    if (t_method == Solver_keys._KEY_ABB || t_method == Solver_keys._KEY_SSGLP)
        //    {
        //        m_solutions = ParseSolution(m_result_string.ToString());
        //    }

        //    return true;
        //}


        //public void ProblemToSolverInput()
        //{
        //    CultureInfo t_original_culture = CultureInfo.CurrentCulture;
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
        //    FileStream fs = new FileStream(m_filename + Solver_keys._SOLVER_INPUT_EXTENSION, FileMode.Create);
        //    StreamWriter sw = new StreamWriter(fs);
        //    string t_str = Solver_keys._FILE_TYPE;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._FILE_NAME + Converters.ToNameString(Path.GetFileName(m_filename) + Solver_keys._SOLVER_INPUT_EXTENSION);
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._MEASUREMENT_UNITS;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._MASS_UNIT;
        //    t_str += DefaultMUsAndValues.MUs.DefaultMaterialMU;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._TIME_UNIT;
        //    t_str += DefaultMUsAndValues.MUs.DefaultTimeMU;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._MONEY_UNIT;
        //    t_str += "Euro";//Defaults.MUs.DefaultCurrencyMU;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._DEFAULTS;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._MATERIAL_TYPE;
        //    t_str += Solver_keys._INTERMEDIATE;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._DEF_MAT_LB;
        //    t_str += DefaultMUsAndValues.DefaultValues.d_minimum_flow;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._DEF_MAT_UB;
        //    t_str += DefaultMUsAndValues.DefaultValues.d_maximum_flow;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._DEF_MAT_PRICE;
        //    t_str += DefaultMUsAndValues.DefaultValues.d_price;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._DEF_OPUNIT_LB;
        //    t_str += DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_lower_bound;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._DEF_OPUNIT_UB;
        //    t_str += DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_upper_bound;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._DEF_OPUNIT_FIX;
        //    t_str += "0";
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._DEF_OPUNIT_PROP;
        //    t_str += "0";
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._NEWLINE;
        //    t_str += Solver_keys._MATERIALS;
        //    t_str += Solver_keys._NEWLINE;
        //    sw.Write(t_str);
        //    foreach (MaterialProperties matprop in m_materials.m_rawlist)
        //    {
        //        t_str = Solver_keys._RAW;
        //        t_str += matprop.gmin != def_Values.d_NperA ? ", " + Solver_keys._MAT_LB + matprop.gmin : "";
        //        t_str += matprop.gmax != def_Values.d_NperA ? ", " + Solver_keys._MAT_UB + matprop.gmax : "";
        //        t_str += matprop.gprice != def_Values.d_NperA ? ", " + Solver_keys._MAT_PRICE + matprop.gprice : "";
        //        sw.Write(matprop.currname + ": " + t_str + Solver_keys._NEWLINE);
        //    }
        //    foreach (MaterialProperties matprop in m_materials.m_intermediatelist)
        //    {
        //        if (matprop.gmax != def_Values.d_NperA) sw.Write(matprop.currname + ": " + Solver_keys._INTERMEDIATE + ", " + Solver_keys._MAT_UB + matprop.gmax + Solver_keys._NEWLINE);
        //    }
        //    foreach (MaterialProperties matprop in m_materials.m_productlist)
        //    {
        //        t_str = Solver_keys._PRODUCT;
        //        t_str += matprop.gmin != def_Values.d_NperA ? ", " + Solver_keys._MAT_LB + matprop.gmin : "";
        //        t_str += matprop.gmax != def_Values.d_NperA ? ", " + Solver_keys._MAT_UB + matprop.gmax : "";
        //        t_str += matprop.gprice != def_Values.d_NperA ? ", " + Solver_keys._MAT_PRICE + matprop.gprice : "";
        //        sw.Write(matprop.currname + ": " + t_str + Solver_keys._NEWLINE);
        //    }
        //    t_str = Solver_keys._NEWLINE;
        //    t_str += Solver_keys._OPUNITS;
        //    t_str += Solver_keys._NEWLINE;
        //    sw.Write(t_str);
        //    foreach (OperatingUnitProperties ouprop in m_operatingunitlist)
        //    {
        //        t_str = "";
        //        if (ouprop.bounds.d_lb != DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_lower_bound) t_str = Solver_keys._OPUNIT_LB + ouprop.bounds.d_lb;
        //        if (ouprop.bounds.d_ub != DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_upper_bound)
        //        {
        //            if (t_str != "") t_str += ", ";
        //            t_str += Solver_keys._OPUNIT_UB + ouprop.bounds.d_ub;
        //        }
        //        if (ouprop.overallcost.g_fix != 0)
        //        {
        //            if (t_str != "") t_str += ", ";
        //            t_str += Solver_keys._OPUNIT_FIX + ouprop.overallcost.g_fix;
        //        }
        //        if (ouprop.overallcost.g_prop != 0)
        //        {
        //            if (t_str != "") t_str += ", ";
        //            t_str += Solver_keys._OPUNIT_PROP + ouprop.overallcost.g_prop;
        //        }
        //        if (t_str != "") sw.Write(ouprop.currname + ": " + t_str + Solver_keys._NEWLINE);
        //    }
        //    t_str = Solver_keys._NEWLINE;
        //    t_str += Solver_keys._OPUNIT_RATES;
        //    t_str += Solver_keys._NEWLINE;
        //    sw.Write(t_str);
        //    foreach (OperatingUnitProperties ouprop in m_operatingunitlist)
        //    {
        //        sw.Write(ouprop.currname + ": ");
        //        t_str = "";
        //        foreach (CustomProp prop in ouprop.imats.list)
        //        {
        //            IOMaterial mat = (IOMaterial)prop.Value;
        //            if (t_str != "") t_str += " + ";
        //            if (mat.g_rate != 1) t_str += mat.g_rate + " ";
        //            t_str += mat.Name;
        //        }
        //        sw.Write(t_str + " => ");
        //        t_str = "";
        //        foreach (CustomProp prop in ouprop.omats.list)
        //        {
        //            IOMaterial mat = (IOMaterial)prop.Value;
        //            if (t_str != "") t_str += " + ";
        //            if (mat.g_rate != 1) t_str += mat.g_rate + " ";
        //            t_str += mat.Name;
        //        }
        //        sw.Write(t_str + Solver_keys._NEWLINE);
        //    }
        //    sw.Write(Solver_keys._NEWLINE);
        //    sw.Close();
        //    fs.Close();
        //    Thread.CurrentThread.CurrentCulture = t_original_culture;
        //}

        //private void StartSolver(string t_method)
        //{
        //    s_pns_editor.tabPageSolutions.Text = def_PnsStudio.SolutionsTabTextField;
        //    Solutions m_solutions = new Solutions();
        //    comboBoxSolutionsLeft.Items.Clear();
        //    comboBoxSolutionsRight.Items.Clear();
        //    UpdateSolutionsTree(s_pns_editor.treeViewSolutionsLeft, s_pns_editor.comboBoxSolutionsLeft, true);
        //    UpdateSolutionsTree(s_pns_editor.treeViewSolutionsRight, s_pns_editor.comboBoxSolutionsRight, false);
        //    s_pns_editor.richTextBoxSolutions.Hide();
        //    s_pns_editor.splitContainerSolutions.Show();
        //    m_method = t_method;
        //    m_result_string = new StringBuilder(1000000);
        //    m_startpos = 0;
        //    Thread t_solver = new Thread(new ParameterizedThreadStart(SolverThread));
        //    m_solver_dlg = new SolverDialog(t_solver);
        //    m_timer = new System.Windows.Forms.Timer();
        //    m_timer.Tick += new EventHandler(m_timer_Tick);
        //    m_timer.Interval = 1000;
        //    t_solver.Start(t_method);
        //    m_solver_dlg.Show();
        //    m_timer.Start();
        //    update = false;
        //}


        //--------------------------------------------------------------------------------SOLVER END-----------------------------------------------------------------------

    }
}
