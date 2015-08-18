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

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PNSDraw
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            /*try
            {
                if (ApplicationDeployment.CurrentDeployment.IsFirstRun) { 

}
            }
            catch (Exception ex)
            {

            }

            MessageBox.Show(ApplicationDeployment.CurrentDeployment.IsFirstRun.ToString());
            */
            DateTime expiredDate = new DateTime(2016, 1, 1);

            int valid = DateTime.Compare(DateTime.Now, expiredDate);

            if (valid > 0)
            {
                MessageBox.Show("The software has expired, please upgrade!", "Expired", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(0);
            }
            
            string filename = "";

            try
            {
                foreach (string commandLineFile in AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData)
                {
                    string[] separator = {"file:///"};
                    string[] parts = commandLineFile.Split(separator,StringSplitOptions.RemoveEmptyEntries);
                    filename = WebUtility.UrlDecode(parts[0]);                 
                }
            }
            catch (Exception ex)
            {
                string[] arguments = Environment.GetCommandLineArgs();
                if (arguments.Length > 1)
                {
                    filename = arguments[1];
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(filename));
        }
    }
}
