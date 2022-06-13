using System;
using System.Collections.Generic;
using System.Numerics;
namespace ThreeDMaker.Geometry.Dimension2
{
    public class Circle2D:Shape2D
    {
        public float R { get; set; }
        public int Sections { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Circle2D(float r, int sections = 16, float x = 0, float y = 0)
        {
            R = r;
            Sections = sections;
            X = x;
            Y = y;
            points = new List<Vector2>();
            UpdatePoints();
        }

        public override void UpdatePoints()
        {
            points.Clear();

            float dAngle = 2 * (float) Math.PI / Sections;

           
            float startAngle2 = -(float) Math.PI / 2;

            for (int i = 0; i < Sections; i++)
            {
                float angle = dAngle * i;
                float x = X + R * (float) Math.Cos(angle + startAngle2);
                float y = Y + R * (float) Math.Sin(angle + startAngle2);
                Add(x, y);
            }
        }

        public override Shape2D GetOffSet(float d)
        {
            return new Circle2D(R + d, Sections);
        }
    }
}
