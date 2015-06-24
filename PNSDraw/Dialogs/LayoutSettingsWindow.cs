using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            textBox_layer.Text = (Globals.DefaultLayerDistance * Globals.GridSize).ToString();
            textBox_node.Text  = (Globals.DefaultNodeDistance * Globals.GridSize).ToString();
            gridsize_label.Text = "Current grid size: " + Globals.GridSize;
            checkBox_weightedarcs.Checked = Globals.WeightedArcs;
            checkBox_fixedraws.Checked = Globals.FixedRaws;
            checkBox_fixedproducts.Checked = Globals.FixedProducts;
            comboBox_engine.SelectedIndex = comboBox_engine.FindStringExact(Globals.selectedEngine);
            comboBox1.SelectedIndex = comboBox1.FindStringExact(Globals.Align);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int temp = 0;
            temp = ConvertManager.ToInt(textBox_layer.Text);
            if (temp > 0)
            {
                Globals.DefaultLayerDistance = temp/Globals.GridSize;
            }
            temp = ConvertManager.ToInt(textBox_node.Text);
            if (temp > 0)
            {
                Globals.DefaultNodeDistance = temp / Globals.GridSize;
            }
            Globals.WeightedArcs = checkBox_weightedarcs.Checked;
            Globals.FixedRaws = checkBox_fixedraws.Checked;
            Globals.FixedProducts = checkBox_fixedproducts.Checked;
            Globals.selectedEngine = (string)comboBox_engine.SelectedItem;

            if ((string)comboBox1.SelectedItem != Globals.Align) 
            {
               int temp_opheight=Globals.OperatingUnitHeight;
                int temp_opwidth=Globals.OperatingUnitWidth;
                int temp_DefaultRootX=Globals.DefaultRootX;
                int temp_DefaultRootY=Globals.DefaultRootY;
                int temp_DefaultLayerDistance=Globals.DefaultLayerDistance;
                int temp_DefaultNodeDistance= Globals.DefaultNodeDistance;

                Globals.OperatingUnitHeight = temp_opwidth;
                Globals.OperatingUnitWidth = temp_opheight;
                //Globals.DefaultRootX = temp_DefaultRootY;
                //Globals.DefaultRootY = temp_DefaultRootX;
                //Globals.DefaultLayerDistance = temp_DefaultNodeDistance;
                //Globals.DefaultNodeDistance = temp_DefaultLayerDistance;
                Globals.Align = (string)comboBox1.SelectedItem;
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
