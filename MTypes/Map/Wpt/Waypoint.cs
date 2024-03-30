using AvionicsEditor.MTypes.Map.Pins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Map.Wpt
{
    public class Waypoint : WPT
    {
        public Waypoint(string name, MTypes.Math.Vector3 position, AvionicsEditor.Map.GameMap parentmap, PinType type) 
        {
            CreateWPT(name, position, parentmap, type);
            //Position = pos;
        }

    }
}
