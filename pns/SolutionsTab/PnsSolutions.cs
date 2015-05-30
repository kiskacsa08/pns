using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using DynamicPropertyGrid;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.Text;
using Pns.PnsSolver;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;
using Pns.Globals;
using Pns.Dialogs;
using Pns.ExcelExport;
using Pns.SolutionsTab;

namespace Pns
{
    public partial class PnsStudio : Form
    {
        #region Members
        private bool m_result;
        private volatile Solutions m_solutions;
        private volatile StringBuilder m_result_string;
        private SolverDialog m_solver_dlg;
        private string m_method;
        private int m_startpos;
        private System.Windows.Forms.Timer m_timer;
        private bool update;
        #endregion

        #region Member functions
        private void UpdateSolutionsTree(TreeView t_treeview, ComboBox t_combobox, bool t_check_result)
        {
            t_treeview.Hide();
            t_treeview.Nodes.Clear();
            if (t_combobox.SelectedIndex != -1)
            {
                t_treeview.Nodes.Add(((Solution)t_combobox.SelectedItem).GenerateTreeResult(t_check_result));
                t_treeview.Nodes[0].Expand();
                foreach (TreeNode t_node in t_treeview.Nodes[0].Nodes) t_node.Expand();
            }
            t_treeview.Show();
        }

        static public void ShowResults()
        {
            if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
            s_pns_editor.comboBoxSolutionsLeft.Items.Clear();
            s_pns_editor.comboBoxSolutionsRight.Items.Clear();
            s_pns_editor.UpdateSolutionsTree(s_pns_editor.treeViewSolutionsLeft, s_pns_editor.comboBoxSolutionsLeft, true);
            s_pns_editor.UpdateSolutionsTree(s_pns_editor.treeViewSolutionsRight, s_pns_editor.comboBoxSolutionsRight, false);
            s_pns_editor.tabPageSolutions.Text = def_PnsStudio.SolutionsTabTextField + " [" + s_pns_editor.m_method + "] - " + s_pns_editor.m_filename.Substring(s_pns_editor.m_filename.LastIndexOf('\\') + 1);
            if (s_pns_editor.m_result && (s_pns_editor.m_method == Solver_keys._KEY_ABB || s_pns_editor.m_method == Solver_keys._KEY_SSGLP))
            {
                s_pns_editor.tabPageSolutions.Text += " (" + s_pns_editor.m_solutions.Count + ")";
                if (s_pns_editor.m_solutions.Count > 0)
                {
                    foreach (Solution t_solution in s_pns_editor.m_solutions)
                    {
                        s_pns_editor.comboBoxSolutionsLeft.Items.Add(t_solution);
                        s_pns_editor.comboBoxSolutionsRight.Items.Add(t_solution);
                    }
                    s_pns_editor.comboBoxSolutionsLeft.SelectedIndex = 0;
                    s_pns_editor.comboBoxSolutionsRight.SelectedIndex = 0;
                    s_pns_editor.richTextBoxSolutions.Hide();
                    s_pns_editor.splitContainerSolutions.Show();
                    return;
                }
            }
            s_pns_editor.richTextBoxSolutions.Text = s_pns_editor.m_result_string.ToString();
            s_pns_editor.richTextBoxSolutions.Select(0, 0);
            s_pns_editor.richTextBoxSolutions.ScrollToCaret();
            s_pns_editor.splitContainerSolutions.Hide();
            s_pns_editor.richTextBoxSolutions.Show();
        }

        static public void DeleteResults()
        {
            if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
            s_pns_editor.comboBoxSolutionsLeft.Items.Clear();
            s_pns_editor.comboBoxSolutionsRight.Items.Clear();
            s_pns_editor.tabPageSolutions.Text = def_PnsStudio.SolutionsTabTextField;
            s_pns_editor.richTextBoxSolutions.Text = "";
            s_pns_editor.richTextBoxSolutions.Select(0, 0);
            s_pns_editor.richTextBoxSolutions.ScrollToCaret();
            s_pns_editor.splitContainerSolutions.Hide();
            s_pns_editor.richTextBoxSolutions.Show();
            s_pns_editor.tabControlPns.SelectedIndex = 0;
        }

        public string StartSolver(string t_method)
        {
            s_pns_editor.tabPageSolutions.Text = def_PnsStudio.SolutionsTabTextField;
            m_solutions = new Solutions();
            comboBoxSolutionsLeft.Items.Clear();
            comboBoxSolutionsRight.Items.Clear();
            UpdateSolutionsTree(s_pns_editor.treeViewSolutionsLeft, s_pns_editor.comboBoxSolutionsLeft, true);
            UpdateSolutionsTree(s_pns_editor.treeViewSolutionsRight, s_pns_editor.comboBoxSolutionsRight, false);
            s_pns_editor.richTextBoxSolutions.Hide();
            s_pns_editor.splitContainerSolutions.Show();
            m_method = t_method;
            m_result_string = new StringBuilder(1000000);
            m_startpos = 0;
            Thread t_solver = new Thread(new ParameterizedThreadStart(SolverThread));
            m_solver_dlg = new SolverDialog(t_solver);
            m_timer = new System.Windows.Forms.Timer();
            m_timer.Tick += new EventHandler(m_timer_Tick);
            m_timer.Interval = 1000;
            t_solver.Start(t_method);
            m_solver_dlg.Show();
            m_timer.Start();
            update = false;
            Console.WriteLine("DDD: "+m_solutions.Count);
            return m_result_string.ToString();
        }

        void m_timer_Tick(object sender, EventArgs e)
        {
            if (update)
            {
                for (int j = 0; j < m_solutions.Count; j++)
                {
                    if (!m_solutions[j].Inserted)
                    {
                        if (j < comboBoxSolutionsLeft.Items.Count) comboBoxSolutionsLeft.Items.Insert(j, m_solutions[j]);
                        else comboBoxSolutionsLeft.Items.Add(m_solutions[j]);
                        if (comboBoxSolutionsLeft.SelectedIndex == -1) comboBoxSolutionsLeft.SelectedIndex = 0;
                        if (j < comboBoxSolutionsRight.Items.Count) comboBoxSolutionsRight.Items.Insert(j, m_solutions[j]);
                        else comboBoxSolutionsRight.Items.Add(m_solutions[j]);
                        if (comboBoxSolutionsRight.SelectedIndex == -1) comboBoxSolutionsRight.SelectedIndex = 0;
                        m_solutions[j].Inserted = true;
                    }
                    if (m_solutions.Count > DefaultMUsAndValues.DefaultValues.i_max_solutions)
                    {
                        for (int i = DefaultMUsAndValues.DefaultValues.i_max_solutions; i < comboBoxSolutionsLeft.Items.Count; )
                        {
                            if (comboBoxSolutionsLeft.SelectedIndex != i && comboBoxSolutionsRight.SelectedIndex != i)
                            {
                                comboBoxSolutionsLeft.Items.RemoveAt(i);
                                comboBoxSolutionsRight.Items.RemoveAt(i);
                                m_solutions.RemoveAt(i);
                            }
                            else i++;
                        }
                    }
                }
                update = false;
            }
        }

        public void ExportToExcel(object sender, defaults.SolutionToExcel t_export_type)
        {
            ComboBox t_combobox = (ComboBox)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            if (t_combobox.SelectedIndex != -1)
            {
                ResultExcelExport t_export = new ResultExcelExport(((Solution)t_combobox.SelectedItem));
                ResultsSummaryExcelExport t_summary = new ResultsSummaryExcelExport(m_materials, m_operatingunitlist, m_solutions);
                switch (t_export_type)
                {
                    case defaults.SolutionToExcel.brief_export:
                        t_export.ResultToExcel(true, false);
                        break;
                    case defaults.SolutionToExcel.brief_view:
                        t_export.ResultToExcel(true, true);
                        break;
                    case defaults.SolutionToExcel.detailed_export:
                        t_export.ResultToExcel(false, false);
                        break;
                    case defaults.SolutionToExcel.detailed_view:
                        t_export.ResultToExcel(false, true);
                        break;
                    case defaults.SolutionToExcel.export_summary_of_results:
                        t_summary.ResultsToExcel(false);
                        break;
                    case defaults.SolutionToExcel.view_summary_of_results:
                        t_summary.ResultsToExcel(true);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region Solver thread functions
        private void SolverThread(object t_method)
        {
            m_result = false;
            m_result = CallSolver((string)t_method);
        }

        public void ProblemToSolverInput()
        {
            CultureInfo t_original_culture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            FileStream fs = new FileStream(m_filename + Solver_keys._SOLVER_INPUT_EXTENSION, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            string t_str = Solver_keys._FILE_TYPE;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._FILE_NAME + Converters.ToNameString(Path.GetFileName(m_filename) + Solver_keys._SOLVER_INPUT_EXTENSION);
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._MEASUREMENT_UNITS;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._MASS_UNIT;
            t_str += DefaultMUsAndValues.MUs.DefaultMaterialMU;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._TIME_UNIT;
            t_str += DefaultMUsAndValues.MUs.DefaultTimeMU;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._MONEY_UNIT;
            t_str += "Euro";//Defaults.MUs.DefaultCurrencyMU;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._DEFAULTS;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._MATERIAL_TYPE;
            t_str += Solver_keys._INTERMEDIATE;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._DEF_MAT_LB;
            t_str += DefaultMUsAndValues.DefaultValues.d_minimum_flow;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._DEF_MAT_UB;
            t_str += DefaultMUsAndValues.DefaultValues.d_maximum_flow;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._DEF_MAT_PRICE;
            t_str += DefaultMUsAndValues.DefaultValues.d_price;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._DEF_OPUNIT_LB;
            t_str += DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_lower_bound;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._DEF_OPUNIT_UB;
            t_str += DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_upper_bound;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._DEF_OPUNIT_FIX;
            t_str += "0";
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._DEF_OPUNIT_PROP;
            t_str += "0";
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._NEWLINE;
            t_str += Solver_keys._MATERIALS;
            t_str += Solver_keys._NEWLINE;
            sw.Write(t_str);
            foreach (MaterialProperties matprop in m_materials.m_rawlist)
            {
                t_str = Solver_keys._RAW;
                t_str += matprop.gmin != def_Values.d_NperA ? ", " + Solver_keys._MAT_LB + matprop.gmin : "";
                t_str += matprop.gmax != def_Values.d_NperA ? ", " + Solver_keys._MAT_UB + matprop.gmax : "";
                t_str += matprop.gprice != def_Values.d_NperA ? ", " + Solver_keys._MAT_PRICE + matprop.gprice : "";
                sw.Write(matprop.currname + ": " + t_str + Solver_keys._NEWLINE);
            }
            foreach (MaterialProperties matprop in m_materials.m_intermediatelist)
            {
                if (matprop.gmax != def_Values.d_NperA) sw.Write(matprop.currname + ": " + Solver_keys._INTERMEDIATE + ", " + Solver_keys._MAT_UB + matprop.gmax + Solver_keys._NEWLINE);
            }
            foreach (MaterialProperties matprop in m_materials.m_productlist)
            {
                t_str = Solver_keys._PRODUCT;
                t_str += matprop.gmin != def_Values.d_NperA ? ", " + Solver_keys._MAT_LB + matprop.gmin : "";
                t_str += matprop.gmax != def_Values.d_NperA ? ", " + Solver_keys._MAT_UB + matprop.gmax : "";
                t_str += matprop.gprice != def_Values.d_NperA ? ", " + Solver_keys._MAT_PRICE + matprop.gprice : "";
                sw.Write(matprop.currname + ": " + t_str + Solver_keys._NEWLINE);
            }
            t_str = Solver_keys._NEWLINE;
            t_str += Solver_keys._OPUNITS;
            t_str += Solver_keys._NEWLINE;
            sw.Write(t_str);
            foreach (OperatingUnitProperties ouprop in m_operatingunitlist)
            {
                t_str = "";
                if (ouprop.bounds.d_lb != DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_lower_bound) t_str = Solver_keys._OPUNIT_LB + ouprop.bounds.d_lb;
                if (ouprop.bounds.d_ub != DefaultMUsAndValues.DefaultValues.CapacityConstraints.d_upper_bound)
                {
                    if (t_str != "") t_str += ", ";
                    t_str += Solver_keys._OPUNIT_UB + ouprop.bounds.d_ub;
                }
                if (ouprop.overallcost.g_fix != 0)
                {
                    if (t_str != "") t_str += ", ";
                    t_str += Solver_keys._OPUNIT_FIX + ouprop.overallcost.g_fix;
                }
                if (ouprop.overallcost.g_prop != 0)
                {
                    if (t_str != "") t_str += ", ";
                    t_str += Solver_keys._OPUNIT_PROP + ouprop.overallcost.g_prop;
                }
                if (t_str != "") sw.Write(ouprop.currname + ": " + t_str + Solver_keys._NEWLINE);
            }
            t_str = Solver_keys._NEWLINE;
            t_str += Solver_keys._OPUNIT_RATES;
            t_str += Solver_keys._NEWLINE;
            sw.Write(t_str);
            foreach (OperatingUnitProperties ouprop in m_operatingunitlist)
            {
                sw.Write(ouprop.currname + ": ");
                t_str = "";
                foreach (CustomProp prop in ouprop.imats.list)
                {
                    IOMaterial mat = (IOMaterial)prop.Value;
                    if (t_str != "") t_str += " + ";
                    if (mat.g_rate != 1) t_str += mat.g_rate + " ";
                    t_str += mat.Name;
                }
                sw.Write(t_str + " => ");
                t_str = "";
                foreach (CustomProp prop in ouprop.omats.list)
                {
                    IOMaterial mat = (IOMaterial)prop.Value;
                    if (t_str != "") t_str += " + ";
                    if (mat.g_rate != 1) t_str += mat.g_rate + " ";
                    t_str += mat.Name;
                }
                sw.Write(t_str + Solver_keys._NEWLINE);
            }
            sw.Write(Solver_keys._NEWLINE);
            sw.Close();
            fs.Close();
            Thread.CurrentThread.CurrentCulture = t_original_culture;
        }

        void t_proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                m_result_string.AppendLine(e.Data);
                int t_begin = 0;
                if ((t_begin = e.Data.IndexOf(Solver_keys._KEY_CURRENT_BEST_STRUCTURE, t_begin)) != -1)
                {
                    try
                    {
                        string t_filename = e.Data.Substring(Solver_keys._KEY_CURRENT_BEST_STRUCTURE.Length);
                        FileStream fs = new FileStream(t_filename, FileMode.Open);
                        StreamReader sr = new StreamReader(fs);
                        StringBuilder t_result_string = new StringBuilder(100000);
                        while (!sr.EndOfStream) t_result_string.AppendLine(sr.ReadLine());
                        sr.Close();
                        fs.Close();
                        try
                        {
                            File.Delete(t_filename);
                        }
                        catch (IOException iox)
                        {
                            m_result_string.AppendLine(iox.ToString());
                        }
                        Solution t_solution=ParseCurrentBestStructure(t_result_string.ToString(), t_filename);
                        if (t_solution != null)
                        {
                            int i;
                            for (i = 0; i < m_solutions.Count && t_solution.Cost >= m_solutions[i].Cost; i++) ;
                            if (i < m_solutions.Count) m_solutions.Insert(i, t_solution);
                            else m_solutions.Add(t_solution);
                            update = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        m_result_string.AppendLine(ex.ToString());
                    }
                }
            }
        }

        Solutions ParseSolution(string t_str)
        {
            CultureInfo t_original_culture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Solutions t_solutions = new Solutions();
            string t_rate_str, t_opunit_str, t_mat_str, t_amount_str;
            int t_begin = 0, t_pos, t_opunits_pos, t_annual_pos, t_solution_index = 0;
            double t_cost;
            while ((t_begin = t_str.IndexOf(Solver_keys._KEY_FEASIBLE_STRUCTURE, t_begin)) != -1)
            {
                t_pos = t_str.IndexOf(Solver_keys._KEY_MATERIALS, t_begin);
                t_opunits_pos = t_str.IndexOf(Solver_keys._KEY_OPERATING_UNITS, t_begin);
                t_cost = 0;
                if ((t_annual_pos = t_str.IndexOf(Solver_keys._KEY_TOTAL_ANNUAL_COST, t_begin)) != -1)
                {
                    int t_bpos = t_str.IndexOf("= ", t_annual_pos) + 2;
                    int t_epos = t_bpos - 1;
                    while (t_str[++t_epos] == ' ') ;
                    while (t_str[++t_epos] != ' ' && t_str[t_epos] != '\n') ;
                    t_cost = Converters.ToDouble(t_str.Substring(t_bpos, t_epos - t_bpos));
                }
                t_begin = t_pos;
                Solution t_solution = new Solution((++t_solution_index).ToString(), t_cost);
                t_solutions.Add(t_solution);
                if (t_begin != -1)
                {
                    do
                    {
                        t_pos = t_begin;
                        while (t_str.Length > t_pos && t_str[t_pos] != '\n') t_pos++;
                        if (t_opunits_pos > t_pos + 1)
                        {
                            t_begin = t_pos + 1;
                            int t_pos_colon = t_str.IndexOf(':', t_begin);
                            t_pos = t_str.IndexOf('(', t_begin);
                            if (t_pos == -1 || t_pos > t_pos_colon) t_pos = t_pos_colon + 1;
                            t_mat_str = t_str.Substring(t_begin, t_pos - 1 - t_begin);
                            t_begin = t_pos_colon + 2;
                            t_pos = t_str.IndexOf('\n', t_begin);
                            t_amount_str = t_str.Substring(t_begin, t_pos - t_begin);
                            t_begin = t_pos;
                            t_solution.AddMaterial(PnsStudio.FindMaterial(t_mat_str), Converters.ToDouble(t_amount_str));
                        }
                    } while (t_opunits_pos > t_pos + 1);
                }
                t_begin = t_opunits_pos;
                if (t_begin != -1)
                {
                    do
                    {
                        t_pos = t_begin;
                        while (t_annual_pos > t_pos + 1 && !(t_str[t_pos] == '\n' && t_str[t_pos + 1] >= '0' && t_str[t_pos + 1] <= '9')) t_pos++;
                        if (t_annual_pos > t_pos + 1)
                        {
                            t_begin = t_pos + 1;
                            t_pos = t_str.IndexOf('*', t_begin);
                            t_rate_str = t_str.Substring(t_begin, t_pos - t_begin);
                            t_begin = t_pos + 1;
                            t_pos = t_str.IndexOf('(', t_begin);
                            t_opunit_str = t_str.Substring(t_begin, t_pos - 1 - t_begin);
                            t_begin = t_pos + 1;
                            t_solution.AddOpUnit(PnsStudio.FindOperatingUnit(t_opunit_str), Converters.ToDouble(t_rate_str));
                        }
                    } while (t_annual_pos > t_pos + 1);
                }
            }
            Thread.CurrentThread.CurrentCulture = t_original_culture;
            return t_solutions;
        }

        Solution ParseCurrentBestStructure(string t_str, string t_ID)
        {
            string t_rate_str, t_opunit_str, t_mat_str, t_amount_str;
            int t_begin = 0, t_pos, t_opunits_pos, t_annual_pos;
            double t_cost;
            if ((t_begin = t_str.IndexOf(Solver_keys._KEY_CURRENT_BEST_STRUCTURE, t_begin)) != -1)
            {
                CultureInfo t_original_culture = CultureInfo.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                t_pos = t_str.IndexOf(Solver_keys._KEY_MATERIALS, t_begin);
                t_opunits_pos = t_str.IndexOf(Solver_keys._KEY_OPERATING_UNITS, t_begin);
                t_cost = 0;
                if ((t_annual_pos = t_str.IndexOf(Solver_keys._KEY_TOTAL_ANNUAL_COST, t_begin)) != -1)
                {
                    int t_bpos = t_str.IndexOf("= ", t_annual_pos) + 2;
                    int t_epos = t_bpos - 1;
                    while (t_str[++t_epos] == ' ') ;
                    while (t_str[++t_epos] != ' ' && t_str[t_epos] != '\n') ;
                    t_cost = Converters.ToDouble(t_str.Substring(t_bpos, t_epos - t_bpos));
                }
                t_begin = t_pos;
                Solution t_solution = new Solution(t_ID, t_cost);
                if (t_begin != -1)
                {
                    do
                    {
                        t_pos = t_begin;
                        while (t_str.Length > t_pos && t_str[t_pos] != '\n') t_pos++;
                        if (t_opunits_pos > t_pos + 1)
                        {
                            t_begin = t_pos + 1;
                            int t_pos_colon = t_str.IndexOf(':', t_begin);
                            t_pos = t_str.IndexOf('(', t_begin);
                            if (t_pos == -1 || t_pos > t_pos_colon) t_pos = t_pos_colon + 1;
                            t_mat_str = t_str.Substring(t_begin, t_pos - 1 - t_begin);
                            t_begin = t_pos_colon + 2;
                            t_pos = t_str.IndexOf('\n', t_begin);
                            t_amount_str = t_str.Substring(t_begin, t_pos - t_begin);
                            t_begin = t_pos;
                            t_solution.AddMaterial(PnsStudio.FindMaterial(t_mat_str), Converters.ToDouble(t_amount_str));
                        }
                    } while (t_opunits_pos > t_pos + 1);
                }
                t_begin = t_opunits_pos;
                if (t_begin != -1)
                {
                    do
                    {
                        t_pos = t_begin;
                        while (t_annual_pos > t_pos + 1 && !(t_str[t_pos] == '\n' && t_str[t_pos + 1] >= '0' && t_str[t_pos + 1] <= '9')) t_pos++;
                        if (t_annual_pos > t_pos + 1)
                        {
                            t_begin = t_pos + 1;
                            t_pos = t_str.IndexOf('*', t_begin);
                            t_rate_str = t_str.Substring(t_begin, t_pos - t_begin);
                            t_begin = t_pos + 1;
                            t_pos = t_str.IndexOf('(', t_begin);
                            t_opunit_str = t_str.Substring(t_begin, t_pos - 1 - t_begin);
                            t_begin = t_pos + 1;
                            t_solution.AddOpUnit(PnsStudio.FindOperatingUnit(t_opunit_str), Converters.ToDouble(t_rate_str));
                        }
                    } while (t_annual_pos > t_pos + 1);
                }
                Thread.CurrentThread.CurrentCulture = t_original_culture;
                return t_solution;
            }
            return null;
        }

        bool CallSolver(string t_method)
        {
            ProblemToSolverInput();
            ProcessStartInfo t_info = new ProcessStartInfo();
            t_info.WorkingDirectory = System.Windows.Forms.Application.StartupPath;
            t_info.FileName = Solver_keys.solver;
            t_info.Arguments = t_method +
                " \"" + m_filename + Solver_keys._SOLVER_INPUT_EXTENSION + "\" " +
                "\"" + m_filename + Solver_keys._SOLVER_ROW_OUTPUT_EXTENSION + "\" " +
                DefaultMUsAndValues.DefaultValues.max_solutions;
            t_info.RedirectStandardError = true;
            t_info.RedirectStandardOutput = true;
            t_info.CreateNoWindow = true;
            t_info.UseShellExecute = false;
            t_info.WindowStyle = ProcessWindowStyle.Hidden;
            Process t_proc = new Process();
            try
            {
                t_proc.StartInfo = t_info;
                t_proc.OutputDataReceived += new DataReceivedEventHandler(t_proc_OutputDataReceived);
                t_proc.Start();
                m_solver_dlg.SolverProcess = t_proc;
                t_proc.BeginOutputReadLine();
                t_proc.WaitForExit();
            }
            catch (Exception ex)
            {
                m_result_string.AppendLine(ex.ToString());
                return false;
            }
            if (t_proc.ExitCode != 0)
            {
                string t_error = t_proc.StandardError.ReadToEnd();
                if (t_error == "") m_result_string.AppendLine("Solver error!\nExit code: " + t_proc.ExitCode);
                else m_result_string.AppendLine(t_error);
                return false;
            }
            m_timer.Stop();
            m_startpos = 0;
            FileStream fs = new FileStream(m_filename + Solver_keys._SOLVER_ROW_OUTPUT_EXTENSION, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            m_result_string = new StringBuilder();
            while (!sr.EndOfStream &&
                (m_result_string.Length < Solver_keys._SOLVER_ROW_OUTPUT_MAX_DISPLAYED_SIZE ||
                t_method == Solver_keys._KEY_ABB || t_method == Solver_keys._KEY_SSGLP)) m_result_string.AppendLine(sr.ReadLine());
            if (!sr.EndOfStream) m_result_string.AppendLine("...");
            sr.Close();
            fs.Close();
            if (t_method == Solver_keys._KEY_ABB || t_method == Solver_keys._KEY_SSGLP)
            {
                m_solutions = ParseSolution(m_result_string.ToString());
            }

            return true;
        }
        #endregion

        #region Solutions Event Handlers
        private void comboBoxSolutionsLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSolutionsTree(treeViewSolutionsLeft, comboBoxSolutionsLeft, true);
        }

        private void comboBoxSolutionsRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSolutionsTree(treeViewSolutionsRight, comboBoxSolutionsRight, comboBoxSolutionsLeft.SelectedIndex != comboBoxSolutionsRight.SelectedIndex);
        }

        private void treeViewSolutionsLeft_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) contextMenuStripSolutionTree.Show((Control)comboBoxSolutionsLeft, e.Location);
        }

        private void treeViewSolutionsRight_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) contextMenuStripSolutionTree.Show((Control)comboBoxSolutionsRight, e.Location);
        }

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToExcel(sender, defaults.SolutionToExcel.brief_export);
        }

        private void viewInExcelFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToExcel(sender, defaults.SolutionToExcel.brief_view);
        }

        private void detailedExportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToExcel(sender, defaults.SolutionToExcel.detailed_export);
        }

        private void detailedViewInExcelFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToExcel(sender, defaults.SolutionToExcel.detailed_view);
        }

        private void exportSummaryOfAllSolutionStructuresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToExcel(sender, defaults.SolutionToExcel.export_summary_of_results);
        }

        private void viewSummaryOfAllSolutionStructuresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToExcel(sender, defaults.SolutionToExcel.view_summary_of_results);
        }
        #endregion

        #region Properties
        static public string SolutionsString
        {
            get
            {
                if (s_pns_editor == null) throw new Exception(def_PnsStudio_ex.Ex_PnsEditor_is_null);
                int t_start = s_pns_editor.m_startpos;
                int t_lenght = s_pns_editor.m_result_string.Length - s_pns_editor.m_startpos;
                s_pns_editor.m_startpos = s_pns_editor.m_result_string.Length;
                return s_pns_editor.m_result_string.ToString(t_start, t_lenght);
            }
        }
        #endregion
    }
}
