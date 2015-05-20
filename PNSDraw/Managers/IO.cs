using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;

namespace PNSDraw
{
    public class IOManager
    {

       



    }

    [XmlRoot(ElementName="PGraph", Namespace="")]
    public class XMLPGraph
    {
        [XmlArray("Materials")]
        [XmlArrayItem("Material")]
        public List<XMLMaterial> Materials;

        [XmlArray("Edges")]
        [XmlArrayItem("Edge")]
        public List<XMLEdge> Edges;

        [XmlArray("OperatingUnits")]
        [XmlArrayItem("OperatingUnit")]
        public List<XMLOperatingUnit> OperatingUnits;

        [XmlElement("Settings")]
        public XMLSettings Settings { get; set; }

        [XmlAttribute("Version")]
        public string Version { get; set; }

        public XMLPGraph()
        {
            Materials = new List<XMLMaterial>();
            Edges = new List<XMLEdge>();
            OperatingUnits = new List<XMLOperatingUnit>();
            Settings = new XMLSettings();
            Version = "0.1";
        }

        public void Reset()
        {
            Materials.Clear();
            Edges.Clear();
            OperatingUnits.Clear();
            Settings = new XMLSettings();
            Version = "0.1";
        }
    }

    public class XMLSettings
    {
        public int DefaultFontSize;
        public int GridSize;

        public XMLSettings()
        {
            DefaultFontSize = 12;
            GridSize = 300;
        }
    }

    public class XMLMaterial
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlElement("Coords")]
        public Point Coords { get; set; }

        [XmlElement("Label")]
        public XMLTextObject Label { get; set; }

        [XmlElement("Comment")]
        public XMLTextObject Comment { get; set; }

        [XmlElement("Parameters")]
        public XMLTextObject Parameters { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlIgnore]
        public Color ObjectColor { get; set; }

        [XmlElement("Color")]
        public int ObjectColorData
        {
            get
            {
                return ObjectColor.ToArgb();
            }
            set
            {
                ObjectColor = Color.FromArgb(value);
            }
        }

        [XmlAttribute("Type")]
        public int Type { get; set; }

        [XmlArray("ParameterList")]
        [XmlArrayItem("Parameter")]
        public List<XMLObjectProperty> ParameterList;

        public XMLMaterial()
        {
            Coords = new Point();
            Label = new XMLTextObject();
            Comment = new XMLTextObject();
            Parameters = new XMLTextObject();
            ObjectColor = Color.Black;
            Type = Globals.MaterialTypes.Intermediate;
            ID = 0;
            Name = "";
            Title = "";
            ParameterList = new List<XMLObjectProperty>();
        }

        public void FillData(Material m)
        {
            m.Title = Title;
            m.NameProp = Name;
            m.SetID(ID);
            m.CoordsProp = Coords;
            m.ColorProp = ObjectColor;
            m.Type = Type;
            m.LabelTextProp = Label.Text;
            m.LabelColorProp = Label.ObjectColor;
            m.LabelOffsetProp = Label.Offset;
            m.LabelFontSize = Label.FontSize;
            m.CommentTextProp = Comment.Text;
            m.CommentColorProp = Comment.ObjectColor;
            m.CommentOffsetProp = Comment.Offset;
            m.CommentFontSize = Comment.FontSize;
            m.ParametersColorProp = Parameters.ObjectColor;
            m.ParametersOffsetProp = Parameters.Offset;
            m.ParametersFontSize = Parameters.FontSize;
            foreach (XMLObjectProperty xop in ParameterList)
            {
                if (m.ParameterList.ContainsKey(xop.Name))
                {
                    m.ParameterList[xop.Name].Prefix = xop.Prefix;
                    m.ParameterList[xop.Name].Value = xop.Value;
                    m.ParameterList[xop.Name].MU = xop.MU;
                    m.ParameterList[xop.Name].Visible = xop.Visible;
                }
            }
            m.UpdateParametersLabel();
        }

        public void ConvertFromMaterial(Material m)
        {
            ID = m.GetID();
            Coords = m.CoordsProp;
            ObjectColor = m.ColorProp;
            Type = m.Type;
            Label.Text = m.LabelTextProp;
            Label.ObjectColor = m.LabelColorProp;
            Label.Offset = m.LabelOffsetProp;
            Label.FontSize = m.LabelFontSize;
            Name = m.Name;
            Title = m.Title;
            Comment.Text = m.CommentTextProp;
            Comment.ObjectColor = m.CommentColorProp;
            Comment.Offset = m.CommentOffsetProp;
            Comment.FontSize = m.CommentFontSize;
            Parameters.ObjectColor = m.ParametersColorProp;
            Parameters.Offset = m.ParametersOffsetProp;
            Parameters.FontSize = m.ParametersFontSize;
            foreach (ObjectProperty op in m.ParameterList.Values)
            {
                XMLObjectProperty xop = new XMLObjectProperty();
                xop.Name = op.Name;
                xop.Prefix = op.Prefix;
                xop.Value = op.Value;
                xop.MU = op.MU;
                xop.Visible = op.Visible;
                ParameterList.Add(xop);
            }
        }

    }

    public class XMLEdge
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("BeginID")]
        public int BeginObjectID { get; set; }

        [XmlAttribute("EndID")]
        public int EndObjectID { get; set; }

        [XmlElement("Label")]
        public XMLTextObject Label { get; set; }

        [XmlAttribute("Rate")]
        public double Rate { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlAttribute("ArrowOnCenter")]
        public bool ArrowOnCenter { get; set; }

        [XmlAttribute("ArrowPosition")]
        public double ArrowPosition { get; set; }

        [XmlIgnore]
        public Color ObjectColor { get; set; }

        [XmlElement("Color")]
        public int ObjectColorData
        {
            get
            {
                return ObjectColor.ToArgb();
            }
            set
            {
                ObjectColor = Color.FromArgb(value);
            }
        }

        [XmlArray("Nodes")]
        [XmlArrayItem("Node")]
        public List<XMLEdgeNode> Nodes;

        public XMLEdge()
        {
            ID = 0;
            Label = new XMLTextObject();
            ObjectColor = Color.Black;
            BeginObjectID = 0;
            EndObjectID = 0;
            Nodes = new List<XMLEdgeNode>();
            ArrowOnCenter = true;
            ArrowPosition = 50F;
            Title = "";
            Rate = 1.0;
        }

        public void FillData(Edge e)
        {
            e.SetID(ID);
            e.ColorProp = ObjectColor;
            e.LabelTextProp = Label.Text;
            e.LabelColorProp = Label.ObjectColor;
            e.LabelOffsetProp = Label.Offset;
            e.LabelFontSize = Label.FontSize;
            e.Title = Title;
            e.RateProp = Rate;
            Nodes.Sort();
            List<EdgeNode> enodes = new List<EdgeNode>();
            foreach(XMLEdgeNode xen in Nodes)
            {
                EdgeNode en = new EdgeNode(e.GetContainer(), e);
                en.Needed = true;
                en.Temporary = false;
                en.CoordsProp = xen.Coords;
                en.Smooth = xen.Smooth;
                en.SmoothSizeAfter = xen.SmoothSizeAfter;
                en.SmoothSizeBefore = xen.SmoothSizeBefore;
                en.SmoothStrength = xen.SmoothStrength;
                enodes.Add(en);
            }
            e.SetNodes(enodes);
            Canvas.IGraphicsObject begin = e.GetContainer().GetObjectByID(BeginObjectID);
            Canvas.IGraphicsObject end = e.GetContainer().GetObjectByID(EndObjectID);
            e.begin = begin as Canvas.IConnectableObject;
            e.end = end as Canvas.IConnectableObject;
            e.ArrowOnCenterProp = ArrowOnCenter;
            e.ArrowPositionProp = ArrowPosition;
        }

        public void ConvertFromEdge(Edge e)
        {
            ID = e.GetID();
            BeginObjectID = ((Canvas.IGraphicsObject)e.begin).GetID();
            EndObjectID = ((Canvas.IGraphicsObject)e.end).GetID();
            ObjectColor = e.ColorProp;
            Label.Text = e.LabelTextProp;
            Label.ObjectColor = e.LabelColorProp;
            Label.Offset = e.LabelOffsetProp;
            Label.FontSize = e.LabelFontSize;
            Title = e.Title;
            Rate = e.RateProp;
            Nodes.Clear();
            List<EdgeNode> enodes = e.GetNodes();
            int counter = 0;
            foreach (EdgeNode en in enodes)
            {
                XMLEdgeNode xen = new XMLEdgeNode();
                xen.Order = counter++;
                xen.Coords = en.CoordsProp;
                xen.Smooth = en.Smooth;
                xen.SmoothSizeAfter = en.SmoothSizeAfter;
                xen.SmoothSizeBefore = en.SmoothSizeBefore;
                xen.SmoothStrength = en.SmoothStrength;
                Nodes.Add(xen);
            }
            ArrowOnCenter = e.ArrowOnCenterProp;
            ArrowPosition = e.ArrowPositionProp;
        }
    }

    public class XMLOperatingUnit
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlElement("Coords")]
        public Point Coords { get; set; }

        [XmlElement("Label")]
        public XMLTextObject Label { get; set; }

        [XmlElement("Comment")]
        public XMLTextObject Comment { get; set; }

        [XmlElement("Parameters")]
        public XMLTextObject Parameters { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlIgnore]
        public Color ObjectColor { get; set; }

        [XmlElement("Color")]
        public int ObjectColorData
        {
            get
            {
                return ObjectColor.ToArgb();
            }
            set
            {
                ObjectColor = Color.FromArgb(value);
            }
        }

        [XmlArray("ParameterList")]
        [XmlArrayItem("Parameter")]
        public List<XMLObjectProperty> ParameterList;

        public XMLOperatingUnit()
        {
            Coords = new Point();
            Label = new XMLTextObject();
            Comment = new XMLTextObject();
            Parameters = new XMLTextObject();
            ObjectColor = Color.Black;
            ID = 0;
            Name = "";
            Title = "";
            ParameterList = new List<XMLObjectProperty>();
        }

        public void FillData(OperatingUnit o)
        {
            o.Title = Title;
            o.NameProp = Name;
            o.SetID(ID);
            o.CoordsProp = Coords;
            o.ColorProp = ObjectColor;
            o.LabelTextProp = Label.Text;
            o.LabelColorProp = Label.ObjectColor;
            o.LabelOffsetProp = Label.Offset;
            o.LabelFontSize = Label.FontSize;
            o.CommentTextProp = Comment.Text;
            o.CommentColorProp = Comment.ObjectColor;
            o.CommentOffsetProp = Comment.Offset;
            o.CommentFontSize = Comment.FontSize;
            o.ParametersColorProp = Parameters.ObjectColor;
            o.ParametersOffsetProp = Parameters.Offset;
            o.ParametersFontSize = Parameters.FontSize;
            foreach (XMLObjectProperty xop in ParameterList)
            {
                if (o.ParameterList.ContainsKey(xop.Name))
                {
                    o.ParameterList[xop.Name].Prefix = xop.Prefix;
                    o.ParameterList[xop.Name].Value = xop.Value;
                    o.ParameterList[xop.Name].MU = xop.MU;
                    o.ParameterList[xop.Name].Visible = xop.Visible;
                }
            }
            o.UpdateParametersLabel();
        }

        public void ConvertFromOperatingUnit(OperatingUnit o)
        {
            ID = o.GetID();
            Coords = o.CoordsProp;
            ObjectColor = o.ColorProp;
            Label.Text = o.LabelTextProp;
            Label.ObjectColor = o.LabelColorProp;
            Label.Offset = o.LabelOffsetProp;
            Label.FontSize = o.LabelFontSize;
            Name = o.Name;
            Title = o.Title;
            Comment.Text = o.CommentTextProp;
            Comment.ObjectColor = o.CommentColorProp;
            Comment.Offset = o.CommentOffsetProp;
            Comment.FontSize = o.CommentFontSize;
            Parameters.ObjectColor = o.ParametersColorProp;
            Parameters.Offset = o.ParametersOffsetProp;
            Parameters.FontSize = o.ParametersFontSize;
            foreach (ObjectProperty op in o.ParameterList.Values)
            {
                XMLObjectProperty xop = new XMLObjectProperty();
                xop.Name = op.Name;
                xop.Prefix = op.Prefix;
                xop.Value = op.Value;
                xop.MU = op.MU;
                xop.Visible = op.Visible;
                ParameterList.Add(xop);
            }
        }
    }

    public class XMLTextObject
    {

        [XmlElement("Offset")]
        public Point Offset { get; set; }

        [XmlAttribute("Text")]
        public String Text { get; set; }

        [XmlIgnore]
        public Color ObjectColor { get; set; }

        [XmlElement("FontSize")]
        public int FontSize { get; set; }

        [XmlElement("Color")]
        public int ObjectColorData
        {
            get
            {
                return ObjectColor.ToArgb();
            }
            set
            {
                ObjectColor = Color.FromArgb(value);
            }
        }


        public XMLTextObject()
        {
            Offset = new Point();
            Text = "";
            ObjectColor = Color.Black;
            FontSize = -1;
        }
    }

    public class XMLObjectProperty
    {

        [XmlAttribute("Name")]
        public String Name { get; set; }

        [XmlAttribute("Prefix")]
        public String Prefix { get; set; }

        [XmlAttribute("Value")]
        public double Value { get; set; }

        [XmlAttribute("MU")]
        public String MU { get; set; }

        [XmlAttribute("Visible")]
        public bool Visible { get; set; }

        public XMLObjectProperty()
        {
            Name = "";
            Prefix = "";
            Value = 0;
            MU = "";
            Visible = false;
        }
    }

    public class XMLEdgeNode : IComparable
    {
        [XmlAttribute("Order")]
        public int Order { get; set; }

        [XmlElement("Coords")]
        public Point Coords { get; set; }

        [XmlAttribute("Smooth")]
        public bool Smooth { get; set; }

        [XmlAttribute("SmoothSizeBefore")]
        public int SmoothSizeBefore { get; set; }

        [XmlAttribute("SmoothSizeAfter")]
        public int SmoothSizeAfter { get; set; }
        
        [XmlAttribute("SmoothStrength")]
        public double SmoothStrength { get; set; }

        public XMLEdgeNode()
        {
            Coords = new Point();
            Order = 0;
            SmoothSizeAfter = 500;
            SmoothSizeBefore = 500;
            SmoothStrength = 0.5F;
        }

        public int CompareTo(object obj)
        {
            XMLEdgeNode other = obj as XMLEdgeNode;
            if (other != null)
            {
                return this.Order.CompareTo(other.Order);
            }
            else
            {
                return 0;
            }
        }
    }

    public class XMLPNSOperatingUnitMaterial
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("rate")]
        public double Rate { get; set; }

    }

    public class XMLPNSMaterial
    {
        
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("min")]
        public string Min { get; set; }

        [XmlAttribute("max")]
        public string Max { get; set; }

        [XmlAttribute("price")]
        public string Price { get; set; }

        [XmlIgnoreAttribute()]
        public bool MinSpecified
        {
            get
            {
                return (Min != "");
            }
            set
            {
                
            }
        }

        [XmlIgnoreAttribute()]
        public bool MaxSpecified
        {
            get
            {
                return (Max != "");
            }
            set
            {

            }
        }

        [XmlIgnoreAttribute()]
        public bool PriceSpecified
        {
            get
            {
                return (Price != "");
            }
            set
            {

            }
        }
    }

    public class XMLPNSOperatingUnit
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("inputMaterial")]
        public List<XMLPNSOperatingUnitMaterial> Input { get; set; }

        [XmlElement("outputMaterial")]
        public List<XMLPNSOperatingUnitMaterial> Output { get; set; }

        [XmlAttribute("investmentFix")]
        public string InvestmentFix { get; set; }

        [XmlAttribute("investmentProp")]
        public string InvestmentProp { get; set; }

        [XmlAttribute("operatingFix")]
        public string OperatingFix { get; set; }

        [XmlAttribute("operatingProp")]
        public string OperatingProp { get; set; }

        [XmlAttribute("lowerBound")]
        public string CapacityLower { get; set; }

        [XmlAttribute("upperBound")]
        public string CapacityUpper { get; set; }

        [XmlElement("workingHoursPerYear")]
        public string WorkingHours { get; set; }

        [XmlElement("payoutPeriod")]
        public string PayoutPeriod { get; set; }

        [XmlIgnoreAttribute()]
        public bool InvestmentFixSpecified
        {
            get
            {
                return (InvestmentFix != "");
            }
            set
            {

            }
        }

        [XmlIgnoreAttribute()]
        public bool InvestmentPropSpecified
        {
            get
            {
                return (InvestmentProp != "");
            }
            set
            {

            }
        }

        [XmlIgnoreAttribute()]
        public bool OperatingFixSpecified
        {
            get
            {
                return (OperatingFix != "");
            }
            set
            {

            }
        }

        [XmlIgnoreAttribute()]
        public bool OperatingPropSpecified
        {
            get
            {
                return (OperatingProp != "");
            }
            set
            {

            }
        }

        [XmlIgnoreAttribute()]
        public bool CapacityLowerSpecified
        {
            get
            {
                return (CapacityLower != "");
            }
            set
            {

            }
        }

        [XmlIgnoreAttribute()]
        public bool CapacityUpperSpecified
        {
            get
            {
                return (CapacityUpper != "");
            }
            set
            {

            }
        }

        [XmlIgnoreAttribute()]
        public bool WorkingHoursSpecified
        {
            get
            {
                return (WorkingHours != "");
            }
            set
            {

            }
        }

        [XmlIgnoreAttribute()]
        public bool PayoutPeriodSpecified
        {
            get
            {
                return (PayoutPeriod != "");
            }
            set
            {

            }
        }

        public XMLPNSOperatingUnit()
        {
            Input = new List<XMLPNSOperatingUnitMaterial>();
            Output = new List<XMLPNSOperatingUnitMaterial>();
        }

    }

    public class XMLPNSMaterials
    {
        [XmlElement("rawMaterial")]
        public List<XMLPNSMaterial> Raw { get; set; }

        [XmlElement("intermediateMaterial")]
        public List<XMLPNSMaterial> Intermediate { get; set; }

        [XmlElement("productMaterial")]
        public List<XMLPNSMaterial> Product { get; set; }


        public XMLPNSMaterials()
        {
            Raw = new List<XMLPNSMaterial>();
            Intermediate = new List<XMLPNSMaterial>();
            Product = new List<XMLPNSMaterial>();
        }
    }


    public class XMLPNSProblem
    {
        [XmlElement("materials")]
        public XMLPNSMaterials Materials { get; set; }

        [XmlArray("operatingUnits")]
        [XmlArrayItem("operatingUnit")]
        public List<XMLPNSOperatingUnit> OperatingUnits { get; set; }

        public XMLPNSProblem()
        {
            OperatingUnits = new List<XMLPNSOperatingUnit>();
            Materials = new XMLPNSMaterials();
        }

    }

    [SerializableAttribute()]
    [XmlTypeAttribute(Namespace="http://www.example.org/PNSXMLSchema")]
    [XmlRootAttribute("pns", Namespace = "http://www.example.org/PNSXMLSchema", IsNullable = false)]
    public class XMLPNS
    {
        [XmlElement("problem")]
        public XMLPNSProblem Problem { get; set; }

        public XMLPNS()
        {
            Problem = new XMLPNSProblem();
        }
    }

}
