using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ThreeDMaker.Geometry.Dimension2;
namespace ThreeDMaker.Geometry
{
    public class Box: ExtrudePathMesh
    {
        public Box(float w, float d, float h, float x = 0, float y = 0, float z = 0)
        {
           

            List<Vector2> points = new List<Vector2>()
            {
                new Vector2(-w / 2, -d / 2),
                new Vector2(w / 2, -d / 2),
                new Vector2(w / 2, d / 2),
                new Vector2(-w / 2, d / 2),
            };
            Polyline2D section = new Polyline2D(points);
            

            List<Vector3> lines = new List<Vector3>()
            {
                new Vector3(x,y,z - h / 2),
                new Vector3(x,y,z + h / 2),
            };

            Path3D axisPoints = new Path3D(lines);

            Generate(section, axisPoints, true);

            FlipFace();
        }

    }
}
