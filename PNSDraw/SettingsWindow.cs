using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            textBox_fontsize.Text = Globals.DefaultFontSize.ToString();
            textBox_gridsize.Text = Globals.GridSize.ToString();

            cb_mt.Checked = Globals.PrintViewSettings.ShowMaterialText;
            cb_ot.Checked = Globals.PrintViewSettings.ShowOperatingUnitText;
            cb_ft.Checked = Globals.PrintViewSettings.ShowEdgeText;
            cb_sc.Checked = Globals.PrintViewSettings.ShowComments;
            cb_sp.Checked = Globals.PrintViewSettings.ShowParameters;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Globals.ResetDefaults();
            textBox_fontsize.Text = Globals.DefaultFontSize.ToString();
            textBox_gridsize.Text = Globals.GridSize.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Globals.ResetDefaults();
            int temp = 0;
            temp = ConvertManager.ToInt(textBox_fontsize.Text);
            if (temp > 0)
            {
                Globals.DefaultFontSize = temp;
            }
            temp = ConvertManager.ToInt(textBox_gridsize.Text);
            if (temp > 0)
            {
                Globals.GridSize = temp;
            }

            Globals.PrintViewSettings.ShowMaterialText = cb_mt.Checked;
            Globals.PrintViewSettings.ShowOperatingUnitText = cb_ot.Checked;
            Globals.PrintViewSettings.ShowEdgeText = cb_ft.Checked;
            Globals.PrintViewSettings.ShowComments = cb_sc.Checked;
            Globals.PrintViewSettings.ShowParameters = cb_sp.Checked;

            Changed = true;
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
