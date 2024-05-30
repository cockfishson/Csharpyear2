using System.Configuration;
using System.Data;
using System.Windows;

namespace laba4sem2
{

    public class GPU
    {
        public int Cost { get; set; }
        public int Frequency { get; set; }
        public int Memory { get; set; }
        public string MemoryType { get; set; }
        public int Bit { get; set; }
        public int Rating { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Producer { get; set; }
        public string CompanyAmdOrIntel { get; set; }
        public GPU(int cost, int frequency, int memory, string memoryType, int bit, int rating, int amount, string name, string description, string img, string model, string producer, string companyAmdOrIntel)
        {
            Cost = cost;
            Frequency = frequency;
            Memory = memory;
            MemoryType = memoryType;
            Bit = bit;
            Rating = rating;
            Amount = amount;
            Name = name;
            Img = img;
            Description = description;
            Model = model;
            Producer = producer;
            CompanyAmdOrIntel = companyAmdOrIntel;
        }
    }
}