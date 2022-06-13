using System;
using System.Collections.Generic;
using System.Numerics;
using ThreeDMaker.Geometry.Dimension2;
using ThreeDMaker.Geometry.Util;

namespace ThreeDMaker.Geometry
{
    public class ExtrudePathMesh:Mesh3D
    {
        public ExtrudePathMesh()
        {

        }
        public ExtrudePathMesh(Shape2D section, Path3D path, bool includeSection = true)
        {
            Generate(section, path, new List<Shape2D>(), includeSection);
        }

        public ExtrudePathMesh(Shape2D section, Path3D path, List<Shape2D> holes, bool includeSection = true)
        {
            Generate(section, path, holes, includeSection);
        }

        public void Generate(Shape2D section, Path3D path, bool includeSection = true)
        {
            Generate(section, path, new List<Shape2D>(), includeSection);
        }


        public void Generate(Shape2D section, Path3D path, List<Shape2D> holes, bool includeSection = true)
        {
            // check clockwise and counter clock wise

            Vertices = new List<Vector3>();
            UVs = new List<Vector2>();
            Triangles = new List<int>();
            int ni = section.Count;
            int nj = path.Count;
            float u = 0;


            for (int i = 0; i < ni; i++)
            {
                float v = 0;
                Vector3 local1 = new Vector3(section.points[i], 0);
                int k = i + 1;
                if (k == ni)
                {
                    k = 0;
                }          
                Vector3 local2 = new Vector3(section.points[k], 0);
                float du = Vector3.Distance(local2, local1);
                for (int j = 0; j < nj - 1; j++)
                {
                    int index = Vertices.Count;
                    AxisPoint3D axis1 = path[j];
                    AxisPoint3D axis2 = path[j + 1];
                  
                    Vector3 v0 = axis1.GetWorld(local1);
                    Vector3 v1 = axis1.GetWorld(local2);
                    Vector3 v2 = axis2.GetWorld(local1);
                    Vector3 v3 = axis2.GetWorld(local2);

                    float dv = Vector3.Distance(axis1.Position, axis2.Position);
                    Vertices.AddRange(new List<Vector3>() { v0, v1, v2, v3 });
                    Vector2 uv0 = new Vector2(u, v);
                    Vector2 uv1 = new Vector2(u + du, v);
                    Vector2 uv2 = new Vector2(u, v + dv);
                    Vector2 uv3 = new Vector2(u + du, v + dv);
                    UVs.AddRange(new List<Vector2>() { uv0, uv1, uv2, uv3 });
                    Triangles.AddRange(new List<int>() { index, index + 1, index + 2, index + 1, index + 3, index + 2 });
                    v += dv;
                }
                u += du;
            }

           

            foreach (var hole in holes)
            {
                u = 0;
                ni = hole.Count;
         
                for (int i = 0; i < ni; i++)
                {
                    float v = 0;
                    Vector3 local1 = new Vector3(hole.points[i], 0);
                    int k = i + 1;
                    if (k == ni)
                    {
                        k = 0;
                    }
                    Vector3 local2 = new Vector3(hole.points[k], 0);
                    float du = Vector3.Distance(local2, local1);
                    for (int j = 0; j < nj - 1; j++)
                    {
                        int index = Vertices.Count;
                        AxisPoint3D axis1 = path[j];
                        AxisPoint3D axis2 = path[j + 1];
                       
                        Vector3 v0 = axis1.GetWorld(local1);
                        Vector3 v1 = axis1.GetWorld(local2);
                        Vector3 v2 = axis2.GetWorld(local1);
                        Vector3 v3 = axis2.GetWorld(local2);
                        float dv = Vector3.Distance(axis1.Position, axis2.Position);
                        Vertices.AddRange(new List<Vector3>() { v0, v1, v2, v3 });
                        Vector2 uv0 = new Vector2(u, v);
                        Vector2 uv1 = new Vector2(u + du, v);
                        Vector2 uv2 = new Vector2(u, v + dv);
                        Vector2 uv3 = new Vector2(u + du, v + dv);
                        UVs.AddRange(new List<Vector2>() { uv0, uv1, uv2, uv3 });
                        Triangles.AddRange(new List<int>() { index, index + 1, index + 2, index + 1, index + 3, index + 2 });
                        v += dv;
                    }
                    u += du;
                }
            }



            if (includeSection)
            {
                ProfileMesh outlineMesh = new ProfileMesh(section, holes);

                int nv = Vertices.Count;

                AxisPoint3D lastAxis = path[nj - 1];
                foreach (var v in outlineMesh.Vertices)
                {
                    Vertices.Add(lastAxis.GetWorld(v));
                    UVs.Add(new Vector2(v.X, v.Y));
                }
                for (int i = 0; i < outlineMesh.Triangles.Count; i++)
                {
                    Triangles.Add(outlineMesh.Triangles[i] + nv);
                }

                nv = Vertices.Count;
                AxisPoint3D firstAxis = path[0];

                foreach (var v in outlineMesh.Vertices)
                {
                    Vertices.Add(firstAxis.GetWorld(v));
                    UVs.Add(new Vector2(v.X, v.Y));
                }
                for (int i = 0; i < outlineMesh.Triangles.Count; i += 3)
                {
                    Triangles.Add(outlineMesh.Triangles[i] + nv);
                    Triangles.Add(outlineMesh.Triangles[i + 2] + nv);
                    Triangles.Add(outlineMesh.Triangles[i + 1] + nv);
                }
            }
            
        }



    }
}
