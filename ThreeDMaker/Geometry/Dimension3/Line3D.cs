using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ThreeDMaker.Geometry.Util;

namespace ThreeDMaker.Geometry.Dimension3
{
    public class Line3D
    {
        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float Z1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float Z2 { get; set; }

        float DX => X2 - X1;
        float DY => Y2 - Y1;
        float DZ => Z2 - Z1;


        public Line3D(Vector3 N, float x1, float y1, float z1)
        {
            X1 = x1;
            Y1 = y1;
            Z1 = z1;
            X2 = x1 + N.X;
            Y2 = y1 + N.Y;
            Z2 = z1 + +N.Z;
        }

        public Line3D(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            X1 = x1;
            Y1 = y1;
            Z1 = z1;
            X2 = x2;
            Y2 = y2;
            Z2 = z2;
        }

        public Line3D(Vector3 v1, Vector3 v2)
        {
            X1 = v1.X;
            Y1 = v1.Y;
            Z1 = v1.Z;
            X2 = v2.X;
            Y2 = v2.Y;
            Z2 = v2.Z;
        }
        public float GetPointDistance(float x, float y, float z)
        {
            var v12 = GetNormolizedV();
            var v = new Vector3(x - X1, y - Y1, z - Z1);
            var d = Vector3.Cross(v12, v).Length();
            return d;
        }

        public Vector3 GetLineIntersect(Line3D line, float tol = 0)
        {
            if(tol == 0)
            {
                tol = GeometryUtil.lengthTol;
            }
            var p1 = GetP1();
            var p2 = line.GetP1();
            var a1 = GetV();
            var a2 = line.GetV();
            float A11 = Vector3.Dot(a1, a1);
            float A12 = -Vector3.Dot(a1, a2);
            float A22 = Vector3.Dot(a2, a2);
            float B1 = Vector3.Dot(p2 - p1, a1);
            float B2 = Vector3.Dot(p1 - p2, a2);
            float d = A11 * A22 - A12 * A12;
            float t = (B1 * A22 - B2 * A12) / d;
            float s = (A11 * B2 - A12 * B1) / d;
            if(float.IsNaN(t)|| float.IsNaN(s))
            {
                return new Vector3(float.NaN, float.NaN, float.NaN);
            }
            var intersect1 = p1 + t * a1;
            var intersect2 = p2 + s * a2;
            if(Vector3.DistanceSquared(intersect1,intersect2) <= tol * tol)
            {
                return intersect1;
            }
            return new Vector3(float.NaN, float.NaN, float.NaN);
        }
        public Vector3 GetNormolizedV()
        {
            return Vector3.Normalize(new Vector3(DX, DY, DZ));
        }
        public Vector3 GetV()
        {
            return new Vector3(DX, DY, DZ);
        }

        public Vector3 GetP1()
        {
            return new Vector3(X1, Y1, Z1);
        }

        public Vector3 GetP2()
        {
            return new Vector3(X2, Y2, Z2);
        }
        public bool IsPointOnLine(float x, float y, float z, float tol = 0)
        {
            if(tol == 0)
            {
                tol = GeometryUtil.lengthTol;
            }
            

            var d = GetPointDistance(x, y, z);
            if (d <= tol)
            {
                var v1 = new Vector3(x - X1, y - Y1, z - Z1);
                var v2 = new Vector3(x - X2, y - Y2, z - Z2);
                if (Vector3.Dot(v1, v2) < tol * tol || v1.LengthSquared() < tol * tol || v2.LengthSquared() < tol * tol)
                {
                    return true;
                }

            }
            return false;
        }

        public override string ToString()
        {
            return "(" + X1 + ", " + Y1 + ", " + Z1 + "), (" + X2 + ", " + Y2 + ", " + Z2 + ")";
        }
    }
}
