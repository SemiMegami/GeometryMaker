using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ThreeDMaker.Geometry;
using ThreeDMaker.Geometry.Dimension2;

namespace GeometryMaker
{
    class CurveRoadMaker : RoadMaker
    {
        public float TurnRadius { get; set; }
        public float TurnAngle { get; set; }
        public override Mesh3D GetMesh()
        {
            float w = LaneWidth * NumberOfLanes / 2;
            int n;
            List<float> UsedTheta = new List<float>();
            if (Thetas == null || Thetas.Count < 1)
            {
                n = (int)MathF.Floor(TurnAngle / 10 + 0.001f) + 1;
                UsedTheta = new List<float>();
                for(int i =0; i < n; i++)
                {
                    UsedTheta.Add(0);
                }
            }
            else if (Thetas.Count == 1)
            {
                n = (int)MathF.Floor(TurnAngle / 10 + 0.001f) + 1;
                UsedTheta = new List<float>();
                for (int i = 0; i < n; i++)
                {
                    UsedTheta.Add(Thetas[0]);
                }
            }
            else
            {
                n = Thetas.Count;
                for (int i = 0; i < n; i++)
                {
                    UsedTheta.Add(Thetas[i]);
                }
            }
               

            List<Vector2> sectionPoints = new List<Vector2>()
            {
                new Vector2(-w, - Thickness),
                 new Vector2(w, - Thickness),
                  new Vector2(w, 0),
                 new Vector2(-w,0),

            };
            Polygon2D polygon = new Polygon2D(sectionPoints);
            List<AxisPoint3D> points = new List<AxisPoint3D>();

            for (int i = 0; i < UsedTheta.Count; i++)
            {
                float theta = UsedTheta[i] * MathF.PI / 180;
                float turnTheta = TurnAngle * i / (n - 1) * MathF.PI / 180;
                points.Add(new AxisPoint3D()
                {
                    Position = new Vector3(TurnRadius * MathF.Cos(turnTheta) - TurnRadius, Dy * i / (n - 1), TurnRadius * MathF.Sin(turnTheta)),
                    Front = new Vector3(-MathF.Sin(turnTheta), 0, MathF.Cos(turnTheta)),
                    Right = new Vector3(MathF.Cos(turnTheta), MathF.Sin(theta), MathF.Sin(turnTheta)),
                    Up = new Vector3(0,1, 0)
                });
            }

            Path3D path = new Path3D(points);

            ExtrudePathMesh mesh = new ExtrudePathMesh(polygon, path);
            mesh.FlipFace();
            return mesh;
        }
    }
}
