using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.Dialogs
{
    public class def_MaterialQuantityTypeChanged
    {
        #region Members
        static public string title = "Material quantity type changed";
        static public string OkText = "Ok";
        static public string CancelText = "Cancel";
        static public string GroupBoxMUSettings = "Measurement unit settings of concerned flows";
        static public string RadioButtonMaterialMU = "Set to measurement unit of the material flow";
        static public string RadioButtonMinMU = "Set to measurement unit of the material minimum flow";
        static public string RadioButtonMaxMU = "Set to measurement unit of the material maximum flow";
        static public string RadioButtonDefMU = "Set to default measurement unit";
        static public string GroupBoxValueSettings = "Value settings of concerned flows";
        static public string RadioButtonActionNeeded = "Set concerned flows to \"User action needed\" state";
        static public string RadioButtonKeepCurrent = "Keep current values";
        static public string RadioButtonDefFlow = "Set concerned flows to default flowrate value";
        #endregion

        #region Properties
        [XmlAttribute()]
        public string Title { get { return title; } set { title = value; } }
        [XmlAttribute()]
        public string OkButtonText { get { return OkText; } set { OkText = value; } }
        [XmlAttribute()]
        public string CancelButtonText { get { return CancelText; } set { CancelText = value; } }
        [XmlAttribute()]
        public string MUSettingsGroupBoxText { get { return GroupBoxMUSettings; } set { GroupBoxMUSettings = value; } }
        [XmlAttribute()]
        public string MatFlowMURadio { get { return RadioButtonMaterialMU; } set { RadioButtonMaterialMU = value; } }
        [XmlAttribute()]
        public string MatMinMURadio { get { return RadioButtonMinMU; } set { RadioButtonMinMU = value; } }
        [XmlAttribute()]
        public string MatMaxMURadio { get { return RadioButtonMaxMU; } set { RadioButtonMaxMU = value; } }
        [XmlAttribute()]
        public string MatDefMURadio { get { return RadioButtonDefMU; } set { RadioButtonDefMU = value; } }
        [XmlAttribute()]
        public string ValueSettingsGroupBoxText { get { return GroupBoxValueSettings; } set { GroupBoxValueSettings = value; } }
        [XmlAttribute()]
        public string ActionNeededRadio { get { return RadioButtonActionNeeded; } set { RadioButtonActionNeeded = value; } }
        [XmlAttribute()]
        public string KeepCurrentRadio { get { return RadioButtonKeepCurrent; } set { RadioButtonKeepCurrent = value; } }
        [XmlAttribute()]
        public string DefFlowRadio { get { return RadioButtonDefFlow; } set { RadioButtonDefFlow = value; } }
        #endregion
    }
}
