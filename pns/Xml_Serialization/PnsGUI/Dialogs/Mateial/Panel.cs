using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.Dialogs.Mateial
{
    public class def_mat_panel
    {
        #region Members
        static public string title_p1 = " - Material properties";
        static public string title_p2 = " (new)";
        static public string UpdateText = "Update";
        static public string CancelText = "Cancel";
        static public string DeleteText = "Delete";
        static public string UpdateText_new = "Save";
        static public string CancelText_new = "Cancel";
        static public string DeleteText_new = "Delete";
        static public string AutoConvertText = "Convert values automatically";
        #endregion

        #region Properties
        [XmlAttribute()]
        public string Title { get { return title_p1; } set { title_p1 = value; } }
        [XmlAttribute()]
        public string TitlePart2NewMaterial { get { return title_p2; } set { title_p2 = value; } }
        [XmlAttribute()]
        public string UpdateButtonText { get { return UpdateText; } set { UpdateText = value; } }
        [XmlAttribute()]
        public string CancelButtonText { get { return CancelText; } set { CancelText = value; } }
        [XmlAttribute()]
        public string DeleteButtonText { get { return DeleteText; } set { DeleteText = value; } }
        [XmlAttribute()]
        public string UpdateButtonTextNewMaterial { get { return UpdateText_new; } set { UpdateText_new = value; } }
        [XmlAttribute()]
        public string CancelButtonTextNewMaterial { get { return CancelText_new; } set { CancelText_new = value; } }
        [XmlAttribute()]
        public string DeleteButtonTextNewMaterial { get { return DeleteText_new; } set { DeleteText_new = value; } }
        [XmlAttribute()]
        public string AutoConvertCheckBoxText { get { return AutoConvertText; } set { AutoConvertText = value; } }
        #endregion
    }
}
