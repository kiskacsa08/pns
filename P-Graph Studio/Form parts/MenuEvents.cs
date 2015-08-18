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

using PNSDraw.Dialogs;
using PNSDraw.Excel_export;
using PNSDraw.ZIMPL_export;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PNSDraw.Configuration;

namespace PNSDraw
{
    public partial class Form1 : Form
    {
        #region File menu events
        #region New
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool process = true;
            if (CheckModification())
            {
                DialogResult dr = MessageBox.Show(
                    "The graph has been changed. Do you want to save the changes?",
                    "New",
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
                Graph.Reset();
                pnsCanvas1.Reset();
                pnsCanvas1.GridSize = Config.Instance.GraphSettings.GridSize;
                pnsCanvas1.Refresh();
                LastGraphXML = Graph.ExportToXML();
                ResetViewList();
                CurrentFile = "";
                CreateUndo();
                UpdateTitle();

                RefreshMinimap(); // Azert kell, mert a TextObjectek rajzolaskor szamoljak ki a meretuket
                ZoomToFit();

                UpdateSolutionsTab();
                tabControl1.SelectedTab = tabPage1;

                RefreshTreeViews();

                isProblemExists = false;
            }
        }
        #endregion

        #region Open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                ofd.Filter = "pgsx file|*.pgsx|xml file|*.xml";
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    OnLoad(ofd.FileName);
                }
                redoToolStripMenuItem.Enabled = false;
                undoToolStripMenuItem.Enabled = false;
            }
        }
        #endregion

        #region Save (as)
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnSave();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnSaveAs();
        }
        #endregion

        #region Print
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += pd_PrintPage;
            PrintDialog dialog = new PrintDialog();
            dialog.Document = pd;
            pd.DefaultPageSettings.Landscape = true;
            pd.DocumentName = Path.GetFileNameWithoutExtension(CurrentFile);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }
        #endregion

        #region Export

        #region Export...
        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Rectangle rect = pnsCanvas1.GetCanvasBoundary();
            Size canvasSize = new Size(rect.Size.Width, rect.Size.Height);

            ExportDialog sd = new ExportDialog(canvasSize, CurrentFile, Export, cmbSolutions.Items.Count);
            sd.ShowDialog();
        }

        private void Export(object sender, string filename, ExportExtensions extension, Size size, ExcelExportType excelType)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            switch (extension)
            {
                case ExportExtensions.JPG:
                    sfd.Filter = "JPEG file|*.jpg";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap bmp = new Bitmap(size.Width, size.Height);
                        Graphics g = Graphics.FromImage(bmp);
                        pnsCanvas1.Export(g, size);
                        bmp.Save(sfd.FileName);
                    }
                    break;
                case ExportExtensions.PNG:
                    sfd.Filter = "PNG file|*.png";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap bmp = new Bitmap(size.Width, size.Height);
                        Graphics g = Graphics.FromImage(bmp);
                        pnsCanvas1.Export(g, size);
                        bmp.Save(sfd.FileName);
                    }
                    break;
                case ExportExtensions.SVG:
                    sfd.Filter = "SVG file|*.svg";
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
                    break;
                case ExportExtensions.XLS:
                    ExportToExcel(sender, filename, excelType);
                    break;
                case ExportExtensions.ZIMPL:
                    ProblemToZimpl.ProblemToZIMPL(filename, Graph);
                    break;
            }
        }
        #endregion

        #region Export to pns
        private void exportToPNSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "pns file|*.pns";
            sfd.FileName = Path.GetFileNameWithoutExtension(CurrentFile);
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(file, Encoding.Unicode);
                    string xml = Graph.ExportToPNS();
                    Console.WriteLine(xml);
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
        #endregion

        #endregion

        #region Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #endregion

        #region Edit menu events
        #region Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoUndo();
        }
        #endregion

        #region Redo
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoRedo();
        }
        #endregion

        #region Copy
        private void copy_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoCopy();
        }
        #endregion

        #region Paste
        private void paste_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoPaste();
        }
        #endregion

        #region Duplicate
        private void duplicate_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoDuplicate();
        }
        #endregion
        #endregion

        #region View menu events
        #region Zoom in
        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnsCanvas1.ZoomIn();
        }
        #endregion

        #region Zoom out
        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnsCanvas1.ZoomOut();
        }
        #endregion

        #region Show grid
        private void showGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnsCanvas1.ShowGrid = ((ToolStripMenuItem)sender).Checked;
        }
        #endregion

        #region Snap to grid
        private void snapToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnsCanvas1.SnapToGrid = ((ToolStripMenuItem)sender).Checked;
        }
        #endregion

        #region Lock
        private void lockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnsCanvas1.Locked = !pnsCanvas1.Locked;
            lockToolStripMenuItem.Checked = pnsCanvas1.Locked;
        }
        #endregion

        #region Position of tabs
        private void rightToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panel2.Dock = DockStyle.Right;
            panel2.SendToBack();
            menuStrip1.SendToBack();
            tableLayoutPanel1.BringToFront();
            CheckMenuItem(positionOfTabsToolStripMenuItem, rightToolStripMenuItem1);
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2.Dock = DockStyle.Left;
            panel2.SendToBack();
            menuStrip1.SendToBack();
            tableLayoutPanel1.BringToFront();
            CheckMenuItem(positionOfTabsToolStripMenuItem, leftToolStripMenuItem);
        }
        #endregion
        #endregion

        #region Preferences menu events
        #region Solver settings
        private void solverSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolverSettingsDialog ssd = new SolverSettingsDialog();
            ssd.ShowDialog();
            UpdateTitle();
            UpdateAlgorithmComboBox();
        }

        public void UpdateAlgorithmComboBox()
        {
            if (Config.Instance.SolverSettings.IsOnlineSolver)
            {
                toolStripComboBox2.Items.Clear();
                toolStripComboBox2.Items.Add("ABB");
                toolStripComboBox2.Items.Add("SSG");
                toolStripComboBox2.SelectedIndex = 0;
            }
            else
            {
                toolStripComboBox2.Items.Clear();
                toolStripComboBox2.Items.Add("ABB");
                toolStripComboBox2.Items.Add("SSG");
                toolStripComboBox2.Items.Add("SSG+LP");
                toolStripComboBox2.Items.Add("MSG");
                toolStripComboBox2.SelectedIndex = 0;
            }
        }

        #endregion

        #region Graph settings
        private void graphSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();

            sw.ShowDialog();

            Graph.RefreshEdgesTitleFormat();

            if (sw.Changed)
            {
                if (LockedMode)
                {
                    solutionComboBox_SelectedIndexChanged(this, EventArgs.Empty);
                }
                else
                {
                    pnsCanvas1.GridSize = Config.Instance.GraphSettings.GridSize;
                    pnsCanvas1.Refresh();
                    propertyGrid1.Refresh();
                }
            }
        }
        #endregion

        #region Problem settings
        private void problemSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProblemSettingsDialog psd = new ProblemSettingsDialog();
            DialogResult res = psd.ShowDialog();
            if (res == DialogResult.OK)
            {
                Graph.UpdateParameterLabels();
                propertyGrid1.Refresh();
                pnsCanvas1.Refresh();
                RefreshTreeViews();                
            }
        }
        #endregion

        #region Solution settings
        private void solutionSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolutionSettingsWindow sw = new SolutionSettingsWindow();

            sw.ShowDialog();

            if (sw.Changed)
            {
                solutionComboBox_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Layout settings
        private void layoutSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutSettingsWindow lw = new LayoutSettingsWindow();
            lw.ShowDialog();

            if (lw.Changed)
            {
                if (LockedMode)
                {
                    solutionComboBox_SelectedIndexChanged(this, EventArgs.Empty);
                }
                else
                {
                    pnsCanvas1.Refresh();
                    propertyGrid1.Refresh();
                }
            }
        }
        #endregion
        #endregion

        #region Help menu events
        #region About
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutPGraphStudio ab = new AboutPGraphStudio();
            ab.ShowDialog();
        }
        #endregion
        #endregion
    }
}