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
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using System.Drawing;
using GraphVizWrapper.Queries;
using GraphVizWrapper.Commands;
using GraphVizWrapper;

using PNSDraw.Configuration;

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

        List<string> UndoXML = new List<string>();
        List<string> RedoXML = new List<string>();
        string CurrentXML = "";

        public int GetFreeUniqueID()
        {
            return ++UniqueID;
        }

        public void CreateUndo()
        {
            if (UndoXML.Count > Config.Instance.MaxUndo)
            {
                UndoXML.RemoveAt(0);
            }
            UndoXML.Add(CurrentXML);
            CurrentXML = ExportToXML();
        }

        public int DoUndo()
        {
            if (UndoXML.Count > 1)
            {
                RedoXML.Add(ExportToXML());
                ImportFromXML(UndoXML[UndoXML.Count - 1]);
                CurrentXML = UndoXML[UndoXML.Count - 1];
                UndoXML.RemoveAt(UndoXML.Count - 1);
            }
            else
            {
                UndoXML.Clear();
            }
            return UndoXML.Count;
        }

        public int DoRedo()
        {
            if (RedoXML.Count > 0)
            {
                UndoXML.Add(ExportToXML());
                ImportFromXML(RedoXML[RedoXML.Count - 1]);
                CurrentXML = RedoXML[RedoXML.Count - 1];
                RedoXML.RemoveAt(RedoXML.Count - 1);
            }
            else
            {
                RedoXML.Clear();
            }
            return RedoXML.Count;
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
            UndoXML.Clear();
            RedoXML.Clear();
            CurrentXML = "";
            Config.Instance.GraphSettings.Reset();
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
                
                if (m.Type == MaterialTypes.Raw)
                {
                    rawmat.Add(tmat);
                }
                else if (m.Type == MaterialTypes.Intermediate)
                {
                    intermediatemat.Add(tmat);
                }
                else if (m.Type == MaterialTypes.Product)
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
                tmat.Rate = e.Rate;
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

        public bool ImportFromPNS(string xml)
        {
            Clear();
            XmlSerializer xs = new XmlSerializer(typeof(XMLPNS));
            XMLPNS pgraph = (XMLPNS)xs.Deserialize(new StringReader(xml));
            foreach (XMLPNSMaterial mat in pgraph.Problem.Materials.Raw)
            {
                Material m = new Material(this);
                m.Name = mat.Name;
                m.Type = MaterialTypes.Raw;

                m.ParameterList["price"].Value = mat.Price != null ? ConvertManager.ToDouble(mat.Price) : Config.Instance.Material.Price;
                m.ParameterList["reqflow"].Value = mat.Min != null ? ConvertManager.ToDouble(mat.Min) : Config.Instance.Material.FlowRateLowerBound;
                m.ParameterList["maxflow"].Value = mat.Max != null ? ConvertManager.ToDouble(mat.Max) : Config.Instance.Material.FlowRateUpperBound;

                AddMaterial(m);
            }

            foreach (XMLPNSMaterial mat in pgraph.Problem.Materials.Intermediate)
            {
                Material m = new Material(this);
                m.Name = mat.Name;
                m.Type = MaterialTypes.Intermediate;

                m.ParameterList["price"].Value = mat.Price != null ? ConvertManager.ToDouble(mat.Price) : Config.Instance.Material.Price;
                m.ParameterList["reqflow"].Value = mat.Min != null ? ConvertManager.ToDouble(mat.Min) : Config.Instance.Material.FlowRateLowerBound;
                m.ParameterList["maxflow"].Value = mat.Max != null ? ConvertManager.ToDouble(mat.Max) : Config.Instance.Material.FlowRateUpperBound;

                AddMaterial(m);
            }

            foreach (XMLPNSMaterial mat in pgraph.Problem.Materials.Product)
            {
                Material m = new Material(this);
                m.Name = mat.Name;
                m.Type = MaterialTypes.Product;

                m.ParameterList["price"].Value = mat.Price != null ? ConvertManager.ToDouble(mat.Price) : Config.Instance.Material.Price;
                m.ParameterList["reqflow"].Value = mat.Min != null ? ConvertManager.ToDouble(mat.Min) : Config.Instance.Material.FlowRateLowerBound;
                m.ParameterList["maxflow"].Value = mat.Max != null ? ConvertManager.ToDouble(mat.Max) : Config.Instance.Material.FlowRateUpperBound;

                AddMaterial(m);
            }

            foreach (XMLPNSOperatingUnit oxml in pgraph.Problem.OperatingUnits)
            {
                OperatingUnit o = new OperatingUnit(this);
                /*top.Name = o.Name;
                top.CapacityLower = o.ParameterList["caplower"].ToPNSValue();
                top.CapacityUpper = o.ParameterList["capupper"].ToPNSValue();
                top.InvestmentFix = o.ParameterList["investcostfix"].ToPNSValue();
                top.InvestmentProp = o.ParameterList["investcostprop"].ToPNSValue();
                top.OperatingFix = o.ParameterList["opercostfix"].ToPNSValue();
                top.OperatingProp = o.ParameterList["opercostprop"].ToPNSValue();
                top.PayoutPeriod = o.ParameterList["payoutperiod"].ToPNSValue();
                top.WorkingHours = o.ParameterList["workinghour"].ToPNSValue();
                ounits[o.GetID()] = top;*/
                o.Name = oxml.Name;
                o.ParameterList["caplower"].Value = oxml.CapacityLower != null ? ConvertManager.ToDouble(oxml.CapacityLower) : Config.Instance.OperatingUnit.CapacityLowerBound;
                o.ParameterList["capupper"].Value = oxml.CapacityUpper != null ? ConvertManager.ToDouble(oxml.CapacityUpper) : Config.Instance.OperatingUnit.CapacityUpperBound;
                o.ParameterList["investcostfix"].Value = oxml.InvestmentFix != null ? ConvertManager.ToDouble(oxml.InvestmentFix) : Config.Instance.OperatingUnit.InvestmentFixCost;
                o.ParameterList["investcostprop"].Value = oxml.InvestmentProp != null ? ConvertManager.ToDouble(oxml.InvestmentProp) : Config.Instance.OperatingUnit.InvestmentPropCost;
                o.ParameterList["opercostfix"].Value = oxml.OperatingFix != null ? ConvertManager.ToDouble(oxml.OperatingFix) : Config.Instance.OperatingUnit.OperatingFixCost;
                o.ParameterList["opercostprop"].Value = oxml.OperatingProp != null ? ConvertManager.ToDouble(oxml.OperatingProp) : Config.Instance.OperatingUnit.OperatingPropCost;
                o.ParameterList["payoutperiod"].Value = oxml.PayoutPeriod != null ? ConvertManager.ToDouble(oxml.PayoutPeriod) : Config.Instance.OperatingUnit.PayoutPeriod;
                o.ParameterList["workinghour"].Value = oxml.WorkingHours != null ? ConvertManager.ToDouble(oxml.WorkingHours) : Config.Instance.OperatingUnit.WorkingHoursPerYear;

                AddOperatingUnit(o);

                foreach (XMLPNSOperatingUnitMaterial edge in oxml.Input)
                {
                    Edge e = new Edge(this);
                    Material m = Materials.Find(x=>x.Name.Equals(edge.Name));
                    e.begin = m;
                    e.end = o;
                    e.Rate = edge.Rate;
                    AddEdge(e);
                }

                foreach (XMLPNSOperatingUnitMaterial edge in oxml.Output)
                {
                    Edge e = new Edge(this);
                    Material m = Materials.Find(x => x.Name.Equals(edge.Name));
                    e.end = m;
                    e.begin = o;
                    e.Rate = edge.Rate;
                    AddEdge(e);
                }
            }

            return true;
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

            pgraph.Settings.GridSize = Config.Instance.GraphSettings.GridSize;
            pgraph.Settings.DefaultFontSize = Config.Instance.GraphSettings.DefaultFontSize;

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

            Config.Instance.GraphSettings.GridSize = pgraph.Settings.GridSize;
            Config.Instance.GraphSettings.DefaultFontSize = pgraph.Settings.DefaultFontSize;

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

        public void RefreshEdgesTitleFormat()
        {
            foreach (Edge e in Edges)
            {
                e.LabelProp.LongFormatProp = Config.Instance.GraphSettings.ShowEdgeLongFormat;
                e.UpdateParametersLabel();
            }
        }

        public void RefreshConnectedEdgesTitle(Canvas.IGraphicsObject obj)
        {
            List<Edge> connected = GetConnectedEdges(obj);
            foreach (Edge e in connected)
            {
                e.UpdateParametersLabel();
            }
        }

        public void AddEdge(Edge edge)
        {
            if (Edges.Contains(edge) == false)
            {
                string matName = "";
                if (edge.begin is Material)
                {
                    Material material = (Material)edge.begin;
                    matName = material.Name;
                    edge.MU = material.ParameterList["reqflow"].MU;
                }
                else if (edge.end is Material)
                {
                    Material material = (Material)edge.end;
                    matName = material.Name;
                    edge.MU = material.ParameterList["reqflow"].MU;
                }

                edge.LabelProp.LongFormatProp = Config.Instance.GraphSettings.ShowEdgeLongFormat;

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
            while (i < 100000)
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
            if (Config.Instance.GraphSettings.ShowMaterialText == false)
            {
                foreach (Material m in Materials)
                {
                    m.Title = " ";
                }
            }
            if (Config.Instance.GraphSettings.ShowOperatingUnitText == false)
            {
                foreach (OperatingUnit o in OperatingUnits)
                {
                    o.Title = " ";
                }
            }
            if (Config.Instance.GraphSettings.ShowEdgeText == false)
            {
                foreach (Edge e in Edges)
                {
                    e.Title = " ";
                }
            }
            if (Config.Instance.GraphSettings.ShowComments == false)
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
            if (Config.Instance.GraphSettings.ShowParameters == false)
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
                m.NodeStyleProp.CoordsProp += new System.Drawing.Size(Config.Instance.GraphSettings.GridSize, Config.Instance.GraphSettings.GridSize);
                xmat.Coords += new System.Drawing.Size(Config.Instance.GraphSettings.GridSize, Config.Instance.GraphSettings.GridSize);
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
                o.NodeStyleProp.CoordsProp += new System.Drawing.Size(Config.Instance.GraphSettings.GridSize, Config.Instance.GraphSettings.GridSize);
                xunit.Coords += new System.Drawing.Size(Config.Instance.GraphSettings.GridSize, Config.Instance.GraphSettings.GridSize);
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
                    en.CoordsProp += new System.Drawing.Size(Config.Instance.GraphSettings.GridSize, Config.Instance.GraphSettings.GridSize);
                }
                foreach (XMLEdgeNode xen in xedge.Nodes)
                {
                    xen.Coords += new System.Drawing.Size(Config.Instance.GraphSettings.GridSize, Config.Instance.GraphSettings.GridSize);
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
                    m.NodeStyleProp.ColorProp = Config.Instance.SolutionSettings.ExcludedItem.Color;
                    m.LabelProp.ColorProp = Config.Instance.SolutionSettings.ExcludedItem.Color;
                    if (Config.Instance.SolutionSettings.ExcludedItem.MaterialText != SolutionSettings.ValueStyle.Original)
                    {
                        m.Title = " ";
                    }
                }
            }
            foreach (OperatingUnit o in OperatingUnits)
            {
                if (s.OperatingUnits.ContainsKey(o.Name) == false)
                {
                    o.NodeStyleProp.ColorProp = Config.Instance.SolutionSettings.ExcludedItem.Color;
                    o.LabelProp.ColorProp = Config.Instance.SolutionSettings.ExcludedItem.Color;
                    if (Config.Instance.SolutionSettings.ExcludedItem.OperatingUnitText != SolutionSettings.ValueStyle.Original)
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
                    e.EdgeStyle.ColorProp = Config.Instance.SolutionSettings.ExcludedItem.Color;
                    e.LabelProp.ColorProp = Config.Instance.SolutionSettings.ExcludedItem.Color;
                    if (Config.Instance.SolutionSettings.ExcludedItem.EdgeText != SolutionSettings.ValueStyle.Original)
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
                m.NodeStyleProp.ColorProp = Config.Instance.SolutionSettings.IncludedItem.Color;
                m.LabelProp.ColorProp = Config.Instance.SolutionSettings.IncludedItem.Color;
                if (Config.Instance.SolutionSettings.IncludedItem.MaterialText == SolutionSettings.ValueStyle.Calculated)
                {
                    m.Title = m.DisplayedText + "(" + value.ToString("0.#####") + ")";
                }
                else if (Config.Instance.SolutionSettings.IncludedItem.MaterialText == SolutionSettings.ValueStyle.None)
                {
                    m.Title = " ";
                }
            }
            else if (GetOperatingUnitByName(name) != null)
            {
                OperatingUnit o = GetOperatingUnitByName(name);
                o.NodeStyleProp.ColorProp = Config.Instance.SolutionSettings.IncludedItem.Color;
                o.LabelProp.ColorProp = Config.Instance.SolutionSettings.IncludedItem.Color;

                if (Config.Instance.SolutionSettings.IncludedItem.OperatingUnitText == SolutionSettings.ValueStyle.Calculated)
                {
                    o.Title = o.DisplayedText + "(" + value.ToString("0.#####") + ")";
                }
                else if (Config.Instance.SolutionSettings.IncludedItem.OperatingUnitText == SolutionSettings.ValueStyle.None)
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
                        e.EdgeStyle.ColorProp = Config.Instance.SolutionSettings.IncludedItem.Color;
                        e.LabelProp.ColorProp = Config.Instance.SolutionSettings.IncludedItem.Color;

                        if (Config.Instance.SolutionSettings.IncludedItem.EdgeText == SolutionSettings.ValueStyle.Calculated)
                        {
                            /*string mu = "f";
                            if (e.begin is Material)
                            {
                                Material m = (Material)e.begin;
                                mu = m.ParameterList["reqflow"].MU.ToString();
                            }
                            else if (e.end is Material)
                            {
                                Material m = (Material)e.end;
                                mu = m.ParameterList["reqflow"].MU.ToString();
                            }*/

                            e.Title = (value * e.Rate).ToString("0.#####") + " " + e.MU;
                        }
                        else if (Config.Instance.SolutionSettings.IncludedItem.EdgeText == SolutionSettings.ValueStyle.None)
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

            for (int i = 0; i < Config.Instance.Quantity.mass_mu.ToString().Length; i++)
            {
                code += Char.ConvertToUtf32(Config.Instance.Quantity.mass_mu.ToString(), i);
            }

            for (int i = 0; i < Config.Instance.Quantity.money_mu.ToString().Length; i++)
            {
                code += Char.ConvertToUtf32(Config.Instance.Quantity.money_mu.ToString(), i);
            }

            for (int i = 0; i < Config.Instance.Quantity.time_mu.ToString().Length; i++)
            {
                code += Char.ConvertToUtf32(Config.Instance.Quantity.time_mu.ToString(), i);
            }

            code += Materials.Count;
            code += OperatingUnits.Count;
            code += Edges.Count;
            code += MutualExclusions.Count;

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

            for (int i = 0; i < MutualExclusions.Count; i++)
            {
                for (int j = 0; j < MutualExclusions[i].Name.Length; j++)
                {
                    code += Char.ConvertToUtf32(MutualExclusions[i].Name, j);
                }
            }

            code = code << 2;

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

        public string ConvertDOTFormat(bool weighted_arcs)
        {
            string DOT = "digraph{ ";
            if (Config.Instance.LayoutSettings.Align == "Horizontal")
            {
                DOT += "rankdir=LR; ";
            }

            foreach (Edge e in Edges)
            {
                if (e.begin is Material && e.end is OperatingUnit)
                {
                    Material m = e.begin as Material;
                    OperatingUnit o = e.end as OperatingUnit;
                    DOT += m.Name;
                    DOT += " -> ";
                    DOT += o.Name;
                    if (weighted_arcs)
                    {
                        if (m.GetCoords().Y < o.GetCoords().Y)
                        {
                            DOT += "[weight=10]";
                        }
                    }
                    DOT += " ; ";
                }
                else if (e.begin is OperatingUnit && e.end is Material)
                {
                    Material m = e.end as Material;
                    OperatingUnit o = e.begin as OperatingUnit;
                    DOT += o.Name;
                    DOT += " -> ";
                    DOT += m.Name;
                    if (weighted_arcs)
                    {
                        if (m.GetCoords().Y > o.GetCoords().Y)
                        {
                            DOT += "[weight=10]";
                        }
                    }
                    DOT += " ; ";
                }
            }
            if (Config.Instance.LayoutSettings.FixedRaws)
            {
                DOT += " { rank=min; ";
                foreach (Material m in Materials)
                {
                    if (m.Type == MaterialTypes.Raw)
                    {
                        DOT += m.Name;
                        DOT += " ";
                    }
                }
                DOT += "} ";
            }
            if (Config.Instance.LayoutSettings.FixedProducts)
            {
                DOT += " { rank=max; ";
                foreach (Material m in Materials)
                {
                    if (m.Type == MaterialTypes.Product)
                    {
                        DOT += m.Name;
                        DOT += " ";
                    }
                }
                DOT += "} ";
            }



            DOT += " }";
            Console.WriteLine(DOT);
            return DOT;
        }

        public void modifyGraph(string name, int x, int y)
        {
            foreach (Material m in Materials)
            {
                if (String.Compare(m.Name, name) == 0)
                {
                    if (m.Fixed_position == 0)
                    {
                        Point p = new Point(x, y);
                        m.SetCoords(p);
                    }

                }

            }
            foreach (OperatingUnit o in OperatingUnits)
            {
                if (String.Compare(o.Name, name) == 0)
                {
                    if (o.Fixed_position == 0)
                    {
                        Point p = new Point(x, y);
                        o.SetCoords(p);
                    }

                }

            }
        }

        public void addEdgeNodes()
        {
            Random random = new Random();
            double shift = 1.5;
            foreach (Edge e in Edges)
            {
                if (e.begin is Material && e.end is OperatingUnit)
                {
                    Material m = e.begin as Material;
                    OperatingUnit o = e.end as OperatingUnit;
                    if (m.GetCoords().Y > o.GetCoords().Y)
                    {
                        List<EdgeNode> enodes = new List<EdgeNode>();
                        EdgeNode en1 = new EdgeNode(e.GetContainer(), e);
                        EdgeNode en2 = new EdgeNode(e.GetContainer(), e);
                        en1.Needed = true;
                        en2.Needed = true;
                        en1.Temporary = false;
                        en2.Temporary = false;
                        if (o.GetCoords().X <= m.GetCoords().X)
                        {
                            Console.WriteLine("1");
                            shift = random.NextDouble() * 0.5 + 1.25;
                            en1.CoordsProp = new Point(o.GetCoords().X - Convert.ToInt32(shift * Config.Instance.GraphSettings.GridSize), m.GetCoords().Y + Config.Instance.GraphSettings.GridSize);
                            en2.CoordsProp = new Point(o.GetCoords().X - Convert.ToInt32(shift * Config.Instance.GraphSettings.GridSize), o.GetCoords().Y - Config.Instance.GraphSettings.GridSize);
                        }
                        else
                        {
                            Console.WriteLine("2");
                            shift = random.NextDouble() * 0.5 + 1.25;
                            en1.CoordsProp = new Point(o.GetCoords().X + Convert.ToInt32(shift * Config.Instance.GraphSettings.GridSize), m.GetCoords().Y + Config.Instance.GraphSettings.GridSize);
                            en2.CoordsProp = new Point(o.GetCoords().X + Convert.ToInt32(shift * Config.Instance.GraphSettings.GridSize), o.GetCoords().Y - Config.Instance.GraphSettings.GridSize);
                        }
                        //en1.CoordsProp = new Point(m.GetCoords().X-2*Globals.GridSize,m.GetCoords().Y+Globals.GridSize);
                        //en2.CoordsProp = new Point(o.GetCoords().X - 2 * Globals.GridSize, o.GetCoords().Y-Globals.GridSize);
                        enodes.Add(en1);
                        enodes.Add(en2);
                        e.SetNodes(enodes);
                    }
                }
                else if (e.begin is OperatingUnit && e.end is Material)
                {
                    Material m = e.end as Material;
                    OperatingUnit o = e.begin as OperatingUnit;
                    if (m.GetCoords().Y < o.GetCoords().Y)
                    {
                        List<EdgeNode> enodes = new List<EdgeNode>();
                        EdgeNode en1 = new EdgeNode(e.GetContainer(), e);
                        EdgeNode en2 = new EdgeNode(e.GetContainer(), e);
                        en1.Needed = true;
                        en2.Needed = true;
                        en1.Temporary = false;
                        en2.Temporary = false;
                        if (m.GetCoords().X <= o.GetCoords().X)
                        {
                            Console.WriteLine("3");
                            shift = random.NextDouble() * 0.5 + 1.25;
                            en1.CoordsProp = new Point(m.GetCoords().X - Convert.ToInt32(shift * Config.Instance.GraphSettings.GridSize), o.GetCoords().Y + Config.Instance.GraphSettings.GridSize);
                            en2.CoordsProp = new Point(m.GetCoords().X - Convert.ToInt32(shift * Config.Instance.GraphSettings.GridSize), m.GetCoords().Y - Config.Instance.GraphSettings.GridSize);
                        }
                        else
                        {
                            Console.WriteLine("4");
                            shift = random.NextDouble() * 0.5 + 1.25;
                            en1.CoordsProp = new Point(m.GetCoords().X + Convert.ToInt32(shift * Config.Instance.GraphSettings.GridSize), o.GetCoords().Y + Config.Instance.GraphSettings.GridSize);
                            en2.CoordsProp = new Point(m.GetCoords().X + Convert.ToInt32(shift * Config.Instance.GraphSettings.GridSize), m.GetCoords().Y - Config.Instance.GraphSettings.GridSize);
                        }
                        //en1.CoordsProp = new Point(o.GetCoords().X - 2 * Globals.GridSize, o.GetCoords().Y + Globals.GridSize);
                        //en2.CoordsProp = new Point(m.GetCoords().X - 2 * Globals.GridSize, m.GetCoords().Y - Globals.GridSize);
                        enodes.Add(en1);
                        enodes.Add(en2);
                        e.SetNodes(enodes);
                    }
                }
            }
        }

        public void removeEdgeNodes()
        {
            foreach (Edge e in Edges)
            {
                List<EdgeNode> enodes = e.GetNodes();
                foreach (EdgeNode en in enodes)
                {
                    if (((Canvas.IGraphicsObject)e.begin).getPin() == 0 || ((Canvas.IGraphicsObject)e.end).getPin() == 0)
                    {
                        e.RemoveNode(en);
                    }
                }
            }
        }

        public void GenerateLayout()
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var wrapper = new GraphGeneration(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand);
            
            if (Config.Instance.LayoutSettings.SelectedEngine == "Neato")
            {
                wrapper.RenderingEngine = Enums.RenderingEngine.Neato;
            }
            else
            {
                if (Config.Instance.LayoutSettings.SelectedEngine == "Fdp")
                {
                    wrapper.RenderingEngine = Enums.RenderingEngine.Fdp;
                }
                else
                {
                    if (Config.Instance.LayoutSettings.SelectedEngine == "Sfdp")
                    {
                        wrapper.RenderingEngine = Enums.RenderingEngine.Sfdp;
                    }
                    else
                    {
                        if (Config.Instance.LayoutSettings.SelectedEngine == "Twopi")
                        {
                            wrapper.RenderingEngine = Enums.RenderingEngine.Twopi;
                        }
                        else
                        {
                            if (Config.Instance.LayoutSettings.SelectedEngine == "Circo")
                            {
                                wrapper.RenderingEngine = Enums.RenderingEngine.Circo;
                            }
                            else
                            {
                                wrapper.RenderingEngine = Enums.RenderingEngine.Dot;
                            }
                        }
                    }
                }
            }
            byte[] output = wrapper.GenerateGraph(ConvertDOTFormat(false), Enums.GraphReturnType.plain);
            var stream = new StreamReader(new MemoryStream(output));
            string line;
            while ((line = stream.ReadLine()) != null)
            {
                string[] words = line.Split(' ');
                if (String.Compare(words[0], "node") == 0)
                {
                    modifyGraph(words[1],
                                        Convert.ToInt32(Convert.ToDouble(words[2], CultureInfo.InvariantCulture)) * Config.Instance.LayoutSettings.DefaultNodeDistance * Config.Instance.GraphSettings.GridSize + Config.Instance.LayoutSettings.DefaultRootX * Config.Instance.GraphSettings.GridSize,
                                        Convert.ToInt32(Convert.ToDouble(words[3], CultureInfo.InvariantCulture)) * Config.Instance.LayoutSettings.DefaultLayerDistance * Config.Instance.GraphSettings.GridSize + Config.Instance.LayoutSettings.DefaultRootY * Config.Instance.GraphSettings.GridSize);
                }
            }
            removeEdgeNodes();

            if (Config.Instance.LayoutSettings.WeightedArcs)
            {
                //lemeno elek nagyobb sullyal
                byte[] output2 = wrapper.GenerateGraph(ConvertDOTFormat(true), Enums.GraphReturnType.plain);
                var stream2 = new StreamReader(new MemoryStream(output2));
                string line2;
                while ((line2 = stream2.ReadLine()) != null)
                {
                    string[] words = line2.Split(' ');
                    if (String.Compare(words[0], "node") == 0)
                    {
                        modifyGraph(words[1],
                                    Convert.ToInt32(Convert.ToDouble(words[2], CultureInfo.InvariantCulture)) * Config.Instance.LayoutSettings.DefaultNodeDistance * Config.Instance.GraphSettings.GridSize + Config.Instance.LayoutSettings.DefaultRootX * Config.Instance.GraphSettings.GridSize,
                                    Convert.ToInt32(Convert.ToDouble(words[3], CultureInfo.InvariantCulture)) * Config.Instance.LayoutSettings.DefaultLayerDistance * Config.Instance.GraphSettings.GridSize + Config.Instance.LayoutSettings.DefaultRootY * Config.Instance.GraphSettings.GridSize);
                    }
                }
                addEdgeNodes();
            }
        }

        public void UpdateParameterLabels()
        {
            foreach (Material m in Materials)
            {
                m.UpdateParametersLabel();
            }

            foreach (OperatingUnit op in OperatingUnits)
            {
                op.UpdateParametersLabel();
            }

            foreach (Edge e in Edges)
            {
                e.UpdateParametersLabel();
            }
        }
    }
}
