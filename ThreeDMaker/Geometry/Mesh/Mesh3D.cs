using System;
using System.Collections.Generic;
using System.Numerics;

using System.IO;
using ThreeDMaker.Geometry.Dimension3;
using System.Linq;
using ThreeDMaker.Geometry.Util;
using ThreeDMaker.Geometry.Dimension2;



namespace ThreeDMaker.Geometry
{

    public partial class Mesh3D
    {
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<Vector2> UVs { get; set; }
        public List<int> Triangles { get; set; }
        public List<int> Lines { get; set; }
        public List<int> FirstIndice { get; set; }

        public Mesh3D()
        {
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            Triangles = new List<int>();
            UVs = new List<Vector2>();
            FirstIndice = new List<int>();
            Lines = new List<int>();
        }

        public Mesh3D(Mesh3D mesh)
        {
            Vertices = new List<Vector3>(mesh.Vertices);
            Normals = new List<Vector3>(mesh.Normals);
            Triangles = new List<int>(mesh.Triangles);
            UVs = new List<Vector2>(mesh.UVs);
            FirstIndice = new List<int>(mesh.FirstIndice);
            Lines = new List<int>(mesh.Lines);
        }

        public Mesh3D(List<Mesh3D> meshs) : this()
        {

            int nv = 0;
            foreach (var mesh in meshs)
            {
                nv = Vertices.Count;
                Vertices.AddRange(mesh.Vertices);
                if (mesh.UVs.Count == mesh.Vertices.Count)
                {
                    UVs.AddRange(mesh.UVs);
                }
                else
                {
                    for (int i = 0; i < mesh.Vertices.Count; i++)
                    {
                        UVs.Add(new Vector2(mesh.Vertices[i].X, mesh.Vertices[i].Y));
                    }
                }
                foreach (var i in mesh.Triangles)
                {
                    Triangles.Add(nv + i);
                }
            }
        }


        public void CleanBadTriangles()
        {
            for (int i = 0; i < Triangles.Count; i += 3)
            {
                var p1 = Vertices[Triangles[i]];
                var p2 = Vertices[Triangles[i + 1]];
                var p3 = Vertices[Triangles[i + 2]];

                if (Vector3.Cross(p3 - p1, p2 - p1).LengthSquared() < 0.0000001)
                {
                    Triangles.RemoveAt(i + 2);
                    Triangles.RemoveAt(i + 1);
                    Triangles.RemoveAt(i);
                    i -= 3;
                }
            }
        }

        public void ReCalculateNormal()
        {
            Normals.Clear();

            int n = Vertices.Count;

            for (int i = 0; i < n; i++)
            {
                Normals.Add(new Vector3());
            }

            var triangleNormals = FaceNormals();

            for (int i = 0; i < triangleNormals.Count * 3; i += 3)
            {
                var fNormal = triangleNormals[i / 3];
                Normals[Triangles[i]] += fNormal;
                Normals[Triangles[i + 1]] += fNormal;
                Normals[Triangles[i + 2]] += fNormal;
            }


            for (int i = 0; i < n; i++)
            {
                Normals[i] = Vector3.Normalize(Normals[i]);
            }

        }

        public List<float> FaceAreas()
        {
            List<float> areas = new List<float>();
            int n = Triangles.Count;

            for (int i = 0; i < n; i += 3)
            {
                var v1 = Vertices[Triangles[i]];
                var v2 = Vertices[Triangles[i + 1]];
                var v3 = Vertices[Triangles[i + 2]];
                areas.Add(Vector3.Cross(v2 - v1, v3 - v1).Length() * 0.5f);
            }
            return areas;
        }

        public List<Vector3> FaceNormals()
        {
            List<Vector3> normals = new List<Vector3>();
            int n = Triangles.Count;
            for (int i = 0; i < n; i += 3)
            {
                var v1 = Vertices[Triangles[i]];
                var v2 = Vertices[Triangles[i + 1]];
                var v3 = Vertices[Triangles[i + 2]];
                normals.Add(Vector3.Cross(v2 - v1, v3 - v1));
            }
            return normals;
        }

        public List<Vector3> NormalizedFaceNormals()
        {
            List<Vector3> normals = new List<Vector3>();
            int n = Triangles.Count;
            for (int i = 0; i < n; i += 3)
            {
                var v1 = Vertices[Triangles[i]];
                var v2 = Vertices[Triangles[i + 1]];
                var v3 = Vertices[Triangles[i + 2]];
                normals.Add(Vector3.Normalize(Vector3.Cross(v2 - v1, v3 - v1)));
            }
            return normals;
        }


        public void FlipFace()
        {
            int temp;
            for (int i = 0; i < Triangles.Count; i += 3)
            {
                temp = Triangles[i];
                Triangles[i] = Triangles[i + 1];
                Triangles[i + 1] = temp;
            }
            ReCalculateNormal();
        }


        public void ExportToObj(string path, bool swapxyz = false)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {

                if (swapxyz)
                {
                    foreach (var v in Vertices)
                    {
                        writer.Write("v " + v.X + " " + (v.Z) + " " + (-v.Y) + "\n");
                    }

                    foreach (var v in Normals)
                    {
                        writer.Write("vn " + v.X + " " + (v.Z) + " " + (-v.Y) + "\n");
                    }

                    foreach (var v in UVs)
                    {
                        writer.Write("vt " + v.X + " " + v.Y + "\n");
                    }

                    int n = Triangles.Count;

                    for (int i = 0; i < n; i += 3)
                    {
                        int f1 = Triangles[i] + 1;
                        int f2 = Triangles[i + 1] + 1;
                        int f3 = Triangles[i + 2] + 1;
                        writer.Write("f " + f1 + "/" + f1 + " " + f2 + "/" + f2 + " " + f3 + "/" + f3 + "\n");
                    }
                }
                else
                {
                    foreach (var v in Vertices)
                    {
                        writer.Write("v " + v.X + " " + v.Y + " " + v.Z + "\n");
                    }

                    foreach (var v in Normals)
                    {
                        writer.Write("vn " + v.X + " " + v.Y + " " + v.Z + "\n");
                    }

                    foreach (var v in UVs)
                    {
                        writer.Write("vt " + v.X + " " + v.Y + "\n");
                    }

                    int n = Triangles.Count;

                    for (int i = 0; i < n; i += 3)
                    {
                        int f1 = Triangles[i] + 1;
                        int f2 = Triangles[i + 1] + 1;
                        int f3 = Triangles[i + 2] + 1;
                        writer.Write("f " + f1 + "/" + f1 + " " + f3 + "/" + f3 + " " + f2 + "/" + f2 + "\n");
                    }
                }

            }
        }



        public void AddMesh(Mesh3D mesh)
        {
            var nv = Vertices.Count;
            Vertices.AddRange(mesh.Vertices);
            if (mesh.UVs.Count == mesh.Vertices.Count)
            {
                UVs.AddRange(mesh.UVs);
            }
            else
            {
                for (int i = 0; i < mesh.Vertices.Count; i++)
                {
                    UVs.Add(new Vector2(mesh.Vertices[i].X, mesh.Vertices[i].Y));
                }
            }
            foreach (var i in mesh.Triangles)
            {
                Triangles.Add(nv + i);
            }
        }



        public Mesh3D GetUnion(Mesh3D mesh)
        {
            Mesh3D mesh1 = TrimMesh(mesh, true);
            Mesh3D mesh2 = mesh.TrimMesh(this, true);
            mesh1.AddMesh(mesh2);
            return mesh1;
        }
        public Mesh3D GetSubstract(Mesh3D mesh)
        {
            Mesh3D mesh1 = TrimMesh(mesh, true,false);
            Mesh3D mesh2 = mesh.TrimMesh(this, false);
            mesh2.FlipFace();
            mesh1.AddMesh(mesh2);
            return mesh1;
        }
        public Mesh3D GetIntersection(Mesh3D mesh)
        {
            Mesh3D mesh1 = TrimMesh(mesh, false,false);
            Mesh3D mesh2 = mesh.TrimMesh(this, false,false);
            mesh1.AddMesh(mesh2);
            return mesh1;
        }

        public Mesh3D TrimMesh(Mesh3D mesh, bool SelectOutside, bool isSamePlaneSameDirOutside = true)
        {
            Mesh3D merged = new Mesh3D(this);
            var tol = GeometryUtil.lengthTol;
            List<Triangle3D> ATriangles = new List<Triangle3D>();
            List<Triangle3D> BTriangles = new List<Triangle3D>();
            List<Vector3> BNormals = new List<Vector3>();
            List<int> removeList = new List<int>();
            for (int i = 0; i < Triangles.Count; i += 3)
            {
                ATriangles.Add(new Triangle3D(
                    Vertices[Triangles[i]],
                    Vertices[Triangles[i + 1]],
                    Vertices[Triangles[i + 2]]
                    ));
            }
            for (int i = 0; i < mesh.Triangles.Count; i += 3)
            {
                var Btriangle = new Triangle3D(
                    mesh.Vertices[mesh.Triangles[i]],
                    mesh.Vertices[mesh.Triangles[i + 1]],
                    mesh.Vertices[mesh.Triangles[i + 2]]
                    );
                BTriangles.Add(Btriangle);
                BNormals.Add(Btriangle.getN());
            }

            for (int i = 0; i < ATriangles.Count; i++)
            {
                List<Line3D> intersectLines = new List<Line3D>();

                for (int j = 0; j < BTriangles.Count; j++)
                {
                    var line = ATriangles[i].GetTriangleIntersect(BTriangles[j], tol / 100);
                    if (line != null)
                    {
                        intersectLines.Add(line);
                    }
                    var line2s = ATriangles[i].GetTriangleLineOverlap(BTriangles[j], tol);
                    if (line2s != null)
                    {
                        intersectLines.AddRange(line2s);
                    }
                }

                if (intersectLines.Count > 0)
                {
                    // merce intersectlines

                    for(int j = 0; j < intersectLines.Count; j++)
                    {
                        var pj1 = intersectLines[j].GetP1();
                        var pj2 = intersectLines[j].GetP2();
                        for (int k = j + 1; k < intersectLines.Count; k++)
                        {

                            var pk1 = intersectLines[k].GetP1();
                            var pk2 = intersectLines[k].GetP2();

                            if (Vector3.DistanceSquared(pk1, pj1) <= tol * tol)
                            {
                                intersectLines[k].X1 = intersectLines[j].X1;
                                intersectLines[k].Y1 = intersectLines[j].Y1;
                                intersectLines[k].Z1 = intersectLines[j].Z1;
                            }
                            if (Vector3.DistanceSquared(pk1, pj2) <= tol * tol)
                            {
                                intersectLines[k].X1 = intersectLines[j].X2;
                                intersectLines[k].Y1 = intersectLines[j].Y2;
                                intersectLines[k].Z1 = intersectLines[j].Z2;
                            }
                            if (Vector3.DistanceSquared(pk2, pj1) <= tol * tol)
                            {
                                intersectLines[k].X2 = intersectLines[j].X1;
                                intersectLines[k].Y2 = intersectLines[j].Y1;
                                intersectLines[k].Z2 = intersectLines[j].Z1;
                            }
                            if (Vector3.DistanceSquared(pk2, pj2) <= tol * tol)
                            {
                                intersectLines[k].X2 = intersectLines[j].X2;
                                intersectLines[k].Y2 = intersectLines[j].Y2;
                                intersectLines[k].Z2 = intersectLines[j].Z2;
                            }
                        }
                    }

                    for (int j = 0; j < intersectLines.Count; j++)
                    {
                        var pj1 = intersectLines[j].GetP1();
                        var pj2 = intersectLines[j].GetP2();
                        if(Vector3.DistanceSquared(pj1, pj2) <= tol * tol)
                        {
                            intersectLines.RemoveAt(j);
                            j--;
                        }
                    }
                    var triangle = ATriangles[i];
                    Vector3 n = triangle.getN();
                    List<List<Vector3>> trianglePointGroups = new List<List<Vector3>>()
                    {
                        new List<Vector3>(){ triangle.P1 },
                        new List<Vector3>(){ triangle.P2 },
                        new List<Vector3>(){ triangle.P3 },
                    };
                    List<Vector3> trianglePoints = new List<Vector3>();


                    List<Line3D> triangleMainlines = new List<Line3D>()
                    {
                        new Line3D(triangle.P1,triangle.P2),
                        new Line3D(triangle.P2,triangle.P3),
                        new Line3D(triangle.P3,triangle.P1)
                    };

                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < intersectLines.Count; k++)
                        {
                            Vector3 p = new Vector3(float.NaN);
                            if (triangleMainlines[j].IsPointOnLine(intersectLines[k].X1, intersectLines[k].Y1, intersectLines[k].Z1, tol))
                            {
                                p = new Vector3(intersectLines[k].X1, intersectLines[k].Y1, intersectLines[k].Z1);
                            }
                            else if (triangleMainlines[j].IsPointOnLine(intersectLines[k].X2, intersectLines[k].Y2, intersectLines[k].Z2, tol))
                            {
                                p = new Vector3(intersectLines[k].X2, intersectLines[k].Y2, intersectLines[k].Z2);
                            }
                            if (!float.IsNaN(p.X))
                            {
                                float dis = Vector3.DistanceSquared(p, trianglePointGroups[j][0]);
                                bool inserted = false;
                                for (int l = 1; l < trianglePointGroups[j].Count; l++)
                                {
                                    if (Vector3.DistanceSquared(trianglePointGroups[j][l], trianglePointGroups[j][0]) > dis)
                                    {
                                        trianglePointGroups[j].Insert(l, p);
                                        inserted = true;
                                        break;
                                    }
                                }
                                if (!inserted)
                                {
                                    trianglePointGroups[j].Add(p);
                                }
                            }
                        }

                        trianglePoints.AddRange(trianglePointGroups[j]);
                    }


                    for (int j = 0; j < trianglePoints.Count; j++)
                    {
                        int k = j - 1;
                        if (k < 0) k += trianglePoints.Count;

                        if (Vector3.DistanceSquared(trianglePoints[j], trianglePoints[k]) <= tol * tol)
                        {
                            trianglePoints.RemoveAt(j);
                            j--;
                        }
                    }


                    List<Line3D> triangleLines = new List<Line3D>();
                    List<Vector3> triangleVecs = new List<Vector3>();

                    for (int j = 0; j < trianglePoints.Count; j++)
                    {
                        int k = j + 1;
                        if (k == trianglePoints.Count) k = 0;

                        Vector3 v1 = trianglePoints[j];
                        Vector3 v2 = trianglePoints[k];
                        triangleLines.Add(new Line3D(v1, v2));
                        triangleVecs.Add(new Vector3(v2.X - v1.X, v2.Y - v1.Y, v2.Z - v1.Z));
                    }

                    List<Line3D> startLines = new List<Line3D>();

                    for (int j = 0; j < intersectLines.Count; j++)
                    {
                        for (int k = 0; k < triangleLines.Count; k++)
                        {

                            if (triangleLines[k].IsPointOnLine(intersectLines[j].X1, intersectLines[j].Y1, intersectLines[j].Z1, tol))
                            {
                                var lineVec = new Vector3(intersectLines[j].X2 - intersectLines[j].X1, intersectLines[j].Y2 - intersectLines[j].Y1, intersectLines[j].Z2 - intersectLines[j].Z1);
                                if (Vector3.Dot(Vector3.Cross(triangleVecs[k], lineVec), n) >= 0 || true)
                                {
                                    startLines.Add(new Line3D(
                                        new Vector3(intersectLines[j].X1, intersectLines[j].Y1, intersectLines[j].Z1),
                                        new Vector3(intersectLines[j].X2, intersectLines[j].Y2, intersectLines[j].Z2)
                                        ));
                                }
                            }
                            if (triangleLines[k].IsPointOnLine(intersectLines[j].X2, intersectLines[j].Y2, intersectLines[j].Z2, tol))
                            {
                                var lineVec = new Vector3(intersectLines[j].X1 - intersectLines[j].X2, intersectLines[j].Y1 - intersectLines[j].Y2, intersectLines[j].Z1 - intersectLines[j].Z2);
                                if (Vector3.Dot(Vector3.Cross(triangleVecs[k], lineVec), n) >= 0 || true)
                                {
                                    startLines.Add(new Line3D(
                                       new Vector3(intersectLines[j].X2, intersectLines[j].Y2, intersectLines[j].Z2),
                                       new Vector3(intersectLines[j].X1, intersectLines[j].Y1, intersectLines[j].Z1)
                                    ));
                                }
                            }
                        }
                    }

                    if (startLines.Count == 0)
                    {
                        startLines.Add(triangleLines[0]);
                    }

                    if (startLines.Count > 0)
                    {
                        // remove duplicated line

                        for (int j = 0; j < startLines.Count; j++)
                        {
                            var vj1 = new Vector3(startLines[j].X1, startLines[j].Y1, startLines[j].Z1);
                            var vj2 = new Vector3(startLines[j].X2, startLines[j].Y2, startLines[j].Z2);
                            for (int k = j + 1; k < startLines.Count; k++)
                            {
                                var vk1 = new Vector3(startLines[k].X1, startLines[k].Y1, startLines[k].Z1);
                                var vk2 = new Vector3(startLines[k].X2, startLines[k].Y2, startLines[k].Z2);
                                if ((vj1 - vk1).LengthSquared() <= tol * tol && (vj2 - vk2).LengthSquared() <= tol * tol)
                                {
                                    startLines.RemoveAt(k);
                                    k--;
                                }
                            }
                        }
                    }


                    List<Line3D> lines = new List<Line3D>();
                    lines.AddRange(intersectLines);
                    lines.AddRange(triangleLines);
                    // merge llines
                    // make 2d
                    var planeAxis = GeometryUtil.GetCartitianPlaneAxis(n);
                    List<Line2D> line2Ds = new List<Line2D>();
                    List<Line2D> startLine2Ds = new List<Line2D>();

                    foreach (var line in lines)
                    {
                        float x1 = line.X1 * planeAxis[0].X + line.Y1 * planeAxis[0].Y + line.Z1 * planeAxis[0].Z;
                        float y1 = line.X1 * planeAxis[1].X + line.Y1 * planeAxis[1].Y + line.Z1 * planeAxis[1].Z;
                        float x2 = line.X2 * planeAxis[0].X + line.Y2 * planeAxis[0].Y + line.Z2 * planeAxis[0].Z;
                        float y2 = line.X2 * planeAxis[1].X + line.Y2 * planeAxis[1].Y + line.Z2 * planeAxis[1].Z;
                        line2Ds.Add(new Line2D(x1, y1, x2, y2));
                    }
                    foreach (var line in startLines)
                    {
                        float x1 = line.X1 * planeAxis[0].X + line.Y1 * planeAxis[0].Y + line.Z1 * planeAxis[0].Z;
                        float y1 = line.X1 * planeAxis[1].X + line.Y1 * planeAxis[1].Y + line.Z1 * planeAxis[1].Z;
                        float x2 = line.X2 * planeAxis[0].X + line.Y2 * planeAxis[0].Y + line.Z2 * planeAxis[0].Z;
                        float y2 = line.X2 * planeAxis[1].X + line.Y2 * planeAxis[1].Y + line.Z2 * planeAxis[1].Z;
                        startLine2Ds.Add(new Line2D(x1, y1, x2, y2));
                    }

                    // Area
                    List<bool> lineUseds = new List<bool>();
                    foreach (var line in line2Ds)
                    {
                        lineUseds.Add(false);
                    }
                    List<List<Vector2>> point2DGroups = new List<List<Vector2>>();
                    List<List<int>> indexGroups = new List<List<int>>();
                    List<List<bool>> isX1Groups = new List<List<bool>>();
                    foreach (var start in startLine2Ds)
                    {

                        List<Vector2> points2Ds = new List<Vector2>();
                        point2DGroups.Add(points2Ds);
                        points2Ds.Add(new Vector2(start.X1, start.Y1));

                        List<int> index2Ds = new List<int>();
                        indexGroups.Add(index2Ds);
                        List<bool> isX1s = new List<bool>();
                        isX1Groups.Add(isX1s);

                        Vector2 currentPoint = new Vector2(start.X2, start.Y2);
                        Vector2 previousPoint = new Vector2(start.X1, start.Y1);
                        Vector2 nextPoint = new Vector2(start.X1, start.Y1);
                        Vector2 point0 = new Vector2(start.X1, start.Y1);

                        for (int j = 0; j < line2Ds.Count; j++)
                        {
                            var p1 = new Vector2(line2Ds[j].X1, line2Ds[j].Y1);
                            var p2 = new Vector2(line2Ds[j].X2, line2Ds[j].Y2);
                            if ((p1 - previousPoint).LengthSquared() <= tol * tol)
                            {
                                if ((p2 - currentPoint).LengthSquared() <= tol * tol)
                                {
                                    index2Ds.Add(j);
                                    isX1s.Add(true);
                                    lineUseds[j] = true;
                                }
                            }
                            else if ((p2 - previousPoint).LengthSquared() <= tol * tol)
                            {
                                if ((p1 - currentPoint).LengthSquared() <= tol * tol)
                                {
                                    index2Ds.Add(j);
                                    isX1s.Add(false);
                                    lineUseds[j] = true;
                                }
                            }
                        }
                        // find loop
                        while (Vector2.DistanceSquared(currentPoint, point0) >= tol * tol)
                        {

                            var v01 = Vector2.Normalize(new Vector2(currentPoint.X - previousPoint.X, currentPoint.Y - previousPoint.Y));
                            float maxAngle = -10;
                            int targetj = -1;
                            bool isX1 = false;
                            for (int j = 0; j < line2Ds.Count; j++)
                            {
                                var p1 = new Vector2(line2Ds[j].X1, line2Ds[j].Y1);
                                var p2 = new Vector2(line2Ds[j].X2, line2Ds[j].Y2);
                                // compare size
                                float dist1 = new Vector2(line2Ds[j].X1 - currentPoint.X, line2Ds[j].Y1 - currentPoint.Y).LengthSquared();
                                float dist2 = new Vector2(line2Ds[j].X2 - currentPoint.X, line2Ds[j].Y2 - currentPoint.Y).LengthSquared();
                               
                                if (dist1 <= tol * tol && dist2 != 0)
                                {
                                    //console.WriteLine("P1 : " + j);
                                    var v12 = Vector2.Normalize(new Vector2(line2Ds[j].X2 - currentPoint.X, line2Ds[j].Y2 - currentPoint.Y));
                                    float angle = (float) Math.Acos(Vector2.Dot(v01, v12)) * Math.Sign(v01.X * v12.Y - v01.Y * v12.X);
                                    if (float.IsNaN(angle))
                                    {
                                        float dot = Vector2.Dot(v01, v12);
                                        if(dot > 0)
                                        {
                                            angle = 0;
                                        }
                                        else
                                        {
                                            angle = -(float) Math.PI;
                                        }
                                        //console.WriteLine("Error " + Vector2.Dot(v01, v12));
                                       
                                    }
                                    if (angle > maxAngle)
                                    {
                                        if ((p2 - previousPoint).LengthSquared() <= tol * tol)
                                        {
                                            continue;
                                        }

                                        if (lineUseds[j])
                                        {
                                            bool foundDub = false;
                                            for (int k = 1; k < index2Ds.Count; k++)
                                            {
                                                if (index2Ds[k] == j)
                                                {
                                                    foundDub = true;
                                                    break;
                                                }
                                            }
                                            if (foundDub)
                                            {
                                                continue;
                                            }
                                        }

                                        maxAngle = angle;
                                        nextPoint = new Vector2(line2Ds[j].X2, line2Ds[j].Y2);
                                        isX1 = true;
                                        targetj = j;
                                        //console.WriteLine("    angle : " + angle * 180 / (Math.PI) + " updated");
                                    }
                                    else
                                    {
                                        //console.WriteLine("    angle : " + angle * 180 / (Math.PI) + " NOT updated");
                                    }
                                }


                                if (dist2 <= tol * tol && dist1 != 0)
                                {
                                    var v12 = Vector2.Normalize(new Vector2(line2Ds[j].X1 - currentPoint.X, line2Ds[j].Y1 - currentPoint.Y));
                                    var angle = (float)Math.Acos(Vector2.Dot(v01, v12)) * Math.Sign(v01.X * v12.Y - v01.Y * v12.X);
                                    if (float.IsNaN(angle))
                                    {
                                        float dot = Vector2.Dot(v01, v12);
                                        if (dot > 0)
                                        {
                                            angle = 0;
                                        }
                                        else
                                        {
                                            angle = -(float) Math.PI;
                                        }
                                        //console.WriteLine("Error " + Vector2.Dot(v01, v12));
                                    }
                                    if (angle > maxAngle)
                                    {
                                        //console.WriteLine("P2 : " + j);
                                        if ((p1 - previousPoint).LengthSquared() <= tol * tol)
                                        {
                                            continue;
                                        }
                                        if (lineUseds[j])
                                        {
                                            bool foundDub = false;
                                            for (int k = 1; k < index2Ds.Count; k++)
                                            {
                                                if (index2Ds[k] == j)
                                                {
                                                    foundDub = true;
                                                    break;
                                                }
                                            }
                                            if (foundDub)
                                            {
                                                continue;
                                            }
                                        }
                                        maxAngle = angle;
                                        nextPoint = new Vector2(line2Ds[j].X1, line2Ds[j].Y1);
                                        isX1 = false;
                                        targetj = j;
                                        //console.WriteLine("    angle : " + angle * 180 / (Math.PI) + " updated");
                                    }
                                    else
                                    {
                                        //console.WriteLine("    angle : " + angle * 180 / (Math.PI) + " NOT updated");
                                    }
                                }


                            }
                            if (targetj > -1)
                            {
                                //console.WriteLine("target :" + targetj);
                                lineUseds[targetj] = true;
                                previousPoint = new Vector2(currentPoint.X, currentPoint.Y);
                                points2Ds.Add(new Vector2(currentPoint.X, currentPoint.Y));
                                index2Ds.Add(targetj);
                                isX1s.Add(isX1);
                                currentPoint = new Vector2(nextPoint.X, nextPoint.Y);
                                tol = GeometryUtil.lengthTol;

                               
                            }
                            else
                            {
         
                                List<Mesh3D> emeshes = new List<Mesh3D>();
                                for (int j = 0; j < line2Ds.Count; j++)
                                {
                                  
                                    Circle2D circle = new Circle2D(0.01f);
                                    List<Vector3> epoints = new List<Vector3>()
                                    {
                                        new Vector3(line2Ds[j].X1,line2Ds[j].Y1,0),
                                        new Vector3(line2Ds[j].X2,line2Ds[j].Y2,0)
                                    };
                                    Path3D p = new Path3D(epoints);
                                    var em = new ExtrudePathMesh(circle, p);
                                    em.FlipFace();
                                    emeshes.Add(em);
                                }
                                new Mesh3D(emeshes).ExportToObj("../../../../../" + "error" + ".obj");
                                tol += GeometryUtil.lengthTol / 10;


                                  break;
                            }
                            if (points2Ds.Count > line2Ds.Count)
                            {

                            }
                        }

                        tol = GeometryUtil.lengthTol;
                    }
                    //if (needRerun)
                    //{
                    //    i--;
                    //    continue;
                    //}
                    removeList.Add(3 * i);
                    removeList.Add(3 * i + 1);
                    removeList.Add(3 * i + 2);

                    //check hole
                    bool findHole = true;
                    List<List<Vector2>> holePoint2DGroups = new List<List<Vector2>>();
                    List<List<int>> holeIndexGroups = new List<List<int>>();
                    List<List<bool>> holeIsX1Groups = new List<List<bool>>();
                    while (findHole)
                    {
                        findHole = false;
                        Line2D holeStartLine2Ds = new Line2D(0, 0, 1, 1);
                        for (int j = 0; j < lineUseds.Count; j++)
                        {
                            if (!lineUseds[j])
                            {
                                findHole = true;
                                holeStartLine2Ds = line2Ds[j];
                                break;
                            }

                        }
                        if (findHole)
                        {
                            var start = holeStartLine2Ds;
                            List<Vector2> points2Ds = new List<Vector2>();
                            points2Ds.Add(new Vector2(start.X1, start.Y1));
                            List<int> index2Ds = new List<int>();

                            List<bool> isX1s = new List<bool>();
                            Vector2 currentPoint = new Vector2(start.X2, start.Y2);
                            Vector2 previousPoint = new Vector2(start.X1, start.Y1);
                            Vector2 nextPoint = new Vector2(start.X1, start.Y1);
                            Vector2 point0 = new Vector2(start.X1, start.Y1);

                            for (int j = 0; j < line2Ds.Count; j++)
                            {
                                if (Math.Abs(line2Ds[j].X1 - previousPoint.X) <= tol && Math.Abs(line2Ds[j].Y1 - previousPoint.Y) <= tol)
                                {
                                    if (Math.Abs(line2Ds[j].X2 - currentPoint.X) <= tol && Math.Abs(line2Ds[j].Y2 - currentPoint.Y) <= tol)
                                    {
                                        index2Ds.Add(j);
                                        isX1s.Add(true);
                                        lineUseds[j] = true;
                                    }
                                }
                                else if (Math.Abs(line2Ds[j].X2 - previousPoint.X) <= tol && Math.Abs(line2Ds[j].Y2 - previousPoint.Y) <= tol)
                                {
                                    if (Math.Abs(line2Ds[j].X1 - currentPoint.X) <= tol && Math.Abs(line2Ds[j].Y1 - currentPoint.Y) <= tol)
                                    {
                                        index2Ds.Add(j);
                                        isX1s.Add(false);
                                        lineUseds[j] = true;
                                    }
                                }
                            }
                            // find loop
                            while (Vector2.DistanceSquared(currentPoint, point0) >= tol * tol)
                            {

                                var v01 = Vector2.Normalize(new Vector2(currentPoint.X - previousPoint.X, currentPoint.Y - previousPoint.Y));
                                float maxAngle = -10;
                                int targetj = -1;
                                bool isX1 = false;
                                for (int j = 0; j < line2Ds.Count; j++)
                                {

                                    float dist1 = new Vector2(line2Ds[j].X1 - currentPoint.X, line2Ds[j].Y1 - currentPoint.Y).Length();
                                    float dist2 = new Vector2(line2Ds[j].X2 - currentPoint.X, line2Ds[j].Y2 - currentPoint.Y).Length();
                                    if (dist1 <= dist2)
                                    {
                                        if (dist1 <= tol)
                                        {
                                            if (Math.Abs(line2Ds[j].X1 - currentPoint.X) <= tol && Math.Abs(line2Ds[j].Y1 - currentPoint.Y) <= tol)
                                            {
                                                var v12 = Vector2.Normalize(new Vector2(line2Ds[j].X2 - currentPoint.X, line2Ds[j].Y2 - currentPoint.Y));
                                                var angle = (float)Math.Acos(Vector2.Dot(v01, v12)) * Math.Sign(v01.X * v12.Y - v01.Y * v12.X);
                                                if (Math.Abs(angle - Math.PI) <= tol || Math.Abs(line2Ds[j].X2 - previousPoint.X) <= tol && Math.Abs(line2Ds[j].Y2 - previousPoint.Y) <= tol)
                                                {
                                                    continue;
                                                }
                                                if (angle > maxAngle)
                                                {
                                                    maxAngle = angle;
                                                    nextPoint = new Vector2(line2Ds[j].X2, line2Ds[j].Y2);
                                                    isX1 = true;
                                                    targetj = j;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (dist2 <= tol)
                                        {
                                            if (Math.Abs(line2Ds[j].X2 - currentPoint.X) <= tol && Math.Abs(line2Ds[j].Y2 - currentPoint.Y) <= tol)
                                            {

                                                var v12 = Vector2.Normalize(new Vector2(line2Ds[j].X1 - currentPoint.X, line2Ds[j].Y1 - currentPoint.Y));
                                                var angle = (float) Math.Acos(Vector2.Dot(v01, v12)) * Math.Sign(v01.X * v12.Y - v01.Y * v12.X);
                                                if (Math.Abs(angle - Math.PI) <= tol || Math.Abs(line2Ds[j].X1 - previousPoint.X) <= tol && Math.Abs(line2Ds[j].Y1 - previousPoint.Y) <= tol)
                                                {
                                                    continue;
                                                }
                                                if (angle > maxAngle)
                                                {
                                                    maxAngle = angle;
                                                    nextPoint = new Vector2(line2Ds[j].X1, line2Ds[j].Y1);
                                                    isX1 = false;
                                                    targetj = j;
                                                }
                                            }
                                        }
                                    }





                                }
                                if (targetj > -1)
                                {
                                    lineUseds[targetj] = true;
                                    previousPoint = new Vector2(currentPoint.X, currentPoint.Y);
                                    points2Ds.Add(new Vector2(currentPoint.X, currentPoint.Y));
                                    index2Ds.Add(targetj);
                                    isX1s.Add(isX1);
                                    currentPoint = new Vector2(nextPoint.X, nextPoint.Y);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            // check area
                            float A = 0;
                            for (int j = 0; j < points2Ds.Count; j++)
                            {
                                int k = j + 1;
                                if (k == points2Ds.Count)
                                {
                                    k = 0;
                                }
                                A += points2Ds[j].X * points2Ds[k].Y - points2Ds[k].X * points2Ds[j].Y;
                            }
                            if (A > 0)
                            {
                                points2Ds = GeometryUtil.GetInverseList(points2Ds);
                                index2Ds = GeometryUtil.GetInverseList(index2Ds);
                                isX1s = GeometryUtil.GetInverseList(isX1s);
                            }

                            holePoint2DGroups.Add(points2Ds);
                            holeIndexGroups.Add(index2Ds);
                            holeIsX1Groups.Add(isX1s);
                            tol = GeometryUtil.lengthTol;
                        }
                    }


                    //construct mesh
                    for (int j = 0; j < point2DGroups.Count; j++)
                    {
                        ProfileMesh pmesh = new ProfileMesh(point2DGroups[j]);

                        //checkhole
                        List<List<Vector2>> holes = new List<List<Vector2>>();

                        for (int k = 0; k < holePoint2DGroups.Count; k++)
                        {
                            var pTest = holePoint2DGroups[k][0];
                            for (int l = 0; l < pmesh.Triangles.Count; l += 3)
                            {
                                var x1 = pmesh.Vertices[pmesh.Triangles[l]].X;
                                var y1 = pmesh.Vertices[pmesh.Triangles[l]].Y;
                                var x2 = pmesh.Vertices[pmesh.Triangles[l + 1]].X;
                                var y2 = pmesh.Vertices[pmesh.Triangles[l + 1]].Y;
                                var x3 = pmesh.Vertices[pmesh.Triangles[l + 2]].X;
                                var y3 = pmesh.Vertices[pmesh.Triangles[l + 2]].Y;
                                if (GeometryUtil.IsonTriangle(pTest, new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3)))
                                {
                                    holes.Add(holePoint2DGroups[k]);
                                    break;
                                }
                            }

                        }
                        if (holes.Count == 0)
                        {
                            var PIndices = pmesh.Triangles;

                            for (int k = 0; k < PIndices.Count; k++)
                            {
                                var lin = lines[indexGroups[j][PIndices[k]]];
                                //global index
                                if (isX1Groups[j][PIndices[k]])
                                {
                                    pmesh.Vertices[PIndices[k]] = new Vector3(lin.X1, lin.Y1, lin.Z1);
                                }
                                else
                                {
                                    pmesh.Vertices[PIndices[k]] = new Vector3(lin.X2, lin.Y2, lin.Z2);
                                }
                            }
                        }
                        else
                        {
                            pmesh = new ProfileMesh(point2DGroups[j], holes);
                            for (int k = 0; k < pmesh.Vertices.Count; k++)
                            {
                                //
                                var v1 = pmesh.Vertices[k];
                                bool converted = false;
                                for (int l = 0; l < point2DGroups[j].Count; l++)
                                {
                                    var v2 = point2DGroups[j][l];
                                    if (Math.Abs(v1.X - v2.X) <= tol && Math.Abs(v1.Y - v2.Y) <= tol)
                                    {

                                        var lin = lines[indexGroups[j][l]];
                                        //global index
                                        if (isX1Groups[j][l])
                                        {
                                            pmesh.Vertices[k] = new Vector3(lin.X1, lin.Y1, lin.Z1);
                                        }
                                        else
                                        {
                                            pmesh.Vertices[k] = new Vector3(lin.X2, lin.Y2, lin.Z2);
                                        }
                                        converted = true;
                                        break;
                                    }
                                }

                                if (!converted)
                                {
                                    for (int m = 0; m < holePoint2DGroups.Count; m++)
                                    {
                                        for (int l = 0; l < holePoint2DGroups[m].Count; l++)
                                        {
                                            var v2 = holePoint2DGroups[m][l];
                                            if (Math.Abs(v1.X - v2.X) <= tol && Math.Abs(v1.Y - v2.Y) <= tol)
                                            {
                                                var lin = lines[holeIndexGroups[m][l]];
                                                //global index
                                                if (holeIsX1Groups[m][l])
                                                {
                                                    pmesh.Vertices[k] = new Vector3(lin.X1, lin.Y1, lin.Z1);
                                                }
                                                else
                                                {
                                                    pmesh.Vertices[k] = new Vector3(lin.X2, lin.Y2, lin.Z2);
                                                }
                                                converted = true;
                                                break;
                                            }
                                        }
                                        if (converted)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        merged.AddMesh(pmesh);
                    }

                }
            }




            // remove modified triangle

            for (int i = removeList.Count - 1; i >= 0; i--)
            {
                merged.Triangles.RemoveAt(removeList[i]);
            }

            removeList = new List<int>();
            // check if inside or outside
            for (int i = 0; i < merged.Triangles.Count; i += 3)
            {
                float x = 0;
                float y = 0;
                float z = 0;
                
                for (int j = 0; j < 3; j++)
                {
                    var v = merged.Vertices[merged.Triangles[i + j]];
                    x += v.X;
                    y += v.Y;
                    z += v.Z;
                }
                x /= 3;
                y /= 3;
                z /= 3;
                Vector3 v1 = merged.Vertices[merged.Triangles[i]];
                Vector3 v2 = merged.Vertices[merged.Triangles[i+1]];
                Vector3 v3 = merged.Vertices[merged.Triangles[i +2]];
                Vector3 n = Vector3.Normalize(Vector3.Cross(v2 - v1, v3 - v1));
                if (float.IsNaN(n.X) || float.IsNaN(n.Y) || float.IsNaN(n.Z))
                {
                    removeList.Add(i);
                    removeList.Add(i + 1);
                    removeList.Add(i + 2);
                    continue;
                }
                Dictionary<float, List<int>> indicesDics = new Dictionary<float, List<int>>();
                bool isInside = false;


                if (Math.Abs(n.X) >= Math.Abs(n.Y)&& Math.Abs(n.X) >= Math.Abs(n.Z))
                {
                    for (int j = 0; j < BTriangles.Count; j++)
                    {
                        var triangle = BTriangles[j];
                        var xInt = triangle.GetX(y, z, tol);
                        if (!float.IsNaN(xInt))
                        {
                            float dx = xInt - x;
                            bool found = false;
                            foreach (var minLength in indicesDics.Keys)
                            {
                                if (Math.Abs(dx - minLength) <= tol)
                                {
                                    indicesDics[minLength].Add(j);
                                    found = true;
                                    break;
                                }
                            }
                            if (!found && dx >= -tol)
                            {
                                if (dx <= tol)
                                {
                                    dx = 0;
                                }
                                indicesDics.Add(dx, new List<int>());
                                indicesDics[dx].Add(j);
                            }
                        }
                    }
                   
                    var dists = indicesDics.Keys.ToList();
                    dists.Sort();
                    if(dists.Count > 0 && dists[0] < 2 * tol)
                    {
                        var index = indicesDics[dists[0]][0];
                        if (Vector3.Dot(BNormals[index], n) > 0)
                        {
                            isInside = !isSamePlaneSameDirOutside;
                        }
                        else
                        {
                            isInside = isSamePlaneSameDirOutside;
                        }
                    }
                    else
                    {
                        foreach (var dist in dists)
                        {
                            var indices = indicesDics[dist];
                            bool foundRight = false;
                            bool foundWrong = false;

                            foreach (var index in indices)
                            {
                                if (BNormals[index].X < 0)
                                {
                                    foundRight = true;
                                }
                                else if (BNormals[index].X > 0)
                                {
                                    foundWrong = true;
                                }
                            }

                            if (foundRight && !foundWrong)
                            {
                                isInside = true;
                                break;
                            }
                            else if (!foundRight && foundWrong)
                            {
                                break;
                            }
                        }
                    }
                    
                }


                else if (Math.Abs(n.Y) >= Math.Abs(n.Z) && Math.Abs(n.Y) >= Math.Abs(n.X))
                {
                    for (int j = 0; j < BTriangles.Count; j++)
                    {
                        var triangle = BTriangles[j];
                        var yInt = triangle.GetY(z, x, tol);
                        if (!float.IsNaN(yInt))
                        {
                            float dy = yInt - y;
                            bool found = false;
                            foreach (var minLength in indicesDics.Keys)
                            {
                                if (Math.Abs(dy - minLength) <= tol)
                                {
                                    indicesDics[minLength].Add(j);
                                    found = true;
                                    break;
                                }
                            }
                            if (!found && dy >= -tol)
                            {
                                if(dy <= tol)
                                {
                                    dy = 0;
                                }
                                indicesDics.Add(dy, new List<int>());
                                indicesDics[dy].Add(j);
                            }
                        }
                    }

                    var dists = indicesDics.Keys.ToList();
                    dists.Sort();
                    if (dists.Count > 0 && dists[0] < 2 * tol)
                    {
                        var index = indicesDics[dists[0]][0];
                        if (Vector3.Dot(BNormals[index], n) > 0)
                        {
                            isInside = !isSamePlaneSameDirOutside;
                        }
                        else
                        {
                            isInside = isSamePlaneSameDirOutside;
                        }
                    }
                    else
                    {
                        foreach (var dist in dists)
                        {
                            var indices = indicesDics[dist];
                            bool foundRight = false;
                            bool foundWrong = false;

                            foreach (var index in indices)
                            {
                                if (BNormals[index].Y < 0)
                                {
                                    foundRight = true;
                                }
                                else if (BNormals[index].Y > 0)
                                {
                                    foundWrong = true;
                                }
                            }

                            if (foundRight && !foundWrong)
                            {
                                isInside = true;
                                break;
                            }
                            else if (!foundRight && foundWrong)
                            {
                                break;
                            }
                        }
                    }      
                }




                else 
                {
                    for (int j = 0; j < BTriangles.Count; j++)
                    {
                        var triangle = BTriangles[j];
                        var zInt = triangle.GetZ(x, y, tol);
                        if (!float.IsNaN(zInt))
                        {
                            float dz = zInt - z;
                            bool found = false;
                            foreach (var minLength in indicesDics.Keys)
                            {
                                if (Math.Abs(dz - minLength) <= tol)
                                {
                                    indicesDics[minLength].Add(j);
                                    found = true;
                                    break;
                                }
                            }
                            if (!found && dz >= -tol)
                            {
                                if(dz <= tol)
                                {
                                    dz = 0;
                                }
                                indicesDics.Add(dz, new List<int>());
                                indicesDics[dz].Add(j);
                            }
                        }
                    }

                    var dists = indicesDics.Keys.ToList();
                    dists.Sort();
                    if (dists.Count > 0 && dists[0] < 2 * tol)
                    {
                        var index = indicesDics[dists[0]][0];
                        if (Vector3.Dot(BNormals[index], n) > 0)
                        {
                            isInside = !isSamePlaneSameDirOutside;
                        }
                        else
                        {
                            isInside = isSamePlaneSameDirOutside;
                        }
                    }
                    else
                    {
                        foreach (var dist in dists)
                        {
                            var indices = indicesDics[dist];
                            bool foundRight = false;
                            bool foundWrong = false;

                            foreach (var index in indices)
                            {
                                if (BNormals[index].Z < 0)
                                {
                                    foundRight = true;
                                }
                                else if (BNormals[index].Z > 0)
                                {
                                    foundWrong = true;
                                }
                            }

                            if (foundRight && !foundWrong)
                            {
                                isInside = true;
                                break;
                            }
                            else if (!foundRight && foundWrong)
                            {
                                break;
                            }
                        }
                    }
                    
                }




                if ((isInside && SelectOutside) || (!isInside && !SelectOutside))
                {
                    removeList.Add(i);
                    removeList.Add(i + 1);
                    removeList.Add(i + 2);
                }

            }

            for (int i = removeList.Count - 1; i >= 0; i--)
            {
                merged.Triangles.RemoveAt(removeList[i]);
            }
            return merged;
        }

    }
}
