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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNSDraw
{
    public partial class Form1 : Form
    {
        #region Tree view events
        #region double click
        private void treeMaterials_DoubleClick(object sender, EventArgs e)
        {
            TreeNode selectedNode = ((TreeView)sender).SelectedNode;
            Canvas.IGraphicsObject obj = null;
            bool found = false;

            foreach (Material mat in Graph.Materials)
            {
                try
                {
                    if (mat.Name.Equals(selectedNode.Text))
                    {
                        found = true;
                        obj = (Canvas.IGraphicsObject)mat;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " (" + ex.Source + ")");
                }
            }

            if (!found)
            {
                foreach (OperatingUnit ou in Graph.OperatingUnits)
                {
                    try
                    {
                        if (ou.Name.Equals(selectedNode.Text))
                        {
                            found = true;
                            obj = (Canvas.IGraphicsObject)ou;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + " (" + ex.Source + ")");
                    }
                }
            }

            if (obj != null)
            {
                propertyGrid1.SelectedObject = obj.GetParentObject();
                copy_toolStripMenuItem.Enabled = false;
                duplicate_toolStripMenuItem.Enabled = false;
                tabControl1.SelectedTab = tabPage1;
            }
        }
        #endregion

        #region node mouse click
        private void treeMaterials_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                e.Node.TreeView.SelectedNode = e.Node;
            }
        }
        #endregion
        #endregion
    }
}

