using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvionicsEditor.MTypes.Math
{
    public class RInt
    {
        public int Value { get; }
        public RInt(double value)
        {
            Value = RoundValue((int)System.Math.Round(value));
        }
        public RInt(int value)
        {
            Value = RoundValue(value);
        }
        private int RoundValue(int Value)
        {
            double Tmp = Value;
            int i = 0, j = 0;
            while (Tmp / 10 != (int)(Tmp / 10))
            {
                Tmp++;
                i++;
            }
            Tmp = Tmp - i;
            while (Tmp / 10 != (int)(Tmp / 10))
            {
                Tmp--;
                j++;
            }
            if (i < j)
            {
                Tmp = Tmp + j + i;
            }
            return (int)Tmp;
            

            /*decimal Bigger, Smaller;
            Bigger = System.Math.Ceiling(decimal.Parse($"{Value}"));
            Smaller = System.Math.Floor(decimal.Parse($"{Value}"));
            if (Value - Smaller < Bigger - Value) return (int)Smaller;
            else return (int)Bigger;*/
            
            /*
            5->10           
            [0]: i=1; 5+1 = 6
            [1]: i=2; 6+1 = 7
            [2]: i=3; 7+1 = 8
            [3]: 4; 8+1 = 9; 0.9 != 0
            [4]: 5; 9+1 = 10; 1 == 1
            ---
            [0] j = 1; i = 5; 10 - 5 = 5 => 5 - 1 = 4; 4/10 = 0.4 != 0
            [2] j = 2; i = 5; 4 - 1 = 3; 3/10 = 0.3 != 0
            [3] j = 3; i = 5; 3 - 1 = 2; 2/10
            [4] 4; 5; 2-1 = 1; 1/10 = 0.1
            [5] 5; 5; 1-1 = 0; 0/10 = 0 == 0
            i = 5; j = 5; 5 == 5;
            val = 10
            
            25->30      
            71->70
            89->90
            80->80
            100->100    
            */
        }
    }
}
