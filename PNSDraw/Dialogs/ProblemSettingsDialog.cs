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
        //private string[] massUnits = new string[] { "gram", "kilogram", "ton" };
        //private string[] volumeUnits = new string[] { "cubic meter", "cubic decimeter", "cubic centimeter" };
        //private string[] substanceUnits = new string[] { "mole", "millimole", "kilomole" };
        //private string[] energyUnits = new string[] { "joule", "kilojoule", "megajoule", "gigajoule", "terajoule", "watthour", "kilowatthour", "megawatthour", "gigawatthour", "terawatthour" };
        //private string[] lengthUnits = new string[] { "meter", "kilometer", "decimeter", "centimeter", "millimeter", "coll" };
        //private string[] currentUnits = new string[] { "ampere", "milliampere", "kiloampere" };
        //private string[] areaUnits = new string[] { "square meter", "square kilometer", "square centimeter", "hectar" };
        //private string[] speedUnits = new string[] { "meters per second", "kilometers per hour", "miles per hour" };
        //private string[] accelerationUnits = new string[] { "meter per second squared" };
        //private string[] massDensUnits = new string[] { "kilogram per cubic meter", "ton per cubic meter" };
        //private string[] thermoTempUnits = new string[] { "kelvin" };
        //private string[] luminIntensUnits = new string[] { "candela" };
        //private string[] concentrationUnits = new string[] { "mole per cubic meter", "mole per cubic decimeter" };
        //private string[] forceUnits = new string[] { "newton" };
        //private string[] pressureUnits = new string[] { "pascal", "kilopascal", "megapascal" };
        //private string[] powerUnits = new string[] { "watt", "kilowatt", "megawatt", "gigawatt", "terawatt" };
        //private string[] capacityUnits = new string[] { "unit" };

        private string selectedMassUnit;
        private string selectedVolumeUnit;
        private string selectedSubstanceUnit;
        private string selectedEnergyUnit;
        private string selectedLengthUnit;
        private string selectedCurrentUnit;
        private string selectedAreaUnit;
        private string selectedSpeedUnit;
        private string selectedAccelerationUnit;
        private string selectedMassDensUnit;
        private string selectedThermoTempUnit;
        private string selectedLuminIntensUnit;
        private string selectedConcentrationUnit;
        private string selectedForceUnit;
        private string selectedPressureUnit;
        private string selectedPowerUnit;
        private string selectedCapacityUnit;

        public ProblemSettingsDialog()
        {
            InitializeComponent();
        }

        private void SolverSettingsDialog_Load(object sender, EventArgs e)
        {
            selectedMassUnit = Default.GetDefaultMUName("Mass") + " (" + Default.GetDefaultMUSymbol("Mass") + ")";
            selectedVolumeUnit = Default.GetDefaultMUName("Volume") + " (" + Default.GetDefaultMUSymbol("Volume") + ")";
            selectedSubstanceUnit = Default.GetDefaultMUName("Amount of substance") + " (" + Default.GetDefaultMUSymbol("Amount of substance") + ")";
            selectedEnergyUnit = Default.GetDefaultMUName("Energy, work, heat") + " (" + Default.GetDefaultMUSymbol("Energy, work, heat") + ")";
            selectedLengthUnit = Default.GetDefaultMUName("Length") + " (" + Default.GetDefaultMUSymbol("Length") + ")";
            selectedCurrentUnit = Default.GetDefaultMUName("Electric current") + " (" + Default.GetDefaultMUSymbol("Electric current") + ")";
            selectedAreaUnit = Default.GetDefaultMUName("Area") + " (" + Default.GetDefaultMUSymbol("Area") + ")";
            selectedSpeedUnit = Default.GetDefaultMUName("Speed") + " (" + Default.GetDefaultMUSymbol("Speed") + ")";
            selectedAccelerationUnit = Default.GetDefaultMUName("Acceleration") + " (" + Default.GetDefaultMUSymbol("Acceleration") + ")";
            selectedMassDensUnit = Default.GetDefaultMUName("Mass density") + " (" + Default.GetDefaultMUSymbol("Mass density") + ")";
            selectedThermoTempUnit = Default.GetDefaultMUName("Thermodinamic temperature") + " (" + Default.GetDefaultMUSymbol("Thermodinamic temperature") + ")";
            selectedLuminIntensUnit = Default.GetDefaultMUName("Luminous intensity") + " (" + Default.GetDefaultMUSymbol("Luminous intensity") + ")";
            selectedConcentrationUnit = Default.GetDefaultMUName("Concentration") + " (" + Default.GetDefaultMUSymbol("Concentration") + ")";
            selectedForceUnit = Default.GetDefaultMUName("Force") + " (" + Default.GetDefaultMUSymbol("Force") + ")";
            selectedPressureUnit = Default.GetDefaultMUName("Pressure") + " (" + Default.GetDefaultMUSymbol("Pressure") + ")";
            selectedPowerUnit = Default.GetDefaultMUName("Power") + " (" + Default.GetDefaultMUSymbol("Power") + ")";
            selectedCapacityUnit = Default.GetDefaultMUName("Capacity") + " (" + Default.GetDefaultMUSymbol("Capacity") + ")";

            cmbDefMat.SelectedIndex = Default.type;
            cmbQuantity.SelectedItem = Default.quant_type;
            cmbDefUnit.DataSource = Default.GetListOfMUs(cmbQuantity.SelectedItem.ToString());
            SetSelectedUnit();
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
            Default.quant_type = (string)cmbQuantity.SelectedItem;
            Default.mass_mu = Default.GetMUSymbolFromText(selectedMassUnit);
            Default.vol_mu = Default.GetMUSymbolFromText(selectedVolumeUnit);
            Default.sub_mu = Default.GetMUSymbolFromText(selectedSubstanceUnit);
            Default.energy_mu = Default.GetMUSymbolFromText(selectedEnergyUnit);
            Default.length_mu = Default.GetMUSymbolFromText(selectedLengthUnit);
            Default.curr_mu = Default.GetMUSymbolFromText(selectedCurrentUnit);
            Default.area_mu = Default.GetMUSymbolFromText(selectedAreaUnit);
            Default.speed_mu = Default.GetMUSymbolFromText(selectedSpeedUnit);
            Default.acc_mu = Default.GetMUSymbolFromText(selectedAccelerationUnit);
            Default.mdens_mu = Default.GetMUSymbolFromText(selectedMassDensUnit);
            Default.temp_mu = Default.GetMUSymbolFromText(selectedThermoTempUnit);
            Default.lum_mu = Default.GetMUSymbolFromText(selectedLuminIntensUnit);
            Default.conc_mu = Default.GetMUSymbolFromText(selectedConcentrationUnit);
            Default.force_mu = Default.GetMUSymbolFromText(selectedForceUnit);
            Default.press_mu = Default.GetMUSymbolFromText(selectedPressureUnit);
            Default.power_mu = Default.GetMUSymbolFromText(selectedPowerUnit);
            Default.cap_mu = Default.GetMUSymbolFromText(selectedCapacityUnit);
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
            if (btnApply.Enabled == false)
            {
                btnApply.Enabled = true;
            }
            cmbDefUnit.DataSource = Default.GetListOfMUs(cmbQuantity.SelectedItem.ToString());
            SetSelectedUnit();
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
                    selectedMassUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 1:
                    selectedVolumeUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 2:
                    selectedSubstanceUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 3:
                    selectedEnergyUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 4:
                    selectedLengthUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 5:
                    selectedCurrentUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 6:
                    selectedAreaUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 7:
                    selectedSpeedUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 8:
                    selectedAccelerationUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 9:
                    selectedMassDensUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 10:
                    selectedThermoTempUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 11:
                    selectedLuminIntensUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 12:
                    selectedConcentrationUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 13:
                    selectedForceUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 14:
                    selectedPressureUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 15:
                    selectedPowerUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                case 16:
                    selectedCapacityUnit = cmbDefUnit.SelectedItem.ToString();
                    break;
                default:
                    break;
            }
        }

        private void SetSelectedUnit()
        {
            switch (cmbQuantity.SelectedItem.ToString())
            {
                case "Mass":
                    cmbDefUnit.SelectedItem = selectedMassUnit;
                    break;
                case "Volume":
                    cmbDefUnit.SelectedItem = selectedVolumeUnit;
                    break;
                case "Amount of substance":
                    cmbDefUnit.SelectedItem = selectedSubstanceUnit;
                    break;
                case "Energy, work, heat":
                    cmbDefUnit.SelectedItem = selectedEnergyUnit;
                    break;
                case "Length":
                    cmbDefUnit.SelectedItem = selectedLengthUnit;
                    break;
                case "Electric current":
                    cmbDefUnit.SelectedItem = selectedCurrentUnit;
                    break;
                case "Area":
                    cmbDefUnit.SelectedItem = selectedAreaUnit;
                    break;
                case "Speed":
                    cmbDefUnit.SelectedItem = selectedSpeedUnit;
                    break;
                case "Acceleration":
                    cmbDefUnit.SelectedItem = selectedAccelerationUnit;
                    break;
                case "Mass density":
                    cmbDefUnit.SelectedItem = selectedMassDensUnit;
                    break;
                case "Thermodinamic temperature":
                    cmbDefUnit.SelectedItem = selectedThermoTempUnit;
                    break;
                case "Luminous intensity":
                    cmbDefUnit.SelectedItem = selectedLuminIntensUnit;
                    break;
                case "Concentration":
                    cmbDefUnit.SelectedItem = selectedConcentrationUnit;
                    break;
                case "Force":
                    cmbDefUnit.SelectedItem = selectedForceUnit;
                    break;
                case "Pressure":
                    cmbDefUnit.SelectedItem = selectedPressureUnit;
                    break;
                case "Power":
                    cmbDefUnit.SelectedItem = selectedPowerUnit;
                    break;
                case "Capacity":
                    cmbDefUnit.SelectedItem = selectedCapacityUnit;
                    break;
                default:
                    break;
            }
        }

    }
}
