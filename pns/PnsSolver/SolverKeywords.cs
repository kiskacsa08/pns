using System;
using System.Collections.Generic;
using System.Text;

namespace Pns.PnsSolver
{
    static public class Solver_keys
    {
        #region Members
        static public string solver = "pns_depth.exe";
        //static public string solver = "pns_solver.exe";
        //static public string solver = "GUItest.exe";
        static public string _SOLVER_INPUT_EXTENSION = ".in";
        static public string _SOLVER_ROW_OUTPUT_EXTENSION = ".out";
        static public string _NEWLINE = "\r\n";
        static public string _FILE_TYPE = "file_type=PNS_problem_v1";
        static public string _FILE_NAME = "file_name=";
        static public string _MEASUREMENT_UNITS = "measurement_units:";
        static public string _MASS_UNIT = "mass_unit=";
        static public string _TIME_UNIT = "time_unit=";
        static public string _MONEY_UNIT = "money_unit=";
        static public string _DEFAULTS = "defaults:";
        static public string _MATERIAL_TYPE = "material_type=";
        static public string _INTERMEDIATE = "intermediate";
        static public string _DEF_MAT_LB = "material_flow_rate_lower_bound=";
        static public string _DEF_MAT_UB = "material_flow_rate_upper_bound=";
        static public string _DEF_MAT_PRICE = "material_price=";
        static public string _DEF_OPUNIT_LB = "operating_unit_capacity_lower_bound=";
        static public string _DEF_OPUNIT_UB = "operating_unit_capacity_upper_bound=";
        static public string _DEF_OPUNIT_FIX = "operating_unit_fix_cost=";
        static public string _DEF_OPUNIT_PROP = "operating_unit_proportional_cost=";
        static public string _MATERIALS = "materials:";
        static public string _RAW = "raw_material";
        static public string _PRODUCT = "product";
        static public string _MAT_LB = "flow_rate_lower_bound=";
        static public string _MAT_UB = "flow_rate_upper_bound=";
        static public string _MAT_PRICE = "price=";
        static public string _OPUNITS = "operating_units:";
        static public string _OPUNIT_LB = "capacity_lower_bound=";
        static public string _OPUNIT_UB = "capacity_upper_bound=";
        static public string _OPUNIT_FIX = "fix_cost=";
        static public string _OPUNIT_PROP = "proportional_cost=";
        static public string _OPUNIT_RATES = "material_to_operating_unit_flow_rates:";
        static public string _KEY_FEASIBLE_STRUCTURE = "Feasible structure ";
        static public string _KEY_CURRENT_BEST_STRUCTURE = "Current best structure:";
        static public string _KEY_MATERIALS = "Materials:";
        static public string _KEY_OPERATING_UNITS = "Operating units:";
        static public string _KEY_TOTAL_ANNUAL_COST = "Total annual cost";
        static public string _KEY_MSG = "MSG";
        static public string _KEY_SSG = "SSG";
        static public string _KEY_SSGLP = "SSGLP";
        static public string _KEY_ABB = "INSIDEOUT";
        static public int _SOLVER_ROW_OUTPUT_MAX_DISPLAYED_SIZE = 1000000;
        #endregion
    }
}
