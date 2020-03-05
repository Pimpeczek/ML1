using System;
using System.Collections.Generic;
using System.IO;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
namespace ML1
{
    class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task(1000, 10000, 10000);
            int bestPossibleScore = task.MaxSize + task.MaxWeight;
            int goodScore = bestPossibleScore * 90 / 100;
            Population population = new Population(task, 1, 0);
            int temp;
            int pop;
            int curBest = 0;
            int curBestTime;
            int[] sizes = new int [4]{ 100, 20, 20, 20 };
            int totalRounds = sizes[0] * sizes[1] * sizes[2] * sizes[3];
            int[,,,][] data = new int[sizes[0], sizes[1], sizes[2], sizes[3]][];
            int progress = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            StreamWriter sw = new StreamWriter("data.txt");
            string writeDtata;
            for (int p = 1; p <= sizes[0]; p += 1)
            {
                sw = new StreamWriter($"data_{p}.txt");
                writeDtata = "";
                for (int t = 1; t <= sizes[1]; t += 1)
                {
                    for (int c = 0; c < sizes[2]; c += 1)
                    {
                        for (int m = 0; m < sizes[3]; m += 1)
                        {
                            curBest = 0;
                            curBestTime = 0;
                            data[p-1, t-1, c, m] = new int[2];
                            for (int i = 0; i < 100 && curBest < goodScore; i++)
                            {
                                pop = p * 10;
                                population = new Population(task, pop, 0)
                                {
                                    TournamentSize = pop * t / sizes[1],
                                    CrossoverRate = (double)c / sizes[2],
                                    MutationRate = (double)m / sizes[3] / 10
                                };
                                population.Tournament();
                                temp = population.EvaluateBest();
                                if(temp > curBest)
                                {
                                    curBest = temp;
                                    curBestTime = i;
                                }
                            }
                            data[p-1, t-1, c, m][0] = curBest;
                            data[p-1, t-1, c, m][1] = curBestTime;
                            progress++;
                            writeDtata += $"{p};{t};{c};{m};{curBest};{curBestTime}\n";
                            //Console.WriteLine($"Acc: {(double)curBest/bestPossibleScore} | Tim: {curBestTime}\n Pop: {p * 4} | Trs: {population.TournamentSize} | Crs: {population.CrossoverRate} | Mut: {population.MutationRate}\n Prs: {(double)progress / 2000000}");
                        }
                        Console.WriteLine($"Prs: {(double)progress * 100 / totalRounds}\n P: {p} | T: {t} | C: {c}\n Time: {stopwatch.ElapsedMilliseconds}");
                    }
                    
                }
                sw.WriteLine(writeDtata);
                sw.Close();
                
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
