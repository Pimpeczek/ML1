using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Diagnostics;
namespace ML1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[1] - nutation tests\n[2] - crossover tests\n[3] - tournament tests\n[3] - population tests");
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D1)
                Calculate(MakePopMut, "mut_graph.png", 0);
            if (key == ConsoleKey.D2)
                Calculate(MakePopCrs, "crs_graph.png", 1);
            if (key == ConsoleKey.D3)
                Calculate(MakePopTur, "tur_graph.png", 2);
            if (key == ConsoleKey.D4)
                Calculate(MakePopPop, "pop_graph.png", 3);

            Console.WriteLine("\n  - DONE! -");
            Console.ReadKey(true);
        }

        static void Calculate(Func<int, Task, int, int, Population> popCreator, string name, int pos)
        {
            Stopwatch stopwatch = new Stopwatch();
            Population[] population;
            Task[] tasks;
            int temp;
            
            int groupSize = 8;
            int taskCount = 8;
            int[] pointBestPrice = new int[taskCount];
            int bestPrice;
            int[] pointWorstPrice = new int[taskCount];
            int worstPrice;
            int generationCount = 100;
            int[] dims = { 10, generationCount };
            int[] tileSize = { 10, 10 };
            int[] imageSize = { tileSize[0] * dims[0], tileSize[1] * dims[1] };
            int maxAverageScore = 0;
            int averageScore;
            int populationsCount = groupSize * taskCount;
            int updateTime = 0;
            Random random = new Random(10);
            population = new Population[populationsCount];
            tasks = new Task[taskCount];
            Bitmap bmp = new Bitmap(imageSize[0], imageSize[1]);
            Graphics graph = Graphics.FromImage(bmp);
            SolidBrush[] brushes = new SolidBrush[256];
            SolidBrush[] badBrushes = new SolidBrush[256];
            int[,] data1 = new int[dims[0], dims[1]];
            for (int br = 0; br < 256; br++)
            {
                brushes[br] = new SolidBrush(Color.FromArgb(br, br, br));
                badBrushes[br] = new SolidBrush(Color.FromArgb(br, 0, 0));
            }

            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = new Task(random.Next(1000, 2000), random.Next(1000, 2000), random.Next(1000, 2000));
                maxAverageScore += tasks[i].BestPossibleScore;
            }

            stopwatch.Restart();
            for (int m = 0; m < dims[0]; m++)
            {
                for (int t = 0; t < taskCount; t++)
                {
                    for (int g = 0; g < groupSize; g++)
                    {
                        population[t * groupSize + g] = popCreator.Invoke(m, tasks[t], g, g * 13);
                    }
                }
                for (int gr = 0; gr < dims[1]; gr++)
                {
                    averageScore = 0;
                    for (int t = 0; t < taskCount; t++)
                    {
                        for (int g = 0; g < groupSize; g++)
                        {
                            temp = population[t * groupSize + g].Tournament();

                            averageScore += temp;
                            if (pointBestPrice[t] < temp)
                                pointBestPrice[t] = temp;
                            if (pointWorstPrice[t] > temp)
                                pointWorstPrice[t] = temp;

                        }

                    }
                    averageScore /= populationsCount;
                    data1[m, gr] = averageScore;
                    if (updateTime + 200 < stopwatch.ElapsedMilliseconds)
                    {
                        Console.WriteLine($"{name} : generation [{gr + 1}/{dims[1]}] | test no. [{m + 1}/{dims[0]}]");
                        updateTime = (int)stopwatch.ElapsedMilliseconds;
                    }

                }

            }
            bestPrice = 0;
            worstPrice = 0;
            for(int i = 0; i < taskCount; i++)
            {
                bestPrice += pointBestPrice[i];
                worstPrice += pointWorstPrice[i];
            }
            bestPrice /= taskCount;
            for (int m = 0; m < dims[0]; m++)
            {
                for (int gr = 0; gr < dims[1]; gr++)
                {
                    if (data1[m, gr] < 0)
                    {
                        graph.FillRectangle(badBrushes[data1[m, gr] * 255 / (worstPrice + 1)], m * tileSize[0], gr * tileSize[1], tileSize[0], tileSize[1]);
                    }
                    else
                    {
                        graph.FillRectangle(brushes[data1[m, gr] * 255 / (bestPrice + 1)], m * tileSize[0], gr * tileSize[1], tileSize[0], tileSize[1]);
                    }
                }
            }
            graph.Save();
            graph.Dispose();
            bmp.Save(name);
        }

        static Population MakePopMut(int m, Task t, int g, int s)
        {
            return new Population(t, 100, (g + 19) * (g + 19) * 13)
            {
                CrossoverRate = 0.9,
                MutationRate = (double)m / 5000,
                TournamentSize = 24
            };
        }
        static Population MakePopCrs(int m, Task t, int g, int s)
        {
            return new Population(t, 100, (g + 19) * (g + 19) * 13)
            {
                CrossoverRate = (double)m/100,
                MutationRate = 0.002,
                TournamentSize = 24
            };
        }
        static Population MakePopTur(int m, Task t, int g, int s)
        {
            return new Population(t, 100, (g + 19) * (g + 19) * 13)
            {
                CrossoverRate = 0.9,
                MutationRate = 0.002,
                TournamentSize = m
            };
        }
        static Population MakePopPop(int m, Task t, int g, int s)
        {
            return new Population(t, m * 5 + 5, (g + 19) * (g + 19) * 13)
            {
                CrossoverRate = 0.9,
                MutationRate = 0.002,
                TournamentSize = m + 1
            };
        }

        static void Generate(int objectCount, int maxWeight, int maxSize, string fileName)
        {
            Task task = new Task(objectCount, maxSize, maxWeight);
            task.Save(fileName);
        }
    }

    

    

    
}
