using System;
using System.Collections.Generic;
using System.Numerics;

namespace ThreeDMaker.Geometry.Dimension2
{
    public class Polygon2D:Polyline2D
    {

        public Polygon2D()
        {
            Points = new List<Vector2>();
        }

        public Polygon2D(List<Vector2> vector2s)
        {
            Points = new List<Vector2>();
            foreach (var v in vector2s)
            {
                Points.Add(v);
            }
            UpdatePoints();
        }
        public Polygon2D(Polyline2D polyLine)
        {
            Points = new List<Vector2>();
            Points.AddRange(polyLine.points);
            UpdatePoints();
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

            

            for (int i = 0; i < offestlines.Count; i++)
            {
                int j = i - 1;
                if(j < 0)
                {
                    j = Count - 1;
                }
                Vector2 point = ((Line2D)offestlines[j]).GetLineInterSec((Line2D)offestlines[i]);
                if (float.IsNaN(point.X) || float.IsNaN(point.Y))
                {
                    offset.Add(offestlines[i].points[0]);
                }
                else
                {
                    offset.Add(point);
                }
            }
            return new Polygon2D(offset);
        }

        public override void UpdatePoints()
        {
            points = Points;
        }
    }
}
