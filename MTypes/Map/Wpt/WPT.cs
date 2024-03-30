using AvionicsEditor.MTypes.Map.Pins;
using Mapsui.Layers;
using Mapsui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;
using Mapsui.Styles;

namespace AvionicsEditor.MTypes.Map.Wpt
{
    public abstract class WPT
    {
        public MTypes.Math.Vector3 Position { get; set; }
        public MemoryLayer WPTLayer { get; private set; }
        public AvionicsEditor.Map.GameMap ParentMap { get; private set; }
        public PinType PType { get; private set; }

        public WPT CreateWPT(string name, MTypes.Math.Vector3 position, AvionicsEditor.Map.GameMap parentmap, PinType type)
        {

            MPoint PinPoint = new MPoint(position.X, position.Y);
            ParentMap = parentmap;
            PointFeature PointFeature = new Mapsui.Layers.PointFeature(PinPoint);
            WPTLayer = new MemoryLayer { Features = new List<PointFeature> { PointFeature }, Name = name, Opacity = 1, Style = GetActualStyle() };
            ParentMap.MapVar.Layers.Insert((int)AvionicsEditor.Map.LayerType.Waypoint, WPTLayer);
            PType = type;
            Position = position;


            return this;
        }

        public void SetPType(PinType type)
        {
            PType = type;
        }

        public void Delete()
        {
            ParentMap.MapVar.Layers.Remove(WPTLayer);
        }

        private Mapsui.Styles.SymbolStyle GetActualStyle()
        {
            return new Mapsui.Styles.SymbolStyle
            {
                SymbolScale = 0.25,
                RotateWithMap = true,
                SymbolType = SymbolType.Ellipse,
                Fill = new Brush(new Color(248, 248, 255))//(248, 248, 255)
            };
        }
    }
}
