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
using System.Threading.Tasks;
using System.Windows.Forms;

using PNSDraw.Configuration;

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
            selectedMassUnit = Quantity.GetDefaultMUName("Mass") + " (" + Quantity.GetDefaultMUSymbol("Mass") + ")";
            selectedVolumeUnit = Quantity.GetDefaultMUName("Volume") + " (" + Quantity.GetDefaultMUSymbol("Volume") + ")";
            selectedSubstanceUnit = Quantity.GetDefaultMUName("Amount of substance") + " (" + Quantity.GetDefaultMUSymbol("Amount of substance") + ")";
            selectedEnergyUnit = Quantity.GetDefaultMUName("Energy, work, heat") + " (" + Quantity.GetDefaultMUSymbol("Energy, work, heat") + ")";
            selectedLengthUnit = Quantity.GetDefaultMUName("Length") + " (" + Quantity.GetDefaultMUSymbol("Length") + ")";
            selectedCurrentUnit = Quantity.GetDefaultMUName("Electric current") + " (" + Quantity.GetDefaultMUSymbol("Electric current") + ")";
            selectedAreaUnit = Quantity.GetDefaultMUName("Area") + " (" + Quantity.GetDefaultMUSymbol("Area") + ")";
            selectedSpeedUnit = Quantity.GetDefaultMUName("Speed") + " (" + Quantity.GetDefaultMUSymbol("Speed") + ")";
            selectedAccelerationUnit = Quantity.GetDefaultMUName("Acceleration") + " (" + Quantity.GetDefaultMUSymbol("Acceleration") + ")";
            selectedMassDensUnit = Quantity.GetDefaultMUName("Mass density") + " (" + Quantity.GetDefaultMUSymbol("Mass density") + ")";
            selectedThermoTempUnit = Quantity.GetDefaultMUName("Thermodinamic temperature") + " (" + Quantity.GetDefaultMUSymbol("Thermodinamic temperature") + ")";
            selectedLuminIntensUnit = Quantity.GetDefaultMUName("Luminous intensity") + " (" + Quantity.GetDefaultMUSymbol("Luminous intensity") + ")";
            selectedConcentrationUnit = Quantity.GetDefaultMUName("Concentration") + " (" + Quantity.GetDefaultMUSymbol("Concentration") + ")";
            selectedForceUnit = Quantity.GetDefaultMUName("Force") + " (" + Quantity.GetDefaultMUSymbol("Force") + ")";
            selectedPressureUnit = Quantity.GetDefaultMUName("Pressure") + " (" + Quantity.GetDefaultMUSymbol("Pressure") + ")";
            selectedPowerUnit = Quantity.GetDefaultMUName("Power") + " (" + Quantity.GetDefaultMUSymbol("Power") + ")";
            selectedCapacityUnit = Quantity.GetDefaultMUName("Capacity") + " (" + Quantity.GetDefaultMUSymbol("Capacity") + ")";

            cmbDefMat.SelectedIndex = Config.Instance.Material.Type;
            cmbQuantity.SelectedItem = Config.Instance.Quantity.quant_type;
            cmbDefUnit.DataSource = Quantity.GetListOfMUs(cmbQuantity.SelectedItem.ToString());
            SetSelectedUnit();
            cmbMoneyUnit.SelectedIndex = (int)Config.Instance.Quantity.money_mu;
            cmbTimeUnit.SelectedIndex = (int)Config.Instance.Quantity.time_mu;
            numWorkingHour.Value = (decimal)Config.Instance.OperatingUnit.WorkingHoursPerYear;
            numPayoutPeriod.Value = (decimal)Config.Instance.OperatingUnit.PayoutPeriod;
            numMatFlowRateLower.Value = (decimal)Config.Instance.Material.FlowRateLowerBound;
            numMatFlowRateUpper.Value = (decimal)Config.Instance.Material.FlowRateUpperBound;
            numMatPrice.Value = (decimal)Config.Instance.Material.Price;
            numOUCapacityLower.Value = (decimal)Config.Instance.OperatingUnit.CapacityLowerBound;
            numOUCapacityUpper.Value = (decimal)Config.Instance.OperatingUnit.CapacityUpperBound;
            numOFixed.Value = (decimal)Config.Instance.OperatingUnit.OperatingFixCost;
            numOProp.Value = (decimal)Config.Instance.OperatingUnit.OperatingPropCost;
            numIFixed.Value = (decimal)Config.Instance.OperatingUnit.InvestmentFixCost;
            numIProp.Value = (decimal)Config.Instance.OperatingUnit.InvestmentPropCost;
            numFlowRate.Value = (decimal)Config.Instance.Edge.FlowRate;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Config.Instance.Material.Type = cmbDefMat.SelectedIndex;
            Config.Instance.Quantity.quant_type = (string)cmbQuantity.SelectedItem;
            Config.Instance.Quantity.mass_mu = Quantity.GetMUSymbolFromText(selectedMassUnit);
            Config.Instance.Quantity.vol_mu = Quantity.GetMUSymbolFromText(selectedVolumeUnit);
            Config.Instance.Quantity.sub_mu = Quantity.GetMUSymbolFromText(selectedSubstanceUnit);
            Config.Instance.Quantity.energy_mu = Quantity.GetMUSymbolFromText(selectedEnergyUnit);
            Config.Instance.Quantity.length_mu = Quantity.GetMUSymbolFromText(selectedLengthUnit);
            Config.Instance.Quantity.curr_mu = Quantity.GetMUSymbolFromText(selectedCurrentUnit);
            Config.Instance.Quantity.area_mu = Quantity.GetMUSymbolFromText(selectedAreaUnit);
            Config.Instance.Quantity.speed_mu = Quantity.GetMUSymbolFromText(selectedSpeedUnit);
            Config.Instance.Quantity.acc_mu = Quantity.GetMUSymbolFromText(selectedAccelerationUnit);
            Config.Instance.Quantity.mdens_mu = Quantity.GetMUSymbolFromText(selectedMassDensUnit);
            Config.Instance.Quantity.temp_mu = Quantity.GetMUSymbolFromText(selectedThermoTempUnit);
            Config.Instance.Quantity.lum_mu = Quantity.GetMUSymbolFromText(selectedLuminIntensUnit);
            Config.Instance.Quantity.conc_mu = Quantity.GetMUSymbolFromText(selectedConcentrationUnit);
            Config.Instance.Quantity.force_mu = Quantity.GetMUSymbolFromText(selectedForceUnit);
            Config.Instance.Quantity.press_mu = Quantity.GetMUSymbolFromText(selectedPressureUnit);
            Config.Instance.Quantity.power_mu = Quantity.GetMUSymbolFromText(selectedPowerUnit);
            Config.Instance.Quantity.cap_mu = Quantity.GetMUSymbolFromText(selectedCapacityUnit);
            Config.Instance.Quantity.money_mu = (Config.MoneyUnit)cmbMoneyUnit.SelectedIndex;
            Config.Instance.Quantity.time_mu = (Config.TimeUnit)cmbTimeUnit.SelectedIndex;
            Config.Instance.OperatingUnit.WorkingHoursPerYear = (int)numWorkingHour.Value;
            Config.Instance.OperatingUnit.PayoutPeriod = (double)numPayoutPeriod.Value;
            Config.Instance.Material.FlowRateLowerBound = (double)numMatFlowRateLower.Value;
            Config.Instance.Material.FlowRateUpperBound = (double)numMatFlowRateUpper.Value;
            Config.Instance.Material.Price = (double)numMatPrice.Value;
            Config.Instance.OperatingUnit.CapacityLowerBound = (double)numOUCapacityLower.Value;
            Config.Instance.OperatingUnit.CapacityUpperBound = (double)numOUCapacityUpper.Value;
            Config.Instance.OperatingUnit.OperatingFixCost = (double)numOFixed.Value;
            Config.Instance.OperatingUnit.OperatingPropCost = (double)numOProp.Value;
            Config.Instance.OperatingUnit.InvestmentFixCost = (double)numIFixed.Value;
            Config.Instance.OperatingUnit.InvestmentPropCost = (double)numIProp.Value;
            Config.Instance.Edge.FlowRate = (double)numFlowRate.Value;
        }

        private void cmbDefMat_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cmbQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDefUnit.DataSource = Quantity.GetListOfMUs(cmbQuantity.SelectedItem.ToString());
            SetSelectedUnit();
        }

        private void cmbDefUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
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
