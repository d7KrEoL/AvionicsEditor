using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace AvionicsEditor.MTypes.Map.Wpt
{
    internal class Targetpoint : TGT
    {
        public Targetpoint(string trgname, int modelid, double sradius, MTypes.Math.Vector3 position)
        {
            SetTargetName(trgname);
            SetModelID(modelid);
            SetRadius(sradius);
            Position = position;
        }
    }
}
