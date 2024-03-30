using AvionicsEditor.MTypes.Map.Lines;
using Mapsui.Layers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Math
{
    public class PolyLine : IPolyLine
    {
        public List<Vector3> Points { get; private set; } = new List<Vector3>();
        public List<MultiLineBase> MultiLines { get; private set; } = new List<MultiLineBase>();
        public List<ILine> Lines { get; private set; } = new List<ILine>();
        public System.Drawing.Color PolyColor { get; private set; }
        public AvionicsEditor.Map.GameMap ParentMap { get; private set; }
        public MemoryLayer PolyLayer { get; private set; } = null;

        public PolyLine(List<Vector3> points, System.Drawing.Color color) 
        { 
            Points = points;
            PolyColor = color;
        }
        public PolyLine(Vector3 point, System.Drawing.Color color)
        {
            Points.Add(point);
            if (Points.Count > 1) AddLine(Points[Points.Count - 2], Points[Points.Count - 1]);
        }
        public PolyLine(double x, double y, double z, System.Drawing.Color color)
        {
            Points.Add(new Vector3(x, y, z));
            if (Points.Count > 1) AddLine(Points[Points.Count - 2], Points[Points.Count -1]);
            PolyColor = color;
        }
        public PolyLine(System.Drawing.Color color)
        {
            PolyColor = color;
        }
        public int FindNearest(Vector3 point)
        {
            int nearidx = 0;
            double neardist = 3500;
            double curdist;
            for(int i = 0; i < Points.Count; i++)
            {
                curdist = Points[i].FindDistance(point);
                if (curdist < neardist)
                {
                    nearidx = i;
                    neardist = curdist;
                }
            }
            return nearidx;
        }
        public int FindNearest(double x, double y, double z)
        {
            return FindNearest(new Vector3(x, y, z));
        }
        public int FindNearest(Vector2 point)
        {
            return FindNearest(new Vector3(point, 0));
        }
        public int FindNearest2D(double x, double y)
        {
            return FindNearest(new Vector3(x, y, 0));
        }
        public void AddPoint(Vector3 point)
        {
            Points.Add(point);
            if (Points.Count > 1) AddLine(Points[Points.Count - 2], Points[Points.Count - 1]);
            RefreshLines();
        }
        public void AddPoint(double x, double y, double z)
        {
            Points.Add(new Vector3(x, y, z));
            if (Points.Count > 1) AddLine(Points[Points.Count - 2], Points[Points.Count - 1]);
            RefreshLines();
        }
        public void AddPoint(int index, Vector3 point)
        {
            Points.Insert(index, point);
            RefreshLines();
        }
        public void AddPoint(int index, double x, double y, double z)
        {
            Points.Insert(index, new Vector3(x, y, z));
            RefreshLines();
        }
        public void AddLine(Vector3 point1, Vector3 point2)
        {
            Lines.Add(new Line(point1, point2));
        }
        public void AddLine(Vector2 point1, Vector2 point2)
        {
            Lines.Add(new Line(new Vector3(point1, 0), new Vector3(point2, 0)));
        }
        public void RefreshLines()
        {
            Lines.Clear();
            for(int i = 1; i < Points.Count; i++)
            {
                AddLine(Points[i - 1], Points[i]);
            }
        }
        public void RemovePoint(int index)
        {
            Points.RemoveAt(index);
        }
        public void RemovePoint(Vector3 point)
        {
            Points.Remove(point);
        }
        public void Clear()
        {
            Points.Clear();
            Lines.Clear();//???????????
            MultiLines.Clear();//?????????
        }
        public void RemovePoint(double x, double y, double z)
        {
            Points.Remove(new Vector3(x, y, z));
        }
        public void SetPolyLayer(MemoryLayer layer)
        {
            PolyLayer = layer;
        }
        public void AddLine(ILine line)
        {
            Lines.Add(line);
        }
        /*public void RefreshLines()
        {
            Lines.Clear();
            for (int i = 1; i < Lines.Count;i++)
            {
                Lines.Add(new Line(Points[i - 1], Points[i]));
            }
        }*/
        public double Length()
        {
            double Len = 0;
            for (int i = 0; i < Points.Count-1; i++)
            {
                Len = Len + Points[i].FindDistance(Points[i + 1]);
            }
            return Len;
        }
        public int PointsCount()
        {
            return Points.Count;
        }

        public NetTopologySuite.Geometries.Coordinate[] ToGeometryCoordinates()
        {
            NetTopologySuite.Geometries.Coordinate[] Res = new NetTopologySuite.Geometries.Coordinate[Points.Count];
            for (int i = 0; i < Points.Count; i++)
            {
                Res[i] = new NetTopologySuite.Geometries.Coordinate(Points[i].X, Points[i].Y);
            }
            return Res;
        }
    }
}
