using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.MessageBoxes
{
    public class def_ou_msg
    {
        #region Members
        static public string Msg_can_not_clone_temporary = "You can not clone temporary operating unit!";

        static public string Msg_ou_exists_p1 = "The operating unit '";
        static public string Msg_ou_exists_p2 = "' already exists!";
        static public string Msg_ou_exists_title = "Invalid operating unit name";
        #endregion

        #region Properties
        public MessageBoxXMLTag MessageBoxCanNotClone { get { return new MessageBoxXMLTag(new TextXMLTag(Msg_can_not_clone_temporary)); } set { Msg_can_not_clone_temporary = value.Message.TextPart1; } }
        public MessageBoxXMLTag MessageBoxOperatingUnitExists
        {
            get { return new MessageBoxXMLTag(new TextXMLTag(Msg_ou_exists_p1, Msg_ou_exists_p2), Msg_ou_exists_title); }
            set
            {
                Msg_ou_exists_p1 = value.Message.TextPart1;
                Msg_ou_exists_p2 = value.Message.TextPart2;
                Msg_ou_exists_title = value.Title;
            }
        }
        #endregion
    }
}
