using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Map.Wpt
{
    public class BRDMBeacon : BCN
    {
        public readonly Avionics.Landing.LandBeaconType LandingType = Avionics.Landing.LandBeaconType.BRDM;

        public BRDMBeacon(string name, MTypes.Math.Vector3 position, double workradius, int type)
        {
            SetName(name);
            Position = position;
            SetRadius(workradius);
            SetType(type);
        }
    }
}
