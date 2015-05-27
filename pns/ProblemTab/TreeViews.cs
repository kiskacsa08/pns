using System;
using System.Drawing;
using System.Windows.Forms;

using Pns.Globals;
using Pns.Xml_Serialization.PnsGUI.TreeViews;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;

namespace Pns
{
    public partial class PnsStudio : Form
    {
        #region Members
        private TreeNodeGhost m_ghost;
        private bool m_do_drag;
        private TreeNode m_selected_mat_tree_node;
        private TreeNode m_selected_ou_tree_node;
        #endregion

        #region Member functions
        private bool IsTreeNodeArea(TreeNode t_node, System.Drawing.Point t_p)
        {
            return t_p.X >= t_node.Bounds.Left && t_p.X <= t_node.Bounds.Right && t_p.Y >= t_node.Bounds.Top && t_p.Y <= t_node.Bounds.Bottom;
        }

        static public defaults.MatTypes MatTypeTreeNode(TreeNode t_node)
        {
            if (PnsStudio.s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
            if (t_node != null)
            {
                if (t_node.Name == def_mat_tree.RawsName) return defaults.MatTypes.raw;
                if (t_node.Name == def_mat_tree.IntermediatesName) return defaults.MatTypes.intermediate;
                if (t_node.Name == def_mat_tree.ProductsName) return defaults.MatTypes.product;
            }
            return defaults.MatTypes.none;
        }

        private bool IsIOFlowsTreeNode(TreeNode t_node)
        {
            return t_node != null && (t_node.Name == def_ou_tree.InputMaterialsName || t_node.Name == def_ou_tree.OutputMaterialsName);
        }

        static public defaults.TreeNodeTypes TreeNodeType(TreeNode t_node)
        {
            if (PnsStudio.s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
            if (t_node != null)
            {
                if (t_node.TreeView == s_pns_editor.treeViewMaterials)
                {
                    if (t_node.Name == def_mat_tree.MaterialsName) return defaults.TreeNodeTypes.materials;
                    if (MatTypeTreeNode(t_node) != defaults.MatTypes.none) return defaults.TreeNodeTypes.mat_type;
                    if (MatTypeTreeNode(t_node.Parent) != defaults.MatTypes.none)
                    {
                        if (t_node.Name == def_mat_tree.NewName) return defaults.TreeNodeTypes.new_material;
                        return defaults.TreeNodeTypes.material;
                    }
                }
                else if (t_node.TreeView == s_pns_editor.treeViewOperatingUnits)
                {
                    if (t_node.Name == def_ou_tree.OperatingUnitsName) return defaults.TreeNodeTypes.opunits;
                    if (t_node.Parent != null && t_node.Parent.Name == def_ou_tree.OperatingUnitsName)
                    {
                        if (t_node.Name == def_ou_tree.NewName) return defaults.TreeNodeTypes.new_opunit;
                        return defaults.TreeNodeTypes.opunit;
                    }
                    if (s_pns_editor.IsIOFlowsTreeNode(t_node)) return defaults.TreeNodeTypes.io_flows;
                    if (s_pns_editor.IsIOFlowsTreeNode(t_node.Parent)) return defaults.TreeNodeTypes.io_flow;
                }
            }
            return defaults.TreeNodeTypes.none;
        }

        private void treeView_KeyPress(TreeNode t_node, Keys t_char)
        {
            defaults.TreeNodeTypes t_node_type = TreeNodeType(t_node);
            if (t_char == Keys.Enter || t_char == Keys.Space)
            {
                if (t_node_type == defaults.TreeNodeTypes.new_material) CreateNewMaterial(t_node);
                else if (t_node_type == defaults.TreeNodeTypes.material) modifyMaterial(t_node);
                else if (t_node_type == defaults.TreeNodeTypes.new_opunit) CreateNewOpUnit(t_node);
                else if (t_node_type == defaults.TreeNodeTypes.opunit) modifyOperatingUnit(t_node);
            }
            if (t_char == Keys.Delete)
            {
                if (t_node_type == defaults.TreeNodeTypes.material) deleteMaterial(t_node.Name);
                else if (t_node_type == defaults.TreeNodeTypes.opunit) deleteOperatingUnit(t_node.Name);
                else if (t_node_type == defaults.TreeNodeTypes.io_flow) deleteIOFlow(t_node);
            }
        }
        #endregion

        #region Problem Tab Event Handlers
        private void splitContainer1_Panel1_ClientSizeChanged(object sender, EventArgs e)
        {
            int ncol = splitContainerMaterialOpunitProperties.Panel1.ClientSize.Width / 300;
            if (ncol == 0) ncol = 1;
            tableLayoutPanelMaterialProperties.ColumnCount = ncol;
        }
        private void splitContainerMaterialOpunitProperties_Panel2_ClientSizeChanged(object sender, EventArgs e)
        {
            int ncol = splitContainerMaterialOpunitProperties.Panel2.ClientSize.Width / 355;
            if (ncol == 0) ncol = 1;
            tableLayoutPanelOperatingUnitProperties.ColumnCount = ncol;
        }
        private void splitContainerMaterialOpunitProperties_Panel1_Click(object sender, EventArgs e)
        {
            splitContainerMaterialOpunitProperties.Panel1.Focus();
        }
        private void splitContainerMaterialOpunitProperties_Panel2_Click(object sender, EventArgs e)
        {
            splitContainerMaterialOpunitProperties.Panel2.Focus();
        }
        private void splitContainerTrees_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
            else m_ghost.DoPainting = m_do_drag;
        }
        private void splitContainerTrees_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                if (files.Length > 1) MessageBox.Show(def_PnsStudio.Msg_multi_file_drop, def_PnsStudio.Msg_multi_file_drop_title);
                if (check_changes()) pns_open(files[0]);
            }
        }
        private void splitContainerTrees_DragLeave(object sender, EventArgs e)
        {
            m_ghost.DoPainting = false;
        }
        private void splitContainerTrees_DragOver(object sender, DragEventArgs e)
        {
            m_ghost.Location = new Point(e.X, e.Y);
        }
        #endregion

        #region Materials Event Handlers

        private void treeViewMaterials_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Material_NodeMouseClick(sender, e);
        }

        private void Modify_Click(object sender, EventArgs e)
        {
            modifyMaterial(((TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl).SelectedNode);
        }

        private void Clone_Click(object sender, EventArgs e)
        {
            cloneMaterial(((TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl).SelectedNode);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            deleteMaterial(((TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl).SelectedNode.Name);
        }

        private void treeViewMaterials_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Material_ItemDrag((TreeNode)e.Item);
        }

        private void treeViewMaterials_DragOver(object sender, DragEventArgs e)
        {
            Material_DragOver(sender, e);
        }

        private void treeViewMaterials_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                if (files.Length > 1) MessageBox.Show(def_PnsStudio.Msg_multi_file_drop, def_PnsStudio.Msg_multi_file_drop_title);
                if (check_changes()) pns_open(files[0]);
            }
            else Material_DragDrop(sender, e);
        }

        private void treeViewMaterials_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
            else m_ghost.DoPainting = m_do_drag;
        }

        private void treeViewMaterials_DragLeave(object sender, EventArgs e)
        {
            m_ghost.DoPainting = false;
        }

        private void treeViewMaterials_KeyDown(object sender, KeyEventArgs e)
        {
            treeView_KeyPress(treeViewMaterials.SelectedNode, e.KeyCode);
        }
        #endregion

        #region Operating Units Event Handlers

        private void treeViewOperatingUnit_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            OperatingUnit_NodeMouseClick(sender, e);
        }

        private void ModifyOU_Click(object sender, EventArgs e)
        {
            modifyOperatingUnit(((TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl).SelectedNode);
        }

        private void CloneOU_Click(object sender, EventArgs e)
        {
            cloneOperatingUnit(((TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl).SelectedNode);
        }

        private void DeleteOU_Click(object sender, EventArgs e)
        {
            deleteOperatingUnit(((TreeView)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl).SelectedNode.Name);
        }

        private void treeViewOperatingUnits_ItemDrag(object sender, ItemDragEventArgs e)
        {
            OperatingUnit_ItemDrag((TreeNode)e.Item);
        }

        private void treeViewOperatingUnits_DragOver(object sender, DragEventArgs e)
        {
            OperatingUnit_DragOver(sender, e);
        }

        private void treeViewOperatingUnits_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                if (files.Length > 1) MessageBox.Show(def_PnsStudio.Msg_multi_file_drop, def_PnsStudio.Msg_multi_file_drop_title);
                if (check_changes()) pns_open(files[0]);
            }
            else OperatingUnit_DragDrop(sender, e);
        }

        private void treeViewOperatingUnits_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                m_ghost.DoPainting = m_do_drag;
                m_over_outree = true;
            }
        }

        private void treeViewOperatingUnits_DragLeave(object sender, EventArgs e)
        {
            m_ghost.DoPainting = false;
            m_over_outree = false;
        }

        private void treeViewOperatingUnits_KeyDown(object sender, KeyEventArgs e)
        {
            treeView_KeyPress(treeViewOperatingUnits.SelectedNode, e.KeyCode);
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeViewOperatingUnits.Hide();
            treeViewOperatingUnits.ExpandAll();
            treeViewOperatingUnits.Show();
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeViewOperatingUnits.CollapseAll();
            treeViewOperatingUnits.Nodes[0].Expand();
        }
        #endregion
    }
}
