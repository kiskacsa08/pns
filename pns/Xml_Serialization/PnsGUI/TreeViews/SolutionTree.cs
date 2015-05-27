using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.TreeViews
{
    public class def_Solution_Tree
    {
        #region Members
        static public string text_consumed = "consumed amount";
        static public string text_produced = "produced amount";
        static public string text_balanced = "balanced";
        static public string text_size_factor = "Size factor";
        static public string text_cost = "Cost";
        #endregion

        #region Properties
        [XmlAttribute()]
        public string ConsumedAmountText { get { return text_consumed; } set { text_consumed = value; } }
        [XmlAttribute()]
        public string ProducedAmountText { get { return text_produced; } set { text_produced = value; } }
        [XmlAttribute()]
        public string BalancedText { get { return text_balanced; } set { text_balanced = value; } }
        [XmlAttribute()]
        public string SizeFactorText { get { return text_size_factor; } set { text_size_factor = value; } }
        [XmlAttribute()]
        public string CostText { get { return text_cost; } set { text_cost = value; } }
        #endregion
    }
}
