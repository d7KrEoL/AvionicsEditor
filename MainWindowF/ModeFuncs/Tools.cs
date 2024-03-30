using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MainWindowF.ModeFuncs
{
    public static class Tools
    {
        public static void ClearCurrentMode(MainWindow MWindow)
        {
            switch (GVars.CurrentMode)
            {
                case Processing.EditorMode.None:
                    break;
                case Processing.EditorMode.AddWPT:
                    break;
                case Processing.EditorMode.AddTarget:
                    break;
                case Processing.EditorMode.EditPoints:
                    break;
                case Processing.EditorMode.AddAproach:
                    break;
                case Processing.EditorMode.AddGlidepath:
                    break;
                case Processing.EditorMode.AddDRPM:
                    break;
                case Processing.EditorMode.AddBRDM:
                    break;
                case Processing.EditorMode.AddRunway:
                    break;
                case Processing.EditorMode.AddBeacon:
                    break;
                case Processing.EditorMode.CalcDistance:
                    WindowModes.CalcDistance.Processor.OnClear(MWindow);
                    break;
                case Processing.EditorMode.CalcAngle:
                    WindowModes.CalcAngle.Processor.OnClear(MWindow);
                    break;
                case Processing.EditorMode.GetPos:
                    break;
                default:
                    break;
            }
        }
        public static void SetMode_None(MainWindow MWindow)
        {
            //if (GVars.CurrentMode == Processing.EditorMode.CalcDistance) WindowModes.CalcDistance.Processor.OnClear(MWindow);
            ClearCurrentMode(MWindow);
            GVars.SetCurrentMode(Processing.EditorMode.None, 0);
            GVars.SetCurrentModeStage(0);
            MWindow.SetDebugText("No tools selected");
            MWindow.SetAllColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
        }
    }
}
