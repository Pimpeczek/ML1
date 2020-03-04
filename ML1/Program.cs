using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace ML1
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        static void Generate(int objectCount, int maxWeight, int maxSize, string fileName)
        {
            
        }
    }

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
    }

    public class Task
    {
        public int ObjectCount
        {
            get
            {
                if (packages == null)
                    return 0;
                return packages.GetLength(0);
            }
        }
        public int MaxSize { get; protected set; }
        public int MaxWeight { get; protected set; }
        

        protected int[,] packages;


        public Task(int objectCount, int maxSize, int maxWeight)
        {
            MaxSize = maxSize;
            MaxWeight = maxWeight;
            
            Populate(objectCount);
        }

        protected void Populate(int count)
        {
            packages = new int[count, 2];
            int maxPackageSize = MaxSize * 10 / count;
            int maxPackageWeight = MaxWeight * 10 / count;
            for (int i = count - 1; i >= 0; i--)
            {
                packages[i, 0] = Misc.GetInt(maxPackageSize);
                packages[i, 1] = Misc.GetInt(maxPackageWeight);
            }
        }

        public void Save(string filename)
        {
            int count = ObjectCount;
            using (StreamWriter sw = new StreamWriter($"{filename}.task"))
            {
                sw.WriteLine($"{count}{Misc.ESC}{MaxSize}{Misc.ESC}{MaxWeight}");

                for (int i = count - 1; i >= 0; i--)
                {
                    sw.WriteLine($"{packages[i, 0]}{Misc.ESC}{packages[i, 0]}");
                }
                sw.Close();
            }
        }
    }
    /*
    public class Package
    {
        public int Size { get; protected set; }
        public int Weight { get; protected set; }
        public Package(int maxSize, int maxWeight)
        {
            Size = Misc.GetInt(maxSize);
            Weight = Misc.GetInt(maxWeight);
        }
    }
    */

}
