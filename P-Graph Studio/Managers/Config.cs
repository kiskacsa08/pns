/* Copyright 2015 Department of Computer Science and Systems Technology, University of Pannonia

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. 
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PNSDraw.Configuration
{
    public static class MaterialTypes
    {
        public const int Raw = 0;
        public const int Intermediate = 1;
        public const int Product = 2;
    }

    [Serializable()]
    public class Login
    {
        public string Username = "";
        public string Email = "";
        public bool IsLoggedIn = false;
    }

    [Serializable()]
    public class Material
    {
        public double FlowRateLowerBound = 0;
        public double FlowRateUpperBound = 1000000000;
        public double Price = 0;
        public int Type = MaterialTypes.Intermediate;
    }

    [Serializable()]
    public class OperatingUnit
    {
        public double OperatingFixCost = 0;
        public double InvestmentFixCost = 0;

        public double OpUnitFixCost;

        public double OperatingPropCost = 0;
        public double InvestmentPropCost = 0;

        public double OpUnitPropCost;

        public double CapacityLowerBound = 0;
        public double CapacityUpperBound = 1000000000;

        public double PayoutPeriod = 10;
        public int WorkingHoursPerYear = 8000;

        public OperatingUnit()
        {
            if (WorkingHoursPerYear == 0 || PayoutPeriod == 0)
            {
                OpUnitFixCost = OperatingFixCost;
                OpUnitPropCost = OperatingPropCost;
            }
            else
            {
                OpUnitFixCost = OperatingFixCost + InvestmentFixCost / (WorkingHoursPerYear * PayoutPeriod);
                OpUnitPropCost = OperatingPropCost + InvestmentPropCost / (WorkingHoursPerYear * PayoutPeriod);
            }
        }
    }

    [Serializable()]
    public class Edge
    {
        public double FlowRate = 1;
    }

    [Serializable()]
    public class Mongo
    {
        public string Host = "193.6.19.25";
        public int Port = 27017;
        public string Database = "pgraph";
        public string Username = "";
        public string Password = "";
    }

    [Serializable()]
    public class SolverSettings
    {
        public int SolutionLimit = 10;
        public int NumberOfSolverProccess = 1;
        public string OfflineSolverTempFolder = Path.GetTempPath();
        public bool IsKeepTempFiles = false;
        public bool IsOnlineSolver = false;
        public string OnlineSolverHost = "pgraph.dcs.uni-pannon.hu";
        public int OnlineSolverPort = 51000;
    }

    [Serializable()]
    public class Quantity
    {
        #region QuantityTypes
        [XmlIgnore]
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
        #endregion

        public string quant_type = "Mass";

        public string mass_mu;
        public string vol_mu;
        public string sub_mu;
        public string energy_mu;
        public string length_mu;
        public string curr_mu;
        public string area_mu;
        public string speed_mu;
        public string acc_mu;
        public string mdens_mu;
        public string temp_mu;
        public string lum_mu;
        public string conc_mu;
        public string force_mu;
        public string press_mu;
        public string power_mu;
        public string cap_mu;
        public Config.TimeUnit time_mu = Config.TimeUnit.y;
        public Config.MoneyUnit money_mu = Config.MoneyUnit.EUR;

        public Quantity()
        {
            mass_mu = Quantity.quantities["Mass"]["ton"];
            vol_mu = Quantity.quantities["Volume"]["cubic meter"];
            sub_mu = Quantity.quantities["Amount of substance"]["mole"];
            energy_mu = Quantity.quantities["Energy, work, heat"]["joule"];
            length_mu = Quantity.quantities["Length"]["meter"];
            curr_mu = Quantity.quantities["Electric current"]["ampere"];
            area_mu = Quantity.quantities["Area"]["square meter"];
            speed_mu = Quantity.quantities["Speed"]["meters per second"];
            acc_mu = Quantity.quantities["Acceleration"]["meter per second squared"];
            mdens_mu = Quantity.quantities["Mass density"]["ton per cubic meter"];
            temp_mu = Quantity.quantities["Thermodinamic temperature"]["kelvin"];
            lum_mu = Quantity.quantities["Luminous intensity"]["candela"];
            conc_mu = Quantity.quantities["Concentration"]["mole per cubic meter"];
            force_mu = Quantity.quantities["Force"]["newton"];
            press_mu = Quantity.quantities["Pressure"]["pascal"];
            power_mu = Quantity.quantities["Power"]["watt"];
            cap_mu = Quantity.quantities["Capacity"]["unit"];
        }

        #region Getter functions

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
                    MU = Config.Instance.Quantity.mass_mu;
                    break;
                case "Volume":
                    MU = Config.Instance.Quantity.vol_mu;
                    break;
                case "Amount of substance":
                    MU = Config.Instance.Quantity.sub_mu;
                    break;
                case "Energy, work, heat":
                    MU = Config.Instance.Quantity.energy_mu;
                    break;
                case "Length":
                    MU = Config.Instance.Quantity.length_mu;
                    break;
                case "Electric current":
                    MU = Config.Instance.Quantity.curr_mu;
                    break;
                case "Area":
                    MU = Config.Instance.Quantity.area_mu;
                    break;
                case "Speed":
                    MU = Config.Instance.Quantity.speed_mu;
                    break;
                case "Acceleration":
                    MU = Config.Instance.Quantity.acc_mu;
                    break;
                case "Mass density":
                    MU = Config.Instance.Quantity.mdens_mu;
                    break;
                case "Thermodinamic temperature":
                    MU = Config.Instance.Quantity.temp_mu;
                    break;
                case "Luminous intensity":
                    MU = Config.Instance.Quantity.lum_mu;
                    break;
                case "Concentration":
                    MU = Config.Instance.Quantity.conc_mu;
                    break;
                case "Force":
                    MU = Config.Instance.Quantity.force_mu;
                    break;
                case "Pressure":
                    MU = Config.Instance.Quantity.press_mu;
                    break;
                case "Power":
                    MU = Config.Instance.Quantity.power_mu;
                    break;
                case "Capacity":
                    MU = Config.Instance.Quantity.cap_mu;
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
                        if (Unit.Value.Equals(Config.Instance.Quantity.mass_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Volume":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.vol_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Amount of substance":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.sub_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Energy, work, heat":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.energy_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Length":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.length_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Electric current":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.curr_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Area":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.area_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Speed":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.speed_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Acceleration":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.acc_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Mass density":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.mdens_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Thermodinamic temperature":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.temp_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Luminous intensity":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.lum_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Concentration":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.conc_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Force":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.force_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Pressure":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.press_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Power":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.power_mu))
                        {
                            MU = Unit.Key;
                        }
                    }
                    break;
                case "Capacity":
                    foreach (KeyValuePair<string, string> Unit in quantities[quantityType])
                    {
                        if (Unit.Value.Equals(Config.Instance.Quantity.cap_mu))
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
        #endregion
    }

    [Serializable()]
    public class GraphSettings
    {
        public bool ShowMaterialText = true;
        public bool ShowOperatingUnitText = true;
        public bool ShowEdgeText = true;
        public bool ShowComments = true;
        public bool ShowParameters = true;
        public bool ShowEdgeLongFormat = true;
        public int GridSize = 300;
        public int DefaultFontSize = 12;

        public void Reset()
        {
            GridSize = 300;
            DefaultFontSize = 12;
        }
    }

    [Serializable()]
    public class SolutionSettings
    {

        public enum ValueStyle { None = 0, Original = 1, Calculated = 2 }
        public enum Visibility { Hide = 0, Show = 1 }

        public class StructureItem
        {
            [XmlIgnore]
            public System.Drawing.Color Color = System.Drawing.Color.Black;

            public ValueStyle EdgeText = ValueStyle.Calculated;
            public ValueStyle MaterialText = ValueStyle.Original;
            public ValueStyle OperatingUnitText = ValueStyle.Original;
            public Visibility Visible = Visibility.Show;

            public int ColorARGB
            {
                get
                {
                    return Color.ToArgb();
                }
                set
                {
                    Color = Color.FromArgb(value);
                }
            }
        }

        public StructureItem IncludedItem = new StructureItem();

        public StructureItem ExcludedItem = new StructureItem();

        public SolutionSettings()
        {
            ExcludedItem.Color = System.Drawing.Color.LightGray;
            ExcludedItem.EdgeText = ValueStyle.None;            
        }

    }

    [Serializable()]
    public class LayoutSettings
    {
        public int DefaultRootX = 5;
        public int DefaultRootY = 2;
        public int DefaultLayerDistance = 3;
        public int DefaultNodeDistance = 2;
        public bool WeightedArcs = false;
        public bool FixedRaws = false;
        public bool FixedProducts = false;
        public string SelectedEngine = "Dot";
        public String Align = "Vertical";
    }

    [Serializable()]
    public class Config
    {
        #region Instance

        private static Config instance = null;

        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Config();
                }

                return instance;
            }
            private set { }
        }

        #endregion

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

        #region Values

        public Login Login = new Login();

        public Material Material = new Material();
        public OperatingUnit OperatingUnit = new OperatingUnit();
        public Edge Edge = new Edge();

        public Quantity Quantity = new Quantity();

        public Mongo Mongo = new Mongo();

        public SolverSettings SolverSettings = new SolverSettings();        
        public GraphSettings GraphSettings = new GraphSettings();
        public SolutionSettings SolutionSettings = new SolutionSettings();
        public LayoutSettings LayoutSettings = new LayoutSettings();

        public int MaxUndo = 50;

        public int ImageWidth = 6000;
        public int ImageHeight = 6000;

        public int MaterialSize = 100;
        public int EdgeNodeSize = 40;
        public int TemporaryEdgeNodeSize = 20;
        public int OperatingUnitWidth = 750;
        public int OperatingUnitHeight = 100;

        public int DefaultLineHeight = 120;

        public int LineSize = 10;

        [XmlIgnore]
        public Dictionary<string, string> DefaultParameters = new Dictionary<string, string>();

        #endregion

        #region Constructors

        private Config()
        {
            DefaultParameters.Add("price", "0");
            DefaultParameters.Add("payoutperiod", "10");
            DefaultParameters.Add("workinghour", "8000");
        }

        #endregion

        #region IO functions

        public void Save(string fileName)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(Config.Instance.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, instance);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Source);
            }
        }

        public void Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return; }

            try
            {
                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = Config.Instance.GetType();

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        instance = (Config)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion
    }
}
