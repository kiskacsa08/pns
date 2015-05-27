using System.Xml.Serialization;

namespace Pns.Xml_Serialization.PnsGUI.Dialogs.OpUnit
{
    public class def_iomat_prop
    {
        #region Members
        static public string DN_rate = "Rate";
        static public string D_rate = "";
        static public string DN_mu = "Measurement unit";
        static public string D_mu = "";
        #endregion

        #region Properties
        public PropertyGridItemXMLTag FlowRate
        {
            get { return new PropertyGridItemXMLTag(DN_rate, D_rate); }
            set
            {
                DN_rate = value.DisplayName;
                D_rate = value.Description;
            }
        }
        public PropertyGridItemXMLTag FlowRateMU
        {
            get { return new PropertyGridItemXMLTag(DN_mu, D_mu); }
            set
            {
                DN_mu = value.DisplayName;
                D_mu = value.Description;
            }
        }
        #endregion
    }
}
