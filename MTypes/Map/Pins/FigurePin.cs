using Mapsui;
using Mapsui.Layers;
using Mapsui.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AvionicsEditor.MTypes.Map.Pins
{
    public class FigurePin : PinBase
    {
        public enum ModelType
        {
            None = 0,
            Circle = 1,
            Triangle = 2,
            Square = 3,
        }
        private ModelType PinModel;
        private double PinSize;
        private double PinWidth;
        public FigurePin(string name, double X, double Y, PinType type, AvionicsEditor.Map.GameMap maplink, double size = 1, double PinWidth = 0, ModelType model = ModelType.Circle)
        {
            SetParentMap(maplink);
            PinModel = model;
            PinSize = size;
            CreatePin(name, X, Y, type);
        }

        public override PinBase CreatePin(string labeltext, double X, double Y, PinType type)
        {
            SetName(labeltext);
            SetType(type);
            SetPosition(new MTypes.Math.Vector2(X, Y));

            MPoint PinPoint = new MPoint(X, Y);

            var feature = new PointFeature(PinPoint);

            double mapzoom = GVars.MWindow.MainMap.MapVar.Navigator.Viewport.Resolution * 100;
            //Console.WriteLine($"CURRENT RES: {mapzoom}");
            switch (PinModel)
            {
                case ModelType.Circle:
                    feature.Styles.Add(new Mapsui.Styles.SymbolStyle
                    {
                        SymbolType = SymbolType.Ellipse,
                        SymbolScale = CalcSymbolScaleCircle(PinSize, mapzoom),//PinSize,
                        Fill = null,
                        Outline = new Pen
                        {
                            Color = Color.LightBlue,
                            Width = PinWidth,
                        }
                    });
                    break;
            }
            SetPinLayer(new MemoryLayer { Features = new List<PointFeature> { feature }, Name = labeltext, Opacity = 1, Style = GetActualStyle() });
            ParentMap.MapVar.Layers.Insert((int)type, PinLayer);

            return this;
        }
        private Mapsui.Styles.SymbolStyle GetActualStyle()
        {
            return new Mapsui.Styles.SymbolStyle
            {
                //SymbolOffset = new Mapsui.Styles.Offset(Res.X * CFX.X, Res.Y * CFX.Y),
                /*Text = labeltext,
                ForeColor = new Color(13, 197, 255),
                BackColor = new Brush(new Color(13, 197, 255)),*/
                SymbolScale = 0.25,
                RotateWithMap = true,
                SymbolType = SymbolType.Ellipse,
                Fill = new Brush(new Color(13, 197, 255))

            };
        }

        private double CalcSymbolScaleCircle(double scale, double zoom)
        {
            switch (zoom)
            {
                case double n when (n <= 1): return 0;//M=1max
                case double n when (n <= 4): return scale * 199;//M=2
                case double n when (n <= 10): return scale * 102;//M=5
                case double n when (n <= 20): return scale * 51.5;//M=10
                case double n when (n <= 40): return scale * 25.5;//M=20
                case double n when (n <= 60): return scale * 12.7;//M=20min
                case double n when (n <= 110): return scale * 6.36;//M=50
                case double n when (n <= 210): return scale * 3.19;//M=100
                case double n when (n <= 410): return scale * 1.6;//M=200
                case double n when (n <= 1010): return scale * 0.8;//M=500
                case double n when (n <= 2010): return scale * 0.4;//M=1000
                case double n when (n <= 4010): return scale * 0.2;//M=2000
                case double n when (n <= 10010): return scale * 0.1;//M=5000
                case double n when (n <= 20010): return scale * 0.05;//M=10000
                case double n when (n <= 30010): return scale * 0.025;//M=20000
                case double n when (n > 30010): return 0;//M=20000min
            }
            return scale * (0.001 + (zoom * 0.001));
        }
    }
}
