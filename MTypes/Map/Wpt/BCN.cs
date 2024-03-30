using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Map.Wpt
{
    public abstract class BCN : WPT
    {
        public string Name { get; private set; }
        public double RadiusRadiation { get; private set; }
        public int BeaconType { get; private set; }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetRadius(double radiation)
        {
            RadiusRadiation = radiation;
        }

        public void SetType(int type)
        {
            BeaconType = type;
        }
    }
}
