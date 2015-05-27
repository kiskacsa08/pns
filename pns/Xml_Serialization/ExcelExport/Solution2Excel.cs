using System;
using System.Collections.Generic;
using System.Text;
using Pns.Xml_Serialization.PnsGUI;

namespace Pns.Xml_Serialization.ExcelExport
{
    public class def_Solution_Excel
    {
        #region Members
        public enum HAlign { left, center, right }
        public enum VAlign { top, center, bottom }

        #region SaveDialogSettings
        static public string excel_extension = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
        static public string excel_filename_p1 = "Brief";
        static public string excel_filename_p2 = "Detailed";
        static public string excel_filename_p3 = " result of solution";
        static public string excel_filename_p4 = " of ";
        static public string excel_filename_p5 = "Summary of results of ";
        #endregion

        #region Colors
        static public int color_mat = System.Drawing.Color.FromArgb(250, 225, 200).ToArgb();
        static public int color_mat_dark = System.Drawing.Color.FromArgb(225, 202, 180).ToArgb();
        static public int color_rate = System.Drawing.Color.FromArgb(240, 240, 240).ToArgb();
        static public int color_rate_dark = System.Drawing.Color.FromArgb(220, 220, 220).ToArgb();
        static public int color_ou = System.Drawing.Color.FromArgb(210, 250, 210).ToArgb();
        static public int color_ou_dark = System.Drawing.Color.FromArgb(189, 225, 189).ToArgb();
        static public int color_size = System.Drawing.Color.FromArgb(200, 250, 250).ToArgb();
        static public int color_border = System.Drawing.Color.FromArgb(0, 0, 0).ToArgb();
        static public int color_solution = System.Drawing.Color.FromArgb(200, 230, 250).ToArgb();
        static public int color_solution_dark = System.Drawing.Color.FromArgb(180, 207, 225).ToArgb();
        #endregion

        #region Materials
        static public string text_ws_materials = "Materials";
        static public string text_materials = "Materials";
        static public string text_mat_name = "Name";
        static public string text_type = "Type";
        static public string text_quantity = "Quantity type";
        static public string text_minflow = "Min. flow";
        static public string text_minflowmu = "";
        static public string text_maxflow = "Max. flow";
        static public string text_maxflowmu = "";
        static public string text_amount = "Flow";
        static public string text_flowmu = "";
        static public string text_price = "Price";
        static public string text_pricemu = "";
        static public string text_mat_cost = "Cost";
        static public string text_mats_cost = "Total cost of materials";
        #endregion

        #region OpUnits
        static public string text_ws_opunits = "Operating units";
        static public string text_opunits = "Operating units";
        static public string text_ou_name = "Name";
        static public string text_size = "Size\r\nfactor";
        static public string text_ou_cost = "Cost";
        static public string text_ous_cost = "Total cost of operating units";
        static public string text_working = "Working\r\nhours\r\n[h/yr]";
        static public string text_payout = "Payout\r\nperiod\r\n[yr]";
        static public string text_capacity = "Capacity";
        static public string text_lb = "Lower\r\nbound";
        static public string text_ub = "Upper\r\nbound";
        static public string text_opcost = "Operating cost";
        static public string text_ofix = "Fixed\r\ncharge";
        static public string text_ofixmu = "";
        static public string text_oprop = "Prop.\r\nconstant";
        static public string text_opropmu = "";
        static public string text_invcost = "Investment cost";
        static public string text_ifix = "Fixed\r\ncharge";
        static public string text_ifixmu = "";
        static public string text_iprop = "Prop.\r\nconstant";
        static public string text_ipropmu = "";
        static public string text_overallcost = "Overall cost";
        static public string text_fix = "Fixed\r\ncharge";
        static public string text_fixmu = "";
        static public string text_prop = "Prop.\r\nconstant";
        static public string text_propmu = "";
        #endregion

        #region Flows
        static public string text_ws_flows = "Flows";
        static public string text_rates_and_flows_label = "Material rates and flows\r\nof operating units";
        static public string text_opunit_size_label = "Operating unit size factors";
        static public string text_overall_flows_label = "Overall material flows";
        static public string text_rate = "rate";
        static public string text_flow = "flow";
        #endregion

        #region Summary
        static public string text_solution_summary_label = "Solution summary information";
        static public string text_solution_cost = "Overall cost of solution";
        #endregion

        #region Summary of results
        static public string text_ws_summary_of_results = "Summary";
        static public string text_raw_materials = "Raw materials";
        static public string text_product_materials = "Products";
        static public string text_solution_structures = "Solution\nstructures";
        static public string text_summary_cost = "Cost";
        static public string text_structure = "Structure";
        #endregion

        #endregion

        #region Properties

        #region SaveDialogSettings
        public string ExtensionInfo { get { return excel_extension; } set { excel_extension = value; } }
        public TextXMLTag DefaultXLSFileName
        {
            get { return new TextXMLTag(excel_filename_p1, excel_filename_p2, excel_filename_p3, excel_filename_p4, excel_filename_p5); }
            set
            {
                excel_filename_p1 = value.TextPart1;
                excel_filename_p2 = value.TextPart2;
                excel_filename_p3 = value.TextPart3;
                excel_filename_p4 = value.TextPart4;
                excel_filename_p5 = value.TextPart5;
            }
        }
        #endregion

        #region Colors
        public ColorXMLTag MaterialBkColor
        {
            get { return new ColorXMLTag(System.Drawing.Color.FromArgb(color_mat).B, System.Drawing.Color.FromArgb(color_mat).G, System.Drawing.Color.FromArgb(color_mat).R); }
            set { color_mat = System.Drawing.Color.FromArgb(value.Blue, value.Green, value.Red).ToArgb(); }
        }
        public ColorXMLTag MaterialDarkBkColor
        {
            get { return new ColorXMLTag(System.Drawing.Color.FromArgb(color_mat_dark).B, System.Drawing.Color.FromArgb(color_mat_dark).G, System.Drawing.Color.FromArgb(color_mat_dark).R); }
            set { color_mat_dark = System.Drawing.Color.FromArgb(value.Blue, value.Green, value.Red).ToArgb(); }
        }
        public ColorXMLTag RateBkColor
        {
            get { return new ColorXMLTag(System.Drawing.Color.FromArgb(color_rate).B, System.Drawing.Color.FromArgb(color_rate).G, System.Drawing.Color.FromArgb(color_rate).R); }
            set { color_rate = System.Drawing.Color.FromArgb(value.Blue, value.Green, value.Red).ToArgb(); }
        }
        public ColorXMLTag RateDarkBkColor
        {
            get { return new ColorXMLTag(System.Drawing.Color.FromArgb(color_rate_dark).B, System.Drawing.Color.FromArgb(color_rate_dark).G, System.Drawing.Color.FromArgb(color_rate_dark).R); }
            set { color_rate_dark = System.Drawing.Color.FromArgb(value.Blue, value.Green, value.Red).ToArgb(); }
        }
        public ColorXMLTag OperatingUnitBkColor
        {
            get { return new ColorXMLTag(System.Drawing.Color.FromArgb(color_ou).B, System.Drawing.Color.FromArgb(color_ou).G, System.Drawing.Color.FromArgb(color_ou).R); }
            set { color_ou = System.Drawing.Color.FromArgb(value.Blue, value.Green, value.Red).ToArgb(); }
        }
        public ColorXMLTag OperatingUnitDarkBkColor
        {
            get { return new ColorXMLTag(System.Drawing.Color.FromArgb(color_ou_dark).B, System.Drawing.Color.FromArgb(color_ou_dark).G, System.Drawing.Color.FromArgb(color_ou_dark).R); }
            set { color_ou_dark = System.Drawing.Color.FromArgb(value.Blue, value.Green, value.Red).ToArgb(); }
        }
        public ColorXMLTag SizeBkColor
        {
            get { return new ColorXMLTag(System.Drawing.Color.FromArgb(color_size).B, System.Drawing.Color.FromArgb(color_size).G, System.Drawing.Color.FromArgb(color_size).R); }
            set { color_size = System.Drawing.Color.FromArgb(value.Blue, value.Green, value.Red).ToArgb(); }
        }
        public ColorXMLTag BorderColor
        {
            get { return new ColorXMLTag(System.Drawing.Color.FromArgb(color_border).B, System.Drawing.Color.FromArgb(color_border).G, System.Drawing.Color.FromArgb(color_border).R); }
            set { color_border = System.Drawing.Color.FromArgb(value.Blue, value.Green, value.Red).ToArgb(); }
        }
        public ColorXMLTag SolutionBkColor
        {
            get { return new ColorXMLTag(System.Drawing.Color.FromArgb(color_solution).B, System.Drawing.Color.FromArgb(color_solution).G, System.Drawing.Color.FromArgb(color_solution).R); }
            set { color_solution = System.Drawing.Color.FromArgb(value.Blue, value.Green, value.Red).ToArgb(); }
        }
        public ColorXMLTag SolutionDarkBkColor
        {
            get { return new ColorXMLTag(System.Drawing.Color.FromArgb(color_solution_dark).B, System.Drawing.Color.FromArgb(color_solution_dark).G, System.Drawing.Color.FromArgb(color_solution_dark).R); }
            set { color_solution_dark = System.Drawing.Color.FromArgb(value.Blue, value.Green, value.Red).ToArgb(); }
        }
        #endregion

        #region Materials
        public string MaterialsWorkSheetLabel { get { return text_ws_materials; } set { text_ws_materials = value; } }
        public string MaterialsLabel { get { return text_materials; } set { text_materials = value; } }
        public string MaterialNameLabel { get { return text_mat_name; } set { text_mat_name = value; } }
        public string MaterialTypeLabel { get { return text_type; } set { text_type = value; } }
        public string MaterialQuantityTypeLabel { get { return text_quantity; } set { text_quantity = value; } }
        public string MaterialMinFlowLabel { get { return text_minflow; } set { text_minflow = value; } }
        public string MaterialMinFlowMULabel { get { return text_minflowmu; } set { text_minflowmu = value; } }
        public string MaterialMaxFlowLabel { get { return text_maxflow; } set { text_maxflow = value; } }
        public string MaterialMaxFlowMULabel { get { return text_maxflowmu; } set { text_maxflowmu = value; } }
        public string MaterialFlowLabel { get { return text_amount; } set { text_amount = value; } }
        public string MaterialFlowMULabel { get { return text_flowmu; } set { text_flowmu = value; } }
        public string MaterialPriceLabel { get { return text_price; } set { text_price = value; } }
        public string MaterialPriceMULabel { get { return text_pricemu; } set { text_pricemu = value; } }
        public string MaterialCostLabel { get { return text_mat_cost; } set { text_mat_cost = value; } }
        public string MaterialsTotalCostLabel { get { return text_mats_cost; } set { text_mats_cost = value; } }
        #endregion

        #region OpUnits
        public string OperatingUnitsWorkSheetLabel { get { return text_ws_opunits; } set { text_ws_opunits = value; } }
        public string OperatingUnitsLabel { get { return text_opunits; } set { text_opunits = value; } }
        public string OperatingUnitNameLabel { get { return text_ou_name; } set { text_ou_name = value; } }
        public string OperatingUnitSizeLabel { get { return text_size; } set { text_size = value; } }
        public string OperatingUnitCostLabel { get { return text_ou_cost; } set { text_ou_cost = value; } }
        public string OperatingUnitsTotalCostLabel { get { return text_ous_cost; } set { text_ous_cost = value; } }
        public string OperatingUnitWorkingHoursLabel { get { return text_working; } set { text_working = value; } }
        public string OperatingUnitPayoutLabel { get { return text_payout; } set { text_payout = value; } }
        public string OperatingUnitCapacityLabel { get { return text_capacity; } set { text_capacity = value; } }
        public string OperatingUnitLowerBoundLabel { get { return text_lb; } set { text_lb = value; } }
        public string OperatingUnitUpperBoundLabel { get { return text_ub; } set { text_ub = value; } }
        public string OperatingUnitOperatingCostLabel { get { return text_opcost; } set { text_opcost = value; } }
        public string OperatingUnitOperatingCostFixLabel { get { return text_ofix; } set { text_ofix = value; } }
        public string OperatingUnitOperatingCostFixMULabel { get { return text_ofixmu; } set { text_ofixmu = value; } }
        public string OperatingUnitOperatingCostPropLabel { get { return text_oprop; } set { text_oprop = value; } }
        public string OperatingUnitOperatingCostPropMULabel { get { return text_opropmu; } set { text_opropmu = value; } }
        public string OperatingUnitInvestmentCostLabel { get { return text_invcost; } set { text_invcost = value; } }
        public string OperatingUnitInvestmentCostFixLabel { get { return text_ifix; } set { text_ifix = value; } }
        public string OperatingUnitInvestmentCostFixMULabel { get { return text_ifixmu; } set { text_ifixmu = value; } }
        public string OperatingUnitInvestmentCostPropLabel { get { return text_iprop; } set { text_iprop = value; } }
        public string OperatingUnitInvestmentCostPropMULabel { get { return text_ipropmu; } set { text_ipropmu = value; } }
        public string OperatingUnitOverallCostLabel { get { return text_overallcost; } set { text_overallcost = value; } }
        public string OperatingUnitOverallCostFixLabel { get { return text_fix; } set { text_fix = value; } }
        public string OperatingUnitOverallCostFixMULabel { get { return text_fixmu; } set { text_fixmu = value; } }
        public string OperatingUnitOverallCostPropLabel { get { return text_prop; } set { text_prop = value; } }
        public string OperatingUnitOverallCostPropMULabel { get { return text_propmu; } set { text_propmu = value; } }
        #endregion

        #region Flows
        public string FlowsWorkSheetLabel { get { return text_ws_flows; } set { text_ws_flows = value; } }
        public string FlowsRateAndFlowsLabel { get { return text_rates_and_flows_label; } set { text_rates_and_flows_label = value; } }
        public string FlowsOperatingUnitSizeLabel { get { return text_opunit_size_label; } set { text_opunit_size_label = value; } }
        public string FlowsOverallFlowsLabel { get { return text_overall_flows_label; } set { text_overall_flows_label = value; } }
        public string FlowsRateLabel { get { return text_rate; } set { text_rate = value; } }
        public string FlowsFlowLabel { get { return text_flow; } set { text_flow = value; } }
        #endregion

        #region Summary
        public string SolutionSummaryLabel { get { return text_solution_summary_label; } set { text_solution_summary_label = value; } }
        public string SolutionOverallCostLabel { get { return text_solution_cost; } set { text_solution_cost = value; } }
        #endregion

        #region Summary of results
        public string SummaryWorkSheetLabel { get { return text_ws_summary_of_results; } set { text_ws_summary_of_results = value; } }
        public string RawMaterialsLabel { get { return text_raw_materials; } set { text_raw_materials = value; } }
        public string ProductMaterialsLabel { get { return text_product_materials; } set { text_product_materials = value; } }
        public string SolutionStructuresLabel { get { return text_solution_structures; } set { text_solution_structures = value; } }
        public string SummaryCostLabel { get { return text_summary_cost; } set { text_summary_cost = value; } }
        public string SummaryStructureLabel { get { return text_structure; } set { text_structure = value; } }
        #endregion

        #endregion
    }
}
