using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ThreeDMaker.Geometry.Util;
namespace ThreeDMaker.Geometry.Dimension2
{
    public class Line2D:Shape2D
    {

        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }

        float DX => X2 - X1;
        float DY => Y2 - Y1;

        public Line2D(float x1, float y1, float x2, float y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
           
            points = new List<Vector2>();
            Add(X1, X2);
            Add(Y1, Y2);
         //   UpdatePoints();
        }

        public Line2D(Vector2 v1, Vector2 v2)
        {
            X1 = v1.X;
            Y1 = v1.Y;
            X2 = v2.X;
            Y2 = v2.Y;
            points = new List<Vector2>();
            Add(X1, X2);
            Add(Y1, Y2);
        //    UpdatePoints();
        }

        // offsetLine
        public float GetY(float x)
        {
            return DY / DX * (x - X1) + Y1;
        }
        public float GetX(float y)
        {
            return DX / DY * (y - Y1) + X1;
        }
        public Vector2 GetV()
        {
            return new  Vector2(DX, DY);
        }
        public Vector2 GetNormolizedV()
        {
            return Vector2.Normalize(GetV());
        }

        public Vector2 GetP1()
        {
            return new Vector2(X1, Y1);
        }

        public Vector2 GetP2()
        {
            return new Vector2(X2, Y2);
        }

        public float GetLenght()
        {
            return GetV().Length();
        }

        public float GetLenghtSq()
        {
            return GetV().LengthSquared();
        }
        public override Shape2D GetOffSet(float d)
        {
            Vector2 v = GetV();
            Vector2 n = d * GeometryUtil.GetRight(v);
            return new Line2D(X1 + n.X, Y1 + n.Y, X2 + n.X, Y2 + n.Y);
        }
        public bool IsLineInterSec(Line2D l)
        {
            var v = GetLineInterSec(l);
            return !float.IsNaN(v.X);
        }

        public bool IsLineCross(Line2D l)
        {
            var v = GetLineInterSec(l);
            if (float.IsNaN(v.X)) return false;

            if(l.IsPointOnline(X1,Y1) || l.IsPointOnline(X2, Y2) || IsPointOnline(l.X1, l.Y1) || IsPointOnline(l.X2, l.Y2))
            {
                return false;
            }
            return true;
        }
        public Vector2 GetLineInterSec(Line2D l, bool noCheckIntersec = false)
        {
            //https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection
            float x3 = l.X1;
            float y3 = l.Y1;
            float x4 = l.X2;
            float y4 = l.Y2;

            float x12 = X1 - X2;
            float y12 = Y1 - Y2;
            float x34 = x3 - x4;
            float y34 = y3 - y4;
            float D = x12 * y34 - y12 * x34;

            if (Math.Abs(D) < GeometryUtil.AreaTol)
            {
                return new Vector2 (float.NaN, float.NaN);
            }
            float xy12 = X1 * Y2 - Y1 * X2;
            float xy34 = x3 * y4 - y3 * x4;
            float x = (xy12 * x34 - x12 * xy34) / D;
            float y = (xy12 * y34 - y12 * xy34) / D;
            if (noCheckIntersec)
            {
                return new Vector2(x, y);
            }
            if (IsPointOnline(x,y) && l.IsPointOnline(x,y))
            {
                return new Vector2(x, y);
            }
            return new Vector2(float.NaN, float.NaN);
        }
        public float GetPointDistance(float x, float y, bool absolute = true)
        {
            var v12 = new Vector3(GetNormolizedV(),0);
            var v = new Vector3(x - X1, y - Y1,0);
            var d = Vector3.Cross(v12,v).Z;
            if (absolute)
            {
                return Math.Abs(d);
            }
            else
            {
                return d;
            }
        }

       
        public bool IsPointOnline(float x, float y)
        {
            var tol =GeometryUtil.lengthTol;

            var d = GetPointDistance(x, y);
            if(d <= tol)
            {
                var v1 = new Vector2(x - X1, y - Y1);
                var v2 = new Vector2(x - X2, y - Y2);
                if(Vector2.Dot(v1,v2) < tol * tol || v1.LengthSquared() < tol * tol || v2.LengthSquared() < tol * tol)
                {
                    return true;
                }
                
            }
            return false;
        }

        public bool IsPointSplitline(float x, float y)
        {
            var tol = GeometryUtil.lengthTol;

            var d = GetPointDistance(x, y);
            if (d <= tol)
            {
                var v1 = new Vector2(x - X1, y - Y1);
                var v2 = new Vector2(x - X2, y - Y2);
                
                if (Vector2.Dot(v1, v2) < tol * tol && !(v1.LengthSquared() < tol * tol || v2.LengthSquared() < tol * tol))
                {
                    return true;
                }

            }
            return false;
        }

        public override void UpdatePoints()
        {
            points[0] = new Vector2(X1, X2);
            points[1] = new Vector2(Y1, Y2);
        }

        public override string ToString()
        {
            return "(" + X1 + ", " + Y1 + "), (" + X2 + ", " + Y2 + ")";
        }
    }
}
