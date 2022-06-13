using System;
using System.Collections.Generic;
using ThreeDMaker.Geometry;
namespace GeometryMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            StraightRoadMaker straightRoadMaker = new StraightRoadMaker()
            {
                NumberOfLanes = 1,
                LaneWidth = 2.5f,
                Length = 10,
                Thickness = 0.1f,
                Dy = 0,
                Thetas = new List<float>() { 0, 5, 10, 15, 20 }
            };
            Mesh3D mesh = straightRoadMaker.GetMesh();
            mesh.ExportToObj("Road Test.obj");
            CurveRoadMaker curveRoadMaker = new CurveRoadMaker()
            {
                NumberOfLanes = 1,
                LaneWidth = 2.5f,
                Thickness = 0.1f,
                TurnRadius = 10,
                TurnAngle = 45,
                Dy = 0,
                Thetas = new List<float>() { 0, 5, 10, 15, 20 }
            };
            mesh = curveRoadMaker.GetMesh();
            mesh.ExportToObj("Curve Road Test.obj");
        }
    }
}
