using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ThreeDMaker.Geometry;
using ThreeDMaker.Geometry.Util;
namespace ThreeDMaker.Geometry.Dimension2
{
    public class Profile2D
    {
     
        public Shape2D OutterCurve { get; set; }
        public List<Shape2D> InnerCurves { get; set; }

        public List<Profile2D> Composites;
       
        public Profile2D()
        {
            InnerCurves = new List<Shape2D>();
            Composites = new List<Profile2D>();
        }
        public Profile2D(Shape2D OutterCurve, List<Shape2D> InnerCurves)
        {
            this.OutterCurve = OutterCurve;
            this.InnerCurves = InnerCurves;
        }

        public Profile2D(List<Vector2> OuterCurve, List<List<Vector2>> InnerCurves)
        {
            List<Vector2> outer = new List<Vector2>(OuterCurve);
            if( GeometryUtil.Area(OuterCurve) < 0)
            {
                outer = GeometryUtil.GetInverseList(OuterCurve);
            }
            this.OutterCurve = new Polygon2D(outer);

            
            this.InnerCurves = new List<Shape2D>();
            foreach(var innercurve in InnerCurves)
            {

                List<Vector2> inner = new List<Vector2>(innercurve);
                if (GeometryUtil.Area(innercurve) > 0)
                {
                    inner = GeometryUtil.GetInverseList(innercurve);
                }
                this.InnerCurves.Add(new Polygon2D(inner));
            }
        }
    }
}
