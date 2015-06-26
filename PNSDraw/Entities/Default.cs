using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNSDraw
{
    class Default
    {
        #region Enums
        public enum MassUnit
        {
            g,
            kg,
            t
        }

        public enum VolumeUnit
        {
            m3,
            dm3,
            cm3
        }

        public enum SubstanceUnit
        {
            mol,
            mmol,
            kmol
        }

        public enum EnergyUnit
        {
            J,
            kJ,
            MJ,
            GJ,
            TJ,
            Wh,
            kWh,
            MWh,
            GWh,
            TWh
        }

        public enum LengthUnit
        {
            m,
            km,
            dm,
            cm,
            mm,
            coll
        }

        public enum CurrentUnit
        {
            A,
            mA,
            kA
        }

        public enum AreaUnit
        {
            m2,
            km2,
            cm2,
            ha
        }

        public enum SpeedUnit
        {
            mps,
            kmph,
            Mph
        }

        public enum AccelerationUnit
        {
            mps2
        }

        public enum MassDensityUnit
        {
            kgpm3,
            tpm3
        }

        public enum ThermoTempUnit
        {
            K
        }

        public enum LuminIntensUnit
        {
            cd
        }

        public enum ConcentrationUnit
        {
            molpm3,
            molpdm3
        }

        public enum ForceUnit
        {
            N
        }

        public enum PressureUnit
        {
            Pa,
            kPa,
            MPa
        }

        public enum PowerUnit
        {
            W,
            kW,
            MW,
            GW,
            TW
        }

        public enum CapacityUnit
        {
            unit
        }

        public enum TimeUnit
        {
            s,
            h,
            d,
            w,
            m,
            y
        }

        public enum MoneyUnit
        {
            EUR,
            HUF,
            USD
        }
        #endregion

        #region QuantityTypes
        static public Dictionary<string, string[]> quantities = new Dictionary<string, string[]>
        {
            {"Mass", new string[] { "gram", "kilogram", "ton" }},
            {"Volume", new string[] { "cubic meter", "cubic decimeter", "cubic centimeter" }},
            {"Amount of substance", new string[] { "mole", "millimole", "kilomole" }},
            {"Energy, work, heat", new string[] { "joule", "kilojoule", "megajoule", "gigajoule", "terajoule", "watthour", "kilowatthour", "megawatthour", "gigawatthour", "terawatthour" }},
            {"Length", new string[] { "meter", "kilometer", "decimeter", "centimeter", "millimeter", "coll" }},
            {"Electric current", new string[] { "ampere", "milliampere", "kiloampere" }},
            {"Area", new string[] { "square meter", "square kilometer", "square centimeter", "hectar" }},
            {"Speed", new string[] { "meters per second", "kilometers per hour", "miles per hour" }},
            {"Acceleration", new string[] { "meter per second squared" }},
            {"Mass density", new string[] { "kilogram per cubic meter", "ton per cubic meter" }},
            {"Thermodinamic temperature", new string[] { "kelvin" }},
            {"Luminous intensity", new string[] { "candela" }},
            {"Concentration", new string[] { "mole per cubic meter", "mole per cubic decimeter" }},
            {"Force", new string[] { "newton" }},
            {"Pressure", new string[] { "pascal", "kilopascal", "megapascal" }},
            {"Power", new string[] { "watt", "kilowatt", "megawatt", "gigawatt", "terawatt" }},
            {"Capacity", new string[] { "unit" }}
        };
        #endregion

        #region Values
        static public double flow_rate_lower_bound = 0; //flow_rate_lower_bound
        static public double flow_rate_upper_bound = 1000000000; //flow_rate_upper_bound
        static public double price = 0; //price
        static public int type = Globals.MaterialTypes.Intermediate; //type

        static public string tempFolder = Path.GetTempPath(); //temporary folder

        static public int limit = 10;
        static public int processes = 1;
        static public bool online = false;

        static public string host = "193.6.33.141";
        static public int port = 51000;

        static public double o_fix = 0; //fix_cost
        static public double i_fix = 0; //fix_cost

        public static double fix_cost = o_fix + i_fix;

        static public double o_prop = 0; //proportional_cost
        static public double i_prop = 0; //proportional_cost

        public static double prop_cost = o_prop + i_prop;

        static public double capacity_lower_bound = 0; //capacity_lower_bound
        static public double capacity_upper_bound = 1000000000; //capacity_upper_bound

        static public double io_flowrate = 1; //flow rate

        static public string mass_mu = quantities["Mass"][2];
        static public string vol_mu = quantities["Volume"][0];
        static public string sub_mu = quantities["Amount of substance"][0];
        static public string energy_mu = quantities["Energy, work, heat"][0];
        static public string length_mu = quantities["Length"][0];
        static public string curr_mu = quantities["Electric current"][0];
        static public string area_mu = quantities["Area"][0];
        static public string speed_mu = quantities["Speed"][0];
        static public string acc_mu = quantities["Acceleration"][0];
        static public string mdens_mu = quantities["Mass density"][1];
        static public string temp_mu = quantities["Thermodinamic temperature"][0];
        static public string lum_mu = quantities["Luminous intensity"][0];
        static public string conc_mu = quantities["Concentration"][0];
        static public string force_mu = quantities["Force"][0];
        static public string press_mu = quantities["Pressure"][0];
        static public string power_mu = quantities["Power"][0];
        static public string cap_mu = quantities["Capacity"][0];
        static public TimeUnit time_mu = TimeUnit.y;
        static public MoneyUnit money_mu = MoneyUnit.EUR;

        public const double d_NperA = -1;
        static public double payout_period = 10;
        static public int working_hours_per_year = 8000;

        static public double d_tolerance = 0.00000001;
        static public double d_solution_tolerance = 0.0001;
        static public int max_solutions = 10;
        static public bool b_auto_convert = false;
        #endregion

        public Default() { }
    }
}
