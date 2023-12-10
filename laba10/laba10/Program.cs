using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters;
using System.Collections.Generic;

public class Abiturient
{
    private string lastName;
    private string firstName;
    private string patronymic;
    private string address;
    private string phoneNumber;
    private static int nextId = 1; // Счетчик для генерации уникальных ID
    private const int MaxGrade = 10; // Максимальная оценка
    public int Id { get; } // Уникальный идентификатор абитуриента
    public string LastName
    {
        get { return lastName; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("lastName не может быть пустым или равным null.");
            }
            lastName = value;
        }
    }
    public string FirstName
    {
        get { return firstName; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("firstName не может быть пустым или равным null.");
            }
            firstName = value;
        }
    }
    public string Patronymic
    {
        get { return patronymic; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("patronymic не может быть пустым или равным null.");
            }
            patronymic = value;
        }
    }
    public string Address
    {
        get { return address; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("address не может быть пустым или равным null.");
            }
            address = value;
        }
    }
    public string PhoneNumber
    {
        get { return phoneNumber; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("phoneNumber не может быть пустым или равным null.");
            }
            phoneNumber = value;
        }
    }
    private int[] grades; 
    private static int objectCount = 0;

    public Abiturient()
    {
        Id = nextId++; // Генерация уникального ID
        LastName = "";
        FirstName = "";
        Patronymic = "";
        Address = "";
        PhoneNumber = "";
        grades = new int[0]; // Пустой массив оценок
        objectCount++; // Увеличение счетчика объектов
    }

    public Abiturient(string lastName, string firstName, string patronymic, string address, string phoneNumber, int[] grades)
    {
        Id = nextId++; // Генерация уникального ID
        LastName = lastName;
        FirstName = firstName;
        Patronymic = patronymic;
        Address = address;
        PhoneNumber = phoneNumber;
        SetGrades(grades); // Установка оценок через метод, чтобы проверить их корректность
        objectCount++; // Увеличение счетчика объектов
    }
    static Abiturient()
    {
        Console.WriteLine("Static constructor called.");
    }
    private Abiturient(int id, string lastName)
    {
        Id = id;
        LastName = lastName;
    }
    public int GetId()
    {
        return Id;
    }
    public const string UniversityName = "My University";
    public int[] Grades
    {
        get { return grades; }
        set { SetGrades(value); }
    }
    public int MaxGrades()
    {
        return Grades.Max();
    }
    public bool BadGrades()
    {
        foreach (int grade in grades)
        {
            if (grade < 4) {
                return true;
            }
        }
        return false;
    }
    private void SetGrades(int[] newGrades)
    {
        if (newGrades == null || newGrades.Length == 0 || newGrades.Any(grade => grade < 1 || grade > MaxGrade))
            if (newGrades == null || newGrades.Length == 0 || newGrades.Any(grade => grade < 1 || grade > MaxGrade))
            {
                throw new ArgumentException("Invalid grades. Grades should be between 1 and 10.");
            }
        grades = newGrades;
    }
    public double AverageGrade()
    {
        return Grades.Average();
    }
    public double SumGrade()
    {
        return Grades.Sum();
    }
    public int MinGrade()
    {
        return Grades.Min();
    }
    public static void PrintClassInfo()
    {
        Console.WriteLine($"Class Info: Object Count = {objectCount}");
    }
    public override bool Equals(object obj)
    {
        if (obj is Abiturient other)
        {
            return Id == other.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    public override string ToString()
    {
        return $"Abiturient #{Id}: {lastName} {firstName} {patronymic}, Address: {address}, Phone: {phoneNumber}, Average Grade: {AverageGrade()}";
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine("n length");
        string[] Month = { "January", "February", "March", "April","May","June","July","August","September","October","November","December"};
        int n = 5;
        var NlenghtMonth = from p in Month where p.Length == n orderby p select p;
        foreach(string month in NlenghtMonth)
        {
            Console.WriteLine(month);
        }
        Console.WriteLine("Summer and Winter");
        var SummerAndWinter = from p in Month where p.Contains("J") || p.Contains("Aug") || p.Contains("Feb") || p.Contains("Dec") orderby p select p;
        foreach (string month in SummerAndWinter)
        {
            Console.WriteLine(month);
        }
        Console.WriteLine("Alphabetic order");
        var Alphabet = from p in Month  orderby p select p;
        foreach (string month in Alphabet)
        {
            Console.WriteLine(month);
        }
        Console.WriteLine("Has U");
        var HasU = from p in Month where p.ToLower().Contains("u") && p.Length >=4 orderby p select p;
        foreach (string month in HasU)
        {
            Console.WriteLine(month);
        }
        List<Abiturient> abiturients = new List<Abiturient>();
        Abiturient satsuk = new Abiturient("Сацюк", "Вован", "Пинскович", "ул. Грибоедова, 28", "+375-29-367-5585", new int[] { 10, 5, 7, 7, 9 });
        Abiturient petrov = new Abiturient("Петров", "Петр", "Петрович", "ул. Ленина, 2", "+375-44-654-3210", new int[] { 4, 9, 3, 4, 4 });
        Abiturient sidorovich = new Abiturient("Сидоров", "Сидор", "Сидорович", "ул. Гагарина, 3", "+375-26-555-5555", new int[] { 5, 5, 5, 5, 5 });
        Abiturient kyxapka = new Abiturient("Кухарев", "Максим", "Оршавия", "ул. Грибоедова, 28", "+375-29-367-5585", new int[] { 2, 5, 7, 7, 9 });
        Abiturient ivanov = new Abiturient("Иванов", "Павел", "Андреевич", "ул. Ленина, 2", "+375-44-654-3210", new int[] { 4, 9, 10, 4, 4 });
        Abiturient shynkaruk = new Abiturient("Шинкарюк", "Павел", "Андреевич", "ул. Ленина, 2", "+375-44-654-3210", new int[] { 4, 9, 10, 4, 4 });
        Abiturient kashchenko = new Abiturient("Кащенко", "Павел", "Андреевич", "ул. Ленина, 2", "+375-44-654-3210", new int[] { 4, 9, 10, 4, 4 });
        Abiturient petrashkevich = new Abiturient("Петрашкевич", "Антон", "Фёдорович", "ул. Ленина, 2", "+375-44-654-3210", new int[] { 4, 9, 10, 4, 4 });
        Abiturient labko = new Abiturient("Лабко", "Павел", "Андреевич", "ул. Гагарина, 3", "+375-26-555-5555", new int[] { 2, 2, 2, 2, 2 });
        Abiturient breakvan = new Abiturient("Кручков", "Иван", "Могилевович", "ул. Грибоедова, 28", "+375-29-367-5585", new int[] { 4, 5, 7, 7, 5 });
        abiturients.Add(satsuk);
        abiturients.Add(petrov);
        abiturients.Add(sidorovich);
        abiturients.Add(kyxapka);
        abiturients.Add(ivanov);
        abiturients.Add(shynkaruk);
        abiturients.Add(kashchenko);
        abiturients.Add(petrashkevich);
        abiturients.Add(labko);
        abiturients.Add(breakvan);
        var BadMarks = abiturients.Where(p => p.BadGrades()).OrderBy(p => p.LastName).ToList();
        Console.WriteLine("Bad marks");
        foreach (var abiturient in BadMarks)
        {
            Console.WriteLine(abiturient.ToString());
        }
        Console.WriteLine("Sum is more then 25");
        var HigherThenX = abiturients.Where(p => p.SumGrade()>25).OrderBy(p => p.LastName).ToList();
        foreach (var abiturient in HigherThenX)
        {
            Console.WriteLine(abiturient.ToString());
        }
        Console.WriteLine("10 in OOP");
        var TenInOOP = abiturients.Where(p => p.Grades[0] == 10).OrderBy(p => p.LastName).ToList();
        foreach (var abiturient in TenInOOP)
        {
            Console.WriteLine(abiturient.ToString());
        }
        Console.WriteLine("Last 4 - bad, boohoo");
        var OrderedByName = abiturients.OrderBy(a => a.LastName).ToList();
        var BottomFour = abiturients.OrderBy(a => a.AverageGrade()).Take(4).ToList();
        foreach (var abiturient in OrderedByName)
        {
            int k = 0;
            foreach (var abit in BottomFour)
            {
                if(abiturient.Id == abit.Id)
                {
                    k++;
                }
            }
            if(k== 0)
            {
            Console.WriteLine(abiturient.ToString());
            }
        }
        foreach (var abiturient in BottomFour)
        {
            Console.WriteLine(abiturient.ToString());
        }
        Console.WriteLine("5 quories");
        var result = abiturients
        .Where(p => p.AverageGrade() > 4.5) 
        .OrderByDescending(p => p.LastName) 
        .GroupBy(p => p.Address) 
        .Select(group => new
        {
            Address = group.Key,
            AverageGrade = group.Average(a => a.AverageGrade()), 
            TotalAbiturients = group.Count() 
        });

        foreach (var item in result)
        {
            Console.WriteLine($"Address: {item.Address}, Average Grade: {item.AverageGrade}, Total Abiturients: {item.TotalAbiturients}");
        }
        List<Abiturient> secondAbiturients = new List<Abiturient>
        {
            new Abiturient("Иванов", "Иван", "Иванович", "ул. Пушкина, 10", "+375-29-123-4567", new int[] { 7, 8, 9, 6, 7 }),
            new Abiturient("Петров", "Петр", "Петрович", "ул. Лермонтова, 5", "+375-44-987-6543", new int[] { 6, 5, 7, 6, 8 }),
        };
        var joinedAbiturients = abiturients
            .Join(secondAbiturients,
                a1 => a1.LastName,
                a2 => a2.LastName,
                (a1, a2) => new { Abiturient1 = a1, Abiturient2 = a2 });
        foreach (var joined in joinedAbiturients)
        {
            Console.WriteLine($"First: {joined.Abiturient1.FirstName} {joined.Abiturient1.LastName}, Second: {joined.Abiturient2.FirstName} {joined.Abiturient2.LastName}");
        }
    }
}