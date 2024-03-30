using System.Collections.Generic;
using System.Windows.Media;

namespace AvionicsEditor.MTypes.Math
{
    internal class Multiline : IPolygon
    {
        public List<Line> Lines { get; }
        public Color PolyColor { get; set; }
        public void AddLine(Line line)
        {
            Lines.Add(line);
        }
        public void RemoveLine(Line line)
        {
            Lines.Remove(line);
        }
        public double CalcArea()
        {
            return 0;//TODO
        }
        public bool IsPointInside(Vector3 Point)
        {
            return false;//TODO
        }
        public bool CollidesLine(Line line)
        {
            return false;//TODO
        }
        public bool CollidesPoly(Line line)
        {
            return false;//TODO
        }
    }
}
