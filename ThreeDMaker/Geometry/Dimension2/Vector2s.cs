using System;
using System.Collections.Generic;
using System.Numerics;

namespace ThreeDMaker.Geometry.Dimension2
{
    public class Vector2s:List<Vector2>
    {
        public Vector2s()
        {
        }
        public void Add(float x, float y)
        {
            Add(new Vector2(x, y));
        }
    }
}
