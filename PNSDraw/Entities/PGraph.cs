using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using PNSDraw.Entities;
using System.Globalization;

namespace PNSDraw
{
    public class PGraph : Canvas.IGraphicsStructure
    {

        public List<Material> Materials { get; set; }
        public List<OperatingUnit> OperatingUnits { get; set; }
        public List<Edge> Edges { get; set; }
        public List<MutualExclusion> MutualExclusions { get; set; }

        List<Canvas.IGraphicsObject> GraphicsObjectList;

        public List<Solution> Solutions;

        XMLPGraph CopiedStructure;

        public int SolutionCount
        {
            get
            {
                return Solutions.Count;
            }
        }

        int UniqueID = 0;

        string UndoXML = "";
        string CurrentXML = "";
        string RedoXML = "";

        public int GetFreeUniqueID()
        {
            return ++UniqueID;
        }

        public void CreateUndo()
        {
            UndoXML = CurrentXML;
            CurrentXML = ExportToXML();
            RedoXML = "";
        }

        public bool DoUndo()
        {
            if (UndoXML != "")
            {
                RedoXML = ExportToXML();
                ImportFromXML(UndoXML);
                CurrentXML = UndoXML;
                UndoXML = "";
                return true;
            }
            else 
            {
                return false;
            }
        }

        public bool DoRedo()
        {
            if (RedoXML != "")
            {
                UndoXML = ExportToXML();
                ImportFromXML(RedoXML);
                CurrentXML = RedoXML;
                RedoXML = "";
                return true;
            }
            else
            {
                return false;
            }
        }

        public PGraph()
        {
            GraphicsObjectList = new List<Canvas.IGraphicsObject>();
            Materials = new List<Material>();
            OperatingUnits = new List<OperatingUnit>();
            Edges = new List<Edge>();
            MutualExclusions = new List<MutualExclusion>();
            Solutions = new List<Solution>();
            UniqueID = 0;
            CopiedStructure = new XMLPGraph();
        }

        public void AddSingleObject(Canvas.IGraphicsObject obj)
        {
            if (obj.GetID() == 0)
            {
                obj.SetID(GetFreeUniqueID());
            }
            if (obj is Material)
            {
                Material m = obj as Material;
                AddMaterial(m);
            }
            else if (obj is OperatingUnit)
            {
                OperatingUnit ou = obj as OperatingUnit;
                AddOperatingUnit(ou);
            }
            else if (obj is Edge)
            {
                Edge e = obj as Edge;
                AddEdge(e);
            }

        }

        public void RemoveSingleObject(Canvas.IGraphicsObject obj)
        {
            if (obj is Material)
            {
                Material m = obj as Material;
                RemoveMaterial(m);
            }
            else if (obj is OperatingUnit)
            {
                OperatingUnit ou = obj as OperatingUnit;
                RemoveOperatingUnit(ou);
            }
            else if (obj is Edge)
            {
                Edge e = obj as Edge;
                RemoveEdge(e);
            }
            else if (obj is EdgeNode)
            {
                EdgeNode en = obj as EdgeNode;
                RemoveEdgeNode(en);
            }
            else if (obj is TextObject)
            {
                TextObject to = obj as TextObject;
                List<Canvas.IGraphicsObject> toremove = new List<Canvas.IGraphicsObject>();
                toremove.Add(to.GetParentObject());
                RemoveObjects(toremove);
            }

        }

        public void AddSingleGraphicsObject(Canvas.IGraphicsObject obj)
        {
            if (GraphicsObjectList.Contains(obj) == false)
            {
                if (obj.GetID() == 0) obj.SetID(GetFreeUniqueID());
                GraphicsObjectList.Add(obj);
            }
        }

        public void RemoveSingleGraphicsObject(Canvas.IGraphicsObject obj)
        {
            if (GraphicsObjectList.Contains(obj))
            {
                GraphicsObjectList.Remove(obj);
            }
        }

        public void RemoveObjects(List<Canvas.IGraphicsObject> toremove)
        {
            foreach (Canvas.IGraphicsObject obj in toremove)
            {
                List<Edge> connected = GetConnectedEdges(obj);
                RemoveSingleObject(obj);
                foreach (Edge ed in connected)
                {
                    RemoveSingleObject(ed);
                }
            }
        }

        public string GetSolutionTitle(int index)
        {
            if (index >= 0 && index < SolutionCount)
            {
                return Solutions[index].Title;
            }
            else
            {
                return "";
            }
        }

        public void Reset()
        {
            Clear();
            UndoXML = "";
            RedoXML = "";
            CurrentXML = "";
            Globals.ResetDefaults();
            Solutions.Clear();
        }

        public void Clear()
        {
            Materials.Clear();
            OperatingUnits.Clear();
            Edges.Clear();
            GraphicsObjectList.Clear();
            UniqueID = 0;
        }


        public string ExportToPNS()
        {
            List<XMLPNSMaterial> rawmat = new List<XMLPNSMaterial>();
            List<XMLPNSMaterial> intermediatemat = new List<XMLPNSMaterial>();
            List<XMLPNSMaterial> productmat = new List<XMLPNSMaterial>();

            foreach (Material m in Materials)
            {
                XMLPNSMaterial tmat = new XMLPNSMaterial();
                tmat.Name = m.Name;
                tmat.Price = m.ParameterList["price"].ToPNSValue();
                tmat.Min = m.ParameterList["reqflow"].ToPNSValue();
                tmat.Max = m.ParameterList["maxflow"].ToPNSValue();
                
                if (m.Type == Globals.MaterialTypes.Raw)
                {
                    rawmat.Add(tmat);
                }
                else if (m.Type == Globals.MaterialTypes.Intermediate)
                {
                    intermediatemat.Add(tmat);
                }
                else if (m.Type == Globals.MaterialTypes.Product)
                {
                    productmat.Add(tmat);
                }
            }

            Dictionary<int, XMLPNSOperatingUnit> ounits = new Dictionary<int, XMLPNSOperatingUnit>();
            foreach (OperatingUnit o in OperatingUnits)
            {
                XMLPNSOperatingUnit top = new XMLPNSOperatingUnit();
                top.Name = o.Name;
                top.CapacityLower = o.ParameterList["caplower"].ToPNSValue();
                top.CapacityUpper = o.ParameterList["capupper"].ToPNSValue();
                top.InvestmentFix = o.ParameterList["investcostfix"].ToPNSValue();
                top.InvestmentProp = o.ParameterList["investcostprop"].ToPNSValue();
                top.OperatingFix = o.ParameterList["opercostfix"].ToPNSValue();
                top.OperatingProp = o.ParameterList["opercostprop"].ToPNSValue();
                top.PayoutPeriod = o.ParameterList["payoutperiod"].ToPNSValue();
                top.WorkingHours = o.ParameterList["workinghour"].ToPNSValue();
                ounits[o.GetID()] = top;
            }

            foreach (Edge e in Edges)
            {
                XMLPNSOperatingUnitMaterial tmat = new XMLPNSOperatingUnitMaterial();
                tmat.Rate = e.RateProp;
                if (e.begin is Material && e.end is OperatingUnit)
                {
                    Material m = e.begin as Material;
                    tmat.Name = m.Name;
                    OperatingUnit o = e.end as OperatingUnit;
                    ounits[o.GetID()].Input.Add(tmat);
                }
                else if (e.begin is OperatingUnit && e.end is Material)
                {
                    Material m = e.end as Material;
                    tmat.Name = m.Name;
                    OperatingUnit o = e.begin as OperatingUnit;
                    ounits[o.GetID()].Output.Add(tmat);
                }
            }

            XMLPNS pns = new XMLPNS();
            pns.Problem.Materials.Raw = rawmat;
            pns.Problem.Materials.Intermediate = intermediatemat;
            pns.Problem.Materials.Product = productmat;

            foreach (XMLPNSOperatingUnit xop in ounits.Values)
            {
                pns.Problem.OperatingUnits.Add(xop);
            }

            StringWriter xmlwriter = new StringWriter();
            XmlSerializer xs = new XmlSerializer(typeof(XMLPNS));
            xs.Serialize(xmlwriter, pns);
            return xmlwriter.ToString();
        }

        public string ExportToSVG()
        {
            string svg = "";

            svg = "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">";
            svg += "<svg width=\"100%\" height=\"100%\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">";

            foreach (Edge e in Edges)
            {
                svg += e.CovertToSVG(0);
            }

            foreach (Material m in Materials)
            {
                svg += m.ConvertToSVG(0);
            }

            foreach (OperatingUnit o in OperatingUnits)
            {
                svg += o.ConvertToSVG(0);
            }

            svg += "</svg>";

            return svg;
        }

        public string ExportToXML()
        {
            XMLPGraph pgraph = new XMLPGraph();
            pgraph.Version = "1.0";

            foreach (Material m in Materials)
            {
                XMLMaterial xmat = new XMLMaterial();
                xmat.ConvertFromMaterial(m);
                pgraph.Materials.Add(xmat);
            }

            foreach (OperatingUnit o in OperatingUnits)
            {
                XMLOperatingUnit xunit = new XMLOperatingUnit();
                xunit.ConvertFromOperatingUnit(o);
                pgraph.OperatingUnits.Add(xunit);
            }

            foreach (Edge e in Edges)
            {
                XMLEdge xedge = new XMLEdge();
                xedge.ConvertFromEdge(e);
                pgraph.Edges.Add(xedge);
            }

            pgraph.Settings.GridSize = Globals.GridSize;
            pgraph.Settings.DefaultFontSize = Globals.DefaultFontSize;

            StringWriter xmlwriter = new StringWriter();
            XmlSerializer xs = new XmlSerializer(typeof(XMLPGraph));
            xs.Serialize(xmlwriter, pgraph);
            return xmlwriter.ToString();
        }

        public bool ImportFromXML(string xml)
        {
            Clear();
            XmlSerializer xs = new XmlSerializer(typeof(XMLPGraph));
            XMLPGraph pgraph = (XMLPGraph) xs.Deserialize(new StringReader(xml));

            foreach (XMLMaterial xmat in pgraph.Materials)
            {
                Material m = new Material(this);
                xmat.FillData(m);
                AddMaterial(m);
            }

            foreach (XMLOperatingUnit xunit in pgraph.OperatingUnits)
            {
                OperatingUnit o = new OperatingUnit(this);
                xunit.FillData(o);
                AddOperatingUnit(o);
            }

            foreach (XMLEdge xedge in pgraph.Edges)
            {
                Edge e = new Edge(this);
                xedge.FillData(e);
                AddEdge(e);
            }

            Globals.GridSize = pgraph.Settings.GridSize;
            Globals.DefaultFontSize = pgraph.Settings.DefaultFontSize;

            return true;
        }

        List<Edge> GetConnectedEdges(Canvas.IGraphicsObject obj)
        {
            List<Edge> connected = new List<Edge>();
            foreach (Edge e in Edges)
            {
                if (e.begin == obj || e.end == obj)
                {
                    connected.Add(e);
                }
            }
            return connected;
        }

        public void AddEdge(Edge edge)
        {
            if (Edges.Contains(edge) == false)
            {
                Edges.Add(edge);
                edge.OnAddToGraph();
            }
        }

        public void AddMaterial(Material material)
        {
            if (Materials.Contains(material) == false)
            {
                if (material.GetID() > UniqueID)
                {
                    UniqueID = material.GetID();
                }
                Materials.Add(material);
                material.OnAddToGraph();
            }
        }

        public void AddOperatingUnit(OperatingUnit operatingunit)
        {
            if (OperatingUnits.Contains(operatingunit) == false)
            {
                if (operatingunit.GetID() > UniqueID)
                {
                    UniqueID = operatingunit.GetID();
                }
                OperatingUnits.Add(operatingunit);
                operatingunit.OnAddToGraph();
            }
        }

        public void AddMutualExclusion(MutualExclusion mutualexclusion)
        {
            if (MutualExclusions.Contains(mutualexclusion) == false)
            {
                if (mutualexclusion.ID > UniqueID)
                {
                    UniqueID = mutualexclusion.ID;
                }
                MutualExclusions.Add(mutualexclusion);
            }
        }

        public void RemoveEdge(Edge edge)
        {
            if (Edges.Contains(edge))
            {
                if (edge.GetID() > UniqueID)
                {
                    UniqueID = edge.GetID();
                }
                edge.OnRemoveFromGraph();
                Edges.Remove(edge);
            }
        }

        public void RemoveEdgeNode(EdgeNode node)
        {
            Edge edge = node.GetParentObject() as Edge;
            if (edge == null)
            {
                return;
            }
            edge.RemoveNode(node);
        }

        public void RemoveMaterial(Material material)
        {
            if (Materials.Contains(material))
            {
                material.OnRemoveFromGraph();
                Materials.Remove(material);
                foreach (Solution s in Solutions)
                {
                    s.RemoveMaterial(material.Name);
                }
            }
        }

        public void RemoveOperatingUnit(OperatingUnit operatingunit)
        {
            if (OperatingUnits.Contains(operatingunit))
            {
                operatingunit.OnRemoveFromGraph(); 
                OperatingUnits.Remove(operatingunit);
                foreach (Solution s in Solutions)
                {
                    s.RemoveOperatingUnit(operatingunit.Name);
                }
            }
        }

        public void RemoveMutualExclusion(MutualExclusion mutualexclusion)
        {
            if (MutualExclusions.Contains(mutualexclusion))
            {
                MutualExclusions.Remove(mutualexclusion);
            }
        }

        public List<Canvas.IGraphicsObject> GetObjectList()
        {
            return GraphicsObjectList;
        }


        public Canvas.IGraphicsObject GetObjectByID(int objectid)
        {
            foreach (Material m in Materials)
            {
                if (m.GetID() == objectid)
                {
                    return m;
                }
            }
            foreach (OperatingUnit o in OperatingUnits)
            {
                if (o.GetID() == objectid)
                {
                    return o;
                }
            }
            return null;
        }

        public Material GetMaterialByName(string name)
        {
            foreach (Material m in Materials)
            {
                if (m.Name == name)
                {
                    return m;
                }
            }
            return null;
        }

        public OperatingUnit GetOperatingUnitByName(string name)
        {
            foreach (OperatingUnit o in OperatingUnits)
            {
                if (o.Name == name)
                {
                    return o;
                }
            }
            return null;
        }

        public string ValidateName(Canvas.IGraphicsObject obj, string name)
        {
            if (name == null)
            {
                name = "_";
            }

            string validchars = "0123456789_QWERTZUIOPASDFGHJKLYXCVBNMqwertzuiopasdfghjklyxcvbnm";
            string validfirstchars = "_QWERTZUIOPASDFGHJKLYXCVBNMqwertzuiopasdfghjklyxcvbnm";

            name = name.Replace(' ', '_');
            name = name.Replace('-', '_');

            string validname = "";

            int i=0;
            for (i = 0; i < name.Length; i++)
            {
                if (validchars.IndexOf(name[i])>=0)
                {
                    validname += name[i];
                }
            }
            if (validname.Length > 0)
            {
                if (validfirstchars.IndexOf(validname[0]) < 0)
                {
                    validname = "_" + validname;
                }
            }
            else
            {
                validname = "_";
            }

            bool ok = true;
            foreach (Material m in Materials)
            {
                if (m.Name == validname && m != obj)
                {
                    ok = false;
                }
            }

            foreach (OperatingUnit o in OperatingUnits)
            {
                if (o.Name == validname && o != obj)
                {
                    ok = false;
                }
            }

            if (ok == true)
            {
                return validname;
            }
            else
            {
                if (obj == null)
                {
                    return "";
                }
                else if (obj is Material)
                {
                    return ((Material)obj).Name;
                }
                else if (obj is OperatingUnit)
                {
                    return ((OperatingUnit)obj).Name;
                }
            }
            

            return "";
        }

        public string GeneratePostfixName(string name)
        {
            string validname = ValidateName(null, name);
            if (validname != "")
            {
                return "";
            }
            int i = 1;
            
            while (i < 1000)
            {
                string postfix = "_" + i.ToString();
                validname = ValidateName(null, name + postfix);
                if (validname != "")
                {
                    return postfix;
                }
                i++;
            }
            return "_unkonwn";
        }


        public string GenerateName(Canvas.IGraphicsObject obj)
        {
            string prefix = "node_";
            if (obj is Material)
            {
                prefix = "material_";
            }
            else if (obj is OperatingUnit)
            {
                prefix = "operatingunit_";
            }
            int i = 1;
            while (i < 1000)
            {
                string validname = ValidateName(null, prefix + i.ToString());
                if (validname != "")
                {
                    return validname;
                }
                i++;
            }
            return "unkonwn";
        }

        public void PaintSolution(int index)
        {
            foreach (Solution s in Solutions)
            {
                if (s.Index == index)
                {
                    PaintSolutionInit(s);
                    foreach (string materialname in s.Materials.Keys)
                    {
                        PaintSolutionItem(materialname, s.Materials[materialname].Flow);
                    }
                    foreach (string operatingunitname in s.OperatingUnits.Keys)
                    {
                        PaintSolutionItem(operatingunitname, s.OperatingUnits[operatingunitname].Size);
                    }
                }
            }
        }

        public void PaintPrintView()
        {
            if (Globals.PrintViewSettings.ShowMaterialText == false)
            {
                foreach (Material m in Materials)
                {
                    m.Title = " ";
                }
            }
            if (Globals.PrintViewSettings.ShowOperatingUnitText == false)
            {
                foreach (OperatingUnit o in OperatingUnits)
                {
                    o.Title = " ";
                }
            }
            if (Globals.PrintViewSettings.ShowEdgeText == false)
            {
                foreach (Edge e in Edges)
                {
                    e.Title = " ";
                }
            }
            if (Globals.PrintViewSettings.ShowComments == false)
            {
                foreach (Material m in Materials)
                {
                    m.CommentText = "";
                }
                foreach (OperatingUnit o in OperatingUnits)
                {
                    o.CommentText = "";
                }
            }
            if (Globals.PrintViewSettings.ShowParameters == false)
            {
                foreach (Material m in Materials)
                {
                    foreach (ObjectProperty op in m.ParameterList.Values)
                    {
                        op.VisibleProp = false;
                    }
                    m.UpdateParametersLabel();
                }
                foreach (OperatingUnit o in OperatingUnits)
                {
                    foreach (ObjectProperty op in o.ParameterList.Values)
                    {
                        op.VisibleProp = false;
                    }
                    o.UpdateParametersLabel();
                }
            }
        }

        public void DoCopy()
        {
            CopiedStructure.Reset();
            foreach (Material m in Materials)
            {
                if (m.IsSelected()){
                    XMLMaterial xmat = new XMLMaterial();
                    xmat.ConvertFromMaterial(m);
                    CopiedStructure.Materials.Add(xmat);
                }
            }
            foreach (OperatingUnit o in OperatingUnits)
            {
                if (o.IsSelected())
                {
                    XMLOperatingUnit xunit = new XMLOperatingUnit();
                    xunit.ConvertFromOperatingUnit(o);
                    CopiedStructure.OperatingUnits.Add(xunit);
                }
            }
            foreach(Edge e in Edges)
            {
                if (e.IsSelected())
                {
                    XMLEdge xedge = new XMLEdge();
                    xedge.ConvertFromEdge(e);
                    CopiedStructure.Edges.Add(xedge);
                }
            }
        }

        public void DoPaste()
        {
            ClearSelection();
            Dictionary<int, int> UniqueIDMap = new Dictionary<int, int>();
            foreach (XMLMaterial xmat in CopiedStructure.Materials)
            {
                Material m = new Material(this);
                xmat.FillData(m);
                m.SetID(GetFreeUniqueID());
                string postfix = GeneratePostfixName(xmat.Name);
                m.Name = xmat.Name + postfix;
                if (m.Title.Trim() != "")
                {
                    m.Title += postfix;
                }
                m.CoordsProp += new System.Drawing.Size(Globals.GridSize, Globals.GridSize);
                xmat.Coords += new System.Drawing.Size(Globals.GridSize, Globals.GridSize);
                m.SetSelected(true);
                AddMaterial(m);
                UniqueIDMap[xmat.ID] = m.GetID();
                xmat.ID = m.GetID();
            }
            foreach (XMLOperatingUnit xunit in CopiedStructure.OperatingUnits)
            {
                OperatingUnit o = new OperatingUnit(this);
                xunit.FillData(o);
                o.SetID(GetFreeUniqueID());
                string postfix = GeneratePostfixName(xunit.Name);
                o.Name = xunit.Name + postfix;
                if (o.Title.Trim() != "")
                {
                    o.Title += postfix;
                }
                o.CoordsProp += new System.Drawing.Size(Globals.GridSize, Globals.GridSize);
                xunit.Coords += new System.Drawing.Size(Globals.GridSize, Globals.GridSize);
                o.SetSelected(true);
                AddOperatingUnit(o);
                UniqueIDMap[xunit.ID] = o.GetID();
                xunit.ID = o.GetID();
            }
            foreach (XMLEdge xedge in CopiedStructure.Edges)
            {
                Edge e = new Edge(this);
                xedge.BeginObjectID = UniqueIDMap[xedge.BeginObjectID];
                xedge.EndObjectID = UniqueIDMap[xedge.EndObjectID];
                xedge.FillData(e);
                foreach (EdgeNode en in e.GetNodes())
                {
                    en.SetID(GetFreeUniqueID());
                    en.CoordsProp += new System.Drawing.Size(Globals.GridSize, Globals.GridSize);
                }
                foreach (XMLEdgeNode xen in xedge.Nodes)
                {
                    xen.Coords += new System.Drawing.Size(Globals.GridSize, Globals.GridSize);
                }
                e.SetID(GetFreeUniqueID());
                AddEdge(e);
            }
        }

        public void ClearSelection()
        {
            foreach (Canvas.IGraphicsObject obj in GraphicsObjectList)
            {
                obj.SetSelected(false);
            }
        }

        public void DoDuplicate()
        {
            DoCopy();
            DoPaste();
        }

        private void PaintSolutionInit(Solution s)
        {
            foreach(Material m in Materials)
            {
                if (s.Materials.ContainsKey(m.Name) == false)
                {
                    m.ColorProp = Globals.SolutionSettings.ExcludedItem.Color;
                    m.LabelColorProp = Globals.SolutionSettings.ExcludedItem.Color;
                    if (Globals.SolutionSettings.ExcludedItem.MaterialText != Globals.SolutionSettings.ValueStyle.Original)
                    {
                        m.Title = " ";
                    }
                }
            }
            foreach (OperatingUnit o in OperatingUnits)
            {
                if (s.OperatingUnits.ContainsKey(o.Name) == false)
                {
                    o.ColorProp = Globals.SolutionSettings.ExcludedItem.Color;
                    o.LabelColorProp = Globals.SolutionSettings.ExcludedItem.Color;
                    if (Globals.SolutionSettings.ExcludedItem.OperatingUnitText != Globals.SolutionSettings.ValueStyle.Original)
                    {
                        o.Title = " ";
                    }
                }
            }
            foreach (Edge e in Edges)
            {
                OperatingUnit o = null;
                if (e.begin is Material && e.end is OperatingUnit)
                {
                    o = e.end as OperatingUnit;
                }
                else if (e.begin is OperatingUnit && e.end is Material)
                {
                    o = e.begin as OperatingUnit;
                }
                if (o != null && s.OperatingUnits.ContainsKey(o.Name) == false)
                {
                    e.ColorProp = Globals.SolutionSettings.ExcludedItem.Color;
                    e.LabelColorProp = Globals.SolutionSettings.ExcludedItem.Color;
                    if (Globals.SolutionSettings.ExcludedItem.EdgeText != Globals.SolutionSettings.ValueStyle.Original)
                    {
                        e.Title = " ";
                    }
                }
            }


            // TODO - solution settings window-ba betenni
            if (true)
            {
                foreach (Material m in Materials)
                {
                    m.CommentText = "";
                }
                foreach (OperatingUnit o in OperatingUnits)
                {
                    o.CommentText = "";
                }
            }
            if (true)
            {
                foreach (Material m in Materials)
                {
                    foreach (ObjectProperty op in m.ParameterList.Values)
                    {
                        op.VisibleProp = false;
                    }
                    m.UpdateParametersLabel();
                }
                foreach (OperatingUnit o in OperatingUnits)
                {
                    foreach (ObjectProperty op in o.ParameterList.Values)
                    {
                        op.VisibleProp = false;
                    }
                    o.UpdateParametersLabel();
                }
            }
        }

        private void PaintSolutionItem(string name, double value)
        {
            if (GetMaterialByName(name) != null)
            {
                Material m = GetMaterialByName(name);
                m.ColorProp = Globals.SolutionSettings.IncludedItem.Color;
                m.LabelColorProp = Globals.SolutionSettings.IncludedItem.Color;
                if (Globals.SolutionSettings.IncludedItem.MaterialText == Globals.SolutionSettings.ValueStyle.Calculated)
                {
                    m.Title = m.DisplayedText + "(" + value.ToString("0.#####") + ")";
                }
                else if (Globals.SolutionSettings.IncludedItem.MaterialText == Globals.SolutionSettings.ValueStyle.None)
                {
                    m.Title = " ";
                }
            }
            else if (GetOperatingUnitByName(name) != null)
            {
                OperatingUnit o = GetOperatingUnitByName(name);
                o.ColorProp = Globals.SolutionSettings.IncludedItem.Color;
                o.LabelColorProp = Globals.SolutionSettings.IncludedItem.Color;

                if (Globals.SolutionSettings.IncludedItem.OperatingUnitText == Globals.SolutionSettings.ValueStyle.Calculated)
                {
                    o.Title = o.DisplayedText + "(" + value.ToString("0.#####") + ")";
                }
                else if (Globals.SolutionSettings.IncludedItem.OperatingUnitText == Globals.SolutionSettings.ValueStyle.None)
                {
                    o.Title = " ";
                }

                foreach (Edge e in Edges)
                {
                    bool ok = false;
                    if (e.begin is Material && e.end is OperatingUnit && e.end == o)
                    {
                        ok = true;
                    }
                    else if (e.begin is OperatingUnit && e.end is Material && e.begin == o)
                    {
                        ok = true;
                    }
                    if (ok)
                    {
                        e.ColorProp = Globals.SolutionSettings.IncludedItem.Color;
                        e.LabelColorProp = Globals.SolutionSettings.IncludedItem.Color;

                        if (Globals.SolutionSettings.IncludedItem.EdgeText == Globals.SolutionSettings.ValueStyle.Calculated)
                        {
                            e.Title = (value*e.Rate).ToString("0.#####");
                        }
                        else if (Globals.SolutionSettings.IncludedItem.EdgeText == Globals.SolutionSettings.ValueStyle.None)
                        {
                            e.Title = " ";
                        }
                    }
                }
            }
        }

        public void ParseSolution(string t_str, int limit)
        {
            Solutions.Clear();

            string t_rate_str, t_opunit_str, t_mat_str, t_amount_str;
            int t_begin = 0, t_pos, t_opunits_pos, t_annual_pos, t_index = 0;
            double t_cost;
            Solution solution;

            //----------- Maximal Structure -------------
            if (t_str.IndexOf("Maximal Structure", t_begin) != -1)
            {
                solution = new Solution(t_index++, "Maximal structure");
                t_begin = t_str.IndexOf("Maximal Structure", t_begin);
                if (t_begin == -1)
                {
                    return;
                }
                t_begin = t_str.IndexOf("Materials", t_begin);
                t_begin = t_str.IndexOf("\n", t_begin);
                t_begin++;
                t_pos = t_str.IndexOf("Operating units", t_begin);
                string[] matlist = (t_str.Substring(t_begin, t_pos - t_begin)).Split(',');
                foreach (string item in matlist)
                {
                    solution.AddMaterial(item.Trim(), 1);
                }
                t_begin = t_str.IndexOf("Operating units", t_begin);
                t_begin = t_str.IndexOf("\n", t_begin);
                t_begin++;
                t_pos = t_str.IndexOf("Solution structure", t_begin);
                Console.WriteLine("t_pos: " + t_pos + ", t_begin: " + t_begin);
                string[] opunitlist;
                if (t_pos != -1)
                {
                    opunitlist = (t_str.Substring(t_begin, t_pos - t_begin)).Split(',');
                }
                else
                {
                    t_pos = t_str.IndexOf("Feasible structure", t_begin);
                    if (t_pos != -1)
                    {
                        opunitlist = (t_str.Substring(t_begin, t_pos - t_begin)).Split(',');
                    }
                    else
                    {
                        opunitlist = (t_str.Substring(t_begin, t_str.Length - 6 - t_begin)).Split(',');
                    }
                }
                foreach (string item in opunitlist)
                {
                    solution.AddOperatingUnit(item.Trim(), 1);
                }
                Solutions.Add(solution); 
            }
            //-------------------------------------------

            //-----------------SSG----SSG+LP-------------
            if (t_str.IndexOf("Feasible structure", 0) == -1)
            {
                t_pos = t_str.IndexOf("Solution structure", 0);
                int i = 1;
                while (i <= limit && t_pos < t_str.Length - 6 && t_pos != -1)
                {
                    solution = new Solution(t_index, "Solution structure #" + t_index.ToString());
                    t_index++;
                    //Console.WriteLine("Hanyadik: " + i);
                    t_begin = t_str.IndexOf("Solution structure #" + i.ToString(), 0);
                    t_begin = t_str.IndexOf("Materials", t_begin);
                    t_begin = t_str.IndexOf("\n", t_begin);
                    t_begin++;
                    t_opunits_pos = t_str.IndexOf("Operating units", t_begin);
                        string[] solmatlist = (t_str.Substring(t_begin, t_opunits_pos - t_begin)).Split(',');
                        foreach (string item in solmatlist)
                        {
                            solution.AddMaterial(item.Trim(), 1);
                        }
                    t_begin = t_opunits_pos;
                    t_begin = t_str.IndexOf("\n", t_begin);
                    t_begin++;
                    t_pos = t_str.IndexOf("Solution structure #" + (i + 1).ToString(), 0);
                    if (i == limit || t_pos == -1)
                    {
                        t_pos = t_str.Length - 6;
                    }
                    //Console.WriteLine("t_pos: " + t_pos);
                    string[] sol_opunitlist = (t_str.Substring(t_begin, t_pos - t_begin)).Split(',');
                    foreach (string item in sol_opunitlist)
                    {
                        solution.AddOperatingUnit(item.Trim(), 1);
                    }
                    Solutions.Add(solution);
                    i++;
                } 
            }
            //-------------------------------------------

            //-----------------MSG----ABB----------------
            else
            {
                while ((t_begin = t_str.IndexOf("Feasible structure", t_begin)) != -1)
                {
                    solution = new Solution(t_index, "Feasible structure #" + t_index.ToString());
                    t_index++;
                    t_pos = t_str.IndexOf("Materials", t_begin);
                    t_opunits_pos = t_str.IndexOf("Operating units", t_begin);
                    t_cost = 0;
                    if ((t_annual_pos = t_str.IndexOf("Total annual cost", t_begin)) != -1)
                    {
                        int pos = t_str.IndexOf("= ", t_annual_pos);
                        pos = pos + 2;
                        int endpos = t_str.IndexOf(" ", pos);
                        string value = t_str.Substring(pos, endpos - pos);
                        t_cost = Convert.ToDouble(value, CultureInfo.GetCultureInfo("en-GB"));
                    }
                    t_begin = t_pos;
                    if (t_begin != -1)
                    {
                        do
                        {
                            t_pos = t_begin;
                            while (t_str.Length > t_pos && t_str[t_pos] != '\n') t_pos++;
                            if (t_opunits_pos > t_pos + 1)
                            {
                                t_begin = t_pos + 1;
                                int t_pos_colon = t_str.IndexOf(':', t_begin);
                                t_pos = t_str.IndexOf('(', t_begin);
                                if (t_pos == -1 || t_pos > t_pos_colon) t_pos = t_pos_colon + 1;
                                t_mat_str = t_str.Substring(t_begin, t_pos - 1 - t_begin);
                                t_begin = t_pos_colon + 2;
                                t_pos = t_str.IndexOf('\n', t_begin);
                                t_amount_str = t_str.Substring(t_begin, t_pos - t_begin);
                                t_begin = t_pos;

                                //t_solution.AddMaterial(PnsEditor.FindMaterial(t_mat_str), Convert.ToDouble(t_amount_str));

                                if (GetMaterialByName(t_mat_str) != null)
                                {
                                    if (t_amount_str == "balanced")
                                    {
                                        t_amount_str = "0";
                                    }
                                    solution.AddMaterial(t_mat_str, ConvertManager.ToDouble(t_amount_str, false));
                                }

                            }
                        } while (t_opunits_pos > t_pos + 1);
                    }
                    t_begin = t_opunits_pos;
                    if (t_begin != -1)
                    {
                        do
                        {
                            t_pos = t_begin;
                            while (t_annual_pos > t_pos + 1 && !(t_str[t_pos] == '\n' && t_str[t_pos + 1] >= '0' && t_str[t_pos + 1] <= '9')) t_pos++;
                            if (t_annual_pos > t_pos + 1)
                            {
                                t_begin = t_pos + 1;
                                t_pos = t_str.IndexOf('*', t_begin);
                                t_rate_str = t_str.Substring(t_begin, t_pos - t_begin);
                                t_begin = t_pos + 1;
                                t_pos = t_str.IndexOf('(', t_begin);
                                t_opunit_str = t_str.Substring(t_begin, t_pos - 1 - t_begin);
                                t_begin = t_pos + 1;
                                //t_solution.AddOpUnit(PnsEditor.FindOperatingUnit(t_opunit_str), Convert.ToDouble(t_rate_str));

                                if (GetOperatingUnitByName(t_opunit_str) != null)
                                {
                                    solution.AddOperatingUnit(t_opunit_str, ConvertManager.ToDouble(t_rate_str, false));
                                }
                            }
                        } while (t_annual_pos > t_pos + 1);
                    }
                    solution.OptimalValue = t_cost;
                    Solutions.Add(solution);
                }
            }
            //-------------------------------------------
        }

        public override int GetHashCode()
        {
            int code = 0;

            for (int i = 0; i < Default.mass_mu.ToString().Length; i++)
            {
                code += Char.ConvertToUtf32(Default.mass_mu.ToString(), i);
            }

            for (int i = 0; i < Default.money_mu.ToString().Length; i++)
            {
                code += Char.ConvertToUtf32(Default.money_mu.ToString(), i);
            }

            for (int i = 0; i < Default.time_mu.ToString().Length; i++)
            {
                code += Char.ConvertToUtf32(Default.time_mu.ToString(), i);
            }

            code += Materials.Count;
            code += OperatingUnits.Count;
            code += Edges.Count;

            code = code << 2;

            for (int i = 0; i < Materials.Count; i++)
            {
                code += Materials[i].Name.Length;
                code += Materials[i].Type;
                code += (int)Materials[i].ParameterList["price"].Value;
                code += (int)Materials[i].ParameterList["maxflow"].Value;
                code += (int)Materials[i].ParameterList["reqflow"].Value;

                code += GetMUASCII(Materials[i].ParameterList["price"]);
                code += GetMUASCII(Materials[i].ParameterList["maxflow"]);
                code += GetMUASCII(Materials[i].ParameterList["reqflow"]);
            }

            code = code << 4;

            for (int i = 0; i < OperatingUnits.Count; i++)
            {
                code += OperatingUnits[i].Name.Length;
                code += (int)OperatingUnits[i].ParameterList["caplower"].Value;
                code += (int)OperatingUnits[i].ParameterList["capupper"].Value;
                code += (int)(OperatingUnits[i].ParameterList["investcostfix"].Value + OperatingUnits[i].ParameterList["opercostfix"].Value);
                code += (int)(OperatingUnits[i].ParameterList["investcostprop"].Value + OperatingUnits[i].ParameterList["opercostprop"].Value);

                code += GetMUASCII(OperatingUnits[i].ParameterList["caplower"]);
                code += GetMUASCII(OperatingUnits[i].ParameterList["capupper"]);
                code += GetMUASCII(OperatingUnits[i].ParameterList["investcostfix"]);
                code += GetMUASCII(OperatingUnits[i].ParameterList["opercostfix"]);
                code += GetMUASCII(OperatingUnits[i].ParameterList["investcostprop"]);
                code += GetMUASCII(OperatingUnits[i].ParameterList["opercostprop"]);
            }

            code = code << 2;

            for (int i = 0; i < Edges.Count; i++)
            {
                code += (int)Edges[i].Rate;
            }

            code = code << 4;

            return code;
        }

        public int GetMUASCII(ObjectProperty prop)
        {
            int code = 0;
            for (int i = 0; i < prop.MU.Length; i++)
            {
                code += Char.ConvertToUtf32(prop.MU, i);
            }

            return code;
        }
    }
}
