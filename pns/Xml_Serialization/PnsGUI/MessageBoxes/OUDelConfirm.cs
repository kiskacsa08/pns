﻿using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.MessageBoxes
{
    public class def_ou_delete_confirm_msg
    {
        #region Members
        static public string Msg_delete_confirm_title = "Confirmation";
        static public string Msg_delete_confirm = "Delete operating unit?";
        #endregion

        #region Properties
        public MessageBoxXMLTag MessageBoxDeleteOperatingUnit
        {
            get { return new MessageBoxXMLTag(new TextXMLTag(Msg_delete_confirm), Msg_delete_confirm_title); }
            set
            {
                Msg_delete_confirm = value.Message.TextPart1;
                Msg_delete_confirm_title = value.Title;
            }
        }
        #endregion
    }
}