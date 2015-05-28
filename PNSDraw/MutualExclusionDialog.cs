using PNSDraw.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNSDraw
{
    public partial class MutualExclusionDialog : Form
    {
        private PGraph graph;
        public MutualExclusionDialog(PGraph graph)
        {
            InitializeComponent();
            this.graph = graph;
        }

        private void RefreshTree()
        {
            treeMutExclusions.BeginUpdate();
            treeMutExclusions.Nodes.Clear();
            List<string> mutExclNames = new List<string>();
            int i = 0;
            foreach (MutualExclusion mutExcl in graph.MutualExclusions)
            {
                treeMutExclusions.Nodes.Add(mutExcl.Name);
                foreach (OperatingUnit ou in mutExcl.OpUnits)
                {
                    treeMutExclusions.Nodes[i].Nodes.Add(ou.Name);
                }
                i++;
            }
            treeMutExclusions.Update();
            treeMutExclusions.EndUpdate();
        }

        private void MutualExclusionDialog_Load(object sender, EventArgs e)
        {
            List<string> opUnitsNames = new List<string>();
            foreach (OperatingUnit ou in graph.OperatingUnits)
            {
                opUnitsNames.Add(ou.Name);
            }
            lstOpUnits.DataSource = opUnitsNames;

            RefreshTree();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lstOpUnits.CheckedItems.Count < 2)
            {
                MessageBox.Show("You must select at least two operating units to create a new mutual exclusion set!");
            }
            else
            {
                List<OperatingUnit> selectedOUs = new List<OperatingUnit>();
                foreach (string ouName in lstOpUnits.CheckedItems)
                {
                    foreach (OperatingUnit ou in graph.OperatingUnits)
                    {
                        if (ouName.Equals(ou.Name))
                        {
                            selectedOUs.Add(ou);
                        }
                    }
                }
                MutualExclusion me = new MutualExclusion(selectedOUs);
                graph.AddMutualExclusion(me);
                RefreshTree();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string selectedName = treeMutExclusions.SelectedNode.Text;
            foreach (MutualExclusion mutExcl in graph.MutualExclusions)
            {
                if (selectedName.Equals(mutExcl.Name))
                {
                    graph.RemoveMutualExclusion(mutExcl);
                    break;
                }
            }
            RefreshTree();
        }
    }
}
