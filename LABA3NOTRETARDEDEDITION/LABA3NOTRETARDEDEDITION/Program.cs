﻿using System;
using System.Linq;

public class MySet
{
    public int[] elements;
    private int size;

    public int this[int index]
    {
        get { return elements[index]; }
        set
        {
            if (value >= 0)
                elements[index] = value;
            else
                Console.WriteLine("Значение не может быть отрицательным");
        }
    }

    public int Size
    {
        get { return size; }
        set { if (value < 1) { throw new ArgumentException("ERROR"); } size=value; }
    }

    public Production ProductionInfo { get; set; }

    public class Production
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; }
    }

    public class Developer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
    }

    public MySet(int capacity)
    {
        elements = new int[capacity];
        size = 0;
        ProductionInfo = new Production();
    }

    public void Add(int item)
    {
        if (!Contains(item))
        {
            if (size < elements.Length)
            {
                elements[size] = item;
                size++;
            }
            else
            {
                Console.WriteLine("Множество переполнено");
            }
        }
    }

    public bool Contains(int item)
    {
        for (int i = 0; i < size; i++)
        {
            if (elements[i] == item)
            {
                return true;
            }
        }
        return false;
    }

    public void Remove(int item)
    {
        for (int i = 0; i < size; i++)
        {
            if (elements[i] == item)
            {
                for (int j = i; j < size - 1; j++)
                {
                    var tmp = new List<int>(elements);
                    tmp.RemoveAt(j);
                    var element = tmp.ToArray();
                    elements = element;
                }
                size--;
                return;
            }
        }
    }

    public void Display()
    {
        Console.Write("Множество: ");
        foreach (int item in elements)
        {
            Console.Write($"{item}");
        }
        Console.WriteLine();
    }

    public static MySet operator >>(MySet set, int item)
    {
        set.Remove(item);
        return set;
    }
    public static MySet operator <<(MySet set, int item)
    {
        set.Add(item);
        return set;
    }

    public static bool operator >(MySet set1, MySet set2)
    {
        if (set1.size > set2.size)
            return false;

        for (int i = 0; i < set1.size; i++)
        {
            if (!set2.Contains(set1.elements[i]))
                return false;
        }

        return true;
    }
    public static bool operator <(MySet set1, MySet set2)
    {
        if (set1.size > set2.size)
            return false;

        for (int i = 0; i < set1.size; i++)
        {
            if (!set2.Contains(set1.elements[i]))
                return false;
        }

        return true;
    }
    public static bool operator !=(MySet set1, MySet set2)
    {
        if (set1.size == set2.size)
            return false;

        for (int i = 0; i < set1.size; i++)
        {
            if (!set2.Contains(set1.elements[i]))
                return true;
        }

        return false;
    }
    public static bool operator ==(MySet set1, MySet set2)
    {
        if (set1.size > set2.size)
            return false;

        for (int i = 0; i < set1.size; i++)
        {
            if (!set2.Contains(set1.elements[i]))
                return false;
        }

        return true;
    }
    public static MySet operator %(MySet set1, MySet set2)
    {
        MySet intersection = new MySet(Math.Max(set1.size, set2.size));

        foreach (int item in set1.elements)
        {
            if (set2.Contains(item))
            {
                intersection.Add(item);
            }
        }

        return intersection;
    }
}



public static class StatisticOperation
{
    public static int Sum(this MySet set)
    {
        int sum = 0;
        for (int i = 0; i < set.Size; i++)
        {
            sum += set[i];
        }
        return sum;
    }

    public static int Difference(this MySet set)
    {
        if (set.Size == 0)
            return 0;

        int max = set[0];
        int min = set[0];
        for (int i = 1; i < set.Size; i++)
        {
            if (set[i] > max)
                max = set[i];
            if (set[i] < min)
                min = set[i];
        }
        return max - min;
    }

    public static int Count(this MySet set)
    {
        return set.Size;
    }

    public static string AddDotAtEnd(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        if (input.EndsWith("."))
            return input;

        return input + ".";
    }
    public static void RemoveZeros(this MySet set)
    {
        for (int i = 0; i < set.Size; i++)
        {
            if (set[i] == 0)
            {
                set.Remove(set[i]);
            }
        }
    }
}

class Program
{
    static void Main()
    {
        MySet set1 = new MySet(5);
        set1 = set1 << 1;
        set1 = set1 << 2;
        set1 = set1 << 3;

        MySet.Production productionInfo = new MySet.Production
        {
            Id = 1,
            OrganizationName = "Magilev Development"
        };
        set1.ProductionInfo = productionInfo;

        MySet.Developer developer = new MySet.Developer
        {
            Id = 101,
            FullName = "Pavel Lobko",
            Department = "Magilev Development"
        };

        
        Console.WriteLine("Production Info: Id={0}, Name={1}", set1.ProductionInfo.Id, set1.ProductionInfo.OrganizationName);
        set1.Display(); // Вывод: Множество: 1 2 3
        int sum = set1.Sum();
        Console.WriteLine("sum " + sum);

        int difference = set1.Difference();
        Console.WriteLine("delta min max " + difference);

        int count = set1.Count();
        Console.WriteLine("Кол-сць " + count);

        MySet set2 = new MySet(5);
        set2 = set2 << 1;
        set2 = set2 << 2;
        set2 = set2 << 3;
        set2 = set2 << 4;
        set2 = set2 << 0;
        set2.Display();
        set2.RemoveZeros();
        set2 = set2 >> 3; // Удалить элемент 3
        set2.Display();

        set1.Display(); // Вывод: Множество: 1 2 3 

        bool isSubset = set1 > set2;
        Console.WriteLine(isSubset); // Вывод: False

        MySet set3 = new MySet(5);
        set3 = set3 << 1;
        set3 = set3 << 2;
        set3 = set3 << 3;
        bool isNotEqual = set1 != set3;
        Console.WriteLine(isNotEqual);

        MySet intersection = set2 % set3;
        intersection.Display();

        string text = "Я люблю ООП";
        Console.WriteLine(text.AddDotAtEnd());
    }
}
