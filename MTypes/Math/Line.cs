

using Mapsui;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Shapes;

namespace AvionicsEditor.MTypes.Math
{
    public class Line : ILine
    {
        public Vector3 PointA { get; set; }
        public Vector3 PointB { get; set; }
        
        public Line(Vector3 PointStart, Vector3 PointEnd)
        {
            PointA = PointStart;
            PointB = PointEnd;
        }
        public Line()
        {
            PointA = new Vector3();
            PointB = new Vector3();
        }
        public Vector2 Interpolate2D(double Percent)
        {
            Vector2 Res = new Vector2();
            //((maxVal-minVal)*proc)+minVal
            Res.X = ((PointB.X - PointA.X) * Percent) + PointA.X;
             Res.Y = ((PointB.Y - PointA.Y) * Percent) + PointA.Y;
            return Res;
        }

        public Vector3 Interpolate3D(double Percent)
        {
            Vector3 Res = new Vector3();
            Res.X = ((PointB.X - PointA.X) * Percent) + PointA.X;
            Res.Y = ((PointB.Y - PointA.Y) * Percent) + PointA.Y;
            Res.Z = ((PointB.Z - PointA.Z) * Percent) + PointA.Z;

            return Res;
        }

        public double FindPercent(Vector2 Point)
        {
            Vector2 PercVec = new Vector2();
            PercVec.X = (Point.X - PointA.X) / (PointB.X - PointA.X);
            PercVec.Y = (Point.Y - PointA.Y) / (PointB.Y - PointA.Y);
            return PercVec.X + PercVec.Y;
        }

        public double FindPercent(Vector3 Point)
        {
            Vector3 PercVec = new Vector3();
            PercVec.X = (Point.X - PointA.X) / (PointB.X - PointA.X);
            PercVec.Y = (Point.Y - PointA.Y) / (PointB.Y - PointA.Y);
            PercVec.Z = (Point.Z - PointA.Z) / (PointB.Z - PointA.Z);
            return PercVec.X + PercVec.Y + PercVec.Z;
        }

        public double FindPercent(double PercLength)
        {
            return PercLength / Length();
        }

        public Line Add(Line line2)
        {
            Line Res = new Line();
            Res.PointA.X = PointA.X + line2.PointA.X;
            Res.PointA.Y = PointA.Y + line2.PointA.Y;
            Res.PointB.X = PointB.X + line2.PointB.X;
            Res.PointB.Y = PointB.Y + line2.PointB.Y;
            return Res;
        }

        public double Length()
        {
            return PointA.FindDistance(PointB);
        }

        public double GetAngle2D(Line line)
        {
            //Vector3 Res = new Vector3();
            // Use Atan2 for angle
            var LLthis = Length();
            var LLother = line.Length();
            var LLBC = PointB.FindDistance(line.PointB);


            /*

                radians = degrees * (pi/180)

                degrees = radians * (180/pi)
            */
            var radians = System.Math.Acos(((LLthis * LLthis) + (LLBC * LLBC) - (LLother * LLother)) / (2 * LLthis * LLBC));//B
            //var radians = System.Math.Acos(((LLother * LLother) + (LLthis * LLthis) - (LLBC * LLBC)) / (2 * LLother * LLthis));//A
            //var radians = System.Math.Acos(((LLBC * LLBC) + (LLother * LLother) - (LLthis * LLthis)) / (2 * LLBC * LLother));//C
            // Radians into degrees
            return radians * (180 / System.Math.PI);

            //return Res;//TODO
        }
        public double GetAngle2D(Line line, int AngleABC)
        {
            //Vector3 Res = new Vector3();
            // Use Atan2 for angle
            var LLthis = Length();
            var LLother = line.Length();
            var LLBC = PointB.FindDistance(line.PointB);


            /*

                radians = degrees * (pi/180)

                degrees = radians * (180/pi)
            */
            double radians = 0;
            switch (AngleABC)
            {
                case 0:
                    radians = System.Math.Acos(((LLthis * LLthis) + (LLBC * LLBC) - (LLother * LLother)) / (2 * LLthis * LLBC));//B
                    break;
                case 1:
                    radians = System.Math.Acos(((LLother * LLother) + (LLthis * LLthis) - (LLBC * LLBC)) / (2 * LLother * LLthis));//A
                    break;
                case 2:
                    radians = System.Math.Acos(((LLBC * LLBC) + (LLother * LLother) - (LLthis * LLthis)) / (2 * LLBC * LLother));//C
                    break;
            }
            // Radians into degrees
            return radians * (180 / System.Math.PI);

            //return Res;//TODO
        }
        public Vector3 GetAngle(Line line)
        {
            return new Vector3(0, 0, 0);//TODO
        }
        public Vector3 GetAngle(Vector3 Point)
        {
            return new Vector3(0, 0, 0);//TODO
        }
        /*public double GetAngle(Vector3 Point)
        {
            //Vector3 Res = new Vector3();
            //return Res;//TODO
            return System.Math.Atan2(Point.Y - PointA.Y, Point.X - PointA.X);
        }*/

        public double GetAzimuth()
        {
            Line North = new Line(PointA, new Vector3(PointA.X, 4000));
            double percent = North.FindPercent(Length());
            North = new Line(North.PointA, new Vector3(North.Interpolate2D(percent), North.PointB.Z));
            double Res = North.GetAngle2D(this, 1);
            if (North.PointB.X < PointB.X) Res = Res;//Right sight (0-180)
            else Res = 360 - Res;//Left sight (180-360)

            return Res;
        }
        public Line AddAngle(Vector3 Angle)
        {
            Line Res = new Line();
            return Res;//TODO
        }

        public Line Cut(double Percent)
        {
            Line Res = new Line();
            return Res;//TODO
        }
        public Line Slice(Line line)
        {
            Line Res = new Line();
            return Res;//TODO
        }
    }
}
