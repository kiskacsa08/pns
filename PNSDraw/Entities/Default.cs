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

        static public Dictionary<string, Dictionary<string, string>> quantities = new Dictionary<string, Dictionary<string, string>>
        {
            {"Mass", new Dictionary<string, string> { {"gram","g"}, {"kilogram","kg"}, {"ton","t"} }},
            {"Volume", new Dictionary<string, string> { {"cubic meter","m³"}, {"cubic decimeter","dm³"}, {"cubic centimeter","cm³"} }},
            {"Amount of substance", new Dictionary<string, string> { {"mole","mol"}, {"millimole","mmol"}, {"kilomole","kmol"} }},
            {"Energy, work, heat", new Dictionary<string, string> { {"joule","J"}, {"kilojoule","kJ"}, {"megajoule","MJ"}, {"gigajoule","GJ"}, {"terajoule","TJ"}, {"watthour","Wh"}, {"kilowatthour","kWh"}, {"megawatthour","MWh"}, {"gigawatthour","GWh"}, {"terawatthour","TWh"} }},
            {"Length", new Dictionary<string, string> { {"meter","m"}, {"kilometer","km"}, {"decimeter","dm"}, {"centimeter","cm"}, {"millimeter","mm"}, {"coll","\""} }},
            {"Electric current", new Dictionary<string, string> { {"ampere","A"}, {"milliampere","mA"}, {"kiloampere","kA"} }},
            {"Area", new Dictionary<string, string> { {"square meter","m²"}, {"square kilometer","km²"}, {"square centimeter","cm²"}, {"hectar","ha"} }},
            {"Speed", new Dictionary<string, string> { {"meters per second","m/s"}, {"kilometers per hour","km/h"}, {"miles per hour","mph"} }},
            {"Acceleration", new Dictionary<string, string> { {"meter per second squared","m/s²"} }},
            {"Mass density", new Dictionary<string, string> { {"kilogram per cubic meter","kg/m³"}, {"ton per cubic meter","t/m³"} }},
            {"Thermodinamic temperature", new Dictionary<string, string> { {"kelvin","K"} }},
            {"Luminous intensity", new Dictionary<string, string> { {"candela","cd"} }},
            {"Concentration", new Dictionary<string, string> { {"mole per cubic meter","mol/m³"}, {"mole per cubic decimeter","mol/dm³"} }},
            {"Force", new Dictionary<string, string> { {"newton","N"} }},
            {"Pressure", new Dictionary<string, string> { {"pascal","Pa"}, {"kilopascal","kPa"}, {"megapascal","MPa"} }},
            {"Power", new Dictionary<string, string> { {"watt","W"}, {"kilowatt","kW"}, {"megawatt","MW"}, {"gigawatt","GW"}, {"terawatt","TW"} }},
            {"Capacity", new Dictionary<string, string> { {"unit","u"} }}
        };

        #region QuantityTypes
        //static public Dictionary<string, string[]> quantities = new Dictionary<string, string[]>
        //{
        //    {"Mass", new string[] { "gram", "kilogram", "ton" }},
        //    {"Volume", new string[] { "cubic meter", "cubic decimeter", "cubic centimeter" }},
        //    {"Amount of substance", new string[] { "mole", "millimole", "kilomole" }},
        //    {"Energy, work, heat", new string[] { "joule", "kilojoule", "megajoule", "gigajoule", "terajoule", "watthour", "kilowatthour", "megawatthour", "gigawatthour", "terawatthour" }},
        //    {"Length", new string[] { "meter", "kilometer", "decimeter", "centimeter", "millimeter", "coll" }},
        //    {"Electric current", new string[] { "ampere", "milliampere", "kiloampere" }},
        //    {"Area", new string[] { "square meter", "square kilometer", "square centimeter", "hectar" }},
        //    {"Speed", new string[] { "meters per second", "kilometers per hour", "miles per hour" }},
        //    {"Acceleration", new string[] { "meter per second squared" }},
        //    {"Mass density", new string[] { "kilogram per cubic meter", "ton per cubic meter" }},
        //    {"Thermodinamic temperature", new string[] { "kelvin" }},
        //    {"Luminous intensity", new string[] { "candela" }},
        //    {"Concentration", new string[] { "mole per cubic meter", "mole per cubic decimeter" }},
        //    {"Force", new string[] { "newton" }},
        //    {"Pressure", new string[] { "pascal", "kilopascal", "megapascal" }},
        //    {"Power", new string[] { "watt", "kilowatt", "megawatt", "gigawatt", "terawatt" }},
        //    {"Capacity", new string[] { "unit" }}
        //};
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

        static public string quant_type = "Mass";

        static public string mass_mu = quantities["Mass"]["ton"];
        static public string vol_mu = quantities["Volume"]["cubic meter"];
        static public string sub_mu = quantities["Amount of substance"]["mole"];
        static public string energy_mu = quantities["Energy, work, heat"]["joule"];
        static public string length_mu = quantities["Length"]["meter"];
        static public string curr_mu = quantities["Electric current"]["ampere"];
        static public string area_mu = quantities["Area"]["square meter"];
        static public string speed_mu = quantities["Speed"]["meters per second"];
        static public string acc_mu = quantities["Acceleration"]["meter per second squared"];
        static public string mdens_mu = quantities["Mass density"]["ton per cubic meter"];
        static public string temp_mu = quantities["Thermodinamic temperature"]["kelvin"];
        static public string lum_mu = quantities["Luminous intensity"]["candela"];
        static public string conc_mu = quantities["Concentration"]["mole per cubic meter"];
        static public string force_mu = quantities["Force"]["newton"];
        static public string press_mu = quantities["Pressure"]["pascal"];
        static public string power_mu = quantities["Power"]["watt"];
        static public string cap_mu = quantities["Capacity"]["unit"];
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

        public static string GetMUName(string quantityType, string symbol)
        {
            foreach (KeyValuePair<string, string> MU in quantities[quantityType])
            {
                if (MU.Value.Equals(symbol))
                {
                    return MU.Key;
                }
            }
            return "";
        }

        public static string GetMUSymbol(string quantityType, string name)
        {
            foreach (KeyValuePair<string, string> MU in quantities[quantityType])
            {
                if (MU.Key.Equals(name))
                {
                    return MU.Value;
                }
            }
            return "";
        }

        public static string GetQuantityTypeByName(string name)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> quantityType in quantities)
            {
                foreach (KeyValuePair<string, string> MU in quantityType.Value)
                {
                    if (MU.Key.Equals(name))
                    {
                        return quantityType.Key;
                    }
                }
            }
            return "";
        }

        public static string GetQuantityTypeBySymbol(string symbol)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> quantityType in quantities)
            {
                foreach (KeyValuePair<string, string> MU in quantityType.Value)
                {
                    if (MU.Value.Equals(symbol))
                    {
                        return quantityType.Key;
                    }
                }
            }
            return "";
        }

        public static string GetDefaultMUSymbol(string quantityType)
        {
            string MU;
            switch (quantityType)
            {
                case "Mass":
                    MU = mass_mu;
                    break;
                case "Volume":
                    MU = vol_mu;
                    break;
                case "Amount of substance":
                    MU = sub_mu;
                    break;
                case "Energy, work, heat":
                    MU = energy_mu;
                    break;
                case "Length":
                    MU = length_mu;
                    break;
                case "Electric current":
                    MU = curr_mu;
                    break;
                case "Area":
                    MU = area_mu;
                    break;
                case "Speed":
                    MU = speed_mu;
                    break;
                case "Acceleration":
                    MU = acc_mu;
                    break;
                case "Mass density":
                    MU = mdens_mu;
                    break;
                case "Thermodinamic temperature":
                    MU = temp_mu;
                    break;
                case "Luminous intensity":
                    MU = lum_mu;
                    break;
                case "Concentration":
                    MU = conc_mu;
                    break;
                case "Force":
                    MU = force_mu;
                    break;
                case "Pressure":
                    MU = press_mu;
                    break;
                case "Power":
                    MU = power_mu;
                    break;
                case "Capacity":
                    MU = cap_mu;
                    break;
                default:
                    MU = "";
                    break;
            }
            return MU;
        }

        public static string GetDefaultMUName(string quantityType)
        {
            string MU = "";
            switch (quantityType)
            {
                case "Mass":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(mass_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Volume":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(vol_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Amount of substance":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(sub_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Energy, work, heat":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(energy_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Length":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(length_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Electric current":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(curr_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Area":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(area_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Speed":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(speed_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Acceleration":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(acc_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Mass density":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(mdens_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Thermodinamic temperature":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(temp_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Luminous intensity":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(lum_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Concentration":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(conc_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Force":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(force_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Pressure":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(press_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Power":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(power_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Capacity":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(cap_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                default:
                    MU = "";
                    break;
            }
            return MU;
        }

        public static List<string> GetListOfMUs(string quantityType)
        {
            List<string> MUList = new List<string>();
            foreach (KeyValuePair<string, string> MU in quantities[quantityType])
            {
                string text = MU.Key + " (" + MU.Value + ")";
                MUList.Add(text);
            }

            return MUList;
        }

        public static string GetMUSymbolFromText(string text)
        {
            int begin = text.IndexOf("(") + 1;
            int end = text.IndexOf(")");
            return text.Substring(begin, end - begin);
        }

        public static string GetMUNameFromText(string text)
        {
            int begin = 0;
            int end = text.IndexOf(" (");
            return text.Substring(begin, end - begin);
        }

        public static string GetTextFromSymbol(string symbol)
        {
            string quantity = GetQuantityTypeBySymbol(symbol);
            string name = GetMUName(quantity, symbol);
            return name + " (" + symbol + ")";
        }
    }
}
