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
