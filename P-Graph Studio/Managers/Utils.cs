using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PNSDraw.Configuration;
using System.Windows.Forms;
using PNSDraw.Dialogs;
using MsgBox;

namespace PNSDraw
{
    public class Utils
    {
        private static Utils instance = null;

        public static Utils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Utils();
                }
                return instance;
            }
            private set { }
        }

        private Utils()
        {

        }

        public bool Report(string level, string text, bool needMsgToUser = true)
        {
            if (needMsgToUser)
            {
                DialogResult result = MessageBox.Show("Problem was occurred while the software is running.\nWant to provide feedback to developers about it?", "Problem occured", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    return new WebserverHttpClient().Report(level, text);
                }
                return false;
            }
            else
            {
                return new WebserverHttpClient().Report(level, text);
            }
        }

        public bool CheckLogin()
        {
            if (Config.Instance.Login.IsLoggedIn)
            {
                return true;
            }
            else
            {
                DialogResult result = MessageBox.Show("To use this feature you must log in! Please log in!\nIf you do not have an account, please register one!\nPlease try again after you've logged in!"
                    , "You are not logged in!"
                    , MessageBoxButtons.OKCancel
                    , MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    OnNotify();
                }

                return false;
            }
        }

        private void OnNotify()
        {
            if (LoginChanged != null)
            {
                LoginChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler LoginChanged;
    }
}
