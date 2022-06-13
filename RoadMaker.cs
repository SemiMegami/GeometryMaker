using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeDMaker.Geometry;
namespace GeometryMaker
{
    abstract class RoadMaker
    {
        public int NumberOfLanes { get; set; }
        public float LaneWidth { get; set; }
        public float Thickness { get; set; }
        public float Dy { get; set; }
        public List<float> Thetas { get; set; }
        public abstract Mesh3D GetMesh();
    }
}
