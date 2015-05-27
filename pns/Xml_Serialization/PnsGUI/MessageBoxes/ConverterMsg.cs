using System.Xml.Serialization;
using Pns.Xml_Serialization.PnsGUI.Dialogs.DefValues;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;

namespace Pns.Xml_Serialization.PnsGUI.MessageBoxes
{
    public class def_Convert_msg
    {
        #region Members
        static public string Msg_lb = "Value must be >= ";
        static public string Msg_ub = "Value must be <= ";
        static public string Msg_solver_limit = "Value must be <= than ";
        #endregion

        #region Properties
        static public string Msg_solver_ub { get { return Msg_solver_limit + def_DefaultValuesDlg.dialog_text + def_PnsStudio.PathDelimiter + def_DefaultValuesPropGrid.Cat_solver + def_PnsStudio.PathDelimiter + def_DefaultValuesPropGrid.DN_solver_ub; } }
        [XmlAttribute()]
        public string LowerBoundMessage { get { return Msg_lb; } set { Msg_lb = value; } }
        [XmlAttribute()]
        public string UpperBoundMessage { get { return Msg_ub; } set { Msg_ub = value; } }
        [XmlAttribute()]
        public string SolverUpperBoundMessage { get { return Msg_solver_limit; } set { Msg_solver_limit = value; } }
        #endregion
    }
}
