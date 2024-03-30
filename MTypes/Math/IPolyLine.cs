using GeoAPI.Geometries;
using Mapsui.Layers;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Math
{
    public interface IPolyLine
    {
        List<Vector3> Points { get; }
        List<ILine> Lines { get; }
        System.Drawing.Color PolyColor { get; }
        AvionicsEditor.Map.GameMap ParentMap { get; }
        MemoryLayer PolyLayer { get; }
        void AddPoint(Vector3 point);
        void AddPoint(double x, double y, double z);
        void AddPoint(int index, Vector3 point);
        void AddPoint(int index, double x, double y, double z);
        void AddLine(Vector3 pont1, Vector3 point2);
        void AddLine(Vector2 point1, Vector2 point2);
        void Clear();
        void RemovePoint(int index);
        void RemovePoint(Vector3 point);
        void RemovePoint(double x, double y, double z);
        void SetPolyLayer(MemoryLayer layer);
        double Length();
        int PointsCount();
        NetTopologySuite.Geometries.Coordinate[] ToGeometryCoordinates();
    }
}
