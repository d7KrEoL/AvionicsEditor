using System;
using System.Collections.Generic;
using System.Linq;


namespace AvionicsEditor.MTypes.Math
{
    public interface ILine
    {
        Vector2 Interpolate2D(double Percent);
        Vector3 Interpolate3D(double Percent);

        double FindPercent(Vector2 Point);
        double FindPercent(Vector3 Point);

        Line Add(Line line2);

        double Length();

        Vector3 GetAngle(Line line);
        Vector3 GetAngle(Vector3 Point);
        Line AddAngle(Vector3 Angle);
        double GetAzimuth();
        Line Cut(double Percent);
        Line Slice(Line line);
    }
}
