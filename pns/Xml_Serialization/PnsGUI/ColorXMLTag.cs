using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI
{
    public class ColorXMLTag
    {
        #region Members
        int m_red, m_green, m_blue;
        #endregion

        #region Constructors
        public ColorXMLTag() { }
        public ColorXMLTag(int t_red, int t_green, int t_blue)
        {
            m_red = t_red;
            m_green = t_green;
            m_blue = t_blue;
        }
        #endregion

        #region Properties
        [XmlAttribute()]
        public int Red { get { return m_red; } set { m_red = value; } }
        [XmlAttribute()]
        public int Green { get { return m_green; } set { m_green = value; } }
        [XmlAttribute()]
        public int Blue { get { return m_blue; } set { m_blue = value; } }
        #endregion
    }
}
