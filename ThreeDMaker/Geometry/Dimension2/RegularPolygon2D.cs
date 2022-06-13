using System;
using System.Collections.Generic;
using System.Numerics;

namespace ThreeDMaker.Geometry.Dimension2
{
    public class RegularPolygon2D:Shape2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float RotationDegree { get; set; }
        public float R { get; set; }
        public int N { get; set; }
        public bool StartWithCorner { get; set; }
        public bool IsCentertoFaceSize { get; set; }

        public RegularPolygon2D(float r, int n, bool startWithCorner = false, bool isCentertoFaceSize = true, float x = 0, float y = 0, float rotationDegree = 0)
        {
            R = r;
            N = n;
            StartWithCorner = startWithCorner;
            IsCentertoFaceSize = isCentertoFaceSize;
            X = x;
            Y = y;
            RotationDegree = rotationDegree;
        }

        public override void UpdatePoints()
        {
            points.Clear();

            float dAngle = 2 * (float)Math.PI / N;
            if (IsCentertoFaceSize)
            {
                R *= (float)Math.Cos(dAngle / 2);
            }

            float startAngle2 = (-RotationDegree - 90) * (float)Math.PI / 2;

            if (!StartWithCorner)
            {
                startAngle2 += dAngle / 2;
            }
            for (int i = 0; i <= N; i++)
            {
                float angle = dAngle * i;
                float x = R * (float)Math.Cos(angle + startAngle2);
                float y = R * (float)Math.Sin(angle + startAngle2);
                Add(x, y);
            }
            
        }

        public override Shape2D GetOffSet(float d)
        {
            return new RegularPolygon2D(R + d, N, StartWithCorner, IsCentertoFaceSize);
        }
    }
}
