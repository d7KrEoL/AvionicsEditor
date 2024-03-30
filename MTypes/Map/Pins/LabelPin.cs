using Mapsui;
using Mapsui.Layers;
using Mapsui.Styles;
using Mapsui.Widgets.ScaleBar;
using NetTopologySuite.Operation.Overlay.Validate;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvionicsEditor.MTypes.Map.Pins
{
    public class LabelPin : PinBase
    {
        public enum ModelType
        {
            None = 0,
            Circle = 1,
            Triangle = 2,
            Square = 3,
        }
        private ModelType PinModel;
        private double Scale;
        public LabelPin(string name, double X, double Y, PinType type, AvionicsEditor.Map.GameMap maplink, double scale, ModelType model = ModelType.None)
        {
            SetParentMap(maplink);
            PinModel = model;
            Scale = scale;
            CreatePin(name, X, Y, type);
        }
        public override PinBase CreatePin(string labeltext, double X, double Y, PinType type)
        {
            SetName(labeltext);
            SetType(type);
            SetPosition(new MTypes.Math.Vector2(X, Y));


            MPoint PinPoint = new MPoint(X, Y);
            /*PointFeature PointFeature = new Mapsui.Layers.PointFeature(PinPoint) { ["Label"] = labeltext };

            SetPinLayer(new MemoryLayer { Features = new List<PointFeature> { PointFeature }, Name = labeltext, Opacity = 1, Style = GetActualStyle() });

            ParentMap.MapVar.Layers.Insert((int)type, PinLayer);*/

            var LabelFeature = CreateDiverseFeatures(PinPoint, labeltext, type);
            SetPinLayer(new MemoryLayer { Features = LabelFeature, Name = labeltext, Opacity = 1, Style = GetActualStyle() });
            ParentMap.MapVar.Layers.Insert((int)type, PinLayer);

            return this;
            //return base.CreatePin(labeltext, X, Y, type);
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
                SymbolType = SymbolType.Triangle,
                Fill = new Brush(new Color(13, 197, 255))

            };
        }
        private IEnumerable<IFeature> CreateDiverseFeatures(MPoint point, string text, PinType type)
        {
            var features = new List<IFeature>();
            //var styles = CreateDiverseStyles().ToList();
            PointFeature feature;
            if (text == string.Empty) feature = new PointFeature(point);
            else
            {
                feature = new PointFeature(point)
                {
                    ["Label"] = text
                };
            }

            //feature.Styles.Add(styles[0]);
            feature.Styles.Add(SmalleDot());
            features.Add(feature);
            features.Add(CreatePointWithStackedStyles(text, point, type));
            return features;
        }

        private IStyle SmalleDot()
        {
            return new SymbolStyle { SymbolScale = 0.2, Fill = new Brush(new Color(13, 197, 255)) };
        }

        private IEnumerable<IStyle> CreateDiverseStyles()
        {
            const int diameter = 16;
            return new List<IStyle>
        {
            new SymbolStyle {SymbolScale = 0.6, SymbolOffset = new Offset(diameter, diameter), SymbolType = SymbolType.Rectangle, Fill = new Brush(Color.Black)},
        };
        }

        private IFeature CreatePointWithStackedStyles(string text, MPoint point, PinType type)
        {
            //var feature = new PointFeature(new MPoint(5000000, -5000000));
            var feature = new PointFeature(point);
            //double scale = 2.0f;
            //double.TryParse(text, out scale);
            double mapzoom = GVars.MWindow.MainMap.MapVar.Navigator.Viewport.Resolution;
            if (PinModel == ModelType.Circle)
            {
                feature.Styles.Add(new Mapsui.Styles.SymbolStyle
                {
                    //SymbolScale = CalcSymbolScaleCircle(scale, mapzoom),
                    SymbolScale = CalcSymbolScaleCircle(Scale, mapzoom),
                    SymbolType = SymbolType.Ellipse,

                    Fill = null,
                    Outline = new Pen
                    {
                        Color = Color.LightBlue,
                        //Width = 1 - (scale * 0.001),
                        Width = 1 - (Scale * 0.001),
                    }
                });
            }

            /*feature.Styles.Add(new SymbolStyle
            {
                SymbolScale = 0.8f,
                Fill = new Brush { Color = Color.Red }
            });

            feature.Styles.Add(new SymbolStyle
            {
                SymbolScale = 0.5f,
                Fill = new Brush { Color = Color.Black }
            });*/

            if (type == PinType.ScheaticFigure)
            {
                feature.Styles.Add(new LabelStyle
                {
                    Text = text,
                    HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Left
                });
            }
            return feature;
        }

        private double CalcSymbolScaleCircle(double scale, double zoom)
        {
            switch (zoom)
            {
                /*case double n when (n <= 0.004): return scale * 31;//M=1max
                case double n when (n <= 0.007): return scale * 16;//M=1big
                case double n when (n <= 0.008): return scale * 8;//M=1
                case double n when (n <= 0.016): return scale * 4;//M=1min
                case double n when (n <= 0.031): return scale * 2;//M=2
                case double n when (n <= 0.062): return scale * 1;//M=5
                case double n when (n <= 0.124): return scale * 0.5;//M=10
                case double n when (n <= 0.247): return scale * 0.25;//M=20
                case double n when (n <= 0.494): return scale * 0.13;//M=20min
                case double n when (n <= 0.987): return scale * 0.065;//M=50
                case double n when (n <= 1.973): return scale * 0.033;//M=100
                case double n when (n <= 3.946): return scale * 0.016;//M=200
                case double n when (n <= 7.893): return scale * 0.008;//M=500
                case double n when (n <= 15.783): return scale * 0.004;//M=1000
                case double n when (n <= 31.566): return scale * 0.002;//M=2000
                case double n when (n <= 31.567): return scale * 0.001;//M=2000
                case double n when (n <= 63.132): return scale * 0.00099;//M=5000
                case double n when (n <= 126.263): return scale * 0.0005;//M=10000
                case double n when (n <= 252.526): return scale * 0.00025;//M=20000
                case double n when (n <= 505.051): return scale * 0.00025;//M=20000min*/
                case double n when (n <= 0.0041): return scale * 31;//M=1max
                case double n when (n <= 0.0071): return scale * 16;//M=1big
                case double n when (n <= 0.0081): return scale * 8;//M=1
                case double n when (n <= 0.0161): return scale * 4;//M=1min
                case double n when (n <= 0.0311): return scale * 2;//M=2
                case double n when (n <= 0.0621): return scale * 1;//M=5
                case double n when (n <= 0.1241): return scale * 0.5;//M=10
                case double n when (n <= 0.2471): return scale * 0.25;//M=20
                case double n when (n <= 0.4941): return scale * 0.13;//M=20min
                case double n when (n <= 0.9871): return scale * 0.065;//M=50
                case double n when (n <= 1.9731): return scale * 0.033;//M=100
                case double n when (n <= 3.9461): return scale * 0.016;//M=200
                case double n when (n <= 7.8931): return scale * 0.008;//M=500
                case double n when (n <= 15.7831): return scale * 0.004;//M=1000
                case double n when (n <= 31.5661): return scale * 0.002;//M=2000
                case double n when (n <= 31.5671): return scale * 0.001;//M=2000
                case double n when (n <= 63.1321): return scale * 0.00099;//M=5000
                case double n when (n <= 126.2631): return scale * 0.0005;//M=10000
                case double n when (n <= 252.5261): return scale * 0.00025;//M=20000
                case double n when (n <= 505.0511): return scale * 0.00025;//M=20000min
            }
            return scale * (0.001 + (zoom * 0.001));
        }
    }
}
