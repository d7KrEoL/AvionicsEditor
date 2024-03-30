using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MainWindowF.MapControl.OnZoom
{
    public static class CalcDistance
    {
        public static async void DoTask(MainWindow MWindow, bool IsZooming)
        {
            if (IsZooming)
            {
                WindowModes.CalcDistance.Processor.UnRenderPoly();
                WindowModes.CalcDistance.Processor.RenderPoly(false);
                await Task.Delay(360);
            }
            else
            {
                await Task.Delay(100);  
            }
            Console.WriteLine("DONE!");
            WindowModes.CalcDistance.Processor.OnUpdate(MWindow, false);
        }
    }
}
