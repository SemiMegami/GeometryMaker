using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ThreeDMaker.Geometry.Util;
using ThreeDMaker.Geometry.Dimension2;
namespace ThreeDMaker.Geometry
{
    class Mesh2DGeometryUtil
    {
        public static List<int> EarClippingAlgorithm(List<Vector2> vertices)
        {
            int n = vertices.Count;
            List<int> indices = new List<int>();

            for (int i = 0; i < n; i++)
            {
                indices.Add(i);
            }
            return EarClippingAlgorithm(vertices, indices);
        }


        public static List<int> EarClippingAlgorithm(List<Vector2> vertices, List<int> indices)
        {
            if(vertices.Count < 3)
            {
                return new List<int>();
            }
            if (vertices.Count == 3)
            {
                if(GeometryUtil.IsTurningLeft(vertices[0], vertices[1], vertices[2],true))
                {
                    return indices;
                }
                else
                {
                    return new List<int>();
                }
            }

            List<int> triangle = new List<int>();
            int n = vertices.Count;

            List<Line2D> lines = new List<Line2D>();
            for (int i = 0; i < n; i++)
            {
                int j = i + 1;
                if (j >= n) j -= n;
                lines.Add(new Line2D(vertices[i], vertices[j]));
            }
            for (int i = 0; i < n; i++)
            {
                int j = i + 1;
                int k = i + 2;
                if (j >= n) j -= n;
                if (k >= n) k -= n;
                if (GeometryUtil.IsTurningLeft(vertices[i], vertices[j], vertices[k], true))
                {
                    Line2D line = new Line2D(vertices[i], vertices[k]);
                    bool foundInside = false;
                    // check it any point inside the new triangle
                    for (int l = 0; l < n; l++)
                    {
                        if (l != i && l != j && l != k && GeometryUtil.IsonTriangle(vertices[l], vertices[i], vertices[j], vertices[k], false))
                        {
                            foundInside = true;
                            break;
                        }
                        if (l != i && l != j && l != k && line.IsPointSplitline(vertices[l].X, vertices[l].Y))
                        {
                            foundInside = true;
                            break;
                        }
                    }
                    if (!foundInside)
                    {
                        for (int l = 0; l < n; l++)
                        {
                            if (line.IsLineCross(lines[l]))
                            {
                                foundInside = true;
                                break;
                            }
                        }
                    }

                    if (!foundInside)
                    {
                        List<int> subIndices = new List<int>();
                        List<Vector2> subVertices = new List<Vector2>();
                        for (int l = 0; l < n; l++)
                        {
                            if (l != j)
                            {
                                subIndices.Add(indices[l]);
                                subVertices.Add(vertices[l]);
                            }
                        }
                        triangle = EarClippingAlgorithm(subVertices, subIndices);
                        triangle.Add(indices[i]);
                        triangle.Add(indices[j]);
                        triangle.Add(indices[k]);
                        return triangle;
                    }
                }

            }
            return triangle;
        }
    }
}
