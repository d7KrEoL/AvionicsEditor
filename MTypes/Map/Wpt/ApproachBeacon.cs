﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Map.Wpt
{
    public class ApproachBeacon : BCN
    {
        public readonly Avionics.Landing.LandBeaconType LandingType = Avionics.Landing.LandBeaconType.Approach;

        public ApproachBeacon(string name, MTypes.Math.Vector3 position, double workradius, int type)
        {
            SetName(name);
            Position = position;
            SetRadius(workradius);
            SetType(type);
        }
    }
}
