using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Pns.Xml_Serialization.PnsGUI.MessageBoxes;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;

namespace Pns.Globals
{
    static public class Converters
    {
        #region Member functions
        static public bool IsDoubleEqual(double t_num1, double t_num2)
        {
            return IsDoubleEqual(t_num1, t_num2, def_Values.d_tolerance);
        }
        static public bool IsDoubleEqual(double t_num1, double t_num2, double t_tolerance)
        {
            if (t_num2 != 0)
            {
                double t_res = t_num1 / t_num2;
                return t_res > (1 - t_tolerance) && t_res < (1 + t_tolerance);
            }
            return t_num1 > -t_tolerance && t_num1 < t_tolerance;
        }
        static public double ToDouble(double lb, string str)
        {
            if (str == "") return def_Values.d_NperA;
            double value = ToDouble(str);
            if (value < lb)
            {
                MessageBox.Show(def_Convert_msg.Msg_lb + lb);
                value = lb;
            }
            return value;
        }
        static public double ToDouble(string str, double ub)
        {
            if (str == "") return def_Values.d_NperA;
            double value = ToDouble(str);
            if (ub > DefaultMUsAndValues.DefaultValues.d_solver_ub) ub = DefaultMUsAndValues.DefaultValues.d_solver_ub;
            if (value > ub)
            {
                if (ub == DefaultMUsAndValues.DefaultValues.d_solver_ub) MessageBox.Show(def_Convert_msg.Msg_solver_ub);
                else MessageBox.Show(def_Convert_msg.Msg_ub + ub);
                value = ub;
            }
            return value;
        }
        static public double ToDouble(double lb, string str, double ub)
        {
            if (str == "") return def_Values.d_NperA;
            double value = ToDouble(str);
            if (value < lb)
            {
                MessageBox.Show(def_Convert_msg.Msg_lb + lb);
                value = lb;
            }
            else
            {
                if (value > ub)
                {
                    if (ub == DefaultMUsAndValues.DefaultValues.d_solver_ub) MessageBox.Show(def_Convert_msg.Msg_solver_ub);
                    else MessageBox.Show(def_Convert_msg.Msg_ub + ub);
                    value = ub;
                }
            }
            return value;
        }
        static public double ToDouble(string str)
        {
            int t_len = str.Length + 1;
            double value = t_len > 1 ? 0 : def_Values.d_NperA;
            while (--t_len > 0 && !double.TryParse(str.Substring(0, t_len), out value)) ;
            return value;
        }
        static public string ToNameString(string str)
        {
            for (int i = 0; i < str.Length; i++) if (defaults.NameCharSet.IndexOf(str[i]) == -1) str = str.Replace(str[i], '_');
            if (str == "") str = def_PnsStudio.def_no_name;
            if (str[0] >= '0' && str[0] <= '9') str = str.Insert(0, "_");
            return str;
        }
        #endregion
    }
}
