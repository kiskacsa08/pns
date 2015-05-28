﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNSDraw
{
    class Default
    {
        static public double flow_rate_lower_bound = 0; //flow_rate_lower_bound
        static public double flow_rate_upper_bound = 100000000; //flow_rate_upper_bound
        static public double price = 0; //price
        static public int type { get { return Globals.MaterialTypes.Raw; } } //type

        static private double o_fix = 0; //fix_cost
        static private double i_fix = 0; //fix_cost

        static public double fix_cost { get { return o_fix + i_fix; } }

        static private double o_prop = 0; //proportional_cost
        static private double i_prop = 0; //proportional_cost

        static public double prop_cost { get { return o_prop + i_prop; } }

        static public double capacity_lower_bound = 0; //capacity_lower_bound
        static public double capacity_upper_bound = 100000000; //capacity_upper_bound

        static public double io_flowrate = 1; //flow rate

        public const double d_NperA = -1;
        static public double payout_period = 10;
        static public int worging_hours_per_year = 8000;

        static public double d_tolerance = 0.00000001;
        static public double d_solution_tolerance = 0.0001;
        static public int max_solutions = 10;
        static public bool b_auto_convert = false;

        public Default() { }
    }
}