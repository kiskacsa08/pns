using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.Dialogs.DefValues
{
    public class def_DefaultValuesPropGrid
    {
        #region Members
        static public string Cat_material = "\t\tMaterial";
        static public string DN_minimum_flow = "Required flow";
        static public string D_minimum_flow = "";
        static public string DN_maximum_flow = "Maximum flow";
        static public string D_maximum_flow = "";
        static public string DN_price = "Price";
        static public string D_price = "";

        static public string Cat_op = "\tOperating unit";
        static public string DN_flow_rate = "Flow rate";
        static public string D_flow_rate = "";
        static public string DN_ocost = "Operating cost";
        static public string D_ocost = "";
        static public string DN_ofix = "Fixed charge";
        static public string D_ofix = "";
        static public string DN_oprop = "Proportionality constant";
        static public string D_oprop = "";
        static public string DN_icost = "Investment cost";
        static public string D_icost = "";
        static public string DN_ifix = "Fixed charge";
        static public string D_ifix = "";
        static public string DN_iprop = "Proportionality constant";
        static public string D_iprop = "";
        static public string DN_cap_constr = "Capacity constraints";
        static public string D_cap_constr = "";
        static public string DN_lb = "Lower bound";
        static public string D_lb = "";
        static public string DN_ub = "Upper bound";
        static public string D_ub = "";

        static public string Cat_solver = "Solver numerical limits";
        static public string DN_solver_ub = "Upper limit";
        static public string D_solver_ub = "";
        static public string DN_max_solutions = "Maximum number of solutions";
        static public string D_max_solutions = "";
        #endregion

        #region Properties
        public string MaterialCategory { get { return Cat_material; } set { Cat_material = value; } }
        public PropertyGridItemXMLTag RequiredFlow
        {
            get { return new PropertyGridItemXMLTag(DN_minimum_flow, D_minimum_flow); }
            set
            {
                DN_minimum_flow = value.DisplayName;
                D_minimum_flow = value.Description;
            }
        }
        public PropertyGridItemXMLTag MaximumFlow
        {
            get { return new PropertyGridItemXMLTag(DN_maximum_flow, D_maximum_flow); }
            set
            {
                DN_maximum_flow = value.DisplayName;
                D_maximum_flow = value.Description;
            }
        }
        public PropertyGridItemXMLTag Price
        {
            get { return new PropertyGridItemXMLTag(DN_price, D_price); }
            set
            {
                DN_price = value.DisplayName;
                D_price = value.Description;
            }
        }
        public string OperatingUnitCategory { get { return Cat_op; } set { Cat_op = value; } }
        public PropertyGridItemXMLTag FlowRate
        {
            get { return new PropertyGridItemXMLTag(DN_flow_rate, D_flow_rate); }
            set
            {
                DN_flow_rate = value.DisplayName;
                D_flow_rate = value.Description;
            }
        }
        public PropertyGridItemXMLTag OperatingCost
        {
            get { return new PropertyGridItemXMLTag(DN_ocost, D_ocost); }
            set
            {
                DN_ocost = value.DisplayName;
                D_ocost = value.Description;
            }
        }
        public PropertyGridItemXMLTag OperatingCostFix
        {
            get { return new PropertyGridItemXMLTag(DN_ofix, D_ofix); }
            set
            {
                DN_ofix = value.DisplayName;
                D_ofix = value.Description;
            }
        }
        public PropertyGridItemXMLTag OperatingCostProp
        {
            get { return new PropertyGridItemXMLTag(DN_oprop, D_oprop); }
            set
            {
                DN_oprop = value.DisplayName;
                D_oprop = value.Description;
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
        public PropertyGridItemXMLTag InvestmentCostFix
        {
            get { return new PropertyGridItemXMLTag(DN_ifix, D_ifix); }
            set
            {
                DN_ifix = value.DisplayName;
                D_ifix = value.Description;
            }
        }
        public PropertyGridItemXMLTag InvestmentCostProp
        {
            get { return new PropertyGridItemXMLTag(DN_iprop, D_iprop); }
            set
            {
                DN_iprop = value.DisplayName;
                D_iprop = value.Description;
            }
        }
        public PropertyGridItemXMLTag CapacityConstraints
        {
            get { return new PropertyGridItemXMLTag(DN_cap_constr, D_cap_constr); }
            set
            {
                DN_cap_constr = value.DisplayName;
                D_cap_constr = value.Description;
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
        public string SolverCategory { get { return Cat_solver; } set { Cat_solver = value; } }
        public PropertyGridItemXMLTag SolverUpperBound
        {
            get { return new PropertyGridItemXMLTag(DN_solver_ub, D_solver_ub); }
            set
            {
                DN_solver_ub = value.DisplayName;
                D_solver_ub = value.Description;
            }
        }
        public PropertyGridItemXMLTag MaximumNumberOfSolutions
        {
            get { return new PropertyGridItemXMLTag(DN_max_solutions, D_max_solutions); }
            set
            {
                DN_max_solutions = value.DisplayName;
                D_max_solutions = value.Description;
            }
        }
        #endregion
    }
}
