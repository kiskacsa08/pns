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

namespace PNSDraw
{
    class GeometryTools
    {
        static public Rectangle CreateRectangleFromPoints(Point p1, Point p2)
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

        static public Rectangle GetPointsBoundary(List<Point> points)
        {
            if (points.Count == 0)
            {
                return new Rectangle();
            }
            Point min = points[0];
            Point max = points[0];
            foreach (Point p in points)
            {
                if (p.X < min.X)
                {
                    min.X = p.X;
                }
                if (p.Y < min.Y)
                {
                    min.Y = p.Y;
                }
                if (p.X > max.X)
                {
                    max.X = p.X;
                }
                if (p.Y > max.Y)
                {
                    max.Y = p.Y;
                }
            }
            Rectangle ret = CreateRectangleFromPoints(min, max);
            return ret;
        }

    }
}
