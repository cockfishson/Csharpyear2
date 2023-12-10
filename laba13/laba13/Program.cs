using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text.Json;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
#pragma warning disable SYSLIB0011
namespace Laba4
{
    public interface ISerializer
    {
        void Serialize<T>(T obj, string filePath);
        T Deserialize<T>(string filePath);
    }

//    public class BinarySerializer : ISerializer
//    {
//        public void Serialize<T>(T obj, string filePath)
//        {
//            using (FileStream fs = new FileStream(filePath, FileMode.Create))
//            {
//                BinaryFormatter formatter = new BinaryFormatter();
//                formatter.Serialize(fs, obj);
//           }
//        }
//
//        public T Deserialize<T>(string filePath)
//        {
//            using (FileStream fs = new FileStream(filePath, FileMode.Open))
//            {
//                BinaryFormatter formatter = new BinaryFormatter();
//                return (T)formatter.Deserialize(fs);
//            }
//        }
//    }

//    public class SoapSerializer : ISerializer
//    {
//        public void Serialize<T>(T obj, string filePath)
//        {
//          using (FileStream fs = new FileStream(filePath, FileMode.Create))
//            {
//                SoapFormatter formatter = new SoapFormatter();
//                formatter.Serialize(fs, obj);
//            }
//        }
//
//        public T Deserialize<T>(string filePath)
//        {
//           using (FileStream fs = new FileStream(filePath, FileMode.Open))
//            {
//                SoapFormatter formatter = new SoapFormatter();
//                return (T)formatter.Deserialize(fs);
//            }
//        }
//    }

    public class JsonSerializer : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(obj);
            File.WriteAllText(filePath, json);
        }

        public T Deserialize<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
    }


    public class XmlSerializer : ISerializer
    {
        public void Serialize<T>(T obj, string filePath)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public T Deserialize<T>(string filePath)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }


    public class CustomSerializer
    {
        private ISerializer serializer;

        public CustomSerializer(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public void Serialize<T>(T obj, string filePath)
        {
            serializer.Serialize(obj, filePath);
        }

        public T Deserialize<T>(string filePath)
        {
            return serializer.Deserialize<T>(filePath);
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
    public  class Goods
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
        public string ScreenResolution { get; set; }
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
//            CustomSerializer binarySerializer = new CustomSerializer(new BinarySerializer());
//            binarySerializer.Serialize(LenovaYoga11, "LenovaYoga11.bin");
//           Tablet deserializedTablet = binarySerializer.Deserialize<Tablet>("LenovaYoga11.bin");

//            CustomSerializer soapSerializer = new CustomSerializer(new SoapSerializer());
//            soapSerializer.Serialize(LenovaYoga11, "LenovaYoga11.soap");
//            Tablet deserializedTabletSoap = soapSerializer.Deserialize<Tablet>("LenovaYoga11.soap");

            CustomSerializer jsonSerializer = new CustomSerializer(new JsonSerializer());
            jsonSerializer.Serialize(LenovaYoga11, "LenovaYoga11.json");
            Tablet deserializedTabletJson = jsonSerializer.Deserialize<Tablet>("LenovaYoga11.json");
            Console.WriteLine(deserializedTabletJson.ToString());

            CustomSerializer xmlSerializer = new CustomSerializer(new XmlSerializer());
            xmlSerializer.Serialize(LenovaYoga11, "LenovaYoga11.xml");
            Tablet deserializedTabletXml = xmlSerializer.Deserialize<Tablet>("LenovaYoga11.xml");
            Console.WriteLine(deserializedTabletXml.ToString());   

            List<Goods> goodsList = new List<Goods> { MyPC, LenovaYoga11, Myscaner };


            CustomSerializer jsonSerializerCollection = new CustomSerializer(new JsonSerializer());
            jsonSerializerCollection.Serialize(goodsList, "goodsList.json");


            List<Goods> deserializedGoodsList = jsonSerializerCollection.Deserialize<List<Goods>>("goodsList.json");

            foreach (var item in deserializedGoodsList)
            {
                Printer printer = new Printer();
                printer.IAmPrinting(item);
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("LenovaYoga11.xml");


            XmlNodeList tabletNodes = xmlDoc.SelectNodes("//Tablet");


            XmlNodeList pcCostNodes = xmlDoc.SelectNodes("//Tablet/Cost");

            XDocument newXmlDocument = new XDocument(
                new XElement("GoodsList",
                    new XElement("Tablet",
                        new XElement("Id", 4),
                        new XElement("Name", "Samsung Galaxy Tab S7"),
                        new XElement("MadeBy", "Samsung"),
                        new XElement("Cost", 800),
                        new XElement("Weight", 0.5),
                        new XElement("Size", "11 inches"),
                        new XElement("Portable", true),
                        new XElement("ElectricPower", 120),
                        new XElement("GPU", "Adreno 650"),
                        new XElement("CPU", "Snapdragon 865+"),
                        new XElement("OS", "Android"),
                        new XElement("StorageType", "SSD"),
                        new XElement("StorageAmount", "128GB"),
                        new XElement("ScreenResolution", "2560x1600"),
                        new XElement("ScreenType", "Super AMOLED"),
                        new XElement("ChargingPort", "USB-C")
                    ),
                    new XElement("PC",
                        new XElement("Id", 5),
                        new XElement("Name", "Random PC"),
                        new XElement("MadeBy", "Fuck if I know"),
                        new XElement("Cost", 1800),
                        new XElement("Weight", 472),
                        new XElement("Size", "idk"),
                        new XElement("Portable", true),
                        new XElement("ElectricPower", 120),
                        new XElement("GPU", "GTX 650"),
                        new XElement("CPU", "Athlone"),
                        new XElement("OS", "Linux"),
                        new XElement("StorageType", "SSD"),
                        new XElement("StorageAmount", "127839428GB")
                    )
                )
            );

            newXmlDocument.Save("NewGoodsList.xml");


            var tabletNames = from tablet in newXmlDocument.Descendants("Tablet")
                              select tablet.Element("Name")?.Value;

            Console.WriteLine("Tablet Names:");
            foreach (var name in tabletNames)
            {
                Console.WriteLine(name);
            }

            var heavy = from goods in newXmlDocument.Descendants("PC")
                                 where (int)goods.Element("Weight") > 10
                                 select goods;

            Console.WriteLine("\nheavy Goods:");
            foreach (var heavyGood in heavy)
            {
                Console.WriteLine(heavyGood);
            }

        }
    }
}
