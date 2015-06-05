using Microsoft.Office.Interop.Excel;
using PNSDraw.online;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNSDraw.Excel_export
{
    class ProblemToExcel
    {
        public static void PNSProblemToExcel(bool t_visible, string filename, PGraph graph)
        {
            #region Excel initialisation
            string t_path = "";
            if (!t_visible)
            {
                t_path = Path.GetDirectoryName(filename);
                if (t_path == "") t_path = Directory.GetCurrentDirectory();
                SaveFileDialog t_dialog;
                t_dialog = new SaveFileDialog();
                t_dialog.InitialDirectory = t_path;
                t_dialog.Filter = def_Solution_Excel.excel_extension;
                t_dialog.FileName = Converters.ToNameString(def_Problem_Excel.excel_filename_p1 +
                Path.GetFileName(filename) + def_Problem_Excel.excel_filename_p2);
                if (t_dialog.ShowDialog() != DialogResult.OK) return;
                t_path = t_dialog.FileName;
            }
            CultureInfo t_original_culture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            ExcelDoc t_xlsx = new ExcelDoc(false);
            t_xlsx.WSMaterials.Name = def_Solution_Excel.text_ws_materials;
            t_xlsx.WSOpUnits.Name = def_Solution_Excel.text_ws_opunits;
            t_xlsx.WSFlows.Name = def_Problem_Excel.text_ws_rates;

            Range t_cells;
            int t_mattop = 2, t_matleft = 2, t_outop = 2, t_ouleft = 2, t_flowtop = 2, t_flowleft = 2,
                t_v_flows_offset = 2, t_h_flows_offset = 2, t_v_ous_offset = 0, t_h_ous_offset = 0,
                t_v_offset, t_h_offset, t_top, t_left, i, j;
            #endregion

            #region Colors
            int color_mat = def_Solution_Excel.color_mat;
            int color_mat_dark = def_Solution_Excel.color_mat_dark;
            int color_rate = def_Solution_Excel.color_rate;
            int color_rate_dark = def_Solution_Excel.color_rate_dark;
            int color_ou = def_Solution_Excel.color_ou;
            int color_ou_dark = def_Solution_Excel.color_ou_dark;
            int color_size = def_Solution_Excel.color_size;
            int color_border = def_Solution_Excel.color_border;
            int color_solution = def_Solution_Excel.color_solution;
            #endregion

            #region Materials Sheet
            if (graph.Materials.Count > 0)
            {
                ((_Worksheet)t_xlsx.WSMaterials).Activate();

                #region Positions
                t_top = t_mattop;
                t_left = t_matleft;
                t_v_offset = 1;
                t_h_offset = 0;
                i = t_v_offset;
                j = t_h_offset;
                #endregion

                object[,] t_mats_data = new object[t_top + t_v_offset + graph.Materials.Count, t_left + t_h_offset + 8];
                #region Materials table header labels
                t_mats_data[t_top + i - 2, t_left - 1] = def_Solution_Excel.text_materials;
                t_cells = t_xlsx.Cells(t_top + i - 1, t_left, 1, 9);
                t_cells.Merge(false);

                t_cells = t_xlsx.Cells(t_top + i - 1, t_left, graph.Materials.Count + 2, 9);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.center);
                t_cells.Interior.Color = color_mat;

                t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_mat_name;
                t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_type;
                t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_quantity;
                t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_price;
                if (def_Solution_Excel.text_pricemu == "")
                {
                    t_cells = t_xlsx.Cells(t_top + i, t_left + j++ - 1, 1, 2);
                    t_cells.Merge(false);
                }
                else t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_pricemu;
                t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_minflow;
                if (def_Solution_Excel.text_minflowmu == "")
                {
                    t_cells = t_xlsx.Cells(t_top + i, t_left + j++ - 1, 1, 2);
                    t_cells.Merge(false);
                }
                else t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_minflowmu;
                t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_maxflow;
                if (def_Solution_Excel.text_maxflowmu == "")
                {
                    t_cells = t_xlsx.Cells(t_top + i++, t_left + j++ - 1, 1, 2);
                    t_cells.Merge(false);
                }
                else t_mats_data[t_top + i++ - 1, t_left + j++ - 1] = def_Solution_Excel.text_maxflowmu;
                #endregion

                #region Material table
                foreach (Material mat in graph.Materials)
                {
                    if (mat.Type == Globals.MaterialTypes.Raw)
                    {
                        j = t_h_offset;
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.Name;
                        t_cells = t_xlsx.Cells(t_top + i, t_left + j - 1, 1, 1);
                        t_cells.AddComment(mat.DisplayedText);
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = "Raw material";
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = MUs.GetQuantity(mat.ParameterList["reqflow"].MU);
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.PriceProp.Value == -1 ? Default.price.ToString() : mat.PriceProp.Value.ToString();
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ParameterList["price"].MU.Length == 0 ? Default.money_mu.ToString() : mat.ParameterList["price"].MU;
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ReqFlowProp.Value == -1 ? Default.flow_rate_lower_bound.ToString() : mat.ReqFlowProp.Value.ToString();
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ParameterList["reqflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["reqflow"].MU;
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.MaxFlowProp.Value == -1 ? Default.flow_rate_upper_bound.ToString() : mat.MaxFlowProp.Value.ToString();
                        t_mats_data[t_top + i++ - 1, t_left + j++ - 1] = mat.ParameterList["maxflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["maxflow"].MU;

                        if ((t_top + i - t_v_offset) % 2 == 0)
                        {
                            t_cells = t_xlsx.Cells(t_top + i - 1, t_left, 1, j);
                            t_cells.Interior.Color = color_mat_dark;
                        }
                    }

                    if (mat.Type == Globals.MaterialTypes.Intermediate)
                    {
                        j = t_h_offset;
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.Name;
                        t_cells = t_xlsx.Cells(t_top + i, t_left + j - 1, 1, 1);
                        t_cells.AddComment(mat.DisplayedText);
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = "Intermediate material";
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = MUs.GetQuantity(mat.ParameterList["reqflow"].MU);
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.PriceProp.Value == -1 ? Default.price.ToString() : mat.PriceProp.Value.ToString();
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ParameterList["price"].MU.Length == 0 ? Default.money_mu.ToString() : mat.ParameterList["price"].MU;
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ReqFlowProp.Value == -1 ? Default.flow_rate_lower_bound.ToString() : mat.ReqFlowProp.Value.ToString();
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ParameterList["reqflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["reqflow"].MU;
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.MaxFlowProp.Value == -1 ? Default.flow_rate_upper_bound.ToString() : mat.MaxFlowProp.Value.ToString();
                        t_mats_data[t_top + i++ - 1, t_left + j++ - 1] = mat.ParameterList["maxflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["maxflow"].MU;

                        if ((t_top + i - t_v_offset) % 2 == 0)
                        {
                            t_cells = t_xlsx.Cells(t_top + i - 1, t_left, 1, j);
                            t_cells.Interior.Color = color_mat_dark;
                        }
                    }

                    if (mat.Type == Globals.MaterialTypes.Product)
                    {
                        j = t_h_offset;
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.Name;
                        t_cells = t_xlsx.Cells(t_top + i, t_left + j - 1, 1, 1);
                        t_cells.AddComment(mat.DisplayedText);
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = "Product material";
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = MUs.GetQuantity(mat.ParameterList["reqflow"].MU);
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.PriceProp.Value == -1 ? Default.price.ToString() : mat.PriceProp.Value.ToString();
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ParameterList["price"].MU.Length == 0 ? Default.money_mu.ToString() : mat.ParameterList["price"].MU;
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ReqFlowProp.Value == -1 ? Default.flow_rate_lower_bound.ToString() : mat.ReqFlowProp.Value.ToString();
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ParameterList["reqflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["reqflow"].MU;
                        t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.MaxFlowProp.Value == -1 ? Default.flow_rate_upper_bound.ToString() : mat.MaxFlowProp.Value.ToString();
                        t_mats_data[t_top + i++ - 1, t_left + j++ - 1] = mat.ParameterList["maxflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["maxflow"].MU;

                        if ((t_top + i - t_v_offset) % 2 == 0)
                        {
                            t_cells = t_xlsx.Cells(t_top + i - 1, t_left, 1, j);
                            t_cells.Interior.Color = color_mat_dark;
                        }
                    }
                }
                #endregion

                #region Material table format
                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left, graph.Materials.Count + 1, 3);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);
                int t_h = 3;
                t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h, graph.Materials.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
                t_cells.NumberFormat = "#,##0.00";
                t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 1, graph.Materials.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);

                t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 2, graph.Materials.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
                t_cells.NumberFormat = "#,##0.00";
                t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 3, graph.Materials.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);

                t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 4, graph.Materials.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
                t_cells.NumberFormat = "#,##0.00";
                t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 5, graph.Materials.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);
                #endregion

                #region Materials table borders
                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left, graph.Materials.Count + 1, t_h + 1);
                t_cells.Borders[XlBordersIndex.xlInsideVertical].Color = color_border;
                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left + t_h + 2, graph.Materials.Count + 1, 2);
                t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = color_border;
                t_cells = t_xlsx.Cells(t_top + t_v_offset - 1, t_left, graph.Materials.Count + 2, j);
                t_cells.Borders[XlBordersIndex.xlEdgeTop].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = color_border;
                t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left, 1, j);
                t_cells.Borders[XlBordersIndex.xlEdgeTop].Color = color_border;
                #endregion

                t_xlsx.WSMaterials.get_Range("A1", t_xlsx.CellName(t_mattop + graph.Materials.Count + 1,
                    t_matleft + 8)).Value2 = t_mats_data;
            }
            #endregion

            #region Operating Units Sheet
            if (graph.OperatingUnits.Count > 0)
            {
                ((_Worksheet)t_xlsx.WSOpUnits).Activate();
                object[,] t_ous_data = new object[t_outop + t_v_ous_offset + graph.OperatingUnits.Count + 2, t_ouleft + t_h_ous_offset + 10];
                t_top = t_outop;
                t_left = t_ouleft;
                i = t_v_ous_offset;
                j = t_h_ous_offset;
                int c_payout = 1, c_ofix = 1, c_oprop = 1, c_ifix = 1, c_iprop = 1;

                t_ous_data[t_top + i - 1, t_left - 1] = def_Solution_Excel.text_opunits;
                t_cells = t_xlsx.Cells(t_top + i++, t_left, 1, 11);
                t_cells.Merge(false);

                t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_ou_name;
                t_cells = t_xlsx.Cells(t_top + i, t_left + j++, 2, 1);
                t_cells.Merge(false);

                t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_working;
                t_cells = t_xlsx.Cells(t_top + i, t_left + j++, 2, 1);
                t_cells.Merge(false);

                c_payout = t_left + j;
                t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_payout;
                t_cells = t_xlsx.Cells(t_top + i, t_left + j++, 2, 1);
                t_cells.Merge(false);

                t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_capacity;
                t_cells = t_xlsx.Cells(t_top + i, t_left + j, 1, 2);
                t_cells.Merge(false);

                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_lb;
                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_ub;

                t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_opcost;
                t_cells = t_xlsx.Cells(t_top + i, t_left + j, 1, 2);
                t_cells.Merge(false);

                c_ofix = t_left + j;
                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_ofix + "\n[" + Default.money_mu.ToString() + "]";
                c_oprop = t_left + j;
                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_oprop + "\n[" + Default.money_mu.ToString() + "]";

                t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_invcost;
                t_cells = t_xlsx.Cells(t_top + i, t_left + j, 1, 2);
                t_cells.Merge(false);

                c_ifix = t_left + j;
                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_ifix + "\n[" + Default.money_mu.ToString() + "]";
                c_iprop = t_left + j;
                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_iprop + "\n[" + Default.money_mu.ToString() + "]";
                t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_overallcost;
                t_cells = t_xlsx.Cells(t_top + i, t_left + j, 1, 2);
                t_cells.Merge(false);

                int c_fix = t_left + j;
                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_fix + "\n[" + Default.money_mu.ToString() + "]";
                int c_prop = t_left + j;
                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_prop + "\n[" + Default.money_mu.ToString() + "]";

                t_cells = t_xlsx.Cells(t_top, t_left, 3, 11);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.center);
                t_cells.Interior.Color = color_ou;

                t_cells = t_xlsx.Cells(t_top + 3, t_left + 1, graph.OperatingUnits.Count, 10);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
                t_cells.NumberFormat = "#,##0.00";
                t_cells.Interior.Color = color_ou;

                t_cells = t_xlsx.Cells(t_top + 3, t_left, graph.OperatingUnits.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);
                t_cells.Interior.Color = color_ou;

                t_cells = t_xlsx.Cells(t_top + 3, t_left + 1, graph.OperatingUnits.Count, 1);
                t_cells.NumberFormat = "#,##0";

                t_cells = t_xlsx.Cells(t_top + 1, t_left, graph.OperatingUnits.Count + 2, 11);
                t_cells.Borders[XlBordersIndex.xlInsideVertical].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;

                t_cells = t_xlsx.Cells(t_top, t_left, graph.OperatingUnits.Count + 3, 11);
                t_cells.Borders[XlBordersIndex.xlEdgeTop].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = color_border;

                t_cells = t_xlsx.Cells(t_top + 2, t_left, 1, 11);
                t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;
                i += 2;
                foreach (OperatingUnit ou in graph.OperatingUnits)
                {
                    if ((t_top + i) % 2 == 1)
                    {
                        t_cells = t_xlsx.Cells(t_top + i, t_left, 1, 11);
                        t_cells.Interior.Color = color_ou_dark;
                    }
                    j = 0;
                    t_ous_data[t_top + i - 1, t_left + j - 1] = ou.Name;
                    t_cells = t_xlsx.Cells(t_top + i, t_left + j++, 1, 1);
                    t_cells.AddComment(ou.DisplayedText);
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.WorkingHourProp.Value == -1 ? Default.worging_hours_per_year.ToString() : ou.WorkingHourProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.PayoutPeriodProp.Value == -1 ? Default.payout_period.ToString() : ou.PayoutPeriodProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.CapacityLowerProp.Value == -1 ? Default.capacity_lower_bound.ToString() : ou.CapacityLowerProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.CapacityUpperProp.Value == -1 ? Default.capacity_upper_bound.ToString() : ou.CapacityUpperProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.OperatingCostFixProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.OperatingCostPropProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.InvestmentCostFixProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.InvestmentCostPropProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = "=" + t_xlsx.CellName(t_top + i, c_ofix) +
                        "+" + t_xlsx.CellName(t_top + i, c_ifix) + "/" + t_xlsx.CellName(t_top + i, c_payout);
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = "=" + t_xlsx.CellName(t_top + i, c_oprop) +
                        "+" + t_xlsx.CellName(t_top + i, c_iprop) + "/" + t_xlsx.CellName(t_top + i, c_payout);
                    i++;
                }

                t_xlsx.WSOpUnits.get_Range("A1", t_xlsx.CellName(t_outop + t_v_ous_offset + graph.OperatingUnits.Count + 2, t_ouleft +
                    t_h_ous_offset + 10)).Value2 = t_ous_data;
            }
            #endregion

            #region Flows Sheet
            if (graph.Materials.Count > 0 && graph.OperatingUnits.Count > 0)
            {
                ((_Worksheet)t_xlsx.WSFlows).Activate();
                object[,] t_flows_data = new object[t_flowtop + t_v_flows_offset + graph.Materials.Count - 1, t_flowleft + t_h_flows_offset + graph.OperatingUnits.Count - 1];

                #region Positions
                t_top = t_flowtop;
                t_left = t_flowleft;
                t_v_offset = t_v_flows_offset;
                t_h_offset = t_h_flows_offset;
                i = t_v_offset;
                j = t_h_offset;
                #endregion

                #region Rates and Operating Units label
                t_cells = t_xlsx.Cells(t_top, t_left, graph.Materials.Count + t_v_offset, t_h_offset);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.center);
                t_cells.Interior.Color = color_mat;

                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left + t_h_offset, graph.Materials.Count, graph.OperatingUnits.Count);
                t_cells.Interior.Color = color_rate;

                t_flows_data[t_top - 1, t_left - 1] = def_Problem_Excel.text_rates_label;
                t_cells = t_xlsx.Cells(t_top, t_left, t_v_offset, t_h_offset);
                t_cells.Merge(false);

                t_flows_data[t_top - 1, t_left + t_h_offset - 1] = def_Solution_Excel.text_opunits;
                t_cells = t_xlsx.Cells(t_top, t_left + t_h_offset, 1, graph.OperatingUnits.Count);
                t_cells.Merge(false);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.center);
                t_cells.Interior.Color = color_ou;
                #endregion

                #region Material flows, FlowMUs
                foreach (Material mat in graph.Materials)
                {
                    if (mat.Type == Globals.MaterialTypes.Raw)
                    {
                        t_flows_data[t_top + i - 1, t_left - 1] = mat.Name;
                        t_flows_data[t_top + i - 1, t_left] = "[" + (mat.ParameterList["reqflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["reqflow"].MU) + "]";
                        if ((i - t_v_offset) % 2 == 0)
                        {
                            t_cells = t_xlsx.Cells(t_top + i, t_left, 1, t_h_offset);
                            t_cells.Interior.Color = color_mat_dark;

                            t_cells = t_xlsx.Cells(t_top + i, t_left + t_h_offset, 1, graph.OperatingUnits.Count);
                            t_cells.Interior.Color = color_rate_dark;
                        }
                        i++;
                    }

                    if (mat.Type == Globals.MaterialTypes.Intermediate)
                    {
                        t_flows_data[t_top + i - 1, t_left - 1] = mat.Name;
                        t_flows_data[t_top + i - 1, t_left] = "[" + (mat.ParameterList["reqflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["reqflow"].MU) + "]";
                        if ((i - t_v_offset) % 2 == 0)
                        {
                            t_cells = t_xlsx.Cells(t_top + i, t_left, 1, t_h_offset);
                            t_cells.Interior.Color = color_mat_dark;

                            t_cells = t_xlsx.Cells(t_top + i, t_left + t_h_offset, 1, graph.OperatingUnits.Count);
                            t_cells.Interior.Color = color_rate_dark;
                        }
                        i++;
                    }

                    if (mat.Type == Globals.MaterialTypes.Product)
                    {
                        t_flows_data[t_top + i - 1, t_left - 1] = mat.Name;
                        t_flows_data[t_top + i - 1, t_left] = "[" + (mat.ParameterList["reqflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["reqflow"].MU) + "]";
                        if ((i - t_v_offset) % 2 == 0)
                        {
                            t_cells = t_xlsx.Cells(t_top + i, t_left, 1, t_h_offset);
                            t_cells.Interior.Color = color_mat_dark;

                            t_cells = t_xlsx.Cells(t_top + i, t_left + t_h_offset, 1, graph.OperatingUnits.Count);
                            t_cells.Interior.Color = color_rate_dark;
                        }
                        i++;
                    }
                }

                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left, graph.Materials.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);

                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left + 1, graph.Materials.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
                #endregion

                #region Opunit name, rates matrix
                foreach (OperatingUnit ou in graph.OperatingUnits)
                {
                    t_flows_data[t_top, t_left + j - 1] = ou.Name;
                    foreach (KeyValuePair<Material, double> t_mat in FileConnector.GetOpUnitBeginEnd(ou, graph)["input"])
                    {
                        i = t_v_offset;
                        bool t_found = false;
                        foreach (Material mat in graph.Materials)
                        {
                            if (t_found = mat.Name == t_mat.Key.Name) break;
                            i++;
                        }
                        t_flows_data[t_top + i - 1, t_left + j - 1] = (-t_mat.Value).ToString();
                    }
                    foreach (KeyValuePair<Material, double> t_mat in FileConnector.GetOpUnitBeginEnd(ou, graph)["output"])
                    {
                        i = t_v_offset;
                        bool t_found = false;
                        foreach (Material mat in graph.Materials)
                        {
                            if (t_found = mat.Name == t_mat.Key.Name) break;
                            i++;
                        }
                        t_flows_data[t_top + i - 1, t_left + j - 1] = (t_mat.Value).ToString();
                    }
                    j++;
                }

                t_cells = t_xlsx.Cells(t_top + 1, t_left + t_h_offset, 1, graph.OperatingUnits.Count);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.bottom);
                t_cells.Interior.Color = color_ou;
                t_cells.Orientation = 90;

                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left + t_h_offset, graph.Materials.Count, graph.OperatingUnits.Count);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
                t_cells.NumberFormat = "#,##0.00";
                #endregion

                #region Flow matrix borders
                t_cells = t_xlsx.Cells(t_top, t_left + t_h_offset - 1, t_v_offset + graph.Materials.Count, graph.OperatingUnits.Count + 1);
                t_cells.Borders[XlBordersIndex.xlInsideVertical].Color = color_border;

                t_cells = t_xlsx.Cells(t_top, t_left, t_v_offset + graph.Materials.Count, t_h_offset + graph.OperatingUnits.Count);
                t_cells.Borders[XlBordersIndex.xlEdgeTop].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = color_border;

                t_cells = t_xlsx.Cells(t_top + 1, t_left + t_h_offset, 1, graph.OperatingUnits.Count);
                t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;
                #endregion

                t_xlsx.WSFlows.get_Range("A1", t_xlsx.CellName(t_flowtop + t_v_flows_offset + graph.Materials.Count - 1,
                    t_flowleft + t_h_flows_offset + graph.OperatingUnits.Count - 1)).Value2 = t_flows_data;
            }
            #endregion

            #region Excel finalisation
            t_xlsx.WSMaterials.Cells.Rows.AutoFit();
            t_xlsx.WSMaterials.Cells.Columns.AutoFit();
            t_xlsx.WSOpUnits.Cells.Rows.AutoFit();
            t_xlsx.WSOpUnits.Cells.Columns.AutoFit();
            t_xlsx.WSFlows.Cells.Rows.AutoFit();
            t_xlsx.WSFlows.Cells.Columns.AutoFit();
            t_xlsx.App.Visible = t_visible;
            if (!t_visible) t_xlsx.Save(t_path, true);
            Thread.CurrentThread.CurrentCulture = t_original_culture;
            #endregion
        }
    }
}
