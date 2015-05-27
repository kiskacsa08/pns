using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.PnsStudioFom
{
    public class def_Solutions
    {
        #region Members
        static public string text_solution = "Solution";
        static public string Msg_solution_tolerance_error_p1 = "The solver calculated annual cost (";
        static public string Msg_solution_tolerance_error_p2 = ") and the GUI calculated annual cost (";
        static public string Msg_solution_tolerance_error_p3 = ") differ significantly.";
        static public string text_total_cost = "Total cost";
        #endregion

        #region Properties
        [XmlAttribute()]
        public string SolutionText { get { return text_solution; } set { text_solution = value; } }
        public MessageBoxXMLTag MessageBoxSolutions
        {
            get { return new MessageBoxXMLTag(new TextXMLTag(Msg_solution_tolerance_error_p1, Msg_solution_tolerance_error_p2, Msg_solution_tolerance_error_p3)); }
            set
            {
                Msg_solution_tolerance_error_p1 = value.Message.TextPart1;
                Msg_solution_tolerance_error_p2 = value.Message.TextPart2;
                Msg_solution_tolerance_error_p3 = value.Message.TextPart3;
            }
        }
        [XmlAttribute()]
        public string TotalCostText { get { return text_total_cost; } set { text_total_cost = value; } }
        #endregion
    }
}
