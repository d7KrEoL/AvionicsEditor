using AvionicsEditor.MTypes.Math;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Map.Pins
{
    public abstract class PinBase
    {
        public string Name { get; private set; }
        public PinType Type { get; private set; }
        //public MemoryLayer PinLayer { get; private set; }
        public MemoryLayer PinLayer { get; private set; }
        public AvionicsEditor.Map.GameMap ParentMap { get; private set; }
        public MTypes.Math.Vector2 Position { get; private set; }
        public void SetType(PinType type)
        {
            Type = type;
        }
        public void SetParentMap(AvionicsEditor.Map.GameMap maplink)
        {
            ParentMap = maplink;
        }
        public void SetPinLayer(MemoryLayer pinlayer)
        {
            PinLayer = pinlayer;
        }
        public void SetPosition(MTypes.Math.Vector2 pos)
        {
            Position = pos;
        }
        public void SetName(string name)
        {
            Name = name;
        }

        public virtual PinBase CreatePin(string name, double X, double Y, PinType type)
        {
            Name = name;
            Type = type;
            Position = new MTypes.Math.Vector2(X, Y);


            MPoint PinPoint = new MPoint(X, Y);

            //MPoint PinPoint = new MPoint(Mapsui.Projections.SphericalMercator.FromLonLat(X, Y).ToMPoint());


            //MPoint PinPoint = new MPoint(Mapsui.Projections.SphericalMercator.FromLonLat(X, Y).ToMPoint());


            //var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(PinPoint.X, PinPoint.Y).ToMPoint();
            //PointFeature PointFeature = new Mapsui.Layers.PointFeature(sphericalMercatorCoordinate);*/

            // Set the center of the viewport to the coordinate. The UI will refresh automatically
            // Additionally you might want to set the resolution, this could depend on your specific purpose

            //ParentMap.MapVar.Navigator.CenterOnAndZoomTo(sphericalMercatorCoordinate, ParentMap.MapVar.Navigator.Resolutions[9]);
            
            //PointFeature PointFeature = new Mapsui.Layers.PointFeature(PinPoint);
            PointFeature PointFeature = new Mapsui.Layers.PointFeature(PinPoint);

            /*var memoryProvider = new MemoryProvider(PointFeature)
            {
                CRS = null // The DataSource CRS needs to be set
            };*/

            /*var dataSource = new ProjectingProvider()
            {
                CRS = "EPSG:3857"
            };*/

            /*PinLayer = new Layer
            {
                DataSource = dataSource,
                Name = "Pin",
                Style = new Mapsui.Styles.SymbolStyle { SymbolScale = 0.25, },
                IsMapInfoLayer = true
            };*/
            //PinLayer = new MemoryLayer { Features = new List<PointFeature> { PointFeature }, Name = name, Opacity = 1, Style = new Mapsui.Styles.SymbolStyle { SymbolScale = 0.25, } };

            //ParentMap.MapVar.Layers.Insert((int)AvionicsEditor.Map.LayerType.Pin, PinLayer);
            /*ParentMap.MapVar.Layers.Add(PinLayer);
            ParentMap.MapVar.Layers.Move((int)AvionicsEditor.Map.LayerType.Pin, PinLayer);
            ParentMap.MapVar.Refresh();*/


            PinLayer = new MemoryLayer { Features = new List<PointFeature> { PointFeature }, Name = name, Opacity = 1, Style = GetActualStyle() };

            //ParentMap.MapVar.Layers.Insert((int)AvionicsEditor.Map.LayerType.Pin,PinLayer);
            ParentMap.MapVar.Layers.Insert((int)type, PinLayer);

            return this;
        }

        private Mapsui.Styles.SymbolStyle GetActualStyle()
        {
            /*Vector2 Res;
            if (GVars.MWindow.WindowState == System.Windows.WindowState.Maximized)
            {
                Res = new Vector2(System.Windows.SystemParameters.PrimaryScreenWidth, System.Windows.SystemParameters.PrimaryScreenHeight);
                GVars.MWindow.SetDebugText($"XY: {Res.X}x{Res.Y}");
            }
            else
                Res = new Vector2(GVars.MWindow.Width, GVars.MWindow.Height);

            var CFX = new Vector2(0, 0);

            if (Res.X > 1900) CFX.X = -0.02;
            else if (Res.X > 1300) CFX.X = -0.023;
            else if (Res.X > 600) CFX.X = -0.02;
            else CFX.X = -0.02;
            if (Res.Y > 1060) CFX.Y = 0.15;
            else if (Res.Y > 1040) CFX.Y = 0.19;
            else if (Res.Y > 700) CFX.Y = 0.19;
            else if (Res.Y > 600) CFX.Y = 0.19;
            else CFX.Y = 0.185;*/
            return new Mapsui.Styles.SymbolStyle
            {
                //SymbolOffset = new Mapsui.Styles.Offset(Res.X * CFX.X, Res.Y * CFX.Y),
                SymbolScale = 0.25,
                RotateWithMap = true,
                SymbolType = SymbolType.Ellipse,
                Fill = new Brush(new Color(13, 197, 255))
            };
        }

        public void DeletePin()
        {
            ParentMap.MapVar.Layers.Remove(PinLayer);
        }
    }
}
