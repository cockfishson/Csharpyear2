using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml.Linq;

namespace Laba5
{
    public class Controller
    {
        private Lab lab;

        public Controller(Lab lab)
        {
            this.lab = lab;
        }

        public void AddGoods(Goods item)
        {
            lab.AddGoods(item);
        }

        public void RemoveGoods(Goods item)
        {
            lab.RemoveGoods(item);
        }

        public void DisplayGoods()
        {
            lab.DisplayGoods();
        }
        public void DisplayOldTech(int years) {
            lab.DisplayOldTech(years);
        }

        public void DisplayCountByType()
        {
            Dictionary<string, int> countByType = lab.CountGoodsByType();
            foreach (var kvp in countByType)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }

        public void DisplayGoodsByDescendingPrice()
        {
            lab.DisplayGoodsByDescendingPrice();
        }
    }
    public class InvalidId : Exception
    {
        public InvalidId(string message) : base(message) { }
    }
    public class NameIsNull : Exception
    {
        public NameIsNull(string message) : base(message) { } 
    }
    public class NegativeCost : Exception
    {
        public NegativeCost(string message) : base(message) { } 
    }
    public class IncorrectWeight : Exception
    {
        public IncorrectWeight(string message) : base(message) { }
    }
    public class IncorrectSize : Exception
    {
        public IncorrectSize(string message) : base(message) { }
    }
    public class Lab
    {
        private List<Goods> goodsList = new List<Goods>();

        public void AddGoods(Goods item)
        {
            goodsList.Add(item);
        }

        public void RemoveGoods(Goods item)
        {
            goodsList.Remove(item);
        }

        public void DisplayGoods()
        {
            foreach (Goods item in goodsList)
            {
                Console.WriteLine(item.ToString());
            }
        }
        public List<Goods> FindTechOlderThan(int years)
        {
            DateOnly currentYear = new DateOnly(2023, 10, 21);
            List<Goods> result = new List<Goods>();

            foreach (Goods item in goodsList)
            {
                if (item is Tech tech)
                {
                    if (currentYear.Year - tech.YearOfManufacture > years)
                    {
                        result.Add(tech);
                    }
                }
                
            }
            return result;
        }


        public Dictionary<string, int> CountGoodsByType()
        {
            Dictionary<string, int> countByType = new Dictionary<string, int>();

            foreach (Goods item in goodsList)
            {
                string typeName = item.GetType().Name;
                if (countByType.ContainsKey(typeName))
                {
                    countByType[typeName]++;
                }
                else
                {
                    countByType[typeName] = 1;
                }
            }

            return countByType;
        }
        public void DisplayOldTech(int years)
        {
            List<Goods> oldTech = FindTechOlderThan(years);
            foreach (Goods item in oldTech)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public void DisplayCountByType()
        {
            Dictionary<string, int> countByType = CountGoodsByType();
            foreach (var kvp in countByType)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }


        public void DisplayGoodsByDescendingPrice()
        {
            List<Goods> sortedList = goodsList.OrderByDescending(item => item.Cost).ToList();

            foreach (Goods item in sortedList)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
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
        private int id;
        public int Id {
            get
            {
                return id;
            }
            set
            {
                if (value < 0)
                {
                    string err = "Invalid Id";
                    throw new InvalidId(err);
                }
                id = value;
            }
        }
        private string name;
        private int cost;
        public string Name {
            get {
                return name;
            }
            set {
                if (value == "Popa")
                {
                    string err = "Invalid Name";
                    throw new NameIsNull(err);
                }
                name = value;
            } 
        }
        public string MadeBy { get; set; }
        public int Cost {
            get {
                return cost;
            }
            set {
                if(value < 0)
                {
                    string err = "Invalid Cost";
                    throw new NegativeCost(err);
                }
                cost = value;
            }
        }
        private double weight;
        public double Weight {
            get
            {
                return weight;
            }
            set
            {
                if (value < 0)
                {
                    string err = "Invalid Weight";
                    throw new IncorrectWeight(err);
                }
                    weight = value;
            }
        }
        private string size;
        public string Size {
            get
            {
                return size;
            }
            set
            {
                if (value == "0x0")
                {
                    string err = "Can't have 0x0 size";
                    throw new IncorrectSize(err);
                }
                size = value;
            }
        }
        public int YearOfManufacture {  get; set; }
        public Goods()
        {
            Id = 0;
            Name = " ";
            MadeBy = "";
            Cost = 0;
            Weight = 0.0;
            Size = "";

        }
        public Goods(int id, string name, string madeBy, int cost, double weight, string size,int yearofmanufacture)
        {
            Id = id;
            Name = name;
            MadeBy = madeBy;
            Cost = cost;
            Weight = weight;
            Size = size;
            YearOfManufacture = yearofmanufacture;
        }
        public override string ToString()
        {
            return $"Type: {this.GetType().Name}, Id: {Id}, Name: {Name}, MadeBy: {MadeBy}, Cost: {Cost}, Weight: {Weight}, Size: {Size}";
        }
    }

    public class Tech : Goods, Cart
    {
        public bool Portable { get; set; }
        public int ElectricPower { get; set; }
        public Tech()
       : base()
        {
            Portable = false;
            ElectricPower = 0;
        }
        public Tech(int id, string name, string madeBy, int cost, double weight, string size, int yearofmanufacture, bool portable, int electricPower)
       : base(id, name, madeBy, cost, weight, size, yearofmanufacture)
        {
            Portable = portable;
            ElectricPower = electricPower;
        }

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
        public PrintingTech()
      : base()
        {
            CanPrint = false;
            PrintsInColour = false;
            PrintingType = "";
        }
        public PrintingTech(int id, string name, string madeBy, int cost, double weight, string size, int yearofmanufacture, bool portable, int electricPower, bool canPrint, bool printsInColour, string printingType)
      : base(id, name, madeBy, cost, weight, size,yearofmanufacture, portable, electricPower )
        {
            CanPrint = canPrint;
            PrintsInColour = printsInColour;
            PrintingType = printingType;
        }
        public override string ToString()
        {
            return base.ToString() + $", CanPrint: {CanPrint}, PrintsInColour: {PrintsInColour}, PrintingType: {PrintingType}";
        }
    }

    public class Scaner : PrintingTech
    {
        public string ScanResolution { get; set; }
        public Scaner()
       : base()
        {
            ScanResolution = "";
        }
        public Scaner(int id, string name, string madeBy, int cost, double weight, string size, int yearofmanufacture, bool portable, int electricPower, bool canPrint, bool printsInColour, string printingType, string scanResolution)
       : base(id, name, madeBy, cost, weight, size, yearofmanufacture ,portable,  electricPower, canPrint, printsInColour, printingType)
        {
            ScanResolution = scanResolution;
        }
        public override string ToString()
        {
            return base.ToString() + $", ScanResolution: {ScanResolution}";
        }
    }

    public partial class PC : Tech
    {
        public string GPU { get; set; }
        public string CPU { get; set; }
        public string OS { get; set; }
        public string StorageType { get; set; }
        public string StorageAmount { get; set; }
        public PC()
        : base()
        {
            GPU = "";
            CPU = "";
            OS = "";
            StorageType = "";
            StorageAmount = "";
        }
        public PC(int id, string name, string madeBy, int cost, double weight, string size, int yearofmanufacture, bool portable, int electricPower, string gpu, string cpu, string os, string storageType, string storageAmount)
        : base(id, name, madeBy, cost, weight, size, yearofmanufacture, portable, electricPower)
        {
            GPU = gpu;
            CPU = cpu;
            OS = os;
            StorageType = storageType;
            StorageAmount = storageAmount;
        }
        
        public override string ToString()
        {
            return base.ToString() + $", GPU: {GPU}, CPU: {CPU}, OS: {OS}, StorageType: {StorageType}, StorageAmount: {StorageAmount}";
        }
    }

    public sealed class Tablet : PC
    {
        public string ScreenResolution { get; set; }
        public string ScreenType { get; set; }
        public string ChargingPort { get; set; }
        public Tablet()
       : base()
        {
            ScreenResolution = "";
            ScreenType = "";
            ChargingPort = "";
        }
        public Tablet(int id, string name, string madeBy, int cost, double weight, string size, int yearofmanufacture, bool portable, int electricPower, string gpu, string cpu, string os, string storageType, string storageAmount, string screenResolution, string screenType, string chargingPort)
       : base(id, name, madeBy, cost, weight, size, yearofmanufacture,portable, electricPower, gpu, cpu, os, storageType, storageAmount)
        {
            ScreenResolution = screenResolution;
            ScreenType = screenType;
            ChargingPort = chargingPort;
        }
        public override string ToString()
        {
            return base.ToString() + $", ScreenResolution: {ScreenResolution}, ScreenType: {ScreenType}, ChargingPort: {ChargingPort}";
        }
    }
    struct Struct
    {
        public string a;
        public int b;
        public void Print() => Console.WriteLine($"Random string: {a}  Random int: {b}");
    }
    enum ENUM
    {
        Lol,
        Lmao,
        Even
    }

    class Program
    {
        static void Main()
        {
            Tablet Tablet1 = new Tablet(1, "Lenovo Yoga Tab 11", "Lenovo", 500, 0.5, "10 inches", 2023, true, 100, "Snapdragon lolkek", "Snapdragon lolkek", "Android", "SSD", "256GB", "1920x1200", "Amoled", "USB-C");
            Tablet Tablet2 = new Tablet(1, "Lenovo Yoga Tab 10", "Lenovo", 250, 0.5, "10 inches", 2015, true, 100, "Snapdragon j", "Snapdragon j", "Android", "SSD", "128GB", "1920x1200", "Amoled", "USB-C");

            Tablet LenovoYoga11 = new Tablet(1, "Lenovo Yoga Tab 11", "Lenovo", 500, 0.5, "10 inches", 2023, true, 100, "Snapdragon lolkek", "Snapdragon lolkek", "Android", "SSD", "256GB", "1920x1200", "Amoled", "USB-C");
            PC MyPC = new PC
            {
                Id = 2,
                Name = "Pavel Ivanov PC",
                MadeBy = "Myself",
                Cost = 10000000,
                Weight = 4,
                Size = "Big",
                YearOfManufacture = 2020,
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
                YearOfManufacture = 2014,
                Portable = false,
                ElectricPower = 1,
                CanPrint = false,
                PrintsInColour = false,
                PrintingType = "none",
                ScanResolution = "Shmat"
            };
            Console.WriteLine(LenovoYoga11.ToString());
            Goods[] goodsArray = { LenovoYoga11, MyPC, Myscaner };

            Struct X = new Struct();
            X.a = "lol";
            X.b = 1;
            X.Print();
            Lab lab = new Lab();
            Controller controller = new Controller(lab);
            // Добавляем объекты в контейнер
            controller.AddGoods(LenovoYoga11);
            controller.AddGoods(MyPC);
            controller.AddGoods(Myscaner);
            controller.AddGoods(Tablet1);
            controller.AddGoods(Tablet2);
            controller.DisplayGoods();
            Console.WriteLine("Old tech");
            int years = 5;
            controller.DisplayOldTech(years);
            Console.WriteLine("tech ammount");
            // Подсчитываем количество каждого вида техники
            controller.DisplayCountByType();

            // Выводим список техники в порядке убывания цены
            controller.DisplayGoodsByDescendingPrice();
            Tablet exceptiontablet = new Tablet();
            try { 

            try
            {
                exceptiontablet.Id = -1000;
            }
            catch (InvalidId ex)
            {
                Console.WriteLine($"{ex.Message}");
                    throw new InvalidId("ID CAN'T BE NEGATIVE");
                }
            try
            {
                exceptiontablet.Cost = -1000;
            }
            catch (NegativeCost ex)
            {
                Console.WriteLine($"{ex.Message}");
                    throw new NegativeCost("COST CAN'T BE NEGATIVE");
            }
            try
            {
                exceptiontablet.Name = "Popa";
            }
            catch (NameIsNull ex)
            {
                Console.WriteLine($"{ex.Message}");
                    throw new NameIsNull("SWEARING IS BAD");
                }
            try
            {
                exceptiontablet.Size = "0x0";
             }
            catch (IncorrectSize ex)
            {
                Console.WriteLine(ex.Message);
                    throw new IncorrectSize("SIZE CAN'T BE 0x0");
                }
            try
            {
                exceptiontablet.Weight = -1000.0;
            }
            catch(IncorrectWeight ex)
            {
               Console.WriteLine(ex.Message);
                    throw new NegativeCost("COST CAN'T BE NEGATIVE");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Finally");
            }
            
            Console.WriteLine();
            //
            int[] values = null;
            Debug.Assert(values != null,"NOT NULL");
            Debugger.Break();
        }

    }
}