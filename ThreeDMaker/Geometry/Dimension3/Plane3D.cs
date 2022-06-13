using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDMaker.Geometry.Dimension3
{
    public class Plane3D
    {
        Vector3 N { get; set;}
        float X0 { get; set; }
        float Y0 { get; set; }
        float Z0 { get; set; }

        public Plane3D()
        {
        }
            public Plane3D(Vector3 n, float x, float y, float z)
        {
            N = n;
            X0 = x;
            Y0 = y;
            Z0 = z;
        }

        public Plane3D(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 p12 = p2 - p1;
            Vector3 p13 = p3 - p1;
            N = Vector3.Normalize(Vector3.Cross(p12, p13));
            X0 = p1.X;
            Y0 = p1.Y;
            Z0 = p1.Z;
        }

        public Plane3D(float A, float B, float C, float D)
        {
            N = new Vector3(A, B, C);
            if(A != 0)
            {
                X0 = - D / A;
                Y0 = 0;
                Z0 = 0;
            }
            else if(B != 0)
            {
                X0 = 0;
                Y0 = -D /A;
                Z0 = 0;
            }
            else
            {
                X0 = 0;
                Y0 = 0;
                Z0 = -D / A;
            }

        }

        public Vector3 GetLineIntersect(Line3D line)
        {
            var V = new Vector3(line.X2 - line.X1, line.Y2 - line.Y1, line.Z2 - line.Z1);

            //if(Vector3.Cross(N,V).LengthSquared() == 0)
            //{
            //    return new Vector3(float.NaN, float.NaN, float.NaN);
            //}
            
            float Aa = N.X * V.X + N.Y * V.Y + N.Z * V.Z;
            float D = GetD();
            float Ax = N.X * line.X1 + N.Y * line.Y1 + N.Z * line.Z1;

            float t = -(Ax + D) / Aa;

            return new Vector3(line.X1 + V.X * t , line.Y1 + V.Y * t , line.Z1 + V.Z * t);
        }

        float GetD()
        {
            return - N.X * X0 - N.Y * Y0 - N.Z * Z0;
        }

 
    }
}
