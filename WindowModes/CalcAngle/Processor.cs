using AvionicsEditor.MTypes.Map.Pins;
using BruTile.Wms;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.WindowModes.CalcAngle
{
    public static class Processor
    {
        private static MTypes.Math.Vector3 Point1;
        private static MTypes.Math.Vector3 Point2;
        private static MTypes.Math.Vector3 Point3;
        private static MTypes.Map.Pins.LabelPin TextBox { get; set; }
        private static MTypes.Math.PolyLine AnglePoly = new MTypes.Math.PolyLine(System.Drawing.Color.Black);
        private static MTypes.Map.Lines.Polyline AnglePolyLayer;
        public static void OnUpdate(MainWindow MWindow, bool IsUpdating)
        {
            var pos = MWindow.MainMap.GetMouseMapCoords(MWindow);
            switch (GVars.CurrentModeStage)
            {
                case 0:
                    if (!IsUpdating) return;
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

                    AddPolyPoint(Point2, 1);
                    if (IsUpdating)
                    {
                        var ResPin2 = new DefaultPin($"Point B", Point2.X, Point2.Y, PinType.SelectedPoint, MWindow.MainMap);
                        GVars.AddPin(ResPin2);
                        GVars.SetCurrentModeStage(2);
                    }
                    MWindow.SetDebugText($"Got point B ({(int)Point2.X};{(int)Point2.Y}), now select point C");
                    break;
                case 2:
                    Point3 = new MTypes.Math.Vector3(pos.X, pos.Y, 0d);

                    AddPolyPoint(Point3, 2);
                    if (IsUpdating)
                    {
                        var ResPin3 = new DefaultPin($"Point C", Point3.X, Point3.Y, PinType.SelectedPoint, MWindow.MainMap);
                        var ResPin2 = new DefaultPin($"Point B", Point2.X, Point2.Y, PinType.SelectedPoint, MWindow.MainMap);
                        GVars.AddPin(ResPin3);
                        GVars.AddPin(ResPin2);
                        GVars.SetCurrentModeStage(0);
                    }
                    MWindow.SetDebugText($"Angle: {ProcessCalc().ToString("0.00")}°");
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
            AnglePoly.AddPoint(Pos);
        }

        private static void AddPolyPoint(MTypes.Math.Vector3 Pos, int idx)
        {
            if (AnglePoly.Points.Count - 1 < idx) AddPolyPoint(Pos);
            else AnglePoly.Points[idx] = Pos;
        }

        private static void ClearPolyPoints()
        {
            //DistancePoly.Points.Clear();//????????
            AnglePoly.Clear();
        }

        private static void RenderPoly()
        {
            if (AnglePoly.Points.Count > 1)
            {
                AnglePolyLayer = new MTypes.Map.Lines.Polyline("Waypoints", AnglePoly, GVars.MWindow.MainMap, MTypes.Map.Pins.PinType.SelectedPoint);
                if (AnglePoly.Lines.Count > 1)
                {
                    var pos = Point2;
                    if (TextBox != null && TextBox.PinLayer != null) TextBox.ParentMap.MapVar.Layers.Remove(TextBox.PinLayer);
                    TextBox = new LabelPin($"{ProcessCalc().ToString("0.00")}", pos.X, pos.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, 0, LabelPin.ModelType.None);
                    GVars.AddPin(TextBox);
                    //Console.WriteLine($"{pos.X} {pos.Y}");
                }
            }
        }

        private static void UnRenderPoly()
        {
            if (AnglePolyLayer == null) return;
            GVars.MWindow.MainMap.MapVar.Layers.Remove(AnglePolyLayer.PinLayer);
            GVars.RemovePins(PinType.ScheaticFigure);
            AnglePolyLayer = null;
        }
        private static double ProcessCalc()
        {
            var LNA = new MTypes.Math.Line(Point1, Point2);
            return LNA.GetAngle2D(new MTypes.Math.Line(Point1, Point3));
        }
    }
}
