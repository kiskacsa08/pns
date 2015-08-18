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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PNSDraw.Configuration;

namespace PNSDraw
{
    public partial class LayoutSettingsWindow : Form
    {
        public bool Changed = false;
        public LayoutSettingsWindow()
        {
            InitializeComponent();
        }

        private void LayoutSettingsWindow_Load(object sender, EventArgs e)
        {
            textBox_layer.Text = (Config.Instance.LayoutSettings.DefaultLayerDistance * Config.Instance.GraphSettings.GridSize).ToString();
            textBox_node.Text = (Config.Instance.LayoutSettings.DefaultNodeDistance * Config.Instance.GraphSettings.GridSize).ToString();
            gridsize_label.Text = "Current grid size: " + Config.Instance.GraphSettings.GridSize;
            checkBox_weightedarcs.Checked = Config.Instance.LayoutSettings.WeightedArcs;
            checkBox_fixedraws.Checked = Config.Instance.LayoutSettings.FixedRaws;
            checkBox_fixedproducts.Checked = Config.Instance.LayoutSettings.FixedProducts;
            comboBox_engine.SelectedIndex = comboBox_engine.FindStringExact(Config.Instance.LayoutSettings.SelectedEngine);
            comboBox1.SelectedIndex = comboBox1.FindStringExact(Config.Instance.LayoutSettings.Align);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int temp = 0;
            temp = ConvertManager.ToInt(textBox_layer.Text);
            if (temp > 0)
            {
                Config.Instance.LayoutSettings.DefaultLayerDistance = temp / Config.Instance.GraphSettings.GridSize;
            }
            temp = ConvertManager.ToInt(textBox_node.Text);
            if (temp > 0)
            {
                Config.Instance.LayoutSettings.DefaultNodeDistance = temp / Config.Instance.GraphSettings.GridSize;
            }
            Config.Instance.LayoutSettings.WeightedArcs = checkBox_weightedarcs.Checked;
            Config.Instance.LayoutSettings.FixedRaws = checkBox_fixedraws.Checked;
            Config.Instance.LayoutSettings.FixedProducts = checkBox_fixedproducts.Checked;
            Config.Instance.LayoutSettings.SelectedEngine = (string)comboBox_engine.SelectedItem;

            if ((string)comboBox1.SelectedItem != Config.Instance.LayoutSettings.Align) 
            {
                int temp_opheight = Config.Instance.OperatingUnitHeight;
                int temp_opwidth = Config.Instance.OperatingUnitWidth;
                int temp_DefaultRootX = Config.Instance.LayoutSettings.DefaultRootX;
                int temp_DefaultRootY = Config.Instance.LayoutSettings.DefaultRootY;
                int temp_DefaultLayerDistance = Config.Instance.LayoutSettings.DefaultLayerDistance;
                int temp_DefaultNodeDistance = Config.Instance.LayoutSettings.DefaultNodeDistance;

                Config.Instance.OperatingUnitHeight = temp_opwidth;
                Config.Instance.OperatingUnitWidth = temp_opheight;

                Config.Instance.LayoutSettings.Align = (string)comboBox1.SelectedItem;
            }
            Changed = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
