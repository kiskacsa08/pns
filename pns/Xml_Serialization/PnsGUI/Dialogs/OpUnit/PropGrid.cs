using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.Dialogs.OpUnit
{
    public class def_ou_prop
    {
        #region Members
        static public string cat1 = "\t\t\t\t\tBasic";
        static public string DN_name = "Name";
        static public string D_name = "Name of the operating unit. It must be unique in the problem definition.";
        static public string DN_working_hours_per_year = "Working hours per year";
        static public string D_working_hours_per_year = "";
        static public string DN_payout_period = "Payout period";
        static public string D_payout_period = "";

        static public string cat2 = "\t\tInput and output streams";
        static public string DN_imats = "Input materials";
        static public string D_imats = "";
        static public string DN_omats = "Output materials";
        static public string D_omats = "";

        static public string cat3 = "\t\t\tCost parameters";
        static public string DN_ocost = "Operating cost";
        static public string D_ocost = "";
        static public string DN_ofix = "Fixed charge";
        static public string D_ofix = "";
        static public string DN_ofixmu = "Fixed charge Mu";
        static public string D_ofixmu = "";
        static public string DN_oprop = "Proportionality constant";
        static public string D_oprop = "";
        static public string DN_opropmu = "Proportionality constant Mu";
        static public string D_opropmu = "";

        static public string DN_icost = "Investment cost";
        static public string D_icost = "";
        static public string DN_ifix = "Fixed charge";
        static public string D_ifix = "";
        static public string DN_ifixmu = "Fixed charge Mu";
        static public string D_ifixmu = "";
        static public string DN_iprop = "Proportionality constant";
        static public string D_iprop = "";
        static public string DN_ipropmu = "Proportionality constant Mu";
        static public string D_ipropmu = "";

        static public string DN_overallcost = "Overall cost";
        static public string D_overallcost = "";
        static public string DN_fix = "Fixed charge";
        static public string D_fix = "";
        static public string DN_fixmu = "Fixed charge Mu";
        static public string D_fixmu = "";
        static public string DN_prop = "Proportionality constant";
        static public string D_prop = "";
        static public string DN_propmu = "Proportionality constant Mu";
        static public string D_propmu = "";

        static public string cat4 = "\t\t\t\tCapacity constraints";
        static public string DN_bounds = "Capacity constraints";
        static public string D_bounds = "";
        static public string DN_lb = "Lower bound";
        static public string D_lb = "";
        static public string DN_ub = "Upper bound";
        static public string D_ub = "";

        static public string cat5 = "\tDescription";
        static public string DN_description = "Description";
        static public string D_description = "Description";
        #endregion

        #region Properties
        public string BasicCategory { get { return cat1; } set { cat1 = value; } }
        public PropertyGridItemXMLTag Name
        {
            get { return new PropertyGridItemXMLTag(DN_name, D_name); }
            set
            {
                DN_name = value.DisplayName;
                D_name = value.Description;
            }
        }
        public PropertyGridItemXMLTag WorkingHoursPerYear
        {
            get { return new PropertyGridItemXMLTag(DN_working_hours_per_year, D_working_hours_per_year); }
            set
            {
                DN_working_hours_per_year = value.DisplayName;
                D_working_hours_per_year = value.Description;
            }
        }
        public PropertyGridItemXMLTag PayoutPeriod
        {
            get { return new PropertyGridItemXMLTag(DN_payout_period, D_payout_period); }
            set
            {
                DN_payout_period = value.DisplayName;
                D_payout_period = value.Description;
            }
        }
        public string IOStreamsCategory { get { return cat2; } set { cat2 = value; } }
        public PropertyGridItemXMLTag InputStreams
        {
            get { return new PropertyGridItemXMLTag(DN_imats, D_imats); }
            set
            {
                DN_imats = value.DisplayName;
                D_imats = value.Description;
            }
        }
        public PropertyGridItemXMLTag OutputStreams
        {
            get { return new PropertyGridItemXMLTag(DN_omats, D_omats); }
            set
            {
                DN_omats = value.DisplayName;
                D_omats = value.Description;
            }
        }
        public string CostParamsCategory { get { return cat3; } set { cat3 = value; } }
        public PropertyGridItemXMLTag OperatingCost
        {
            get { return new PropertyGridItemXMLTag(DN_ocost, D_ocost); }
            set
            {
                DN_ocost = value.DisplayName;
                D_ocost = value.Description;
            }
        }
        public PropertyGridItemXMLTag OperatingFixCost
        {
            get { return new PropertyGridItemXMLTag(DN_ofix, D_ofix); }
            set
            {
                DN_ofix = value.DisplayName;
                D_ofix = value.Description;
            }
        }
        public PropertyGridItemXMLTag OperatingFixCostMU
        {
            get { return new PropertyGridItemXMLTag(DN_ofixmu, D_ofixmu); }
            set
            {
                DN_ofixmu = value.DisplayName;
                D_ofixmu = value.Description;
            }
        }
        public PropertyGridItemXMLTag OperatingPropCost
        {
            get { return new PropertyGridItemXMLTag(DN_oprop, D_oprop); }
            set
            {
                DN_oprop = value.DisplayName;
                D_oprop = value.Description;
            }
        }
        public PropertyGridItemXMLTag OperatingPropCostMU
        {
            get { return new PropertyGridItemXMLTag(DN_opropmu, D_opropmu); }
            set
            {
                DN_opropmu = value.DisplayName;
                D_opropmu = value.Description;
            }
        }
        public PropertyGridItemXMLTag InvestmentCost
        {
            get { return new PropertyGridItemXMLTag(DN_icost, D_icost); }
            set
            {
                DN_icost = value.DisplayName;
                D_icost = value.Description;
            }
        }
        public PropertyGridItemXMLTag InvestmentFixCost
        {
            get { return new PropertyGridItemXMLTag(DN_ifix, D_ifix); }
            set
            {
                DN_ifix = value.DisplayName;
                D_ifix = value.Description;
            }
        }
        public PropertyGridItemXMLTag InvestmentFixCostMU
        {
            get { return new PropertyGridItemXMLTag(DN_ifixmu, D_ifixmu); }
            set
            {
                DN_ifixmu = value.DisplayName;
                D_ifixmu = value.Description;
            }
        }
        public PropertyGridItemXMLTag InvestmentPropCost
        {
            get { return new PropertyGridItemXMLTag(DN_iprop, D_iprop); }
            set
            {
                DN_iprop = value.DisplayName;
                D_iprop = value.Description;
            }
        }
        public PropertyGridItemXMLTag InvestmentPropCostMU
        {
            get { return new PropertyGridItemXMLTag(DN_ipropmu, D_ipropmu); }
            set
            {
                DN_ipropmu = value.DisplayName;
                D_ipropmu = value.Description;
            }
        }
        public PropertyGridItemXMLTag OverallCost
        {
            get { return new PropertyGridItemXMLTag(DN_overallcost, D_overallcost); }
            set
            {
                DN_overallcost = value.DisplayName;
                D_overallcost = value.Description;
            }
        }
        public PropertyGridItemXMLTag OverallFixCost
        {
            get { return new PropertyGridItemXMLTag(DN_fix, D_fix); }
            set
            {
                DN_fix = value.DisplayName;
                D_fix = value.Description;
            }
        }
        public PropertyGridItemXMLTag OverallFixCostMU
        {
            get { return new PropertyGridItemXMLTag(DN_fixmu, D_fixmu); }
            set
            {
                DN_fixmu = value.DisplayName;
                D_fixmu = value.Description;
            }
        }
        public PropertyGridItemXMLTag OverallPropCost
        {
            get { return new PropertyGridItemXMLTag(DN_prop, D_prop); }
            set
            {
                DN_prop = value.DisplayName;
                D_prop = value.Description;
            }
        }
        public PropertyGridItemXMLTag OverallPropCostMU
        {
            get { return new PropertyGridItemXMLTag(DN_propmu, D_propmu); }
            set
            {
                DN_propmu = value.DisplayName;
                D_propmu = value.Description;
            }
        }
        public string CapacityCategory { get { return cat4; } set { cat4 = value; } }
        public PropertyGridItemXMLTag Bounds
        {
            get { return new PropertyGridItemXMLTag(DN_bounds, D_bounds); }
            set
            {
                DN_bounds = value.DisplayName;
                D_bounds = value.Description;
            }
        }
        public PropertyGridItemXMLTag LowerBound
        {
            get { return new PropertyGridItemXMLTag(DN_lb, D_lb); }
            set
            {
                DN_lb = value.DisplayName;
                D_lb = value.Description;
            }
        }
        public PropertyGridItemXMLTag UpperBound
        {
            get { return new PropertyGridItemXMLTag(DN_ub, D_ub); }
            set
            {
                DN_ub = value.DisplayName;
                D_ub = value.Description;
            }
        }
        public string DescriptionCategory { get { return cat5; } set { cat5 = value; } }
        public PropertyGridItemXMLTag Description
        {
            get { return new PropertyGridItemXMLTag(DN_description, D_description); }
            set
            {
                DN_description = value.DisplayName;
                D_description = value.Description;
            }
        }
        #endregion
    }
}
