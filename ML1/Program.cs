using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using ML1_Lib;
namespace ML1
{
    class Program
    {
        //Mut 0.002 - 0.006
        //Crs 0.8 - 1.0
        //Tur 0.2 - 0.4
        //Pop 100 - 1000
        
        static int taskCount = 1;
        static int populationCount = 1;
        static double[] mutationBorders = { 0.001, 0.004 };
        static double[] crossoverBorders = { 0.8, 1 };
        static double[] tournamenBorders = { 0.2, 0.4 };
        static int[] populationBorders = { 100, 1000 };
        static int[] itemCountBorders = { 1000, 1000 };
        static int[] sizeBorders = { 10000, 10000 };
        static int[] weightBorders = { 10000, 10000 };
        static int density = 10;

        static void Main(string[] args)
        {

            Task[] tasks = new Task[10];

            Random rng = new Random();
            Population population;
            for(int t = 0; t < taskCount; t++)
            {
                //tasks[t] = new Task(1000, 10000, 10000);
                tasks[t] = new Task("testTask.task");
            }

            int[] popRange = new int[density];
            double[] turRange = new double[density];
            double[] crsRange = new double[density];
            double[] mutRange = new double[density];
            int tPop = (populationBorders[1] - populationBorders[0]) / (density);
            double tTur = (tournamenBorders[1] - tournamenBorders[0]) / (density);
            double tCrs = (crossoverBorders[1] - crossoverBorders[0]) / (density);
            double tMut = (mutationBorders[1] - mutationBorders[0]) / (density);

            int tempScore;
            int bestScore = int.MinValue;
            int bestPop;
            double bestTur;
            double bestCrs;
            double bestMut;

            for (int i = 0; i < density; i++)
            {
                popRange[i] = populationBorders[0] + tPop * i;
                turRange[i] = tournamenBorders[0] + tTur * i;
                crsRange[i] = crossoverBorders[0] + tCrs * i;
                mutRange[i] = mutationBorders[0] + tMut * i;

            }

            int counter = 0;
            for (int pop = 9; pop < density; pop++)
            {
                for (int tur = 0; tur < density; tur++)
                {
                    for (int crs = 0; crs < density; crs++)
                    {
                        for (int mut = 0; mut < density; mut++)
                        {
                            for (int t = 0; t < taskCount; t++)
                            {
                                
                                for (int p = 0; p < populationCount; p++)
                                {
                                    counter++;
                                    population = new Population(tasks[t], popRange[pop], (int)(popRange[pop] * turRange[tur]), crsRange[crs], mutRange[mut], 0.2, p * 13 + 13);
                                    for(int i = 999; i >=0; i--)
                                    {
                                        if((tempScore = population.CreateNewGeneration()) > bestScore)
                                        {
                                            bestScore = tempScore;
                                            bestPop = popRange[pop];
                                            bestTur = turRange[tur];
                                            bestCrs = crsRange[crs];
                                            bestMut = mutRange[mut];
                                            Console.SetCursorPosition(0,1);
                                            Console.WriteLine($"Score: {bestScore}     \n Pop: {bestPop}     \n Tur: {bestTur}  \n Crs: {bestCrs}  \n Mut: {bestMut}  ");
                                        }
                                    }

                                }
                                Console.SetCursorPosition(0, 0);
                                Console.Write(counter);
                            }
                        }
                        
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
