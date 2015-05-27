using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace PNSDraw.online
{
    class MUs
    {
        private static XmlDocument doc;

        private static void Read(){
            string XMLText = File.ReadAllText(@"Units.xml");

            doc = new XmlDocument();
            doc.LoadXml(XMLText);
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

        public static double GetFactorByQuantity(string unit, string quantity)
        {
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

            return Convert.ToDouble(XMLUnit.Attributes["factor"].Value);
        }

        public static double GetFactorByQuantity(string unit)
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

            return Convert.ToDouble(XMLUnit.Attributes["factor"].Value);
        }
    }
}
