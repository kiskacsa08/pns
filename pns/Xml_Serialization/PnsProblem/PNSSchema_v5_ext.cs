using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using Pns.Globals;


// Az xsd által generált xml tároló osztályok kiegészítése a mértékegységek tárolásához
// szükséges field és property elemekkel.

namespace Pns.Xml_Serialization.PnsProblem
{

    public partial class XMLMU
    {
        private int group_idField;
        private int item_idField;
        [XmlAttribute]
        public int group_id { get { return group_idField; } set { group_idField = value; } }
        [XmlAttribute]
        public int item_id { get { return item_idField; } set { item_idField = value; } }
    }

    public partial class XMLFlowMU
    {
        private XMLMU quantity_muField;
        private XMLMU time_muField;

        public XMLMU quantity_mu { get { return quantity_muField; } set { quantity_muField = value; } }
        public XMLMU time_mu { get { return time_muField; } set { time_muField = value; } }
    }

    public partial class XMLPriceMU
    {
        private XMLMU currency_muField;
        private XMLMU quantity_muField;

        public XMLMU currency_mu { get { return currency_muField; } set { currency_muField = value; } }
        public XMLMU quantity_mu { get { return quantity_muField; } set { quantity_muField = value; } }
    }

    public partial class XMLMatMU
    {
        private XMLFlowMU reqFlowMUField;
        private XMLFlowMU maxFlowMUField;
        private XMLPriceMU priceMUField;

        public XMLFlowMU reqFlowMU { get { return reqFlowMUField; } set { reqFlowMUField = value; } }
        public XMLFlowMU maxFlowMU { get { return maxFlowMUField; } set { maxFlowMUField = value; } }
        public XMLPriceMU priceMU { get { return priceMUField; } set { priceMUField = value; } }
    }

    public partial class XMLCostMU
    {
        private XMLMU currency_muField;
        private XMLMU time_muField;

        public XMLMU currency_mu { get { return currency_muField; } set { currency_muField = value; } }
        public XMLMU time_mu { get { return time_muField; } set { time_muField = value; } }
    }

    public partial class ProductMaterial
    {
        private int categoryField = (int)def_Values.d_NperA;
        private XMLMatMU matMUField;
        private string descriptionField;

        [XmlAttribute()]
        [DefaultValue((int)def_Values.d_NperA)]
        public int category { get { return categoryField; } set { categoryField = value; } }
        public XMLMatMU matMU { get { return matMUField; } set { matMUField = value; } }
        public string description { get { return descriptionField; } set { descriptionField = value; } }
    }

    public partial class IntermediateMaterial
    {
        private int categoryField = (int)def_Values.d_NperA;
        private XMLMatMU matMUField;
        private string descriptionField;

        [XmlAttribute()]
        [DefaultValue((int)def_Values.d_NperA)]
        public int category { get { return categoryField; } set { categoryField = value; } }
        public XMLMatMU matMU { get { return matMUField; } set { matMUField = value; } }
        public string description { get { return descriptionField; } set { descriptionField = value; } }
    }

    public partial class RawMaterial
    {
        private int categoryField = (int)def_Values.d_NperA;
        private XMLMatMU matMUField;
        private string descriptionField;

        [XmlAttribute()]
        [DefaultValue((int)def_Values.d_NperA)]
        public int category { get { return categoryField; } set { categoryField = value; } }
        public XMLMatMU matMU { get { return matMUField; } set { matMUField = value; } }
        public string description { get { return descriptionField; } set { descriptionField = value; } }
    }

    public partial class FlowMaterial
    {
        private int rateposxField;
        private int rateposyField;
        private XMLFlowMU flowrateMUField;

        public XMLFlowMU flowrateMU { get { return flowrateMUField; } set { flowrateMUField = value; } }
        [XmlAttribute()]
        public int ratePosX { get { return this.rateposxField; } set { this.rateposxField = value; } }
        [XmlAttribute()]
        public int ratePosY { get { return this.rateposyField; } set { this.rateposyField = value; } }
    }

    public partial class OperatingUnit
    {
        private XMLCostMU operatingFixMUField;
        private XMLCostMU operatingPropMUField;
        private XMLMU investmentFixMUField;
        private XMLMU investmentPropMUField;
        private XMLCostMU totalFixMUField;
        private XMLCostMU totalPropMUField;
        private int workingHoursPerYearField = (int)def_Values.d_NperA;
        private double payoutPeriodField = def_Values.d_NperA;
        private string descriptionField;

        public XMLCostMU operatingFixMU { get { return operatingFixMUField; } set { operatingFixMUField = value; } }
        public XMLCostMU operatingPropMU { get { return operatingPropMUField; } set { operatingPropMUField = value; } }
        public XMLMU investmentFixMU { get { return investmentFixMUField; } set { investmentFixMUField = value; } }
        public XMLMU investmentPropMU { get { return investmentPropMUField; } set { investmentPropMUField = value; } }
        public XMLCostMU totalFixMU { get { return totalFixMUField; } set { totalFixMUField = value; } }
        public XMLCostMU totalPropMU { get { return totalPropMUField; } set { totalPropMUField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public int workingHoursPerYear { get { return workingHoursPerYearField; } set { workingHoursPerYearField = value; } }
        [DefaultValue(def_Values.d_NperA)]
        public double payoutPeriod { get { return payoutPeriodField; } set { payoutPeriodField = value; } }
        public string description { get { return descriptionField; } set { descriptionField = value; } }
    }

    public partial class Problem
    {
        private List<XMLMU> quantityMUsField;
        private int materialQuantityTypeIdField = (int)def_Values.d_NperA;
        private double minimumFlowField = def_Values.d_NperA;
        private double maximumFlowField = def_Values.d_NperA;
        private double priceField = def_Values.d_NperA;
        private double flowrateField = def_Values.d_NperA;
        private double solverUpperLimitField = def_Values.d_NperA;
        private int maxSolutionsField = (int)def_Values.d_NperA;
        private Pns.MU_XML MeasurementUnitsDatabaseField;

        [XmlArrayItem("unit")]
        public List<XMLMU> defaultMeasurementUnits { get { return quantityMUsField; } set { quantityMUsField = value; } }
        [XmlAttribute()]
        [DefaultValue((int)def_Values.d_NperA)]
        public int materialQuantityTypeId { get { return materialQuantityTypeIdField; } set { materialQuantityTypeIdField = value; } }
        [XmlAttribute()]
        [DefaultValue(def_Values.d_NperA)]
        public double minimumFlow { get { return minimumFlowField; } set { minimumFlowField = value; } }
        [XmlAttribute()]
        [DefaultValue(def_Values.d_NperA)]
        public double maximumFlow { get { return maximumFlowField; } set { maximumFlowField = value; } }
        [XmlAttribute()]
        [DefaultValue(def_Values.d_NperA)]
        public double price { get { return priceField; } set { priceField = value; } }
        [XmlAttribute()]
        [DefaultValue(def_Values.d_NperA)]
        public double flowrate { get { return flowrateField; } set { flowrateField = value; } }
        [XmlAttribute()]
        [DefaultValue(def_Values.d_NperA)]
        public double solverUpperLimit { get { return solverUpperLimitField; } set { solverUpperLimitField = value; } }
        [DefaultValue((int)def_Values.d_NperA)]
        public int maxSolutions { get { return maxSolutionsField; } set { maxSolutionsField = value; } }
        [XmlElement("mu_database")]
        public Pns.MU_XML MeasurementUnitsDatabase { get { return MeasurementUnitsDatabaseField; } set { MeasurementUnitsDatabaseField = value; } }
    }
}