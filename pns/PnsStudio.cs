using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

using Pns.Dialogs;
using Pns.Globals;
using Pns.PnsSolver;
using Pns.Xml_Serialization.PnsProblem;
using Pns.Xml_Serialization.PnsGUI;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;

namespace Pns
{
    public partial class PnsStudio : Form
    {
        #region Members
        static public PnsStudio s_pns_editor = null;
        private PNS m_pns;
        internal string m_filename;
        private string m_cmp_pnsstr;
        private RecentFileList m_recents;
        #endregion

        #region Constructors
        public PnsStudio()
        {
            s_pns_editor = this;
            InitializeComponent();
            TextsFromFile();
            m_ghost = new TreeNodeGhost();
            m_do_drag = false;
            m_selected_mat_tree_node = null;
            m_selected_ou_tree_node = null;
            tabPageProblem.Text = def_PnsStudio.ProblemTabTextField;
            tabPageSolutions.Text = def_PnsStudio.SolutionsTabTextField;
            m_recents = new RecentFileList(defaults.MaxRecentFiles);
            FormClosing += new FormClosingEventHandler(PnsEditor_FormClosing);
            events.MatPropChange += new MatPropEventHandler(events_MatPropChange);
            events.OUPropChange += new OUPropEventHandler(events_OUPropChange);
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                pns_open(args[1]);
            }
            else
            {
                create_new_pns();
            }
        }
        #endregion

        #region Member functions
        private void TextsFromFile()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PnsUserInterfaceTexts));
                FileStream fs = new FileStream(Application.StartupPath + "\\" + defaults.Texts_file, FileMode.Open);
                PnsUserInterfaceTexts t_def = (PnsUserInterfaceTexts)serializer.Deserialize(fs);
                fs.Close();
            }
            catch
            {
                TextsToFile();
                MessageBox.Show(def_PnsStudio.Msg_text_file_missing, def_PnsStudio.Msg_text_file_missing_title);
            }
        }
        
        private void TextsToFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PnsUserInterfaceTexts));
            FileStream fs = new FileStream(Application.StartupPath + "\\" + defaults.Texts_file, FileMode.Create);
            serializer.Serialize(fs, new PnsUserInterfaceTexts());
            fs.Close();
        }

        private void update_caption_recents(bool t_new_problem)
        {
            m_recents.UpdateRecentList(recentFilesToolStripMenuItem, t_new_problem ? "" : m_filename);
            this.Text = m_filename.Substring(m_filename.LastIndexOf('\\') + 1) + " - " + def_PnsStudio.defappname;
        }

        private void create_new_pns()
        {
            new DefaultMUsAndValues(null);
            m_filename = def_PnsStudio.deffilename;
            m_pns = new PNS();
            m_pns.problem = new Problem();
            m_pns.problem.materials = new Materials();
            m_pns.problem.operatingUnits = new OperatingUnit[0];
            DefaultMUsAndValues.ToXML(m_pns.problem);
            StringWriter t_strw = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(PNS));
            serializer.Serialize(t_strw, m_pns);
            m_cmp_pnsstr = t_strw.ToString();
            update_caption_recents(true);
            LoadMaterials(m_pns.problem.materials);
            LoadOperatingUnits(m_pns.problem.operatingUnits);
            PnsStudio.DeleteResults();
        }

        private void update_changes()
        {
            m_pns.problem.materials = m_materials.ToXML();
            m_pns.problem.operatingUnits = m_operatingunitlist.ToXML();
            DefaultMUsAndValues.ToXML(m_pns.problem);
        }

        private bool check_changes()
        {
            update_changes();
            XmlSerializer serializer = new XmlSerializer(typeof(PNS));
            if (m_pns != null)
            {
                StringWriter t_strw = new StringWriter();
                serializer.Serialize(t_strw, m_pns);
                string t_str = t_strw.ToString();
                if (t_str != m_cmp_pnsstr)
                {
                    DialogResult t_ret;
                    if ((t_ret = MessageBox.Show(def_PnsStudio.Msg_file_changed, def_PnsStudio.Msg_file_changed_title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)) == DialogResult.Yes)
                    {
                        pns_save();
                    }
                    else if (t_ret == DialogResult.Cancel) return false;
                }
            }
            return true;
        }

        private void pns_new()
        {
            if (!check_changes()) return;
            create_new_pns();
        }

        private void pns_open_recent(string t_filename)
        {
            if (!check_changes()) return;
            pns_open(t_filename);
        }

        private void pns_open_dialog()
        {
            if (!check_changes()) return;
            if (openFileDialog.ShowDialog() == DialogResult.OK) pns_open(openFileDialog.FileName);
        }

        private void pns_open(string t_filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PNS));
                m_filename = t_filename;
                FileStream fs = new FileStream(m_filename, FileMode.Open);
                m_pns = (PNS)serializer.Deserialize(fs);
                StringWriter t_strw = new StringWriter();
                serializer.Serialize(t_strw, m_pns);
                m_cmp_pnsstr = t_strw.ToString();
                fs.Close();
                update_caption_recents(false);
                new DefaultMUsAndValues(m_pns.problem);
                LoadMaterials(m_pns.problem.materials);
                LoadOperatingUnits(m_pns.problem.operatingUnits);
	        PnsStudio.DeleteResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), def_PnsStudio.Ex_file_open_error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                create_new_pns();
            }
        }

        private void pns_save()
        {
            if (m_filename == def_PnsStudio.deffilename)
            {
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                m_filename = saveFileDialog.FileName;
                update_caption_recents(false);
            }
            pns_savefile();
        }

        private void pns_savefile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PNS));
            FileStream fs = new FileStream(m_filename, FileMode.Create);
            if (m_pns == null) m_pns = new PNS();
            update_changes();
            serializer.Serialize(fs, m_pns);
            fs.Close();
            StringWriter t_strw = new StringWriter();
            serializer.Serialize(t_strw, m_pns);
            m_cmp_pnsstr = t_strw.ToString();
        }
        #endregion

        #region PnsEditor Event Handlers
        private void newToolStripButton_Click(object sender, EventArgs e) { pns_new(); }
        private void newToolStripMenuItem_Click(object sender, EventArgs e) { pns_new(); }
        private void openToolStripButton_Click(object sender, EventArgs e) { pns_open_dialog(); }
        private void openToolStripMenuItem_Click(object sender, EventArgs e) { pns_open_dialog(); }
        private void saveToolStripButton_Click(object sender, EventArgs e) { pns_save(); }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) { pns_save(); }
        private void problemExportToExcelToolStripMenuItem_Click(object sender, EventArgs e) { PNSProblemToExcel(false); }
        private void problemExportToZIMPLToolStripMenuItem_Click(object sender, EventArgs e) { ProblemToZIMPL(); }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { Close(); }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) { new AboutPnsEditor().ShowDialog(); }
        private void helpToolStripButton_Click(object sender, EventArgs e) { new AboutPnsEditor().ShowDialog(); }
        private void defaultToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            new DefaultMU().ShowDialog();
            m_materials.UpdateAllValues();
            m_materials_tmp.UpdateAllValues();
            m_operatingunitlist.UpdateAllValues();
            m_operatingunitlist_tmp.UpdateAllValues();
            UpdateMaterialTreeToolTips();
            UpdateOpUnitTreeToolTips();
            RefreshMaterialGrids();
            RefreshOpUnitGrids();
        }
        private void defaultValuesToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            new DefaultValue().ShowDialog();
            UpdateMaterialTreeToolTips();
            UpdateOpUnitTreeToolTips();
            RefreshMaterialGrids();
            RefreshOpUnitGrids();
        }
        private void saveOptionsToolStripMenuItem_Click(object sender, EventArgs e) { DefaultMUsAndValues.SaveDefaults(); }
        private void PnsEditor_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
            else e.Effect = DragDropEffects.None;
        }
        private void PnsEditor_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files!=null && files.Length > 0)
            {
                if (files.Length > 1) MessageBox.Show(def_PnsStudio.Msg_multi_file_drop, def_PnsStudio.Msg_multi_file_drop_title);
                if (check_changes()) pns_open(files[0]);
            }
        }
        void PnsEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(def_PnsStudio.Msg_close_confirm, def_PnsStudio.Msg_close_confirm_title,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                e.Cancel = !check_changes();
                return;
            }
            e.Cancel = true;
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                m_filename = saveFileDialog.FileName;
                update_caption_recents(false);
                pns_savefile();
            }
        }
        private void recentFilesToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            RecentFile t_recent=e.ClickedItem.Tag as RecentFile;
            if(t_recent!=null)pns_open_recent(t_recent.Path);
        }
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_recents.UpdateRecentList(recentFilesToolStripMenuItem, "");
        }
        private void MSGToolStripMenuItem_Click(object sender, EventArgs e) { StartSolver(Solver_keys._KEY_MSG); }
        private void SSGToolStripMenuItem_Click(object sender, EventArgs e) { StartSolver(Solver_keys._KEY_SSG); }
        private void SSGLPToolStripMenuItem_Click(object sender, EventArgs e) { StartSolver(Solver_keys._KEY_SSGLP); }
        private void ABBToolStripMenuItem_Click(object sender, EventArgs e) { StartSolver(Solver_keys._KEY_ABB); }
        #endregion
    }
}
