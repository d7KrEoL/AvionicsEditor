using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvionicsEditor.MTypes.Map.Wpt
{
    public class MilBeacon : BCN
    {
        public string CryptoCode { get; private set; }

        public MilBeacon(string name, MTypes.Math.Vector3 position, double workradius, int type, string code)
        {
            SetName(name);
            Position = position;
            SetRadius(workradius);
            SetType(type);
            CryptoCode = code;
         }

        public void ChangeCode(string code)
        {
            CryptoCode = code;
        }
    }
}
