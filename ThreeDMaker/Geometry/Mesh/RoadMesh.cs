using System;
using System.Collections.Generic;
using System.Numerics;
using ThreeDMaker.Geometry.Dimension2;
using ThreeDMaker.Geometry.Util;
namespace ThreeDMaker.Geometry
{
    public class RoadMesh:ExtrudePathMesh
    {
        public RoadMesh(Shape2D section, List<Vector3> points)
        {
            Path3D path = new Path3D();
            float rotateAngle;
            int n = points.Count;
            int j, k;
            Vector2 v1, v2;
            float widthRatio;
            for (int i = 0; i < n; i++)
            {
                j = i + 1;
                k = i - 1;
                Vector3 front;
                Vector3 right;
                if (k == -1)
                {
                    widthRatio = 1;
                    v1 = Vector2.Zero;
                    v2 = Vector2.Normalize(new Vector2(points[j].X - points[i].X, points[j].Y - points[i].Y));
                  
                }
                else if (j == n)
                {
                    widthRatio = 1;
                    v1 = Vector2.Normalize(new Vector2(points[i].X - points[k].X, points[i].Y - points[k].Y));
                    v2 = Vector2.Zero;
              
                }
                else
                {
                    v1 = Vector2.Normalize(new Vector2(points[i].X - points[k].X, points[i].Y - points[k].Y));
                    v2 = Vector2.Normalize(new Vector2(points[j].X - points[i].X, points[j].Y - points[i].Y));
                    rotateAngle = (float) Math.Acos(Vector2.Dot(v1, v2));
                   
                    widthRatio = 1 / (float) Math.Cos(rotateAngle / 2);
                 

                }
                front = Vector3.Normalize(new Vector3(v1 + v2, 0));
                right = - GeometryUtil.GetRight(front) * widthRatio;
                AxisPoint3D point = new AxisPoint3D()
                {
                    Position = points[i],
                    Front = front,
                    Right = right,
                    Up = Vector3.UnitZ
                };
                path.Add(point);
            }
            Generate(section, path);
        }
    }
}
