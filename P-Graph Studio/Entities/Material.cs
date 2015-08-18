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
using System.Windows.Forms;

namespace PNSDraw
{

    [TypeConverter(typeof(ObjectPropertySorter))]
    public class Material : Canvas.IGraphicsObject, Canvas.IConnectableObject
    {
        #region Core private data variables

        int ID;
        //Point coords;
        Point offset;

        int type;
        //Color color;
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

        #endregion

        public Material(Canvas.IGraphicsStructure container)
        {
            NodeStyle = new NodeStyle(container, this);
            NodeStyle.Text = "Node style properties";
            NodeStyle.SetCoords(new Point(0, 0));

            ID = 0;
            Coords = new Point(0, 0);
            offset = new Point(0, 0);
            type = MaterialTypes.Raw;
            Color = Color.Black;
            connectedobjects = new List<Canvas.IConnectableObject>();
            Container = container;

            Label = new DisplayTextStyle(container, this);
            Label.Text = "Material";
            Label.SetCoords(new Point(Config.Instance.MaterialSize + 3, -Config.Instance.MaterialSize));

            Highlighted = false;
            Name = container.GenerateName(this);
            Title = "";
            QuantityType = Config.Instance.Quantity.quant_type;
            MeasurementUnit = Quantity.GetTextFromSymbol(Quantity.GetDefaultMUSymbol(QuantityType));
            UpdateListOfMUs();
            PriceMU = Config.Instance.Quantity.money_mu.ToString() + "/" + Config.Instance.Quantity.time_mu;

            this.QuantityPropertyChanged += new EventHandler(QuantityPropChanged);
            this.MUPropertyChanged += new EventHandler(MUPropChanged);
            this.PriceMUPropertyChanged += new EventHandler(PriceMUPropChanged);

            parameterlist = new Dictionary<string, ObjectProperty>();

            Comment = new DisplayTextStyle(container, this);
            Comment.Text = "";
            Comment.SetCoords(new Point(Config.Instance.MaterialSize + 3, -Config.Instance.MaterialSize + Config.Instance.DefaultLineHeight));

            Parameters = new DisplayTextStyle(container, this);
            Parameters.Text = "";
            Parameters.SetCoords(new Point(Config.Instance.MaterialSize + 3, -Config.Instance.MaterialSize + 2 * Config.Instance.DefaultLineHeight));

            List<string> otherprops = new List<string>();
            otherprops.Add("price");
            otherprops.Add("reqflow");
            otherprops.Add("maxflow");
            foreach (string prop in otherprops)
            {
                ObjectProperty op = new ObjectProperty(prop, container, this);
                op.PropertyChanged += new EventHandler(ParametersPropertyChanged);
                parameterlist[prop] = op;
            }
            parameterlist["price"].Prefix = "Price: ";
            parameterlist["price"].Value = -1;//Config.Instance.Material.Price;
            parameterlist["price"].NonValue = -1;
            parameterlist["price"].MU = PriceMU;
            parameterlist["reqflow"].Prefix = "Required flow: ";
            parameterlist["reqflow"].Value = -1;//Config.Instance.Material.FlowRateLowerBound;
            parameterlist["reqflow"].NonValue = -1;
            parameterlist["reqflow"].MU = Quantity.GetMUSymbolFromText(MeasurementUnit) + "/" + Config.Instance.Quantity.time_mu;
            parameterlist["maxflow"].Prefix = "Maximum flow: ";
            parameterlist["maxflow"].Value = -1;// Config.Instance.Material.FlowRateUpperBound;
            parameterlist["maxflow"].NonValue = -1;
            parameterlist["maxflow"].MU = Quantity.GetMUSymbolFromText(MeasurementUnit) + "/" + Config.Instance.Quantity.time_mu;
        }


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

        #endregion

        #region Public data properties

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
                ((PGraph)Container).RefreshConnectedEdgesTitle(this);
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
        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
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

        #region GrpahicsObject interface functions

        public int GetID()
        {
            return ID;
        }

        public void Draw(Graphics g, bool plain)
        {
            if (plain == true)
            {
                DrawMaterial(g, Color, Coords, Color, false);
            }
            else if (Selected)
            {
                DrawMaterial(g, Color, Coords, Color.Red, true);
            }
            else if (Highlighted)
            {
                DrawMaterial(g, Color, Coords, Color.Gray, true);
            }
            else
            {
                DrawMaterial(g, Color, Coords, Color, false);
            }
        }

        public bool HitTest(Point mousecoords)
        {
            double dx = (double)Coords.X - mousecoords.X;
            double dy = (double)Coords.Y - mousecoords.Y;
            if (Math.Round(Math.Sqrt(dx * dx + dy * dy)) < Config.Instance.MaterialSize)
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
            Coords = Coords + (Size)offset;
            offset.X = 0;
            offset.Y = 0;
        }

        public void Pin(int fp)
        {
            fixed_position = fp;
            if (fp == 1)
            {
                NodeStyleProp.ColorProp = Color.Brown;
            }
            else
            {
                NodeStyleProp.ColorProp = Color.Black;
            }
        }
        public int getPin()
        {
            return fixed_position;
        }

        public bool IntersectsWith(Rectangle rect)
        {
            rect.Inflate(Config.Instance.MaterialSize, Config.Instance.MaterialSize);
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

        public void DrawGhost(Graphics g)
        {
            Color color = Color.Silver;
            if (offset.IsEmpty == false)
            {
                DrawMaterial(g, color, GetCurrentCoords(), color, false);
            }
        }

        public Point GetOffset()
        {
            return offset;
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
            Size halfmatsize = new Size(Config.Instance.MaterialSize, Config.Instance.MaterialSize);
            return new Rectangle(Coords - halfmatsize, new Size(Config.Instance.MaterialSize * 2, Config.Instance.MaterialSize * 2));
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
            return GetCoords();
        }

        public Point GetConnectorEndCoords()
        {
            return GetCoords();
        }

        public Point GetCurrentConnectorBeginCoords()
        {
            return GetCurrentCoords();
        }


        public Point GetCurrentConnectorEndCoords()
        {
            return GetCurrentCoords();
        }

        public bool IsValidConnectorBegin(Canvas.IConnectableObject end)
        {
            if (end == null)
            {
                return true;
            }
            else if (end is OperatingUnit)
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
            else if (begin is OperatingUnit)
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
            parameterlist["price"].MU = PriceMU;
        }

        void MUPropChanged(object sender, EventArgs e)
        {
            parameterlist["reqflow"].MU = Quantity.GetMUSymbolFromText(MeasurementUnit) + "/" + Config.Instance.Quantity.time_mu;
            parameterlist["maxflow"].MU = Quantity.GetMUSymbolFromText(MeasurementUnit) + "/" + Config.Instance.Quantity.time_mu;
            ((PGraph)Container).RefreshConnectedEdgesTitle(this);
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

        private void UpdateListOfMUs()
        {
            MeasurementUnits.listOfMUs = new string[Quantity.GetListOfMUs(QuantityType).Count];
            MeasurementUnits.listOfMUs = Quantity.GetListOfMUs(QuantityType).ToArray();
        }

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
            if (type == MaterialTypes.Intermediate)
            {
                svg = String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"{3}\" />\n",
                    ConvertManager.ToString(Coords.X / 5),
                    ConvertManager.ToString(Coords.Y / 5),
                    ConvertManager.ToString(Config.Instance.MaterialSize / 5),
                    ConvertManager.ToHtml(Color));
            }
            else if (type == MaterialTypes.Product)
            {
                svg = "<g>\n";
                svg += String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"{3}\" />\n",
                    ConvertManager.ToString(Coords.X / 5),
                    ConvertManager.ToString(Coords.Y / 5),
                    ConvertManager.ToString(Config.Instance.MaterialSize / 5),
                    ConvertManager.ToHtml(Color));
                svg += String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"white\" />\n",
                    ConvertManager.ToString(Coords.X / 5),
                    ConvertManager.ToString(Coords.Y / 5),
                    ConvertManager.ToString(Config.Instance.MaterialSize / 5 * 3 / 4));
                svg += String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"{3}\" />\n",
                    ConvertManager.ToString(Coords.X / 5),
                    ConvertManager.ToString(Coords.Y / 5),
                    ConvertManager.ToString(Config.Instance.MaterialSize / 5 * 1 / 2),
                    ConvertManager.ToHtml(Color));
                svg += String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"white\" />\n",
                    ConvertManager.ToString(Coords.X / 5),
                    ConvertManager.ToString(Coords.Y / 5),
                    ConvertManager.ToString(Config.Instance.MaterialSize / 5 * 1 / 4));
                svg += "</g>\n";
            }
            else if (type == MaterialTypes.Raw)
            {
                Point tmp = new Point();

                List<Point> tps = new List<Point>();
                tmp = new Point();
                tmp.X = Coords.X + offset.X - (int)Math.Round(Math.Cos(Math.PI / 6) * Config.Instance.MaterialSize);
                tmp.Y = Coords.Y + offset.Y - (int)Math.Round(Math.Sin(Math.PI / 6) * Config.Instance.MaterialSize);
                tps.Add(tmp);

                tmp = new Point();
                tmp.X = Coords.X + offset.X - (int)Math.Round(Math.Cos(Math.PI / 6 * 5) * Config.Instance.MaterialSize);
                tmp.Y = Coords.Y + offset.Y - (int)Math.Round(Math.Sin(Math.PI / 6 * 5) * Config.Instance.MaterialSize);
                tps.Add(tmp);

                tmp = new Point();
                tmp.X = Coords.X + offset.X;
                tmp.Y = Coords.Y + offset.Y + Config.Instance.MaterialSize;
                tps.Add(tmp);

                svg = "<g>\n";
                svg += String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"{3}\" />\n",
                    ConvertManager.ToString(Coords.X / 5),
                    ConvertManager.ToString(Coords.Y / 5),
                    ConvertManager.ToString(Config.Instance.MaterialSize / 5),
                    ConvertManager.ToHtml(Color));
                svg += String.Format("<polygon points=\"{0},{1} {2},{3} {4},{5}\" fill=\"white\" />\n",
                    ConvertManager.ToString(tps[0].X / 5),
                    ConvertManager.ToString(tps[0].Y / 5),
                    ConvertManager.ToString(tps[1].X / 5),
                    ConvertManager.ToString(tps[1].Y / 5),
                    ConvertManager.ToString(tps[2].X / 5),
                    ConvertManager.ToString(tps[2].Y / 5));
                svg += "</g>\n";
            }

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

        private void DrawMaterial(Graphics g, Color c, Point mcoords, Color line, bool drawstroke)
        {

            if (c == Color.Transparent)
            {
                return;
            }

            SolidBrush brush = new SolidBrush(c);
            SolidBrush whitebrush = new SolidBrush(Color.White);
            Pen p = new Pen(new SolidBrush(line), Config.Instance.LineSize);

            Point tmp = new Point(mcoords.X - Config.Instance.MaterialSize,
                mcoords.Y - Config.Instance.MaterialSize);
            Rectangle rect = new Rectangle(tmp, new Size(2 * Config.Instance.MaterialSize, 2 * Config.Instance.MaterialSize));


            if (type == MaterialTypes.Intermediate)
            {
                g.FillEllipse(brush, rect);
            }
            else if (type == MaterialTypes.Product)
            {
                g.FillEllipse(brush, rect);
                Rectangle rect2 = new Rectangle(rect.Location.X + Config.Instance.MaterialSize / 4,
                    rect.Location.Y + Config.Instance.MaterialSize / 4,
                    Config.Instance.MaterialSize * 3 / 2,
                    Config.Instance.MaterialSize * 3 / 2);
                g.FillEllipse(whitebrush, rect2);

                Rectangle rect3 = new Rectangle(rect.Location.X + Config.Instance.MaterialSize / 2,
                    rect.Location.Y + Config.Instance.MaterialSize / 2,
                    Config.Instance.MaterialSize,
                    Config.Instance.MaterialSize);
                g.FillEllipse(brush, rect3);

                Rectangle rect4 = new Rectangle(rect.Location.X + Config.Instance.MaterialSize * 3 / 4,
                    rect.Location.Y + Config.Instance.MaterialSize * 3 / 4,
                    Config.Instance.MaterialSize / 2,
                    Config.Instance.MaterialSize / 2);
                g.FillEllipse(whitebrush, rect4);
            }
            else if (type == MaterialTypes.Raw)
            {
                g.FillEllipse(brush, rect);
                List<Point> trianglepoints = new List<Point>();
                tmp = new Point();
                tmp.X = mcoords.X - (int)Math.Round(Math.Cos(Math.PI / 6) * Config.Instance.MaterialSize);
                tmp.Y = mcoords.Y - (int)Math.Round(Math.Sin(Math.PI / 6) * Config.Instance.MaterialSize);
                trianglepoints.Add(tmp);

                tmp = new Point();
                tmp.X = mcoords.X - (int)Math.Round(Math.Cos(Math.PI / 6 * 5) * Config.Instance.MaterialSize);
                tmp.Y = mcoords.Y - (int)Math.Round(Math.Sin(Math.PI / 6 * 5) * Config.Instance.MaterialSize);
                trianglepoints.Add(tmp);

                tmp = new Point();
                tmp.X = mcoords.X;
                tmp.Y = mcoords.Y + Config.Instance.MaterialSize;
                trianglepoints.Add(tmp);

                if (Color == Color.Red)
                {
                    Console.WriteLine(trianglepoints[0].ToString());
                }
                g.FillPolygon(whitebrush, trianglepoints.ToArray());
            }
            if (drawstroke)
            {
                rect.X -= 20;
                rect.Y -= 20;
                rect.Width += 40;
                rect.Height += 40;
                g.DrawEllipse(p, rect);
            }
        }

        void ParametersPropertyChanged(object sender, EventArgs e)
        {
            UpdateParametersLabel();
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
                if (type == MaterialTypes.Raw)
                {
                    return "Raw Material";
                }
                else if (type == MaterialTypes.Intermediate)
                {
                    return "Intermediate Material";
                }
                else if (type == MaterialTypes.Product)
                {
                    return "Product Material";
                }
                else
                {
                    return "Unknown";
                }
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
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(30)]
        [DisplayName("Price")]
        public ObjectProperty PriceProp
        {
            get
            {
                if (parameterlist.ContainsKey("price"))
                {
                    return parameterlist["price"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (parameterlist.ContainsKey("price"))
                {
                    parameterlist.Remove("price");
                }
                parameterlist["price"] = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(40)]
        [DisplayName("Req. flow")]
        public ObjectProperty ReqFlowProp
        {
            get
            {
                if (parameterlist.ContainsKey("reqflow"))
                {
                    return parameterlist["reqflow"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (parameterlist.ContainsKey("reqflow"))
                {
                    parameterlist.Remove("reqflow");
                }
                parameterlist["reqflow"] = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(50)]
        [DisplayName("Max. flow")]
        public ObjectProperty MaxFlowProp
        {
            get
            {
                if (parameterlist.ContainsKey("maxflow"))
                {
                    return parameterlist["maxflow"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (parameterlist.ContainsKey("maxflow"))
                {
                    parameterlist.Remove("maxflow");
                }
                parameterlist["maxflow"] = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(60)]
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
        [Category("\t\tParameters"), PropertyOrder(70)]
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
                MeasurementUnit = value;
                OnMUNotify();
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(80)]
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
        [Category("\t\tParameters"), PropertyOrder(90)]
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
