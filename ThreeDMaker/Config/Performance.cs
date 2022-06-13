using System;
namespace ThreeDMaker.Config
{
    public class Performance
    {
        public int curveDetail;
        public int CurveAngleDegree { get; set; }

        public int NCSections => 360 / CurveAngleDegree;
        public Performance()
        {
            curveDetail = 5;
        }
    }
}
