using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace AvionicsEditor.Files_IO.Parser
{
    public static class FlightPlan
    {
        public static void Open(string path)
        {
            ParseFile(LoadFile(path));
        }

        private static string LoadFile(string path)
        {
            string Res;
            try
            {
                using (var sr = new StreamReader(path))
                {
                    Res = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (FileNotFoundException ex)
            {
                Res = ex.Message;
            }
            return Res;
        }

        private static void ParseFile(string filedata)
        {
            string[] Res;
            Res = filedata.Split('\n');
            string[] InfoData;

            int DataType = 0;
            int DataNumber = 0;
            string DataString = string.Empty;
            double x = 0, y = 0, z = 0;
            int OptionalNumber = 0;
            double OptionalDouble = 0;
            string OptionalString = string.Empty;
            for (int i = 0; i < Res.Length; i++)
            {
                //InfoData = Res[i].Split(',');
                LoadDataHight(Res[i].Split(','), ref DataType, ref DataNumber, ref DataString, ref x, ref y, ref z, ref OptionalNumber, ref OptionalDouble, ref OptionalString);
                switch (DataType)
                {
                    case 0:
                        LoadWaypoint(x, y, z);
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
            }
        }

        private static void LoadDataHight(string[] data, ref int DataType, ref int DataNumber, ref string DataString, ref double x, ref double y, ref double z, ref int optn, ref double optd, ref string opts)
        {
            if (data.Length < 1) return;
            string[] dataparsed;
            
            
            for (int i = 0; i < data.Length; i++)
            {
                dataparsed = data[i].Split('=');
                switch (dataparsed[0])
                {
                    case "{Waypoint":
                        DataType = 0;
                        int.TryParse(dataparsed[1].Replace("}", ""), out DataNumber);
                        break;
                    case "{Target":
                        DataType = 1;
                        int.TryParse(dataparsed[1].Replace("}", ""), out DataNumber);
                        break;
                    case "{Airport":
                        DataType = 2;
                        DataString = dataparsed[1].Replace("}", "");
                        break;
                    case "{Beacon":
                        DataType = 3;
                        DataString = dataparsed[1].Replace("}", "");
                        break;
                    case "PosX":
                        double.TryParse(dataparsed[1].Replace("}", ""), out x);
                        break;
                    case "PosY":
                        double.TryParse(dataparsed[1].Replace("}", ""), out y);
                        break;
                    case "PosZ":
                        double.TryParse(dataparsed[1].Replace("}", ""), out z);
                        break;
                    case "ModelID":
                        int.TryParse(dataparsed[1].Replace("}", ""), out optn);
                        break;
                    case "Radius":
                        double.TryParse(dataparsed[1].Replace("}", ""), out optd);
                        break;
                    case "Code":
                        opts = dataparsed[1].Replace("}", "");
                        break;
                }
            }
        }
        private static void LoadWaypoint(double x, double y, double z)
        {
            WindowModes.AddWaypoint.Processor.OnUpdate(new MTypes.Math.Vector3(x, y, z));
        }

        private static void LoadTarget(int waypoint, double x, double y, double z, int modelid)
        {

        }
    }
}
