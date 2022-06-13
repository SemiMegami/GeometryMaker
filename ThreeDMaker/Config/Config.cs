using System;
using System.Collections.Generic;
namespace ThreeDMaker.Config
{
    public class Option
    {
        public static Performance defultPerformance = new Performance();

        public Dictionary<string, Performance> Performances { get; set; }
    }
}
