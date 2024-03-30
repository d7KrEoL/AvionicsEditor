using AvionicsEditor.Map;
using AvionicsEditor.MTypes.Map.Pins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Map.Lines
{
    public class Polyline : MultiLineBase
    {
        public Polyline(string name, MTypes.Math.IPolyLine polyline, GameMap parentmap, PinType type)
        {
            Create(name, polyline, parentmap, type);
        }
    }
}
