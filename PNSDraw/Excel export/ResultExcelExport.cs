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
    class ResultExcelExport
    {
        private Solution f_solution;

        public ResultExcelExport(Solution solution) { f_solution = solution; }

        public void ResultToExcel(bool t_brief, bool t_visible, string filename, PGraph graph)
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
                t_dialog.FileName = Converters.ToNameString((t_brief ? def_Solution_Excel.excel_filename_p1 : def_Solution_Excel.excel_filename_p2) +
                    def_Solution_Excel.excel_filename_p3 + (f_solution.Index) + def_Solution_Excel.excel_filename_p4 +
                    Path.GetFileName(filename));
                if (t_dialog.ShowDialog() != DialogResult.OK) return;
                t_path = t_dialog.FileName;
            }
            CultureInfo t_original_culture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            ExcelDoc t_xlsx = new ExcelDoc(t_brief);
            t_xlsx.WSMaterials.Name = def_Solution_Excel.text_ws_materials;
            t_xlsx.WSOpUnits.Name = def_Solution_Excel.text_ws_opunits;
            if (!t_brief) t_xlsx.WSFlows.Name = def_Solution_Excel.text_ws_flows;

            Range t_cells;
            object[,] solution_data;
            int t_mattop = 2, t_outop = 2, t_matleft = 2, t_ouleft = 2, t_flowtop = 7, t_flowleft = 2;
            int t_v_flows_offset = 3, t_h_flows_offset = 3, t_v_mat_brief_offset = 6, t_v_mat_detailed_offset = 1;
            int t_v_ous_offset = 0, t_h_ous_offset = 0;
            int t_v_offset;
            int t_h_offset;
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
            ((_Worksheet)t_xlsx.WSMaterials).Activate();

            #region Positions
            int t_top = t_mattop;
            int t_left = t_matleft;
            t_v_offset = t_brief ? t_v_mat_brief_offset : t_v_mat_detailed_offset;
            t_h_offset = 0;
            int i = t_v_offset;
            int j = t_h_offset;
            #endregion

            object[,] t_mats_data = new object[t_top + t_v_offset + f_solution.Materials.Count + 1, t_left + t_h_offset + (t_brief ? 5 : 11)];
            #region Materials table header labels
            t_mats_data[t_top + i - 2, t_left - 1] = def_Solution_Excel.text_materials;
            t_cells = t_xlsx.Cells(t_top + i - 1, t_left, 1, t_brief ? 6 : 12);
            t_cells.Merge(false);

            t_cells = t_xlsx.Cells(t_top + i - 1, t_left, f_solution.Materials.Count + 3, t_brief ? 6 : 12);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.center);
            t_cells.Interior.Color = color_mat;

            t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_mat_name;
            if (!t_brief)
            {
                t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_type;
                t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_quantity;
            }
            int c_matprice = t_left + j;
            t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_price;
            if (def_Solution_Excel.text_pricemu == "")
            {
                t_cells = t_xlsx.Cells(t_top + i, t_left + j++ - 1, 1, 2);
                t_cells.Merge(false);
            }
            else t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_pricemu;
            if (!t_brief)
            {
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
                    t_cells = t_xlsx.Cells(t_top + i, t_left + j++ - 1, 1, 2);
                    t_cells.Merge(false);
                }
                else t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_maxflowmu;
            }
            int c_matflow = t_left + j;
            t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_amount;
            if (def_Solution_Excel.text_flowmu == "")
            {
                t_cells = t_xlsx.Cells(t_top + i, t_left + j++ - 1, 1, 2);
                t_cells.Merge(false);
            }
            else t_mats_data[t_top + i - 1, t_left + j++ - 1] = def_Solution_Excel.text_flowmu;
            int c_matcost = t_left + j;
            t_mats_data[t_top + i++ - 1, t_left + j++ - 1] = def_Solution_Excel.text_mat_cost + " [" + Default.money_mu.ToString() + "/" + Default.time_mu.ToString() + "]";
            #endregion

            #region Material table
            string t_firstcostcell = t_xlsx.CellName(t_top + i, c_matcost);
            foreach (KeyValuePair<string, MaterialProperty> matProp in f_solution.Materials)
            {
                Material mat = graph.GetMaterialByName(matProp.Value.Name);
                if (mat == null)
                {
                    MessageBox.Show("Cannot export solution to Excel! Materials or Operating units are not the same in the problem and in the solution.");
                    return;
                }
                j = t_h_offset;
                t_mats_data[t_top + i - 1, t_left + j++ - 1] = matProp.Value.Name;
                //Console.WriteLine("Ide írtam: " + matProp.Value.Name);
                t_cells = t_xlsx.Cells(t_top + i, t_left + j - 1, 1, 1);
                t_cells.AddComment(mat.CommentText != null ? mat.CommentText : "");
                if (!t_brief)
                {
                    t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.TypeProp;
                    t_mats_data[t_top + i - 1, t_left + j++ - 1] = MUs.GetQuantity(mat.ParameterList["reqflow"].MU);
                }
                t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.PriceProp.Value == mat.ParameterList["price"].NonValue ? Default.price.ToString() : mat.PriceProp.Value.ToString();
                t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ParameterList["price"].MU.Length == 0 ? Default.money_mu.ToString() : mat.ParameterList["price"].MU;
                if (!t_brief)
                {
                    t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ReqFlowProp.Value == mat.ParameterList["reqflow"].NonValue ? Default.flow_rate_lower_bound.ToString() : mat.ReqFlowProp.Value.ToString();
                    t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ParameterList["reqflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["reqflow"].MU;
                    t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.MaxFlowProp.Value == mat.ParameterList["maxflow"].NonValue ? Default.flow_rate_upper_bound.ToString() : mat.MaxFlowProp.Value.ToString();
                    t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ParameterList["maxflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["maxflow"].MU;
                    t_mats_data[t_top + i - 1, t_left + j++ - 1] = "=" + def_Solution_Excel.text_ws_flows + "!" +
                        t_xlsx.CellName(t_flowtop + 2 * i, t_left + t_h_flows_offset + f_solution.OperatingUnits.Count);
                }
                else t_mats_data[t_top + i - 1, t_left + j++ - 1] = matProp.Value.Flow.ToString();
                t_mats_data[t_top + i - 1, t_left + j++ - 1] = mat.ParameterList["reqflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["reqflow"].MU;
                t_mats_data[t_top + i++ - 1, t_left + j++ - 1] = "=-" + t_xlsx.CellName(t_top + i - 1, c_matflow) + "*" +
                    t_xlsx.CellName(t_top + i - 1, c_matprice);

                if ((t_top + i - t_v_offset) % 2 == 0)
                {
                    t_cells = t_xlsx.Cells(t_top + i - 1, t_left, 1, j);
                    t_cells.Interior.Color = color_mat_dark;
                }
            }
            string t_lastcostcell = t_xlsx.CellName(t_top + i - 1, c_matcost);
            t_mats_data[t_top + i - 1, t_left - 1] = def_Solution_Excel.text_mats_cost + " [" + Default.money_mu.ToString() + "/" + Default.time_mu.ToString() + "]";
            string t_matcostcell = t_xlsx.CellName(t_top + i, c_matcost);
            if (f_solution.Materials.Count > 0) t_mats_data[t_top + i++ - 1, c_matcost - 1] = "=sum(" + t_firstcostcell + ":" + t_lastcostcell + ")";
            else i++;
            if ((t_top + i - t_v_offset) % 2 == 0)
            {
                t_cells = t_xlsx.Cells(t_top + i - 1, t_left, 1, j);
                t_cells.Interior.Color = color_mat_dark;
            }
            #endregion

            #region Material table format
            t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left, f_solution.Materials.Count + 2, t_brief ? 1 : 3);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);
            int t_h = t_brief ? 1 : 3;
            t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h, f_solution.Materials.Count, 1);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
            t_cells.NumberFormat = "#,##0.00";
            t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 1, f_solution.Materials.Count, 1);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);

            t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 2, f_solution.Materials.Count, 1);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
            t_cells.NumberFormat = "#,##0.00";
            t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 3, f_solution.Materials.Count, 1);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);

            t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 4, f_solution.Materials.Count + (t_brief ? 1 : 0), 1);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
            t_cells.NumberFormat = "#,##0.00";
            if (!t_brief)
            {
                t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 5, f_solution.Materials.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);

                t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 6, f_solution.Materials.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
                t_cells.NumberFormat = "#,##0.00";
                t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 7, f_solution.Materials.Count, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);

                t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left + t_h + 8, f_solution.Materials.Count + 1, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
                t_cells.NumberFormat = "#,##0.00";
            }
            t_cells = t_xlsx.Cells(t_top + i - 1, t_left, 1, j - 1);
            t_cells.Merge(false);
            #endregion

            #region Materials table borders
            t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left, f_solution.Materials.Count + 1, t_h + 1);
            t_cells.Borders[XlBordersIndex.xlInsideVertical].Color = color_border;
            t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left + t_h + 2, f_solution.Materials.Count + 1, 2 * t_h);
            t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = color_border;
            if (!t_brief)
            {
                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left + t_h + 4, f_solution.Materials.Count + 1, 2);
                t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = color_border;
            }
            t_cells = t_xlsx.Cells(t_top + t_v_offset - 1, t_left, f_solution.Materials.Count + 3, j);
            t_cells.Borders[XlBordersIndex.xlEdgeTop].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = color_border;
            t_cells = t_xlsx.Cells(t_top + t_v_offset + 1, t_left, f_solution.Materials.Count, j);
            t_cells.Borders[XlBordersIndex.xlEdgeTop].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;
            #endregion
            #endregion

            #region Operating Units Sheet
            ((_Worksheet)t_xlsx.WSOpUnits).Activate();
            object[,] t_ous_data = new object[t_outop + t_v_ous_offset + f_solution.OperatingUnits.Count + 3, t_ouleft + t_h_ous_offset + (t_brief ? 4 : 12)];
            t_top = t_outop;
            t_left = t_ouleft;
            i = t_v_ous_offset;
            j = t_h_ous_offset;
            int c_payout = 1, c_ofix = 1, c_oprop = 1, c_ifix = 1, c_iprop = 1;

            t_ous_data[t_top + i - 1, t_left - 1] = def_Solution_Excel.text_opunits;
            t_cells = t_xlsx.Cells(t_top + i++, t_left, 1, t_brief ? 5 : 13);
            t_cells.Merge(false);

            t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_ou_name;
            t_cells = t_xlsx.Cells(t_top + i, t_left + j++, 2, 1);
            t_cells.Merge(false);

            int c_size = t_left + j;
            t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_size;
            t_cells = t_xlsx.Cells(t_top + i, t_left + j++, 2, 1);
            t_cells.Merge(false);

            if (!t_brief)
            {
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
                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_ofix + "\n[" + Default.money_mu.ToString() + "/" + Default.time_mu.ToString() + "]";
                c_oprop = t_left + j;
                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_oprop + "\n[" + Default.money_mu.ToString() + "/" + Default.time_mu.ToString() + "]";

                t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_invcost;
                t_cells = t_xlsx.Cells(t_top + i, t_left + j, 1, 2);
                t_cells.Merge(false);

                c_ifix = t_left + j;
                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_ifix + "\n[" + Default.money_mu.ToString() + "]";
                c_iprop = t_left + j;
                t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_iprop + "\n[" + Default.money_mu.ToString() + "]";
            }
            t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_overallcost;
            t_cells = t_xlsx.Cells(t_top + i, t_left + j, 1, 2);
            t_cells.Merge(false);

            int c_fix = t_left + j;
            t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_fix + "\n[" + Default.money_mu.ToString() + "/" + Default.time_mu.ToString() + "]";
            int c_prop = t_left + j;
            t_ous_data[t_top + i, t_left + j++ - 1] = def_Solution_Excel.text_prop + "\n[" + Default.money_mu.ToString() + "/" + Default.time_mu.ToString() + "]";
            int c_oucost = t_left + j;
            t_ous_data[t_top + i - 1, t_left + j - 1] = def_Solution_Excel.text_ou_cost + "\n[" + Default.money_mu.ToString() + "/" + Default.time_mu.ToString() + "]";
            t_cells = t_xlsx.Cells(t_top + i++, t_left + j++, 2, 1);
            t_cells.Merge(false);

            t_cells = t_xlsx.Cells(t_top, t_left, 3, t_brief ? 5 : 13);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.center);
            t_cells.Interior.Color = color_ou;

            t_cells = t_xlsx.Cells(t_top + 3, t_left + 1, f_solution.OperatingUnits.Count + 1, t_brief ? 4 : 12);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
            t_cells.NumberFormat = "#,##0.00";
            t_cells.Interior.Color = color_ou;

            t_cells = t_xlsx.Cells(t_top + 3, t_left, f_solution.OperatingUnits.Count + 1, 1);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);
            t_cells.Interior.Color = color_ou;

            t_cells = t_xlsx.Cells(t_top + 3, t_left + 2, f_solution.OperatingUnits.Count, 1);
            t_cells.NumberFormat = "#,##0";

            t_cells = t_xlsx.Cells(t_top + 1, t_left, f_solution.OperatingUnits.Count + 2, t_brief ? 5 : 13);
            t_cells.Borders[XlBordersIndex.xlInsideVertical].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;

            t_cells = t_xlsx.Cells(t_top, t_left, f_solution.OperatingUnits.Count + 4, t_brief ? 5 : 13);
            t_cells.Borders[XlBordersIndex.xlEdgeTop].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = color_border;

            t_cells = t_xlsx.Cells(t_top + 2, t_left, 1, t_brief ? 5 : 13);
            t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;

            t_firstcostcell = t_xlsx.CellName(t_top + ++i, c_oucost);
            foreach (KeyValuePair<string, OperatingUnitProperty> ouProp in f_solution.OperatingUnits)
            {
                OperatingUnit ou = graph.GetOperatingUnitByName(ouProp.Value.Name);
                if (ou == null)
                {
                    MessageBox.Show("Cannot export solution to Excel! Materials or Operating units are not the same in the problem and in the solution.");
                    return;
                }
                if ((t_top + i) % 2 == 1)
                {
                    t_cells = t_xlsx.Cells(t_top + i, t_left, 1, t_brief ? 5 : 13);
                    t_cells.Interior.Color = color_ou_dark;
                }
                j = 0;
                t_ous_data[t_top + i - 1, t_left + j - 1] = ouProp.Value.Name;
                t_cells = t_xlsx.Cells(t_top + i, t_left + j++, 1, 1);
                //Console.WriteLine("OU név: " + ouProp.Value.Name);
                t_cells.AddComment(ou.CommentText);
                if (!t_brief)
                {
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = "=" + def_Solution_Excel.text_ws_flows + "!" +
                        t_xlsx.CellName(t_flowtop + t_v_flows_offset - 1, t_flowleft + t_h_flows_offset + i - t_v_flows_offset);
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.WorkingHourProp.Value == ou.ParameterList["workinghour"].NonValue ? Default.worging_hours_per_year.ToString() : ou.WorkingHourProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.PayoutPeriodProp.Value == ou.ParameterList["payoutperiod"].NonValue ? Default.payout_period.ToString() : ou.PayoutPeriodProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.CapacityLowerProp.Value == ou.ParameterList["caplower"].NonValue ? Default.capacity_lower_bound.ToString() : ou.CapacityLowerProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.CapacityUpperProp.Value == ou.ParameterList["capupper"].NonValue ? Default.capacity_upper_bound.ToString() : ou.CapacityUpperProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.OperatingCostFixProp.Value == ou.ParameterList["opercostfix"].NonValue ? Default.o_fix.ToString() : ou.OperatingCostFixProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.OperatingCostPropProp.Value == ou.ParameterList["opercostprop"].NonValue ? Default.o_prop.ToString() : ou.OperatingCostPropProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.InvestmentCostFixProp.Value == ou.ParameterList["investcostfix"].NonValue ? Default.i_fix.ToString() : ou.InvestmentCostFixProp.Value.ToString();
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ou.InvestmentCostPropProp.Value == ou.ParameterList["investcostprop"].NonValue ? Default.i_prop.ToString() : ou.InvestmentCostPropProp.Value.ToString();
                    /* Ezeket a mezőket nem számoljuk, mert az idő mértékegység miatt nehézkes lenne, ha a z idő nem évben van 
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = "=" + t_xlsx.CellName(t_top + i, c_ofix) +
                        "+" + t_xlsx.CellName(t_top + i, c_ifix) + "/" + t_xlsx.CellName(t_top + i, c_payout);
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = "=" + t_xlsx.CellName(t_top + i, c_oprop) +
                        "+" + t_xlsx.CellName(t_top + i, c_iprop) + "/" + t_xlsx.CellName(t_top + i, c_payout);
                    */
                }
                else
                {
                    t_ous_data[t_top + i - 1, t_left + j++ - 1] = ouProp.Value.Size.ToString();
                }
                t_ous_data[t_top + i - 1, t_left + j++ - 1] = (ou.InvestmentCostFixProp.Value / (ou.PayoutPeriodProp.Value * ou.WorkingHourProp.Value) + ou.OperatingCostFixProp.Value).ToString();
                t_ous_data[t_top + i - 1, t_left + j++ - 1] = (ou.InvestmentCostPropProp.Value / (ou.PayoutPeriodProp.Value * ou.WorkingHourProp.Value) + ou.OperatingCostPropProp.Value).ToString();
                t_ous_data[t_top + i - 1, t_left + j++ - 1] = "=" + t_xlsx.CellName(t_top + i, c_size) + "*" + t_xlsx.CellName(t_top + i, c_prop) +
                    "+" + t_xlsx.CellName(t_top + i, c_fix);
                i++;
            }
            if ((t_top + i) % 2 == 1)
            {
                t_cells = t_xlsx.Cells(t_top + i, t_left, 1, t_brief ? 5 : 13);
                t_cells.Interior.Color = color_ou_dark;
            }
            t_lastcostcell = t_xlsx.CellName(t_top + i - 1, c_oucost);
            t_ous_data[t_top + i - 1, t_left - 1] = def_Solution_Excel.text_ous_cost + " [" + Default.money_mu.ToString() + "/" + Default.time_mu.ToString() + "]";
            string t_oucostcell = t_xlsx.CellName(t_top + i, c_oucost);
            if (f_solution.OperatingUnits.Count > 0) t_ous_data[t_top + i - 1, c_oucost - 1] = "=sum(" + t_firstcostcell + ":" + t_lastcostcell + ")";
            t_cells = t_xlsx.Cells(t_top + i, t_left, 1, t_brief ? 4 : 12);
            t_cells.Merge(false);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);

            t_xlsx.WSOpUnits.get_Range("A1", t_xlsx.CellName(t_outop + t_v_ous_offset + f_solution.OperatingUnits.Count + 3, t_ouleft +
                t_h_ous_offset + (t_brief ? 4 : 12))).Value2 = t_ous_data;
            #endregion

            #region Flows Sheet (only in detailed view)
            if (!t_brief)
            {
                ((_Worksheet)t_xlsx.WSFlows).Activate();
                object[,] t_flows_data = new object[t_flowtop + t_v_flows_offset + f_solution.Materials.Count * 2 - 1, t_flowleft + t_h_flows_offset + f_solution.OperatingUnits.Count + 2];

                #region Positions
                t_top = t_flowtop;
                t_left = t_flowleft;
                t_v_offset = t_v_flows_offset;
                t_h_offset = t_h_flows_offset;
                i = t_v_offset;
                j = t_h_offset;
                #endregion

                #region Rates and flows, Size, Overall flows labels
                t_cells = t_xlsx.Cells(t_top, t_left, f_solution.Materials.Count * 2 + t_v_offset, t_h_offset + f_solution.OperatingUnits.Count + 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.center);
                t_cells.Interior.Color = color_mat;

                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left + t_h_offset, f_solution.Materials.Count * 2, f_solution.OperatingUnits.Count);
                t_cells.Interior.Color = color_rate;

                t_flows_data[t_top - 1, t_left - 1] = def_Solution_Excel.text_rates_and_flows_label;
                t_cells = t_xlsx.Cells(t_top, t_left, t_v_offset, t_h_offset);
                t_cells.Merge(false);

                t_flows_data[t_top, t_left + t_v_offset - 1] = def_Solution_Excel.text_opunit_size_label;
                t_cells = t_xlsx.Cells(t_top + 1, t_left + t_v_offset, 1, f_solution.OperatingUnits.Count);
                t_cells.Merge(false);
                t_cells.Interior.Color = color_size;

                t_flows_data[t_top - 1, t_left + t_h_offset + f_solution.OperatingUnits.Count - 1] = def_Solution_Excel.text_overall_flows_label;
                t_cells = t_xlsx.Cells(t_top, t_left + t_h_offset + f_solution.OperatingUnits.Count, t_v_offset, 1);
                t_cells.Merge(false);
                t_cells.Orientation = 90;
                #endregion

                #region Material flows, FlowMUs, rate&flow labels, Overall material flows
                foreach (KeyValuePair<string, MaterialProperty> matProp in f_solution.Materials)
                {
                    Material mat = graph.GetMaterialByName(matProp.Value.Name);
                    if (mat == null)
                    {
                        MessageBox.Show("Cannot export solution to Excel! Materials or Operating units are not the same in the problem and in the solution.");
                        return;
                    }
                    t_flows_data[t_top + i - 1, t_left - 1] = matProp.Value.Name;
                    t_cells = t_xlsx.Cells(t_top + i, t_left, 2, 1);
                    t_cells.Merge(false);

                    t_flows_data[t_top + i - 1, t_left] = "[" + (mat.ParameterList["reqflow"].MU.Length == 0 ? Default.mass_mu.ToString() + "/" + Default.time_mu.ToString() : mat.ParameterList["reqflow"].MU) + "]";
                    t_cells = t_xlsx.Cells(t_top + i, t_left + 1, 2, 1);
                    t_cells.Merge(false);

                    t_flows_data[t_top + i - 1, t_left + 1] = def_Solution_Excel.text_rate;
                    t_flows_data[t_top + i, t_left + 1] = def_Solution_Excel.text_flow;
                    if ((i - t_v_offset) % 4 == 0)
                    {
                        t_cells = t_xlsx.Cells(t_top + i, t_left, 2, t_h_offset + f_solution.OperatingUnits.Count + 1);
                        t_cells.Interior.Color = color_mat_dark;

                        t_cells = t_xlsx.Cells(t_top + i, t_left + t_h_offset, 2, f_solution.OperatingUnits.Count);
                        t_cells.Interior.Color = color_rate_dark;
                    }

                    t_flows_data[t_top + i, t_left + f_solution.OperatingUnits.Count + t_h_offset - 1] = "=sum(" + t_xlsx.CellName(t_top + i + 1, t_left + t_h_offset) + ":" +
                        t_xlsx.CellName(t_top + i + 1, t_left + t_h_offset + f_solution.OperatingUnits.Count - 1) + ")";
                    i += 2;
                }

                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left, f_solution.Materials.Count * 2, t_h_offset);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);

                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left + 1, f_solution.Materials.Count * 2, 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
                #endregion

                #region Opunit name, Size, rates&flows matrix
                foreach (KeyValuePair<string, OperatingUnitProperty> ouProp in f_solution.OperatingUnits)
                {
                    OperatingUnit ou = graph.GetOperatingUnitByName(ouProp.Value.Name);
                    t_flows_data[t_top - 1, t_left + j - 1] = ouProp.Value.Name;

                    t_flows_data[t_top + 1, t_left + j - 1] = ouProp.Value.Size.ToString();

                    foreach (MaterialProperty t_prop in ouProp.Value.Input)
                    {
                        Material t_mat = graph.GetMaterialByName(t_prop.Name);
                        if (t_mat == null)
                        {
                            MessageBox.Show("Cannot export solutin to Excel! Materials or Operating units are not the same in the problem and in the solution.");
                            return;
                        }
                        i = t_v_offset;
                        bool t_found = false;
                        foreach (KeyValuePair<string, MaterialProperty> matProp in f_solution.Materials)
                        {
                            if (matProp.Value.Name == t_mat.Name)
                            {
                                t_found = true;
                                break;
                            }
                            i += 2;
                        }
                        if (t_found)
                        {
                            t_flows_data[t_top + i - 1, t_left + j - 1] = (-t_prop.Flow/ouProp.Value.Size).ToString();

                            t_flows_data[t_top + i, t_left + j - 1] = "=" + t_xlsx.CellName(t_top + 2, t_left + j) + "*" +
                                t_xlsx.CellName(t_top + i, t_left + j);
                        }
                    }

                    foreach (MaterialProperty t_prop in ouProp.Value.Output)
                    {
                        Material t_mat = graph.GetMaterialByName(t_prop.Name);
                        if (t_mat == null)
                        {
                            MessageBox.Show("Cannot export solutin to Excel! Materials or Operating units are not the same in the problem and in the solution.");
                            return;
                        }
                        i = t_v_offset;
                        bool t_found = false;
                        foreach (KeyValuePair<string, MaterialProperty> matProp in f_solution.Materials)
                        {
                            if (matProp.Value.Name == t_mat.Name)
                            {
                                t_found = true;
                                break;
                            }
                            i += 2;
                        }
                        if (t_found)
                        {
                            t_flows_data[t_top + i - 1, t_left + j - 1] = (t_prop.Flow/ouProp.Value.Size).ToString();

                            t_flows_data[t_top + i, t_left + j - 1] = "=" + t_xlsx.CellName(t_top + 2, t_left + j) + "*" +
                                t_xlsx.CellName(t_top + i, t_left + j);
                        }
                    }
                    j++;
                }

                t_cells = t_xlsx.Cells(t_top, t_left + t_h_offset, 1, f_solution.OperatingUnits.Count);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.bottom);
                t_cells.Interior.Color = color_ou;
                t_cells.Orientation = 90;

                t_cells = t_xlsx.Cells(t_top + t_v_offset - 1, t_left + t_h_offset, 1, f_solution.OperatingUnits.Count);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
                t_cells.Interior.Color = color_size;
                t_cells.NumberFormat = "#,##0.00";

                t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left + t_h_offset, f_solution.Materials.Count * 2, f_solution.OperatingUnits.Count + 1);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
                t_cells.NumberFormat = "#,##0.00";
                #endregion

                #region Flow matrix borders
                t_cells = t_xlsx.Cells(t_top, t_left + t_h_offset - 1, t_v_offset + f_solution.Materials.Count * 2, f_solution.OperatingUnits.Count + 2);
                t_cells.Borders[XlBordersIndex.xlInsideVertical].Color = color_border;

                t_cells = t_xlsx.Cells(t_top, t_left, t_v_offset + f_solution.Materials.Count * 2, t_h_offset + f_solution.OperatingUnits.Count + 1);
                t_cells.Borders[XlBordersIndex.xlEdgeTop].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = color_border;

                t_cells = t_xlsx.Cells(t_top + 1, t_left + t_h_offset, 2, f_solution.OperatingUnits.Count);
                t_cells.Borders[XlBordersIndex.xlEdgeTop].Color = color_border;
                t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;
                #endregion

                solution_data = t_flows_data;
            }
            #endregion

            else
            {
                ((_Worksheet)t_xlsx.WSMaterials).Activate();
                solution_data = t_mats_data;
            }

            #region Solution Summary
            solution_data[1, 1] = def_Solution_Excel.text_solution_summary_label;
            t_cells = t_xlsx.Cells(2, 2, 1, 6);
            t_cells.Merge(false);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.center);
            t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;
            t_cells.Interior.Color = color_solution;

            solution_data[2, 1] = def_Solution_Excel.text_mats_cost;
            t_cells = t_xlsx.Cells(3, 2, 1, 3);
            t_cells.Merge(false);

            solution_data[2, 4] = "='" + def_Solution_Excel.text_ws_materials + "'!" + t_matcostcell;
            t_cells = t_xlsx.Cells(3, 5, 1, 2);
            t_cells.Merge(false);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);

            solution_data[2, 6] = Default.money_mu.ToString() + "/" + Default.time_mu.ToString();

            solution_data[3, 1] = def_Solution_Excel.text_ous_cost;
            t_cells = t_xlsx.Cells(4, 2, 1, 3);
            t_cells.Merge(false);

            solution_data[3, 4] = "='" + def_Solution_Excel.text_ws_opunits + "'!" + t_oucostcell;
            t_cells = t_xlsx.Cells(4, 5, 1, 2);
            t_cells.Merge(false);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);

            solution_data[3, 6] = Default.money_mu.ToString() + "/" + Default.time_mu.ToString();

            solution_data[4, 1] = def_Solution_Excel.text_solution_cost;
            t_cells = t_xlsx.Cells(5, 2, 1, 3);
            t_cells.Merge(false);

            solution_data[4, 4] = "=E3+E4";
            t_cells = t_xlsx.Cells(5, 5, 1, 2);
            t_cells.Merge(false);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);

            solution_data[4, 6] = Default.money_mu.ToString() + "/" + Default.time_mu.ToString();

            t_cells = t_xlsx.Cells(3, 2, 3, 3);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);

            t_cells = t_xlsx.Cells(3, 5, 3, 2);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);

            t_cells = t_xlsx.Cells(3, 7, 3, 1);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.center);

            t_cells = t_xlsx.Cells(3, 2, 1, 6);
            t_cells.Interior.Color = color_mat;

            t_cells = t_xlsx.Cells(4, 2, 1, 6);
            t_cells.Interior.Color = color_ou;

            t_cells = t_xlsx.Cells(5, 2, 1, 6);
            t_cells.Interior.Color = color_solution;

            t_cells = t_xlsx.Cells(2, 2, 4, 6);
            t_cells.Borders[XlBordersIndex.xlEdgeTop].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = color_border;
            #endregion

            if (!t_brief) t_xlsx.WSFlows.get_Range("A1", t_xlsx.CellName(t_flowtop + t_v_flows_offset + f_solution.Materials.Count * 2 - 1,
                t_flowleft + t_h_flows_offset + f_solution.OperatingUnits.Count + 2)).Value2 = solution_data;

            t_xlsx.WSMaterials.get_Range("A1", t_xlsx.CellName(t_mattop + (t_brief ? t_v_mat_brief_offset : t_v_mat_detailed_offset) + f_solution.Materials.Count + 1,
                t_matleft + 0 + (t_brief ? 5 : 11))).Value2 = t_mats_data;

            #region Excel finalisation
            t_xlsx.WSMaterials.Cells.Rows.AutoFit();
            t_xlsx.WSMaterials.Cells.Columns.AutoFit();
            t_xlsx.WSOpUnits.Cells.Rows.AutoFit();
            t_xlsx.WSOpUnits.Cells.Columns.AutoFit();
            if (!t_brief)
            {
                t_xlsx.WSFlows.Cells.Rows.AutoFit();
                t_xlsx.WSFlows.Cells.Columns.AutoFit();
                ((Range)(t_xlsx.WSFlows.Cells[1, 2])).ColumnWidth = System.Math.Max(14, (int)(double)((Range)(t_xlsx.WSFlows.Cells[1, 2])).ColumnWidth);
                ((Range)(t_xlsx.WSFlows.Cells[7, 1])).RowHeight = System.Math.Max(75, (int)(double)((Range)(t_xlsx.WSFlows.Cells[7, 1])).RowHeight);
            }
            else
            {
                ((Range)(t_xlsx.WSMaterials.Cells[1, 2])).ColumnWidth = System.Math.Max(17, (int)(double)((Range)(t_xlsx.WSMaterials.Cells[1, 2])).ColumnWidth);
                ((Range)(t_xlsx.WSOpUnits.Cells[1, 2])).ColumnWidth = System.Math.Max(9, (int)(double)((Range)(t_xlsx.WSOpUnits.Cells[1, 2])).ColumnWidth);
            }
            t_xlsx.App.Visible = t_visible;
            if (!t_visible) t_xlsx.Save(t_path, true);
            Thread.CurrentThread.CurrentCulture = t_original_culture;
            #endregion
        }
    }
}
