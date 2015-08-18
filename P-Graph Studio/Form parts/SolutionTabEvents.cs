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
        #region Solution tab events
        #region selected change
        private void cmbSolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Graph.Solutions.Count > 0)
            {
                UpdateSolutionTreeView(cmbSolutions.SelectedIndex);
                solutionComboBox.SelectedIndex = cmbSolutions.SelectedIndex + 3;
            }
        }
        #endregion

        #region mouse click
        private void treeSolution_MouseClick(object sender, MouseEventArgs e)
        {
            /*if (e.Button == MouseButtons.Right)
            {
                //contextSolutions.Show((Control)treeSolution, e.Location);
            }*/
        }
        #endregion
        #endregion
    }
}

