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

        public SolverSettingsDialog()
        {
            InitializeComponent();
        }

        private void SolverSettingsDialog_Load(object sender, EventArgs e)
        {
            selectedMassUnit = (int)Default.mass_mu;
            selectedVolumeUnit = (int)Default.vol_mu;
            selectedSubstanceUnit = (int)Default.sub_mu;
            selectedEnergyUnit = (int)Default.energy_mu;
            selectedLengthUnit = (int)Default.length_mu;
            selectedCurrentUnit = (int)Default.curr_mu;
            selectedAreaUnit = (int)Default.area_mu;
            selectedSpeedUnit = (int)Default.speed_mu;
            selectedAccelerationUnit = (int)Default.acc_mu;
            selectedMassDensUnit = (int)Default.mdens_mu;
            selectedThermoTempUnit = (int)Default.temp_mu;
            selectedLuminIntensUnit = (int)Default.lum_mu;
            selectedConcentrationUnit = (int)Default.conc_mu;
            selectedForceUnit = (int)Default.force_mu;
            selectedPressureUnit = (int)Default.press_mu;
            selectedPowerUnit = (int)Default.power_mu;
            selectedCapacityUnit = (int)Default.cap_mu;

            cmbDefMat.SelectedIndex = Default.type;
            cmbDefUnit.SelectedIndex = selectedMassUnit;
            cmbQuantity.SelectedIndex = 0;
            cmbMoneyUnit.SelectedIndex = (int)Default.money_mu;
            cmbTimeUnit.SelectedIndex = (int)Default.time_mu;
            numWorkingHour.Value = (decimal)Default.worging_hours_per_year;
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
            Default.mass_mu = (Default.MassUnit)selectedMassUnit;
            Default.vol_mu = (Default.VolumeUnit)selectedVolumeUnit;
            Default.sub_mu = (Default.SubstanceUnit)selectedSubstanceUnit;
            Default.energy_mu = (Default.EnergyUnit)selectedEnergyUnit;
            Default.length_mu = (Default.LengthUnit)selectedLengthUnit;
            Default.curr_mu = (Default.CurrentUnit)selectedCurrentUnit;
            Default.area_mu = (Default.AreaUnit)selectedAreaUnit;
            Default.speed_mu = (Default.SpeedUnit)selectedSpeedUnit;
            Default.acc_mu = (Default.AccelerationUnit)selectedAccelerationUnit;
            Default.mdens_mu = (Default.MassDensityUnit)selectedMassDensUnit;
            Default.temp_mu = (Default.ThermoTempUnit)selectedThermoTempUnit;
            Default.lum_mu = (Default.LuminIntensUnit)selectedLuminIntensUnit;
            Default.conc_mu = (Default.ConcentrationUnit)selectedConcentrationUnit;
            Default.force_mu = (Default.ForceUnit)selectedForceUnit;
            Default.press_mu = (Default.PressureUnit)selectedPressureUnit;
            Default.power_mu = (Default.PowerUnit)selectedPowerUnit;
            Default.cap_mu = (Default.CapacityUnit)selectedCapacityUnit;
            Default.money_mu = (Default.MoneyUnit)cmbMoneyUnit.SelectedIndex;
            Default.time_mu = (Default.TimeUnit)cmbTimeUnit.SelectedIndex;
            Default.worging_hours_per_year = (int)numWorkingHour.Value;
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
            switch (cmbQuantity.SelectedIndex)
            {
                case 0:
                    cmbDefUnit.DataSource = massUnits;
                    cmbDefUnit.SelectedIndex = selectedMassUnit;
                    break;
                case 1:
                    cmbDefUnit.DataSource = volumeUnits;
                    cmbDefUnit.SelectedIndex = selectedVolumeUnit;
                    break;
                case 2:
                    cmbDefUnit.DataSource = substanceUnits;
                    cmbDefUnit.SelectedIndex = selectedSubstanceUnit;
                    break;
                case 3:
                    cmbDefUnit.DataSource = energyUnits;
                    cmbDefUnit.SelectedIndex = selectedEnergyUnit;
                    break;
                case 4:
                    cmbDefUnit.DataSource = lengthUnits;
                    cmbDefUnit.SelectedIndex = selectedLengthUnit;
                    break;
                case 5:
                    cmbDefUnit.DataSource = currentUnits;
                    cmbDefUnit.SelectedIndex = selectedCurrentUnit;
                    break;
                case 6:
                    cmbDefUnit.DataSource = areaUnits;
                    cmbDefUnit.SelectedIndex = selectedAreaUnit;
                    break;
                case 7:
                    cmbDefUnit.DataSource = speedUnits;
                    cmbDefUnit.SelectedIndex = selectedSpeedUnit;
                    break;
                case 8:
                    cmbDefUnit.DataSource = accelerationUnits;
                    cmbDefUnit.SelectedIndex = selectedAccelerationUnit;
                    break;
                case 9:
                    cmbDefUnit.DataSource = massDensUnits;
                    cmbDefUnit.SelectedIndex = selectedMassDensUnit;
                    break;
                case 10:
                    cmbDefUnit.DataSource = thermoTempUnits;
                    cmbDefUnit.SelectedIndex = selectedThermoTempUnit;
                    break;
                case 11:
                    cmbDefUnit.DataSource = luminIntensUnits;
                    cmbDefUnit.SelectedIndex = selectedLuminIntensUnit;
                    break;
                case 12:
                    cmbDefUnit.DataSource = concentrationUnits;
                    cmbDefUnit.SelectedIndex = selectedConcentrationUnit;
                    break;
                case 13:
                    cmbDefUnit.DataSource = forceUnits;
                    cmbDefUnit.SelectedIndex = selectedForceUnit;
                    break;
                case 14:
                    cmbDefUnit.DataSource = pressureUnits;
                    cmbDefUnit.SelectedIndex = selectedPressureUnit;
                    break;
                case 15:
                    cmbDefUnit.DataSource = powerUnits;
                    cmbDefUnit.SelectedIndex = selectedPowerUnit;
                    break;
                case 16:
                    cmbDefUnit.DataSource = capacityUnits;
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
