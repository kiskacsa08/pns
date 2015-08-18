﻿/* Copyright 2015 Department of Computer Science and Systems Technology, University of Pannonia

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
using System.Drawing.Drawing2D;
using System.ComponentModel;

using PNSDraw.Configuration;

namespace PNSDraw
{
    [TypeConverter(typeof(ObjectPropertySorter))]
    public class Edge : Canvas.IGraphicsObject
    {

        #region Core private data variables

        int ID;
        Point coords;
        Point offset;

        bool Selected = false;

        bool edited = false;

        public Canvas.IConnectableObject begin;
        public Canvas.IConnectableObject end;

        Canvas.IGraphicsStructure Container;

        int arrowsize = 20;
        double arrowsizeunit = 32.5F;

        EdgeDisplayTextStyle Label;
        EdgeStyle NodeStyle;

        List<EdgeNode> Nodes;

        bool Highlighted;

        double rate = -1;
        public string title;

        string mu;

        #endregion


        #region Private data properties
        [Browsable(false)]
        Color Color
        {
            get { return EdgeStyle.ColorProp; }
            set { EdgeStyle.ColorProp = value; }
        }

        [Browsable(false)]
        Point Coords
        {
            get { return coords; }
            set { coords = value; }
        }

        [Browsable(false)]
        public double Rate
        {
            get 
            {
                if (rate == -1)
                {
                    return Config.Instance.Edge.FlowRate;
                }
                return rate; 
            }
            set
            {
                rate = value;
                Label.Text = Title;
            }
        }

        [Browsable(false)]
        public string MU
        {
            get { return mu; }
            set
            {
                mu = value;
            }
        }

        [Browsable(false)]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                Label.Text = title;
            }
        }

        bool Edited
        {
            get
            {

                return edited;
            }
            set
            {
                edited = value;
                if (edited == false)
                {
                    RemoveNodesFromGraph();
                    RemoveTemporaryNodes();
                    foreach (EdgeNode en in Nodes)
                    {
                        en.Selected = false;
                    }
                }
                else
                {
                    CreateNewNodes();
                    AddNodesToGraph();
                }
            }
        }

        public void Pin(int fp)
        {

        }
        public int getPin()
        {
            return 0;
        }

        #endregion

        public Edge(Canvas.IGraphicsStructure container)
        {
            Label = new EdgeDisplayTextStyle(container, this);
            Label.Text = "";
            Label.SetCoords(new Point(5, 0));

            NodeStyle = new EdgeStyle(container, this);
            NodeStyle.Text = "Edge style";
            NodeStyle.SetCoords(new Point(0, 0));

            ID = 0;
            Coords = new Point(0, 0);
            offset = new Point(0, 0);
            Color = Color.Black;
            Container = container;
            Nodes = new List<EdgeNode>();
            Highlighted = false;

            
        }



        #region GraphicsObject interface functions

        public int GetID()
        {
            return ID;
        }

        public void Draw(Graphics g, bool plain)
        {
            if (end == null)
            {
                return;
            }
            DrawPath(g, Color, false, Color, true);
        }

        public bool HitTest(Point mousecoords)
        {
            if (begin == null || end == null)
            {
                return false;
            }

            bool hitted = false;

            List<PointF> points = GetLineBreakPoints();

            PointF prevcoords = points[0];
            foreach (PointF pf in points)
            {
                if (IsOnLine(mousecoords, prevcoords, pf))
                {
                    hitted = true;
                }
                prevcoords = pf;
            }

            return hitted;
        }

        public Point GetCoords()
        {
            if (end == null)
            {
                return Coords;
            }
            else
            {
                PointF brcoord = GetArrowPosition(GetLineBreakPoints());
                Point ret = new Point();
                ret.X = (int)brcoord.X;
                ret.Y = (int)brcoord.Y;
                return ret;
            }
        }

        public void SetCoords(Point newcoords)
        {
            Coords = newcoords;
        }

        public void SetID(int newid)
        {
            ID = newid;
        }

        public void SetSelected(bool selected)
        {
            Selected = selected;
            if (selected == false)
            {
                Edited = false;
            }
        }

        public void SetOffset(Point newoffset)
        {
            if (Edited == false)
            {
                foreach (EdgeNode en in Nodes)
                {
                    en.SetOffset(newoffset);
                }
                offset = newoffset;
            }
        }

        public void IntegrateOffset()
        {
            offset.X = 0;
            offset.Y = 0;
            if (Edited == false)
            {
                foreach (EdgeNode en in Nodes)
                {
                    en.IntegrateOffset();
                }
            }
        }

        public bool IntersectsWith(Rectangle rect)
        {
            if (begin == null || end == null)
            {
                return false;
            }
            else
            {
                return (rect.Contains(begin.GetConnectorBeginCoords()) && rect.Contains(end.GetConnectorEndCoords()));
            }
        }

        public bool IsSelected()
        {
            Canvas.IGraphicsObject gbegin = begin as Canvas.IGraphicsObject;
            Canvas.IGraphicsObject gend = end as Canvas.IGraphicsObject;
            if (gbegin == null || gend == null)
            {
                return false;
            }
            return ((gbegin.IsSelected() && gend.IsSelected()) || Selected);
        }

        public int GetLayer()
        {
            return 1;
        }

        public Point GetCurrentCoords()
        {
            if (end == null)
            {
                Point cur = (Point)((Size)Coords + (Size)offset);
                return cur;
            }
            else
            {
                PointF brcoord = GetArrowPosition(GetCurrentLineBreakPoints());
                Point ret = new Point();
                ret.X = (int)brcoord.X;
                ret.Y = (int)brcoord.Y;
                return ret;
            }
        }

        public void DrawGhost(Graphics g)
        {
            Point begincoords, endcoords, beginoffset, endoffset;

            begincoords = begin.GetCurrentConnectorBeginCoords();
            beginoffset = begincoords - (Size)begin.GetConnectorBeginCoords();
            if (end == null)
            {
                endcoords = Coords;
                endoffset = Coords;
            }
            else
            {
                endcoords = end.GetCurrentConnectorEndCoords();
                endoffset = endcoords - (Size)end.GetConnectorEndCoords();
            }

            Point tmp = new Point();
            tmp.X += Math.Abs(beginoffset.X);
            tmp.Y += Math.Abs(beginoffset.Y);
            tmp.X += Math.Abs(endoffset.X);
            tmp.Y += Math.Abs(endoffset.Y);
            foreach (EdgeNode en in Nodes)
            {
                tmp.X += Math.Abs(en.GetOffset().X);
                tmp.Y += Math.Abs(en.GetOffset().Y);
            }

            if (tmp.X != 0 || tmp.Y != 0)
            {
                List<PointF> linebreakpoints = GetCurrentLineBreakPoints();
                DrawPath(g, Color.Silver, true, Color.Silver, true);
            }

        }

        public void SetEditSelected(bool selected)
        {
            if (Edited == false && selected == true) Edited = true;
            else if (Edited == true && selected == false) Edited = false;
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
            return Edited;
        }

        public Rectangle GetBoundary()
        {
            if (end != null)
            {
                List<Point> edgenodes = new List<Point>();
                edgenodes.Add(begin.GetConnectorBeginCoords());
                edgenodes.Add(end.GetConnectorEndCoords());

                foreach (EdgeNode en in Nodes)
                {
                    if (en.Needed)
                    {
                        edgenodes.Add(en.GetCoords());
                    }
                }
                return GeometryTools.GetPointsBoundary(edgenodes);
            }
            else
            {
                return new Rectangle(begin.GetCurrentConnectorBeginCoords(), new Size(0, 0));
            }
        }

        public void SetSelectedChild(Canvas.IGraphicsObject child)
        {
            if (child == Label)
            {
                // Edited = false;
            }
            Selected = false;
        }

        public bool IsMoveable()
        {
            return false;
        }

        public bool IsDeletable()
        {
            return true;
        }

        public Point GetOffset()
        {
            PointF acurpos = GetArrowPosition(GetCurrentLineBreakPoints());
            PointF apos = GetArrowPosition(GetLineBreakPoints());
            Point ret = new Point();
            ret.X = (int)(acurpos.X - apos.X);
            ret.Y = (int)(acurpos.Y - apos.Y);
            return ret;
        }

        #endregion

        #region Other functions

        public bool IsNeededNode(EdgeNode node)
        {
            bool found = false;
            Point prevcoords = begin.GetCurrentConnectorBeginCoords();
            Point nextcoords = end.GetCurrentConnectorEndCoords();
            EdgeNode prevnode = null;
            EdgeNode nextnode = null;

            foreach (EdgeNode en in Nodes)
            {
                if (en != node)
                {
                    if (en.Temporary == false)
                    {
                        if (found == false)
                        {
                            prevnode = en;
                        }
                        else if (nextnode == null)
                        {
                            nextnode = en;
                        }
                    }
                }
                else
                {
                    found = true;
                }
            }
            if (prevnode != null)
            {
                prevcoords = prevnode.GetCurrentCoords();
            }
            if (nextnode != null)
            {
                nextcoords = nextnode.GetCurrentCoords();
            }
            double distance = PointDistanceToLine(node.GetCurrentCoords(), prevcoords, nextcoords);
            if (distance < 20)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void CreateNewNodes()
        {
            List<EdgeNode> newlist = new List<EdgeNode>();

            Point prevcoords = begin.GetConnectorBeginCoords();
            Point nextcoords = prevcoords;
            foreach (EdgeNode en in Nodes)
            {
                nextcoords = en.GetCoords();
                EdgeNode newnode = new EdgeNode(this.Container, this);
                newnode.SetCoords(new Point((prevcoords.X + nextcoords.X) / 2, (prevcoords.Y + nextcoords.Y) / 2));

                newlist.Add(newnode);
                prevcoords = nextcoords;
                newlist.Add(en);
            }
            nextcoords = end.GetConnectorEndCoords();

            EdgeNode lastnode = new EdgeNode(this.Container, this);
            lastnode.SetCoords(new Point((prevcoords.X + nextcoords.X) / 2, (prevcoords.Y + nextcoords.Y) / 2));

            newlist.Add(lastnode);

            Nodes = newlist;
        }

        public void UpdateNodes()
        {
            if (Edited == true)
            {
                RemoveNodesFromGraph();
                RemoveTemporaryNodes();
                CreateNewNodes();
                AddNodesToGraph();
            }
        }

        public void AddNodesToGraph()
        {
            foreach (EdgeNode en in Nodes)
            {
                Container.AddSingleGraphicsObject(en);
            }

        }

        public void RemoveNodesFromGraph()
        {
            foreach (EdgeNode en in Nodes)
            {
                Container.RemoveSingleGraphicsObject(en);
            }
        }

        public void UpdateNodesNeededState()
        {
            foreach (EdgeNode en in Nodes)
            {
                if (en.Temporary == false || en.IsSelected() == true)
                {
                    en.Needed = IsNeededNode(en);
                }
            }
        }

        public void RemoveNode(EdgeNode node)
        {
            Nodes.Remove(node);
            Container.RemoveSingleGraphicsObject(node);
            UpdateNodes();
        }

        public void RemoveTemporaryNodes()
        {
            List<EdgeNode> toremove = new List<EdgeNode>();
            foreach (EdgeNode en in Nodes)
            {
                if (en.Needed == false)
                {
                    toremove.Add(en);
                }
            }
            foreach (EdgeNode en in toremove)
            {
                Nodes.Remove(en);
            }
        }

        public void SetNodes(List<EdgeNode> nodes)
        {
            RemoveNodesFromGraph();
            Nodes.Clear();
            foreach (EdgeNode en in nodes)
            {
                Nodes.Add(en);
            }
            Selected = false;

        }

        public List<EdgeNode> GetNodes()
        {
            List<EdgeNode> nodes = new List<EdgeNode>();
            foreach (EdgeNode en in Nodes)
            {
                if (en.Needed == true && en.Temporary == false)
                {
                    nodes.Add(en);
                }
            }
            return nodes;
        }

        public double PointDistanceToLine(PointF p, PointF sp0, PointF sp1)
        {
            PointF v = new PointF();
            v.X = sp1.X - sp0.X;
            v.Y = sp1.Y - sp0.Y;
            PointF w = new PointF();
            w.X = p.X - sp0.X;
            w.Y = p.Y - sp0.Y;

            double c1 = v.X * w.X + v.Y * w.Y;
            double distance;
            if (c1 <= 0)
            {
                distance = (p.X - sp0.X) * (p.X - sp0.X) + (p.Y - sp0.Y) * (p.Y - sp0.Y);
                return Math.Sqrt(distance);
            }
            double c2 = v.X * v.X + v.Y * v.Y;
            if (c2 <= c1)
            {
                distance = (p.X - sp1.X) * (p.X - sp1.X) + (p.Y - sp1.Y) * (p.Y - sp1.Y);
                return Math.Sqrt(distance);
            }
            Point pb = new Point((int)Math.Round(sp0.X + (sp1.X - sp0.X) * c1 / c2), (int)Math.Round(sp0.Y + (sp1.Y - sp0.Y) * c1 / c2));
            distance = (p.X - pb.X) * (p.X - pb.X) + (p.Y - pb.Y) * (p.Y - pb.Y);
            return Math.Sqrt(distance);
        }

        public double LineLength(PointF p1, PointF p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        public bool IsOnLine(PointF p, PointF sp0, PointF sp1)
        {
            double distance = PointDistanceToLine(p, sp0, sp1);
            if (distance < 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateEditedState()
        {
            bool editedstate = false;
            if (Label.IsSelected())
            {
                editedstate = true;
            }
            foreach (EdgeNode en in Nodes)
            {
                if (en.IsSelected())
                {
                    editedstate = true;
                }
            }
            if (Selected == false && editedstate == false)
            {
                Edited = false;
            }
        }

        public Canvas.IGraphicsStructure GetContainer()
        {
            return Container;
        }

        public void OnAddToGraph()
        {
            Container.AddSingleGraphicsObject(this);
            Container.AddSingleGraphicsObject(this.Label);
            if (end != null)
            {
                end.AddConnection(begin);
            }
        }

        public void OnRemoveFromGraph()
        {
            Container.RemoveSingleGraphicsObject(this.Label);
            Container.RemoveSingleGraphicsObject(this);
            foreach (EdgeNode en in Nodes)
            {
                Container.RemoveSingleGraphicsObject(en);
            }
            end.RemoveConnection(begin);
        }

        private List<PointF> ComputeArrowPositionAndAngle(List<PointF> pointlist)
        {
            List<PointF> ret = new List<PointF>();
            // nyilhegy poziciojanak meghatarozasa
            // osszeszamoljuk a teljes hosszat
            double length = 0;
            PointF last = pointlist[0];
            foreach (PointF p in pointlist)
            {
                length += LineLength(p, last);
                last = p;
            }
            // megvan a teljes hossz
            // a nyilhegy pozicioja nem lehet az elejen es a vegen (0% es 100%)
            last = pointlist[0];
            double tmplength = 0;
            double arrowposition = EdgeStyle.PositionProp / 100;
            if (arrowposition < 0.0001F)
            {
                arrowposition = 0.0001F;
            }
            if (arrowposition > 0.9999F)
            {
                arrowposition = 0.9999F;
            }
            PointF arrowstart = new PointF();
            // mwegkeressuk melyik szakaszra esik a nyilhegy
            foreach (PointF p in pointlist)
            {
                tmplength += LineLength(p, last);
                double dist = tmplength - length * arrowposition;
                if ((Math.Abs(dist) < arrowsize) && arrowstart.IsEmpty == true)
                {
                    arrowstart = p;
                }
                if (dist > arrowsize || Math.Abs(tmplength - length) < 1)
                {
                    if (arrowstart.IsEmpty == true || Math.Abs(tmplength - length) < 1)
                    {
                        arrowstart = last;
                    }
                    double linelength = LineLength(p, arrowstart);
                    double percent = (tmplength - length * arrowposition) / linelength;
                    if (EdgeStyle.OnCenterProp)
                    {
                        percent = 0.5F;
                    }
                    if (percent > 0.99)
                    {
                        percent = 0.99;
                    }

                    PointF arrowend = new PointF();
                    arrowend = GetLinePoint(arrowstart, p, 1 - percent);


                    ret.Add(arrowstart);
                    ret.Add(arrowend);
                    return ret;
                }
                last = p;
            }
            // biztonsagi okokbol mindig legyen 2 eleme
            ret.Add(begin.GetConnectorBeginCoords());
            ret.Add(end.GetConnectorEndCoords());
            return ret;
        }

        private void DrawPath(Graphics g, Color c, bool current, Color line, bool drawstroke)
        {
            Pen pen = new Pen(new SolidBrush(c), Config.Instance.LineSize * 2);

            PointF bcoords = begin.GetConnectorBeginCoords();
            PointF ecoords = end.GetConnectorEndCoords();

            if (current)
            {
                bcoords = begin.GetCurrentConnectorBeginCoords();
                ecoords = end.GetCurrentConnectorEndCoords();
            }

            GraphicsPath path = new GraphicsPath();

            List<EdgeNode> curnodes = new List<EdgeNode>();
            foreach (EdgeNode en in Nodes)
            {
                if (current == true && en.Needed == true)
                {
                    curnodes.Add(en);
                }
                else if (current == false && en.Temporary == false)
                {
                    curnodes.Add(en);
                }
            }
            int i = 0;
            PointF prev = bcoords;
            PointF next = ecoords;
            PointF lastcoord = prev;
            for (i = 0; i < curnodes.Count; i++)
            {
                if (i + 1 >= curnodes.Count)
                {
                    next = ecoords;
                }
                else
                {
                    next = curnodes[i + 1].GetCoords();
                    if (current)
                    {
                        next = curnodes[i + 1].GetCurrentCoords();
                    }
                }
                if (curnodes[i].Smooth == false)
                {
                    PointF encoords = curnodes[i].GetCoords();
                    if (current)
                    {
                        encoords = curnodes[i].GetCurrentCoords();
                    }
                    path.AddLine(lastcoord, encoords);
                    lastcoord = encoords;
                }
                else
                {
                    List<PointF> bezier = ComputeBezierCurveControlPoints(prev, curnodes[i], next, current);
                    path.AddLine(lastcoord, bezier[0]);
                    path.AddBezier(bezier[0], bezier[1], bezier[2], bezier[3]);
                    lastcoord = bezier[3];
                }
                prev = curnodes[i].GetCoords();
                if (current)
                {
                    prev = curnodes[i].GetCurrentCoords();
                }
            }
            path.AddLine(lastcoord, ecoords);
            g.DrawPath(pen, path);

            DrawArrow(g, c, current);
        }

        private PointF GetArrowPosition(List<PointF> pointlist)
        {
            List<PointF> arrow = ComputeArrowPositionAndAngle(pointlist);
            return arrow[1];
        }

        private void DrawArrow(Graphics g, Color c, bool current)
        {
            List<PointF> arrowpoints = ComputeArrowTriangleCoords(current);
            g.FillPolygon(new SolidBrush(c), arrowpoints.ToArray());
        }

        /*
         * A fuggveny a bezier gorbet szakaszokra bontja es a torespontokbol allo lisat adja vissza
         * 
         * */
        List<PointF> CreateBezierLinePoints(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            List<PointF> ret = new List<PointF>();
            int i = 0;
            int steps = (int)Math.Abs(p3.X - p0.X);
            if (steps < (int)Math.Abs(p3.Y - p0.Y))
            {
                steps = (int)Math.Abs(p3.Y - p0.Y);
            }
            if (steps < 2)
            {
                ret.Add(p0);
                ret.Add(p3);
                return ret;
            }
            if (steps > 1000) steps = 1000;
            for (i = 0; i < steps; i++)
            {
                float t = (float)i / (steps - 1);
                float x = (1 - t) * (1 - t) * (1 - t) * p0.X +
                           3 * t * (1 - t) * (1 - t) * p1.X +
                           3 * t * t * (1 - t) * p2.X +
                           t * t * t * p3.X;
                float y = (1 - t) * (1 - t) * (1 - t) * p0.Y +
                           3 * t * (1 - t) * (1 - t) * p1.Y +
                           3 * t * t * (1 - t) * p2.Y +
                           t * t * t * p3.Y;
                ret.Add(new PointF((int)x, (int)y));
            }
            return ret;
        }

        PointF GetLinePoint(PointF begincoords, PointF endcoords, double percent)
        {
            PointF br = new PointF();
            br.X = (float)(begincoords.X + percent * (endcoords.X - begincoords.X));
            br.Y = (float)(begincoords.Y + percent * (endcoords.Y - begincoords.Y));

            return br;
        }

        /*
         * A fuggveny kiszamolja a bezier gorbet meghatarozo negy pontot a simitott
         * nyil torespont es az elozo es kovetkezo torespont koordinataja alapjan.
         * */
        List<PointF> ComputeBezierCurveControlPoints(PointF prev, EdgeNode center, PointF next, bool current)
        {
            List<PointF> ret = new List<PointF>();

            int before = center.SmoothSizeBefore;
            int after = center.SmoothSizeAfter;

            PointF centercoords = center.GetCoords();
            if (current == true)
            {
                centercoords = center.GetCurrentCoords();
            }

            double perb = before / LineLength(prev, centercoords);
            if (perb > 0.5) perb = 0.5;
            double pera = after / LineLength(centercoords, next);
            if (pera > 0.5) pera = 0.5;

            double sbefore = center.SmoothStrengthBefore;
            double safter = center.SmoothStrengthAfter;


            ret.Add(GetLinePoint(centercoords, prev, perb));
            ret.Add(GetLinePoint(centercoords, prev, perb * sbefore));
            ret.Add(GetLinePoint(centercoords, next, pera * safter));
            ret.Add(GetLinePoint(centercoords, next, pera));

            return ret;
        }

        List<PointF> GetLineBreakPoints()
        {
            List<PointF> ret = new List<PointF>();
            bool smooth = false;
            PointF last = new PointF();
            PointF temp = new PointF();
            int before = 0;
            int after = 0;
            double sbefore = 0.5;
            double safter = 0.5;
            ret.Add((PointF)begin.GetConnectorBeginCoords());
            last = begin.GetConnectorBeginCoords();
            foreach (EdgeNode en in Nodes)
            {
                if (en.Temporary == false)
                {
                    if (smooth)
                    {
                        double perb = before / LineLength(last, temp);
                        if (perb > 0.5) perb = 0.5;
                        double pera = after / LineLength(last, en.GetCoords());
                        if (pera > 0.5) pera = 0.5;

                        List<PointF> bezier = CreateBezierLinePoints(
                            GetLinePoint(last, temp, perb),
                            GetLinePoint(last, temp, perb * sbefore),
                            GetLinePoint(last, en.GetCoords(), pera * safter),
                            GetLinePoint(last, en.GetCoords(), pera));
                        ret.AddRange(bezier);
                        smooth = false;
                    }
                    if (en.Smooth == true)
                    {
                        smooth = true;
                        temp = last;
                        before = en.SmoothSizeBefore;
                        after = en.SmoothSizeAfter;
                        sbefore = en.SmoothStrengthBefore;
                        safter = en.SmoothStrengthAfter;
                    }
                    else
                    {
                        ret.Add((PointF)en.GetCoords());
                    }
                    last = en.GetCoords();
                }
            }
            if (smooth)
            {
                double perb = before / LineLength(last, temp);
                if (perb > 0.5) perb = 0.5;
                double pera = after / LineLength(last, end.GetConnectorEndCoords());
                if (pera > 0.5) pera = 0.5;

                List<PointF> bezier = CreateBezierLinePoints(
                    GetLinePoint(last, temp, perb),
                    GetLinePoint(last, temp, perb * sbefore),
                    GetLinePoint(last, end.GetConnectorEndCoords(), pera * safter),
                    GetLinePoint(last, end.GetConnectorEndCoords(), pera));
                ret.AddRange(bezier);
                smooth = false;
            }

            ret.Add((PointF)end.GetConnectorEndCoords());

            return ret;
        }

        List<PointF> GetCurrentLineBreakPoints()
        {
            List<PointF> ret = new List<PointF>();
            bool smooth = false;
            PointF last = new PointF();
            PointF temp = new PointF();
            int before = 0;
            int after = 0;
            double sbefore = 0.5;
            double safter = 0.5;
            ret.Add((PointF)begin.GetCurrentConnectorBeginCoords());
            last = begin.GetCurrentConnectorBeginCoords();
            foreach (EdgeNode en in Nodes)
            {
                if (en.Needed == true)
                {
                    if (smooth)
                    {
                        double perb = before / LineLength(last, temp);
                        if (perb > 0.5) perb = 0.5;
                        double pera = after / LineLength(last, en.GetCurrentCoords());
                        if (pera > 0.5) pera = 0.5;

                        List<PointF> bezier = CreateBezierLinePoints(
                            GetLinePoint(last, temp, perb),
                            GetLinePoint(last, temp, perb * sbefore),
                            GetLinePoint(last, en.GetCurrentCoords(), pera * safter),
                            GetLinePoint(last, en.GetCurrentCoords(), pera));
                        ret.AddRange(bezier);
                        smooth = false;
                    }
                    if (en.Smooth == true)
                    {
                        smooth = true;
                        temp = last;
                        before = en.SmoothSizeBefore;
                        after = en.SmoothSizeAfter;
                        sbefore = en.SmoothStrengthBefore;
                        safter = en.SmoothStrengthAfter;
                    }
                    else
                    {
                        ret.Add((PointF)en.GetCurrentCoords());
                    }
                    last = en.GetCurrentCoords();
                }
            }
            if (smooth)
            {
                double perb = before / LineLength(last, temp);
                if (perb > 0.5) perb = 0.5;
                double pera = after / LineLength(last, end.GetCurrentConnectorEndCoords());
                if (pera > 0.5) pera = 0.5;

                List<PointF> bezier = CreateBezierLinePoints(
                    GetLinePoint(last, temp, perb),
                    GetLinePoint(last, temp, perb * sbefore),
                    GetLinePoint(last, end.GetCurrentConnectorEndCoords(), pera * safter),
                    GetLinePoint(last, end.GetCurrentConnectorEndCoords(), pera));
                ret.AddRange(bezier);
                smooth = false;
            }
            ret.Add((PointF)end.GetCurrentConnectorEndCoords());
            return ret;
        }

        private List<PointF> ComputeArrowTriangleCoords(bool current)
        {
            List<PointF> ret = new List<PointF>();

            List<PointF> arrowcoords = new List<PointF>();

            if (current)
            {
                arrowcoords = ComputeArrowPositionAndAngle(GetCurrentLineBreakPoints());
            }
            else
            {
                arrowcoords = ComputeArrowPositionAndAngle(GetLineBreakPoints());
            }

            double templength = LineLength(arrowcoords[0], arrowcoords[1]);
            //  meroleges vektor
            PointF mv = new PointF();
            mv.X = (float)((arrowcoords[1].Y - arrowcoords[0].Y) / templength);
            mv.Y = -1.0F * (float)((arrowcoords[1].X - arrowcoords[0].X) / templength);
            PointF a1 = GetLinePoint(arrowcoords[0], arrowcoords[1], (arrowsizeunit + templength) / templength);
            PointF ta = GetLinePoint(arrowcoords[0], arrowcoords[1], (templength - arrowsizeunit) / templength);
            PointF a2 = new PointF();
            a2.X = (float)(ta.X + mv.X * 2 * arrowsizeunit);
            a2.Y = (float)(ta.Y + mv.Y * 2 * arrowsizeunit);
            PointF a3 = new PointF();
            a3.X = (float)(ta.X + mv.X * -2 * arrowsizeunit);
            a3.Y = (float)(ta.Y + mv.Y * -2 * arrowsizeunit);

            ret.Add(a1);
            ret.Add(a2);
            ret.Add(a3);
            return ret;
        }

        public string CovertToSVG(int zorder)
        {
            string svg = "<g>\n";

            string pdata = "";
            PointF bcoords = begin.GetConnectorBeginCoords();
            PointF ecoords = end.GetConnectorEndCoords();
            pdata += "M" + ConvertManager.ToString(bcoords.X / 5.0F) + " " + ConvertManager.ToString(bcoords.Y / 5.0F);
            List<EdgeNode> curnodes = new List<EdgeNode>();
            foreach (EdgeNode en in Nodes)
            {
                if (en.Temporary == false)
                {
                    curnodes.Add(en);
                }
            }
            int i = 0;
            PointF prev = bcoords;
            PointF next = ecoords;
            for (i = 0; i < curnodes.Count; i++)
            {
                if (i + 1 >= curnodes.Count)
                {
                    next = ecoords;
                }
                else
                {
                    next = curnodes[i + 1].GetCoords();
                }
                if (curnodes[i].Smooth == false)
                {
                    PointF encoords = curnodes[i].GetCoords();
                    pdata += " L" + ConvertManager.ToString(encoords.X / 5) + " " + ConvertManager.ToString(encoords.Y / 5);
                }
                else
                {
                    List<PointF> bezier = ComputeBezierCurveControlPoints(prev, curnodes[i], next, false);
                    pdata += " L" + ConvertManager.ToString(bezier[0].X / 5) + " " + ConvertManager.ToString(bezier[0].Y / 5);
                    pdata += " C" + ConvertManager.ToString(bezier[1].X / 5) + " " + ConvertManager.ToString(bezier[1].Y / 5)
                        + " " + ConvertManager.ToString(bezier[2].X / 5) + " " + ConvertManager.ToString(bezier[2].Y / 5)
                        + " " + ConvertManager.ToString(bezier[3].X / 5) + " " + ConvertManager.ToString(bezier[3].Y / 5);
                }
                prev = curnodes[i].GetCoords();
            }
            pdata += " L" + ConvertManager.ToString(ecoords.X / 5.0F) + " " + ConvertManager.ToString(ecoords.Y / 5.0F);
            svg += String.Format("<path d=\"{0}\" fill=\"none\" stroke=\"{1}\" stroke-width=\"4\" />\n", pdata, ConvertManager.ToHtml(Color));

            List<PointF> tc = ComputeArrowTriangleCoords(false);

            svg += String.Format("<polygon points=\"{0},{1} {2},{3} {4},{5}\" style=\"fill:{6}; stroke-width:0;\" />\n",
                ConvertManager.ToString(tc[0].X / 5), ConvertManager.ToString(tc[0].Y / 5),
                ConvertManager.ToString(tc[1].X / 5), ConvertManager.ToString(tc[1].Y / 5),
                ConvertManager.ToString(tc[2].X / 5), ConvertManager.ToString(tc[2].Y / 5),
                ConvertManager.ToHtml(Color));

            svg += "</g>\n";

            svg += Label.ConvertToSVG(zorder);

            return svg;
        }

        public void UpdateParametersLabel()
        {
            if (LabelProp.LongFormatProp)
            {
                string itemName = "";
                if (begin is Material)
                {
                    Material material = (Material)begin;
                    MU = material.ParameterList["reqflow"].MU;

                    OperatingUnit opunit = (OperatingUnit)end;
                    itemName = "X_" + opunit.Name;
                }
                else if (end is Material)
                {
                    Material material = (Material)end;
                    MU = material.ParameterList["reqflow"].MU;

                    OperatingUnit opunit = (OperatingUnit)begin;
                    itemName = "X_" + opunit.Name;
                }

                Title = Rate + " " + " × " + itemName + " " + MU;
            }
            else
            {
                Title = Rate.ToString();
            }
        }

        #endregion

        #region PropertyGrid properties

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(10)]
        [DisplayName("Type")]
        [ReadOnly(true)]
        public string TypeProp
        {
            get
            {
                return "Edge";
            }
        }

        [Browsable(true)]
        [Category("\t\tParameters"), PropertyOrder(20)]
        [DisplayName("Rate")]
        public double RateProp
        {
            get
            {
                return Rate;
            }
            set
            {
                Rate = value;
                UpdateParametersLabel();
            }
        }

        [Browsable(true)]
        [Category("\t\tStyle"), PropertyOrder(10)]
        [DisplayName("Edge")]
        public EdgeStyle EdgeStyle
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
        [Category("\t\tStyle"), PropertyOrder(20)]
        [DisplayName("Label")]
        public EdgeDisplayTextStyle LabelProp
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
        #endregion
    }

}