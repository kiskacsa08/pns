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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PNSDraw.Configuration;

namespace PNSDraw
{
    public partial class SettingsWindow : Form
    {

        public bool Changed = false;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            textBox_fontsize.Text = Config.Instance.GraphSettings.DefaultFontSize.ToString();
            textBox_gridsize.Text = Config.Instance.GraphSettings.GridSize.ToString();

            MaterialTextCB.Checked = Config.Instance.GraphSettings.ShowMaterialText;
            OperatingUnitTextCB.Checked = Config.Instance.GraphSettings.ShowOperatingUnitText;
            EdgeTextCB.Checked = Config.Instance.GraphSettings.ShowEdgeText;
            CommentsCB.Checked = Config.Instance.GraphSettings.ShowComments;
            ParametersCB.Checked = Config.Instance.GraphSettings.ShowParameters;
            EdgeLongFormatCB.Checked = Config.Instance.GraphSettings.ShowEdgeLongFormat;
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            Config.Instance.GraphSettings.Reset();
            textBox_fontsize.Text = Config.Instance.GraphSettings.DefaultFontSize.ToString();
            textBox_gridsize.Text = Config.Instance.GraphSettings.GridSize.ToString();
        }

        private void button_set_Click(object sender, EventArgs e)
        {
            int temp = 0;
            temp = ConvertManager.ToInt(textBox_fontsize.Text);
            if (temp > 0)
            {
                Config.Instance.GraphSettings.DefaultFontSize = temp;
            }
            temp = ConvertManager.ToInt(textBox_gridsize.Text);
            if (temp > 0)
            {
                Config.Instance.GraphSettings.GridSize = temp;
            }

            Config.Instance.GraphSettings.ShowMaterialText = MaterialTextCB.Checked;
            Config.Instance.GraphSettings.ShowOperatingUnitText = OperatingUnitTextCB.Checked;
            Config.Instance.GraphSettings.ShowEdgeText = EdgeTextCB.Checked;
            Config.Instance.GraphSettings.ShowComments = CommentsCB.Checked;
            Config.Instance.GraphSettings.ShowParameters = ParametersCB.Checked;
            Config.Instance.GraphSettings.ShowEdgeLongFormat = EdgeLongFormatCB.Checked;

            Changed = true;            

            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
