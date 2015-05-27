using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Threading;
using Microsoft.Office.Interop.Excel;
using Pns.Xml_Serialization.ExcelExport;
using Pns.Globals;
using Pns.Dialogs;
using Pns.SolutionsTab;

namespace Pns.ExcelExport
{
    class ResultsSummaryExcelExport
    {
        private AllTypeMaterials f_mats;
        private OperatingUnitPropertiesList f_ous;
        private Solutions f_solutions;

        public ResultsSummaryExcelExport(AllTypeMaterials mats, OperatingUnitPropertiesList ous, Solutions solutions) 
        {
            f_mats = mats;
            f_ous = ous;
            f_solutions = solutions;
        }

        #region Member functions
        public void ResultsToExcel(bool t_visible)
        {
            #region Excel initialisation
            string t_path = "";
            if (!t_visible)
            {
                t_path = Path.GetDirectoryName(PnsStudio.s_pns_editor.m_filename);
                if (t_path == "") t_path = Directory.GetCurrentDirectory();
                SaveFileDialog t_dialog;
                t_dialog = new SaveFileDialog();
                t_dialog.InitialDirectory = t_path;
                t_dialog.Filter = def_Solution_Excel.excel_extension;
                t_dialog.FileName = Converters.ToNameString((def_Solution_Excel.excel_filename_p5) +
                    Path.GetFileName(PnsStudio.s_pns_editor.m_filename));
                if (t_dialog.ShowDialog() != DialogResult.OK) return;
                t_path = t_dialog.FileName;
            }
            CultureInfo t_original_culture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            ExcelDoc t_xlsx = new ExcelDoc(true);

            t_xlsx.WSMaterials.Name = def_Solution_Excel.text_ws_summary_of_results;

            Range t_cells;
            int n_raws = f_mats.m_rawlist.Count;
            int n_prods = f_mats.m_productlist.Count;
            int n_ous = f_ous.Count;
            int t_top = 2;
            int t_left = 2;
            int t_v_offset = 3;
            int t_h_offset = 1;
            #endregion

            ((_Worksheet)t_xlsx.WSMaterials).Activate();
            object[,] t_data = new object[t_top + t_v_offset + f_solutions.Count - 1, t_left + t_h_offset + n_raws + n_prods + n_ous];

            #region Header labels
            int t_row, t_col;
            t_data[t_top - 1, t_left - 1] = def_Solution_Excel.text_solution_structures;
            t_xlsx.Cells(t_top, t_left, t_v_offset, 1).Merge(false);
            t_cells = t_xlsx.Cells(t_top, t_left, t_v_offset + f_solutions.Count, 1);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.left, def_Solution_Excel.VAlign.center);
            t_cells.Interior.Color = def_Solution_Excel.color_solution;

            if (n_raws > 0)
            {
                t_data[t_top - 1, t_left] = def_Solution_Excel.text_raw_materials;
                t_xlsx.Cells(t_top, t_left + 1, 1, n_raws).Merge(false);
            }

            if (n_prods > 0)
            {
                t_data[t_top - 1, t_left + n_raws] = def_Solution_Excel.text_product_materials;
                t_xlsx.Cells(t_top, t_left + 1 + n_raws, 1, n_prods).Merge(false);
            }

            if (n_ous > 0)
            {
                t_data[t_top - 1, t_left + n_raws + n_prods] = def_Solution_Excel.text_opunits;
                t_xlsx.Cells(t_top, t_left + 1 + n_raws + n_prods, 1, n_ous).Merge(false);
            }

            t_data[t_top - 1, t_left + n_raws + n_prods + n_ous] = def_Solution_Excel.text_summary_cost;
            t_data[t_top + 1, t_left + n_raws + n_prods + n_ous] = "[" + DefaultMUsAndValues.MUs.DefaultCostMU + "]";
            t_xlsx.Cells(t_top, t_left + 1 + n_raws + n_prods + n_ous, 2, 1).Merge(false);
            t_xlsx.Cells(t_top, t_left + 1 + n_raws + n_prods + n_ous, t_v_offset + f_solutions.Count, 1).Interior.Color = def_Solution_Excel.color_solution;

            t_col = 0;
            if (n_raws + n_prods > 0)
            {
                foreach (MaterialProperties mat in f_mats.m_rawlist)
                {
                    t_data[t_top, t_left + t_col] = mat.name;
                    t_data[t_top + 1, t_left + t_col++] = "[" + mat.DefaultFlowMU + "]";
                }
                foreach (MaterialProperties mat in f_mats.m_productlist)
                {
                    t_data[t_top, t_left + t_col] = mat.name;
                    t_data[t_top + 1, t_left + t_col++] = "[" + mat.DefaultFlowMU + "]";
                }
                t_xlsx.Cells(t_top, t_left + 1, 3 + f_solutions.Count, n_raws + n_prods).Interior.Color = def_Solution_Excel.color_mat;
            }
            if (n_ous > 0)
            {
                foreach (OperatingUnitProperties ou in f_ous) t_data[t_top, t_left + t_col++] = ou.name;
                t_xlsx.Cells(t_top, t_left + 1 + n_raws + n_prods, 3 + f_solutions.Count, n_ous).Interior.Color = def_Solution_Excel.color_ou;
            }
            t_xlsx.Align(t_xlsx.Cells(t_top, t_left, t_v_offset, n_raws + n_prods + n_ous + 2), def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.center);

            if (n_raws + n_prods + n_ous > 0)
            {
                t_cells = t_xlsx.Cells(t_top + 1, t_left + 1, 1, n_raws + n_prods + n_ous);
                t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.center, def_Solution_Excel.VAlign.bottom);
                t_cells.Orientation = 90;
            }
            #endregion

            #region Summary table
            t_row = 0;
            foreach (Solution sol in f_solutions)
            {
                t_col = 0;
                foreach (MaterialProperties mat in f_mats.m_rawlist)
                {
                    SolutionMaterial smat = sol.FindMaterial(mat.name);
                    if (smat != null) t_data[t_top + t_v_offset + t_row - 1, t_left + t_h_offset + t_col - 1] = smat.Amount;
                    t_col++;
                }
                foreach (MaterialProperties mat in f_mats.m_productlist)
                {
                    SolutionMaterial smat = sol.FindMaterial(mat.name);
                    if (smat != null) t_data[t_top + t_v_offset + t_row - 1, t_left + t_h_offset + t_col - 1] = smat.Amount;
                    t_col++;
                }
                foreach (OperatingUnitProperties ou in f_ous)
                {
                    SolutionOpUnit sou = sol.FindOpUnit(ou.name);
                    if (sou != null) t_data[t_top + t_v_offset + t_row - 1, t_left + t_h_offset + t_col - 1] = sou.Size;
                    t_col++;
                }
                t_data[t_top + t_v_offset + t_row - 1, t_left + t_h_offset + t_col - 1] = sol.Cost;
                if (t_row % 2 == 0)
                {
                    t_xlsx.Cells(t_top + t_v_offset + t_row, t_left, 1, 1).Interior.Color = def_Solution_Excel.color_solution_dark;
                    if (n_raws + n_prods > 0) t_xlsx.Cells(t_top + t_v_offset + t_row, t_left + t_h_offset, 1, n_raws + n_prods).Interior.Color = def_Solution_Excel.color_mat_dark;
                    if (n_ous > 0) t_xlsx.Cells(t_top + t_v_offset + t_row, t_left + t_h_offset + n_raws + n_prods, 1, n_ous).Interior.Color = def_Solution_Excel.color_ou_dark;
                    t_xlsx.Cells(t_top + t_v_offset + t_row, t_left + t_h_offset + n_raws + n_prods + n_ous, 1, 1).Interior.Color = def_Solution_Excel.color_solution_dark;
                }
                t_data[t_top + t_v_offset + t_row - 1, t_left - 1] = def_Solution_Excel.text_structure + ++t_row;
            }
            #endregion

            #region Summary table format
            t_cells = t_xlsx.Cells(t_top + t_v_offset, t_left + t_h_offset, f_solutions.Count, n_raws + n_prods + n_ous + 1);
            t_xlsx.Align(t_cells, def_Solution_Excel.HAlign.right, def_Solution_Excel.VAlign.center);
            t_cells.NumberFormat = "#,##0.00";
            #endregion

            #region Summary table borders
            t_cells = t_xlsx.Cells(t_top, t_left, f_solutions.Count + t_v_offset, n_raws + n_prods + n_ous + 2);
            t_cells.Borders[XlBordersIndex.xlEdgeTop].Color = def_Solution_Excel.color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeBottom].Color = def_Solution_Excel.color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = def_Solution_Excel.color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = def_Solution_Excel.color_border;

            t_xlsx.Cells(t_top + 1, t_left, f_solutions.Count + 2, n_raws + n_prods + n_ous + 2).Borders[XlBordersIndex.xlInsideVertical].Color = def_Solution_Excel.color_border;

            t_xlsx.Cells(t_top + 2, t_left, 1, n_raws + n_prods + n_ous + 2).Borders[XlBordersIndex.xlEdgeBottom].Color = def_Solution_Excel.color_border;

            t_cells = t_xlsx.Cells(t_top, t_left + t_h_offset, 1, n_raws);
            t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = def_Solution_Excel.color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = def_Solution_Excel.color_border;

            t_cells = t_xlsx.Cells(t_top, t_left + t_h_offset + n_raws + n_prods, 1, n_ous);
            t_cells.Borders[XlBordersIndex.xlEdgeLeft].Color = def_Solution_Excel.color_border;
            t_cells.Borders[XlBordersIndex.xlEdgeRight].Color = def_Solution_Excel.color_border;
            #endregion

            t_xlsx.WSMaterials.get_Range("A1", t_xlsx.CellName(t_top + t_v_offset + f_solutions.Count - 1, t_left + t_h_offset + n_raws + n_prods + n_ous)).Value2 = t_data;

            #region Excel finalisation
            t_xlsx.WSMaterials.Cells.Rows.AutoFit();
            t_xlsx.WSMaterials.Cells.Columns.AutoFit();
            ((Range)(t_xlsx.WSMaterials.Cells[1, 2])).ColumnWidth = System.Math.Max(12, (int)(double)((Range)(t_xlsx.WSMaterials.Cells[1, 2])).ColumnWidth);

            t_xlsx.App.Visible = t_visible;
            if (!t_visible) t_xlsx.Save(t_path, true);
            Thread.CurrentThread.CurrentCulture = t_original_culture;
            #endregion
        }
        #endregion
    }
}
