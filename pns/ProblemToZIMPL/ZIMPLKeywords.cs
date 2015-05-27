﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pns.ProblemToZIMPL
{
    static public class ZIMPL_keys
    {
        #region Members
        static public string _EXTENSION = ".zpl";
        static public int _N = 5;
        static public string _NEWLINE = "\r\n";
        static public string _TAB = "   ";
        
        static public string _REM_RAWS = "# Set of raw materials";
        static public string _RAWS = "raw_materials";
        static public string _REM_INTERMEDIATES = "# Set of intermediates";
        static public string _INTERMEDIATES = "intermediates";
        static public string _REM_PRODUCTS = "# Set of required or potential products";
        static public string _PRODUCTS = "products";
        static public string _REM_MATERIALS = "# Set of all materials";
        static public string _MATERIALS = "materials";
        static public string _REM_MAT_LB = "# Lower bounds of materials flow rates";
        static public string _MAT_LB = "flow_rate_lower_bound";
        static public string _REM_MAT_UB = "#  Upper bounds of materials flow rates";
        static public string _MAT_UB = "flow_rate_upper_bound";
        static public string _REM_PRICE = "# Prices of raw materials and products";
        static public string _PRICE = "price";

        static public string _REM_OPUNITS = "# Set of operating units";
        static public string _OPUNITS = "operating_units";
        static public string _REM_OPUNIT_LB = "# Lower bounds of operating units capacities";
        static public string _OPUNIT_LB = "capacity_lower_bound";
        static public string _REM_OPUNIT_UB = "# Upper bounds of operating units capacities";
        static public string _OPUNIT_UB = "capacity_upper_bound";
        static public string _REM_OPUNIT_FIX = "# Fix costs of operating units";
        static public string _OPUNIT_FIX = "fix_cost";
        static public string _REM_OPUNIT_PROP = "# Proportional costs of operating units";
        static public string _OPUNIT_PROP = "proportional_cost";
        static public string _REM_OPUNIT_RATES = "# Input and output flow rates of materials to operating units";
        static public string _OPUNIT_RATES = "material_to_operating_unit_flow_rates";
        #endregion
    }
}
