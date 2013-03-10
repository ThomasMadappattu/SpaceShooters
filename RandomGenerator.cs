using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter
{
    class RandomGenerator
    {
        
        
        static Random randGen = new Random(3000);
        public static int getNextRandomNumber(int Num)
        {
            return (int)(randGen.Next(Num));
           
        }
        public static int getNextRandomNumber(int min,int  max)
        {
            return (int)(randGen.Next(min,max));

        }
       

    }
}
