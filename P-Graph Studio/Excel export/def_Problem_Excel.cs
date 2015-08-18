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

namespace PNSDraw.Excel_export
{
    class def_Problem_Excel
    {
        #region Members

        #region SaveDialogSettings
        static public string excel_filename_p1 = "Excel export of ";
        static public string excel_filename_p2 = "problem";
        #endregion

        #region Rates
        static public string text_ws_rates = "Rates";
        static public string text_rates_label = "Material rates\r\nof operating units";
        #endregion

        #endregion

        #region Properties

        #region SaveDialogSettings
        public TextXMLTag DefaultXLSFileName
        {
            get { return new TextXMLTag(excel_filename_p1, excel_filename_p2); }
            set
            {
                excel_filename_p1 = value.TextPart1;
                excel_filename_p2 = value.TextPart2;
            }
        }
        #endregion

        #region Rates
        public string RatesWorkSheetLabel { get { return text_ws_rates; } set { text_ws_rates = value; } }
        public string RatesLabel { get { return text_rates_label; } set { text_rates_label = value; } }
        #endregion

        #endregion
    }
}
