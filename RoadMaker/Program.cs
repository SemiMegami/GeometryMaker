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
                Thetas = new List<float>() { 0,0 }
            };
            Mesh3D mesh = straightRoadMaker.GetMesh();
            mesh.ExportToObj("../../../Outputs/StraightRoad.obj");


            straightRoadMaker = new StraightRoadMaker()
            {
                NumberOfLanes = 1,
                LaneWidth = 2.5f,
                Length = 10,
                Thickness = 0.1f,
                Dy = 3,
                Thetas = new List<float>() { 0, 0 }
            };
            mesh = straightRoadMaker.GetMesh();
            mesh.ExportToObj("../../../Outputs/UpStraightRoad.obj");

            straightRoadMaker = new StraightRoadMaker()
            {
                NumberOfLanes = 1,
                LaneWidth = 2.5f,
                Length = 10,
                Thickness = 0.1f,
                Dy = -3,
                Thetas = new List<float>() { 0, 0 }
            };
            mesh = straightRoadMaker.GetMesh();
            mesh.ExportToObj("../../../Outputs/DownStraightRoad.obj");


            float rad = 10;
            float turnAngle = 45;
            float curvelength = rad * turnAngle * MathF.PI  / 180;

            float dy = curvelength / 10 * 3;

            CurveRoadMaker curveRoadMaker = new CurveRoadMaker()
            {
                NumberOfLanes = 1,
                LaneWidth = 2.5f,
                Thickness = 0.1f,
                TurnRadius = 10,
                TurnAngle = 45,
                Dy = 0,
                Thetas = new List<float>() { 0,0,0,0,0,0,0,0 }
            };
            mesh = curveRoadMaker.GetMesh();
            mesh.ExportToObj("../../../Outputs/RightCurveRoad.obj");

            curveRoadMaker = new CurveRoadMaker()
            {
                NumberOfLanes = 1,
                LaneWidth = 2.5f,
                Thickness = 0.1f,
                TurnRadius = -10,
                TurnAngle = 45,
                Dy = 0,
                Thetas = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            mesh = curveRoadMaker.GetMesh();
            mesh.ExportToObj("../../../Outputs/LeftCurveRoad.obj");



            curveRoadMaker = new CurveRoadMaker()
            {
                NumberOfLanes = 1,
                LaneWidth = 2.5f,
                Thickness = 0.1f,
                TurnRadius = 10,
                TurnAngle = 45,
                Dy = dy,
                Thetas = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            mesh = curveRoadMaker.GetMesh();
            mesh.ExportToObj("../../../Outputs/UpRightCurveRoad.obj");

            curveRoadMaker = new CurveRoadMaker()
            {
                NumberOfLanes = 1,
                LaneWidth = 2.5f,
                Thickness = 0.1f,
                TurnRadius = -10,
                TurnAngle = 45,
                Dy = dy,
                Thetas = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            mesh = curveRoadMaker.GetMesh();
            mesh.ExportToObj("../../../Outputs/UpLeftCurveRoad.obj");

            curveRoadMaker = new CurveRoadMaker()
            {
                NumberOfLanes = 1,
                LaneWidth = 2.5f,
                Thickness = 0.1f,
                TurnRadius = 10,
                TurnAngle = 45,
                Dy = -dy,
                Thetas = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            mesh = curveRoadMaker.GetMesh();
            mesh.ExportToObj("../../../Outputs/DownRightCurveRoad.obj");

            curveRoadMaker = new CurveRoadMaker()
            {
                NumberOfLanes = 1,
                LaneWidth = 2.5f,
                Thickness = 0.1f,
                TurnRadius = -10,
                TurnAngle = 45,
                Dy = -dy,
                Thetas = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            mesh = curveRoadMaker.GetMesh();
            mesh.ExportToObj("../../../Outputs/DownLeftCurveRoad.obj");
        }
    }
}
