using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class Program
{
    public class PrimeNumberTask
    {
        public void ExecutePrimeNumberTask()
        {
            Stopwatch stopwatch = new Stopwatch();

            Task<List<int>> parallelTask = Task.Run(() =>
            {
                stopwatch.Start();

                List<int> primes = FindPrimesUsingSieveOfEratosthenes(100000);

                stopwatch.Stop();

                return primes;
            });

            Console.WriteLine("Current Task ID: " + parallelTask.Id);

            DisplayTaskStatus(parallelTask);

            parallelTask.Wait();

            Console.WriteLine("Task Status: " + parallelTask.Status);

            Console.WriteLine("Execution Time: " + stopwatch.Elapsed);

            for (int i = 0; i < 5; i++)
            {
                stopwatch.Restart();

                List<int> sequentialPrimes = FindPrimesSequentially(100000);

                stopwatch.Stop();

                Console.WriteLine("Pass {0}: {1}", i + 1, stopwatch.Elapsed);
            }

            DisplayTaskStatus(parallelTask);
        }

        public List<int> FindPrimesUsingSieveOfEratosthenes(int n)
        {
            bool[] primes = new bool[n + 1];
            for (int i = 2; i <= n; i++)
            {
                primes[i] = true;
            }

            for (int p = 2; p * p <= n; p++)
            {
                if (primes[p] == true)
                {
                    for (int i = p * p; i <= n; i += p)
                    {
                        primes[i] = false;
                    }
                }
            }

            List<int> result = new List<int>();
            for (int i = 2; i <= n; i++)
            {
                if (primes[i] == true)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        private List<int> FindPrimesSequentially(int n)
        {
            List<int> primes = new List<int>();

            for (int i = 2; i <= n; i++)
            {
                if (IsPrime(i))
                {
                    primes.Add(i);
                }
            }

            return primes;
        }

        private bool IsPrime(int number)
        {
            if (number < 2)
            {
                return false;
            }

            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void DisplayTaskStatus(Task<List<int>> task)
        {
            if (task.IsCompleted)
            {
                Console.WriteLine("Task Completed");
            }
            else
            {
                Console.WriteLine("Task In Progress");
            }
        }
    }

    public class CancellationTask
    {
        public void ExecuteCancellationTask()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Stopwatch stopwatch = new Stopwatch();

            Task<List<int>> parallelTask = Task.Run(() =>
            {
                stopwatch.Start();

                List<int> primes = FindPrimesUsingSieveOfEratosthenes(100000, cancellationTokenSource.Token);

                stopwatch.Stop();

                return primes;
            });

            Console.WriteLine("Current Task ID: " + parallelTask.Id);

            DisplayTaskStatus(parallelTask);

            parallelTask.Wait();

            Console.WriteLine("Task Status: " + parallelTask.Status);

            Console.WriteLine("Execution Time: " + stopwatch.Elapsed);

            for (int i = 0; i < 5; i++)
            {
                stopwatch.Restart();

                parallelTask = Task.Run(() => FindPrimesUsingSieveOfEratosthenes(100000, cancellationTokenSource.Token));
                Console.WriteLine("Task Canceled.");

                parallelTask.Wait();

                stopwatch.Stop();

                Console.WriteLine("Pass {0}: {1}", i + 1, stopwatch.Elapsed);
            }

            cancellationTokenSource.Cancel();

            DisplayTaskStatus(parallelTask);
        }

        public static List<int> FindPrimesUsingSieveOfEratosthenes(int n, CancellationToken cancellationToken)
        {
            bool[] primes = new bool[n + 1];
            for (int i = 2; i <= n; i++)
            {
                primes[i] = true;
            }

            for (int p = 2; p * p <= n; p++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (primes[p] == true)
                {
                    for (int i = p * p; i <= n; i += p)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        primes[i] = false;
                    }
                }
            }

            List<int> result = new List<int>();
            for (int i = 2; i <= n; i++)
            {
                if (primes[i] == true)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        private static void DisplayTaskStatus(Task<List<int>> task)
        {
            if (task.IsCompleted)
            {
                Console.WriteLine("Task Completed");
            }
            else
            {
                Console.WriteLine("Task In Progress");
            }
        }
    }

    static BlockingCollection<string> Stash = new BlockingCollection<string>();

    private static async Task Main()
    {
        PrimeNumberTask task1 = new PrimeNumberTask();
        CancellationTask task2 = new CancellationTask();
        task1.ExecutePrimeNumberTask();
        task2.ExecuteCancellationTask();
        Task<double> task3 = Task.Run(() => handleValue(2));
        Task<double> task4 = Task.Run(() => handleValue(3));
        Task<double> task5 = Task.Run(() => handleValue(4));
        Task<double> task6 = Task.Run(() =>
        {
            double result3 = task3.Result;
            double result4 = task4.Result;
            double result5 = task5.Result;

            return result3 * result4 / result5;
        });
        task6.Wait();
        Console.WriteLine("Result: " + task6.Result);
        Task<int> precedingTaskV1 = Task.Run(() => Sum(1, 2));
        Task<double> continuationTaskV1 = precedingTaskV1.ContinueWith((antecedent) =>
        {
            int result = antecedent.Result;
            return Math.Pow(result, 2);
        });
        continuationTaskV1.Wait();
        Console.WriteLine("Result: " + continuationTaskV1.Result);
        Task<int> precedingTaskV2 = Task.Run(() => Sum(1, 2));
        TaskAwaiter<int> awaiter = precedingTaskV2.GetAwaiter();
        Task<double> continuationTaskV2 = Task.Run(() =>
        {
            int result = awaiter.GetResult();
            return Math.Pow(result, 2);
        });
        continuationTaskV2.Wait();
        Console.WriteLine("Result: " + continuationTaskV2.Result);
        const int arraySize = 100000000;
        const int numArrays = 4;
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < numArrays; i++)
        {
            int[] array = new int[arraySize];
            array[i] = i;
        }
        stopwatch.Stop();
        Console.WriteLine("Regular" + stopwatch.ElapsedMilliseconds + " ms");
        stopwatch.Restart();
        Parallel.For(0, numArrays, (i) =>
        {
            int[] array = new int[arraySize];
            array[i] = i;
        });
        stopwatch.Stop();
        Console.WriteLine("Parallel" + stopwatch.ElapsedMilliseconds + " ms");
        Parallel.Invoke(
            () =>
            {
                Console.WriteLine("block 1");
            },
            () =>
            {
                Console.WriteLine("block 2");
            },
            () =>
            {
                Console.WriteLine("block 3");
            }
        );
        Console.WriteLine("all blocks completed");
        for (int i = 1; i <= 5; i++)
        {
            int handleSupplierId = i;
            Task.Run(() => handleSupplier(handleSupplierId));
        }
        for (int i = 1; i <= 10; i++)
        {
            int handleCustomerId = i;
            Task.Run(() => handleCustomer(handleCustomerId));
        }
        await PerformAsyncOperation();
        static int Sum(int a, int b)
        {
            return a + b;
        }
        static double handleValue(double value)
        {
            return value * 5.2;
        }
        static void handleSupplier(int handleSupplierId)
        {
            Random random = new Random();
            int productCount = 1;

            while (true)
            {
                Thread.Sleep(random.Next(200, 1000));
                string product = $"Product {handleSupplierId} ammount is {productCount++}";
                Stash.Add(product);
                Console.WriteLine($"Supplier {handleSupplierId} delivered product: {product}");
                PrintStashContents();
            }
        }

        static void handleCustomer(int handleCustomerId)
        {
            while (true)
            {
                Thread.Sleep(500);

                if (Stash.TryTake(out string product))
                {
                    Console.WriteLine($"Customer {handleCustomerId} bought product: {product}");
                    PrintStashContents();
                }
                else
                {
                    Console.WriteLine($"Customer {handleCustomerId} left, no products available in the Stash");
                }
            }
        }

        static void PrintStashContents()
        {
            Console.WriteLine($"Products in the Stash: {string.Join(", ", Stash)}");
        }

        static async Task PerformAsyncOperation()
        {
            Console.WriteLine("Start of asynchronous operation.");
            await Task.Delay(4500);
            Console.WriteLine("Asynchronous operation completed.");
        }

    }
}