using AvionicsEditor.MTypes.Map.Pins;
using AvionicsEditor.MTypes.Math;
using BruTile.Wms;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using OsmSharp.IO.Zip.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AvionicsEditor.WindowModes.CalcDistance
{
    public static class Processor
    {
        private static MTypes.Math.Vector3 Point1 { get; set; }
        private static MTypes.Math.Vector3 Point2 { get; set; }
        private static MTypes.Map.Pins.LabelPin TextBox { get; set; }
        private static MTypes.Math.PolyLine DistancePoly = new MTypes.Math.PolyLine(System.Drawing.Color.Black);
        private static MTypes.Map.Lines.Polyline DistancePolyLayer;
        public static void OnUpdate(MainWindow MWindow)
        {
            var pos = MWindow.MainMap.GetMouseMapCoords(MWindow);
            switch (GVars.CurrentModeStage)
            {
                case 0:
                    ClearPolyPoints();

                    Point1 = new MTypes.Math.Vector3(pos.X, pos.Y, 0d);
                    GVars.RemovePins(PinType.SelectedPoint);
                    MWindow.SetDebugText($"Got point A ({(int)Point1.X};{(int)Point1.Y}), now select point B");
                    var ResPin1 = new DefaultPin($"Point A", Point1.X, Point1.Y, PinType.SelectedPoint, MWindow.MainMap);
                    GVars.AddPin(ResPin1);

                    AddPolyPoint(Point1);
                    GVars.SetCurrentModeStage(1);
                    break;
                case 1:
                    Point2 = new MTypes.Math.Vector3(pos.X, pos.Y, 0d);
                    var ResPin2 = new DefaultPin($"Point B", Point2.X, Point2.Y, PinType.SelectedPoint, MWindow.MainMap);
                    GVars.AddPin(ResPin2);
                    GVars.SetCurrentModeStage(0);

                    AddPolyPoint(Point2, 1);
                    MWindow.SetDebugText($"Distance: {ProcessCalc()}(m)");
                    break;
            }
            UnRenderPoly();
            RenderPoly();
        }
        public static void OnUpdate(MainWindow MWindow, bool IsUpdating)
        {
            if (!IsUpdating)
            {
                UnRenderPoly();
                RenderPoly();
                return;
            }
            var pos = MWindow.MainMap.GetMouseMapCoords(MWindow);
            switch (GVars.CurrentModeStage)
            {
                case 1:
                    GVars.RemovePins(PinType.SelectedPoint);

                    Point2 = new MTypes.Math.Vector3(pos.X, pos.Y, 0d);
                    var ResPin2 = new DefaultPin($"Point B", Point2.X, Point2.Y, PinType.SelectedPoint, MWindow.MainMap);
                    GVars.AddPin(ResPin2);
                    ResPin2 = new DefaultPin($"Point A", Point1.X, Point1.Y, PinType.SelectedPoint, MWindow.MainMap);
                    GVars.AddPin(ResPin2);
                    
                    AddPolyPoint(Point2, 1);
                    MWindow.SetDebugText($"Distance: {ProcessCalc().ToString("0.00")}(m)");
                    

                    break;
            }
            UnRenderPoly();
            RenderPoly();
        }

        public static void OnClear(MainWindow MWindow)
        {
            ClearPolyPoints();
            UnRenderPoly();
        }

        private static void AddPolyPoint(MTypes.Math.Vector3 Pos)
        {
            //DistancePoly.Points.Add(Pos);//lolkekcheburek
            DistancePoly.AddPoint(Pos);
        }

        private static void AddPolyPoint(MTypes.Math.Vector3 Pos, int idx)
        {
            if (DistancePoly.Points.Count - 1 < idx) AddPolyPoint(Pos);
            else
            {
                DistancePoly.Points[idx] = Pos;
            }
        }

        private static void ClearPolyPoints()
        {
            //DistancePoly.Points.Clear();//????????
            DistancePoly.Clear();
        }

        public static void RenderPoly()
        {
            if (DistancePoly.Points.Count > 1)
            {
                DistancePolyLayer = new MTypes.Map.Lines.Polyline("Waypoints", DistancePoly, GVars.MWindow.MainMap, MTypes.Map.Pins.PinType.SelectedPoint);
                if (DistancePoly.Lines.Count > 0)
                {
                    DistancePoly.RefreshLines();
                    var pos = DistancePoly.Lines[0].Interpolate2D(0.5);
                    double dist = ProcessCalc();
                    if (TextBox != null && TextBox.PinLayer != null) TextBox.ParentMap.MapVar.Layers.Remove(TextBox.PinLayer);
                    //WTF? textbox only drawning Circle if string is dynamic-changing variable, not "anystaticstring", only (int)dist or smtng
                    var AZ = new LabelPin($"[AZ:{(int)FindAzimuth()}]", Point1.X, Point1.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, dist);
                    GVars.AddPin(AZ);

                    var CirclePin = new FigurePin(string.Empty, Point1.X, Point1.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, dist*0.01, 0.1, FigurePin.ModelType.Circle);
                    GVars.AddPin(CirclePin);
                    //if (GVars.MWindow.MainMap.MapVar.Navigator.Viewport.ScreenToWorld(MPoint()))
                    if (dist > 500)
                    {
                        TextBox = new LabelPin($"Dist:{(int)dist}", Point2.X, Point2.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, dist, LabelPin.ModelType.None);
                        GVars.AddPin(TextBox);
                    }
                    TextBox = new LabelPin(dist.ToString("0.00"), pos.X, pos.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, dist,LabelPin.ModelType.None);
                    GVars.AddPin(TextBox);
                    //Console.WriteLine($"{pos.X} {pos.Y}");
                }
            }
        }

        public static void RenderPoly(bool RenderFigures)
        {
            if (DistancePoly.Points.Count > 1)
            {
                DistancePolyLayer = new MTypes.Map.Lines.Polyline("Waypoints", DistancePoly, GVars.MWindow.MainMap, MTypes.Map.Pins.PinType.SelectedPoint);
                if (DistancePoly.Lines.Count > 0)
                {
                    DistancePoly.RefreshLines();
                    var pos = DistancePoly.Lines[0].Interpolate2D(0.5);
                    double dist = ProcessCalc();
                    if (TextBox != null && TextBox.PinLayer != null) TextBox.ParentMap.MapVar.Layers.Remove(TextBox.PinLayer);
                    var AZ = new LabelPin($"[AZ:{(int)FindAzimuth()}]", Point1.X, Point1.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, dist);
                    GVars.AddPin(AZ);
                    if (RenderFigures)
                    {
                        var CirclePin = new FigurePin(string.Empty, Point1.X, Point1.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, dist * 0.01, 0.1, FigurePin.ModelType.Circle);
                        GVars.AddPin(CirclePin);
                    }
                    if (dist > 500)
                    {
                        TextBox = new LabelPin($"Dist:{(int)dist}", Point2.X, Point2.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, dist, LabelPin.ModelType.None);
                        GVars.AddPin(TextBox);
                    }
                    TextBox = new LabelPin(dist.ToString("0.00"), pos.X, pos.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, dist, LabelPin.ModelType.None);
                    GVars.AddPin(TextBox);
                    //Console.WriteLine($"{pos.X} {pos.Y}");
                }
            }
        }

        public static void UnRenderPoly()
        {
            if (DistancePolyLayer == null) return;
            GVars.MWindow.MainMap.MapVar.Layers.Remove(DistancePolyLayer.PinLayer);
            GVars.RemovePins(PinType.ScheaticFigure);
            DistancePolyLayer = null;
        }

        private static double ProcessCalc()
        {
            MTypes.Math.Line PLine = new Line(Point1, Point2);
            return PLine.Length();
        }

        private static double FindAzimuth()
        {
            var Direction = new Line(Point1, Point2);
            return Direction.GetAzimuth();
        }
    }
}
