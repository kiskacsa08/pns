/* Copyright 2015 Department of Computer Science and Systems Technology, University of Pannonia

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PNSDraw.Excel_export
{
    class ColorXMLTag
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
