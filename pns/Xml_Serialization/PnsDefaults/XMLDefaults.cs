using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Pns.Xml_Serialization.PnsProblem;
using Pns.Globals;
using Pns.Dialogs;

namespace Pns.Xml_Serialization.PnsDefaults
{
    [XmlRoot("pns_defaults")]
    public partial class XMLDefaults
    {
        #region Members
        private List<XMLMU> quantityMUsField;
        private int materialQuantityTypeIdField;
        private double payoutPeriodField;
        private int workingHoursPerYearField;
        private double minimumFlowField;
        private double maximumFlowField;
        private double priceField;
        private double flowrateField;
        private double lowerBoundField;
        private double upperBoundField;
        private double oFixedChargeField;
        private double oPropConstantField;
        private double iFixedChargeField;
        private double iPropConstantField;
        private double solverUpperLimitField;
        private int maxSolutionsField;
        private MU_XML MeasurementUnitsDatabaseField;
        #endregion

        #region Constructors
        public XMLDefaults()
        {
            Init();
        }
        public XMLDefaults(bool t_create_new)
        {
            Init();
            if (t_create_new)
            {
                DefaultMUsAndValues.MUDB = new MU_XML(true);
                DefaultMUsAndValues.MUs = new MeasurementUnits(this);
                DefaultMUsAndValues.DefaultValues = new DefaultValuesProperties(this);
            }
            MeasurementUnitsDatabaseField = DefaultMUsAndValues.MUDB;
            DefaultMUsAndValues.MUs.ToXML(this);
            DefaultMUsAndValues.DefaultValues.ToXML(this);
        }
        #endregion

        #region Member functions
        private void Init()
        {
            materialQuantityTypeIdField = (int)def_Values.d_NperA;
            payoutPeriodField = def_Values.d_NperA;
            workingHoursPerYearField = (int)def_Values.d_NperA;
            minimumFlowField = def_Values.d_NperA;
            maximumFlowField = def_Values.d_NperA;
            priceField = def_Values.d_NperA;
            flowrateField = def_Values.d_NperA;
            lowerBoundField = def_Values.d_NperA;
            upperBoundField = def_Values.d_NperA;
            oFixedChargeField = def_Values.d_NperA;
            oPropConstantField = def_Values.d_NperA;
            iFixedChargeField = def_Values.d_NperA;
            iPropConstantField = def_Values.d_NperA;
            solverUpperLimitField = def_Values.d_NperA;
            maxSolutionsField = (int)def_Values.d_NperA;
        }
        #endregion

        #region Properties
        [XmlArrayItem("unit")]
        public List<XMLMU> default_measurement_units { get { return quantityMUsField; } set { quantityMUsField = value; } }
        [DefaultValue((int)def_Values.d_NperA)]
        public int materialQuantityTypeId { get { return materialQuantityTypeIdField; } set { materialQuantityTypeIdField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double payoutPeriod { get { return payoutPeriodField; } set { payoutPeriodField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public int workingHoursPerYear { get { return workingHoursPerYearField; } set { workingHoursPerYearField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double minimumFlow { get { return minimumFlowField; } set { minimumFlowField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double maximumFlow { get { return maximumFlowField; } set { maximumFlowField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double price { get { return priceField; } set { priceField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double flowrate { get { return flowrateField; } set { flowrateField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double lowerBound { get { return lowerBoundField; } set { lowerBoundField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double upperBound { get { return upperBoundField; } set { upperBoundField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double fixedCharge { get { return oFixedChargeField; } set { oFixedChargeField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double propConstant { get { return oPropConstantField; } set { oPropConstantField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double iFixedCharge { get { return iFixedChargeField; } set { iFixedChargeField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double iPropConstant { get { return iPropConstantField; } set { iPropConstantField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double solverUpperLimit { get { return solverUpperLimitField; } set { solverUpperLimitField = value; } }
        [DefaultValue((int)def_Values.d_NperA)]
        public int maxSolutions { get { return maxSolutionsField; } set { maxSolutionsField = value; } }
        [XmlElement("mu_database")]
        public MU_XML MeasurementUnitsDatabase { get { return MeasurementUnitsDatabaseField; } set { MeasurementUnitsDatabaseField = value; } }
        #endregion
    }
}
