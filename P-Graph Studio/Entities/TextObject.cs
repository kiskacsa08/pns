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

using PNSDraw.Configuration;

namespace PNSDraw
{
    public class TextObject : Canvas.IGraphicsObject
    {
        int ID;
        Point coords;
        Point offset;

        bool Selected = false;
        Canvas.IGraphicsObject Owner;
        Color color;
        Canvas.IGraphicsStructure Container;
        public string text  = "";
        Size textsize;
        int fontsize;

        Point OwnerOriginalCoords;

        bool Locked = false;
        bool Moved = false;

        [Browsable(false)]
        public int FontSize
        {
            get
            {
                return fontsize;
            }
            set
            {
                fontsize = value;
            }
        }

        int CurrentFontSize
        {
            get
            {
                if (fontsize < 0)
                {
                    return Config.Instance.GraphSettings.DefaultFontSize;
                }
                else
                {
                    return fontsize;
                }
            }
        }

        bool Highlighted;

        public TextObject(Canvas.IGraphicsStructure container, Canvas.IGraphicsObject owner)
        {
            ID = 0;
            coords = new Point(0, 0);
            offset = new Point(0, 0);
            color = Color.Black;
            textsize = new Size();
            Container = container;
            Owner = owner;
            Highlighted = false;
            OwnerOriginalCoords = new Point();
            fontsize = -1;
        }

        public void SetID(int newid)
        {
            ID = newid;
        }

        public int GetID()
        {
            return ID;
        }


        public string ConvertToSVG(int zorder)
        {
            string svg = "";
            Point mcoords = Owner.GetCoords() + (Size)coords;
            // A szoveg aljat kell megadni ezert a magassagot hozza kell adni, es hackelni kell magassagot
            mcoords.X += CurrentFontSize;

            List<string> separators = new List<string>();
            separators.Add(Environment.NewLine);

            string[] lines = Text.Split(separators.ToArray(), StringSplitOptions.RemoveEmptyEntries);


            int linecount = lines.GetLength(0);
            foreach (string oneline in lines)
            {

                mcoords.Y += (int)((double)textsize.Height * (1.0 / (double)linecount) * (1 - 1 / ((double)(linecount * 3 + 1))));


                svg += String.Format("<text x=\"{0}\" y=\"{1}\" style=\"fill: {3}; font-size: {4}pt; font-family: sans-serif;\">{2}</text>\n",
                ConvertManager.ToString(mcoords.X / 5),
                ConvertManager.ToString(mcoords.Y / 5), oneline, ConvertManager.ToHtml(color), CurrentFontSize);
            }
            return svg;
        }

        public void DrawString(Graphics g, Color c, Point mcoords)
        {
            Font f = new Font(FontFamily.GenericSansSerif, CurrentFontSize * 5);
            SolidBrush brush = new SolidBrush(c);
            SizeF size = g.MeasureString(Text, f);
            textsize = size.ToSize();
            g.DrawString(Text, f, brush, mcoords);

        }

        public void DrawSelectionRectangle(Graphics g, Color color, bool current)
        {
            Rectangle rect = new Rectangle(Owner.GetCoords() + (Size)coords, textsize);
            if (current)
            {
                rect = new Rectangle(Owner.GetCurrentCoords() + (Size)coords + (Size)offset, textsize);
            }


            Pen p = new Pen(new SolidBrush(color), Config.Instance.LineSize);

            rect.X -= 5;
            rect.Y -= 5;
            rect.Width += 10;
            rect.Height += 10;
            g.DrawRectangle(p, rect);
        }

        public void Draw(Graphics g, bool plain)
        {
            if (Selected && plain == false)
            {
                DrawString(g, Color.Red, Owner.GetCoords() + (Size)coords);


                DrawSelectionRectangle(g, Color.Red, false);

            }
            else
            {
                DrawString(g, color, Owner.GetCoords() + (Size)coords);
                if (GetParentObject().IsSelected() && plain == false)
                {
                    DrawSelectionRectangle(g, Color.Gray, false);
                }

            }
        }

        public void DrawGhost(Graphics g)
        {
            if (offset.IsEmpty == false || Owner.GetOffset().IsEmpty == false)
            {
                Point ownercurrentcoords = Owner.GetCurrentCoords();
                Point diff = ownercurrentcoords - (Size)OwnerOriginalCoords;
                if (diff.IsEmpty == true)
                {
                    DrawString(g, Color.Silver, Owner.GetCurrentCoords() + (Size)coords + (Size)offset);
                }
                else
                {
                    DrawString(g, Color.Silver, Owner.GetCurrentCoords() + (Size)coords);
                }

            }

        }

        public bool HitTest(Point mousecoords)
        {
            Rectangle rect = new Rectangle(Owner.GetCoords() + (Size)coords, textsize);
            if (rect.Contains(mousecoords))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Point GetCoords()
        {
            return Owner.GetCoords() + (Size)coords;
        }

        public void SetCoords(Point newcoords)
        {
            coords = newcoords;
        }

        public void SetSelected(bool selected)
        {
            Selected = selected;
            Owner.SetHighlighted(selected);
        }


        public bool IsSelected()
        {
            return Selected;
        }


        public void SetOffset(Point newoffset)
        {
            if (Owner.IsSelected() == false && Locked == false)
            {
                offset = newoffset;
                OwnerOriginalCoords = Owner.GetCoords();
                Moved = true;
            }
        }


        public void IntegrateOffset()
        {
            if (Moved)
            {
                Point ownercurrentcoords = Owner.GetCurrentCoords();
                Point diff = ownercurrentcoords - (Size)OwnerOriginalCoords;
                if (diff.IsEmpty == true)
                {
                    coords += (Size)offset;
                }

                offset.X = 0;
                offset.Y = 0;
                Moved = false;
            }
        }


        public bool IntersectsWith(Rectangle rect)
        {
            Rectangle textrect = new Rectangle(Owner.GetCoords() + (Size)coords, textsize);
            return rect.IntersectsWith(textrect);
        }


        public int GetLayer()
        {
            return 3;
        }


        public Point GetCurrentCoords()
        {
            Point cur = (Point)(Owner.GetCoords() + (Size)coords + (Size)offset);
            return cur;
        }


        public Point GetOffset()
        {
            return offset;
        }


        [Browsable(false)]
        [DisplayName("Text")]
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        [Browsable(false)]
        [DisplayName("Offset")]
        public Point Offset
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

        [Browsable(false)]
        [DisplayName("Color")]
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
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
            Rectangle rect = new Rectangle(Owner.GetCoords() + (Size)coords, textsize);
            return rect;
        }


        public void SetSelectedChild(Canvas.IGraphicsObject child)
        {

        }

        public void Pin(int fp)
        {

        }
        public int getPin()
        {
            return 0;
        }

        public bool IsMoveable()
        {
            return true;
        }

        public bool IsDeletable()
        {
            return false;
        }
    }
}