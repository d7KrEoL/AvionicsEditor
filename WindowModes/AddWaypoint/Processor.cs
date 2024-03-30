using AvionicsEditor.MTypes.Map.Pins;
using AvionicsEditor.MTypes.Map.Wpt;
using AvionicsEditor.MTypes.Math;
using BruTile.Wms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AvionicsEditor.WindowModes.AddWaypoint
{
    public static class Processor
    {
        private static MTypes.Math.PolyLine WaypointsPoly = new MTypes.Math.PolyLine(System.Drawing.Color.Black);
        private static MTypes.Map.Lines.Polyline WaypointsPolyLayer;
        private static List<MTypes.Map.Pins.LabelPin> TextBox { get; set; } = new List<MTypes.Map.Pins.LabelPin>();
        public static void OnUpdate(MainWindow MWindow)
        {
            var pos = MWindow.MainMap.GetMouseMapCoords(MWindow);
            double Z = 100;
            double.TryParse(MWindow.Z.Text, out Z);
            if (Z > 800) Z = 800;
            else if (Z == 0) Z = Database.ZFile.GetPosZ(pos.X, pos.Y);
            //var point1 = new MTypes.Math.Vector3(pos.X, pos.Y, 0d);

            CheckCurrentStage();
            //if (GVars.CurrentModeStage == 0) ClearPoly(MWindow);
            UnRenderWaypoints(MWindow);
            RenderWaypoints(new MTypes.Math.Vector3(pos.X, pos.Y, Z), MWindow);

            if (GVars.CurrentModeStage > 1)
            {
                UnRenderPoly(MWindow);
                RenderPoly(MWindow);
            }
            else if (GVars.CurrentModeStage > 0)
            {
                RenderPoly(MWindow);
            }

                GVars.AddCurrentModeStage();
            //MWindow.SetDebugText($"wp.points = {WaypointsPoly.Points.Count} | Waypoints {GVars.Waypoints.Count} | Mode: {GVars.CurrentModeStage}");
        }

        public static void OnUpdate(MTypes.Math.Vector3 pos)
        {
            UnRenderWaypoints(GVars.MWindow);
            RenderWaypoints(pos, GVars.MWindow);
            UnRenderPoly(GVars.MWindow);

            if (GVars.CurrentModeStage > 1)
            {
                UnRenderPoly(GVars.MWindow);
                RenderPoly(GVars.MWindow);
            }
            else if (GVars.CurrentModeStage > 0)
            {
                RenderPoly(GVars.MWindow);
            }

            GVars.AddCurrentModeStage();
            
        }

        public static void OnUpdateDelete(MainWindow MWindow)
        {
            if (GVars.CurrentModeStage == 0) return;
            if (GVars.CurrentModeStage == 1)
            {
                ClearWaypoints();
                UnRenderWaypoints(MWindow);
                UnRenderPoly(MWindow);
                GVars.SubCurrentModeStage();
                return;
            }
            DeleteWaypoint(MWindow, GVars.CurrentModeStage-1);
            WaypointsPoly.Points.RemoveAt(GVars.CurrentModeStage-1);
            UnRenderWaypoints(MWindow);
            GVars.SubCurrentModeStage();
            UnRenderPoly(MWindow);
            //MWindow.SetDebugText($"wp.points = {WaypointsPoly.Points.Count} | Waypoints {GVars.Waypoints.Count} | Mode: {GVars.CurrentModeStage}");
            
            RenderWaypoints(MWindow);
            if (GVars.CurrentModeStage > 0) RenderPoly(MWindow);
        }

        public static void ClearWaypoints()
        {
            GVars.RemoveWPTs(MTypes.Map.Pins.PinType.MovePoint);
            WaypointsPoly.Clear();
        }

        public static void UpdateRender(MainWindow MWindow)//, int WaypointIndex, MTypes.Math.Vector3 NewPos)
        {
            if (GVars.CurrentModeStage > 0)
            {
                UpdateWaypoints(MWindow);
                //UnRenderWaypoints(MWindow);
                UnRenderPoly(MWindow);
                /*RenderWaypoints(MWindow);*/
                if (WaypointsPoly.Points.Count > 1) RenderPoly(MWindow);
            }
        }

        private static void UpdateWaypoints(MainWindow MWindow)
        {
            WaypointsPoly.Clear();
            GVars.RemovePins(PinType.ScheaticFigure);
            TextBox.Clear();
            for (int i = 0; i < GVars.Waypoints.Count; i++)
            {
                WaypointsPoly.AddPoint(GVars.Waypoints[i].Position);
                TextBox.Add(new LabelPin($"{WaypointsPoly.Points.Count}\n{GVars.Waypoints[i].Position.Z}", GVars.Waypoints[i].Position.X, GVars.Waypoints[i].Position.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, 0, LabelPin.ModelType.Circle));
                GVars.AddPin(TextBox[i]);
            }
        }

        private static void RenderWaypoints(MTypes.Math.Vector3 pos, MainWindow MWindow)
        {
            //MTypes.Map.Wpt.Waypoint wpt = new MTypes.Map.Wpt.Waypoint(GVars.Waypoints.Count.ToString(), new MTypes.Math.Vector3(pos.X, pos.Y, 300), MWindow.MainMap, MTypes.Map.Pins.PinType.MovePoint);//TODO Z POS
            //Mapsui.MPoint pos = new Mapsui.MPoint(posVec.X, posVec.Y);
            //WaypointsPoly.AddPoint(new MTypes.Math.Vector3(pos.X, pos.Y, Z));//TODO Z POS
            WaypointsPoly.AddPoint(pos);

            TextBox.Add(new LabelPin($"{WaypointsPoly.Points.Count}\n{pos.Z}", pos.X, pos.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, 0, LabelPin.ModelType.Circle));
            GVars.AddPin(TextBox[TextBox.Count-1]);
            //GVars.AddWPT(wpt);
            RenderWaypoints(MWindow);
        }

        private static void RenderWaypoints(MainWindow MWindow)
        {
            int i = 0;
            foreach (MTypes.Math.Vector3 pos in WaypointsPoly.Points)
            {
                MTypes.Map.Wpt.Waypoint wpt = new MTypes.Map.Wpt.Waypoint(GVars.Waypoints.Count.ToString(), new MTypes.Math.Vector3(pos.X, pos.Y, pos.Z), MWindow.MainMap, MTypes.Map.Pins.PinType.MovePoint);//TODO Z POS
                GVars.AddWPT(wpt);
                TextBox.Add(new LabelPin($"WPT:{i}\nAlt:{pos.Z}", pos.X, pos.Y, MTypes.Map.Pins.PinType.ScheaticFigure, GVars.MWindow.MainMap, 0, LabelPin.ModelType.Circle));
                GVars.AddPin(TextBox[TextBox.Count - 1]);
                i++;
            }
        }

        private static void UnRenderWaypoints(MainWindow MWindow)
        {
            if (WaypointsPoly.PolyLayer != null) GVars.RemoveWPTs(MTypes.Map.Pins.PinType.MovePoint);//MWindow.MainMap.MapVar.Layers.Remove(WaypointsPoly.PolyLayer);
            GVars.RemovePins(PinType.ScheaticFigure);
            TextBox.Clear();
        }

        private static void RenderPoly(MainWindow MWindow)
        {
            if (WaypointsPoly.Points.Count > 1)
            {
                WaypointsPolyLayer = new MTypes.Map.Lines.Polyline("Waypoints", WaypointsPoly, MWindow.MainMap, MTypes.Map.Pins.PinType.MovePoint);
            }
        }

        private static void UnRenderPoly(MainWindow MWindow)
        {
            if (WaypointsPolyLayer == null) return;
            MWindow.MainMap.MapVar.Layers.Remove(WaypointsPolyLayer.PinLayer);
            WaypointsPolyLayer = null;
        }

        private static void DeleteWaypoint(MainWindow MWindow, MTypes.Math.Vector3 pos)
        {
            GVars.RemoveWPT(pos);
        }

        private static void DeleteWaypoint(MainWindow MWindow, int index)
        {
            GVars.RemoveWPT(index);
        }

        private static void CheckCurrentStage()
        {
            GVars.SetCurrentModeStage(GVars.Waypoints.Count);
        }
    }
}
