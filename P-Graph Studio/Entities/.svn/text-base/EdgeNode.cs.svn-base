using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace PNSDraw
{
    public class EdgeNode : Canvas.IGraphicsObject
    {
        int ID;
        Point coords;
        Point offset;
        public bool Selected = false;
        Edge Owner;
        Canvas.IGraphicsStructure Container;
        bool Locked=false;

        public bool Temporary;
        public bool Needed;

        public bool Smooth = true;

        public int SmoothSizeBefore = 250;
        public int SmoothSizeAfter = 250;

        public double SmoothStrength = 0.5;

        [Browsable(false)]
        public double SmoothStrengthBefore
        {
            get
            {
                return SmoothStrength;
            }
        }

        [Browsable(false)]
        public double SmoothStrengthAfter
        {
            get
            {
                return SmoothStrength;
            }
        }

        bool Highlighted;

        bool Moved = false;

        public EdgeNode(Canvas.IGraphicsStructure container, Edge owner)
        {
            ID = 0;
            coords = new Point(0, 0);
            offset = new Point(0, 0);
            Container = container;
            Owner = owner;
            Highlighted = false;
            Temporary = true;
            Needed = false;
        }

        public void SetID(int newid)
        {
            ID = newid;
        }

        public int GetID()
        {
            return ID;
        }

        public void DrawNode(Graphics g, Color c, Point mcoords, int size)
        {
            SolidBrush brush = new SolidBrush(c);
          
            Point tmp = new Point(mcoords.X - size,
                mcoords.Y - size);
            Rectangle rect = new Rectangle(tmp, new Size(2 * size, 2 * size));
            g.FillEllipse(brush, rect);
        }

        public void Draw(Graphics g, bool plain)
        {
            if (plain)
            {
                return;
            }
            Color c = Color.Black;
            int size = Globals.EdgeNodeSize;
            if (Selected)
            {
                c = Color.Red;
            }
            if (Needed == false)
            {
                size = Globals.TemporaryEdgeNodeSize;
               
            }
            DrawNode(g, Color.White, coords, size + 10);
            DrawNode(g, c, coords, size);
        }

        public void DrawGhost(System.Drawing.Graphics g)
        {
            if (Needed == true)
            {
                if (offset.IsEmpty == false || Owner.GetOffset().IsEmpty == false)
                {
                    DrawNode(g, Color.Silver, coords + (Size)offset, Globals.EdgeNodeSize);
                }
            }
        }

        public bool HitTest(Point mousecoords)
        {
            double dx = (double)coords.X - mousecoords.X;
            double dy = (double)coords.Y - mousecoords.Y;
            if (Math.Round(Math.Sqrt(dx * dx + dy * dy)) < Globals.EdgeNodeSize)
            {
                return true;
            }
            return false;
        }

        public Point GetCoords()
        {
            return coords;
        }

        public void SetCoords(Point newcoords)
        {
            coords = newcoords;
        }

        public void SetSelected(bool selected)
        {
            Selected = selected;
            Owner.UpdateEditedState();
        }


        public bool IsSelected()
        {
            return Selected;
        }


        public void SetOffset(Point newoffset)
        {
            offset = newoffset;
            Moved = true;
            
            Owner.UpdateNodesNeededState();
            /*
            if (Owner.IsNeededNode(this) == true)
            {
                Temporary = false;
            }
            else
            {
                Temporary = true;
            }*/
        }


        public void IntegrateOffset()
        {
            if (Moved)
            {
                coords += (Size)offset;
                offset.X = 0;
                offset.Y = 0;
                Temporary = false;
                Owner.UpdateNodes();
            }
            Moved = false;
            
        }


        public bool IntersectsWith(Rectangle rect)
        {
            rect.Inflate(Globals.EdgeNodeSize, Globals.EdgeNodeSize);
            return rect.Contains(Owner.GetCoords() + (Size)coords);
        }


        public int GetLayer()
        {
            return 3;
        }


        public Point GetCurrentCoords()
        {
            Point cur = (Point)((Size)coords + (Size)offset);
            return cur;
        }


        public Point GetOffset()
        {
            return offset;
        }

        [Browsable(true)]
        [DisplayName("Offset")]
        public Point CoordsProp
        {
            get
            {
                return coords;
            }
            set
            {
                coords = value;
            }
        }




        public void SetEditSelected(bool selected)
        {

        }

        public bool IsPartialObject()
        {
            return true;
        }

        public Canvas.IGraphicsObject GetParentObject()
        {
            return Owner;
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
            return new Rectangle(coords, new Size(0,0));
        }


        public void SetSelectedChild(Canvas.IGraphicsObject child)
        {
            
        }


        public bool IsMoveable()
        {
            return true;
        }

        public bool IsDeletable()
        {
            return true;
        }


        [Browsable(true)]
        [DisplayName("Smooth")]
        public bool SmoothProp
        {
            get
            {
                return Smooth;
            }
            set
            {
                Smooth = value;
            }
        }

        [Browsable(true)]
        [DisplayName("Smooth Before")]
        public int SmoothSizeBeforeProp
        {
            get
            {
                return SmoothSizeBefore;
            }
            set
            {
                SmoothSizeBefore = value;
            }
        }

        [Browsable(true)]
        [DisplayName("Smooth After")]
        public int SmoothSizeAfterProp
        {
            get
            {
                return SmoothSizeAfter;
            }
            set
            {
                SmoothSizeAfter = value;
            }
        }

        [Browsable(true)]
        [DisplayName("Smooth Strength (%)")]
        public double SmoothStrengthProp
        {
            get
            {
                return Math.Round((1 - SmoothStrength)*1000)/10;
            }
            set
            {
                SmoothStrength = value / 100;
                if (SmoothStrength > 1)
                {
                    SmoothStrength = 1;
                }
                if (SmoothStrength < 0)
                {
                    SmoothStrength = 0;
                }
                SmoothStrength = 1 - SmoothStrength;
            }
        }

        

    }
}
