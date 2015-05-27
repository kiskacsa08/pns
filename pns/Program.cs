using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Pns.Xml_Serialization.PnsGUI.PnsStudioFom;

namespace Pns
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new PnsStudio());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), def_PnsStudio.Ex_fatal_error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}