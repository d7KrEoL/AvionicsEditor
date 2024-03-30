
using BruTile.Cache;
using BruTile.Predefined;
using Mapsui.Tiling.Fetcher;
using Mapsui.Tiling.Layers;
using OsmSharp.Db;
using OsmSharp.Streams;
using OsmSharp.Geo;
using System;
using System.Windows.Controls;
using Mapsui;
using NetTopologySuite.Features;
using System.Threading.Tasks;
using Mapsui.Tiling;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.Zoom;
using Mapsui.Widgets;
using Mapsui.Extensions;
using System.IO;
using System.Numerics;
using System.Linq;
using OsmSharp;
using Mapsui.Layers;
using BruTile;
using System.Collections.Generic;
using Mapsui.Providers;
using NetTopologySuite.Geometries;
using BruTile.FileSystem;
using Mapsui.Widgets.MouseCoordinatesWidget;
using System.Windows.Shapes;
using BruTile.MbTiles;
using SQLite;
using System.Windows.Media.Imaging;
using System.Globalization;
using Mapsui.Rendering.Skia.Extensions;
using HarfBuzzSharp;
using System.Windows.Markup;
using Mapsui.Styles;
using AvionicsEditor.Properties;
using BruTile.Wmts;
using System.Reflection;
using Mapsui.Animations;

namespace AvionicsEditor.Map
{
    public class GameMap : ICanvasMap
    {
        public Mapsui.Map MapVar { get; private set; }
        public Mapsui.Viewport ViewportVar { get; private set; }
        public Image MImage { get; private set; }

        public GameMap(Image MapImage, MTypes.Math.Vector2 LeftDownCornerPos, MTypes.Math.Vector2 RightDownCornerPos, MTypes.Math.Vector2 LeftUpCornerPos, MTypes.Math.Vector2 RightUpCornerPos) 
        {
            CreateMap();
        }

        public Task<Mapsui.Map> CreateMapAsync()
        {
            return Task.FromResult(CreateMap());
        }

        public Mapsui.Map CreateMap()
        {

            ViewportVar = new Mapsui.Viewport();
            MapVar = new Mapsui.Map
            {
                CRS = "EPSG:3857",
                BackColor = Mapsui.Styles.Color.Transparent,
            };
            //----------------------TIFF
            //MapVar.Layers.Insert((int)Map.LayerType.Area,CreateLayerWithRasterFeature(ViewportVar.ToExtent(), $"{Environment.CurrentDirectory}\\resource\\satmap.jpg", 0.9d));
            Console.WriteLine("Loaded!");
            var extentOfImage = new MRect(-3000, -3000, 3000, 3000);
            //MapVar.CRS = "EPSG:3857";
            MapVar.Layers.Insert((int)Map.LayerType.Area, CreateLayerWithRasterFeature(extentOfImage, $"{Environment.CurrentDirectory}\\resource\\satmap.jpg", 0.9d));

            /*MapVar.Layers.Insert((int)Map.LayerType.Background, CreateLayerWithRasterFeature(ViewportVar.ToExtent(), $"{Environment.CurrentDirectory}\\resource\\map.png", 0.9d));
            MapVar.Layers.Insert((int)Map.LayerType.Background, CreateLayerWithRasterFeature(extentOfImage, $"{Environment.CurrentDirectory}\\resource\\map.png", 0.9d));/*

            //---------------------TIFF End


            //----------------------SQLite --Reinstall SQLite libs for this code to work (broken cz of Mapsui.Extensions that changed ver of sql lib)
            /*string path = $"{Environment.CurrentDirectory}\\resource\\sanandreas_tiles.sqlite";//sanandreas_tiles.sqlite
            MbTilesTileSource mbtilesTilesource = new MbTilesTileSource(new SQLiteConnectionString(path, false));
            Console.WriteLine($"This is a byte array of an image file loaded from MBTiles with size: {mbtilesTilesource.Name}");*/

            //2,123267 8,071037

            /*Mapsui.Tiling.Layers.TileLayer SQLiteLayer = new TileLayer(mbtilesTilesource);
            MapVar.Layers.Add(SQLiteLayer);*/
            //-----------------------SQLite End

            MapVar.Widgets.Add(new MouseCoordinatesWidget(MapVar) { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom, BackColor = Mapsui.Styles.Color.Black, TextColor = Mapsui.Styles.Color.White});
            MapVar.Widgets.Add(new ScaleBarWidget(MapVar) { TextAlignment = Alignment.Center, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, TextColor = Mapsui.Styles.Color.Black });
            MapVar.Widgets.Add(new ZoomInOutWidget { MarginX = 20, MarginY = 40, Size = 25f, StrokeColor = Mapsui.Styles.Color.Transparent, BackColor = Mapsui.Styles.Color.Black, TextColor = Mapsui.Styles.Color.White });
            
            MapVar.Navigator.ZoomTo(10, -1);
            var StartPoint = new MPoint(2500, -2500);
            MapVar.Navigator.CenterOn(StartPoint, -1L, null);
            if (MapVar.Layers.Count < 1) { return null; }
            
            return MapVar;
        }

        public MTypes.Math.Vector2 GetGlobalPos(MTypes.Math.Vector2 pos)
        {
            //MTypes.Math.Vector2 Res;
            if (IsPointOnScreen(pos))
            {
                return new MTypes.Math.Vector2(pos.LocalX, pos.LocalY);
            }
            return null;
        }
        public bool IsPointOnScreen(MTypes.Math.Vector2 pos)
        {
            return false;
        }

        public MPoint GetMouseMapCoords(MainWindow MWindow)
        {
            return MapVar.Navigator.Viewport.ScreenToWorld(new MPoint(System.Windows.Input.Mouse.GetPosition(MWindow.MapControl).X, System.Windows.Input.Mouse.GetPosition(MWindow.MapControl).Y));
        }
        private ILayer CreateLayerWithRasterFeature(MRect extent, string path, double opacity)
        {
            //With resources, but long way
            /*
            var path = Resources.Images.satmap;
            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            var rasterFeature = new RasterFeature(new MRaster((byte[])converter.ConvertTo(path, typeof(byte[])), extent)) { Styles = { new RasterStyle() } };
            */

            //Fast way, but using local image file 

            var fileStream = File.OpenRead(path);
            var bytes = fileStream.ToBytes();
            var rasterFeature = new RasterFeature(new MRaster(bytes, extent)) { Styles = { new RasterStyle() { /*Opacity = opacityFloat*/ } } };


            return new MemoryLayer() { Features = new List<RasterFeature> { rasterFeature }, Name = "San Andreas Map", Opacity = opacity };
        }

    }
}
