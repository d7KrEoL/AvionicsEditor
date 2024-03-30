using AvionicsEditor.MTypes.Map.Wpt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.Files_IO.Writer
{
    public static class FlightPlan
    {
        public static void Save(string Path)
        {
            using(StreamWriter sw = new StreamWriter(Path, false))
            {
                foreach(string line in GetAllWaypoints())
                {
                    sw.WriteLine(line);
                }
                sw.Close();
            }
        }

        private static string[] GetAllWaypoints()
        {
            string[] Res = new string[GVars.Waypoints.Count];
            for (int i = 0; i < GVars.Waypoints.Count; i++)
            {
                Res[i] = $"{{Waypoint={i},PosX={GVars.Waypoints[i].Position.X},PosY={GVars.Waypoints[i].Position.Y},PosZ={GVars.Waypoints[i].Position.Z}}}";
            }
            return Res;
        }
    }
}
