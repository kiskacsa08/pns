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
    public partial class SolutionSettingsWindow : Form
    {

        public bool Changed = false;

        private Color ExcludedTempColor;

        public SolutionSettingsWindow()
        {
            InitializeComponent();
        }

        private void button_set_Click(object sender, EventArgs e)
        {
            Changed = true;

            Config.Instance.SolutionSettings.IncludedItem.MaterialText = (SolutionSettings.ValueStyle)cb_im.SelectedIndex;
            Config.Instance.SolutionSettings.IncludedItem.OperatingUnitText = (SolutionSettings.ValueStyle)cb_io.SelectedIndex;
            Config.Instance.SolutionSettings.IncludedItem.EdgeText = (SolutionSettings.ValueStyle)cb_ie.SelectedIndex;


            Config.Instance.SolutionSettings.ExcludedItem.MaterialText = (SolutionSettings.ValueStyle)cb_em.SelectedIndex;
            Config.Instance.SolutionSettings.ExcludedItem.OperatingUnitText = (SolutionSettings.ValueStyle)cb_eo.SelectedIndex;
            Config.Instance.SolutionSettings.ExcludedItem.EdgeText = (SolutionSettings.ValueStyle)cb_ee.SelectedIndex;

            Config.Instance.SolutionSettings.IncludedItem.Color = pb_i.BackColor;
            Config.Instance.SolutionSettings.ExcludedItem.Color = pb_e.BackColor;

            Config.Instance.SolutionSettings.ExcludedItem.Visible = (SolutionSettings.Visibility)cb_ev.SelectedIndex;

            if (cb_ev.SelectedIndex == 0)
            {
                Config.Instance.SolutionSettings.ExcludedItem.Color = System.Drawing.Color.Transparent;
            }

            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Changed = false;
            Close();
        }

        private void SolutionSettingsWindow_Load(object sender, EventArgs e)
        {
            cb_im.SelectedIndex = (int)Config.Instance.SolutionSettings.IncludedItem.MaterialText;
            cb_io.SelectedIndex = (int)Config.Instance.SolutionSettings.IncludedItem.OperatingUnitText;
            cb_ie.SelectedIndex = (int)Config.Instance.SolutionSettings.IncludedItem.EdgeText;

            if (Config.Instance.SolutionSettings.ExcludedItem.MaterialText != SolutionSettings.ValueStyle.Calculated)
            {
                cb_em.SelectedIndex = (int)Config.Instance.SolutionSettings.ExcludedItem.MaterialText;
            }
            if (Config.Instance.SolutionSettings.ExcludedItem.OperatingUnitText != SolutionSettings.ValueStyle.Calculated)
            {
                cb_eo.SelectedIndex = (int)Config.Instance.SolutionSettings.ExcludedItem.OperatingUnitText;
            }
            if (Config.Instance.SolutionSettings.ExcludedItem.EdgeText != SolutionSettings.ValueStyle.Calculated)
            {
                cb_ee.SelectedIndex = (int)Config.Instance.SolutionSettings.ExcludedItem.EdgeText;
            }

            cb_ev.SelectedIndex = (int)Config.Instance.SolutionSettings.ExcludedItem.Visible;

            pb_i.BackColor = Config.Instance.SolutionSettings.IncludedItem.Color;
            pb_e.BackColor = Config.Instance.SolutionSettings.ExcludedItem.Color;

            if (Config.Instance.SolutionSettings.ExcludedItem.Visible == SolutionSettings.Visibility.Hide)
            {
                ExcludedTempColor = Color.LightGray;
            }
            else
            {
                ExcludedTempColor = Config.Instance.SolutionSettings.ExcludedItem.Color;
            }
        }

        private void pb_i_Click(object sender, EventArgs e)
        {
            ColorDialog cdlg = new ColorDialog();

            cdlg.Color = pb_i.BackColor;

            cdlg.ShowDialog();

            pb_i.BackColor = cdlg.Color;

        }

        private void pb_e_Click(object sender, EventArgs e)
        {
            ColorDialog cdlg = new ColorDialog();

            cdlg.Color = pb_e.BackColor;

            cdlg.ShowDialog();

            pb_e.BackColor = cdlg.Color;
        }

        private void cb_ev_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_ev.SelectedIndex == 0)
            {
                ExcludedTempColor = pb_e.BackColor;
                pb_e.BackColor = System.Drawing.Color.Transparent;
            }
            else
            {
                pb_e.BackColor = ExcludedTempColor;
            }
        }
    }
}
