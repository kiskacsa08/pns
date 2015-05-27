using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DynamicPropertyGrid;
using Pns.Xml_Serialization.PnsDefaults;
using Pns.Xml_Serialization.PnsProblem;
using Pns.Xml_Serialization.PnsGUI.Dialogs.DefMUs;
using Pns.Globals;

namespace Pns.Dialogs
{
    public partial class DefaultMU : Form
    {
        #region Members
        private bool m_cancel;
        #endregion

        #region Constructors
        public DefaultMU()
        {
            InitializeComponent();
            Text = def_DefaultMU.dialog_text;
            buttonUpdate.Text = def_DefaultMU.button_update_text;
            buttonCancel.Text = def_DefaultMU.button_cancel_text;
            buttonDefault.Text = def_DefaultMU.button_default_text;
            propertyGridDefaultMU.SelectedObject = new MeasurementUnitsProperties(DefaultMUsAndValues.MUs);
            m_cancel = true;
        }
        #endregion

        #region Event handlers
        private void propertyGridDefaultMU_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            MeasurementUnitsProperties t_MUs_prop = ((MeasurementUnitsProperties)propertyGridDefaultMU.SelectedObject);
            t_MUs_prop.UpdateFields();
            t_MUs_prop.Refresh();
            propertyGridDefaultMU.Refresh();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            ((MeasurementUnitsProperties)propertyGridDefaultMU.SelectedObject).UpdateFields();
            m_cancel = false;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            DefaultMUsAndValues.MUs.Reset();
            ((MeasurementUnitsProperties)propertyGridDefaultMU.SelectedObject).Reset();
            propertyGridDefaultMU.Refresh();
        }

        private void DefaultMU_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_cancel) ((MeasurementUnitsProperties)propertyGridDefaultMU.SelectedObject).Restore();
        }
        #endregion
    }

    public class MeasurementUnits
    {
        #region Members
        private MUCategories f_categories;
        private List<MU> f_backup_mus;
        private MUCategory f_def_mat_category;
        private double f_def_payoutPeriod;
        private int f_def_workingHoursPerYear;
        private FractionMUs f_cost_mus;
        private bool f_autoconvert;
        #endregion

        #region Constructors
        public MeasurementUnits()
        {
            f_categories = new MUCategories();
            f_cost_mus = null;
            Reset();
        }
        public MeasurementUnits(Problem defaults)
        {
            f_categories = new MUCategories();
            f_cost_mus = null;
            if (defaults != null && defaults.defaultMeasurementUnits != null)
            {
                foreach (XMLMU item in defaults.defaultMeasurementUnits)
                {
                    MUCategory t_category = GetDerivedCategoryFromMU(item, true);
                    if (t_category != null) t_category.DefaultMU = FindMU(item);
                }
                f_def_mat_category = f_categories.Find(defaults.materialQuantityTypeId != def_Values.d_NperA ? defaults.materialQuantityTypeId : def_Values.default_material_quantity_type_id);
                f_autoconvert = def_Values.b_auto_convert;
                return;
            }
            Reset();
        }
        public MeasurementUnits(XMLDefaults defaults)
        {
            f_categories = new MUCategories();
            f_cost_mus = null;
            if (defaults != null && defaults.default_measurement_units != null)
            {
                foreach (XMLMU item in defaults.default_measurement_units)
                {
                    MUCategory t_category = GetDerivedCategoryFromMU(item, true);
                    if (t_category != null) t_category.DefaultMU = FindMU(item);
                }
                f_def_mat_category = f_categories.Find(defaults.materialQuantityTypeId != def_Values.d_NperA ? defaults.materialQuantityTypeId : def_Values.default_material_quantity_type_id);
                f_def_workingHoursPerYear = defaults.workingHoursPerYear != (int)def_Values.d_NperA ? defaults.workingHoursPerYear : def_Values.worging_hours_per_year;
                f_def_payoutPeriod = defaults.payoutPeriod != def_Values.d_NperA ? defaults.payoutPeriod : def_Values.payout_period;
                f_autoconvert = def_Values.b_auto_convert;
                return;
            }
            Reset();
        }
        #endregion

        #region Member functions
        public MU FindMU(XMLMU mu) { return f_categories.FindMU(mu); }
        public MUCategory GetDerivedCategoryFromMU(MU mu) { return f_categories.GetDerivedCategoryFromMU(mu); }
        public MUCategory GetDerivedCategoryFromMU(XMLMU mu) { return f_categories.GetDerivedCategoryFromMU(mu); }
        public MUCategory GetDerivedCategoryFromMU(int t_group_id, int t_item_id) { return f_categories.GetDerivedCategoryFromMU(t_group_id, t_item_id); }
        public MUCategory GetDerivedCategoryFromMU(MU mu, bool t_null_allowed) { return f_categories.GetDerivedCategoryFromMU(mu, t_null_allowed); }
        public MUCategory GetDerivedCategoryFromMU(XMLMU mu, bool t_null_allowed) { return f_categories.GetDerivedCategoryFromMU(mu, t_null_allowed); }
        public MUCategory GetDerivedCategoryFromMU(int t_group_id, int t_item_id, bool t_null_allowed) { return f_categories.GetDerivedCategoryFromMU(t_group_id, t_item_id, t_null_allowed); }
        public void Reset()
        {
            foreach (MUCategory t_item in f_categories) t_item.DefaultMU = t_item.MUList[0];
            List<XMLMU> t_def_mus = new List<XMLMU>();
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_lenght));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_mass, (int)defaults.MU_Groups_Mass.tonne));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_time, (int)defaults.MU_Groups_Time.year));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.base_group, (int)defaults.MU_Base_Quantities.b_electric_current));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_amount_of_substance, (int)defaults.MU_Groups_Amount_of_substance.kilomole));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_currency, (int)defaults.MU_Groups_Currency.euro));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_area, (int)defaults.MU_Groups_Area.square_meter));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_volume, (int)defaults.MU_Groups_Volume.cubic_meter));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_speed, (int)defaults.MU_Groups_Speed.meter_per_second));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_acceleration, (int)defaults.MU_Groups_Acceleration.meter_per_second_squared));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_density, (int)defaults.MU_Groups_Mass_density.tonne_per_cubic_meter));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_concentration, (int)defaults.MU_Groups_Concentration.mole_per_cubic_decimeter));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_force, (int)defaults.MU_Groups_Force.newton));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_pressure, (int)defaults.MU_Groups_Pressure.megapascal));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_energy, (int)defaults.MU_Groups_Energy.megajoule));
            t_def_mus.Add(ToXMLMU((int)defaults.MU_Groups.d_power, (int)defaults.MU_Groups_Power.megawatt));
            foreach (XMLMU item in t_def_mus)
            {
                MUCategory t_cat = GetDerivedCategoryFromMU(item, true);
                if (t_cat != null) t_cat.DefaultMU = FindMU(item);
            }
            f_def_mat_category = f_categories.Find(def_Values.default_material_quantity_type_id);
            f_def_payoutPeriod = def_Values.payout_period;
            f_def_workingHoursPerYear = def_Values.worging_hours_per_year;
            f_autoconvert = def_Values.b_auto_convert;
        }
        public List<XMLMU> GetDefMUs()
        {
            List<XMLMU> t_def_mus = new List<XMLMU>();
            foreach (MUCategory item in Categories) t_def_mus.Add(item.DefaultMU.ToXMLMU());
            return t_def_mus;
        }
        static private XMLMU ToXMLMU(int t_category, int t_item_id)
        {
            XMLMU xmlmu = new XMLMU();
            xmlmu.group_id = t_category;
            xmlmu.item_id = t_item_id;
            return xmlmu;
        }
        public void ToXML(Problem xml_tag)
        {
            xml_tag.defaultMeasurementUnits = GetDefMUs();
            xml_tag.materialQuantityTypeId = MaterialQuantityTypeId;
            xml_tag.payoutPeriod = f_def_payoutPeriod;
            xml_tag.workingHoursPerYear = f_def_workingHoursPerYear;
            xml_tag.massMu = DefaultMaterialMU.ToString();
            xml_tag.timeMu = DefaultTimeMU.ToString();
            xml_tag.currencyMu = DefaultCurrencyMU.ToString();
            xml_tag.requiredRawFlowMu = DefaultMaterialQuantity.DefaultFlowMU.ToString();
            xml_tag.requiredIntermediateFlowMu = DefaultMaterialQuantity.DefaultFlowMU.ToString();
            xml_tag.requiredProductFlowMu = DefaultMaterialQuantity.DefaultFlowMU.ToString();
            xml_tag.maximumRawFlowMu = DefaultMaterialQuantity.DefaultFlowMU.ToString();
            xml_tag.maximumIntermediateFlowMu = DefaultMaterialQuantity.DefaultFlowMU.ToString();
            xml_tag.maximumProductFlowMu = DefaultMaterialQuantity.DefaultFlowMU.ToString();
            xml_tag.priceMu = DefaultMaterialQuantity.DefaultPriceMU.ToString();
            xml_tag.flowRateMu = DefaultMaterialQuantity.DefaultFlowMU.ToString();
        }
        public void ToXML(XMLDefaults xml_tag)
        {
            xml_tag.default_measurement_units = GetDefMUs();
            xml_tag.materialQuantityTypeId = MaterialQuantityTypeId;
            xml_tag.payoutPeriod = f_def_payoutPeriod;
            xml_tag.workingHoursPerYear = f_def_workingHoursPerYear;
        }
        #endregion

        #region Properties
        public List<MU> OldDefaultMUs { get { return f_backup_mus; } set { f_backup_mus = value; } }
        public MUCategories Categories { get { return f_categories; } }
        public MUCategory DefaultMaterialQuantity { get { return f_def_mat_category; } set { f_def_mat_category = value; } }
        public int DefaultWorkingHoursPerYear { get { return f_def_workingHoursPerYear; } set { f_def_workingHoursPerYear = value; } }
        public double DefaultPayoutPeriod { get { return f_def_payoutPeriod; } set { f_def_payoutPeriod = value; } }
        public MU DefaultMaterialMU { get { return f_def_mat_category.DefaultMU; } }
        public MUs TimeMUs { get { return f_categories.Find((int)defaults.MU_Groups.d_time).MUList; } }
        public MU DefaultTimeMU { get { return f_categories.Find((int)defaults.MU_Groups.d_time).DefaultMU; } }
        public MUs CurrencyMUs { get { return f_categories.Find((int)defaults.MU_Groups.d_currency).MUList; } }
        public MU DefaultCurrencyMU { get { return f_categories.Find((int)defaults.MU_Groups.d_currency).DefaultMU; } }
        public FractionMUs CostMUs
        {
            get
            {
                if (f_cost_mus == null) { f_cost_mus = new FractionMUs(GetDerivedCategoryFromMU(DefaultCurrencyMU).MUList, GetDerivedCategoryFromMU(DefaultTimeMU).MUList); }
                return f_cost_mus;
            }
        }
        public FractionMU DefaultCostMU { get { return CostMUs.Find(DefaultCurrencyMU, DefaultTimeMU); } }
        public int MaterialQuantityTypeId { get { return DefaultMaterialQuantity.CategoryID; } }
        public bool AutoConvert { get { return f_autoconvert; } set { f_autoconvert = value; } }
        #endregion
    }

    public class MeasurementUnitsProperties : CustomClass
    {
        #region Members
        private MeasurementUnits f_def_units;
        private int f_def_units_startindex;
        private int f_def_quantity_index;
        private MUCategory f_def_quantity;
        private int f_working_hour_per_year_index;
        private int f_working_hour_per_year;
        private int f_payout_period_index;
        private double f_payout_period;
        private int f_autoconvert_index;
        private bool f_autoconvert;
        private int f_flomu_index;
        private int f_pricemu_index;
        private int f_invcostmu_index;
        private int f_opcostmu_index;
        private MU f_quantitimu;
        private MU f_currencymu;
        private MU f_timemu;
        #endregion

        #region Constructors
        public MeasurementUnitsProperties(MeasurementUnits def_units)
        {
            f_def_units = def_units;
            Backup();
            Reset();
        }
        #endregion

        #region Member functions
        public void Reset()
        {
            Clear();
            f_def_units_startindex = Count;
            foreach (MUCategory item in f_def_units.Categories)
            {
                Add(new CustomProp(this.GetType(), item.MUList, item.Category, item.DefaultMU,
                    def_MeasurementUnitsPropGrid.D_def_unit, def_MeasurementUnitsPropGrid.Cat_def_units, typeof(MU), false, true));
            }
            f_def_quantity_index = Count;
            Add(new CustomProp(this.GetType(), f_def_units.Categories, def_MeasurementUnitsPropGrid.DN_def_quantity,
                f_def_units.DefaultMaterialQuantity, def_MeasurementUnitsPropGrid.D_def_quantity, def_MeasurementUnitsPropGrid.Cat_def_quantity, typeof(MUCategory), false, true));
            f_working_hour_per_year_index = Count;
            Add(new CustomProp(this.GetType(), null, def_MeasurementUnitsPropGrid.DN_def_working_hour_per_year,
                WorkingHoursPerYear, def_MeasurementUnitsPropGrid.D_def_working_hour_per_year, def_MeasurementUnitsPropGrid.Cat_def_time_ratios, typeof(string), false, true));
            f_payout_period_index = Count;
            Add(new CustomProp(this.GetType(), null, def_MeasurementUnitsPropGrid.DN_def_payout_period,
                PayoutPeriod, def_MeasurementUnitsPropGrid.D_def_payout_period, def_MeasurementUnitsPropGrid.Cat_def_time_ratios, typeof(string), false, true));
            UpdateMUs();
            f_flomu_index = Count;
            Add(new CustomProp(this.GetType(), null, def_MeasurementUnitsPropGrid.DN_def_quantity_flow,
                DefaultMaterialFlowMU, def_MeasurementUnitsPropGrid.D_def_quantity_flow, def_MeasurementUnitsPropGrid.Cat_def_derived_units, typeof(string), true, true));
            f_pricemu_index = Count;
            Add(new CustomProp(this.GetType(), null, def_MeasurementUnitsPropGrid.DN_def_price,
                DefaultPriceMU, def_MeasurementUnitsPropGrid.D_def_price, def_MeasurementUnitsPropGrid.Cat_def_derived_units, typeof(string), true, true));
            f_invcostmu_index = Count;
            Add(new CustomProp(this.GetType(), null, def_MeasurementUnitsPropGrid.DN_def_invcost,
                DefaultInvCostMU, def_MeasurementUnitsPropGrid.D_def_invcost, def_MeasurementUnitsPropGrid.Cat_def_derived_units, typeof(string), true, true));
            f_opcostmu_index = Count;
            Add(new CustomProp(this.GetType(), null, def_MeasurementUnitsPropGrid.DN_def_opcost,
                DefaultOpCostMU, def_MeasurementUnitsPropGrid.D_def_opcost, def_MeasurementUnitsPropGrid.Cat_def_derived_units, typeof(string), true, true));
            f_autoconvert_index = Count;
            Add(new CustomProp(this.GetType(), null, def_MeasurementUnitsPropGrid.DN_MU_conversion,
                f_def_units.AutoConvert, def_MeasurementUnitsPropGrid.D_MU_conversion, def_MeasurementUnitsPropGrid.Cat_MU_conversion, typeof(bool), false, true));
        }
        public void UpdateFields()
        {
            int i = f_def_units_startindex;
            foreach (MUCategory item in f_def_units.Categories) item.DefaultMU = (MU)((CustomProp)List[i++]).Value;
            f_def_units.DefaultMaterialQuantity = (MUCategory)((CustomProp)List[f_def_quantity_index]).Value;
            WorkingHoursPerYear = (string)((CustomProp)List[f_working_hour_per_year_index]).Value;
            PayoutPeriod = (string)((CustomProp)List[f_payout_period_index]).Value;
            f_def_units.AutoConvert = (bool)((CustomProp)List[f_autoconvert_index]).Value;
        }
        private void UpdateMUs()
        {
            MUCategory t_category = (MUCategory)((CustomProp)List[f_def_quantity_index]).Value;
            for (int i = f_def_units_startindex; i < Count; i++)
            {
                CustomProp t_prop = (CustomProp)List[i];
                if (t_prop.Type == typeof(MU))
                {
                    MU t_mu = (MU)t_prop.Value;
                    if (t_category == DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(t_mu)) f_quantitimu = t_mu;
                    else if (DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(t_mu).CategoryID == (int)defaults.MU_Groups.d_currency) f_currencymu = t_mu;
                    else if (DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(t_mu).CategoryID == (int)defaults.MU_Groups.d_time) f_timemu = t_mu;
                }
            }
        }
        public void Refresh()
        {
            UpdateMUs();
            ((CustomProp)List[f_working_hour_per_year_index]).Value = WorkingHoursPerYear;
            ((CustomProp)List[f_payout_period_index]).Value = PayoutPeriod;
            ((CustomProp)List[f_autoconvert_index]).Value = f_def_units.AutoConvert;
            ((CustomProp)List[f_flomu_index]).Value = DefaultMaterialFlowMU;
            ((CustomProp)List[f_pricemu_index]).Value = DefaultPriceMU;
            ((CustomProp)List[f_invcostmu_index]).Value = DefaultInvCostMU;
            ((CustomProp)List[f_opcostmu_index]).Value = DefaultOpCostMU;
        }
        private void Backup()
        {
            f_def_units.OldDefaultMUs = new List<MU>();
            foreach (MUCategory item in f_def_units.Categories) f_def_units.OldDefaultMUs.Add(item.DefaultMU);
            f_def_quantity = f_def_units.DefaultMaterialQuantity;
            f_payout_period = f_def_units.DefaultPayoutPeriod;
            f_working_hour_per_year = f_def_units.DefaultWorkingHoursPerYear;
            f_autoconvert = f_def_units.AutoConvert;
        }
        public void Restore()
        {
            int i = -1;
            foreach (MUCategory item in f_def_units.Categories) item.DefaultMU = f_def_units.OldDefaultMUs[++i];
            f_def_units.DefaultMaterialQuantity = f_def_quantity;
            f_def_units.DefaultPayoutPeriod = f_payout_period;
            f_def_units.DefaultWorkingHoursPerYear = f_working_hour_per_year;
            f_def_units.AutoConvert = f_autoconvert;
        }
        #endregion

        #region Properties
        public string WorkingHoursPerYear 
        { 
            get { return f_def_units.DefaultWorkingHoursPerYear + " h/yr"; } 
            set 
            { 
                int t_value = (int)Converters.ToDouble(1, value, 8760);
                if (t_value != def_Values.d_NperA) f_def_units.DefaultWorkingHoursPerYear = t_value;
                else f_def_units.DefaultWorkingHoursPerYear = def_Values.worging_hours_per_year;
            } 
        }
        public string PayoutPeriod 
        { 
            get { return f_def_units.DefaultPayoutPeriod + " yr/payout period"; } 
            set 
            { 
                double t_value = Converters.ToDouble(1, value);
                if(t_value!=def_Values.d_NperA) f_def_units.DefaultPayoutPeriod = t_value;
                else f_def_units.DefaultPayoutPeriod = def_Values.payout_period;
            } 
        }
        public string DefaultMaterialFlowMU { get { return DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(f_quantitimu).FlowMUs.Find(f_quantitimu, f_timemu).ToString(); } }
        public string DefaultPriceMU { get { return DefaultMUsAndValues.MUs.GetDerivedCategoryFromMU(f_quantitimu).PriceMUs.Find(f_currencymu, f_quantitimu).ToString(); } }
        public string DefaultInvCostMU { get { return f_currencymu.ToString(); } }
        public string DefaultOpCostMU { get { return DefaultMUsAndValues.MUs.CostMUs.Find(f_currencymu, f_timemu).ToString(); } }
        #endregion
    }
}
