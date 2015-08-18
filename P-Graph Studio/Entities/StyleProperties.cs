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
using System.Reflection;

namespace PNSDraw
{
    [TypeConverter(typeof(StylePropertySorter))]
    public class DisplayTextStyle : TextObject
    {
        public DisplayTextStyle(Canvas.IGraphicsStructure container, Canvas.IGraphicsObject owner):base(container, owner)
        { }

        #region functions

        #endregion

        [Browsable(true)]
        [PropertyOrder(10)]
        [DisplayName("Font size")]
        public string FontSizeProp
        {
            get
            {
                int size = base.FontSize;
                if (size <= 0)
                {
                    return Config.Instance.GraphSettings.DefaultFontSize.ToString() + " (default)";
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
                    base.FontSize = -1;
                }
                else
                {
                    base.FontSize = newsize;
                }
            }
        }

        [Browsable(true)]
        [PropertyOrder(20)]
        [DisplayName("Color")]
        public Color ColorProp
        {
            get
            {
                return base.Color;
            }
            set
            {
                base.Color = value;
            }
        }

        [Browsable(true)]
        [PropertyOrder(30)]
        [DisplayName("Offset")]
        public Point OffsetProp
        {
            get
            {
                return base.Offset;
            }
            set
            {
                base.Offset = value;
            }
        }
    }

    [TypeConverter(typeof(StylePropertySorter))]
    public class NodeStyle : TextObject
    {
        Point coords;

        public NodeStyle(Canvas.IGraphicsStructure container, Canvas.IGraphicsObject owner): base(container,owner)
        {
            coords = new Point(0, 0);
        }

        #region functions

        #endregion

        [Browsable(true)]
        [PropertyOrder(10)]
        [DisplayName("Color")]
        public Color ColorProp
        {
            get
            {
                return base.Color;
            }
            set
            {
                base.Color = value;
            }
        }

        [Browsable(true)]
        [PropertyOrder(20)]
        [DisplayName("Coords")]
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
    }

    [TypeConverter(typeof(StylePropertySorter))]
    public class EdgeDisplayTextStyle : TextObject
    {
        bool longformat;

        Edge owner;

        public EdgeDisplayTextStyle(Canvas.IGraphicsStructure container, Canvas.IGraphicsObject owner)
            : base(container, owner)
        {
            longformat = false;
            if (owner.GetType() == typeof(Edge))
            {
                this.owner = (Edge)owner;
            }
            else
            {
                this.owner = null;
            }
        }

        #region functions

        #endregion

        [Browsable(true)]
        [PropertyOrder(10)]
        [DisplayName("Font size")]
        public string FontSizeProp
        {
            get
            {
                int size = base.FontSize;
                if (size <= 0)
                {
                    return Config.Instance.GraphSettings.DefaultFontSize.ToString() + " (default)";
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
                    base.FontSize = -1;
                }
                else
                {
                    base.FontSize = newsize;
                }
            }
        }

        [Browsable(true)]
        [PropertyOrder(20)]
        [DisplayName("Color")]
        public Color ColorProp
        {
            get
            {
                return base.Color;
            }
            set
            {
                base.Color = value;
            }
        }

        [Browsable(true)]
        [PropertyOrder(30)]
        [DisplayName("Offset")]
        public Point OffsetProp
        {
            get
            {
                return base.Offset;
            }
            set
            {
                base.Offset = value;
            }
        }

        [Browsable(true)]
        [PropertyOrder(40)]
        [DisplayName("Long format")]
        public bool LongFormatProp
        {
            get
            {
                return longformat;
            }

            set
            {
                if (owner != null)
                {
                    longformat = value;
                    owner.UpdateParametersLabel();
                }
            }
        }
    }

    [TypeConverter(typeof(StylePropertySorter))]
    public class EdgeStyle : TextObject
    {
        double position;
        bool center;

        Edge owner;

        public EdgeStyle(Canvas.IGraphicsStructure container, Canvas.IGraphicsObject owner)
            : base(container, owner)
        {
            position = 0.5F;
            center = true;

            if (owner.GetType() == typeof(Edge))
            {
                this.owner = (Edge)owner;
            }
            else
            {
                this.owner = null;
            }
        }

        #region functions

        #endregion

        [Browsable(true)]
        [PropertyOrder(10)]
        [DisplayName("Color")]
        public Color ColorProp
        {
            get
            {
                return base.Color;
            }
            set
            {
                base.Color = value;
            }
        }

        [Browsable(true)]
        [PropertyOrder(20)]
        [DisplayName("Arrow Position (%)")]
        public double PositionProp
        {
            get
            {
                return Math.Round(position * 100);
            }
            set
            {
                if (owner != null)
                {
                    position = value / 100;
                    if (position < 0)
                    {
                        position = 0;
                    }
                    if (position > 1)
                    {
                        position = 1;
                    }
                    
                }                
            }
        }

        [Browsable(true)]
        [PropertyOrder(30)]
        [DisplayName("Arrow on center")]
        public bool OnCenterProp
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }
    }
}
