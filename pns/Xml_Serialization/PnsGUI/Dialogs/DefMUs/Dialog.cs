using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.Dialogs.DefMUs
{
    public class def_DefaultMU
    {
        #region Members
        static public string dialog_text = "Default Measurement Units";
        static public string button_update_text = "Update";
        static public string button_cancel_text = "Cancel";
        static public string button_default_text = "Restore defaults";
        #endregion

        #region Properties
        [XmlAttribute()]
        public string Title { get { return dialog_text; } set { dialog_text = value; } }
        [XmlAttribute()]
        public string UpdateButtonText { get { return button_update_text; } set { button_update_text = value; } }
        [XmlAttribute()]
        public string CancelButtonText { get { return button_cancel_text; } set { button_cancel_text = value; } }
        [XmlAttribute()]
        public string DefaultButtonText { get { return button_default_text; } set { button_default_text = value; } }
        #endregion
    }
}
