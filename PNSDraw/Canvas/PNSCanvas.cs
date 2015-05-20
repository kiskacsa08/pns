using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PNSDraw.Canvas
{
    public partial class PNSCanvas : UserControl
    {

        public List<IGraphicsObject> Objects=new List<IGraphicsObject>();
        IGraphicsObject DraggedObject;
        public IGraphicsStructure GraphicsStructure = null;
        IGraphicsObject CurrentlyEdited = null;
        
        int gridsize = 300;

        int rollstep = 50;

        bool connectormode = false;

        bool addobjectmode = false;

        public bool ConnectorMode
        {
            get
            {
                return connectormode;
            }
            set
            {
                connectormode = value;
                CurrentConnector.Reset();
            }
        }

        public Cursor AddCursor = Cursors.Arrow;

        public bool AddObjectMode
        {
            get
            {
                return addobjectmode;
            }
            set
            {
                addobjectmode = value;
            }
        }

        ObjectConnector CurrentConnector;

        public int GridSize
        {
            get
            {
                return (gridsize > 0) ? gridsize : 1;
            }
            set
            {
                if (value > 0) gridsize = value;
            }
        }

        bool showgrid = true;

        public bool ShowGrid
        {
            get
            {
                return showgrid;
            }
            set
            {
                if (value != showgrid)
                {
                    showgrid = value;
                    Refresh();
                }
            }
        }

        bool snaptogrid = true;

        public bool SnapToGrid
        {
            get
            {
                return snaptogrid;
            }
            set
            {
                if (value != snaptogrid)
                {
                    snaptogrid = value;
                }
            }
        }

        class DragState
        {
            public bool Dragged = false;
            public Point OriginalCoords = new Point();
            public int ObjectID = 0;
            public Point LastCoords = new Point();
            public IGraphicsObject SelectedObject = null;
            public List<IGraphicsObject> PreviousSelection = new List<IGraphicsObject>();
           
            public void Reset()
            {
                Dragged = false;
                ObjectID = 0;
                SelectedObject = null;
                PreviousSelection.Clear();
            }
        }

        public class ViewState
        {
            public double Zoom = 0.2;
            public Point Offset = new Point();

            public void Reset()
            {
                Zoom = 0.2;
                Offset = new Point();
            }
        }

        private bool locked = false;

        public bool Locked
        {
            get
            {
                return locked;
            }
            set
            {
                locked = value;
            }
        }

        DragState DragInfo;
        ViewState ViewInfo;

        public event CanvasEventHandler NewItem;
        public event CanvasEventHandler ClickItem;
        public event CanvasEventHandler EditItem;
        public event CanvasEventHandler NewConnector;
        public event CanvasEventHandler RemoveObjects;
        public event CanvasEventHandler DataChanged;
        public event CanvasEventHandler SelectionChanged;
        public event CanvasEventHandler ViewChanged;
        public event CanvasEventHandler Copy;
        public event CanvasEventHandler Paste;
        public event CanvasEventHandler Duplicate;

        public PNSCanvas()
        {
            InitializeComponent();
            DragInfo = new DragState();
            ViewInfo = new ViewState();
            this.MouseWheel += new MouseEventHandler(PNSCanvas_MouseWheel);
            DraggedObject = null;
            CurrentConnector = new ObjectConnector();
        }

        IGraphicsObject GetNewItem()
        {
            CanvasEventArgs e = new CanvasEventArgs();
            if (NewItem != null)
            {
                NewItem(this, e);
            }
            return (e.Data == null)?(null):(e.Data as IGraphicsObject);
        }

        bool OnClickedItem(IGraphicsObject obj)
        {
            CanvasEventArgs e = new CanvasEventArgs();
            e.Data = obj;
            if (ClickItem != null)
            {
                ClickItem(this, e);
            }
            return e.Handled;
        }

        bool OnEditItem(IGraphicsObject obj)
        {
            CanvasEventArgs e = new CanvasEventArgs();
            e.Data = obj;
            if (EditItem != null)
            {
                EditItem(this, e);
            }
            return e.Handled;
        }

        bool OnSelectionChanged()
        {
            CanvasEventArgs e = new CanvasEventArgs();
            e.Data = GetSelectedObjects();
            if (SelectionChanged != null)
            {
                SelectionChanged(this, e);
            }
            return e.Handled;
        }

        bool OnViewChanged()
        {
            CanvasEventArgs e = new CanvasEventArgs();
            if (ViewChanged != null)
            {
                ViewChanged(this, e);
            }
            return e.Handled;
        }

        bool OnCopy()
        {
            CanvasEventArgs e = new CanvasEventArgs();
            if (Copy != null)
            {
                Copy(this, e);
            }
            return e.Handled;
        }

        bool OnPaste()
        {
            CanvasEventArgs e = new CanvasEventArgs();
            if (Paste != null)
            {
                Paste(this, e);
            }
            return e.Handled;
        }

        bool OnDuplicate()
        {
            CanvasEventArgs e = new CanvasEventArgs();
            if (Duplicate != null)
            {
                Duplicate(this, e);
            }
            return e.Handled;
        }

        List<IGraphicsObject> GetSelectedObjects()
        {
            List<IGraphicsObject> selectionlist = new List<IGraphicsObject>();
            
            foreach (IGraphicsObject obj in Objects)
            {
                if (obj.IsSelected())
                {
                    selectionlist.Add(obj);
                }
            }
            return selectionlist;
        }

        void OnNewConnector()
        {
            CanvasEventArgs e = new CanvasEventArgs();
            e.Data = CurrentConnector;
            if (NewConnector != null)
            {
                NewConnector(this, e);
            }
        }

        void OnRemoveObjects(List<IGraphicsObject> toremove)
        {
            CanvasEventArgs e = new CanvasEventArgs();
            e.Data = toremove;
            if (RemoveObjects != null)
            {
                RemoveObjects(this, e);
            }
            Refresh();
        }

        void OnDataChanged()
        {
            CanvasEventArgs e = new CanvasEventArgs();
            if (DataChanged != null)
            {
                DataChanged(this, e);
            }
        }

        public void ZoomIn()
        {
            ZoomIn(new Point(this.Width/2, this.Height/2));
        }

        public void ZoomOut()
        {
            ZoomOut(new Point(this.Width / 2, this.Height / 2));
        }

        void ZoomIn(Point center)
        {
            Zoom(ViewInfo.Zoom * 1.1, center);
        }

        void ZoomOut(Point center)
        {
            Zoom(ViewInfo.Zoom / 1.1, center);
        }

        public Point GetCanvasCenter()
        {
            Rectangle rect = GetCanvasBoundary();
            Point p = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            return p;
        }

        public void ZoomToFit()
        {
            ZoomToFit(false);
        }


        public void ZoomToFit(bool force)
        {
            Rectangle crect = GetCanvasBoundary();
            Rectangle vrect = GetCurrentViewBoundary();

            double zoomw = (double) vrect.Width / (double) crect.Width * ViewInfo.Zoom;
            double zoomh = (double) vrect.Height / (double)crect.Height * ViewInfo.Zoom;

            double newzoom = (zoomw < zoomh) ? (zoomw) : (zoomh);

            if (force)
            {
                ViewInfo.Zoom = newzoom;
            }
            else if (newzoom < ViewInfo.Zoom)
            {
                ViewInfo.Zoom = newzoom;
            }
            ViewInfo.Offset = TranslateCanvasToScreen(new Point(-crect.Left, -crect.Top));

            Refresh();
            OnViewChanged();
        }

        void Zoom(double zoom, Point center)
        {
            Point canvaspos = TranslateScreenToCanvas(center);
            ViewInfo.Zoom = zoom;
            if (ViewInfo.Zoom < 0.04)
            {
                ViewInfo.Zoom = 0.04;
            }
            if (ViewInfo.Zoom > 0.8)
            {
                ViewInfo.Zoom = 0.8;
            }
            Point screenpos = TranslateCanvasToScreen(canvaspos);
            ViewInfo.Offset.X = ViewInfo.Offset.X + center.X - screenpos.X;
            ViewInfo.Offset.Y = ViewInfo.Offset.Y + center.Y - screenpos.Y;
            Refresh();
            OnViewChanged();
        }

        void Roll(Point direction)
        {
            ViewInfo.Offset.X += direction.X * rollstep;
            ViewInfo.Offset.Y += direction.Y * rollstep;
            Refresh();
            OnViewChanged();
        }

        void PNSCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if (DragInfo.Dragged == false)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    if (e.Delta > 0)
                    {
                        ZoomIn(e.Location);
                    }
                    else
                    {
                        ZoomOut(e.Location);
                    }
                }
                else if (Control.ModifierKeys == Keys.Shift)
                {
                    Point dir = new Point();
                    dir.X = Math.Sign(e.Delta);
                    Roll(dir);
                }
                else
                {
                    Point dir = new Point();
                    dir.Y = Math.Sign(e.Delta);
                    Roll(dir);
                }
                
            }
        }

        public void AddObject(IGraphicsObject newobj)
        {
            GraphicsStructure.AddSingleObject(newobj);
            if (SnapToGrid)
            {
                newobj.SetCoords(SnapCanvasPointToGrid(newobj.GetCoords()));
            }
        }

        public void RemoveObject(IGraphicsObject obj)
        {
            GraphicsStructure.RemoveSingleObject(obj);
        }

        private void DrawGrid(Graphics g)
        {
            Pen p = new Pen(Color.LightGray, 1f);
            Point gp_min, gp_max;
            gp_min = TranslateScreenToCanvas(new Point(0, 0));
            gp_max = TranslateScreenToCanvas(new Point(this.Width, this.Height));

            int i = 0;
            Point tmp1, tmp2;
            for (i = gp_min.X / GridSize; i <= gp_max.X / GridSize; i++)
            {
                tmp1 = TranslateCanvasToScreen(new Point(i * GridSize, gp_min.Y - 1)); // biztosan kitöltse a canvast
                tmp2 = TranslateCanvasToScreen(new Point(i * GridSize, gp_max.Y + 1)); // biztosan kitöltse a canvast
                g.DrawLine(p, tmp1, tmp2);
            }

            for (i = gp_min.Y / GridSize; i <= gp_max.Y / GridSize; i++)
            {
                tmp1 = TranslateCanvasToScreen(new Point(gp_min.X - 1, i * GridSize)); // biztosan kitöltse a canvast
                tmp2 = TranslateCanvasToScreen(new Point(gp_max.X - 1, i * GridSize)); // biztosan kitöltse a canvast
                g.DrawLine(p, tmp1, tmp2);
            }

            Point zero = TranslateCanvasToScreen(new Point(0, 0));
            p.Color = Color.Black;
            tmp1 = new Point();
            tmp2 = new Point();
            if (zero.X > 0 && zero.X < this.Width && zero.Y < this.Height)
            {
                tmp1.X = zero.X;
                tmp1.Y = (zero.Y>0)?(zero.Y):(0);
                tmp2.X = zero.X;
                tmp2.Y = this.Height;
                g.DrawLine(p, tmp1, tmp2);
            }
            if (zero.Y > 0 && zero.Y < this.Height && zero.X < this.Width)
            {
                tmp1.X = (zero.X > 0) ? (zero.X) : (0);
                tmp1.Y = zero.Y;
                tmp2.X = this.Width;
                tmp2.Y = zero.Y;
                g.DrawLine(p, tmp1, tmp2);
            }

        }

        public Rectangle GetCanvasBoundary()
        {
            if (Objects.Count == 0)
            {
                Rectangle ret = GetCurrentViewBoundary();
                ret.X = 0;
                ret.Y = 0;
                return ret;
            }
            List<Point> points = new List<Point>();
            foreach(IGraphicsObject obj in Objects)
            {
                Rectangle rect = obj.GetBoundary();
                points.Add(rect.Location);
                points.Add(rect.Location + rect.Size);
            }
            Rectangle boundary = GeometryTools.GetPointsBoundary(points);
            boundary.Inflate(75, 75);
            return boundary;
        }

        public void Export(Graphics g, Size s)
        {
            Rectangle boundary = GetCanvasBoundary();

            double zoom = s.Width / (double)boundary.Width;
            if (zoom > s.Height / (double)boundary.Height)
            {
                zoom = s.Height / (double)boundary.Height;
            }
            


            g.Clear(Color.White);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            PointF offset = new Point();
            if (s.Width > (int)Math.Round(boundary.Width * zoom))
            {
                offset.X = (s.Width - (int)Math.Round(boundary.Width * zoom)) / 2;
            }
            if (s.Height > (int)Math.Round(boundary.Height * zoom))
            {
                offset.Y = (s.Height - (int)Math.Round(boundary.Height * zoom)) / 2;
            }

            g.TranslateTransform( (float) (-boundary.Left*zoom) + offset.X, (float) (-boundary.Top*zoom) + offset.Y);
            g.ScaleTransform((float)zoom, (float)zoom);

            if (Objects != null)
            {
                int layer =0;
                for (layer = 0; layer < 5; layer++)
                {
                    foreach (IGraphicsObject obj in Objects)
                    {
                        if (obj.GetLayer() == layer)
                        {
                            obj.Draw(g, true);
                        }
                    }
                   
                }  
            }

            g.ResetTransform();

        }

        private void DrawBoundary(Graphics g)
        {
            Point zero = TranslateCanvasToScreen(new Point(0, 0));
            if (zero.X>0)
            {
                Rectangle fill = new Rectangle();
                fill.X = 0;
                fill.Y = 0;
                fill.Width = (zero.X < this.Width) ?(zero.X):(this.Width);
                fill.Height = this.Height;

                SolidBrush br = new SolidBrush(Color.FromArgb(230, 230, 230));

                g.FillRectangle(br, fill);
            }
            if (zero.Y > 0)
            {
                Rectangle fill = new Rectangle();
                fill.X = 0;
                fill.Y = 0;
                fill.Width =this.Width;
                fill.Height = (zero.Y < this.Height) ? (zero.Y) : (this.Height);

                SolidBrush br = new SolidBrush(Color.FromArgb(230, 230, 230));

                g.FillRectangle(br, fill);
            }
        }

        private void PNSCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
           
            g.Clear(Color.White);

            DrawBoundary(g);

            if (ShowGrid)
            {
                DrawGrid(g);
            }

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            g.TranslateTransform(ViewInfo.Offset.X, ViewInfo.Offset.Y);
            g.ScaleTransform((float)ViewInfo.Zoom, (float)ViewInfo.Zoom);

            
            if (Objects != null)
            {
                int layer =0;
                for (layer = 0; layer < 5; layer++)
                {
                    foreach (IGraphicsObject obj in Objects)
                    {
                        if (obj.GetLayer() == layer)
                        {
                            obj.Draw(g, false);
                        }
                    }
                   
                }

                layer = 0;
                for (layer = 0; layer < 5; layer++)
                {
                    
                    foreach (IGraphicsObject obj in Objects)
                    {
                        if (obj.GetLayer() == layer)
                        {
                            obj.DrawGhost(g);
                        }
                    }
                }
            }

            if (DraggedObject!=null)
            {
                DraggedObject.Draw(g, false);
            }

            g.ResetTransform();
           
            if (DragInfo.Dragged == true && DragInfo.ObjectID == -2)
            {
                Pen p = new Pen(Color.Red, 1);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
           
                Rectangle selection = CreateRectangleFromPoints(DragInfo.OriginalCoords, DragInfo.LastCoords);
                g.DrawRectangle(p, selection);
            }

            if (ConnectorMode)
            {
                if (CurrentConnector.Begin != null)
                {
                    Pen p = new Pen(Color.Black, 1);
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                    Point begin = TranslateCanvasToScreen(CurrentConnector.Begin.GetConnectorBeginCoords());
                    Point end = PointToClient(Cursor.Position);

                    g.DrawLine(p, begin, end);
                }
            }
        }

        private void PNSCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (ConnectorMode)
                {
                    return;
                }
                IGraphicsObject hitted = GetHittedObject(e.Location);

                if (hitted == null)
                {
                    DragInfo.Reset();
                    DragInfo.Dragged = true;
                    DragInfo.ObjectID = -2;
                    DragInfo.OriginalCoords = e.Location;
                    DragInfo.LastCoords = e.Location;
                    DragInfo.PreviousSelection = GetSelectedObjects();
                    
                    
                    Rectangle selection = CreateRectangleFromPoints(TranslateScreenToCanvas(DragInfo.OriginalCoords), TranslateScreenToCanvas(DragInfo.LastCoords));
                    bool invertselectionmode = (Control.ModifierKeys == Keys.Shift);
                    UpdateSelection(selection, invertselectionmode);
                }
                else
                {
                    DragInfo.Reset();
                    DragInfo.Dragged = true;
                    DragInfo.ObjectID = hitted.GetID();
                    DragInfo.SelectedObject = hitted;
                    DragInfo.OriginalCoords = e.Location;
                    DragInfo.LastCoords = e.Location;

                    bool invertselectionmode = (Control.ModifierKeys == Keys.Shift);
                    if (hitted.IsSelected() == false || invertselectionmode==true)
                    {
                        UpdateSelection(hitted, invertselectionmode);
                    }
                    if (hitted.IsSelected() == true && hitted.IsMoveable())
                    {
                        Cursor = Cursors.SizeAll;
                    }
                    else
                    {
                        Cursor = Cursors.Hand;
                        DragInfo.ObjectID = 0;
                        DragInfo.Dragged = false;
                        DragInfo.SelectedObject = null;
                    }
                   
                 
                }

            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DragInfo.Reset();
                DragInfo.Dragged = true;
                DragInfo.ObjectID = -1;
                DragInfo.OriginalCoords = e.Location;
                DragInfo.LastCoords = e.Location;
            }
            
           
            Refresh();
        }

        private void PNSCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.None)
            {
                if (ConnectorMode)
                {
                    Cursor = Cursors.No;
                    IGraphicsObject ghitted = GetHittedObject(e.Location);
                    IConnectableObject hitted = ghitted as IConnectableObject;
                    if (hitted == null)
                    {
                        if (CurrentConnector.Begin != null)
                        {
                            Refresh();
                        }
                        return;
                    }
                    if (CurrentConnector.Begin == null && hitted.IsValidConnectorBegin(null))
                    {
                        Cursor = Cursors.Cross;
                    }
                    if (CurrentConnector.Begin != null && hitted.IsValidConnectorEnd(CurrentConnector.Begin))
                    {
                        Cursor = Cursors.Cross;
                    }
                    if (CurrentConnector.Begin != null)
                    {
                        Refresh();
                    }
                    return;
                }
            }
            if (DragInfo.Dragged)
            {
                if (e.Location.X == DragInfo.LastCoords.X && e.Location.Y == DragInfo.LastCoords.Y)
                {
                    return;
                }
                Point translatedcoords = TranslateScreenToCanvas(e.Location);
                if (DragInfo.ObjectID == -1)
                {
                    Point tmp = ViewInfo.Offset;
                    tmp.X += e.Location.X - DragInfo.LastCoords.X;
                    tmp.Y += e.Location.Y - DragInfo.LastCoords.Y;
                    ViewInfo.Offset = tmp;
                    Refresh();
                    OnViewChanged();
                    Cursor = Cursors.NoMove2D;
                }
                else if (DragInfo.ObjectID == -2)
                {
                    if (AddObjectMode)
                    {
                        Cursor = AddCursor;
                        return;
                    }
                    DragInfo.LastCoords = e.Location;
                    Rectangle selection = CreateRectangleFromPoints(TranslateScreenToCanvas(DragInfo.OriginalCoords), TranslateScreenToCanvas(DragInfo.LastCoords));
                    bool invertselectionmode = (Control.ModifierKeys == Keys.Shift);
                    UpdateSelection(selection, invertselectionmode);
                    Cursor = Cursors.Cross;
                }
                else if (Objects != null && Locked == false)
                {
                    
                    Point location = e.Location;

                    Point tmp = new Point();
                    tmp.X = (int)Math.Round((location.X - DragInfo.OriginalCoords.X) / ViewInfo.Zoom);
                    tmp.Y = (int)Math.Round((location.Y - DragInfo.OriginalCoords.Y) / ViewInfo.Zoom);

                    if (SnapToGrid && Control.ModifierKeys != Keys.Alt && DragInfo.SelectedObject != null && DragInfo.SelectedObject.IsLocked() == false && DragInfo.SelectedObject.IsMoveable() == true)
                    {
                        Point tmpcur = DragInfo.SelectedObject.GetCoords() + (Size)tmp;
                        Point tmpgrid = SnapCanvasPointToGrid(tmpcur);
                        tmp.X += tmpgrid.X - tmpcur.X;
                        tmp.Y += tmpgrid.Y - tmpcur.Y;
                        
                    }
                    
                    foreach (IGraphicsObject obj in Objects)
                    {
                        if (obj.IsSelected() && obj.IsLocked() == false)
                        {
                            
                            obj.SetOffset(tmp);
                        }
                    }
                }
                DragInfo.LastCoords = e.Location;
                Refresh();
            }
            else
            {
                IGraphicsObject hitted = GetHittedObject(e.Location);
                if (hitted == null)
                {
                    if (AddObjectMode)
                    {
                        if (Cursor != AddCursor)
                        {
                            Cursor = AddCursor;
                        }
                    }
                    else
                    {
                        if (Cursor != Cursors.Arrow) 
                        {
                            Cursor = Cursors.Arrow;
                        }
                    }
                }
                else
                {
                    if (hitted.IsSelected() && hitted.IsMoveable())
                    {
                        Cursor = Cursors.SizeAll;
                    }
                    else
                    {
                        Cursor = Cursors.Hand;
                    }
                }
            }
           
        }

        private void UpdateSelection(IGraphicsObject selected, bool invertselectionmode)
        {
            if (invertselectionmode == true)
            {
                selected.SetSelected(!selected.IsSelected());
                if (CurrentlyEdited != null && CurrentlyEdited != selected.GetParentObject())
                {
                    CurrentlyEdited.SetEditSelected(false);
                    CurrentlyEdited = null;
                }
            }
            else
            {
                List<IGraphicsObject> todeselect = new List<IGraphicsObject>();
                foreach (IGraphicsObject obj in Objects)
                {
                    if (obj!=selected && obj!=selected.GetParentObject())
                    {
                        if (obj.IsSelected() == true)
                        {
                            todeselect.Add(obj);
                        }
                    }
                }
                if (selected.IsSelected() == false)
                {
                    selected.SetSelected(true);
                    if (selected != selected.GetParentObject())
                    {
                        selected.GetParentObject().SetSelectedChild(selected);
                    }
                    if (CurrentlyEdited != null && CurrentlyEdited != selected && CurrentlyEdited != selected.GetParentObject())
                    {
                        CurrentlyEdited.SetEditSelected(false);
                    }
                    if (selected.IsPartialObject() == false)
                    {
                        selected.SetEditSelected(true);
                        CurrentlyEdited = selected;
                    }
                    else if (selected.GetParentObject() != CurrentlyEdited)
                    {
                        selected.GetParentObject().SetEditSelected(true);
                        CurrentlyEdited = selected.GetParentObject();
                    }
                }
                foreach (IGraphicsObject obj in todeselect)
                {
                    obj.SetSelected(false);
                }
                
            }
            /*
            if (selected != CurrentlyEdited && selected.GetParentObject() != CurrentlyEdited)
            {
                selected = null;
            }
             */
            if (GetSelectedObjects().Count != 1)
            {
                selected = null;
            }
            bool handled = OnClickedItem(selected);
            if (handled)
            {
                return;
            }
        }

        private void UpdateSelection(Rectangle selection, bool invertselectionmode)
        {
            List<IGraphicsObject> toselect = new List<IGraphicsObject>();
            List<IGraphicsObject> todeselect = new List<IGraphicsObject>();
            foreach (IGraphicsObject obj in Objects)
            {
                if (obj.IntersectsWith(selection))
                {
                    if (invertselectionmode == false)
                    {
                        if (obj.IsSelected() == false)
                        {
                            toselect.Add(obj);
                        }
                    }
                    else
                    {
                        if (DragInfo.PreviousSelection.Contains(obj))
                        {
                            if (obj.IsSelected() == true)
                            {
                                todeselect.Add(obj);
                            }
                        }
                        else
                        {
                            if (obj.IsSelected() == false)
                            {
                                toselect.Add(obj);
                            }
                        }
                    }
                }
                else
                {
                    if (invertselectionmode == false)
                    {
                        if (obj.IsSelected() == true)
                        {
                            todeselect.Add(obj);
                        }
                    }
                    else
                    {
                        if (DragInfo.PreviousSelection.Contains(obj))
                        {
                            if (obj.IsSelected() == false)
                            {
                                toselect.Add(obj);
                            }
                        }
                        else
                        {
                            if (obj.IsSelected() == true)
                            {
                                todeselect.Add(obj);
                            }
                        }
                    }
                }
            }
            foreach (IGraphicsObject obj in toselect)
            {
                obj.SetSelected(true);
            }
            foreach (IGraphicsObject obj in todeselect)
            {
                obj.SetSelected(false);
            }
            if (CurrentlyEdited != null)
            {
                CurrentlyEdited.SetEditSelected(false);
                CurrentlyEdited = null;
            }
            OnSelectionChanged();
        }

        private void PNSCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (ConnectorMode && Locked == false)
            {
                IGraphicsObject ghitted = GetHittedObject(e.Location);
                IConnectableObject hitted = ghitted as IConnectableObject;
                if (hitted == null)
                {
                    
                    CurrentConnector.Reset();
                    DragInfo.Reset();
                    Refresh();
                    return;
                }
                if (CurrentConnector.Begin == null && hitted.IsValidConnectorBegin(null))
                {
                    CurrentConnector.Begin = hitted;
                }
                else if (CurrentConnector.Begin != null)
                {
                    if (hitted.IsValidConnectorEnd(CurrentConnector.Begin))
                    {
                        CurrentConnector.End = hitted;

                        OnNewConnector();
                        OnDataChanged();
                    }
                    CurrentConnector.Reset();
                }
                DragInfo.Reset();
                Refresh();
                return;
            }
            if (Objects != null && DragInfo.Dragged == true && DragInfo.ObjectID>=0 && Locked==false)
            {
                List<IGraphicsObject> toupdate = new List<IGraphicsObject>();
                bool datachanged = false;
                foreach (IGraphicsObject obj in Objects)
                {
                    toupdate.Add(obj);
                }
                foreach (IGraphicsObject obj in toupdate)
                {
                    if (obj.GetOffset().IsEmpty == false)
                    {
                        datachanged = true;
                    }
                    obj.IntegrateOffset();
                }
                if (datachanged)
                {
                    OnDataChanged();
                }
            }
            DragInfo.Reset();
            this.Refresh();
        }

        private IGraphicsObject GetHittedObject(Point location)
        {
            Point translatedcoords = TranslateScreenToCanvas(location);
            int selectedlayer = 0;
            IGraphicsObject selected = null;
            if (Objects != null)
            {
                foreach (IGraphicsObject obj in Objects)
                {
                    if (obj.HitTest(translatedcoords) && selectedlayer <= obj.GetLayer())
                    {
                        selected = obj;
                        selectedlayer = obj.GetLayer();
                    }
                }
            }
            return selected;
        }

        private void PNSCanvas_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private Point SnapCanvasPointToGrid(Point canvaspoint)
        {
            Point ret = new Point();

            ret.X = ((int)Math.Round(canvaspoint.X / (double) GridSize)) * GridSize;
            ret.Y = ((int)Math.Round(canvaspoint.Y / (double) GridSize)) * GridSize;

            return ret;
        }

        private Point SnapScreenPointToGrid(Point screenpoint)
        {
            return TranslateCanvasToScreen(SnapCanvasPointToGrid(TranslateScreenToCanvas(screenpoint)));
        }

        private Point TranslateScreenToCanvas(Point screenpoint)
        {
            Point ret = new Point();
            ret.X = (int)Math.Round((screenpoint.X - ViewInfo.Offset.X) / ViewInfo.Zoom);
            ret.Y = (int)Math.Round((screenpoint.Y - ViewInfo.Offset.Y) / ViewInfo.Zoom);
            return ret;
        }

        private Point TranslateCanvasToScreen(Point canvaspoint)
        {
            Point ret=new Point();
            ret.X = ((int)Math.Round(canvaspoint.X * ViewInfo.Zoom)) + ViewInfo.Offset.X;
            ret.Y = ((int)Math.Round(canvaspoint.Y * ViewInfo.Zoom)) + ViewInfo.Offset.Y;
            return ret;
        }

        private Rectangle CreateRectangleFromPoints(Point p1, Point p2)
        {
            int x, y, w, h;
            if (p1.X < p2.X)
            {
                x = p1.X;
                w = p2.X - p1.X;
            }
            else
            {
                x = p2.X;
                w = p1.X - p2.X;
            }
            if (p1.Y < p2.Y)
            {
                y = p1.Y;
                h = p2.Y - p1.Y;
            }
            else
            {
                y = p2.Y;
                h = p1.Y - p2.Y;
            }
            Rectangle ret = new Rectangle(x, y, w, h);
            return ret;
        }

        private void PNSCanvas_DragEnter(object sender, DragEventArgs e)
        {
            String[] dataformats = e.Data.GetFormats();
            Object tmpobj = null;
            if (dataformats.Length > 0 && e.Data.GetDataPresent(Type.GetType(dataformats[0])))
            {
                tmpobj = e.Data.GetData(Type.GetType(dataformats[0])) as Object;
            }
            IGraphicsObject obj = null;
            if (tmpobj != null)
            {
                obj = tmpobj as IGraphicsObject;
            }
            if (obj != null)
            {
                DraggedObject = obj;
                e.Effect = DragDropEffects.Move;
            }
           
        }

        private void PNSCanvas_DragDrop(object sender, DragEventArgs e)
        {
            if (Locked == false)
            {
                AddObject(DraggedObject);
            }
            DraggedObject = null;
            Refresh();
            OnDataChanged();

        }

        private void PNSCanvas_DragLeave(object sender, EventArgs e)
        {
            DraggedObject = null;
            Refresh();
        }

        private void PNSCanvas_DragOver(object sender, DragEventArgs e)
        {
            if (DraggedObject != null)
            {
                Point canvascoords = PointToClient(new Point(e.X, e.Y));
                DraggedObject.SetCoords(TranslateScreenToCanvas(canvascoords));
                Refresh();
            }
        }

        private void PNSCanvas_Click(object sender, EventArgs e)
        {
            bool found = false;
            MouseEventArgs m = e as MouseEventArgs;
            if (m == null || (m.X != DragInfo.OriginalCoords.X || m.Y != DragInfo.OriginalCoords.Y))
            {
                return;
            }
            Point translatedcoords = TranslateScreenToCanvas(m.Location);
            if (Objects != null)
            {
                foreach (IGraphicsObject obj in Objects)
                {
                    if (obj.HitTest(translatedcoords))
                    {
                        if (m.Button == MouseButtons.Left)
                        {
                            found = true;
                        }
                    }
                }
            }
            if (found == false && Locked==false && m.Button == MouseButtons.Left)
            {
                IGraphicsObject newitem = GetNewItem();
                if (newitem != null)
                {
                    newitem.SetCoords(translatedcoords);
                    this.AddObject(newitem);
                    Refresh();
                    OnDataChanged();
                }
            }
        }

        public void SnapSelectedObjectsToGrid()
        {
            if (Objects != null && Locked == false)
            {
                List<IGraphicsObject> tosnap = new List<IGraphicsObject>();
                foreach (IGraphicsObject obj in Objects)
                {
                    if (obj.IsSelected() && obj.IsMoveable())
                    {
                        tosnap.Add(obj);
                    }
                }

                foreach (IGraphicsObject obj in tosnap)
                {
                    Point Offset = SnapCanvasPointToGrid(obj.GetCoords()) - (Size)obj.GetCoords();
                    obj.SetOffset(Offset);
                    
                }

                foreach (IGraphicsObject obj in tosnap)
                {
                    obj.IntegrateOffset();
                }
            }
            Refresh();
        }

        public void Reset()
        {
            ViewInfo.Reset();
        }

        private void PNSCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && Locked == false)
            {
                List<IGraphicsObject> toremove = new List<IGraphicsObject>();
                foreach (IGraphicsObject obj in Objects)
                {
                    if (obj.IsSelected())
                    {
                        toremove.Add(obj);
                    }
                }
                if (toremove.Count > 0)
                {
                    OnRemoveObjects(toremove);
                }
            }
            if (e.KeyCode == Keys.C && e.Control == true)
            {
                OnCopy();
            }
            if (e.KeyCode == Keys.V && e.Control == true)
            {
                OnPaste();
            }
            if (e.KeyCode == Keys.D && e.Control == true)
            {
                OnDuplicate();
            }
            ResetKeyModifiers();
            e.Handled = true;
        }

        private void ResetKeyModifiers()
        {
            
        }

        private void PNSCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = false;
        }

        private void PNSCanvas_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs m = e as MouseEventArgs;
            IGraphicsObject o = GetHittedObject(m.Location);
            if (o!=null)
            {
                OnEditItem(o);
            }
        }

        public void SetViewCenter(Point center)
        {
            Point screencentercoords = TranslateCanvasToScreen(center);

            ViewInfo.Offset -= (Size)screencentercoords - new Size(this.Width / 2, this.Height / 2);
            Refresh();
            OnViewChanged();
        }

        public Rectangle GetCurrentViewBoundary()
        {
            Point tl = TranslateScreenToCanvas(new Point(0,0));
            Point br = TranslateScreenToCanvas(new Point(this.Width, this.Height));

            return CreateRectangleFromPoints(tl, br);
        }

        public Point TranslateBoundaryToCanvas(Rectangle boundary, Point boundarycoord)
        {

            Rectangle canvasboundary = GetCanvasBoundary();

            double zoom = boundary.Width / (double)canvasboundary.Width;
            if (zoom > boundary.Height / (double)canvasboundary.Height)
            {
                zoom = boundary.Height / (double)canvasboundary.Height;
            }

            double ratiox = boundary.Width / (zoom * canvasboundary.Width);
            double ratioy = boundary.Height / (zoom * canvasboundary.Height);

            Point offset = new Point();
            offset.X = (int)((boundarycoord.X - boundary.Width / 2) / ((double)(boundary.Width / 2 / ratiox)) * (canvasboundary.Width / 2) + canvasboundary.Width / 2 + canvasboundary.X);
            offset.Y = (int)((boundarycoord.Y - boundary.Height / 2) / ((double)(boundary.Height / 2 / ratioy)) * (canvasboundary.Height / 2) + canvasboundary.Height / 2 + canvasboundary.Y);
            return offset;
        }

        public Point TranslateCanvasToBoundary(Rectangle boundary, Point canvascoord)
        {
            Rectangle canvasboundary = GetCanvasBoundary();

            double zoom = boundary.Width / (double)canvasboundary.Width;
            if (zoom > boundary.Height / (double)canvasboundary.Height)
            {
                zoom = boundary.Height / (double)canvasboundary.Height;
            }

            double ratiox = boundary.Width / (zoom * canvasboundary.Width);
            double ratioy = boundary.Height / (zoom * canvasboundary.Height);


            Point offset = new Point();
           
            offset.X = (int)((canvascoord.X - (canvasboundary.Width / 2 + canvasboundary.X)) * ((double)(boundary.Width / 2 / ratiox)) / ((double)(canvasboundary.Width / 2)) + boundary.Width / 2);
            offset.Y = (int)((canvascoord.Y - (canvasboundary.Height / 2 + canvasboundary.Y)) * ((double)(boundary.Height / 2 / ratioy)) / ((double)(canvasboundary.Height / 2)) + boundary.Height / 2);
            return offset;
        }

        private void PNSCanvas_SizeChanged(object sender, EventArgs e)
        {
            OnViewChanged();
        }

    }

    public delegate void CanvasEventHandler(object sender, CanvasEventArgs e);

    public class CanvasEventArgs : EventArgs
    {
        public Object Data;
        public bool Handled;

        public CanvasEventArgs()
        {
            this.Data = null;
            this.Handled = false;
        }
    }

    public class ObjectConnector
    {
        public IConnectableObject Begin;
        public IConnectableObject End;

        public ObjectConnector()
        {
            Begin = null;
            End = null;
        }

        public void Reset()
        {
            Begin = null;
            End = null;
        }
    }
}
