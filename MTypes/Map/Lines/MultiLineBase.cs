using AvionicsEditor.Map;
using AvionicsEditor.MTypes.Map.Pins;
using AvionicsEditor.MTypes.Math;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling.Layers;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Map.Lines
{
    public abstract class MultiLineBase
    {
        public string Name { get; private set; }
        public PinType Type { get; private set; }
        public MemoryLayer PinLayer { get; private set; }
        public AvionicsEditor.Map.GameMap ParentMap { get; private set; }
        public MTypes.Math.IPolyLine PolyLine { get; private set; }
        
        public void Create(string name, MTypes.Math.IPolyLine polyline, GameMap parentmap, PinType type)
        {
            Name = name;
            Type = type;
            ParentMap = parentmap;
            PolyLine = polyline;

            ParentMap.MapVar.Layers.Insert((int)AvionicsEditor.Map.LayerType.VectorData, CreateLine());
        }

        private MemoryLayer CreateLine()
        {
            /*Coordinate coords = PolyLine.ToCoords();
            var polyline = new LineString(PolyLine.Points)
            polyline = new LineString(polyline.Coordinate.Select(v => SphericalMercator.FromLonLat(v.X, v.Y).ToCoordinate().ToArray()));

            var linelayer = new MemoryLayer()
            {
                Features = new[] { new GeometryFeature { Geometry = polyline } },
                Name = "LineStringLayer",
                Style = CreateLineStringStyle()
            };
            return linelayer;*/
            var polyline = new LineString(PolyLine.ToGeometryCoordinates());
            if (PolyLine.PolyLayer == null)
            {
                var linelayer = new MemoryLayer()
                {
                    Features = new[] { new GeometryFeature { Geometry = polyline } },
                    Name = Name,
                    Style = CreateLineStringStyle()
                };
                PolyLine.SetPolyLayer(linelayer);  
            }
            else
            {
                PolyLine.SetPolyLayer(new MemoryLayer()
                {
                    Features = new[] {new GeometryFeature {  Geometry = polyline } },
                    Name = Name,
                    Style = CreateLineStringStyle()
                });
            }
            PinLayer = PolyLine.PolyLayer;
            return PolyLine.PolyLayer;
        }

        private IStyle CreateLineStringStyle()
        {
            return new VectorStyle
            {
                Fill = null,
                Outline = null,
#pragma warning disable CS8670 // Object or collection initializer implicitly dereferences possibly null member.
                Line = { Color = Color.FromString("DodgerBlue"), Width = 3 }//DodgerBlue
            };
        }
    }
}
