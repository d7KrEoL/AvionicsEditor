using BruTile.Wms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AvionicsEditor.WindowModes.EditPoints
{
    public static class Processor
    {
        private static MTypes.Map.Wpt.WPT CurPoint;
        private static MTypes.Math.Vector3 StartPos;
        public static void OnStart(MainWindow MWindow)
        {
            GVars.RemovePins(MTypes.Map.Pins.PinType.SelectedPoint);
            var pos = MWindow.MainMap.GetMouseMapCoords(MWindow);
            double ZPos = 0;
            double.TryParse(GVars.MWindow.Z.Text, out ZPos);

            CurPoint = GVars.FindNearestWPT(new MTypes.Math.Vector2(pos.X, pos.Y));
            if (CurPoint == null) return;
            StartPos = CurPoint.Position;
            GVars.AddCurrentModeStage();
            var pin = new MTypes.Map.Pins.DefaultPin("Edit", pos.X, pos.Y, MTypes.Map.Pins.PinType.SelectedPoint, MWindow.MainMap);
            GVars.AddPin(pin);
            //AddWaypoint.Processor.UpdateRender(MWindow);

            //if(ZPos != StartPos.Z) if (MSG_ChangeZ()) MWindow.Z.Text = StartPos.Z.ToString();
            MWindow.Z.Text = StartPos.Z.ToString();
        }

        public static void OnUpdate(MainWindow MWindow)
        {
            var pos = MWindow.MainMap.GetMouseMapCoords(MWindow);
            if (GVars.CurrentModeStage > 0)
            {
                GVars.RemovePins(MTypes.Map.Pins.PinType.SelectedPoint);
                double ZPos = 0;
                double.TryParse(GVars.MWindow.Z.Text, out ZPos);
                GVars.EditWPT(CurPoint, new MTypes.Math.Vector3(pos.X, pos.Y, ZPos));
                var pin = new MTypes.Map.Pins.DefaultPin("Edit", pos.X, pos.Y, MTypes.Map.Pins.PinType.SelectedPoint, MWindow.MainMap);
                GVars.AddPin(pin);
                AddWaypoint.Processor.UpdateRender(MWindow);
            }
        }

        public static void DeletePoint(MainWindow MWindow)
        {
            GVars.RemoveWPT(GVars.GetWPTIdx(CurPoint));
            AddWaypoint.Processor.UpdateRender(MWindow);
            GVars.RemovePins(MTypes.Map.Pins.PinType.SelectedPoint);
            GVars.SetCurrentModeStage(0);
        }

        public static void OnStop(MainWindow MWindow, bool Cancel)
        {
            if (Cancel)
            {
                GVars.EditWPT(CurPoint, StartPos);   
            }
            else
            {
                double ZPos = 0;
                double.TryParse(GVars.MWindow.Z.Text, out ZPos);
                GVars.EditWPT(CurPoint, new MTypes.Math.Vector3(CurPoint.Position.X, CurPoint.Position.Y, ZPos));
            }

            GVars.RemovePins(MTypes.Map.Pins.PinType.SelectedPoint);
            AddWaypoint.Processor.UpdateRender(MWindow);
            GVars.SetCurrentModeStage(0);
        }

        public static void OnStop(MainWindow MWindow)
        {
            var pos = MWindow.MainMap.GetMouseMapCoords(MWindow);
            double ZPos = 0;
            double.TryParse(GVars.MWindow.Z.Text, out ZPos);
            if (ZPos == 0) if (MSG_GroundZ())
            {
                    ZPos = Database.ZFile.GetPosZ(new MTypes.Math.Vector2(pos.X, pos.Y));
                    MWindow.Z.Text = ZPos.ToString();
            }
            GVars.EditWPT(CurPoint, new MTypes.Math.Vector3(pos.X, pos.Y, ZPos));
            GVars.RemovePins(MTypes.Map.Pins.PinType.SelectedPoint);
            AddWaypoint.Processor.UpdateRender(MWindow);
            GVars.SetCurrentModeStage(0);
        }

        private static bool MSG_ChangeZ()
        {
            string messageBoxText = "You have different altitude comparing to a point\nDo you want to change YOUR altitude as it is in a point?";
            string caption = "Avionics Editor - Edit Waypoint";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            switch(result)
            {
                case MessageBoxResult.Yes:
                    return true;
                /*case MessageBoxResult.No:
                    return false;
                case MessageBoxResult.None:
                    return false;*/
            }
            return false;
        }

        private static bool MSG_GroundZ()
        {
            string messageBoxText = "You have entered 0 as altitude\nDo you want to set ground level's altitude [Yes] or leave a 0 as it is now [No]?";
            string caption = "Avionics Editor - Edit Waypoint";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    return true;
                    /*case MessageBoxResult.No:
                        return false;
                    case MessageBoxResult.None:
                        return false;*/
            }
            return false;
        }
    }
}
