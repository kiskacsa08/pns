using System;
using System.Collections.Generic;
using System.Text;

namespace Pns.Globals
{
    static public class def_Values
    {
        #region Members
        public const double d_NperA = -1;
        static public double payout_period = 10;
        static public int worging_hours_per_year = 8000;
        static public int default_material_quantity_type_id { get { return (int)defaults.MU_Groups.d_mass; } }
        static public double minimum_flow = 0;
        static public double price = 0;
        static public double io_flowrate = 1;
        static public double o_fix = 0;
        static public double o_prop = 0;
        static public double i_fix = 0;
        static public double i_prop = 0;
        static public double lower_bound = 0;
        static public double solver_upper_limit = 100000000;
        static public double d_tolerance = 0.00000001;
        static public double d_solution_tolerance = 0.0001;
        static public int max_solutions = 10;
        static public bool b_auto_convert = false;
        #endregion
    }
    static public class defaults
    {
        #region Enums
        public enum MatTypes { raw, intermediate, product, none }
        public enum MatPropButtons { update, cancel, delete }
        public enum OUPropButtons { update, cancel, delete }
        public enum CostTypes { investment, operating, overall }
        public enum ChangeCategoryChoise { min_mu, max_mu, def_mu, action_needed, keep_current, default_flowrate }
        public enum TreeNodeTypes { none, materials, mat_type, material, new_material, opunits, opunit, new_opunit, io_flows, io_flow }
        public enum SolutionToExcel { brief_export, brief_view, detailed_export, detailed_view, export_summary_of_results, view_summary_of_results }
        #endregion

        #region MU enums
        public enum MU_Base_Quantities
        {
            b_undefined, b_lenght, b_mass, b_time, b_electric_current, b_thermodynamic_temperature,
            b_amount_of_substance, b_luminous_intensity, b_currency, b_capacity
        }
        public enum MU_Groups
        {
            undefined, base_group, d_lenght, d_mass, d_time, d_electric_current, d_thermodynamic_temperature,
            d_amount_of_substance, d_luminous_intensity, d_currency, d_area, d_volume, d_speed, d_acceleration, d_density,
            d_concentration, d_force, d_pressure, d_energy, d_power, d_capacity
        }
        public enum MU_Groups_Length { undefined, kilometer, decimeter, centimeter, millimeter, coll }
        public enum MU_Groups_Mass { undefined, tonne, gram }
        public enum MU_Groups_Time { undefined, minute, hour, year, payout_period }
        public enum MU_Groups_Electric_current { undefined, milliamper, kiloamper }
        public enum MU_Groups_Therodynamic_temperature { undefined }
        public enum MU_Groups_Amount_of_substance { undefined, millimole, kilomole }
        public enum MU_Groups_Luminous_intensity { undefined }
        public enum MU_Groups_Currency { undefined, euro, dollar }
        public enum MU_Groups_Area { undefined, square_meter, square_kilometer, square_centimeter, hectar }
        public enum MU_Groups_Volume { undefined, cubic_meter, cubic_decimeter, cubic_centimeter }
        public enum MU_Groups_Speed { undefined, meter_per_second, kilometer_per_hour, miles_per_hour }
        public enum MU_Groups_Acceleration { undefined, meter_per_second_squared }
        public enum MU_Groups_Mass_density { undefined, kilogram_per_cubic_meter, tonne_per_cubic_meter }
        public enum MU_Groups_Concentration { undefined, mole_per_cubic_meter, mole_per_cubic_decimeter }
        public enum MU_Groups_Force { undefined, newton }
        public enum MU_Groups_Pressure { undefined, pascal, kilopascal, megapascal }
        public enum MU_Groups_Energy
        {
            undefined, joule, kilojoule, megajoule, gigajoule, terajoule, watthour,
            kilowatthour, megawatthour, gigawatthour, terawatthour
        }
        public enum MU_Groups_Power { undefined, watt, kilowatt, megawatt, gigawatt, terawatt }
        public enum MU_Groups_Capacity { undefined, capacity }
        #endregion

        #region Members
        static public int MaxRecentFiles = 8;
        static public string Defaults_file = "Defaults.xml";
        static public string Texts_file = "PnsUserInterfaceTexts.xml";
        static public string NameCharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_[]";
        #endregion
    }
    static public class def_PnsStudio_ex
    {
        #region Members
        static public string Ex_PnsEditor_is_null = "PnsEditor object is null!";
        static public string Ex_material_not_found = "Material not found!";
        static public string Ex_opunit_not_found = "Operating unit not found!";
        static public string Ex_ou_node_not_found = "Operating unit tree structural error.";
        static public string Ex_ioflows_node_not_found = "Operating unit tree structural error.";
        static public string Ex_iomaterial_type_mismatch = "Operating unit Input/Output material type mismatch";
        #endregion
    }
    static public class def_MU_ex
    {
        #region Members
        static public string Ex_category_not_found = "Measurement unit category not found";
        static public string Ex_derived_category_not_found = "Derived category not found";
        static public string Ex_unit_not_found = "Measurement unit not found";
        static public string Ex_FMU_not_found = "Fraction MU not found";
        static public string Ex_MU_not_found = "Measurement unit not found.";
        static public string Ex_derived_group_not_found = "Derived quantity group not found";
        static public string Ex_derived_unit_not_found = "Derived unit not found";
        static public string Ex_base_quantity_not_found = "Base quantity not found";
        static public string Ex_Defaults_is_null = "Defaults object is null";
        #endregion
    }
}
