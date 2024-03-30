using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.Processing
{
    public enum EditorMode
    {
        None = 0,
        AddWPT = 1,
        AddTarget = 2,
        EditPoints = 3,
        AddAproach = 4,
        AddGlidepath = 5,
        AddDRPM = 6,
        AddBRDM = 7,
        AddRunway = 8,
        AddBeacon = 9,
        CalcDistance = 10,
        CalcAngle = 11,
        GetPos = 12,
    }
}
