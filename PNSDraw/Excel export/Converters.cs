using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNSDraw.Excel_export
{
    static public class Converters
    {
        static public string ToNameString(string str)
        {
            string NameCharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_[]";
            for (int i = 0; i < str.Length; i++) if (NameCharSet.IndexOf(str[i]) == -1) str = str.Replace(str[i], '_');
            if (str == "") str = "NO_NAME";
            if (str[0] >= '0' && str[0] <= '9') str = str.Insert(0, "_");
            return str;
        }
    }
}
