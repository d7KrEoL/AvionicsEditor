using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Map.Wpt
{
    public abstract class TGT : WPT
    {
        public string TargetName { get; private set; }
        public int ModelID { get; private set; }
        public double SearchRadius { get; private set; } = 20;

        public void SetTargetName(string targetName)
        {
            TargetName = targetName;
        }

        public void SetModelID(int modelid)
        {
            ModelID = modelid;
        }

        public void SetRadius(double radius)
        {
            SearchRadius = radius;
        }
    }
}
