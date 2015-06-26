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
    public partial class ProblemSettingsDialog : Form
    {
        private string[] massUnits = new string[] { "gram", "kilogram", "ton" };
        private string[] volumeUnits = new string[] { "cubic meter", "cubic decimeter", "cubic centimeter" };
        private string[] substanceUnits = new string[] { "mole", "millimole", "kilomole" };
        private string[] energyUnits = new string[] { "joule", "kilojoule", "megajoule", "gigajoule", "terajoule", "watthour", "kilowatthour", "megawatthour", "gigawatthour", "terawatthour" };
        private string[] lengthUnits = new string[] { "meter", "kilometer", "decimeter", "centimeter", "millimeter", "coll" };
        private string[] currentUnits = new string[] { "ampere", "milliampere", "kiloampere" };
        private string[] areaUnits = new string[] { "square meter", "square kilometer", "square centimeter", "hectar" };
        private string[] speedUnits = new string[] { "meters per second", "kilometers per hour", "miles per hour" };
        private string[] accelerationUnits = new string[] { "meter per second squared" };
        private string[] massDensUnits = new string[] { "kilogram per cubic meter", "ton per cubic meter" };
        private string[] thermoTempUnits = new string[] { "kelvin" };
        private string[] luminIntensUnits = new string[] { "candela" };
        private string[] concentrationUnits = new string[] { "mole per cubic meter", "mole per cubic decimeter" };
        private string[] forceUnits = new string[] { "newton" };
        private string[] pressureUnits = new string[] { "pascal", "kilopascal", "megapascal" };
        private string[] powerUnits = new string[] { "watt", "kilowatt", "megawatt", "gigawatt", "terawatt" };
        private string[] capacityUnits = new string[] { "unit" };

        private int selectedMassUnit;
        private int selectedVolumeUnit;
        private int selectedSubstanceUnit;
        private int selectedEnergyUnit;
        private int selectedLengthUnit;
        private int selectedCurrentUnit;
        private int selectedAreaUnit;
        private int selectedSpeedUnit;
        private int selectedAccelerationUnit;
        private int selectedMassDensUnit;
        private int selectedThermoTempUnit;
        private int selectedLuminIntensUnit;
        private int selectedConcentrationUnit;
        private int selectedForceUnit;
        private int selectedPressureUnit;
        private int selectedPowerUnit;
        private int selectedCapacityUnit;

        public ProblemSettingsDialog()
        {
            InitializeComponent();
        }

        private void SolverSettingsDialog_Load(object sender, EventArgs e)
        {
            selectedMassUnit = Array.IndexOf(Default.quantities["Mass"], Default.mass_mu);
            selectedVolumeUnit = Array.IndexOf(Default.quantities["Volume"], Default.vol_mu);
            selectedSubstanceUnit = Array.IndexOf(Default.quantities["Amount of substance"], Default.sub_mu);
            selectedEnergyUnit = Array.IndexOf(Default.quantities["Energy, work, heat"], Default.energy_mu);
            selectedLengthUnit = Array.IndexOf(Default.quantities["Length"], Default.length_mu);
            selectedCurrentUnit = Array.IndexOf(Default.quantities["Electric current"], Default.curr_mu);
            selectedAreaUnit = Array.IndexOf(Default.quantities["Area"], Default.area_mu);
            selectedSpeedUnit = Array.IndexOf(Default.quantities["Speed"], Default.speed_mu);
            selectedAccelerationUnit = Array.IndexOf(Default.quantities["Acceleration"], Default.acc_mu);
            selectedMassDensUnit = Array.IndexOf(Default.quantities["Mass density"], Default.mdens_mu);
            selectedThermoTempUnit = Array.IndexOf(Default.quantities["Thermodinamic temperature"], Default.temp_mu);
            selectedLuminIntensUnit = Array.IndexOf(Default.quantities["Luminous intensity"], Default.lum_mu);
            selectedConcentrationUnit = Array.IndexOf(Default.quantities["Concentration"], Default.conc_mu);
            selectedForceUnit = Array.IndexOf(Default.quantities["Force"], Default.force_mu);
            selectedPressureUnit = Array.IndexOf(Default.quantities["Pressure"], Default.press_mu);
            selectedPowerUnit = Array.IndexOf(Default.quantities["Power"], Default.power_mu);
            selectedCapacityUnit = Array.IndexOf(Default.quantities["Capacity"], Default.cap_mu);

            cmbDefMat.SelectedIndex = Default.type;
            cmbDefUnit.SelectedIndex = selectedMassUnit;
            cmbQuantity.SelectedIndex = 0;
            cmbMoneyUnit.SelectedIndex = (int)Default.money_mu;
            cmbTimeUnit.SelectedIndex = (int)Default.time_mu;
            numWorkingHour.Value = (decimal)Default.working_hours_per_year;
            numPayoutPeriod.Value = (decimal)Default.payout_period;
            numMatFlowRateLower.Value = (decimal)Default.flow_rate_lower_bound;
            numMatFlowRateUpper.Value = (decimal)Default.flow_rate_upper_bound;
            numMatPrice.Value = (decimal)Default.price;
            numOUCapacityLower.Value = (decimal)Default.capacity_lower_bound;
            numOUCapacityUpper.Value = (decimal)Default.capacity_upper_bound;
            numOFixed.Value = (decimal)Default.o_fix;
            numOProp.Value = (decimal)Default.o_prop;
            numIFixed.Value = (decimal)Default.i_fix;
            numIProp.Value = (decimal)Default.i_prop;
            numFlowRate.Value = (decimal)Default.io_flowrate;
            btnApply.Enabled = false;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Default.type = cmbDefMat.SelectedIndex;
            Default.mass_mu = Default.quantities["Mass"][selectedMassUnit];
            Default.vol_mu = Default.quantities["Volume"][selectedVolumeUnit];
            Default.sub_mu = Default.quantities["Amount of substance"][selectedSubstanceUnit];
            Default.energy_mu = Default.quantities["Energy, work, heat"][selectedEnergyUnit];
            Default.length_mu = Default.quantities["Length"][selectedLengthUnit];
            Default.curr_mu = Default.quantities["Electric current"][selectedCurrentUnit];
            Default.area_mu = Default.quantities["Area"][selectedAreaUnit];
            Default.speed_mu = Default.quantities["Speed"][selectedSpeedUnit];
            Default.acc_mu = Default.quantities["Acceleration"][selectedAccelerationUnit];
            Default.mdens_mu = Default.quantities["Mass density"][selectedMassDensUnit];
            Default.temp_mu = Default.quantities["Thermodinamic temperature"][selectedThermoTempUnit];
            Default.lum_mu = Default.quantities["Luminous intensity"][selectedLuminIntensUnit];
            Default.conc_mu = Default.quantities["Concentration"][selectedConcentrationUnit];
            Default.force_mu = Default.quantities["Force"][selectedForceUnit];
            Default.press_mu = Default.quantities["Pressure"][selectedPressureUnit];
            Default.power_mu = Default.quantities["Power"][selectedPowerUnit];
            Default.cap_mu = Default.quantities["Capacity"][selectedCapacityUnit];
            Default.money_mu = (Default.MoneyUnit)cmbMoneyUnit.SelectedIndex;
            Default.time_mu = (Default.TimeUnit)cmbTimeUnit.SelectedIndex;
            Default.working_hours_per_year = (int)numWorkingHour.Value;
            Default.payout_period = (double)numPayoutPeriod.Value;
            Default.flow_rate_lower_bound = (double)numMatFlowRateLower.Value;
            Default.flow_rate_upper_bound = (double)numMatFlowRateUpper.Value;
            Default.price = (double)numMatPrice.Value;
            Default.capacity_lower_bound = (double)numOUCapacityLower.Value;
            Default.capacity_upper_bound = (double)numOUCapacityUpper.Value;
            Default.o_fix = (double)numOFixed.Value;
            Default.o_prop = (double)numOProp.Value;
            Default.i_fix = (double)numIFixed.Value;
            Default.i_prop = (double)numIProp.Value;
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

        private void cmbQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDefUnit.DataSource = Default.quantities[cmbQuantity.SelectedItem.ToString()];

            switch (cmbQuantity.SelectedIndex)
            {
                case 0:
                    cmbDefUnit.SelectedIndex = selectedMassUnit;
                    break;
                case 1:
                    cmbDefUnit.SelectedIndex = selectedVolumeUnit;
                    break;
                case 2:
                    cmbDefUnit.SelectedIndex = selectedSubstanceUnit;
                    break;
                case 3:
                    cmbDefUnit.SelectedIndex = selectedEnergyUnit;
                    break;
                case 4:
                    cmbDefUnit.SelectedIndex = selectedLengthUnit;
                    break;
                case 5:
                    cmbDefUnit.SelectedIndex = selectedCurrentUnit;
                    break;
                case 6:
                    cmbDefUnit.SelectedIndex = selectedAreaUnit;
                    break;
                case 7:
                    cmbDefUnit.SelectedIndex = selectedSpeedUnit;
                    break;
                case 8:
                    cmbDefUnit.SelectedIndex = selectedAccelerationUnit;
                    break;
                case 9:
                    cmbDefUnit.SelectedIndex = selectedMassDensUnit;
                    break;
                case 10:
                    cmbDefUnit.SelectedIndex = selectedThermoTempUnit;
                    break;
                case 11:
                    cmbDefUnit.SelectedIndex = selectedLuminIntensUnit;
                    break;
                case 12:
                    cmbDefUnit.SelectedIndex = selectedConcentrationUnit;
                    break;
                case 13:
                    cmbDefUnit.SelectedIndex = selectedForceUnit;
                    break;
                case 14:
                    cmbDefUnit.SelectedIndex = selectedPressureUnit;
                    break;
                case 15:
                    cmbDefUnit.SelectedIndex = selectedPowerUnit;
                    break;
                case 16:
                    cmbDefUnit.SelectedIndex = selectedCapacityUnit;
                    break;
                default:
                    break;
            }
        }

        private void cmbDefUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (btnApply.Enabled == false)
            {
                btnApply.Enabled = true;
            }

            switch (cmbQuantity.SelectedIndex)
            {
                case 0:
                    selectedMassUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 1:
                    selectedVolumeUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 2:
                    selectedSubstanceUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 3:
                    selectedEnergyUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 4:
                    selectedLengthUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 5:
                    selectedCurrentUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 6:
                    selectedAreaUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 7:
                    selectedSpeedUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 8:
                    selectedAccelerationUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 9:
                    selectedMassDensUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 10:
                    selectedThermoTempUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 11:
                    selectedLuminIntensUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 12:
                    selectedConcentrationUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 13:
                    selectedForceUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 14:
                    selectedPressureUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 15:
                    selectedPowerUnit = cmbDefUnit.SelectedIndex;
                    break;
                case 16:
                    selectedCapacityUnit = cmbDefUnit.SelectedIndex;
                    break;
                default:
                    break;
            }
        }

    }
}
