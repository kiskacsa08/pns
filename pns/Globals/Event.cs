using System;
using Pns.Dialogs;

namespace Pns.Globals
{
    public delegate void MatPropEventHandler(object sender, MaterialPropertyEventArgs e);
    public delegate void SelectMaterialPanelEventHandler(object sender, EventArgs e);
    public delegate void OUPropEventHandler(object sender, OperatingUnitPropertyEventArgs e);
    public delegate void SelectOperatingUnitPanelEventHandler(object sender, EventArgs e);

    public class MaterialPropertyEventArgs : EventArgs
    {
        public MaterialPropertyEventArgs(MaterialProperties matprop, defaults.MatPropButtons buttonclick)
        {
            this.matprop = matprop;
            this.buttonclick = buttonclick;
        }
        public MaterialProperties matprop;
        public defaults.MatPropButtons buttonclick;
    }

    public class OperatingUnitPropertyEventArgs : EventArgs
    {
        public OperatingUnitPropertyEventArgs(OperatingUnitProperties ouprop, defaults.OUPropButtons buttonclick)
        {
            this.ouprop = ouprop;
            this.buttonclick = buttonclick;
        }
        public OperatingUnitProperties ouprop;
        public defaults.OUPropButtons buttonclick;
    }

    static public class events
    {
        static public event MatPropEventHandler MatPropChange;
        static public void OnMatPropChange(object sender, MaterialPropertyEventArgs e)
        {
            if (MatPropChange != null) MatPropChange(sender, e);
        }

        static public event SelectMaterialPanelEventHandler MaterialPanelChange;
        static public void OnMaterialPanelChange(object sender, EventArgs e)
        {
            if (MaterialPanelChange != null) MaterialPanelChange(sender, e);
        }
        static public event OUPropEventHandler OUPropChange;
        static public void OnOUPropChange(object sender, OperatingUnitPropertyEventArgs e)
        {
            if (OUPropChange != null) OUPropChange(sender, e);
        }

        static public event SelectOperatingUnitPanelEventHandler OperatingUnitPanelChange;
        static public void OnOperatingUnitPanelChange(object sender, EventArgs e)
        {
            if (OperatingUnitPanelChange != null) OperatingUnitPanelChange(sender, e);
        }
    }
}
