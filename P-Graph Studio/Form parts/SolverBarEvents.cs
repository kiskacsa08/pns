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
        #region Solver bar events
        #region Mutual exclusions
        private void mutualExlusionsButton_Click(object sender, EventArgs e)
        {
            MutualExclusionDialog med = new MutualExclusionDialog(Graph);
            med.ShowDialog();
        }
        #endregion

        #region Solve button
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!isProblemExists)
            {
                MessageBox.Show("There is no problem!");
                return;
            }

            switch (toolStripComboBox2.SelectedIndex)
            {
                case 0:
                    algorithm = "INSIDEOUT";
                    break;
                case 1:
                    algorithm = "SSG";
                    break;
                case 2:
                    algorithm = "SSGLP";
                    break;
                case 3:
                    algorithm = "MSG";
                    break;
                default:
                    algorithm = "";
                    break;
            }

            if (Config.Instance.SolverSettings.IsOnlineSolver)
            {
                if (backgroundWorkerOnline.IsBusy != true)
                {
                    pwd = new PleaseWaitDialog();
                    pwd.Canceled += new EventHandler<EventArgs>(cancelAsyncButton_Click);
                    //pwd.StartPosition = FormStartPosition.CenterScreen;
                    pwd.Show(this);
                    backgroundWorkerOnline.RunWorkerAsync();
                }
            }
            else
            {
                if (backgroundWorkerOffline.IsBusy != true)
                {
                    pwd = new PleaseWaitDialog();
                    pwd.Canceled += new EventHandler<EventArgs>(cancelAsyncButton_Click);
                    //pwd.StartPosition = FormStartPosition.CenterScreen;
                    pwd.Show(this);
                    backgroundWorkerOffline.RunWorkerAsync();
                }
            }
        }
        #endregion
        #endregion
    }
}

