using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
            if (grade < 4)
            {
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
                throw new ArgumentException("Invalid grades. Grades should be between 1 and 5.");
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
class MyClass
{
    public void MyMethod(int intValue, string stringValue)
    {
        Console.WriteLine($"Method called with parameters: {intValue}, {stringValue}");
    }
}
public static class Reflector
{
    public static void ExploreClass(string className, string outputFile)
    {
        Type type = Type.GetType(className);

        if (type == null)
        {
            Console.WriteLine($"Class {className} wasn't found.");
            return;
        }

        using (StreamWriter writer = new StreamWriter(outputFile))
        {
            writer.WriteLine($"Name of an assembly: {type.Assembly.FullName}");
            writer.WriteLine($"Is there any public constructors: {HasPublicConstructors(type)}");

            writer.WriteLine("Public methods:");
            foreach (string methodInfo in GetPublicMethods(type))
            {
                writer.WriteLine(methodInfo);
            }
            writer.WriteLine("Public properties:");
            foreach (string propertyInfo in GetPublicProperties(type))
            {
                writer.WriteLine(propertyInfo);
            }
            writer.WriteLine("Interfaces:");
            foreach (string interfaceInfo in GetImplementedInterfaces(type))
            {
                writer.WriteLine(interfaceInfo);
            }
            writer.WriteLine("Public methods with parameters:");
            foreach (string methodInfoWithParameters in GetPublicMethodsWithParameters(type))
            {
                writer.WriteLine(methodInfoWithParameters);
            }
        }

        Console.WriteLine($"Information about {className} parsed into {outputFile}.");
    }

    private static bool HasPublicConstructors(Type type)
    {
        return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any();
    }

    private static IEnumerable<string> GetPublicMethods(Type type)
    {
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

        return methods.Select(method => $"{method.ReturnType.Name} {method.Name}({GetParametersInfo(method)})");
    }

    private static IEnumerable<string> GetPublicProperties(Type type)
    {
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        return properties.Select(propertie => $"{propertie.PropertyType} {propertie.Name}");
    }

    private static IEnumerable<string> GetImplementedInterfaces(Type type)
    {
        return type.GetInterfaces().Select(inter => inter.FullName);
    }
    private static string GetParametersInfo(MethodInfo method)
    {
        ParameterInfo[] parameters = method.GetParameters();
        return string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
    }
    private static IEnumerable<string> GetPublicMethodsWithParameters(Type type)
    {
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

        return methods
            .Where(method => method.GetParameters().Length > 0)
            .Select(method => $"{method.ReturnType.Name} {method.Name}({GetParametersInfo(method)})");
    }
    public static void InvokeMethodFromFile(string filePath, string className, string methodName)
    {
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 2)
        {
            Console.WriteLine("Invalid file format.");
            return;
        }
        Type classType = Type.GetType(className);
        if (classType == null)
        {
            Console.WriteLine($"Class {className} not found.");
            return;
        }

        object classInstance = Activator.CreateInstance(classType);

        MethodInfo method = classType.GetMethod(methodName);
        if (method == null)
        {
            Console.WriteLine($"Method {methodName} not found in class {className}.");
            return;
        }

        string[] parameterTypeNames = lines[0].Split(',');
        string[] parameterValues = lines[1].Split(',');

        object[] parameters = new object[parameterTypeNames.Length];
        for (int i = 0; i < parameterTypeNames.Length; i++)
        {
            Type parameterType = Type.GetType(parameterTypeNames[i].Trim());
            parameters[i] = Convert.ChangeType(parameterValues[i].Trim(), parameterType);
        }

        method.Invoke(classInstance, parameters);
    }
    public static T Create<T>()
    {
        Type type = typeof(T);
        ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        if (constructors.Length == 0)
        {
            Console.WriteLine($"Error: No public constructors found for type {type.FullName}.");
            return default;
        }
        ConstructorInfo selectedConstructor = constructors[0];

        try
        {
            ParameterInfo[] parameters = selectedConstructor.GetParameters();
            object[] constructorArgs = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                constructorArgs[i] = parameters[i].ParameterType.IsValueType
                    ? Activator.CreateInstance(parameters[i].ParameterType)
                    : null;
            }
            T instance = (T)selectedConstructor.Invoke(constructorArgs);

            return instance;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating an instance of type {type.FullName}: {ex.Message}");
            return default;
        }
    }
}
class Program
{
    static void Main()
    {
        Reflector.ExploreClass("Abiturient", "output.txt");
        Reflector.InvokeMethodFromFile("parameters.txt", "MyClass", "MyMethod");
        MyClass myInstance = Reflector.Create<MyClass>();

        if (myInstance != null)
        {
            myInstance.MyMethod(42, "Hello, Reflection!");
        }
    }
}