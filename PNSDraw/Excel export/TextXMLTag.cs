﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PNSDraw.Excel_export
{
    public class TextXMLTag
    {
        string m_part1, m_part2, m_part3, m_part4, m_part5;

        public TextXMLTag(string t_part1, string t_part2)
            : this(t_part1, t_part2, null, null, null)
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
}