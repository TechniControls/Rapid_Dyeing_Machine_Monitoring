using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_Stenter_Dryer.Model
{
    public class TemperaturePoint
    {
        public double Time { get; set; }      // OADate
        public float ProcessFirstPt100 { get; set; }   // PV of PT100-1
        public float ProcessSecondPt100 { get; set; }   // PV of PT100-2
        public float SetPoint { get; set; }  // SP
    }
}
