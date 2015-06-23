using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace PNSDraw
{
    [TypeConverter(typeof(PropertySorter))]
    public class ObjectProperty
    {
        #region Core private data variables

        string name = "";

        [Browsable(false)]
        public string Name {
            get
            {
                return name;
            }
        }

        bool visible = false;

        [Browsable(false)]
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

        double value = 0.0;

        [Browsable(false)]
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        string mu = "";

        [Browsable(false)]
        public string MU
        {
            get
            {
                return this.mu;
            }
            set
            {
                this.mu = value;
            }
        }

        string prefix = "";

        [Browsable(false)]
        public string Prefix
        {
            get
            {
                return this.prefix;
            }
            set
            {
                this.prefix = value;
            }
        }

        [Browsable(false)]
        public string DisplayedText
        {
            get
            {
                //if (Visible)
                //{
                //    if (NonValue != null && Value == NonValue)
                //    {
                //        if (Globals.DefaultParameters.ContainsKey(Name))
                //        {
                //            return Prefix + Globals.DefaultParameters[Name] + MU;
                //        }
                //        else
                //        {
                //            return Prefix + "-";
                //        }
                //    }
                //    else
                //    {
                //        return Prefix + Value.ToString() + " " + MU;
                //    }
                //}
                //else
                //{
                    //return "";
                //}
                return Visible.ToString() + ";" + Prefix.ToString() + ";" + Value.ToString() + ";" + MU.ToString();
            }
        }

        double? nonvalue=null;

        [Browsable(false)]
        public double? NonValue
        {
            get
            {
                return nonvalue;
            }
            set
            {
                nonvalue = value;
            }
        }

        #endregion

        public ObjectProperty(string name, Canvas.IGraphicsStructure container, Canvas.IGraphicsObject owner)
        {
            this.name = name;
            Visible = false;
            Value = 0.0;
            MU = "";
            Prefix = this.name + ": ";
        }

        public ObjectProperty(string prefix)
        {
            Visible = false;
            Value = 0.0;
            MU = "";
            Prefix = prefix;
        }

        public override string ToString()
        {
            return DisplayedText;
            //return Value + " " + MU;
        }

        public string ToPNSValue()
        {
            if (Value == NonValue)
            {
                return "";
            }
            else
            {
                return ConvertManager.ToString(Value);
            }
        }

        #region PropertyGrid properties

        [Browsable(true)]
        [Category("Other"), PropertyOrder(11)]
        [DisplayName("Value")]
        public double ValueProp
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
                OnNotify();
            }
        }

        [Browsable(true)]
        [Category("Other"), PropertyOrder(12)]
        [DisplayName("Mu")]
        //[EditorAttribute(typeof(EditorUI), typeof(System.Drawing.Design.UITypeEditor))]
        //[TypeConverter(typeof(MUConverter))]
        public string MUProp
        {
            get
            {
                return MU;
            }
            set
            {
                MU = value;
                OnNotify();
            }
        }

        [Browsable(true)]
        [Category("Other"), PropertyOrder(10)]
        [DisplayName("Prefix")]
        public string PrefixProp
        {
            get
            {
                return Prefix;
            }
            set
            {
                Prefix = value;
                OnNotify();
            }
        }

        [Browsable(true)]
        [Category("Other"), PropertyOrder(8)]
        [DisplayName("Visible")]
        public bool VisibleProp
        {
            get
            {
                return Visible;
            }
            set
            {
                Visible = value;
                OnNotify();
            }
        }

        #endregion

        void OnNotify()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler PropertyChanged;
    }
}
