using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.TreeViews
{
    public class def_mat_tree
    {
        #region Members
        static public string MaterialsName = "Materials";
        static public string MaterialsText = "Materials";
        static public string RawsName = "Raws";
        static public string RawsText = "Raw Materials";
        static public string IntermediatesName = "Intermediates";
        static public string IntermediatesText = "Intermediates";
        static public string ProductsName = "Products";
        static public string ProductsText = "Products";
        static public string NewName = "New";
        static public string NewText = "<New>";
        static public string GenBaseName = "Material";
        static public string GenBaseConcat = "";
        static public string GenCloneConcat = "_";
        #endregion

        #region Properties
        [XmlAttribute()]
        public string MaterialsNode { get { return MaterialsText; } set { MaterialsText = value; } }
        [XmlAttribute()]
        public string RawsNode { get { return RawsText; } set { RawsText = value; } }
        [XmlAttribute()]
        public string IntermediatesNode { get { return IntermediatesText; } set { IntermediatesText = value; } }
        [XmlAttribute()]
        public string ProductsNode { get { return ProductsText; } set { ProductsText = value; } }
        [XmlAttribute()]
        public string NewNode { get { return NewText; } set { NewText = value; } }
        [XmlAttribute()]
        public string MaterialBaseName { get { return GenBaseName; } set { GenBaseName = value; } }
        [XmlAttribute()]
        public string MaterialBaseConcatChar { get { return GenBaseConcat; } set { GenBaseConcat = value; } }
        [XmlAttribute()]
        public string MaterialCloneConcatChar { get { return GenCloneConcat; } set { GenCloneConcat = value; } }
        #endregion
    }
}
