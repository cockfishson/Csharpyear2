using System;
using System.Diagnostics;
using System.Threading;

class VideoChannel
{
    public int Id { get; private set; }

    public VideoChannel(int id)
    {
        Id = id;
    }
}

class Client
{
    public int Id { get; private set; }

    public Client(int id)
    {
        Id = id;
    }
}

class Program
{

    static void PrintRunningProcesses()
    {
        Process[] processes = Process.GetProcesses();

        foreach (Process process in processes)
        {
            if (process.Id != 0)
            {
                Console.WriteLine($"Process ID: {process.Id}");
                Console.WriteLine($"Name: {process.ProcessName}");
                Console.WriteLine($"Priority: {process.BasePriority}");
                Console.WriteLine($"Start Time: {process.StartTime}");
                Console.WriteLine($"Current State: {process.Responding}");
                Console.WriteLine($"Total Processor Time: {process.TotalProcessorTime}");
                Console.WriteLine("------------------------------------------------------");
            }
        }
    }

    static void ExploreAppDomain()
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;

        Console.WriteLine("Информация о текущем домене приложения:");
        Console.WriteLine($"Имя: {currentDomain.FriendlyName}");
        Console.WriteLine($"Детали конфигурации: {currentDomain.SetupInformation}");

        Console.WriteLine("Загруженные сборки в домене:");
        foreach (var assembly in currentDomain.GetAssemblies())
        {
            Console.WriteLine(assembly.FullName);
        }

        Console.WriteLine();
    }

    static void GenerateAndPrintPrimes(int n)
    {
        Console.WriteLine($"Генерация простых чисел от 1 до {n}:");
        for (int i = 2; i <= n; i++)
        {
            if (IsPrime(i))
            {
                Console.WriteLine(i);
                Thread.Sleep(100);
            }
        }
    }

    static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0) return false;
        }
        return true;
    }

    static void PrintThreadInfo(Thread thread)
    {
        Console.WriteLine($"Информация о потоке: Имя: {thread.Name}, Приоритет: {thread.Priority}, " +
            $"ID: {thread.ManagedThreadId}, Состояние: {thread.ThreadState}");
    }


    static void PrintTime(object state)
    {
        Console.WriteLine($"Текущее время: {DateTime.Now.TimeOfDay}");
    }

    private static object lockObject = new object();
    private static int n = 8; 
    private static StreamWriter writer;

    static void PrintEvenNumbers()
    {
        for (int i = 2; i <= n; i += 2)
        {
            lock (lockObject)
            {
                writer.WriteLine($"Even: {i}");
                Console.WriteLine($"Even: {i}");
            }

            Thread.Sleep(100); 
        }
    }

    static void PrintOddNumbers()
    {
        for (int i = 1; i <= n; i += 2)
        {
            lock (lockObject)
            {
                writer.WriteLine($"Odd: {i}");
                Console.WriteLine($"Odd: {i}");
            }

            Thread.Sleep(50); 
        }
    }
    static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

    static void UnloadWarehouse(object truckName)
    {
        semaphore.Wait();
        Console.WriteLine($"{truckName} начал разгрузку склада.");
        Thread.Sleep(2000); 
        Console.WriteLine($"{truckName} завершил разгрузку склада.");
        semaphore.Release();
    }

    static SemaphoreSlim semaphorelol = new SemaphoreSlim(3, 3);

    static void UseVideoChannel(Client client)
    {
        if (semaphorelol.Wait(1000))
        {
            Console.WriteLine($"Клиент {client.Id} использует видеоканал.");
            Thread.Sleep(2000); 
            Console.WriteLine($"Клиент {client.Id} завершил использование видеоканала.");
            semaphorelol.Release();
        }
        else
        {
            Console.WriteLine($"Клиент {client.Id} не смог получить доступ к видеоканалу в течение 1 секунды.");
        }
    }

    static void Main()
    {
        PrintRunningProcesses();


        ExploreAppDomain();


        Console.Write("Введите значение n для генерации простых чисел: ");
        int n = int.Parse(Console.ReadLine());
        Thread thread = new Thread(() => GenerateAndPrintPrimes(n));
        thread.Start();
        Thread.Sleep(2000);
        thread.Join();
        Console.WriteLine("Главный поток завершил работу.");

        writer = new StreamWriter("output.txt");
        Thread evenThread = new Thread(PrintEvenNumbers);
        evenThread.Priority = ThreadPriority.AboveNormal; 
        Thread oddThread = new Thread(PrintOddNumbers);
        evenThread.Start();
        oddThread.Start();
        evenThread.Join();
        oddThread.Join();
        writer.Close();

        Timer timer = new Timer(PrintTime, null, 0, 1000);
        Thread.Sleep(5000);
        timer.Change(Timeout.Infinite, Timeout.Infinite);
        Console.WriteLine("Таймер остановлен.");

        Thread truck1 = new Thread(UnloadWarehouse);
        Thread truck2 = new Thread(UnloadWarehouse);
        Thread truck3 = new Thread(UnloadWarehouse);
        truck1.Start("Грузовик 1");
        truck2.Start("Грузовик 2");
        truck3.Start("Грузовик 3");
        truck1.Join();
        truck2.Join();
        truck3.Join();

        for (int i = 1; i <= 5; i++)
        {
            Thread clientThread = new Thread(() => UseVideoChannel(new Client(i)));
            clientThread.Start();
        }
    }
}
