using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.Dialogs.DefMUs
{
    public class def_MeasurementUnitsPropGrid
    {
        #region Members
        static public string Cat_def_units = "\t\t\t\t\tDefault measurement units of available quantity types";
        static public string D_def_unit = "Selecting this quantity type item as material quantity type," +
            " item value will be used to generate default quantity based derived measurement units.";

        static public string Cat_def_time_ratios = "\t\t\t\tDefault ratios of time units";
        static public string DN_def_working_hour_per_year = "Default working hour per year value";
        static public string D_def_working_hour_per_year = "This is the default annual operation time of the operating units." +
            " This ratio will be used when converting time based quantities." +
            "You can change this value individually in operating unit settings.";
        static public string DN_def_payout_period = "Default payout period value";
        static public string D_def_payout_period = "It is the length of investment payback period in year. It can be fraction number." +
            "This ratio will be used when converting time based quantities." +
            "You can change this value individually in operating unit settings.";

        static public string Cat_def_quantity = "\t\t\tDefault quantity type";
        static public string DN_def_quantity = "Default quantity type";
        static public string D_def_quantity = "When you create a new material," +
            " this quantity type will be selected automatically.";

        static public string Cat_def_derived_units = "\t\tDefault derived units based on quantity, time and currency";
        static public string DN_def_quantity_flow = "Default material flow MU";
        static public string D_def_quantity_flow = "Default measurement unit of material flow.";
        static public string DN_def_price = "Default price MU";
        static public string D_def_price = "Default measurement unit of price.";
        static public string DN_def_invcost = "Default investment cost MU";
        static public string D_def_invcost = "Default measurement unit of investment cost.";
        static public string DN_def_opcost = "Default operating cost MU";
        static public string D_def_opcost = "Default measurement unit of operating cost.";

        static public string Cat_MU_conversion = "\tDefault measurement unit conversion";
        static public string DN_MU_conversion = "Automatically convert values";
        static public string D_MU_conversion = "If true then change of measurement unit will convert the value automatically.";
        #endregion

        #region Properties
        public string DefaultMUsCategory { get { return Cat_def_units; } set { Cat_def_units = value; } }
        public PropertyGridItemXMLTag DefaultMUs
        {
            get { return new PropertyGridItemXMLTag(null, D_def_unit); }
            set
            {
                D_def_unit = value.Description;
            }
        }
        public string DefaultTimeRatiosCategory { get { return Cat_def_time_ratios; } set { Cat_def_time_ratios = value; } }
        public PropertyGridItemXMLTag DefaultWorkingHoursPerYear
        {
            get { return new PropertyGridItemXMLTag(DN_def_working_hour_per_year, D_def_working_hour_per_year); }
            set
            {
                DN_def_working_hour_per_year = value.DisplayName;
                D_def_working_hour_per_year = value.Description;
            }
        }
        public PropertyGridItemXMLTag DefaultPayoutPeriod
        {
            get { return new PropertyGridItemXMLTag(DN_def_payout_period, D_def_payout_period); }
            set
            {
                DN_def_payout_period = value.DisplayName;
                D_def_payout_period = value.Description;
            }
        }
        public string DefaultQuantityCategory { get { return Cat_def_quantity; } set { Cat_def_quantity = value; } }
        public PropertyGridItemXMLTag DefaultQuantity
        {
            get { return new PropertyGridItemXMLTag(DN_def_quantity, D_def_quantity); }
            set
            {
                DN_def_quantity = value.DisplayName;
                D_def_quantity = value.Description;
            }
        }
        public string DefaultDerivedUnitsCategory { get { return Cat_def_derived_units; } set { Cat_def_derived_units = value; } }
        public PropertyGridItemXMLTag DefaultQuantityFlowMU
        {
            get { return new PropertyGridItemXMLTag(DN_def_quantity_flow, D_def_quantity_flow); }
            set
            {
                DN_def_quantity_flow = value.DisplayName;
                D_def_quantity_flow = value.Description;
            }
        }
        public PropertyGridItemXMLTag DefaultPriceMU
        {
            get { return new PropertyGridItemXMLTag(DN_def_price, D_def_price); }
            set
            {
                DN_def_price = value.DisplayName;
                D_def_price = value.Description;
            }
        }
        public PropertyGridItemXMLTag DefaultInvestmentCostMU
        {
            get { return new PropertyGridItemXMLTag(DN_def_invcost, D_def_invcost); }
            set
            {
                DN_def_invcost = value.DisplayName;
                D_def_invcost = value.Description;
            }
        }
        public PropertyGridItemXMLTag DefaultOperatingCostMU
        {
            get { return new PropertyGridItemXMLTag(DN_def_opcost, D_def_opcost); }
            set
            {
                DN_def_opcost = value.DisplayName;
                D_def_opcost = value.Description;
            }
        }
        public string DefaultMUConversionCategory { get { return Cat_MU_conversion; } set { Cat_MU_conversion = value; } }
        public PropertyGridItemXMLTag DefaultMUConversion
        {
            get { return new PropertyGridItemXMLTag(DN_MU_conversion, D_MU_conversion); }
            set
            {
                DN_MU_conversion = value.DisplayName;
                D_MU_conversion = value.Description;
            }
        }
        #endregion
    }
}
