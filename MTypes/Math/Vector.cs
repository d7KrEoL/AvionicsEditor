using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Math
{
    public abstract class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double LocalX { get; set; }
        public double LocalY { get; set; }
        public virtual double FindDistance(Vector2 DistanceTo)
        {
            return System.Math.Sqrt(((DistanceTo.X - X) * (DistanceTo.X - X)) + ((DistanceTo.Y - Y) * (DistanceTo.Y - Y)));
        }
        public virtual double FindDistance(Vector3 DistanceTo)
        {
            return System.Math.Sqrt(((DistanceTo.X - X) * (DistanceTo.X - X)) + ((DistanceTo.Y - Y) * (DistanceTo.Y - Y)) + (DistanceTo.Z * DistanceTo.Z));
        }
        public virtual bool IsEqual(Vector2 Pos)
        {
            if (X == Pos.X && Y == Pos.Y) return true;
            return false;
        }
        public virtual bool IsEqual(Vector3 Pos)
        {
            if (X == Pos.X && Y == Pos.Y && Pos.Z == 0) return true;
            return false;
        }
        public virtual bool InAreaOf(Vector2 Area, double radius)
        {
            //double d = System.Math.Sqrt(((X - Area.X) * (X - Area.X)) + ((Y - Area.Y) * (Y - Area.Y)));
            //double d = FindDistance(Area);
            if (FindDistance(Area) < radius)
            {
                return true;
            }
            return false;
        }
        public virtual bool InAreaOf(Vector3 Area, double radius)
        {
            if (FindDistance(Area) < radius)
            {
                return true;
            }
            return false;
        }
        public virtual bool InAreaOf(Vector2 RectPos1, Vector2 RectPos2)
        {
            Vector2 RectPos3 = new Vector2(RectPos1.X, RectPos2.Y);
            Vector2 RectPos4 = new Vector2(RectPos2.X, RectPos1.X);

            return false;
        }
        //public virtual void SetX(double x) { X = x; }
        //public virtual void SetY(double y) { Y = y; }
        //public virtual void SetLocalX(double localX) { LocalX = localX; }
        //public virtual void SetLocalY(double localY) { LocalX = localY; }
    }
}
