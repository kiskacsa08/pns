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
using System.Text;
using System.Globalization;
using System.Drawing;

namespace PNSDraw
{
    public class ConvertManager
    {


        public static string ToString(double d)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", true).NumberFormat;
            return d.ToString(nfi);
        }

        public static string ToHtml(Color c)
        {
            return ColorTranslator.ToHtml(Color.FromArgb(c.ToArgb()));
        }

        public static int ToInt(string s)
        {
            return ToInt(s, true);
        }

        public static int ToInt(string s, bool strict)
        {
            int ret=0;

            NumberFormatInfo nfi = new CultureInfo("en-US", true).NumberFormat;
            nfi.CurrencyDecimalSeparator = ".";
            nfi.CurrencyGroupSeparator = " ";

            string num = s;
            
            if (strict == false)
            {
                int i=0;
                num="";
                for (i = 0; i < s.Length; i++)
                {
                    if (s[i]>='0' && s[i] <= '9')
                    {
                        num += s[i];
                    }
                }
            }

            try
            {
                ret = Int32.Parse(num, nfi);
            }
            catch (Exception)
            {
                ret = 0;
            }
            return ret;
        }

        public static double ToDouble(string s)
        {
            return ToDouble(s, true);
        }

        public static double ToDouble(string s, bool strict)
        {
            double ret = 0;
            s = s.Replace(',' , '.');

            NumberFormatInfo nfi = new CultureInfo("en-US", true).NumberFormat;
            nfi.CurrencyDecimalSeparator = ".";
            nfi.CurrencyGroupSeparator = " ";

            string num = s;

            if (strict == false)
            {
                int i = 0;
                num = "";
                for (i = 0; i < s.Length; i++)
                {
                    num = s.Substring(0, s.Length-i);
                    if (Double.TryParse(num, System.Globalization.NumberStyles.Any, nfi, out ret) == true)
                    {
                        return ret;
                    }
                }
            }

            try
            {
                ret = Double.Parse(num, nfi);
            }
            catch (Exception)
            {
                ret = 0;
            }
            return ret;
        }

    }
}
