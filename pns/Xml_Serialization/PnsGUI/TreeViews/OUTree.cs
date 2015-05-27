using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.TreeViews
{
    public class def_ou_tree
    {
        #region Members
        static public string OperatingUnitsName = "OperatingUnits";
        static public string OperatingUnitsText = "Operating Units";
        static public string InputMaterialsName = "InputMaterials";
        static public string InputMaterialsText = "Input Materials";
        static public string OutputMaterialsName = "OutputMaterials";
        static public string OutputMaterialsText = "Output Materials";
        static public string NewName = "New";
        static public string NewText = "<New>";
        static public string GenBaseName = "OperatingUnit";
        static public string GenBaseConcat = "";
        static public string GenCloneConcat = "_";
        #endregion

        #region Properties
        [XmlAttribute()]
        public string OperatingUnitsNode { get { return OperatingUnitsText; } set { OperatingUnitsText = value; } }
        [XmlAttribute()]
        public string InputMaterialsNode { get { return InputMaterialsText; } set { InputMaterialsText = value; } }
        [XmlAttribute()]
        public string OutputMaterialsNode { get { return OutputMaterialsText; } set { OutputMaterialsText = value; } }
        [XmlAttribute()]
        public string NewNode { get { return NewText; } set { NewText = value; } }
        [XmlAttribute()]
        public string OperatingUnitBaseName { get { return GenBaseName; } set { GenBaseName = value; } }
        [XmlAttribute()]
        public string OperatingUnitBaseConcatChar { get { return GenBaseConcat; } set { GenBaseConcat = value; } }
        [XmlAttribute()]
        public string OperatingUnitCloneConcatChar { get { return GenCloneConcat; } set { GenCloneConcat = value; } }
        #endregion
    }
}
