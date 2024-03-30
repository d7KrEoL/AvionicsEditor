using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Math
{
    public class Vector3 : Vector
    {
        public double Z { get; set; }
        public double LocalZ { get; set; }
        
        public Vector3(double x = 0, double y = 0, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3(MTypes.Math.Vector2 vec, double z)
        {
            X = vec.X;
            Y = vec.Y;
            Z = z;
        }
        public override double FindDistance(Vector3 DistanceTo)
        {
            return System.Math.Sqrt(((DistanceTo.X - X) * (DistanceTo.X - X)) + ((DistanceTo.Y - Y) * (DistanceTo.Y - Y)) + ((DistanceTo.Z - Z) * (DistanceTo.Z - Z)));
        }

        public override bool IsEqual(Vector3 Pos)
        {
            if (Pos.X == X && Pos.Y == Y && Pos.Z == Z) return true;
            else return false;
        }
        public override bool InAreaOf(Vector3 Pos, double radius)
        {
            return false;
        }
    }
}
