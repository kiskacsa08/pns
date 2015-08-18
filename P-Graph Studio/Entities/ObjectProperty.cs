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
using System.ComponentModel;
using System.Drawing;

using PNSDraw.Configuration;

namespace PNSDraw
{
    [TypeConverter(typeof(ObjectPropertySorter))]
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
                if (this.value == NonValue)
                {
                    return GetDefault();
                }
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
                if (Visible)
                {
                    if (NonValue != null && Value == NonValue)
                    {
                        if (Config.Instance.DefaultParameters.ContainsKey(Name))
                        {
                            return Prefix + Config.Instance.DefaultParameters[Name] + MU;
                        }
                        else
                        {
                            return Prefix + "-";
                        }
                    }
                    else
                    {
                        return Prefix + Value.ToString() + " " + MU;
                    }
                }
                else
                {
                    return "";
                }
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
            //return DisplayedText;
            //return Value + " " + MU;
            return Value.ToString();
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

        private double GetDefault()
        {
            switch (this.name)
            {
                case "price":
                    return Config.Instance.Material.Price;
                case "maxflow":
                    return Config.Instance.Material.FlowRateUpperBound;
                case "reqflow":
                    return Config.Instance.Material.FlowRateLowerBound;
                case "caplower":
                    return Config.Instance.OperatingUnit.CapacityLowerBound;
                case "capupper":
                    return Config.Instance.OperatingUnit.CapacityUpperBound;
                case "investcostfix":
                    return Config.Instance.OperatingUnit.InvestmentFixCost;
                case "investcostprop":
                    return Config.Instance.OperatingUnit.InvestmentPropCost;
                case "opercostfix":
                    return Config.Instance.OperatingUnit.OperatingFixCost;
                case "opercostprop":
                    return Config.Instance.OperatingUnit.OperatingPropCost;
                case "payoutperiod":
                    return Config.Instance.OperatingUnit.PayoutPeriod;
                case "workinghour":
                    return Config.Instance.OperatingUnit.WorkingHoursPerYear;
            }
            return 0;
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
        [ReadOnly(true)]
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
