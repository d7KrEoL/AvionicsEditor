using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Map.Pins
{
    public enum PinType
    {
        Default = 0,
        SelectedPoint = 1,
        MovePoint = 2,
        TargetPoint = 3,
        ApproachPoint = 4,
        GlidepathPoint = 5,
        DRDM = 6,
        BRDM = 7,
        Runway = 8,
        Beacon = 9,
        ScheaticFigure = 10,
    }
}
