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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Globalization;

using PNSDraw.Configuration;

namespace PNSDraw.online
{
    class MUs
    {
        private static XmlDocument doc;

        public static double ConvertToUnifiedUnit(string fromUnit, double value)
        {
            if (fromUnit.Length == 0)
            {
                return value;
            }

            string toUnit = GetBaseUnit(fromUnit, false);
            string quantity = GetQuantity(toUnit);

            return UnitConvert(fromUnit, toUnit, value, quantity);
        }

        public static double ConvertToSpecialUnit(string toUnit, double value)
        {
            if (toUnit.Length == 0)
            {
                return value;
            }

            string fromUnit = GetBaseUnit(toUnit, false);
            string quantity = GetQuantity(fromUnit);

            return UnitConvert(fromUnit, toUnit, value, quantity);
        }

        public static double UnitConvert(string fromUnit, string toUnit, double value, string quantity)
        {
            double factor = GetFactorByQuantity(fromUnit, quantity);
            double convertedValue = value;

            if (factor > 0)
            {
                convertedValue *= factor;
            }
            else
            {
                //throw new Exception("From unit not found or null!");
                return value;
            }

            factor = GetFactorByQuantity(toUnit, quantity);

            if (factor > 0)
            {
                convertedValue /= factor;
            }
            else
            {
                //throw new Exception("To unit not found or null!");
                return value;
            }
            return convertedValue;
        }

        private static void Read(){            
            doc = new XmlDocument();
            doc.LoadXml(PNSDraw.Properties.Resources.Units);
        }

        public static string GetBaseQuantity(string quantity)
        {
            if (doc == null)
            {
                Read();
            }

            foreach (XmlNode node in doc.SelectNodes("//base_quantity"))
            {
                if (node.Attributes["quantity"].Value.ToUpper().Equals(quantity.ToUpper()))
                {
                    return node.Attributes["symbol"].Value;
                }
            }
            return "";
        }

        private static string GetDefaultUnit(string quantity)
        {
            string unit = Config.Instance.Quantity.mass_mu.ToString();
            switch (quantity)
            {
                case "mass":
                    unit = Config.Instance.Quantity.mass_mu.ToString();
                    break;
                case "currency":
                    unit = Config.Instance.Quantity.money_mu.ToString();
                    break;
                case "time":
                    unit = Config.Instance.Quantity.time_mu.ToString();
                    break;
            }
            return unit;
        }

        private static double GetFactorByQuantity(string unit, string quantity)
        {
            if (unit.Length == 0)
            {
                switch (quantity)
                {
                    case "mass":
                        unit = Config.Instance.Quantity.mass_mu.ToString();
                        break;
                    case "currency":
                        unit = Config.Instance.Quantity.money_mu.ToString();
                        break;
                    case "time":
                        unit = Config.Instance.Quantity.time_mu.ToString();
                        break;
                    default:
                        unit = Config.Instance.Quantity.mass_mu.ToString();
                        break;
                }
            }

            if (doc == null)
            {
                Read();
            }

            XmlNode derived = null;

            foreach (XmlNode node in doc.SelectNodes("//derived_quantity"))
            {
                if (node.Attributes["quantity"].Value.ToUpper().Equals(quantity.ToUpper()))
                {
                    derived = node;
                    break;
                }
            }

            if (derived == null)
            {
                return 1;
            }

            XmlNode XMLUnit = null;

            foreach (XmlNode node in derived.SelectNodes("units/unit"))
            {
                if (node.Attributes["symbol"].Value.ToUpper().Equals(unit.ToUpper()))
                {
                    XMLUnit = node.FirstChild.FirstChild;
                }
            }

            if (XMLUnit == null)
            {
                return 1;
            }

            return Convert.ToDouble(XMLUnit.Attributes["factor"].Value, CultureInfo.InvariantCulture);
        }

        /*private static double GetFactorByQuantity(string unit)
        {
            if (doc == null)
            {
                Read();
            }

            XmlNode XMLUnit = null;

            foreach (XmlNode node in doc.SelectNodes("//derived_quantity/units/unit"))
            {
                if (node.Attributes["symbol"].Value.ToUpper().Equals(unit.ToUpper()))
                {
                    XMLUnit = node.FirstChild.FirstChild;
                }
            }

            if (XMLUnit == null)
            {
                return 1;
            }

            return Convert.ToDouble(XMLUnit.Attributes["factor"].Value, CultureInfo.InvariantCulture);
        }*/

        private static string GetBaseUnit(string unit, bool fromXML = true)
        {
            
            if (doc == null)
            {
                Read();
            }

            XmlNode XMLUnit = null;

            foreach (XmlNode node in doc.SelectNodes("//derived_quantity/units/unit"))
            {
                if (node.Attributes["symbol"].Value.ToUpper().Equals(unit.ToUpper()))
                {
                    XMLUnit = node;
                }
            }

            if (XMLUnit == null)
            {
                return unit;
            }

            string quantity = XMLUnit.ParentNode.ParentNode.Attributes["quantity"].Value;

            if (fromXML)
            {
                return GetBaseQuantity(quantity);
            }
            else
            {
                string ret = Config.Instance.Quantity.mass_mu.ToString();
                switch (quantity)
                {
                    case "mass":
                        ret = Config.Instance.Quantity.mass_mu.ToString();
                        break;
                    case "currency":
                        ret = Config.Instance.Quantity.money_mu.ToString();
                        break;
                    case "time":
                        ret = Config.Instance.Quantity.time_mu.ToString();
                        break;
                }
                return ret;
            }
        }

        public static string GetQuantity(string unit)
        {
            if (unit.Length == 0)
            {
                return "mass";
            }

            if (doc == null)
            {
                Read();
            }

            XmlNode XMLUnit = null;

            foreach (XmlNode node in doc.SelectNodes("//derived_quantity/units/unit"))
            {
                if (node.Attributes["symbol"].Value.ToUpper().Equals(unit.ToUpper()))
                {
                    XMLUnit = node;
                }
            }

            if (XMLUnit == null)
            {
                return unit;
            }

            string quantity = XMLUnit.ParentNode.ParentNode.Attributes["quantity"].Value;
            

            return quantity;
        }
    }
}
