using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.MessageBoxes
{
    public class def_mat_msg
    {
        #region Members
        static public string Msg_can_not_clone_temporary = "You can not clone temporary material!";

        static public string Msg_material_is_in_use_p1 = "The material '";
        static public string Msg_material_is_in_use_p2 = "' is in use!";
        static public string Msg_material_is_in_use_title = "Deleting material error";

        static public string Msg_material_exists_p1 = "The material '";
        static public string Msg_material_exists_p2 = "' already exists!";
        static public string Msg_material_exists_title = "Invalid material name";
        #endregion

        #region Properties
        public MessageBoxXMLTag MessageBoxCanNotClone { get { return new MessageBoxXMLTag(new TextXMLTag(Msg_can_not_clone_temporary)); } set { Msg_can_not_clone_temporary = value.Message.TextPart1; } }
        public MessageBoxXMLTag MessageBoxMaterialIsInUse
        {
            get { return new MessageBoxXMLTag(new TextXMLTag(Msg_material_is_in_use_p1, Msg_material_is_in_use_p2), Msg_material_is_in_use_title); }
            set
            {
                Msg_material_is_in_use_p1 = value.Message.TextPart1;
                Msg_material_is_in_use_p2 = value.Message.TextPart2;
                Msg_material_is_in_use_title = value.Title;
            }
        }
        public MessageBoxXMLTag MessageBoxMaterialExists
        {
            get { return new MessageBoxXMLTag(new TextXMLTag(Msg_material_exists_p1, Msg_material_exists_p2), Msg_material_exists_title); }
            set
            {
                Msg_material_exists_p1 = value.Message.TextPart1;
                Msg_material_exists_p2 = value.Message.TextPart2;
                Msg_material_exists_title = value.Title;
            }
        }
        #endregion
    }
}
