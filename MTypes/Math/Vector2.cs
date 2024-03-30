using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Math
{
    public class Vector2 : Vector
    {
        public Vector2(double x = 0, double y = 0)
        {
            X = x;
            Y = y;
        }
    }
}
