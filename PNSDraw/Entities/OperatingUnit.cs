using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace PNSDraw
{
    [TypeConverter(typeof(PropertySorter))]
    public class OperatingUnit : Canvas.IGraphicsObject, Canvas.IConnectableObject
    {
        #region Core private data variables
        
        int ID;
        Point coords;
        Point offset;

        Color color;
        bool locked = false;
        bool Selected = false;

        string name;
        string title;

        List<Canvas.IConnectableObject> connectedobjects;
        Canvas.IGraphicsStructure Container;

        TextObject Label;

        TextObject Comment;

        TextObject Parameters;

        bool Highlighted;

        Dictionary<string, ObjectProperty> parameterlist;

        #endregion;


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


        public OperatingUnit(Canvas.IGraphicsStructure container)
        {
            ID = 0;
            Coords = new Point(0, 0);
            offset = new Point(0, 0);
            Color = Color.Black;
            connectedobjects = new List<Canvas.IConnectableObject>();
            Container = container;
            Label = new TextObject(container, this);
            Label.Text = "OperatingUnit";
            Label.SetCoords(new Point(Globals.OperatingUnitWidth/2 +3 , -Globals.OperatingUnitHeight/2));
            Highlighted = false;
            Name = Container.GenerateName(this);
            Title = "";

            parameterlist = new Dictionary<string, ObjectProperty>();
            Comment = new TextObject(container, this);
            Comment.Text = "";
            Comment.SetCoords(new Point(Globals.OperatingUnitWidth / 2 + 3, -Globals.OperatingUnitHeight / 2 + Globals.DefaultLineHeight));

            Parameters = new TextObject(container, this);
            Parameters.Text = "";
            Parameters.SetCoords(new Point(Globals.OperatingUnitWidth / 2 + 3, -Globals.OperatingUnitHeight / 2 + 2 * Globals.DefaultLineHeight));

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
            parameterlist["caplower"].Value = -1;
            parameterlist["caplower"].NonValue = -1;
            parameterlist["capupper"].Prefix = "Capacity, upper bound: ";
            parameterlist["capupper"].Value = -1;
            parameterlist["capupper"].NonValue = -1;
            parameterlist["investcostfix"].Prefix = "Investment cost, fix: ";
            parameterlist["investcostprop"].Prefix = "Investment cost, proportional: ";
            parameterlist["opercostfix"].Prefix = "Operating cost, fix: ";
            parameterlist["opercostprop"].Prefix = "Operating cost, proportional: ";
            parameterlist["payoutperiod"].Prefix = "Payout period: ";
            parameterlist["payoutperiod"].Value = -1;
            parameterlist["payoutperiod"].NonValue = -1;
            parameterlist["workinghour"].Prefix = "Working hours per year: ";
            parameterlist["workinghour"].Value = -1;
            parameterlist["workinghour"].NonValue = -1;
        }


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
            if ((Math.Abs(dx) < (Globals.OperatingUnitWidth / 2)) && (Math.Abs(dy) < (Globals.OperatingUnitHeight / 2)))
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

        public bool IntersectsWith(Rectangle rect)
        {
            rect.Inflate(Globals.OperatingUnitWidth / 2, Globals.OperatingUnitHeight / 2);
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
            Size halfopsize = new Size(Globals.OperatingUnitWidth / 2, Globals.OperatingUnitHeight / 2);
            return new Rectangle(Coords - halfopsize, new Size(Globals.OperatingUnitWidth, Globals.OperatingUnitHeight));
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

            Point tmp = new Point(Coords.X - Globals.OperatingUnitWidth / 2,
                Coords.Y - Globals.OperatingUnitHeight / 2);
            Rectangle rect = new Rectangle(tmp, new Size(Globals.OperatingUnitWidth, Globals.OperatingUnitHeight));

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
            Pen p = new Pen(new SolidBrush(line), Globals.LineSize);

            Point tmp = new Point(mcoords.X - Globals.OperatingUnitWidth / 2,
                mcoords.Y - Globals.OperatingUnitHeight / 2);
            Rectangle rect = new Rectangle(tmp, new Size(Globals.OperatingUnitWidth, Globals.OperatingUnitHeight));

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
                return "Operating Unit";
            }
        }

        [Browsable(true)]
        [Category("\t\tLabel"), PropertyOrder(100)]
        [DisplayName("Text")]
        public string LabelTextProp
        {
            get
            {
                return Title;
            }
            set
            {
                Title = value;
            }
        }

        [Browsable(true)]
        [Category("\t\t\tMain"), PropertyOrder(2)]
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
        [Category("\t\t\tMain"), PropertyOrder(4)]
        [DisplayName("Coords")]
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
        [Category("Parameters"), PropertyOrder(1)]
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
        [Category("Parameters"), PropertyOrder(2)]
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
        [Category("Parameters"), PropertyOrder(3)]
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
        [Category("Parameters"), PropertyOrder(4)]
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
        [Category("Parameters"), PropertyOrder(5)]
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
        [Category("Parameters"), PropertyOrder(6)]
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
        [Category("Parameters"), PropertyOrder(7)]
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
        [Category("Parameters"), PropertyOrder(8)]
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

        #endregion

        
    }
}
