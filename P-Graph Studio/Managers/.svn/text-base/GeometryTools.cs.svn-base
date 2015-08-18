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
