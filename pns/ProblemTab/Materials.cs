using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Pns.Xml_Serialization.PnsProblem;
using Pns.Xml_Serialization.PnsGUI.MessageBoxes;
using Pns.Xml_Serialization.PnsGUI.ExceptionMsgs;
using Pns.Xml_Serialization.PnsGUI.TreeViews;
using Pns.Globals;
using Pns.Dialogs;

namespace Pns
{

    public partial class PnsStudio : Form
    {
        #region Members (materials)
        private AllTypeMaterials m_materials;
        private AllTypeMaterials m_materials_tmp;
        #endregion

        #region Member functions (materials)
        /// <summary>
        /// New és Open esetén hívódik meg. Input XML alapján feltölti az alábbi elemeket:
        /// - MaterialTreeView,
        /// - raw material list,
        /// - intermediate material list,
        /// - product material list.
        /// </summary>
        public void LoadMaterials(Materials mats)
        {
            m_materials = new AllTypeMaterials(mats);
            m_materials_tmp = new AllTypeMaterials();
            foreach (MaterialPanel t_matpanel in tableLayoutPanelMaterialProperties.Controls)
            {
                events.MaterialPanelChange -= new SelectMaterialPanelEventHandler(t_matpanel.events_MaterialPanelChange);
                t_matpanel.Dispose();
            }
            tableLayoutPanelMaterialProperties.Controls.Clear();
            treeViewMaterials.Hide();
            treeViewMaterials.Nodes.Clear();
            TreeNode t_node = treeViewMaterials.Nodes.Add(def_mat_tree.MaterialsName, def_mat_tree.MaterialsText);
            m_materials.BuildTree(t_node);
            t_node.Nodes[def_mat_tree.RawsName].Nodes.Add(def_mat_tree.NewName, def_mat_tree.NewText);
            t_node.Nodes[def_mat_tree.IntermediatesName].Nodes.Add(def_mat_tree.NewName, def_mat_tree.NewText);
            t_node.Nodes[def_mat_tree.ProductsName].Nodes.Add(def_mat_tree.NewName, def_mat_tree.NewText);
            treeViewMaterials.ExpandAll();
            treeViewMaterials.Show();
            UpdateMaterialTreeToolTips();
        }

        /// <summary>
        /// A végleges és az ideiglenes anyaglistákban név alapján keres, ha megtalálja, true-val tér visza.
        /// </summary>
        private bool MaterialExists(string t_name)
        {
            return m_materials.Find(t_name) != null || m_materials_tmp.Find(t_name) != null;
        }

        /// <summary>
        /// A MaterialProperty panel Delete gomb megnyomására hajtódik végre.
        /// </summary>
        private bool CheckMaterialUsage(string t_name)
        {
            if (MaterialInUse(t_name, Color.Red, true))
            {
                MessageBox.Show(def_mat_msg.Msg_material_is_in_use_p1 + t_name + def_mat_msg.Msg_material_is_in_use_p2,
                    def_mat_msg.Msg_material_is_in_use_title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                MaterialInUse(t_name, SystemColors.WindowText, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Egyedi anyagnevet generál.
        /// </summary>
        private string GenMatName(string namebase, string concat)
        {
            int i = 0;
            string name;
            do
            {
                i++;
                name = namebase + concat + i.ToString();
            } while (MaterialExists(name));
            return name;
        }

        static public MaterialProperties FindMaterial(string t_name)
        {
            if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
            MaterialProperties t_matprop = s_pns_editor.m_materials.Find(t_name);
            if (t_matprop == null) throw new Exception(def_PnsStudio_ex.Ex_material_not_found);
            return t_matprop;
        }
        private void RefreshMaterialGrids()
        {
            foreach (Control item in tableLayoutPanelMaterialProperties.Controls)
            {
                PropertyGrid t_prop_grid = ((MaterialPanel)item).MatPropGrid;
                MaterialProperties t_matprop = (MaterialProperties)t_prop_grid.SelectedObject;
                t_matprop.Refresh();
                t_prop_grid.Refresh();
            }
        }
        private void UpdateMaterialTreeToolTips()
        {
            foreach (TreeNode t_node in FindMatTypeNode(defaults.MatTypes.raw).Nodes)
            {
                MaterialProperties t_matprop = m_materials.Find(t_node.Name);
                if (t_matprop != null) t_node.ToolTipText = t_matprop.ToString();
            }
            foreach (TreeNode t_node in FindMatTypeNode(defaults.MatTypes.intermediate).Nodes)
            {
                MaterialProperties t_matprop = m_materials.Find(t_node.Name);
                if (t_matprop != null) t_node.ToolTipText = t_matprop.ToString();
            }
            foreach (TreeNode t_node in FindMatTypeNode(defaults.MatTypes.product).Nodes)
            {
                MaterialProperties t_matprop = m_materials.Find(t_node.Name);
                if (t_matprop != null) t_node.ToolTipText = t_matprop.ToString();
            }
        }
        private void CreateNewMaterial(TreeNode t_node)
        {
            string name = Converters.ToNameString(GenMatName(def_mat_tree.GenBaseName, def_mat_tree.GenBaseConcat));
            t_node.Parent.Nodes.Insert(t_node.Index, name, name);
            MaterialProperties mat = new MaterialProperties(name, MatTypeTreeNode(t_node.Parent));
            m_materials_tmp.Add(mat);
            MaterialPanel matpanel;
            tableLayoutPanelMaterialProperties.Controls.Add(matpanel = new MaterialPanel(mat));
            MatPanelEnsureVisible(matpanel);
        }
        static internal void MatPanelEnsureVisible(MaterialPanel t_matpanel)
        {
            t_matpanel.MatPropLabel.Focus();
            t_matpanel.MatPropUpdate.Focus();
            t_matpanel.MatPropGrid.Focus();
        }
        #endregion

        #region MaterialPanel Update, Cancel, Delete eseményei
        static internal TreeNode FindMatTypeNode(defaults.MatTypes t_type)
        {
            if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
            TreeNode[] t_nodes;
            string t_mattype_name;
            switch (t_type)
            {
                case defaults.MatTypes.raw: t_mattype_name = def_mat_tree.RawsName; break;
                case defaults.MatTypes.intermediate: t_mattype_name = def_mat_tree.IntermediatesName; break;
                case defaults.MatTypes.product: t_mattype_name = def_mat_tree.ProductsName; break;
                default: throw new Exception(def_mat_ex.Ex_unknown_material_type);
            }
            t_nodes = s_pns_editor.treeViewMaterials.Nodes.Find(t_mattype_name, true);
            foreach (TreeNode t_node in t_nodes) if (TreeNodeType(t_node) == defaults.TreeNodeTypes.mat_type && t_node.Name == t_mattype_name) return t_node;
            throw new Exception(def_PnsStudio_ex.Ex_material_not_found);
        }
        static internal TreeNode FindMatNode(string t_matname)
        {
            if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
            TreeNode[] t_nodes = s_pns_editor.treeViewMaterials.Nodes.Find(t_matname, true);
            foreach (TreeNode t_node in t_nodes) if (TreeNodeType(t_node) == defaults.TreeNodeTypes.material && t_node.Name == t_matname) return t_node;
            throw new Exception(def_PnsStudio_ex.Ex_material_not_found);
        }
        private MaterialPanel FindMatPanel(string t_name)
        {
            Control[] controls = tableLayoutPanelMaterialProperties.Controls.Find(t_name, false);
            foreach (MaterialPanel t_matpanel in controls) if (t_matpanel.Name == t_name) return t_matpanel;
            return null;
        }
        /// <summary>
        /// A MaterialProperty panel Update gomb megnyomására hajtódik végre.
        /// </summary>
        private bool materialUpdate(MaterialProperties matprop)
        {
            TreeNode node;
            MaterialProperties mat;
            int t_category_update_choise = 0;
            if ((mat = m_materials.Find(matprop.name)) != null && mat.currname != matprop.currname)
            {
                MessageBox.Show(def_mat_msg.Msg_material_exists_p1 + matprop.name + def_mat_msg.Msg_material_exists_p2,
                    def_mat_msg.Msg_material_exists_title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if ((mat = m_materials.Find(matprop.currname)) != null && mat.material_category != matprop.material_category && MaterialInUse(matprop.currname, Color.Orange, true))
            {
                MaterialQuantityTypeChanged t_dialog = new MaterialQuantityTypeChanged(matprop);
                DialogResult t_result = t_dialog.ShowDialog();
                MaterialInUse(matprop.currname, SystemColors.WindowText, false);
                if (t_result == DialogResult.OK) t_category_update_choise = t_dialog.Choise;
                else return false;
            }
            if ((mat = m_materials_tmp.Find(matprop.name)) != null && mat.currname != matprop.currname)
            {
                node = FindMatNode(mat.currname);
                string name = GenMatName(def_mat_tree.GenBaseName, def_mat_tree.GenBaseConcat);
                MaterialPanel panel = FindMatPanel(mat.currname);
                panel.labelText = name;
                panel.Name = name;
                panel.cmp_currname = name;
                mat.currname = name;
                node.Name = name;
                node.Text = name;
            }
            m_materials_tmp.Remove(matprop);
            node = FindMatNode(matprop.currname);
            node.Name = matprop.name;
            node.Text = matprop.name;
            node.Nodes.Clear();
            matprop.BuildTree(node);
            matprop.isnewitem = false;
            if (!m_materials.ReplaceMaterial(matprop)) m_materials.InsertAtNode(node.NextNode, matprop);
            string t_old_name = matprop.currname;
            matprop.currname = matprop.name;
            if (t_old_name != matprop.name || t_category_update_choise != 0)
            {
                if (t_old_name != matprop.name) UpdateFlowMaterialName(t_old_name, matprop.name);
                if (t_category_update_choise != 0) UpdateFlowMaterialCategory(matprop.currname, t_category_update_choise);
                UpdateOpUnitTree(matprop.currname);
            }
            UpdateOpUnitTreeToolTips();
            UpdateMaterialTreeToolTips();
            RefreshOpUnitGrids();
            return true;
        }

        /// <summary>
        /// A MaterialProperty panel Cancel gomb megnyomására hajtódik végre.
        /// </summary>
        private void materialCancel(MaterialProperties matprop)
        {
            m_materials_tmp.Remove(matprop);
            if (m_materials.Find(matprop.currname) == null) FindMatNode(matprop.currname).Remove();
        }

        /// <summary>
        /// A MaterialProperty panel Delete gomb megnyomására hajtódik végre.
        /// </summary>
        private bool materialDelete(MaterialProperties matprop)
        {
            if (MessageBox.Show(def_mat_delete_confirm_msg.Msg_delete_confirm, def_mat_delete_confirm_msg.Msg_delete_confirm_title, MessageBoxButtons.YesNo) == DialogResult.No) return false;
            if (CheckMaterialUsage(matprop.name)) return false;
            m_materials_tmp.Remove(matprop);
            TreeNode t_node = FindMatNode(matprop.currname);
            m_materials.Remove(t_node);
            t_node.Remove();
            return true;
        }

        /// <summary>
        /// A MaterialProperty panel Update, Cancel vagy Delete gomb megnyomására hajtódik végre.
        /// </summary>
        void events_MatPropChange(object sender, MaterialPropertyEventArgs e)
        {
            switch (e.buttonclick)
            {
                case defaults.MatPropButtons.update:
                    if (!materialUpdate(e.matprop)) return;
                    break;
                case defaults.MatPropButtons.cancel:
                    materialCancel(e.matprop);
                    break;
                case defaults.MatPropButtons.delete:
                    if (!materialDelete(e.matprop)) return;
                    break;
            }
            events.MaterialPanelChange -= new SelectMaterialPanelEventHandler(((MaterialPanel)sender).events_MaterialPanelChange);
            tableLayoutPanelMaterialProperties.Controls.Remove((Control)sender);
            ((Control)sender).Dispose();
            treeViewMaterials.Focus();
        }
        #endregion

        #region Materials tree modify, clone, delete
        /// <summary>
        /// Az anyag módosításának kezdetén létrehozza illetve megkeresi az anyaghoz tartozó property panelt.
        /// </summary>
        private void modifyMaterial(TreeNode node)
        {
            MaterialProperties mat, tmpmat;
            MaterialPanel matpanel;
            if ((mat = m_materials_tmp.Find(node.Name)) != null)
            {
                MatPanelEnsureVisible(FindMatPanel(mat.currname));
                return;
            }
            mat = m_materials.Find(node.Name);
            tmpmat = new MaterialProperties(mat);
            switch (PnsStudio.MatTypeTreeNode(node.Parent))
            {
                case defaults.MatTypes.raw: m_materials_tmp.m_rawlist.Add(tmpmat); break;
                case defaults.MatTypes.intermediate: m_materials_tmp.m_intermediatelist.Add(tmpmat); break;
                case defaults.MatTypes.product: m_materials_tmp.m_productlist.Add(tmpmat); break;
                default: throw new Exception(def_mat_ex.Ex_unknown_material_type);
            }
            tableLayoutPanelMaterialProperties.Controls.Add(matpanel = new MaterialPanel(tmpmat));
            MatPanelEnsureVisible(matpanel);
        }

        private void cloneMaterial(TreeNode node)
        {
            m_materials.Clone(node, GenMatName(node.Name, def_ou_tree.GenCloneConcat));
        }

        private void deleteMaterial(string t_name)
        {
            if (MessageBox.Show(def_mat_delete_confirm_msg.Msg_delete_confirm, def_mat_delete_confirm_msg.Msg_delete_confirm_title, MessageBoxButtons.YesNo) == DialogResult.No) return;
            if (CheckMaterialUsage(t_name)) return;
            TreeNode t_node = FindMatNode(t_name);
            MaterialPanel matpanel = FindMatPanel(t_name);
            if (matpanel != null)
            {
                MaterialProperties matprop = (MaterialProperties)matpanel.MatPropGrid.SelectedObject;
                m_materials_tmp.Remove(matprop);
                events.MaterialPanelChange -= new SelectMaterialPanelEventHandler(matpanel.events_MaterialPanelChange);
                tableLayoutPanelMaterialProperties.Controls.Remove(matpanel);
                matpanel.Dispose();
            }
            m_materials.Remove(t_node);
            t_node.Remove();
        }
        #endregion

        #region Materials tree node mouse click
        /// <summary>
        /// Az anyagok fára történő egérkattintás kezelője.
        /// </summary>
        private void Material_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!IsTreeNodeArea(e.Node, e.Location)) return;
            defaults.TreeNodeTypes t_nodetype = TreeNodeType(e.Node);
            if (t_nodetype == defaults.TreeNodeTypes.new_material) CreateNewMaterial(e.Node);
            else if (t_nodetype == defaults.TreeNodeTypes.material)
            {
                // Modify
                modifyMaterial(e.Node);
                // Dropdown menu
                if (e.Button == MouseButtons.Right)
                {
                    e.Node.TreeView.SelectedNode = e.Node;
                    contextMenuStriptreeviewMaterial.Show((Control)sender, e.Location);
                }
            }
        }
        #endregion

        #region Materials tree Drag & Drop eseménykezelői
        private void Material_ItemDrag(TreeNode t_node)
        {
            if (TreeNodeType(t_node) == defaults.TreeNodeTypes.material)
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
                treeViewMaterials.DoDragDrop(t_node, DragDropEffects.All);
                treeViewMaterials.SelectedNode = m_selected_mat_tree_node;
                treeViewOperatingUnits.SelectedNode = m_selected_ou_tree_node;
                m_do_drag = false;
                m_ghost.DoPainting = false;
                t_timer.Enabled = false;
            }
        }

        private void Material_DragOver(object sender, DragEventArgs e)
        {
            if (!m_do_drag) return;
            e.Effect = DragDropEffects.None;
            treeViewMaterials.Focus();
            m_ghost.Location = new Point(e.X, e.Y);
            TreeView t_tree = (TreeView)sender;
            Point t_point = t_tree.PointToClient(new Point(e.X, e.Y));
            TreeNode t_node = t_tree.GetNodeAt(t_point);
            TreeNode t_source_node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            defaults.TreeNodeTypes t_source_node_type = TreeNodeType(t_source_node);
            defaults.TreeNodeTypes t_node_type = TreeNodeType(t_node);
            if (t_source_node_type == defaults.TreeNodeTypes.io_flow) e.Effect = DragDropEffects.Move;
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
                        if (t_node.Parent == t_source_node.Parent)
                        {
                            if (t_node != t_source_node && t_node.Index != t_source_node.Index + 1) e.Effect = DragDropEffects.Move;
                        }
                        else if (t_node_type == defaults.TreeNodeTypes.material || t_node_type == defaults.TreeNodeTypes.new_material) e.Effect = DragDropEffects.Move;
                    }
                    return;
                }
            }
            treeViewMaterials.SelectedNode = m_selected_mat_tree_node;
        }

        private void Material_DragDrop(object sender, DragEventArgs e)
        {
            TreeView t_tree = (TreeView)sender;
            TreeNode t_node = t_tree.GetNodeAt(t_tree.PointToClient(new Point(e.X, e.Y)));
            TreeNode t_source_node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            defaults.TreeNodeTypes t_source_node_type = TreeNodeType(t_source_node);
            if (t_source_node_type == defaults.TreeNodeTypes.material && e.Effect == DragDropEffects.Move)
            {
                MaterialProperties t_matprop = m_materials.Remove(t_source_node);
                if (t_matprop != null)
                {
                    t_matprop.type = MatTypeTreeNode(t_node.Parent);
                    m_materials.InsertAtNode(t_node, t_matprop);
                }
                MaterialPanel matpanel = FindMatPanel(t_source_node.Name);
                if (matpanel != null)
                {
                    t_matprop = (MaterialProperties)matpanel.MatPropGrid.SelectedObject;
                    m_materials_tmp.Remove(t_matprop);
                    t_matprop.type = MatTypeTreeNode(t_node.Parent);
                    matpanel.MatPropGrid.Refresh();
                    m_materials_tmp.InsertAtNode(t_node, t_matprop);
                }
                t_source_node.Remove();
                t_node.Parent.Nodes.Insert(t_node.Index, (TreeNode)t_source_node.Clone());
                UpdateMaterialTreeToolTips();
            }
            else if (t_source_node_type == defaults.TreeNodeTypes.io_flow && e.Effect == DragDropEffects.Move)
            {
                deleteIOFlow(t_source_node);
            }
        }
        #endregion

        #region Properties
        static internal TreeView MatTree
        {
            get
            {
                if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
                return s_pns_editor.treeViewMaterials;
            }
        }
        #endregion
    }

    public class MaterialPropertiesList : List<MaterialProperties>
    {
        #region Members
        private string m_cmp_name;
        #endregion

        #region Constructors
        public MaterialPropertiesList()
        {
        }
        public MaterialPropertiesList(RawMaterial[] mats)
        {
            if (mats != null)
            {
                foreach (RawMaterial mat in mats)
                {
                    CheckExistance(mat.name);
                    Add(new MaterialProperties(mat));
                }
            }
        }
        public MaterialPropertiesList(IntermediateMaterial[] mats)
        {
            if (mats != null)
            {
                foreach (IntermediateMaterial mat in mats)
                {
                    CheckExistance(mat.name);
                    Add(new MaterialProperties(mat));
                }
            }
        }
        public MaterialPropertiesList(ProductMaterial[] mats)
        {
            if (mats != null)
            {
                foreach (ProductMaterial mat in mats)
                {
                    CheckExistance(mat.name);
                    Add(new MaterialProperties(mat));
                }
            }
        }
        #endregion

        #region Member functions
        private void CheckExistance(string t_name)
        {
            if (Find(t_name) != null) throw new Exception(def_mat_ex.Ex_more_inctance_p1 + t_name + def_mat_ex.Ex_more_inctance_p2);
        }
        public void BuildTree(TreeNode t_node)
        {
            foreach (MaterialProperties mat in this)
            {
                mat.BuildTree(t_node.Nodes.Add(mat.name, mat.name));
            }
        }
        /// <summary>
        /// A listába beilleszti a node-nak megfelelő elem elé a matprop elemet. Ha a node-hoz
        /// nem tartozik elem a listában, akkor a következő node-ot veszi. Ha nem sikerül beszúrni,
        /// akkor a lista végéhez adja az elemet.
        /// </summary>
        public void InsertAtNode(TreeNode node, MaterialProperties matprop)
        {
            int i;
            bool inserted = false;
            while (!inserted && node != null && node.Name != def_mat_tree.NewName)
            {
                if (inserted = ((i = FindIndex(node.Name)) > -1)) Insert(i, matprop);
                else node = node.NextNode;
            }
            if (!inserted) Add(matprop);
        }
        public void Clone(TreeNode t_node, string t_clonename)
        {
            int i;
            if ((i = FindIndex(t_node.Name)) > -1)
            {
                MaterialProperties t_mat = new MaterialProperties(this[i]);
                t_mat.currname = t_mat.name = t_clonename;
                Insert(i + 1, t_mat);
                t_mat.BuildTree(t_node.Parent.Nodes.Insert(t_node.Index + 1, t_mat.name, t_mat.name));
            }
            else MessageBox.Show(def_mat_msg.Msg_can_not_clone_temporary);
        }
        public MaterialProperties Remove(string t_name)
        {
            int i;
            MaterialProperties matprop = null;
            if ((i = FindIndex(t_name)) > -1)
            {
                matprop = this[i];
                RemoveAt(i);
            }
            return matprop;
        }
        private bool materialMatch(MaterialProperties matprop)
        {
            return matprop.currname == m_cmp_name;
        }
        public int FindIndex(string name)
        {
            m_cmp_name = name;
            return base.FindIndex(materialMatch);
        }
        public MaterialProperties Find(string name)
        {
            m_cmp_name = name;
            return base.Find(materialMatch);
        }
        public bool ReplaceMaterial(MaterialProperties matprop)
        {
            int i = FindIndex(matprop.currname);
            if (i > -1) this[i] = matprop;
            return i > -1;
        }
        public RawMaterial[] rToXML()
        {
            RawMaterial[] xmlmats = new RawMaterial[Count];
            int i = 0;
            foreach (MaterialProperties matprop in this)
            {
                xmlmats[i++] = matprop.rToXML();
            }
            return xmlmats;
        }
        public IntermediateMaterial[] iToXML()
        {
            IntermediateMaterial[] xmlmats = new IntermediateMaterial[Count];
            int i = 0;
            foreach (MaterialProperties matprop in this)
            {
                xmlmats[i++] = matprop.iToXML();
            }
            return xmlmats;
        }
        public ProductMaterial[] pToXML()
        {
            ProductMaterial[] xmlmats = new ProductMaterial[Count];
            int i = 0;
            foreach (MaterialProperties matprop in this)
            {
                xmlmats[i++] = matprop.pToXML();
            }
            return xmlmats;
        }
        public void UpdateAllValues()
        {
            foreach (MaterialProperties matprop in this) matprop.UpdateAllValues();
        }
        public override string ToString() { return ""; }
        #endregion
    }

    public class AllTypeMaterials
    {
        #region Members
        public MaterialPropertiesList m_rawlist;
        public MaterialPropertiesList m_intermediatelist;
        public MaterialPropertiesList m_productlist;
        #endregion

        #region Constructors
        public AllTypeMaterials()
        {
            m_rawlist = new MaterialPropertiesList();
            m_intermediatelist = new MaterialPropertiesList();
            m_productlist = new MaterialPropertiesList();
        }
        public AllTypeMaterials(Materials mats)
        {
            m_rawlist = new MaterialPropertiesList(mats.rawMaterial);
            m_intermediatelist = new MaterialPropertiesList(mats.intermediateMaterial);
            m_productlist = new MaterialPropertiesList(mats.productMaterial);
        }
        #endregion

        #region Member functions
        public void BuildTree(TreeNode t_node)
        {
            m_rawlist.BuildTree(t_node.Nodes.Add(def_mat_tree.RawsName, def_mat_tree.RawsText));
            m_intermediatelist.BuildTree(t_node.Nodes.Add(def_mat_tree.IntermediatesName, def_mat_tree.IntermediatesText));
            m_productlist.BuildTree(t_node.Nodes.Add(def_mat_tree.ProductsName, def_mat_tree.ProductsText));
        }
        /// <summary>
        /// Az anyaglistákban név alapján keres, és visszaadja az anyag referenciáját.
        /// </summary>
        public MaterialProperties Find(string name)
        {
            MaterialProperties mat;
            if ((mat = m_rawlist.Find(name)) != null) return mat;
            if ((mat = m_intermediatelist.Find(name)) != null) return mat;
            if ((mat = m_productlist.Find(name)) != null) return mat;
            return null;
        }
        public void Add(MaterialProperties mat)
        {
            switch (mat.type)
            {
                case defaults.MatTypes.raw: m_rawlist.Add(mat); break;
                case defaults.MatTypes.intermediate: m_intermediatelist.Add(mat); break;
                case defaults.MatTypes.product: m_productlist.Add(mat); break;
                default: throw new Exception(def_mat_ex.Ex_unknown_material_type);
            }
        }
        public void InsertAtNode(TreeNode t_node, MaterialProperties t_mat)
        {
            switch (t_mat.type)
            {
                case defaults.MatTypes.raw: m_rawlist.InsertAtNode(t_node, t_mat); break;
                case defaults.MatTypes.intermediate: m_intermediatelist.InsertAtNode(t_node, t_mat); break;
                case defaults.MatTypes.product: m_productlist.InsertAtNode(t_node, t_mat); break;
            }
        }
        public void Clone(TreeNode t_node, string t_clonename)
        {

            switch (PnsStudio.MatTypeTreeNode(t_node.Parent))
            {
                case defaults.MatTypes.raw: m_rawlist.Clone(t_node, t_clonename); break;
                case defaults.MatTypes.intermediate: m_intermediatelist.Clone(t_node, t_clonename); break;
                case defaults.MatTypes.product: m_productlist.Clone(t_node, t_clonename); break;
            }
        }
        public void Remove(MaterialProperties mat)
        {
            switch (mat.type)
            {
                case defaults.MatTypes.raw: m_rawlist.Remove(mat); break;
                case defaults.MatTypes.intermediate: m_intermediatelist.Remove(mat); break;
                case defaults.MatTypes.product: m_productlist.Remove(mat); break;
            }
        }
        public MaterialProperties Remove(TreeNode t_node)
        {
            MaterialProperties mat = null;
            switch (PnsStudio.MatTypeTreeNode(t_node.Parent))
            {
                case defaults.MatTypes.raw: mat = m_rawlist.Remove(t_node.Name); break;
                case defaults.MatTypes.intermediate: mat = m_intermediatelist.Remove(t_node.Name); break;
                case defaults.MatTypes.product: mat = m_productlist.Remove(t_node.Name); break;
            }
            return mat;
        }
        public bool ReplaceMaterial(MaterialProperties mat)
        {
            switch (mat.type)
            {
                case defaults.MatTypes.raw: return m_rawlist.ReplaceMaterial(mat);
                case defaults.MatTypes.intermediate: return m_intermediatelist.ReplaceMaterial(mat);
                case defaults.MatTypes.product: return m_productlist.ReplaceMaterial(mat);
            }
            return false;
        }
        public Materials ToXML()
        {
            Materials mats = new Materials();
            mats.rawMaterial = m_rawlist.rToXML();
            mats.intermediateMaterial = m_intermediatelist.iToXML();
            mats.productMaterial = m_productlist.pToXML();
            return mats;
        }
        public void UpdateAllValues()
        {
            m_rawlist.UpdateAllValues();
            m_intermediatelist.UpdateAllValues();
            m_productlist.UpdateAllValues();
        }
        public override string ToString() { return ""; }
        #endregion

        #region Properties
        public int Count { get { return m_rawlist.Count + m_intermediatelist.Count + m_productlist.Count; } }
        #endregion
    }

    
}
