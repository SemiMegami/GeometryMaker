using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeDMaker.Geometry;
using ThreeDMaker.Geometry.Dimension2;
using System.Numerics;

namespace GeometryMaker
{
    class StraightRoadMaker : RoadMaker
    {
        public float Length { get; set; }
        public override Mesh3D GetMesh()
        {
            float w = LaneWidth * NumberOfLanes / 2;
            List<Vector2> sectionPoints = new List<Vector2>()
            {
                new Vector2(-w, - Thickness),
                 new Vector2(w, - Thickness),
                  new Vector2(w, 0),
                 new Vector2(-w,0),
             
            };
            Polygon2D polygon = new Polygon2D(sectionPoints);
            List < AxisPoint3D> points = new List<AxisPoint3D> ();
            if(Thetas== null || Thetas.Count < 1)
            {  
                points.Add(new AxisPoint3D() { Position = new Vector3(0, 0, 0), Front = new Vector3(0, 0, 1), Right = new Vector3(1, 0, 0), Up = new Vector3(0, 1, 0) });
                points.Add(new AxisPoint3D() { Position = new Vector3(0, Dy, Length), Front = new Vector3(0, 0, 1), Right = new Vector3(1, 0, 0), Up = new Vector3(0, 1, 0) });
            }
            else 
            {
                int n = Thetas.Count;
                for (int i =0; i < Thetas.Count; i++)
                {
                    float theta = Thetas[i] * MathF.PI / 180;
                    points.Add(new AxisPoint3D() { 
                        Position = new Vector3(0, Dy * i / (n - 1), Length * i / (n - 1)), 
                        Front = new Vector3(0, 0, 1), 
                        Right = new Vector3(1, MathF.Sin(theta),0), 
                        Up = new Vector3(0, 1, 0) });
                }
            }
            Path3D path = new Path3D(points);

            ExtrudePathMesh mesh = new ExtrudePathMesh(polygon, path);
            mesh.FlipFace();
            return mesh;
        }
    }
}
