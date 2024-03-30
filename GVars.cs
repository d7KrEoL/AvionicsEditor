using AvionicsEditor.Map;
using AvionicsEditor.MTypes.Map.Pins;
using AvionicsEditor.MTypes.Map.Wpt;
using AvionicsEditor.MTypes.Math;
using Mapsui.Layers;
using Mapsui.UI.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor
{
    public static class GVars
    {
        public static MainWindow MWindow { get; private set; }
        public static void SetMainWindow(object sender)
        {
            MWindow = (MainWindow)sender;
        }
        public static List<PinBase> Pins { get; private set; } = new List<PinBase>();
        public static List<WPT> Waypoints { get; private set; } = new List<WPT>();
        public static void AddPin(PinBase pin)
        {
            Pins.Add(pin);
        }
        public static void ReloadPins()
        {
            List<PinBase> PinsReload = Pins.ToList<PinBase>();
            foreach (var pin in Pins)
            {
                pin.DeletePin();
            }
            Pins.Clear();
            foreach (var pin in PinsReload)
            {
                pin.CreatePin(pin.Name, pin.Position.X, pin.Position.Y, pin.Type);
                AddPin(pin);
            }
        }
        public static void RemovePin(PinBase pin)
        {
            Pins.Remove(pin);
        }
        public static void RemovPins(List<PinBase> pins)
        {
            foreach (PinBase pin in pins)
            {
                Pins.Remove(pin);
            }
        }
        public static void RemovePins(PinType pinType)
        {
            List<PinBase> DelPins = new List<PinBase>();
            foreach (PinBase pin in Pins)
            {
                if (pin.Type == pinType) { DelPins.Add(pin); }
            }
            foreach (PinBase pin in DelPins)
            {
                pin.DeletePin();
                Pins.Remove(pin);
            }
        }
        public static void AddWPT(WPT Waypoint)
        {
            Waypoints.Add(Waypoint);
        }
        public static WPT GetWPT(Vector3 Pos)
        {
            for (int i = 0; i < Waypoints.Count; i++)
            {
                if (Waypoints[i].Position == Pos) return Waypoints[i];
            }
            return null;
        }
        public static WPT FindNearestWPT(Vector3 Pos)
        {
            double mindist = 1000;
            WPT res = null;
            for (int i = 0; i < Waypoints.Count; i++)
            {
                if (Waypoints[i].Position.FindDistance(Pos) < mindist)
                {
                    mindist = Waypoints[i].Position.FindDistance(Pos);
                    res = Waypoints[i];
                }
            }
            return res;
        }
        public static void EditWPT(WPT Waypoint, Vector3 Pos)
        {
            int idx = GetWPTIdx(Waypoint);
            if (idx > -1)
            {
                Waypoints[idx].Position = Pos;
                string wptname = Waypoints[idx].WPTLayer.Name;
                Waypoints[idx].Delete();
                Waypoints[idx].CreateWPT(wptname, Pos, Waypoints[idx].ParentMap, Waypoints[idx].PType);
            }
            //if (Waypoints.Contains(Waypoint)) Waypoints.Find(x => x.Position == Waypoint.Position).Position = Pos;
            //else Console.WriteLine("blet");
        }
        public static int GetWPTIdx(WPT WPT)
        {
            for (int i = 0; i < Waypoints.Count; i++)
            {
                if (Waypoints[i].Position.IsEqual(WPT.Position)) return i;
            }
            return -1;
        }
        public static WPT FindNearestWPT(Vector2 Pos)
        {
            double mindist = 1000;
            WPT res = null;
            for (int i = 0; i < Waypoints.Count; i++)
            {
                if (Waypoints[i].Position.FindDistance(Pos) < mindist)
                {
                    mindist = Waypoints[i].Position.FindDistance(Pos);
                    res = Waypoints[i];
                }
            }
            return res;
        }
        public static void RemoveWPT(WPT Waypoint)
        {
            Waypoint.Delete();
            Waypoints.Remove(Waypoint);
        }
        public static void RemoveWPT(Vector3 Pos)
        {
            for (int i = 0; i < Waypoints.Count; i++)
            {
                if (Waypoints[i].Position.Equals(Pos))
                {
                    RemoveWPT(Waypoints[i]);
                    return;
                }
            }
        }
        public static void RemoveWPT(int index)
        {
            if (index > Waypoints.Count || index < 0) return;
            Waypoints[index].Delete();
            Waypoints.Remove(Waypoints[index]);
        }
        public static void RemovWPTs(List<WPT> Waypoints)
        {
            foreach (WPT WP in Waypoints)
            {
                Waypoints.Remove(WP);
            }
        }
        public static void RemoveWPTs(PinType WPType)
        {
            List<WPT> DelPins = new List<WPT>();
            foreach (WPT WP in Waypoints)
            {
                if (WP.PType == WPType) { DelPins.Add(WP); }
            }
            foreach (WPT WP in DelPins)
            {
                WP.Delete();
                Waypoints.Remove(WP);
            }
        }
        public static Processing.EditorMode CurrentMode { get; private set; } = Processing.EditorMode.None;
        public static void SetCurrentMode(Processing.EditorMode Mode, int ModeStage)
        {
            CurrentMode = Mode;
            CurrentModeStage = ModeStage;
        }
        public static int CurrentModeStage { get; private set; } = 0;
        public static void SetCurrentModeStage(int Stage)
        {
            CurrentModeStage = Stage;
        }
        public static void AddCurrentModeStage()
        {
            CurrentModeStage++;
        }
        public static void SubCurrentModeStage()
        {
            CurrentModeStage--;
        }
    }
}
