using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI
{
    public class PropertyGridItemXMLTag
    {
        #region Members
        string DN, D, V1, V2, V3;
        #endregion

        #region Constructors
        public PropertyGridItemXMLTag()
        {
        }
        public PropertyGridItemXMLTag(string t_DN, string t_D)
            : this(t_DN, t_D, null, null, null)
        {
        }
        public PropertyGridItemXMLTag(string t_DN, string t_D, string t_V1)
            : this(t_DN, t_D, t_V1, null, null)
        {
        }
        public PropertyGridItemXMLTag(string t_DN, string t_D, string t_V1, string t_V2)
            : this(t_DN, t_D, t_V1, t_V2, null)
        {
        }
        public PropertyGridItemXMLTag(string t_DN, string t_D, string t_V1, string t_V2, string t_V3)
        {
            DN = t_DN;
            D = t_D;
            V1 = t_V1;
            V2 = t_V2;
            V3 = t_V3;
        }
        #endregion

        #region Properties
        [XmlAttribute()]
        public string DisplayName { get { return DN; } set { DN = value; } }
        [XmlAttribute()]
        public string Description { get { return D; } set { D = value; } }
        [XmlAttribute()]
        public string Value1 { get { return V1; } set { V1 = value; } }
        [XmlAttribute()]
        public string Value2 { get { return V2; } set { V2 = value; } }
        [XmlAttribute()]
        public string Value3 { get { return V3; } set { V3 = value; } }
        #endregion
    }
}
