using Mapsui.Layers;
using Mapsui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Map.Pins
{
    public class DefaultPin : PinBase
    {
        public DefaultPin(string name, double X, double Y, PinType type, AvionicsEditor.Map.GameMap maplink)
        {
            SetParentMap(maplink);
            CreatePin(name, X, Y, type);
        }
    }
}
