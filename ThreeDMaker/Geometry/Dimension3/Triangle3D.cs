using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ThreeDMaker.Geometry.Util;

namespace ThreeDMaker.Geometry.Dimension3
{
    public class Triangle3D
    {
        public Vector3 P1;
        public Vector3 P2;
        public Vector3 P3;

        

        public Triangle3D(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        public Vector3 GetLineIntersect(Line3D line, float tol = 0)
        {
            if(tol == 0)
            {
                tol = GeometryUtil.lengthTol;
            }
            Plane3D plane = new Plane3D(P1, P2, P3);
            Vector3 v = plane.GetLineIntersect(line);
            if (IsPointOnTriangle(v, tol) && line.IsPointOnLine(v.X,v.Y,v.Z, tol))
            {
                return v;
            }
            else
            {
                return new Vector3(float.NaN, float.NaN, float.NaN);
            }
        }

        List<Vector3> GetTriangleIntersectAsPoints(Triangle3D triangle, float tol = 0)
        {
            if (tol == 0)
            {
                tol = GeometryUtil.lengthTol;
            }
            Line3D line12 = new Line3D(triangle.P1, triangle.P2);
            Line3D line23 = new Line3D(triangle.P2, triangle.P3);
            Line3D line31 = new Line3D(triangle.P3, triangle.P1);
            Vector3 p12 = GetLineIntersect(line12, tol);
            Vector3 p23 = GetLineIntersect(line23, tol);
            Vector3 p31 = GetLineIntersect(line31, tol);

            List<Vector3> points = new List<Vector3>();
            if (!float.IsNaN(p12.X))
            {
                points.Add(p12);
            }
            if (!float.IsNaN(p23.X))
            {
                points.Add(p23);
            }
            if (!float.IsNaN(p31.X))
            {
                points.Add(p31);
            }
            if(points.Count > 0)
            {

            }




            return points;
           
        }
        List<Vector3> GetPointList()
        {
            return new List<Vector3>()
            {
                P1,P2,P3
            };
        }
        List<Line3D> GetLineList()
        {
            return new List<Line3D>()
            {
                new Line3D(P1,P2),
                new Line3D(P2,P3),
                new Line3D(P3,P1),
            };
        }
        public Line3D GetTriangleIntersect(Triangle3D triangle, float tol = 0)
        {
            if(tol == 0)
            {
                tol = GeometryUtil.lengthTol;
            }
            List<Vector3> points = GetTriangleIntersectAsPoints(triangle, tol);
            points.AddRange(triangle.GetTriangleIntersectAsPoints(this, tol));
            float maxLength = 0;
            Vector3 p1 = new Vector3();
            Vector3 p2 = new Vector3();
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    float length = (points[i] - points[j]).LengthSquared();
                    if(length > maxLength)
                    {
                        maxLength = length;
                        p1 = points[i];
                        p2 = points[j];
                    }
                }
            }
            if (maxLength > tol)
            {
                var lines = GetLineList();
                for (int j = 0; j < 3; j++)
                {

                    if (lines[j].IsPointOnLine(p1.X, p1.Y, p1.Z, tol) && lines[j].IsPointOnLine(p2.X, p2.Y, p2.Z, tol))
                    {
                        return null;
                    }
                }


                return new Line3D(p1, p2);
            }
            else
            {
                return null;
            }
        }

        public List<Line3D> GetTriangleLineOverlap(Triangle3D triangle3D, float tol = 0)
        {
            if (tol == 0)
            {
                tol = GeometryUtil.lengthTol;
            }
            var n = getN();
            var points1 = GetPointList();
            var points2 = triangle3D.GetPointList();
            var lines1 = GetLineList();
            var lines2 = GetLineList();
            List<Line3D> lines = new List<Line3D>();

            for (int i = 0; i < 3; i++)
            {
                if (Math.Abs(Vector3.Dot(Vector3.Normalize(points1[i] - points2[i]), n)) > tol)
                {
                    return lines;
                }
            }
           


            for (int i1 = 0; i1 < 3; i1++)
            {
               
                int i2 = i1 == 2 ? 0 : (i1 + 1);
                List<Vector3> cornersPoints = new List<Vector3>();
                if (IsPointOnTriangle(points2[i1], tol))
                {
                    cornersPoints.Add(points2[i1]);
                }
                if (IsPointOnTriangle(points2[i2], tol))
                {
                    cornersPoints.Add(points2[i2]);
                }
                if (cornersPoints.Count == 2)
                {
                    lines.Add(lines2[i1]);
                }
                List<Vector3> intersectPoints = new List<Vector3>();
                for (int j1 = 0; j1 < 3; j1++)
                {
                    var p = lines2[i1].GetLineIntersect(lines1[j1]);
                    if (IsPointOnTriangle(p) && triangle3D.IsPointOnTriangle(p))
                    {
                        intersectPoints.Add(p);
                    }
                }

                if (cornersPoints.Count == 1)
                {
                    List<Vector3> sortedPoints = new List<Vector3>();
                    sortedPoints.Add(cornersPoints[0]);
                   

                    for (int j1 = 0; j1 < intersectPoints.Count; j1++)
                    {
                        float dis = Vector3.DistanceSquared(intersectPoints[j1], sortedPoints[0]);
                        bool inserted = false;
                        for (int l = 1; l < sortedPoints.Count; l++)
                        {
                            if (Vector3.DistanceSquared(intersectPoints[l], sortedPoints[0]) > dis)
                            {
                                sortedPoints.Insert(l, intersectPoints[j1]);
                                inserted = true;
                                break;
                            }
                        }
                        if (!inserted)
                        {
                            sortedPoints.Add(intersectPoints[j1]);
                        }
                    }

                    for(int j =0; j < sortedPoints.Count - 1; j++)
                    {
                        if(Vector3.DistanceSquared(sortedPoints[j], sortedPoints[j + 1 ]) > tol * tol)
                        {
                            lines.Add(new Line3D(sortedPoints[j], sortedPoints[j + 1]));
                        }
                    }
                }


                if (cornersPoints.Count == 0 && intersectPoints.Count >= 2)
                {
                    for (int j = 0; j < intersectPoints.Count; j++)
                    {
                        for (int k = j + 1; k < intersectPoints.Count; k++)
                        {
                            if (Vector3.DistanceSquared(intersectPoints[j], intersectPoints[k]) > tol * tol)
                            {
                                lines.Add(new Line3D(intersectPoints[j], intersectPoints[k]));
                            }
                        }              
                    }
                }
            }
            
            for(int i = 0; i < lines.Count; i++)
            {
                var p1 = lines[i].GetP1();
                var p2 = lines[i].GetP2();
                for (int j = 0; j < 3; j++)
                {
                    if(lines1[j].IsPointOnLine(p1.X, p1.Y, p1.Z, tol) && lines1[j].IsPointOnLine(p2.X, p2.Y, p2.Z, tol))
                    {
                        lines.RemoveAt(i);
                        i--;
                    }
                }
            }
            return lines;
    
        }

        public Vector3 getN()
        {
            return Vector3.Cross(P2 - P1, P3 - P1);
        }

        public List<Vector3> GetBound()
        {
            float minx = Math.Min(Math.Min(P1.X, P2.X), P3.X);
            float miny = Math.Min(Math.Min(P1.Y, P2.Y), P3.Y);
            float minz = Math.Min(Math.Min(P1.Z, P2.Z), P3.Z);

            float maxx = Math.Max(Math.Max(P1.X, P2.X), P3.X);
            float maxy = Math.Max(Math.Max(P1.Y, P2.Y), P3.Y);
            float maxz = Math.Max(Math.Max(P1.Z, P2.Z), P3.Z);
            return new List<Vector3>()
            {
                new Vector3(minx,miny,minz),
                new Vector3(maxx,maxy,maxz)
        };
        }

        public bool IsPointOnTriangle(Vector3 p, float tol = 0)
        {
            if(tol == 0)
            {
                tol = GeometryUtil.lengthTol;
            }
            
            var N = getN();
            var A1 = Vector3.Dot(new Triangle3D(p, P2, P3).getN(),N);
            var A2 = Vector3.Dot(new Triangle3D(P1, p, P3).getN(),N);
            var A3 = Vector3.Dot(new Triangle3D(P1, P2, p).getN(),N);
            if(A1 >= -tol && A2 >= -tol && A3 >= -tol)
            {
                return true;
            }      
            return false;
        }

        public float GetX(float y, float z, float tol = 0)
        {
            if(tol == 0)
            {
                tol = GeometryUtil.lengthTol;
            }
            Plane3D plane = new Plane3D(P1, P2, P3);
            Line3D line = new Line3D(Vector3.UnitX, 0, y, z);
            Vector3 v = plane.GetLineIntersect(line);
            if (IsPointOnTriangle(v, tol))
            {
                return v.X;
            }
            else
            {
                return float.NaN;
            }
        }
        public float GetY(float z, float x, float tol = 0)
        {
            if (tol == 0)
            {
                tol = GeometryUtil.lengthTol;
            }
            Plane3D plane = new Plane3D(P1, P2, P3);
            Line3D line = new Line3D(Vector3.UnitY, x, 0, z);
            Vector3 v = plane.GetLineIntersect(line);
            if (IsPointOnTriangle(v, tol))
            {
                return v.Y;
            }
            else
            {
                return float.NaN;
            }
        }
        public float GetZ(float x, float y, float tol = 0)
        {
            if (tol == 0)
            {
                tol = GeometryUtil.lengthTol;
            }
            Plane3D plane = new Plane3D(P1, P2, P3);
            Line3D line = new Line3D(Vector3.UnitZ, x, y, 0);
            Vector3 v = plane.GetLineIntersect(line);
            if (IsPointOnTriangle(v, tol))
            {
                return v.Z;
            }
            else
            {
                return float.NaN;
            }
        }
    }
}
