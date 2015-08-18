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
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;

using PNSDraw.Configuration;

namespace PNSDraw
{
    [TypeConverter(typeof(ObjectPropertySorter))]
    public class OperatingUnit : Canvas.IGraphicsObject, Canvas.IConnectableObject
    {
        #region Core private data variables
        
        int ID;
        Point offset;

        Color color;
        bool locked = false;
        bool Selected = false;

        string name;
        string title;
        int fixed_position;

        public List<Canvas.IConnectableObject> connectedobjects;
        Canvas.IGraphicsStructure Container;

        DisplayTextStyle Label;

        DisplayTextStyle Comment;

        DisplayTextStyle Parameters;

        NodeStyle NodeStyle;

        bool Highlighted;

        Dictionary<string, ObjectProperty> parameterlist;

        string QuantityType;
        string MeasurementUnit;
        string PriceMU;

        #endregion;


        #region Private data properties
        [Browsable(false)]
        Color Color
        {
            get { return NodeStyle.ColorProp; }
            set { NodeStyle.ColorProp = value; }
        }

        [Browsable(false)]
        Point Coords
        {
            get { return NodeStyle.CoordsProp; }
            set { NodeStyle.CoordsProp = value; }
        }

        [Browsable(false)]
        bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        [Browsable(false)]
        public string Name
        {
            get { return name; }
            set
            {
                string validated = Container.ValidateName(this, value);
                name = validated;
                if (Title == "")
                {
                    Label.Text = name;
                }
            }
        }

        [Browsable(false)]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                if (title == "")
                {
                    Label.Text = Name;
                }
                else
                {
                    Label.Text = title;
                }
            }
        }

        [Browsable(false)]
        public int Fixed_position
        {
            get { return fixed_position; }
            set
            {
                fixed_position = value;
            }
        }

        [Browsable(false)]
        public string DisplayedText
        {
            get
            {
                return Label.Text;
            }
        }

        [Browsable(false)]
        public string CommentText
        {
            get
            {
                return Comment.Text;
            }

            set
            {
                if (Comment.Text == value)
                {
                    return;
                }
                Comment.Text = value;
                if (Container != null)
                {
                    if (Comment.Text == "")
                    {
                        Container.RemoveSingleGraphicsObject(Comment);
                    }
                    else
                    {
                        Container.AddSingleGraphicsObject(Comment);
                    }
                }
            }
        }

        [Browsable(false)]
        bool ParametersVisible
        {
            get
            {
                bool parametersvisible = false;
                foreach (ObjectProperty op in parameterlist.Values)
                {
                    if (op.Visible == true)
                    {
                        parametersvisible = true;
                    }
                }
                return parametersvisible;
            }
        }

        [Browsable(false)]
        public Dictionary<string, ObjectProperty> ParameterList
        {
            get
            {
                return parameterlist;
            }
        }

        #endregion

        public event EventHandler QuantityPropertyChanged;
        public event EventHandler MUPropertyChanged;
        public event EventHandler PriceMUPropertyChanged;

        public OperatingUnit(Canvas.IGraphicsStructure container)
        {
            NodeStyle = new NodeStyle(container, this);
            NodeStyle.Text = "Node style properties";
            NodeStyle.SetCoords(new Point(0, 0));

            ID = 0;
            Coords = new Point(0, 0);
            offset = new Point(0, 0);
            Color = Color.Black;
            connectedobjects = new List<Canvas.IConnectableObject>();
            Container = container;
            
            Label = new DisplayTextStyle(container, this);
            Label.Text = "OperatingUnit";
            Label.SetCoords(new Point(Config.Instance.OperatingUnitWidth / 2 + 3, -Config.Instance.OperatingUnitHeight / 2));
            
            Highlighted = false;
            Name = Container.GenerateName(this);
            Title = "";
            QuantityType = Config.Instance.Quantity.quant_type;
            MeasurementUnit = Quantity.GetTextFromSymbol(Quantity.GetDefaultMUSymbol(QuantityType)) + "/" + Config.Instance.Quantity.time_mu;
            PriceMU = Config.Instance.Quantity.money_mu + "/" + Config.Instance.Quantity.time_mu;
            UpdateListOfMUs();

            this.QuantityPropertyChanged += new EventHandler(QuantityPropChanged);
            this.MUPropertyChanged += new EventHandler(MUPropChanged);
            this.PriceMUPropertyChanged += new EventHandler(PriceMUPropChanged);

            parameterlist = new Dictionary<string, ObjectProperty>();
            Comment = new DisplayTextStyle(container, this);
            Comment.Text = "";
            Comment.SetCoords(new Point(Config.Instance.OperatingUnitWidth / 2 + 3, -Config.Instance.OperatingUnitHeight / 2 + Config.Instance.DefaultLineHeight));

            Parameters = new DisplayTextStyle(container, this);
            Parameters.Text = "";
            Parameters.SetCoords(new Point(Config.Instance.OperatingUnitWidth / 2 + 3, -Config.Instance.OperatingUnitHeight / 2 + 2 * Config.Instance.DefaultLineHeight));

            List<string> otherprops = new List<string>();
            otherprops.Add("caplower");
            otherprops.Add("capupper");
            otherprops.Add("investcostfix");
            otherprops.Add("investcostprop");
            otherprops.Add("opercostfix");
            otherprops.Add("opercostprop");
            otherprops.Add("payoutperiod");
            otherprops.Add("workinghour");
            foreach (string prop in otherprops)
            {
                ObjectProperty op = new ObjectProperty(prop, container, this);
                op.PropertyChanged += new EventHandler(ParametersPropertyChanged);
                parameterlist[prop] = op;
            }
            parameterlist["caplower"].Prefix = "Capacity, lower bound: ";
            parameterlist["caplower"].Value = -1;// Config.Instance.OperatingUnit.CapacityLowerBound;
            parameterlist["caplower"].NonValue = -1;
            parameterlist["caplower"].MU = Quantity.GetMUSymbolFromText(MeasurementUnit) + "/" + Config.Instance.Quantity.time_mu;
            parameterlist["capupper"].Prefix = "Capacity, upper bound: ";
            parameterlist["capupper"].Value = -1;// Config.Instance.OperatingUnit.CapacityUpperBound;
            parameterlist["capupper"].NonValue = -1;
            parameterlist["capupper"].MU = Quantity.GetMUSymbolFromText(MeasurementUnit) + "/" + Config.Instance.Quantity.time_mu;
            parameterlist["investcostfix"].Prefix = "Investment cost, fix: ";
            parameterlist["investcostfix"].Value = -1;//Config.Instance.OperatingUnit.InvestmentFixCost;
            parameterlist["investcostfix"].NonValue = -1;
            parameterlist["investcostfix"].MU = PriceMU;
            parameterlist["investcostprop"].Prefix = "Investment cost, proportional: ";
            parameterlist["investcostprop"].Value = -1;//Config.Instance.OperatingUnit.InvestmentPropCost;
            parameterlist["investcostprop"].NonValue = -1;
            parameterlist["investcostprop"].MU = PriceMU;
            parameterlist["opercostfix"].Prefix = "Operating cost, fix: ";
            parameterlist["opercostfix"].Value = -1;//Config.Instance.OperatingUnit.OperatingFixCost;
            parameterlist["opercostfix"].NonValue = -1;
            parameterlist["opercostfix"].MU = PriceMU;
            parameterlist["opercostprop"].Prefix = "Operating cost, proportional: ";
            parameterlist["opercostprop"].Value = -1;//Config.Instance.OperatingUnit.OperatingPropCost;
            parameterlist["opercostprop"].NonValue = -1;
            parameterlist["opercostprop"].MU = PriceMU;
            parameterlist["payoutperiod"].Prefix = "Payout period: ";
            parameterlist["payoutperiod"].Value = -1;//Config.Instance.OperatingUnit.PayoutPeriod;
            parameterlist["payoutperiod"].NonValue = -1;
            parameterlist["workinghour"].Prefix = "Working hours per year: ";
            parameterlist["workinghour"].Value = -1;//Config.Instance.OperatingUnit.WorkingHoursPerYear;
            parameterlist["workinghour"].NonValue = -1;
        }

        #region Change listeners

        void OnQuantityNotify()
        {
            if (QuantityPropertyChanged != null)
            {
                QuantityPropertyChanged(this, EventArgs.Empty);
            }
        }

        void OnMUNotify()
        {
            if (MUPropertyChanged != null)
            {
                MUPropertyChanged(this, EventArgs.Empty);
            }
        }

        void OnPriceMUNotify()
        {
            if (PriceMUPropertyChanged != null)
            {
                PriceMUPropertyChanged(this, EventArgs.Empty);
            }
        }

        void PriceMUPropChanged(object sender, EventArgs e)
        {
            parameterlist["investcostfix"].MU = PriceMU;
            parameterlist["investcostprop"].MU = PriceMU;
            parameterlist["opercostfix"].MU = PriceMU;
            parameterlist["opercostprop"].MU = PriceMU;
        }

        void MUPropChanged(object sender, EventArgs e)
        {
            parameterlist["caplower"].MU = Quantity.GetMUSymbolFromText(MeasurementUnit) + "/" + Config.Instance.Quantity.time_mu;
            parameterlist["capupper"].MU = Quantity.GetMUSymbolFromText(MeasurementUnit) + "/" + Config.Instance.Quantity.time_mu;
            ((PGraph)Container).RefreshConnectedEdgesTitle(this);
        }

        #endregion

        #region GraphicsObject interface functions

        public int GetID()
        {
            return ID;
        }

        public void Draw(Graphics g, bool plain)
        {
            if (plain == true)
            {
                DrawOperatingUnit(g, Color, Coords, Color, false);
                return;
            }
            if (Selected)
            {
                DrawOperatingUnit(g, Color, Coords, Color.Red, true);    
            }
            else if (Highlighted)
            {
                DrawOperatingUnit(g, Color, Coords, Color.Gray, true);
            }
            else
            {
                DrawOperatingUnit(g, Color, Coords, Color, false);
            }
        }

        public bool HitTest(Point mousecoords)
        {
            double dx = (double) Coords.X - mousecoords.X;
            double dy = (double) Coords.Y - mousecoords.Y;
            if ((Math.Abs(dx) < (Config.Instance.OperatingUnitWidth / 2)) && (Math.Abs(dy) < (Config.Instance.OperatingUnitHeight / 2)))
            {
                return true;
            }
            return false;
        }

        public Point GetCoords()
        {
            return Coords;
        }

        public void SetCoords(Point newcoords)
        {
            Coords = newcoords;
        }

        public void SetID(int newid)
        {
            ID = newid;
            if (Name == "")
            {
                NameProp = Container.GenerateName(this);
            }
        }

        public void SetSelected(bool selected)
        {
            Selected = selected;
        }

        public bool IsSelected()
        {
            return Selected;
        }

        public void SetOffset(Point newoffset)
        {
            offset = newoffset;
        }

        public void IntegrateOffset()
        {
            Coords += (Size)offset;
            offset.X = 0;
            offset.Y = 0;
        }

        public void Pin(int fp)
        {
            fixed_position = fp;
            if (fp == 1)
            {
                color = Color.Brown;
            }
            else
            {
                color = Color.Black;
            }
        }

        public int getPin()
        {
            return fixed_position;
        }

        public bool IntersectsWith(Rectangle rect)
        {
            rect.Inflate(Config.Instance.OperatingUnitWidth / 2, Config.Instance.OperatingUnitHeight / 2);
            return rect.Contains(Coords);
        }

        public int GetLayer()
        {
            return 2;
        }

        public Point GetCurrentCoords()
        {
            Point cur = (Point)((Size)Coords + (Size)offset);
            return cur;
        }

        public Point GetOffset()
        {
            return offset;
        }

        public void DrawGhost(Graphics g)
        {
            Color color = Color.Silver;
            if (offset.IsEmpty == false)
            {
                DrawOperatingUnit(g, color, GetCurrentCoords(), color, false);
            }
        }

        public void SetEditSelected(bool selected)
        {

        }

        public bool IsPartialObject()
        {
            return false;
        }

        public Canvas.IGraphicsObject GetParentObject()
        {
            return this;
        }

        public void SetHighlighted(bool highlighted)
        {
            Highlighted = highlighted;
        }

        public bool IsHighlighted()
        {
            return Highlighted;
        }


        public bool IsLocked()
        {
            return Locked;
        }



        public Rectangle GetBoundary()
        {
            Size halfopsize = new Size(Config.Instance.OperatingUnitWidth / 2, Config.Instance.OperatingUnitHeight / 2);
            return new Rectangle(Coords - halfopsize, new Size(Config.Instance.OperatingUnitWidth, Config.Instance.OperatingUnitHeight));
        }


        public void SetSelectedChild(Canvas.IGraphicsObject child)
        {
            Selected = false;
        }


        public bool IsMoveable()
        {
            return true;
        }

        public bool IsDeletable()
        {
            return true;
        }

        #endregion

        #region ConnectableObject interface functions

        public Point GetConnectorBeginCoords()
        {
            Point conn = GetCoords();
            // conn.Y += Globals.OperatingUnitHeight / 2 - 2;
            return conn;
        }

        public Point GetConnectorEndCoords()
        {
            Point conn = GetCoords();
            // conn.Y -= Globals.OperatingUnitHeight / 2 - 2;
            return conn;
        }

        public Point GetCurrentConnectorBeginCoords()
        {
            Point conn = GetCurrentCoords();
            // conn.Y += Globals.OperatingUnitHeight / 2 - 2;
            return conn;
        }

        public Point GetCurrentConnectorEndCoords()
        {
            Point conn = GetCurrentCoords();
            // conn.Y -= Globals.OperatingUnitHeight / 2 - 2;
            return conn;
        }



        public bool IsValidConnectorBegin(Canvas.IConnectableObject end)
        {
            if (end == null)
            {
                return true;
            }
            else if (end is Material)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidConnectorEnd(Canvas.IConnectableObject begin)
        {
            if (connectedobjects.Contains(begin))
            {
                return false;
            }
            if (begin == null)
            {
                return true;
            }
            else if (begin is Material)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddConnection(Canvas.IConnectableObject begin)
        {
            if (connectedobjects.Contains(begin) == false)
            {
                connectedobjects.Add(begin);
            }
        }

        public void RemoveConnection(Canvas.IConnectableObject begin)
        {
            if (connectedobjects.Contains(begin) == true)
            {
                connectedobjects.Remove(begin);
            }
        }

        #endregion

        #region Other functions

        public void OnAddToGraph()
        {
            Container.AddSingleGraphicsObject(this);
            Container.AddSingleGraphicsObject(this.Label);
            if (CommentText != "")
            {
                Container.AddSingleGraphicsObject(this.Comment);
            }
            if (ParametersVisible)
            {
                Container.AddSingleGraphicsObject(this.Parameters);
            }
        }

        public void OnRemoveFromGraph()
        {
            Container.RemoveSingleGraphicsObject(this);
            Container.RemoveSingleGraphicsObject(this.Label);
            Container.RemoveSingleGraphicsObject(this.Comment);
            Container.RemoveSingleGraphicsObject(this.Parameters);
        }

        public string ConvertToSVG(int zorder)
        {
            string svg = "";

            Point tmp = new Point(Coords.X - Config.Instance.OperatingUnitWidth / 2,
                Coords.Y - Config.Instance.OperatingUnitHeight / 2);
            Rectangle rect = new Rectangle(tmp, new Size(Config.Instance.OperatingUnitWidth, Config.Instance.OperatingUnitHeight));

            svg = String.Format("<rect x=\"{0}\" y=\"{1}\" width=\"{2}\" height=\"{3}\" style=\"fill: {4};\" />\n",
                ConvertManager.ToString(rect.X / 5),
                ConvertManager.ToString(rect.Y / 5),
                ConvertManager.ToString(rect.Width / 5),
                ConvertManager.ToString(rect.Height / 5),
                ConvertManager.ToHtml(Color));

            svg += Label.ConvertToSVG(zorder);
            if (CommentText != "")
            {
                svg += Comment.ConvertToSVG(zorder);
            }
            if (ParametersVisible)
            {
                svg += Parameters.ConvertToSVG(zorder);
            }

            return svg;
        }

        public void DrawOperatingUnit(Graphics g, Color c, Point mcoords, Color line, bool drawstroke)
        {
            SolidBrush brush = new SolidBrush(c);
            Pen p = new Pen(new SolidBrush(line), Config.Instance.LineSize);

            Point tmp = new Point(mcoords.X - Config.Instance.OperatingUnitWidth / 2,
                mcoords.Y - Config.Instance.OperatingUnitHeight / 2);
            Rectangle rect = new Rectangle(tmp, new Size(Config.Instance.OperatingUnitWidth, Config.Instance.OperatingUnitHeight));

            g.FillRectangle(brush, rect);
            if (drawstroke)
            {
                rect.X -= 20;
                rect.Y -= 20;
                rect.Width += 40;
                rect.Height += 40;
                g.DrawRectangle(p, rect);
            }
        }

        void ParametersPropertyChanged(object sender, EventArgs e)
        {
            UpdateParametersLabel();
        }

        void QuantityPropChanged(object sender, EventArgs e)
        {
            UpdateListOfMUs();
            switch (QuantityType)
            {
                case "Mass":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.mass_mu);
                    break;
                case "Volume":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.vol_mu);
                    break;
                case "Amount of substance":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.sub_mu);
                    break;
                case "Energy, work, heat":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.energy_mu);
                    break;
                case "Length":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.length_mu);
                    break;
                case "Electric current":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.curr_mu);
                    break;
                case "Area":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.area_mu);
                    break;
                case "Speed":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.speed_mu);
                    break;
                case "Acceleration":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.acc_mu);
                    break;
                case "Mass density":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.mdens_mu);
                    break;
                case "Thermodinamic temperature":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.temp_mu);
                    break;
                case "Luminous intensity":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.lum_mu);
                    break;
                case "Concentration":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.conc_mu);
                    break;
                case "Force":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.force_mu);
                    break;
                case "Pressure":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.press_mu);
                    break;
                case "Power":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.power_mu);
                    break;
                case "Capacity":
                    MeasurementUnitProp = Quantity.GetTextFromSymbol(Config.Instance.Quantity.cap_mu);
                    break;
                default:
                    break;
            }
        }

        public void UpdateParametersLabel()
        {
            Container.RemoveSingleGraphicsObject(Parameters);
            if (ParametersVisible)
            {
                Container.AddSingleGraphicsObject(Parameters);
                Parameters.Text = "";
                foreach (ObjectProperty op in parameterlist.Values)
                {
                    if (op.Visible)
                    {
                        Parameters.Text += op.DisplayedText + Environment.NewLine;
                    }
                }
            }
        }

        private void UpdateListOfMUs()
        {
            MeasurementUnits.listOfMUs = new string[Quantity.GetListOfMUs(QuantityType).Count];
            MeasurementUnits.listOfMUs = Quantity.GetListOfMUs(QuantityType).ToArray();
        }

        #endregion

        #region PropertyGrid parameter properties

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(10)]
        [DisplayName("Type")]
        [ReadOnly(true)]
        public string TypeProp
        {
            get
            {
                return "Operating Unit";
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(20)]
        [DisplayName("Name")]
        public string NameProp
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
                ((PGraph)Container).RefreshConnectedEdgesTitle(this);
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(30)]
        [DisplayName("Capacity - lower bound:")]
        public ObjectProperty CapacityLowerProp
        {
            get
            {
                if (parameterlist.ContainsKey("caplower"))
                {
                    return parameterlist["caplower"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (parameterlist.ContainsKey("caplower"))
                {
                    parameterlist.Remove("caplower");
                }
                parameterlist["caplower"] = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(40)]
        [DisplayName("Capacity - upper bound:")]
        public ObjectProperty CapacityUpperProp
        {
            get
            {
                if (parameterlist.ContainsKey("capupper"))
                {
                    return parameterlist["capupper"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (parameterlist.ContainsKey("capupper"))
                {
                    parameterlist.Remove("capupper");
                }
                parameterlist["capupper"] = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(50)]
        [DisplayName("Investment cost - fix:")]
        public ObjectProperty InvestmentCostFixProp
        {
            get
            {
                if (parameterlist.ContainsKey("investcostfix"))
                {
                    return parameterlist["investcostfix"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (parameterlist.ContainsKey("investcostfix"))
                {
                    parameterlist.Remove("investcostfix");
                }
                parameterlist["investcostfix"] = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(60)]
        [DisplayName("Investment cost - proportional:")]
        public ObjectProperty InvestmentCostPropProp
        {
            get
            {
                if (parameterlist.ContainsKey("investcostprop"))
                {
                    return parameterlist["investcostprop"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (parameterlist.ContainsKey("investcostprop"))
                {
                    parameterlist.Remove("investcostprop");
                }
                parameterlist["investcostprop"] = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(70)]
        [DisplayName("Operating cost - fix:")]
        public ObjectProperty OperatingCostFixProp
        {
            get
            {
                if (parameterlist.ContainsKey("opercostfix"))
                {
                    return parameterlist["opercostfix"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (parameterlist.ContainsKey("opercostfix"))
                {
                    parameterlist.Remove("opercostfix");
                }
                parameterlist["opercostfix"] = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(80)]
        [DisplayName("Operating cost - proportional:")]
        public ObjectProperty OperatingCostPropProp
        {
            get
            {
                if (parameterlist.ContainsKey("opercostprop"))
                {
                    return parameterlist["opercostprop"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (parameterlist.ContainsKey("opercostprop"))
                {
                    parameterlist.Remove("opercostprop");
                }
                parameterlist["opercostprop"] = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(90)]
        [DisplayName("Payout period:")]
        public ObjectProperty PayoutPeriodProp
        {
            get
            {
                if (parameterlist.ContainsKey("payoutperiod"))
                {
                    return parameterlist["payoutperiod"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (parameterlist.ContainsKey("payoutperiod"))
                {
                    parameterlist.Remove("payoutperiod");
                }
                parameterlist["payoutperiod"] = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(100)]
        [DisplayName("Working hour per year:")]
        public ObjectProperty WorkingHourProp
        {
            get
            {
                if (parameterlist.ContainsKey("workinghour"))
                {
                    return parameterlist["workinghour"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (parameterlist.ContainsKey("workinghour"))
                {
                    parameterlist.Remove("workinghour");
                }
                parameterlist["workinghour"] = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(110)]
        [DisplayName("Quantity Type")]
        [TypeConverter(typeof(QuantityTypeConverter))]
        public string QuantityTypeProp
        {
            get
            {
                return QuantityType;
            }
            set
            {
                QuantityType = value;
                OnQuantityNotify();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(120)]
        [DisplayName("Measurement Unit")]
        [TypeConverter(typeof(MUConverter))]
        public string MeasurementUnitProp
        {
            get
            {
                return MeasurementUnit;
            }
            set
            {
                MeasurementUnit = value + "/" + Config.Instance.Quantity.time_mu;
                OnMUNotify();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(130)]
        [DisplayName("Price unit")]
        [TypeConverter(typeof(PriceMUTypeConverter))]
        public string PriceMUProp
        {
            get
            {
                return PriceMU;
            }
            set
            {
                PriceMU = value + "/" + Config.Instance.Quantity.time_mu;
                OnPriceMUNotify();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(140)]
        [DisplayName("Comment")]
        [Editor(typeof(MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string CommentTextProp
        {
            get
            {
                return CommentText;
            }
            set
            {
                CommentText = value;

            }
        }

        #endregion

        #region PropertyGrid style properties

        [Browsable(true)]
        [Category("\tStyle"), PropertyOrder(10)]
        [DisplayName("Node")]
        public NodeStyle NodeStyleProp
        {
            get
            {
                return NodeStyle;
            }
            set
            {
                NodeStyle = value;
            }
        }

        [Browsable(true)]
        [Category("\tStyle"), PropertyOrder(40)]
        [DisplayName("Parameters")]
        public DisplayTextStyle ParametersProp
        {
            get
            {
                return Parameters;
            }
            set
            {
                Parameters = value;
            }

        }

        [Browsable(true)]
        [Category("\tStyle"), PropertyOrder(20)]
        [DisplayName("Label")]
        public DisplayTextStyle LabelProp
        {
            get
            {
                return Label;
            }
            set
            {
                Label = value;
            }

        }

        [Browsable(true)]
        [Category("\tStyle"), PropertyOrder(30)]
        [DisplayName("Comment")]
        public DisplayTextStyle CommentProp
        {
            get
            {
                return Comment;
            }
            set
            {
                Comment = value;
            }

        }

        #endregion
    }
}
