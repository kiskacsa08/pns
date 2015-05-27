using System.Windows.Forms;
using System.Collections.Generic;
using Pns.PnsSolver;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;
using Pns.Globals;
using Pns.Dialogs;

namespace Pns.SolutionsTab
{
    public class SolutionMaterial
    {
        #region Members
        private MaterialProperties m_matprop;
        private double m_amount;
        #endregion

        #region Constructors
        public SolutionMaterial(MaterialProperties t_matprop, double t_amount)
        {
            m_matprop = t_matprop;
            m_amount = t_amount;
        }
        #endregion

        #region Member functions
        public double GenerateResult(ref string t_result)
        {
            double t_amount = m_amount;
            t_result += m_matprop.GenOutput(ref t_amount);
            return t_amount;
        }
        public double GenerateTreeResult(TreeNode t_node)
        {
            return m_matprop.GenTreeOutput(m_amount, t_node);
        }
        #endregion

        #region Properties
        public MaterialProperties MatProp { get { return m_matprop; } }
        public double Amount { get { return m_amount; } }
        #endregion
    }
    public class SolutionMaterials : List<SolutionMaterial> { }
    public class SolutionOpUnit
    {
        #region Members
        private OperatingUnitProperties m_ouprop;
        private double m_size;
        #endregion

        #region Constructors
        public SolutionOpUnit(OperatingUnitProperties t_ouprop, double t_size)
        {
            m_ouprop = t_ouprop;
            m_size = t_size;
        }
        #endregion

        #region Member functions
        public double GenerateResult(ref string t_result)
        {
            double t_size = m_size;
            t_result += m_ouprop.GenOutput(ref t_size);
            return t_size;
        }
        public double GenerateTreeResult(TreeNode t_node)
        {
            return m_ouprop.GenTreeOutput(m_size, t_node);
        }
        #endregion

        #region Properties
        public OperatingUnitProperties OUProp { get { return m_ouprop; } }
        public double Size { get { return m_size; } }
        #endregion
    }
    public class SolutionOpUnits : List<SolutionOpUnit> { }
    public partial class Solution
    {
        #region Members
        private string m_ID;
        private SolutionMaterials m_mats;
        private SolutionOpUnits m_ous;
        private double m_cost;
        private bool m_inserted;
        #endregion

        #region Constructors
        public Solution(string t_ID, double t_cost)
        {
            m_ID = t_ID;
            m_cost = t_cost;
            m_mats = new SolutionMaterials();
            m_ous = new SolutionOpUnits();
            m_inserted = false;
        }
        #endregion

        #region Member functions
        public void AddMaterial(MaterialProperties t_matprop, double t_amount)
        {
            m_mats.Add(new SolutionMaterial(t_matprop, t_amount));
        }

        public SolutionMaterial FindMaterial(string name)
        {
            foreach (SolutionMaterial smat in m_mats) if (smat.MatProp.name == name) return smat;
            return null;
        }

        public void AddOpUnit(OperatingUnitProperties t_ouprop, double t_size)
        {
            m_ous.Add(new SolutionOpUnit(t_ouprop, t_size));
        }

        public SolutionOpUnit FindOpUnit(string name)
        {
            foreach (SolutionOpUnit sou in m_ous) if (sou.OUProp.name == name) return sou;
            return null;
        }

        public string GenerateResult()
        {
            double t_cost = 0;
            string t_ret = Solver_keys._KEY_MATERIALS + "\n";
            foreach (SolutionMaterial item in m_mats) t_cost += item.GenerateResult(ref t_ret);
            t_ret += Solver_keys._KEY_OPERATING_UNITS + "\n";
            foreach (SolutionOpUnit item in m_ous) t_cost += item.GenerateResult(ref t_ret);
            t_ret += "\n" + def_Solutions.text_total_cost + ": " + t_cost + " " + DefaultMUsAndValues.MUs.DefaultCostMU;
            return t_ret;
        }

        public TreeNode GenerateTreeResult(bool t_check_result)
        {
            TreeNode t_result = new TreeNode(ToString());
            double t_cost = 0;
            TreeNode t_node = t_result.Nodes.Add(Solver_keys._KEY_MATERIALS);
            foreach (SolutionMaterial item in m_mats) t_cost += item.GenerateTreeResult(t_node);
            t_node = t_result.Nodes.Add(Solver_keys._KEY_OPERATING_UNITS);
            foreach (SolutionOpUnit item in m_ous) t_cost += item.GenerateTreeResult(t_node);
            if (t_check_result && !Converters.IsDoubleEqual(t_cost, m_cost, def_Values.d_solution_tolerance))
            {
                MessageBox.Show(def_Solutions.Msg_solution_tolerance_error_p1 + m_cost + def_Solutions.Msg_solution_tolerance_error_p2 +
                    t_cost + def_Solutions.Msg_solution_tolerance_error_p3);
            }
            return t_result;
        }

        public override string ToString()
        {
            return def_Solutions.text_solution + " [" + (m_ID) + "]: " + def_Solutions.text_total_cost +
                ": " + m_cost + " " + DefaultMUsAndValues.MUs.DefaultCostMU;
        }
        #endregion

        #region Properties
        public string ID { get { return "[" + m_ID + "]"; } }
        public SolutionMaterials Mats { get { return m_mats; } }
        public SolutionOpUnits OpUnits { get { return m_ous; } }
        public double Cost { get { return m_cost; } }
        public bool Inserted { get { return m_inserted; } set { m_inserted = value; } }
        #endregion
    }
    public class Solutions : List<Solution> { }
}
