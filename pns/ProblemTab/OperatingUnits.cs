using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Pns.Xml_Serialization.PnsProblem;
using Pns.Xml_Serialization.PnsGUI.MessageBoxes;
using Pns.Xml_Serialization.PnsGUI.ExceptionMsgs;
using Pns.Xml_Serialization.PnsGUI.TreeViews;
using Pns.Globals;
using Pns.Dialogs;

namespace Pns
{
    public class OperatingUnitPropertiesList : List<OperatingUnitProperties>
    {
        #region Members
        private string m_cmp_name;
        #endregion

        #region Constructors
        public OperatingUnitPropertiesList()
        {
        }
        public OperatingUnitPropertiesList(OperatingUnit[] ous)
        {
            if (ous != null)
            {
                foreach (OperatingUnit ou in ous)
                {
                    if (Find(ou.name) != null) throw new Exception(def_ou_ex.Ex_more_inctance_p1 + ou.name + def_ou_ex.Ex_more_inctance_p2);
                    Add(new OperatingUnitProperties(ou));
                }
            }
        }
        #endregion

        #region Member functions
        public void BuildTree(TreeNode t_node)
        {
            foreach (OperatingUnitProperties ou in this)
            {
                ou.BuildTree(t_node.Nodes.Add(ou.currname, ou.currname));
            }
        }
        public OperatingUnit[] ToXML()
        {
            OperatingUnit[] ous = new OperatingUnit[Count];
            int i = 0;
            foreach (OperatingUnitProperties ouprop in this)
            {
                ous[i++] = ouprop.ToXML();
            }
            return ous;
        }
        /// <summary>
        /// A listába beilleszti a t_node-nak megfelelő elem elé a t_ouprop elemet. Ha a t_node-hoz
        /// nem tartozik elem a listában, akkor a következő node-ot veszi. Ha nem sikerül beszúrni,
        /// akkor a lista végéhez adja az elemet.
        /// </summary>
        public void InsertAtNode(TreeNode t_node, OperatingUnitProperties t_ouprop)
        {
            int i;
            bool t_inserted = false;
            while (!t_inserted && t_node != null && t_node.Name != "New")
            {
                if (t_inserted = ((i = FindIndex(t_node.Name)) > -1)) Insert(i, t_ouprop);
                else t_node = t_node.NextNode;
            }
            if (!t_inserted) Add(t_ouprop);
        }
        public OperatingUnitProperties Remove(string t_name)
        {
            int i;
            OperatingUnitProperties t_ouprop = null;
            if ((i = FindIndex(t_name)) > -1)
            {
                t_ouprop = this[i];
                RemoveAt(i);
            }
            return t_ouprop;
        }
        public void UpdateFlowMaterialName(string oldname, string newname)
        {
            foreach (OperatingUnitProperties ouprop in this)
            {
                ouprop.UpdateFlowMaterialName(oldname, newname);
            }
        }
        public void UpdateFlowMaterialCategory(string name, int t_choise)
        {
            foreach (OperatingUnitProperties ouprop in this)
            {
                ouprop.UpdateFlowMaterialCategory(name, t_choise);
            }
        }
        public void UpdateAllValues()
        {
            foreach (OperatingUnitProperties ouprop in this)  ouprop.UpdateAllValues(); 
        }
        private bool operatingunitMatch(OperatingUnitProperties ouprop)
        {
            return ouprop.currname == m_cmp_name;
        }
        public int FindIndex(string name)
        {
            m_cmp_name = name;
            return base.FindIndex(operatingunitMatch);
        }
        public OperatingUnitProperties Find(string name)
        {
            m_cmp_name = name;
            return base.Find(operatingunitMatch);
        }
        public override string ToString() { return ""; }
        #endregion
    }

    public partial class PnsStudio : Form
    {
        #region Members (operating units)
        private OperatingUnitPropertiesList m_operatingunitlist;
        private OperatingUnitPropertiesList m_operatingunitlist_tmp;
        private TreeNode m_dragover_node;
        private int m_dragover_tick_counter;
        private bool m_over_outree;
        #endregion

        #region Member functions
        /// <summary>
        /// New és Open esetén hívódik meg. Input XML alapján feltölti az alábbi elemeket:
        /// - OperatingUnitTreeView,
        /// - operating unit list,
        /// </summary>
        private void LoadOperatingUnits(OperatingUnit[] ous)
        {
            m_operatingunitlist = new OperatingUnitPropertiesList(ous);
            m_operatingunitlist_tmp = new OperatingUnitPropertiesList();
            foreach (OperatingUnitPanel t_oupanel in tableLayoutPanelOperatingUnitProperties.Controls)
            {
                events.OperatingUnitPanelChange -= new SelectOperatingUnitPanelEventHandler(t_oupanel.events_OperatingUnitPanelChange);
                t_oupanel.Dispose();
            }
            tableLayoutPanelOperatingUnitProperties.Controls.Clear();
            treeViewOperatingUnits.Hide();
            treeViewOperatingUnits.Nodes.Clear();
            TreeNode t_node = treeViewOperatingUnits.Nodes.Add(def_ou_tree.OperatingUnitsName, def_ou_tree.OperatingUnitsText);
            m_operatingunitlist.BuildTree(t_node);
            t_node.Nodes.Add(def_ou_tree.NewName, def_ou_tree.NewText);
            treeViewOperatingUnits.Nodes[0].Expand();
            treeViewOperatingUnits.Show();
            UpdateOpUnitTreeToolTips();
        }

        /// <summary>
        /// A végleges és az ideiglenes műveleti egység listákban név alapján keres, ha megtalálja, true-val tér visza.
        /// </summary>
        private bool OperatingUnitExists(string t_name)
        {
            return m_operatingunitlist.Find(t_name) != null || m_operatingunitlist_tmp.Find(t_name) != null;
        }

        /// <summary>
        /// Ha valamely anyagot törölni akarjuk, akkor ellenőrzi az Opunit fában, hogy az anyag használatban van-e.
        /// </summary>
        private bool MaterialInUse(string name, Color clr, bool ensurevisible)
        {
            List<TreeNode> t_flow_nodes = OUFlowList(null, name);
            foreach (TreeNode t_node in t_flow_nodes)
            {
                t_node.ForeColor = clr;
                if (ensurevisible) t_node.EnsureVisible();
            }
            return t_flow_nodes.Count > 0;
        }

        /// <summary>
        /// Ha valamely anyag neve megváltozik, akkor ezt update-eli a műveleti egységek IOMaterials anyagaiban és az Opunit fában.
        /// </summary>
        private void UpdateFlowMaterialName(string oldname, string newname)
        {
            m_operatingunitlist.UpdateFlowMaterialName(oldname, newname);
            m_operatingunitlist_tmp.UpdateFlowMaterialName(oldname, newname);
            List<TreeNode> t_nodes = OUFlowList(null, oldname);
            foreach (TreeNode t_node in t_nodes)t_node.Name = newname;            
        }

        /// <summary>
        /// Ha valamely anyag kategóriája megváltozik, akkor ezt update-eli a műveleti egységek IOMaterials anyagaiban és az Opunit fában.
        /// </summary>
        private void UpdateFlowMaterialCategory(string name, int t_choise)
        {
            m_operatingunitlist.UpdateFlowMaterialCategory(name, t_choise);
            m_operatingunitlist_tmp.UpdateFlowMaterialCategory(name, t_choise);
        }
        private void UpdateOpUnitTree(string name)
        {
            List<TreeNode> t_nodes = OUFlowList(null, name);
            foreach (TreeNode t_node in t_nodes)
            {
                if (TreeNodeType(t_node.Parent.Parent) != defaults.TreeNodeTypes.opunit) throw new Exception(def_PnsStudio_ex.Ex_ou_node_not_found);
                if (TreeNodeType(t_node.Parent) != defaults.TreeNodeTypes.io_flows) throw new Exception(def_PnsStudio_ex.Ex_ioflows_node_not_found);
                OperatingUnitProperties t_ouprop = m_operatingunitlist.Find(t_node.Parent.Parent.Name);
                if (t_ouprop == null) t_ouprop = m_operatingunitlist_tmp.Find(t_node.Parent.Parent.Name);
                IOMaterials t_mats;
                if (t_node.Parent.Name == def_ou_tree.InputMaterialsName) t_mats = t_ouprop.imats;
                else t_mats = t_ouprop.omats;
                t_mats.BuildTree(t_node.Parent);
            }
        }
        private void UpdateOpUnitTreeToolTips()
        {
            foreach (TreeNode t_node in treeViewOperatingUnits.Nodes.Find(def_ou_tree.OperatingUnitsName, true)[0].Nodes)
            {
                OperatingUnitProperties t_ouprop = m_operatingunitlist.Find(t_node.Name);
                if (t_ouprop != null)
                {
                    FindIOFlowsNode(t_node, true).ToolTipText = t_ouprop.imats.ToString();
                    FindIOFlowsNode(t_node, false).ToolTipText = t_ouprop.omats.ToString();
                    t_node.ToolTipText = t_ouprop.ToString();
                }
            }
        }
        /// <summary>
        /// Egyedi műveleti egység nevet generál new és clone számára.
        /// </summary>
        private string GenOUName(string namebase, string concat)
        {
            int i = 0;
            string name;
            do
            {
                i++;
                name = namebase + concat + i.ToString();
            } while (OperatingUnitExists(name));
            return name;
        }
        private void RefreshOpUnitGrid(string t_name)
        {
            OperatingUnitPanel t_oupanel = FindOUPanel(t_name);
            if (t_oupanel != null)
            {
                PropertyGrid t_prop_grid = t_oupanel.OUPropGrid;
                OperatingUnitProperties t_ouprop = (OperatingUnitProperties)t_prop_grid.SelectedObject;
                t_ouprop.Refresh();
                t_prop_grid.Refresh();
                t_prop_grid.ExpandAllGridItems();
            }
        }
        private void RefreshOpUnitGrids()
        {
            foreach (Control item in tableLayoutPanelOperatingUnitProperties.Controls)
            {
                PropertyGrid t_prop_grid = ((OperatingUnitPanel)item).OUPropGrid;
                OperatingUnitProperties t_ouprop = (OperatingUnitProperties)t_prop_grid.SelectedObject;
                t_ouprop.Refresh();
                t_prop_grid.Refresh();
            }
        }
        static public OperatingUnitProperties FindOperatingUnit(string t_name)
        {
            if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
            OperatingUnitProperties t_ouprop = s_pns_editor.m_operatingunitlist.Find(t_name);
            if (t_ouprop == null) throw new Exception(def_PnsStudio_ex.Ex_opunit_not_found);
            return t_ouprop;
        }
        private void CreateNewOpUnit(TreeNode t_node)
        {
            string name = Converters.ToNameString(GenOUName(def_ou_tree.GenBaseName, def_ou_tree.GenBaseConcat));
            OperatingUnitProperties ou = new OperatingUnitProperties(name);
            TreeNode t_ou_node;
            ou.BuildTree(t_ou_node = t_node.Parent.Nodes.Insert(t_node.Index, name, name));
            t_ou_node.Expand();
            m_operatingunitlist_tmp.Add(ou);
            OperatingUnitPanel oupanel;
            tableLayoutPanelOperatingUnitProperties.Controls.Add(oupanel = new OperatingUnitPanel(ou));
            OUPanelEnsureVisible(oupanel);
        }
        static internal void OUPanelEnsureVisible(OperatingUnitPanel t_oupanel)
        {
            t_oupanel.OUPropLabel.Focus();
            t_oupanel.OUPropUpdate.Focus();
            t_oupanel.OUPropGrid.Focus();
        }
        private void deleteIOFlow(TreeNode t_ioflow_node)
        {
            FindRemoveIOFlow(t_ioflow_node, true);
        }
        private IOMaterial FindRemoveIOFlow(TreeNode t_ioflow_node, bool t_remove)
        {
            OperatingUnitProperties t_ouprop = m_operatingunitlist.Find(t_ioflow_node.Parent.Parent.Name);
            OperatingUnitProperties t_ouprop_tmp = m_operatingunitlist_tmp.Find(t_ioflow_node.Parent.Parent.Name);
            IOMaterial t_iomat = null;
            if (t_ioflow_node.Parent.Name == def_ou_tree.InputMaterialsName)
            {
                if (t_ouprop_tmp != null) t_iomat = (IOMaterial)t_ouprop_tmp.imats.FindRemove(t_ioflow_node.Name, t_remove).Value;
                if (t_ouprop != null) t_iomat = (IOMaterial)t_ouprop.imats.FindRemove(t_ioflow_node.Name, t_remove).Value;
            }
            else
            {
                if (t_ouprop_tmp != null) t_iomat = (IOMaterial)t_ouprop_tmp.omats.FindRemove(t_ioflow_node.Name, t_remove).Value;
                if (t_ouprop != null) t_iomat = (IOMaterial)t_ouprop.omats.FindRemove(t_ioflow_node.Name, t_remove).Value;
            }
            if (t_remove)
            {
                UpdateOpUnitTreeToolTips();
                RefreshOpUnitGrid(t_ioflow_node.Parent.Parent.Name);
                t_ioflow_node.Remove();
            }
            return t_iomat;
        }
        private void insertIOFlow(TreeNode t_target_node, defaults.TreeNodeTypes t_target_node_type, TreeNode t_source_node)
        {
            insertIOFlow(t_target_node, t_target_node_type, t_source_node, null);
        }
        private void insertIOFlow(TreeNode t_target_node, defaults.TreeNodeTypes t_target_node_type, IOMaterial t_ioflow)
        {
            insertIOFlow(t_target_node, t_target_node_type, null, t_ioflow);
        }
        private void insertIOFlow(TreeNode t_target_node, defaults.TreeNodeTypes t_target_node_type, TreeNode t_source_node, IOMaterial t_ioflow)
        {
            int t_index = t_target_node.Index;
            TreeNode ioflows_node = t_target_node;
            if (t_target_node_type == defaults.TreeNodeTypes.io_flow) ioflows_node = t_target_node.Parent;
            else t_index = t_target_node.Nodes.Count;
            OperatingUnitProperties t_ouprop, t_ouprop_tmp;
            t_ouprop = m_operatingunitlist.Find(ioflows_node.Parent.Name);
            t_ouprop_tmp = m_operatingunitlist_tmp.Find(ioflows_node.Parent.Name);
            IOMaterial t_iomat=null;
            if (t_ouprop != null) t_iomat = t_ouprop.InsertIOMaterial(ioflows_node.Name, t_source_node, t_index, t_ioflow);
            if (t_ouprop_tmp != null) t_iomat = t_ouprop_tmp.InsertIOMaterial(ioflows_node.Name, t_source_node, t_index, t_ioflow);
            ioflows_node.Nodes.Insert(t_index, t_iomat.Name, t_iomat.ToString());
            ioflows_node.Expand();
            UpdateOpUnitTreeToolTips();
            RefreshOpUnitGrid(ioflows_node.Parent.Name);
        }
        #endregion

        #region OperatingUnitPanel Update, Cancel, Delete eseményei
        static internal TreeNode FindOUNode(string t_ouname)
        {
            if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
            TreeNode[] t_nodes = s_pns_editor.treeViewOperatingUnits.Nodes.Find(t_ouname, true);
            foreach (TreeNode t_node in t_nodes) if (TreeNodeType(t_node)==defaults.TreeNodeTypes.opunit && t_node.Name==t_ouname) return t_node;
            throw new Exception(def_PnsStudio_ex.Ex_opunit_not_found);
        }
        static internal TreeNode FindIOFlowsNode(TreeNode t_ou_node, bool t_iflows)
        {
            if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
            string t_key = t_iflows ? def_ou_tree.InputMaterialsName : def_ou_tree.OutputMaterialsName;
            TreeNode[] t_nodes = t_ou_node.Nodes.Find(t_key, true);
            foreach (TreeNode t_node in t_nodes) if (TreeNodeType(t_node) == defaults.TreeNodeTypes.io_flows && t_node.Name == t_key) return t_node;
            return null;
        }
        static internal List<TreeNode> OUFlowList(TreeNode t_ou_node, string t_mat_name)
        {
            if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
            TreeNode[] t_nodes;
            List<TreeNode> t_ret=new List<TreeNode>();
            if (t_ou_node != null) t_nodes = t_ou_node.Nodes.Find(t_mat_name, true);
            else t_nodes = s_pns_editor.treeViewOperatingUnits.Nodes.Find(t_mat_name, true);
            foreach (TreeNode t_node in t_nodes) if (TreeNodeType(t_node) == defaults.TreeNodeTypes.io_flow && t_node.Name == t_mat_name) t_ret.Add(t_node);
            return t_ret;
        }
        private OperatingUnitPanel FindOUPanel(string t_name)
        {
            Control[] controls = tableLayoutPanelOperatingUnitProperties.Controls.Find(t_name, false);
            foreach (OperatingUnitPanel t_oupanel in controls) if (t_oupanel.Name == t_name) return t_oupanel;
            return null;
        }
        /// <summary>
        /// Az OperatingUnitProperty panel Update gomb megnyomására hajtódik végre.
        /// </summary>
        private bool operatingunitUpdate(OperatingUnitProperties ouprop)
        {
            TreeNode node;
            OperatingUnitProperties ou;

            if ((ou = m_operatingunitlist.Find(ouprop.name)) != null && ou.currname != ouprop.currname)
            {
                MessageBox.Show(def_ou_msg.Msg_ou_exists_p1 + ouprop.name + def_ou_msg.Msg_ou_exists_p2,
                    def_ou_msg.Msg_ou_exists_title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if ((ou = m_operatingunitlist_tmp.Find(ouprop.name)) != null && ou.currname != ouprop.currname)
            {
                node = FindOUNode(ou.currname);
                string name = GenOUName(def_ou_tree.GenBaseName, def_ou_tree.GenBaseConcat);
                OperatingUnitPanel panel = FindOUPanel(ou.currname);
                panel.labelText = name;
                panel.Name = name;
                panel.cmp_currname = name;
                ou.currname = name;
                node.Name = name;
                node.Text = name;
            }
            m_operatingunitlist_tmp.Remove(ouprop);
            node = FindOUNode(ouprop.currname);
            node.Name = ouprop.name;
            node.Text = ouprop.name;
            ouprop.BuildTree(node);
            ouprop.isnewitem = false;
            int i = m_operatingunitlist.FindIndex(ouprop.currname);
            ouprop.currname = ouprop.name;
            if (i > -1) m_operatingunitlist[i] = ouprop;
            else m_operatingunitlist.InsertAtNode(node.NextNode, ouprop);
            UpdateOpUnitTreeToolTips();
            return true;
        }

        /// <summary>
        /// Az OperatingUnitProperty panel Cancel gomb megnyomására hajtódik végre.
        /// </summary>
        private void operatingunitCancel(OperatingUnitProperties ouprop)
        {
            m_operatingunitlist_tmp.Remove(ouprop);
            if (m_operatingunitlist.FindIndex(ouprop.currname) == -1) FindOUNode(ouprop.currname).Remove();
        }

        /// <summary>
        /// Az OperatingUnitProperty panel Delete gomb megnyomására hajtódik végre.
        /// </summary>
        private bool operatingunitDelete(OperatingUnitProperties ouprop)
        {
            if (MessageBox.Show(def_ou_delete_confirm_msg.Msg_delete_confirm, def_ou_delete_confirm_msg.Msg_delete_confirm_title, MessageBoxButtons.YesNo) == DialogResult.No) return false;
            m_operatingunitlist_tmp.Remove(ouprop);
            FindOUNode(ouprop.currname).Remove();
            int i = m_operatingunitlist.FindIndex(ouprop.currname);
            if (i > -1) m_operatingunitlist.RemoveAt(i);
            return true;
        }

        /// <summary>
        /// Az OperatingUnitProperty panel Update, Cancel vagy Delete gomb megnyomására hajtódik végre.
        /// </summary>
        void events_OUPropChange(object sender, OperatingUnitPropertyEventArgs e)
        {
            switch (e.buttonclick)
            {
                case defaults.OUPropButtons.update:
                    if (!operatingunitUpdate(e.ouprop)) return;
                    break;
                case defaults.OUPropButtons.cancel:
                    operatingunitCancel(e.ouprop); break;
                case defaults.OUPropButtons.delete:
                    if (!operatingunitDelete(e.ouprop)) return;
                    break;
            }
            events.OperatingUnitPanelChange -= new SelectOperatingUnitPanelEventHandler(((OperatingUnitPanel)sender).events_OperatingUnitPanelChange);
            tableLayoutPanelOperatingUnitProperties.Controls.Remove((Control)sender);
            ((Control)sender).Dispose();
            treeViewOperatingUnits.Focus();
        }
        #endregion

        #region Operating unit tree modify, clone, delete
        /// <summary>
        /// A műveleti egység módosításának kezdetén létrehozza illetve megkeresi a műveleti egységhez tartozó property panelt.
        /// </summary>
        private void modifyOperatingUnit(TreeNode node)
        {
            OperatingUnitProperties ou, tmpou;
            OperatingUnitPanel oupanel;
            if ((ou = m_operatingunitlist_tmp.Find(node.Name)) !=null)
            {
                OUPanelEnsureVisible(FindOUPanel(ou.currname));
                return;
            }
            ou = m_operatingunitlist.Find(node.Name);
            m_operatingunitlist_tmp.Add(tmpou = new OperatingUnitProperties(ou));
            tableLayoutPanelOperatingUnitProperties.Controls.Add(oupanel = new OperatingUnitPanel(tmpou));
            OUPanelEnsureVisible(oupanel);
        }

        private void cloneOperatingUnit(TreeNode t_node)
        {
            int i;
            if ((i = m_operatingunitlist.FindIndex(t_node.Name)) > -1)
            {
                OperatingUnitProperties t_ou = new OperatingUnitProperties(m_operatingunitlist[i]);
                t_ou.currname = t_ou.name = GenOUName(t_ou.name, def_ou_tree.GenCloneConcat);
                m_operatingunitlist.Insert(i + 1, t_ou);
                t_ou.BuildTree(t_node.Parent.Nodes.Insert(t_node.Index + 1, t_ou.name, t_ou.name));
                UpdateOpUnitTreeToolTips();
            }
            else MessageBox.Show(def_ou_msg.Msg_can_not_clone_temporary);
        }

        private void deleteOperatingUnit(string t_name)
        {
            if (MessageBox.Show(def_ou_delete_confirm_msg.Msg_delete_confirm, def_ou_delete_confirm_msg.Msg_delete_confirm_title, MessageBoxButtons.YesNo) == DialogResult.No) return;
            TreeNode t_node = FindOUNode(t_name);
            OperatingUnitPanel t_oupanel = FindOUPanel(t_name);
            if (t_oupanel!=null)
            {
                OperatingUnitProperties ouprop = (OperatingUnitProperties)t_oupanel.OUPropGrid.SelectedObject;
                m_operatingunitlist_tmp.Remove(ouprop);
                events.OperatingUnitPanelChange -= new SelectOperatingUnitPanelEventHandler(t_oupanel.events_OperatingUnitPanelChange);
                tableLayoutPanelOperatingUnitProperties.Controls.Remove(t_oupanel);
                t_oupanel.Dispose();
            }
            m_operatingunitlist.Remove(t_name);
            t_node.Remove();
        }
        #endregion

        #region Operating unit tree node mouse click
        /// <summary>
        /// A műveleti egységek fára történő egérkattintás kezelője.
        /// </summary>
        private void OperatingUnit_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!IsTreeNodeArea(e.Node, e.Location))
            {
                if (e.Button == MouseButtons.Right) contextMenuStripOperatingUnitTree.Show((Control)sender, e.Location);
                return;
            }
            defaults.TreeNodeTypes t_nodetype = TreeNodeType(e.Node);
            if (t_nodetype == defaults.TreeNodeTypes.new_opunit) CreateNewOpUnit(e.Node);
            else if (t_nodetype == defaults.TreeNodeTypes.opunit)
            {
                // Modify
                modifyOperatingUnit(e.Node);
                // Dropdown menu
                if (e.Button == MouseButtons.Right)
                {
                    e.Node.TreeView.SelectedNode = e.Node;
                    contextMenuStriptreeviewOperatingUnit.Show((Control)sender, e.Location);
                }
            }
        }
        #endregion

        #region Operating units tree Drag & Drop eseménykezelői
        private void OperatingUnit_ItemDrag(TreeNode t_node)
        {
            defaults.TreeNodeTypes t_node_type = TreeNodeType(t_node);
            if (t_node_type == defaults.TreeNodeTypes.opunit || t_node_type == defaults.TreeNodeTypes.io_flow)
            {
                Timer t_timer = new Timer();
                t_timer.Tick += new EventHandler(t_timer_Tick);
                t_timer.Interval = 100;
                t_timer.Enabled = true;
                t_timer.Tag = t_node;
                m_dragover_node = null;
                m_dragover_tick_counter = 0;
                m_over_outree = false;
                m_ghost.Show(t_node);
                m_do_drag = true;
                m_selected_mat_tree_node = treeViewMaterials.SelectedNode;
                m_selected_ou_tree_node = treeViewOperatingUnits.SelectedNode;
                treeViewOperatingUnits.DoDragDrop(t_node, DragDropEffects.All);
                treeViewMaterials.SelectedNode = m_selected_mat_tree_node;
                treeViewOperatingUnits.SelectedNode = m_selected_ou_tree_node;
                m_do_drag = false;
                m_ghost.DoPainting = false;
                t_timer.Enabled = false;
            }
        }

        void t_timer_Tick(object sender, EventArgs e)
        {
            if (m_over_outree)
            {
                TreeNode t_source_node = (TreeNode)((Timer)sender).Tag;
                defaults.TreeNodeTypes t_source_node_type = TreeNodeType(t_source_node);
                Point t_point = treeViewOperatingUnits.PointToClient(new Point(MousePosition.X, MousePosition.Y));
                TreeNode t_node = treeViewOperatingUnits.GetNodeAt(t_point);
                if (t_node != null && IsTreeNodeArea(t_node, t_point))
                {
                    defaults.TreeNodeTypes t_node_type = TreeNodeType(t_node);
                    if (m_dragover_node != t_node)
                    {
                        m_dragover_node = t_node;
                        m_dragover_tick_counter = 0;
                    }
                    else if (!t_node.IsExpanded && ++m_dragover_tick_counter > 9) t_node.ExpandAll();
                }
            }
        }

        private void OperatingUnit_DragOver(object sender, DragEventArgs e)
        {
            if (!m_do_drag) return;
            e.Effect = DragDropEffects.None;
            treeViewOperatingUnits.Focus();
            m_ghost.Location = new Point(e.X, e.Y);
            TreeView t_tree = (TreeView)sender;
            Point t_point = t_tree.PointToClient(new Point(e.X, e.Y));
            TreeNode t_node = t_tree.GetNodeAt(t_point);
            TreeNode t_source_node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            defaults.TreeNodeTypes t_source_node_type = TreeNodeType(t_source_node);
            defaults.TreeNodeTypes t_node_type = TreeNodeType(t_node);
            if (t_node_type != defaults.TreeNodeTypes.none)
            {
                if (t_point.Y < 8)
                {
                    if (t_node.PrevVisibleNode != null)
                    {
                        t_node.PrevVisibleNode.EnsureVisible();
                        System.Threading.Thread.Sleep(50);
                    }
                }
                else if (t_tree.ClientSize.Height - t_point.Y < 8)
                {
                    if (t_node.NextVisibleNode != null)
                    {
                        t_node.NextVisibleNode.EnsureVisible();
                        System.Threading.Thread.Sleep(50);
                    }
                }
                if (IsTreeNodeArea(t_node, t_point))
                {
                    t_tree.SelectedNode = t_node;
                    if (t_source_node_type == defaults.TreeNodeTypes.material)
                    {
                        if (m_materials.Find(t_source_node.Name) != null)
                        {
                            TreeNode t_ou_node;
                            if (t_node_type == defaults.TreeNodeTypes.io_flow) t_ou_node = t_node.Parent.Parent;
                            else if (t_node_type == defaults.TreeNodeTypes.io_flows) t_ou_node = t_node.Parent;
                            else return;
                            if (OUFlowList(t_ou_node, t_source_node.Name).Count==0) e.Effect = DragDropEffects.Copy;
                        }
                    }
                    else if (t_source_node_type == defaults.TreeNodeTypes.opunit &&
                        (t_node_type == defaults.TreeNodeTypes.opunit || t_node_type == defaults.TreeNodeTypes.new_opunit) &&
                        t_node != t_source_node && t_node.Index != t_source_node.Index + 1) e.Effect = DragDropEffects.Move;
                    else if (t_source_node_type == defaults.TreeNodeTypes.io_flow)
                    {
                        if(t_node_type == defaults.TreeNodeTypes.io_flow)
                        {
                            if (t_node.Parent == t_source_node.Parent)
                            {
                                if (t_node != t_source_node && t_node.Index != t_source_node.Index + 1) e.Effect = DragDropEffects.Move;
                            }
                            else
                            {
                                if (t_node.Parent.Parent == t_source_node.Parent.Parent)
                                {
                                    e.Effect = DragDropEffects.Move;
                                }
                                else
                                {
                                    if (OUFlowList(t_node.Parent.Parent, t_source_node.Name).Count == 0)
                                    {
                                        if (ModifierKeys == Keys.Control) e.Effect = DragDropEffects.Copy;
                                        else e.Effect = DragDropEffects.Move;
                                    }
                                }
                            }
                        }
                        else if (t_node_type == defaults.TreeNodeTypes.io_flows)
                        {
                            if (t_node == t_source_node.Parent)
                            {
                                if (t_source_node != t_node.LastNode) e.Effect = DragDropEffects.Move;
                            }
                            else
                            {
                                if (t_node.Parent == t_source_node.Parent.Parent)
                                {
                                    e.Effect = DragDropEffects.Move;
                                }
                                else
                                {
                                    if (OUFlowList(t_node.Parent, t_source_node.Name).Count == 0)
                                    {
                                        if (ModifierKeys == Keys.Control) e.Effect = DragDropEffects.Copy;
                                        else e.Effect = DragDropEffects.Move;
                                    }
                                }
                            }
                        }
                    }
                    return;
                }
            }
            treeViewOperatingUnits.SelectedNode = m_selected_ou_tree_node;
        }

        private void OperatingUnit_DragDrop(object sender, DragEventArgs e)
        {
            TreeView t_tree = (TreeView)sender;
            TreeNode t_node = t_tree.GetNodeAt(t_tree.PointToClient(new Point(e.X, e.Y)));
            TreeNode t_source_node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            defaults.TreeNodeTypes t_source_node_type = TreeNodeType(t_source_node);
            defaults.TreeNodeTypes t_node_type = TreeNodeType(t_node);
            if (t_source_node_type == defaults.TreeNodeTypes.opunit && e.Effect == DragDropEffects.Move)
            {
                OperatingUnitProperties t_ouprop = m_operatingunitlist.Remove(t_source_node.Name);
                if (t_ouprop != null) m_operatingunitlist.InsertAtNode(t_node, t_ouprop);
                OperatingUnitPanel oupanel = FindOUPanel(t_source_node.Name);
                if (oupanel != null)
                {
                    t_ouprop = (OperatingUnitProperties)oupanel.OUPropGrid.SelectedObject;
                    m_operatingunitlist_tmp.Remove(t_ouprop);
                    oupanel.OUPropGrid.Refresh();
                    m_operatingunitlist_tmp.InsertAtNode(t_node, t_ouprop);
                }
                t_source_node.Remove();
                t_node.Parent.Nodes.Insert(t_node.Index, t_source_node);
            }
            else if (t_source_node_type == defaults.TreeNodeTypes.material && e.Effect == DragDropEffects.Copy)
            {
                insertIOFlow(t_node, t_node_type, t_source_node);
            }
            else if (t_source_node_type == defaults.TreeNodeTypes.io_flow)
            {
                insertIOFlow(t_node, t_node_type, FindRemoveIOFlow(t_source_node, e.Effect == DragDropEffects.Move));
            }
        }
        #endregion

        #region Properties
        static internal TreeView OUTree
        {
            get
            {
                if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
                return s_pns_editor.treeViewOperatingUnits;
            }
        }
        #endregion
    }
}
