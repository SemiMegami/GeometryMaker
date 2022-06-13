using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDMaker.Geometry.Util
{
    class GeometryUtil
    {
        public static float ratioTol = 0.000001f; // Unitless
        public static float lengthTol = 0.001f;
        public static float AreaTol => lengthTol * lengthTol;

        public static bool IsonTriangle(Vector2 p, Vector2 p1, Vector2 p2, Vector2 p3, bool includeOnLine = true)
        {
            float A = TriangleArea(p1, p2, p3);
            float A1 = TriangleArea(p, p2, p3) / A;
            float A2 = TriangleArea(p, p3, p1) / A;
            float A3 = TriangleArea(p, p1, p2) / A;
         //   tol = 0;
            if (includeOnLine)
            {
                return (A1 >= -ratioTol && A2 >= -ratioTol && A3 >= -ratioTol);
            }
            else
            {
                return (A1 >= ratioTol && A2 >= ratioTol && A3 >= ratioTol);
            }
        }


        public static float Area(List<Vector2> vertices){
            float a = 0;
            int n = vertices.Count;
            for(int i = 0; i < n; i++)
            {
                int j = i + 1;
                if (j == n)
                {
                    j = 0;
                }
                a += vertices[i].X * vertices[j].Y - vertices[j].X * vertices[i].Y;
            }
            return a * 0.5f;
        }

        public static float TriangleArea(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return 0.5f *( p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y - p1.X * p3.Y - p2.X * p1.Y - p3.X * p2.Y);
        }

        public static float TurnAngle(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var p12 = Vector2.Normalize(p2 - p1);
            var p23 = Vector2.Normalize(p3 - p2);
            var sinAngle = p12.X * p23.Y - p12.Y * p23.X;
            var cosAngle = Vector2.Dot (p12,p23);
            return (float)Math.Atan2(sinAngle, cosAngle);
        }

        public static bool IsTurningLeft(Vector2 p1, Vector2 p2, Vector2 p3, bool OrNotTurn = false)
        {
            var p12 = p2 - p1;
            var p23 = p3 - p2;
            if (OrNotTurn)
            {
                return p12.X * p23.Y - p12.Y * p23.X > - AreaTol;
            }
            else
            {
                return p12.X * p23.Y - p12.Y * p23.X > AreaTol;
            }
        }




        public static bool IsTurningRight(Vector2 p1, Vector2 p2, Vector2 p3, bool OrNotTurn = false)
        {
            var p12 = Vector2.Normalize(p2 - p1);
            var p23 = Vector2.Normalize(p3 - p2);
            if (OrNotTurn)
            {
                return p12.X * p23.Y - p12.Y * p23.X < - AreaTol;
            }
            else
            {
                return p12.X * p23.Y - p12.Y * p23.X < AreaTol;
            }
        }

        public static bool IsLineIntereced(Vector2 line1P1, Vector2 line1P2, Vector2 line2P1, Vector2 line2P2, bool OrTouch = true)
        {
            float A1 = TriangleArea(line1P1, line1P2, line2P1);
            float A2 = TriangleArea(line1P1, line1P2, line2P2);

            float B1 = TriangleArea(line2P1, line2P2, line1P1);
            float B2 = TriangleArea(line2P1, line2P2, line1P2);

            float tol = AreaTol;
            if (OrTouch)
            {
                return (A1 * A2 <= -tol && B1 * B2 <= -tol);
            }
            else
            {
                return (A1 * A2 < tol && B1 * B2 < tol);
            }
        }

        public static Vector2 GetRight(Vector2 v)
        {
            if (v.X == 0 && v.Y == 0)
            {
                return Vector2.Zero;
            }
            else
            {
                return Vector2.Normalize(new Vector2(v.Y,-v.X));
            }
        }

        public static Vector3 GetRight(Vector3 v)
        {
            if(v.X == 0 && v.Y == 0)
            {
                return Vector3.UnitX;
            }
            else
            {
                return Vector3.Normalize(Vector3.Cross(v, Vector3.UnitZ));
            }
        }

        public static List<T> GetInverseList<T>(List<T> items)
        {
            List<T> outputs = new List<T>();

            for(int i = items.Count - 1; i >=0; i--)
            {
                outputs.Add(items[i]);
            }
            return outputs;
        }

        public static List<Vector3> GetCartitianPlaneAxis(Vector3 n)
        {
            float ax = Math.Abs(n.X);
            float ay = Math.Abs(n.Y);
            float az = Math.Abs(n.Z);

            if (ax > ay && ax > az)
            {
                if (n.X > 0)
                {
                    return new List<Vector3>()
                    {
                        Vector3.UnitY,
                        Vector3.UnitZ,
                    };
                }
                else
                {
                    return new List<Vector3>()
                    {
                       - Vector3.UnitY,
                        Vector3.UnitZ,
                    };
                }
            }
            else if (ay > az && ay > ax)
            {
                if (n.Y > 0)
                {
                    return new List<Vector3>()
                    {
                        - Vector3.UnitX,
                        Vector3.UnitZ,
                    };
                }
                else
                {
                    return new List<Vector3>()
                    {
                        Vector3.UnitX,
                        Vector3.UnitZ,
                    };
                }
            }
            else
            {
                if (n.Z > 0)
                {
                    return new List<Vector3>()
                    {
                        Vector3.UnitX,
                        Vector3.UnitY,
                    };
                }
                else
                {
                    return new List<Vector3>()
                    {
                        -Vector3.UnitX,
                        Vector3.UnitY,
                    };
                }
            }
        }
    }
}
