using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace ML1
{
public class Task
{
public int[,] Items { get; protected set; }

public int BestPossibleScore { get; protected set; }
public int ItemCount
{
    get
    {
        if (Items == null)
            return 0;
        return Items.GetLength(0);
    }
}

public int MaxSize { get; protected set; }
public int MaxWeight { get; protected set; }

public string Description
{
    get
    {
        return $"Task:" +
            $"\n Item count: {$"{ItemCount}".PadLeft(6)}" +
            $"\n   Max size: {$"{MaxSize}".PadLeft(6)}" +
            $"\n Max weight: {$"{MaxWeight}".PadLeft(6)}";
    }
}

public Task(string fileName)
{
    Load(fileName);
}

public Task(int itemCount, int maxSize, int maxWeight)
{
    MaxSize = maxSize;
    MaxWeight = maxWeight;
    BestPossibleScore = MaxSize + MaxWeight;
    PopulateItems(itemCount);
}


protected void PopulateItems(int itemCount)
{
    Items = new int[itemCount, 3];
    int maxPackageSize = MaxSize * 10 / itemCount;
    int maxPackageWeight = MaxWeight * 10 / itemCount;
    int maxPackagePrice = itemCount;
    int totalSize;
    int totalWeight;
    do
    {
        totalSize = 0;
        totalWeight = 0;
        for (int i = itemCount - 1; i >= 0; i--)
        {
            totalSize += (Items[i, 0] = Misc.GetInt(maxPackageSize));
            totalWeight += (Items[i, 1] = Misc.GetInt(maxPackageWeight));
            totalWeight += (Items[i, 2] = Misc.GetInt(maxPackagePrice));
        }
    } while (totalSize <= maxPackageSize || totalWeight <= maxPackageWeight);
}

public void Save(string filename)
{
    int count = ItemCount;
    using (StreamWriter sw = new StreamWriter($"{filename}.task"))
    {
        sw.WriteLine($"{count}{Misc.ESC}{MaxSize}{Misc.ESC}{MaxWeight}");

        for (int i = count - 1; i >= 0; i--)
        {
            sw.WriteLine($"{Items[i, 0]}{Misc.ESC}{Items[i, 1]}");
        }
        sw.Close();
    }
}

public void Load(string filename)
{
    string[] splitted;
    int tempInt;
    using (StreamReader sr = new StreamReader($"{filename}.task"))
    {
        splitted = sr.ReadLine().Split(Misc.ESC);
        if (splitted.Length != 3)
            throw new InvalidDataException($"Wrong format of a file - too short");
        if (!int.TryParse(splitted[0], out tempInt))
            throw new InvalidDataException($"ItemCount: {splitted[0]}");
        Items = new int[tempInt, 2];

        if (!int.TryParse(splitted[1], out tempInt))
            throw new InvalidDataException($"MaxSize: {splitted[1]}");
        MaxSize = tempInt;

        if (!int.TryParse(splitted[2], out tempInt))
            throw new InvalidDataException($"MaxWeight: {splitted[2]}");
        MaxWeight = tempInt;

        tempInt = Items.GetLength(0);

        for (int i = 0; i < tempInt && !sr.EndOfStream; i++)
        {
            splitted = sr.ReadLine().Split(Misc.ESC);
            if (splitted.Length != 2)
                throw new InvalidDataException($"Wrong format of a file - line {i + 1}");

            if (!int.TryParse(splitted[0], out Items[i, 0]))
                throw new InvalidDataException($"Line {i + 1} MaxSize: {splitted[0]}");

            if (!int.TryParse(splitted[1], out Items[i, 0]))
                throw new InvalidDataException($"Line {i + 1} MaxWeight: {splitted[1]}");
        }
    }
    BestPossibleScore = MaxSize + MaxWeight;
}
}
}
