using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML1
{
    public static class Misc
    {
        public static readonly Random rng;
        public static readonly char ESC = (char)27;
        static Misc()
        {
            rng = new Random((int)DateTime.Now.Ticks);
        }

        public static int GetInt(int exMax)
        {
            return rng.Next(1, exMax);
        }

        public static int[] GetRandomUniqueIntegers(int count, int range)
        {
            return Enumerable.Range(0, range).OrderBy(t => rng.Next()).Take(count).ToArray(); //Got it from StackOverflow :)
        }

        public static List<int> GetRandomUniqueIntegersList(int count, int range)
        {
            return Enumerable.Range(0, range).OrderBy(t => rng.Next()).Take(count).ToList(); //Got it from StackOverflow :)
        }
    }
}
