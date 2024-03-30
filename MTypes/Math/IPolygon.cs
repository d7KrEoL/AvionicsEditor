using System.Collections.Generic;
using System.Windows.Media;

namespace AvionicsEditor.MTypes.Math
{
    public interface IPolygon
    {
        List<Line> Lines { get; }
        Color PolyColor { get; set; }
        void AddLine(Line line);
        void RemoveLine(Line line);
        double CalcArea();
        bool IsPointInside(Vector3 Point);
        bool CollidesLine(Line line);
        bool CollidesPoly(Line line);
    }
}
