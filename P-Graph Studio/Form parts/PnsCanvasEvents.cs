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

using PNSDraw.Configuration;

namespace PNSDraw
{
    public partial class Form1 : Form
    {
        #region PnsCanvas events
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
            if (toremove != null)
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

            if (obj != null)
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
                    mr.Type = MaterialTypes.Raw;
                    e.Data = mr;
                    Graph.AddMaterial(mr);

                    break;
                case CanvasMode.Intermediate:
                    Material mi = new Material(Graph);
                    mi.Type = MaterialTypes.Intermediate;
                    e.Data = mi;
                    Graph.AddMaterial(mi);

                    break;
                case CanvasMode.Product:
                    Material mp = new Material(Graph);
                    mp.Type = MaterialTypes.Product;
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
            isProblemExists = true;
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
        #endregion
    }
}
