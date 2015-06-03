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
