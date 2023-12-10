using System;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Laba4
{
    public class Printer
    {
        public void IAmPrinting(Goods someobj)
        {
            Console.WriteLine($"Object type: {someobj.GetType().Name}");
            Console.WriteLine(someobj.ToString());
            Console.WriteLine();
        }
    }
    public interface Cart
    {
        int Cost { get; set; }
    }
    public abstract class Goods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MadeBy { get; set; }
        public int Cost { get; set; }
        public double Weight { get; set; }
        public string Size { get; set; }
        public override string ToString()
        {
            return $"Type: {this.GetType().Name}, Id: {Id}, Name: {Name}, MadeBy: {MadeBy}, Cost: {Cost}, Weight: {Weight}, Size: {Size}";
        }
    }

    public class Tech : Goods, Cart
    {
        public bool Portable { get; set; }
        public int ElectricPower { get; set; }
        public override string ToString()
        {
            return base.ToString() + $", Portable: {Portable}, ElectricPower: {ElectricPower}";
        }
    }

    public class PrintingTech : Tech
    {
        public bool CanPrint { get; set; }
        public bool PrintsInColour { get; set; }
        public string PrintingType { get; set; }
        public override string ToString()
        {
            return base.ToString() + $", CanPrint: {CanPrint}, PrintsInColour: {PrintsInColour}, PrintingType: {PrintingType}";
        }
    }

    public class Scaner : PrintingTech
    {
        public string ScanResolution { get; set; }
        public override string ToString()
        {
            return base.ToString() + $", ScanResolution: {ScanResolution}";
        }
    }

    public class PC : Tech
    {
        public string GPU { get; set; }
        public string CPU { get; set; }
        public string OS { get; set; }
        public string StorageType { get; set; }
        public string StorageAmount { get; set; }
        public override string ToString()
        {
            return base.ToString() + $", GPU: {GPU}, CPU: {CPU}, OS: {OS}, StorageType: {StorageType}, StorageAmount: {StorageAmount}";
        }
    }

    public sealed class Tablet : PC
    {
        public string ScreenResolution{ get; set; }
        public string ScreenType { get; set; }
        public string ChargingPort { get; set; }
        public override string ToString()
        {
            return base.ToString() + $", ScreenResolution: {ScreenResolution}, ScreenType: {ScreenType}, ChargingPort: {ChargingPort}";
        }
    }

    class Program
    {
        static void Main()
        {
            Tablet LenovaYoga11 = new Tablet
            {
                Id = 1,
                Name = "Lenovo Yoga Tab 11",
                MadeBy = "Lenovo",
                Cost = 500,
                Weight = 0.5,
                Size = "10 inches",
                Portable = true,
                ElectricPower = 100,
                GPU = "Snapdragon lolkek",
                CPU = "Snapdragon lolkek",
                OS = "Android",
                StorageType = "SSD",
                StorageAmount = "256GB",
                ScreenResolution = "1920x1200",
                ScreenType = "Amoled",
                ChargingPort = "USB-C"
            };
            PC MyPC = new PC
            {
                Id = 2,
                Name = "Pavel Ivanov PC",
                MadeBy = "Myself",
                Cost = 10000000,
                Weight = 4,
                Size = "Big",
                Portable = false,
                ElectricPower = 600,
                GPU = "RX580",
                CPU = "Ryzen 5 2600",
                OS = "Linux",
                StorageType = "SSD",
                StorageAmount = "512GB",
            };
            Scaner Myscaner = new Scaner
            {
                Id = 3,
                Name = "HP something",
                MadeBy = "HP",
                Cost = 1,
                Weight = 3,
                Size = "Quite Big",
                Portable = false,
                ElectricPower = 1,
                CanPrint = false,
                PrintsInColour = false,
                PrintingType = "none",
                ScanResolution = "Shmat"
            };
            Goods goods1 = new Tablet(); // Приведение объекта к базовому классу Goods

            if (goods1 is Tablet)
            {
                Tablet tabletlol = (Tablet)goods1; // Приведение к типу Tablet
                Console.WriteLine("This is a Tablet.");
            }
            else if (goods1 is PC)
            {
                PC pc = (PC)goods1; // Приведение к типу PC
                Console.WriteLine("This is a PC.");
            }
            else if (goods1 is Tech)
            {
                Tech tech = (Tech)goods1; // Приведение к типу Tech
                Console.WriteLine("This is a Tech.");
            }
            Goods goods2 = new Tablet(); // Приведение объекта к базовому классу Goods

            Tablet tablet = goods2 as Tablet;
            if (tablet != null)
            {
                Console.WriteLine("This is a Tablet.");
            }
            else
            {
                PC pc = goods2 as PC;
                if (pc != null)
                {
                    Console.WriteLine("This is a PC.");
                }
                else
                {
                    Tech tech = goods2 as Tech;
                    if (tech != null)
                    {
                        Console.WriteLine("This is a Tech.");
                    }
                }
            }
            Console.WriteLine(LenovaYoga11.ToString());
            Printer printer = new Printer();
            Goods[] goodsArray = { LenovaYoga11, MyPC, Myscaner };

            // Вызываем IAmPrinting для каждой ссылки в массиве
            foreach (Goods obj in goodsArray)
            {
                printer.IAmPrinting(obj);
            }
        }

    }
}