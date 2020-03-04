using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace ML1
{
    class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task("test");
            Population population = new Population(task.ItemCount, 100);
            for(int i = 0; i < 100; i++)
            {
                Console.WriteLine(population.Individuals[i][0]);
            }

            Console.ReadKey(true);
        }

        static void Generate(int objectCount, int maxWeight, int maxSize, string fileName)
        {
            Task task = new Task(objectCount, maxSize, maxWeight);
            task.Save(fileName);
        }
    }

    

    

    
}
