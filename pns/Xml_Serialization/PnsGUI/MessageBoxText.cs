using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI
{
    public class TextXMLTag
    {
        #region Members
        string m_part1, m_part2, m_part3, m_part4, m_part5;
        #endregion

        #region Constructors
        public TextXMLTag()
        {
        }
        public TextXMLTag(string t_part1)
            : this(t_part1, null, null, null, null)
        {
        }
        public TextXMLTag(string t_part1, string t_part2)
            : this(t_part1, t_part2, null, null, null)
        {
        }
        public TextXMLTag(string t_part1, string t_part2, string t_part3)
            : this(t_part1, t_part2, t_part3, null, null)
        {
        }
        public TextXMLTag(string t_part1, string t_part2, string t_part3, string t_part4)
            : this(t_part1, t_part2, t_part3, t_part4, null)
        {
        }
        public TextXMLTag(string t_part1, string t_part2, string t_part3, string t_part4, string t_part5)
        {
            m_part1 = t_part1;
            m_part2 = t_part2;
            m_part3 = t_part3;
            m_part4 = t_part4;
            m_part5 = t_part5;
        }
        #endregion

        #region Properties
        [XmlAttribute()]
        public string TextPart1 { get { return m_part1; } set { m_part1 = value; } }
        [XmlAttribute()]
        public string TextPart2 { get { return m_part2; } set { m_part2 = value; } }
        [XmlAttribute()]
        public string TextPart3 { get { return m_part3; } set { m_part3 = value; } }
        [XmlAttribute()]
        public string TextPart4 { get { return m_part4; } set { m_part4 = value; } }
        [XmlAttribute()]
        public string TextPart5 { get { return m_part5; } set { m_part5 = value; } }
        #endregion
    }
    public class MessageBoxXMLTag
    {
        #region Members
        string m_title;
        TextXMLTag m_message;
        #endregion

        #region Constructors
        public MessageBoxXMLTag()
        {
        }
        public MessageBoxXMLTag(TextXMLTag t_message)
            : this(t_message, null)
        {
        }
        public MessageBoxXMLTag(TextXMLTag t_message, string t_title)
        {
            m_message = t_message;
            m_title = t_title;
        }
        #endregion

        #region Properties
        public TextXMLTag Message { get { return m_message; } set { m_message = value; } }
        [XmlAttribute()]
        public string Title { get { return m_title; } set { m_title = value; } }
        #endregion
    }
}
