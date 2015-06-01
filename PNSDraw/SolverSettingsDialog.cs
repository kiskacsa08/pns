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
    public partial class SolverSettingsDialog : Form
    {
        public SolverSettingsDialog()
        {
            InitializeComponent();
        }

        private void SolverSettingsDialog_Load(object sender, EventArgs e)
        {
            cmbDefMat.SelectedIndex = Default.type;
            cmbMassUnit.SelectedIndex = (int)Default.mass_mu;
            cmbMoneyUnit.SelectedIndex = (int)Default.money_mu;
            cmbTimeUnit.SelectedIndex = (int)Default.time_mu;
            numMatFlowRateLower.Value = (decimal)Default.flow_rate_lower_bound;
            numMatFlowRateUpper.Value = (decimal)Default.flow_rate_upper_bound;
            numMatPrice.Value = (decimal)Default.price;
            numOUCapacityLower.Value = (decimal)Default.capacity_lower_bound;
            numOUCapacityUpper.Value = (decimal)Default.capacity_upper_bound;
            numOUFixCost.Value = (decimal)Default.fix_cost;
            numOUPropCost.Value = (decimal)Default.prop_cost;
            numFlowRate.Value = (decimal)Default.io_flowrate;
            btnApply.Enabled = false;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Default.type = cmbDefMat.SelectedIndex;
            Default.mass_mu = (Default.MassUnit)cmbMassUnit.SelectedIndex;
            Default.money_mu = (Default.MoneyUnit)cmbMoneyUnit.SelectedIndex;
            Default.time_mu = (Default.TimeUnit)cmbTimeUnit.SelectedIndex;
            Default.flow_rate_lower_bound = (double)numMatFlowRateLower.Value;
            Default.flow_rate_upper_bound = (double)numMatFlowRateUpper.Value;
            Default.price = (double)numMatPrice.Value;
            Default.capacity_lower_bound = (double)numOUCapacityLower.Value;
            Default.capacity_upper_bound = (double)numOUCapacityUpper.Value;
            Default.fix_cost = (double)numOUFixCost.Value;
            Default.prop_cost = (double)numOUPropCost.Value;
            Default.io_flowrate = (double)numFlowRate.Value;
            btnApply.Enabled = false;
        }

        private void cmbDefMat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (btnApply.Enabled == false)
            {
                btnApply.Enabled = true;
            }
        }

    }
}
