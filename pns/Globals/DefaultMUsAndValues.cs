using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using Pns.Xml_Serialization.PnsDefaults;
using Pns.Xml_Serialization.PnsProblem;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;
using Pns.Globals;
using Pns.Dialogs;

namespace Pns
{
    public class DefaultMUsAndValues
    {
        #region Members
        static public DefaultMUsAndValues s_defaults = null;
        private MU_XML f_mudb;
        private MeasurementUnits f_mus;
        private DefaultValuesProperties f_def_values;
        #endregion

        #region Constructors
        public DefaultMUsAndValues(Problem problem)
        {
            s_defaults = this;
            if (problem != null)
            {
                if (problem.MeasurementUnitsDatabase != null)
                {
                    f_mudb = problem.MeasurementUnitsDatabase;
                    f_mus = new MeasurementUnits(problem);
                    f_def_values = new DefaultValuesProperties(problem);
                }
                else DefaultsFromFile();
                if (problem.workingHoursPerYear != (int)def_Values.d_NperA) f_mus.DefaultWorkingHoursPerYear = problem.workingHoursPerYear;
                if (problem.payoutPeriod != def_Values.d_NperA) f_mus.DefaultPayoutPeriod = problem.payoutPeriod;
                if (problem.maxSolutions != def_Values.d_NperA) f_def_values.i_max_solutions = problem.maxSolutions;
            }
            else DefaultsFromFile();
        }
        #endregion

        #region Member functions
        private void DefaultsFromFile()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XMLDefaults));
                FileStream fs = new FileStream(Application.StartupPath + "\\" + defaults.Defaults_file, FileMode.Open);
                XMLDefaults t_defdb = (XMLDefaults)serializer.Deserialize(fs);
                fs.Close();
                f_mudb = t_defdb.MeasurementUnitsDatabase;
                f_mus = new MeasurementUnits(t_defdb);
                f_def_values = new DefaultValuesProperties(t_defdb);
            }
            catch
            {
                DefaultsToFile(true);
                MessageBox.Show(def_PnsStudio.Msg_mu_file_missing, def_PnsStudio.Msg_mu_file_missing_title);
            }
        }
        private void DefaultsToFile() { DefaultsToFile(false); }
        private void DefaultsToFile(bool t_create_new)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XMLDefaults));
            FileStream fs = new FileStream(Application.StartupPath + "\\" + defaults.Defaults_file, FileMode.Create);
            serializer.Serialize(fs, new XMLDefaults(t_create_new));
            fs.Close();
        }
        static public string GetSymbol(int category_id, int id) { return MUDB.GetSymbol(category_id, id); }
        static public string GetCategory(int category_id) { return MUDB.GetCategory(category_id); }
        static public void GetCategories(MUCategories categories) { MUDB.GetCategories(categories); }
        static public void GetMUs(int t_category_id, MUs t_mus) { MUDB.GetMUs(t_category_id, t_mus); }
        static public double ConvertMU(MU from, MU to, int working_hours_per_year, double payout_period) { return MUDB.convert(from, to, working_hours_per_year, payout_period); }
        static public double ConvertMU(FractionMU from, FractionMU to, int working_hours_per_year, double payout_period) { return MUDB.convert(from, to, working_hours_per_year, payout_period); }
        static public void SaveDefaults()
        {
            if (s_defaults == null) throw new Exception(def_MU_ex.Ex_Defaults_is_null);
            s_defaults.DefaultsToFile();
        }
        static public void ToXML(Problem problem)
        {
            problem.MeasurementUnitsDatabase = MUDB;
            MUs.ToXML(problem);
            DefaultValues.ToXML(problem);
        }
        #endregion

        #region Properties
        static public MU_XML MUDB
        {
            get
            {
                if (s_defaults != null) return s_defaults.f_mudb;
                throw new Exception(def_MU_ex.Ex_Defaults_is_null);
            }
            set
            {
                if (s_defaults != null) s_defaults.f_mudb = value;
                else throw new Exception(def_MU_ex.Ex_Defaults_is_null);
            }
        }
        static public MeasurementUnits MUs
        {
            get
            {
                if (s_defaults != null) return s_defaults.f_mus;
                throw new Exception(def_MU_ex.Ex_Defaults_is_null);
            }
            set
            {
                if (s_defaults != null) s_defaults.f_mus = value;
                else throw new Exception(def_MU_ex.Ex_Defaults_is_null);
            }
        }
        static public DefaultValuesProperties DefaultValues
        {
            get
            {
                if (s_defaults != null) return s_defaults.f_def_values;
                throw new Exception(def_MU_ex.Ex_Defaults_is_null);
            }
            set
            {
                if (s_defaults != null) s_defaults.f_def_values = value;
                else throw new Exception(def_MU_ex.Ex_Defaults_is_null);
            }
        }
        #endregion
    }
}
