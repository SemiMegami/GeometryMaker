using System;
using System.Collections.Generic;
using System.Numerics;

namespace ThreeDMaker.Geometry
{
    public class Path3D : List<AxisPoint3D>
    {
        public Path3D()
        {

        }
        public Path3D(List<Vector3> nodes, bool close = false)
        {
            int n = nodes.Count;
            Clear();

            for (int i = 0; i < n; i++)
            {
                int j = (i < n - 1) ? (i + 1) : 0;
                int k = (i > 0) ? (i - 1) : (n - 1);

                Vector3 vij = nodes[j] - nodes[i];
                Vector3 vki = nodes[i] - nodes[k];
                float dij = vij.Length();
                float dki = vki.Length();
                float d = dij + dki;
                Vector3 front;
                if (close || (i > 0 && i < n - 1))
                {
                    front = Vector3.Normalize(Vector3.Lerp(vki, vij, dij / d));

                }
                else if (i == 0)
                {
                    front = Vector3.Normalize(vij);
                }
                else
                {
                    front = Vector3.Normalize(vki);
                }
                Add(new AxisPoint3D(nodes[i], front));
            }
        }
        public Path3D(List<AxisPoint3D> nodes)
        {
            AddRange(nodes);
        }
    }
}
