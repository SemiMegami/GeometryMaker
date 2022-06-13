using System;
using System.Collections.Generic;
using System.Numerics;
namespace ThreeDMaker.Geometry.Dimension2
{
    public class Polyline2D:Shape2D
    {

        public List<Vector2> Points;
        public Polyline2D()
        {
            Points = new List<Vector2>();
        }

        public Polyline2D(List<Vector2> vector2s)
        {
            Points = new List<Vector2>();
            foreach (var v in vector2s)
            {
                Points.Add(v);
            }
            UpdatePoints();
        }
        public Polyline2D(Polyline2D polyLine)
        {
            Points = new List<Vector2>();
            Points.AddRange(polyLine.points);
            UpdatePoints();
        }
        
        public List<Line2D> GetLines()
        {
            List<Line2D> lines = new List<Line2D>();
            for (int i = 0; i < Count - 1; i++)
            {
                lines.Add(new Line2D(points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y));
            }
            return lines;
        }

        public override Shape2D GetOffSet(float d)
        {
            List<Line2D> lines = GetLines();


            List<Shape2D> offestlines = new List<Shape2D>();
            for (int i = 0; i < lines.Count; i++)
            {
                offestlines.Add(lines[i].GetOffSet(d));
            }

            Vector2s offset = new Vector2s();

            offset.Add(offestlines[0].points[0]);

            for (int i = 0; i < offestlines.Count - 1; i++)
            {
                Vector2 point = ((Line2D)offestlines[i]).GetLineInterSec((Line2D)offestlines[i + 1],true);
                if(float.IsNaN(point.X) || float.IsNaN(point.Y))
                {
                    offset.Add(offestlines[i].points[1]);
                }
                else
                {
                    offset.Add(point);
                }
            }
            offset.Add(offestlines[Count - 1].points[1]);
            return new Polyline2D(offset);
        }

        public override void UpdatePoints()
        {
            points = Points;
        }
    }
}

