using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace PNSDraw
{

    [TypeConverter(typeof(PropertySorter))]
    public class Material : Canvas.IGraphicsObject, Canvas.IConnectableObject
    {
        #region Core private data variables

        int ID;
        Point coords;
        Point offset;
        
        int type;
        Color color;
        bool locked = false;
        bool Selected = false;

        string name;
        string title;
        int fixed_position;

        public List<Canvas.IConnectableObject> connectedobjects;
        Canvas.IGraphicsStructure Container;
    
        TextObject Label;

        TextObject Comment;

        TextObject Parameters;

        bool Highlighted;

        Dictionary<string, ObjectProperty> parameterlist;

        string QuantityType;
        string MeasurementUnit;
        string PriceMU;

        #endregion


        #region Private data properties
        [Browsable(false)]
        Color Color
        {
            get { return color; }
            set { color = value; }
        }

        [Browsable(false)]
        Point Coords
        {
            get { return coords; }
            set { coords = value; }
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
            set {
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
        Point LabelOffset
        {
            get
            {
                return Label.OffsetProp;
            }
            set
            {
                Label.OffsetProp = value;
            }
        }

        [Browsable(false)]
        public Color LabelColor
        {
            get
            {
                return Label.ColorProp;
            }
            set
            {
                Label.ColorProp = value;
            }
        }

        [Browsable(false)]
        public int LabelFontSize
        {
            get
            {
                return Label.FontSize;
            }
            set
            {
                Label.FontSize = value;
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
        public int CommentFontSize
        {
            get
            {
                return Comment.FontSize;
            }
            set
            {
                Comment.FontSize = value;
            }
        }

        [Browsable(false)]
        public int ParametersFontSize
        {
            get
            {
                return Parameters.FontSize;
            }
            set
            {
                Parameters.FontSize = value;
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

        public Material(Canvas.IGraphicsStructure container)
        {
            ID = 0;
            Coords = new Point(0, 0);
            offset = new Point(0, 0);
            type = Globals.MaterialTypes.Raw;
            Color = Color.Black;
            connectedobjects = new List<Canvas.IConnectableObject>();
            Container = container;
            Label = new TextObject(container, this);
            Label.Text = "Material";
            Label.SetCoords(new Point (Globals.MaterialSize +3, -Globals.MaterialSize));
            Highlighted = false;
            Name = container.GenerateName(this);
            Title = "";
            QuantityType = Default.quant_type;
            MeasurementUnit = Default.GetTextFromSymbol(Default.GetDefaultMUSymbol(QuantityType));
            UpdateListOfMUs();
            PriceMU = Default.money_mu.ToString() + "/" + Default.time_mu;

            this.QuantityPropertyChanged += new EventHandler(QuantityPropChanged);
            this.MUPropertyChanged += new EventHandler(MUPropChanged);
            this.PriceMUPropertyChanged += new EventHandler(PriceMUPropChanged);
            
            parameterlist = new Dictionary<string, ObjectProperty>();
            Comment = new TextObject(container, this);
            Comment.Text = "";
            Comment.SetCoords(new Point(Globals.MaterialSize + 3, -Globals.MaterialSize + Globals.DefaultLineHeight));

            Parameters = new TextObject(container, this);
            Parameters.Text = "";
            Parameters.SetCoords(new Point(Globals.MaterialSize + 3, -Globals.MaterialSize + 2*Globals.DefaultLineHeight));
            
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
            parameterlist["price"].Value = Default.price;
            parameterlist["price"].NonValue = -1;
            parameterlist["price"].MU = PriceMU;
            parameterlist["reqflow"].Prefix = "Required flow: ";
            parameterlist["reqflow"].Value = Default.flow_rate_lower_bound;
            parameterlist["reqflow"].NonValue = -1;
            parameterlist["reqflow"].MU = Default.GetMUSymbolFromText(MeasurementUnit) + "/" + Default.time_mu;
            parameterlist["maxflow"].Prefix = "Maximum flow: ";
            parameterlist["maxflow"].Value = Default.flow_rate_upper_bound;
            parameterlist["maxflow"].NonValue = -1;
            parameterlist["maxflow"].MU = Default.GetMUSymbolFromText(MeasurementUnit) + "/" + Default.time_mu;
            
        }

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
            if (Math.Round(Math.Sqrt(dx * dx + dy * dy)) < Globals.MaterialSize)
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
            rect.Inflate(Globals.MaterialSize, Globals.MaterialSize);
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
            Size halfmatsize = new Size(Globals.MaterialSize, Globals.MaterialSize);
            return new Rectangle(Coords - halfmatsize, new Size(Globals.MaterialSize * 2, Globals.MaterialSize * 2));
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
            parameterlist["reqflow"].MU = Default.GetMUSymbolFromText(MeasurementUnit) + "/" + Default.time_mu;
            parameterlist["maxflow"].MU = Default.GetMUSymbolFromText(MeasurementUnit) + "/" + Default.time_mu;
        }

        void QuantityPropChanged(object sender, EventArgs e)
        {
            UpdateListOfMUs();
            Console.WriteLine("Ide írtam: " + sender.ToString());
            switch (QuantityType)
            {
                case "Mass":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.mass_mu);
                    break;
                case "Volume":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.vol_mu);
                    break;
                case "Amount of substance":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.sub_mu);
                    break;
                case "Energy, work, heat":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.energy_mu);
                    break;
                case "Length":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.length_mu);
                    break;
                case "Electric current":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.curr_mu);
                    break;
                case "Area":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.area_mu);
                    break;
                case "Speed":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.speed_mu);
                    break;
                case "Acceleration":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.acc_mu);
                    break;
                case "Mass density":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.mdens_mu);
                    break;
                case "Thermodinamic temperature":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.temp_mu);
                    break;
                case "Luminous intensity":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.lum_mu);
                    break;
                case "Concentration":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.conc_mu);
                    break;
                case "Force":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.force_mu);
                    break;
                case "Pressure":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.press_mu);
                    break;
                case "Power":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.power_mu);
                    break;
                case "Capacity":
                    MeasurementUnitProp = Default.GetTextFromSymbol(Default.cap_mu);
                    break;
                default:
                    break;
            }
        }

        private void UpdateListOfMUs()
        {
            MeasurementUnits.listOfMUs = new string[Default.GetListOfMUs(QuantityType).Count];
            MeasurementUnits.listOfMUs = Default.GetListOfMUs(QuantityType).ToArray();
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
            if (type == Globals.MaterialTypes.Intermediate)
            {
                svg = String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"{3}\" />\n", 
                    ConvertManager.ToString(Coords.X/5), 
                    ConvertManager.ToString(Coords.Y/5), 
                    ConvertManager.ToString(Globals.MaterialSize/5),
                    ConvertManager.ToHtml(Color));
            }
            else if (type == Globals.MaterialTypes.Product)
            {
                svg = "<g>\n";
                svg += String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"{3}\" />\n", 
                    ConvertManager.ToString(Coords.X / 5), 
                    ConvertManager.ToString(Coords.Y / 5),
                    ConvertManager.ToString(Globals.MaterialSize / 5),
                    ConvertManager.ToHtml(Color));
                svg += String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"white\" />\n", 
                    ConvertManager.ToString(Coords.X / 5), 
                    ConvertManager.ToString(Coords.Y / 5), 
                    ConvertManager.ToString(Globals.MaterialSize / 5 *3/4));
                svg += String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"{3}\" />\n", 
                    ConvertManager.ToString(Coords.X / 5), 
                    ConvertManager.ToString(Coords.Y / 5), 
                    ConvertManager.ToString(Globals.MaterialSize / 5 * 1/2),
                    ConvertManager.ToHtml(Color));
                svg += String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"white\" />\n", 
                    ConvertManager.ToString(Coords.X / 5), 
                    ConvertManager.ToString(Coords.Y / 5), 
                    ConvertManager.ToString(Globals.MaterialSize / 5 * 1 / 4));
                svg += "</g>\n";
            }
            else if (type == Globals.MaterialTypes.Raw)
            {
                Point tmp = new Point();

                List<Point> tps = new List<Point>();
                tmp = new Point();
                tmp.X = Coords.X + offset.X - (int)Math.Round(Math.Cos(Math.PI / 6) * Globals.MaterialSize);
                tmp.Y = Coords.Y + offset.Y - (int)Math.Round(Math.Sin(Math.PI / 6) * Globals.MaterialSize);
                tps.Add(tmp);

                tmp = new Point();
                tmp.X = Coords.X + offset.X - (int)Math.Round(Math.Cos(Math.PI / 6 * 5) * Globals.MaterialSize);
                tmp.Y = Coords.Y + offset.Y - (int)Math.Round(Math.Sin(Math.PI / 6 * 5) * Globals.MaterialSize);
                tps.Add(tmp);

                tmp = new Point();
                tmp.X = Coords.X + offset.X;
                tmp.Y = Coords.Y + offset.Y + Globals.MaterialSize;
                tps.Add(tmp);

                svg = "<g>\n";
                svg += String.Format("<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" fill=\"{3}\" />\n", 
                    ConvertManager.ToString(Coords.X / 5), 
                    ConvertManager.ToString(Coords.Y / 5), 
                    ConvertManager.ToString(Globals.MaterialSize / 5),
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
            Pen p = new Pen(new SolidBrush(line), Globals.LineSize);

            Point tmp = new Point(mcoords.X - Globals.MaterialSize,
                mcoords.Y - Globals.MaterialSize);
            Rectangle rect = new Rectangle(tmp, new Size(2 * Globals.MaterialSize, 2 * Globals.MaterialSize));


            if (type == Globals.MaterialTypes.Intermediate)
            {
                g.FillEllipse(brush, rect);
            }
            else if (type == Globals.MaterialTypes.Product)
            {
                g.FillEllipse(brush, rect);
                Rectangle rect2 = new Rectangle(rect.Location.X + Globals.MaterialSize / 4,
                    rect.Location.Y + Globals.MaterialSize / 4,
                    Globals.MaterialSize * 3 / 2,
                    Globals.MaterialSize * 3 / 2);
                g.FillEllipse(whitebrush, rect2);

                Rectangle rect3 = new Rectangle(rect.Location.X + Globals.MaterialSize / 2,
                    rect.Location.Y + Globals.MaterialSize / 2,
                    Globals.MaterialSize,
                    Globals.MaterialSize);
                g.FillEllipse(brush, rect3);

                Rectangle rect4 = new Rectangle(rect.Location.X + Globals.MaterialSize * 3 / 4,
                    rect.Location.Y + Globals.MaterialSize * 3 / 4,
                    Globals.MaterialSize / 2,
                    Globals.MaterialSize / 2);
                g.FillEllipse(whitebrush, rect4);
            }
            else if (type == Globals.MaterialTypes.Raw)
            {
                g.FillEllipse(brush, rect);
                List<Point> trianglepoints = new List<Point>();
                tmp = new Point();
                tmp.X = mcoords.X - (int)Math.Round(Math.Cos(Math.PI / 6) * Globals.MaterialSize);
                tmp.Y = mcoords.Y - (int)Math.Round(Math.Sin(Math.PI / 6) * Globals.MaterialSize);
                trianglepoints.Add(tmp);

                tmp = new Point();
                tmp.X = mcoords.X - (int)Math.Round(Math.Cos(Math.PI / 6 * 5) * Globals.MaterialSize);
                tmp.Y = mcoords.Y - (int)Math.Round(Math.Sin(Math.PI / 6 * 5) * Globals.MaterialSize);
                trianglepoints.Add(tmp);

                tmp = new Point();
                tmp.X = mcoords.X;
                tmp.Y = mcoords.Y + Globals.MaterialSize;
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

        #region PropertyGrid properties

        [Browsable(true)]
        [Category("\t\t\tMain"), PropertyOrder(1)]
        [DisplayName("Type")]
        [ReadOnly(true)]
        public string TypeProp
        {
            get
            {
                if (type == Globals.MaterialTypes.Raw)
                {
                    return "Raw Material";
                }
                else if (type == Globals.MaterialTypes.Intermediate)
                {
                    return "Intermediate Material";
                }
                else if (type == Globals.MaterialTypes.Product)
                {
                    return "Product Material";
                }
                else{
                    return "Unknown";
                }
            }
        }

        [Browsable(false)]
        [Category("\t\tLabel"), PropertyOrder(100)]
        [DisplayName("Text")]
        /* [Editor(typeof(MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))] */
        public string LabelTextProp
        {
            get
            {
                //return Title;
                return Name;
            }
            set
            {
                Name = value;
                //Title = value;
            }
        }

        [Browsable(true)]
        [Category("\t\tLabel"), PropertyOrder(101)]
        [DisplayName("Font Size")]
        public string FontSizeProp
        {
            get
            {
                int size = Label.FontSize;
                if (size <= 0)
                {
                    return Globals.DefaultFontSize.ToString() + " (default)";
                }
                else
                {
                    return size.ToString();
                }
            }
            set
            {
                int newsize = ConvertManager.ToInt(value);
                if (value != newsize.ToString())
                {
                    Label.FontSize = -1;
                }
                else
                {
                    Label.FontSize = newsize;
                }
            }
        }

        [Browsable(true)]
        [Category("\t\t\tMain"), PropertyOrder(2)]
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
        [DisplayName("Coords")]
        [Category("\t\t\tMain"), PropertyOrder(4)]
        public Point CoordsProp
        {
            get
            {
                return Coords;
            }
            set
            {
                Coords = value;
            }
        }

        [Browsable(true)]
        [DisplayName("Color")]
        [Category("\t\t\tMain"), PropertyOrder(3)]
        public Color ColorProp
        {
            get
            {
                return Color;
            }
            set
            {
                Color = value;
            }
        }

        [Browsable(true)]
        [Category("\t\tLabel"), PropertyOrder(107)]
        [DisplayName("Color")]
        public Color LabelColorProp
        {
            get
            {
                return LabelColor;
            }
            set
            {
                LabelColor = value;
            }
        }

        [Browsable(true)]
        [Category("\t\tLabel"), PropertyOrder(108)]
        [DisplayName("Offset")]
        public Point LabelOffsetProp
        {
            get
            {
                return LabelOffset;
            }
            set
            {
                LabelOffset = value;
            }
        }

        [Browsable(true)]
        [Category("Parameters"), PropertyOrder(1)]
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
        [Category("Parameters"), PropertyOrder(2)]
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
        [Category("Parameters"), PropertyOrder(3)]
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
        [Category("\tComment"), PropertyOrder(51)]
        [DisplayName("Text")]
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

       

        [Browsable(true)]
        [Category("\tComment"), PropertyOrder(57)]
        [DisplayName("Color")]
        public Color CommentColorProp
        {
            get
            {
                return Comment.ColorProp;
            }
            set
            {
                Comment.ColorProp = value;
            }
        }

        [Browsable(true)]
        [Category("\tComment"), PropertyOrder(58)]
        [DisplayName("Offset")]
        public Point CommentOffsetProp
        {
            get
            {
                return Comment.OffsetProp;
            }
            set
            {
                Comment.OffsetProp = value;
            }
        }

        [Browsable(true)]
        [Category("\tComment"), PropertyOrder(54)]
        [DisplayName("Font Size")]
        public string CommentFontSizeProp
        {
            get
            {
                int size = Comment.FontSize;
                if (size <= 0)
                {
                    return Globals.DefaultFontSize.ToString() + " (default)";
                }
                else
                {
                    return size.ToString();
                }
            }
            set
            {
                int newsize = ConvertManager.ToInt(value);
                if (value != newsize.ToString())
                {
                    Comment.FontSize = -1;
                }
                else
                {
                    Comment.FontSize = newsize;
                }
            }

        }


        [Browsable(true)]
        [Category("Parameters"), PropertyOrder(17)]
        [DisplayName("Color")]
        public Color ParametersColorProp
        {
            get
            {
                return Parameters.ColorProp;
            }
            set
            {
                Parameters.ColorProp = value;
            }
        }

        [Browsable(true)]
        [Category("Parameters"), PropertyOrder(18)]
        [DisplayName("Offset")]
        public Point ParametersOffsetProp
        {
            get
            {
                return Parameters.OffsetProp;
            }
            set
            {
                Parameters.OffsetProp = value;
            }
        }

        [Browsable(true)]
        [Category("Parameters"), PropertyOrder(16)]
        [DisplayName("Font Size")]
        public string ParametersFontSizeProp
        {
            get
            {
                int size = Parameters.FontSize;
                if (size <= 0)
                {
                    return Globals.DefaultFontSize.ToString() + " (default)";
                }
                else
                {
                    return size.ToString();
                }
            }
            set
            {
                int newsize = ConvertManager.ToInt(value);
                if (value != newsize.ToString())
                {
                    Parameters.FontSize = -1;
                }
                else
                {
                    Parameters.FontSize = newsize;
                }
            }

        }

        [Browsable(true)]
        [Category("Parameters"), PropertyOrder(4)]
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
        [Category("Parameters"), PropertyOrder(5)]
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
        [Category("Parameters"), PropertyOrder(6)]
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
                PriceMU = value + "/" + Default.time_mu;
                OnPriceMUNotify();
            }
        }

        #endregion

    }
}
