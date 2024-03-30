using BruTile.Wms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.Database
{
    public static class ZFile
    {
        public static double GetPosZ(MTypes.Math.Vector2 position)
        {
            MTypes.Math.RInt RPosX = new MTypes.Math.RInt(position.X);
            MTypes.Math.RInt RPosY = new MTypes.Math.RInt(position.Y);
            return ParseFindZ(RPosX, RPosY);
        }
        public static double GetPosZ(double X, double Y)
        {
            MTypes.Math.RInt RPosX = new MTypes.Math.RInt(X);
            MTypes.Math.RInt RPosY = new MTypes.Math.RInt(Y);
            return ParseFindZ(RPosX, RPosY);
        }

        private static double ParseFindZ(MTypes.Math.RInt PosX, MTypes.Math.RInt PosY)
        {
            string[] Tmp = new string[3];
            int X, Y;
            double Z;
            int i = 0;
            foreach (var Str in Resources.Images.GroundLevels.ToString().Split(new string[] { "\n" }, StringSplitOptions.None))
            {
                i++;
                string S = Str.Replace(" ", "");
                Tmp = S.Split(',');
                if (Tmp.Length < 2 || Tmp.Length > 3) continue;
                int.TryParse(Tmp[0], out X);
                int.TryParse(Tmp[1], out Y);
                Tmp[2] = Tmp[2].Replace(".", ",");
                double.TryParse(Tmp[2], out Z);
                if (X == PosX.Value && Y == PosY.Value)
                {
                    Z = System.Math.Ceiling(Z);
                    return Z;
                }
            }
            return 404d;
        }
    }
}
