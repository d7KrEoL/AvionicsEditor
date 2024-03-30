using AvionicsEditor.MTypes.Map.Pins;
using Mapsui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AvionicsEditor.WindowModes.GetPos
{
    public static class Processor
    {
        public static void OnUpdate(MainWindow MWindow)
        {
            //var pos = MainMap.MapVar.Navigator.Viewport.ScreenToWorld(new MPoint(System.Windows.Input.Mouse.GetPosition(this).X, System.Windows.Input.Mouse.GetPosition(this).Y));
            var pos = MWindow.MainMap.GetMouseMapCoords(MWindow);
            var getZ = Database.ZFile.GetPosZ(new MTypes.Math.Vector2(pos.X, pos.Y));
            MWindow.SetDebugText($"Selected point pos [X: {(int)pos.X} | Y: {(int)pos.Y} | Z: {getZ}]");
            Clipboard.SetText($"{(int)pos.X} {(int)pos.Y} {getZ.ToString().Replace(",", ".")}");

            GVars.RemovePins(PinType.SelectedPoint);
            var ResPin1 = new DefaultPin($"Pos point", pos.X, pos.Y, PinType.SelectedPoint, MWindow.MainMap);
            GVars.AddPin(ResPin1);
        }
    }
}
