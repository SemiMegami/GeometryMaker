using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ThreeDMaker.Geometry.Dimension2;

namespace ThreeDMaker.Geometry
{
    public class Sphere:ExtrudePathMesh
    {
        int Sections;
        float X;
        float Y;
        float Z;
        float R;
        public Sphere(float r, int sections = 16, float x = 0, float y = 0, float z = 0)
        {
            
            Sections = sections;
            X = x;
            Y = y;
            Z = z;
            R = r;
            UpdateMesh();
        }
        public void UpdateMesh()
        {
            var circle = new Circle2D(1, Sections);
            List<AxisPoint3D> point3Ds = new List<AxisPoint3D>();

            float dAngle = 2 * (float)Math.PI / Sections;


            float startAngle2 = -(float)Math.PI / 2;
            Vector3 right = new Vector3(1, 0, 0);
            Vector3 up = new Vector3(0, 1, 0);
            Vector3 front = new Vector3(0, 0, 1);
            for (int i = 0; i <= Sections / 2; i++)
            {
                float angle = dAngle * i;
                float z = Z + R * (float)Math.Sin(angle + startAngle2);
                float r = R * (float)Math.Cos(angle + startAngle2);
                AxisPoint3D  p= new AxisPoint3D(new Vector3(X, Y, z), front, -right * r, up * r);
                point3Ds.Add(p);
            }
            Path3D path = new Path3D(point3Ds);

            Generate(circle, path);
            CleanBadTriangles();
        }
        
    }
}
