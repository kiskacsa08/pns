using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.ExceptionMsgs
{
    public class def_mat_ex
    {
        #region Members
        static public string Ex_unknown_material_type = "Unknown material type!";
        static public string Ex_more_inctance_p1 = "The input file corrupted! Material '";
        static public string Ex_more_inctance_p2 = "' has more than 1 instance!";
        #endregion

        #region Properties
        public MessageBoxXMLTag MessageBoxMaterialsCorrupted
        {
            get { return new MessageBoxXMLTag(new TextXMLTag(Ex_more_inctance_p1, Ex_more_inctance_p2)); }
            set
            {
                Ex_more_inctance_p1 = value.Message.TextPart1;
                Ex_more_inctance_p2 = value.Message.TextPart2;
            }
        }
        #endregion
    }
}
