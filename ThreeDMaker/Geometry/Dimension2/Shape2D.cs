using System;
using System.Collections.Generic;
using System.Numerics;

namespace ThreeDMaker.Geometry.Dimension2
{
    public abstract class Shape2D
    {
        protected const int tolFactor = 10000;
        public bool IsCurve;
        public List<Vector2> points { get; protected set; }
       
        public int Count => points.Count;

        public abstract void UpdatePoints();


        protected void Add(float x, float y)
        {
            Add(new Vector2(x, y));
        }
        protected void Add(Vector2 v)
        {
            points.Add(v);
        }
        protected void AddRange(List<Vector2> v)
        {
            points.AddRange(v);
        }

        public abstract Shape2D GetOffSet(float d);
        
    }
}
