using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PNSDraw
{
    public partial class SolutionSettingsWindow : Form
    {

        public bool Changed = false;

        public SolutionSettingsWindow()
        {
            InitializeComponent();
        }

        private void button_set_Click(object sender, EventArgs e)
        {
            Changed = true;

            Globals.SolutionSettings.IncludedItem.MaterialText = (Globals.SolutionSettings.ValueStyle) cb_im.SelectedIndex;
            Globals.SolutionSettings.IncludedItem.OperatingUnitText = (Globals.SolutionSettings.ValueStyle)cb_io.SelectedIndex;
            Globals.SolutionSettings.IncludedItem.EdgeText = (Globals.SolutionSettings.ValueStyle)cb_ie.SelectedIndex;


            Globals.SolutionSettings.ExcludedItem.MaterialText = (Globals.SolutionSettings.ValueStyle)cb_em.SelectedIndex;
            Globals.SolutionSettings.ExcludedItem.OperatingUnitText = (Globals.SolutionSettings.ValueStyle)cb_eo.SelectedIndex;
            Globals.SolutionSettings.ExcludedItem.EdgeText = (Globals.SolutionSettings.ValueStyle)cb_ee.SelectedIndex;

            Globals.SolutionSettings.IncludedItem.Color = pb_i.BackColor;
            Globals.SolutionSettings.ExcludedItem.Color = pb_e.BackColor;

            if (cb_ev.SelectedIndex == 0)
            {
                Globals.SolutionSettings.ExcludedItem.Color = System.Drawing.Color.Transparent;
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
            cb_im.SelectedIndex = (int) Globals.SolutionSettings.IncludedItem.MaterialText;
            cb_io.SelectedIndex = (int) Globals.SolutionSettings.IncludedItem.OperatingUnitText;
            cb_ie.SelectedIndex = (int) Globals.SolutionSettings.IncludedItem.EdgeText;

            if (Globals.SolutionSettings.ExcludedItem.MaterialText != Globals.SolutionSettings.ValueStyle.Calculated)
            {
                cb_em.SelectedIndex = (int)Globals.SolutionSettings.ExcludedItem.MaterialText;
            }
            if (Globals.SolutionSettings.ExcludedItem.OperatingUnitText != Globals.SolutionSettings.ValueStyle.Calculated)
            {
                cb_eo.SelectedIndex = (int)Globals.SolutionSettings.ExcludedItem.OperatingUnitText;
            }
            if (Globals.SolutionSettings.ExcludedItem.EdgeText != Globals.SolutionSettings.ValueStyle.Calculated)
            {
                cb_ee.SelectedIndex = (int)Globals.SolutionSettings.ExcludedItem.EdgeText;
            }

            pb_i.BackColor = Globals.SolutionSettings.IncludedItem.Color;
            pb_e.BackColor = Globals.SolutionSettings.ExcludedItem.Color;

            if (Globals.SolutionSettings.ExcludedItem.Color == System.Drawing.Color.Transparent)
            {
                cb_ev.SelectedIndex = 0;
            }
            else
            {
                cb_ev.SelectedIndex = 1;
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
                pb_e.BackColor = System.Drawing.Color.Transparent;
            }
            else
            {
                pb_e.BackColor = System.Drawing.Color.LightGray;
            }
        }
    }
}
