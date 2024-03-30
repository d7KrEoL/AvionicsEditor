using AvionicsEditor.MTypes.Map.Pins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AvionicsEditor.MainWindowF.ModeFuncs.UpdateMode
{
    public static class Processor
    {
        public static void OnModeChanged(MainWindow MWindow, Processing.EditorMode mode)
        {
            GVars.RemovePins(PinType.SelectedPoint);
            if (GVars.CurrentMode == mode)
            {
                Tools.SetMode_None(MWindow);
                return;
            }

            //if (GVars.CurrentMode == Processing.EditorMode.CalcDistance) WindowModes.CalcDistance.Processor.OnClear(this);
            Tools.ClearCurrentMode(MWindow);
            //New mode set
            switch (mode)
            {
                case Processing.EditorMode.None:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    break;
                case Processing.EditorMode.AddWPT:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    MWindow.BInst_AddWPT.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Waypoint]: Add new waypoint");
                    break;
                case Processing.EditorMode.AddTarget:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    //BInst_AddTGT.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Waypoint]: Add new target");
                    break;
                case Processing.EditorMode.EditPoints:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    MWindow.BInst_EditWPT.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Waypoint]: Edit any waypoint");
                    break;
                case Processing.EditorMode.AddAproach:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    //BInst_APR.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Airport]: Add new approach");
                    break;
                case Processing.EditorMode.AddGlidepath:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    //BInst_GLD.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Airport]: Add new glidepath");
                    break;
                case Processing.EditorMode.AddDRPM:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    //BInst_DRDM.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Airport]: Add new drpm");
                    break;
                case Processing.EditorMode.AddBRDM:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    //BInst_BRDM.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Airport]: Add new brdm");
                    break;
                case Processing.EditorMode.AddRunway:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    //BInst_RW.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Airport]: Add new runway");
                    break;
                case Processing.EditorMode.AddBeacon:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    //BInst_BCN.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Beacons]: Add new beacon");
                    break;
                case Processing.EditorMode.CalcDistance:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    MWindow.BInst_Dist.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Distance]: Select start and end points");
                    break;
                case Processing.EditorMode.CalcAngle:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    MWindow.BInst_Angle.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Angle]: Select starting point and two directions");
                    /*TEST +PASSED
                    MTypes.Math.Line LNAB = new MTypes.Math.Line(new MTypes.Math.Vector3(0, 0, 0), new MTypes.Math.Vector3(43, 0, 0));
                    MTypes.Math.Line LNAC = new MTypes.Math.Line(new MTypes.Math.Vector3(0, 0, 0), new MTypes.Math.Vector3(9.34884, 40.9463, 0));
                    
                    SetDebugText($"[Angle]: 42, 53, 43: {LNAB.GetAngle2D(LNAC)}");*/
                    break;
                case Processing.EditorMode.GetPos:
                    MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    MWindow.BInst_Pos.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 100, 0));
                    MWindow.SetDebugText("[Pos coords]: Select point on map");
                    break;
                default:
                    break;
            }
            GVars.SetCurrentMode(mode, 0);
        }
    }
}
