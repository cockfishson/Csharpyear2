using System;
using System.Text;

namespace ConversionExample;

public class FormatConverter
{
    public static void Pershy()
    {
            Boolean a = true;
            Byte b = 8;
            SByte c = 2;
            Char d = 'L';
            Decimal e = 9994337593543950335;
            Double f = 2.35;
            Single g = .6f;
            Int32 h = -2147483648;
            UInt32 i = 4294967295;
            ///UntPtr - Тып IntPtr можа выкарыстоўвацца мовамі, якія падтрымліваюць yказальнікі, і як звычайны сродак спасылкі на дадзеныя паміж мовамі, якія падтрымліваюць і не падтрымліваюць паказальнікі.
            ///UIntPtr - Гэта int які мае дліну ў 1 біт, тое ж самае, што і ўказальнік. Не ведаю які прыклад зрабіць
            Int64 j = -9223372036854775807;
            UInt64 k = 9223372036854775807;
            Int16 l = -32767;
            UInt16 m = 32767;
            Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}", a, b, c, d, e, f, g, h, i, j, k, l, m));
            //Implicit casting of an integer to a floating point number:
            int intValue = 42;
            float floatValue = intValue;
            // Implicitly cast a floating point number to a higher precision floating point number:
            int intValue1 = 42;
            double doubleValue = intValue1;
            //Implicit casting of a derived class to a base class:
            //class Animal { }
            //class Dog : Animal { }
            //Dog dog = new Dog();
            //Animal animal = dog;
            //Implicit casting of an integer to an enum:
            //enum Color { Red, Green, Blue }
            //int colorValue = 2;
            //Color color = (Color)colorValue;
            //Implicit casting of a floating point number to a string:
            float floatValue2 = 3.14f;
            string floatString = floatValue2.ToString();
            // Explicit type cast (double to int)
            double anotherDoubleValue = 3.14;
            int anotherIntValue = (int)anotherDoubleValue;
            Console.WriteLine($"Explicit cast from double to int: {anotherIntValue}");
            //Explicit conversion of a floating point number to an integer:
            float floatValue3 = 3.14f;
            int intValue3 = (int)floatValue3;
            //Explicitly casting a base class to a derived class:
            // Animal { }
            //class Dog : Animal { }
            //Animal animal = new Dog();
            //Dog dog = (Dog)animal;
            //Explicitly converting a string to a number:
            //string numberString = "42";
            //int number = int.Parse(numberString);
            //Explicitly converting a string to a number:
            string numberString1 = "42";
            int number = int.Parse(numberString1);
            //explicit casting of the enumeration to an integer:
            //enum Color { Red, Green, Blue }
            //int colorValue = (int)Color.Blue;
            // Operation using the Convert class
            string numberString = "123";
            int parsedInt = Convert.ToInt32(numberString);
            Console.WriteLine($"Converting a string to int using Convert: {parsedInt}");
            //Boxing and unboxing value types:
            int intValue9 = 42;

            // Boxing
            object boxedValue = intValue9;

            // Unboxing
            int unboxedValue = (int)boxedValue;


            //2.Working with an implicitly typed variable:
            var str = "Hello, world!"; // The type of str will be determined as string automatically


            //3.An example of working with a Nullable variable:
            int? nullableInt = null; // Create a Nullable variable of type int

            if (nullableInt.HasValue)
            {
                int regularInt = nullableInt.Value; // Get the value if it exists
            }
            else
            {
                Console.WriteLine("nullableInt has no value.");
            }


            //4.An example of an error when using a variable of type var:

            var value = 42; // Type defined as int

            // With the next instruction we will try to assign a value of a different type
            //value = "Error"; Compile error because value type is already defined as int
        }
    }
class Program1
{
    static void chatyrydash6()
    {
        // Set a tuple of 5 elements
        var tuple = (42, "Hello", 'A', "World", ulong.MaxValue);

        // Output the entire tuple
        Console.WriteLine(tuple);

        // Output selected elements of the tuple
        Console.WriteLine($"1st item: {tuple.Item1}");
        Console.WriteLine($"3rd item: {tuple.Item3}");
        Console.WriteLine($"4th item: {tuple.Item4}");

        // Unpack the tuple into variables
        (int first, string second, char third, string fourth, ulong fifth) = tuple;

        // Demonstrate different ways to unpack a tuple
        Console.WriteLine($"first: {first}");
        Console.WriteLine($"second: {second}");
        Console.WriteLine($"third: {third}");
        Console.WriteLine($"fourth: {fourth}");
        Console.WriteLine($"fifth: {fifth}");

        // Use the variable "_" to ignore a specific element
        (int a, _, char c, string d, _) = tuple;
        Console.WriteLine($"a: {a}");
        Console.WriteLine($"c: {c}");
        Console.WriteLine($"d: {d}");

        // Create two tuples for comparison
        var tuple1 = (1, "One");
        var tuple2 = (2, "Two");

        // Compare tuples
        bool areEqual = tuple1.Equals(tuple2);
        Console.WriteLine($"Tuples are equal: {areEqual}");

        // Create an array of integers and a string
        int[] intArray = { 10, 20, 30, 40, 50 };
        string str = "Hello";

        // Call the local function and print the result
        var result = GetMinMaxSumAndFirstLetter(intArray, str);
        Console.WriteLine($"Maximum: {result.max}, Minimum: {result.min}, Sum: {result.sum}, First Letter: {result.firstLetter}");

        // Local functions for working with checked/unchecked
        void CheckedExample()
        {
            checked
            {
                int maxInt = int.MaxValue;
                Console.WriteLine($"Maximum int value (checked): {maxInt}");
            }
        }

        void UncheckedExample()
        {
            unchecked
            {
                int maxInt = int.MaxValue;
                Console.WriteLine($"Maximum int value (unchecked): {maxInt}");
            }
        }

        // Call local functions
        CheckedExample();
        UncheckedExample();
    }

    // Local function for task 5
    static (int max, int min, int sum, char firstLetter) GetMinMaxSumAndFirstLetter(int[] arr, string str)
    {
        int max = arr.Max();
        int min = arr.Min();
        int sum = arr.Sum();
        char firstLetter = str.FirstOrDefault();
        return (max, min, sum, firstLetter);
    }
}
class Program2
{
    static void Main()
    {
        // a. Comparing String Literals
        string str1 = "Hello";
        string str2 = "World";
        bool areEqual = (str1 == str2);
        Console.WriteLine($"Compare string literals: {areEqual}");

        // b. String Operations
        string originalString = "This is a string to demonstrate operations";
        string concatenatedString = originalString + " in C#.";
        string copiedString = string.Copy(originalString);
        string substring = originalString.Substring(5, 6);
        string[] words = originalString.Split(' ');
        string insertedString = originalString.Insert(10, "inserted");
        string modifiedString = originalString.Replace("demos", "changes");
        Console.WriteLine($"String concatenation: {concatenatedString}");
        Console.WriteLine($"Copying a line: {copiedString}");
        Console.WriteLine($"Select substring: {substring}");
        Console.WriteLine($"Splitting a line into words:");
        foreach (var word in words)
        {
            Console.WriteLine(word);
        }
        Console.WriteLine($"Inserting a substring: {insertedString}");
        Console.WriteLine($"Substring replacement: {modifiedString}");

        // String interpolation
        string name = "John";
        int age = 30;
        string interpolatedString = $"Hi, my name is {name} and I am {age} years old.";
        Console.WriteLine(interpolatedString);

        // c. Empty and null strings
        string emptyString = string.Empty;
        string nullString = null;
        bool isEmptyOrNullOrEmpty = string.IsNullOrEmpty(emptyString) || string.IsNullOrEmpty(nullString);
        Console.WriteLine($"Empty or null line: {isEmptyOrNullOrEmpty}");

        // d. Working with StringBuilder
        StringBuilder stringBuilder = new StringBuilder("Example StringBuilder");
        stringBuilder.Remove(7, 12); // Delete specific positions
        stringBuilder.Insert(0, "New "); // Insert at the beginning
        stringBuilder.Append(" end."); // Add to end
        Console.WriteLine(stringBuilder.ToString());
    }
}
class Program
{
    static void Main()
    {
        // a. Creating and displaying a two-dimensional array
        int[,] matrix = new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } };
        Console.WriteLine("Two-dimensional array (matrix):");
        for (int row = 0; row < 2; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                Console.Write(matrix[row, col] + "\t");
            }
            Console.WriteLine();
        }

        // b. Creating and working with a one-dimensional array of strings
        string[] stringArray = new string[] { "one", "two", "three" };
        Console.WriteLine("\nOne-dimensional array of strings:");
        foreach (string item in stringArray)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine($"Array length: {stringArray.Length}");

        Console.Write("Enter position to change (0, 1, or 2): ");
        int position = int.Parse(Console.ReadLine());

        Console.Write("Enter a new value: ");
        string newValue = Console.ReadLine();

        if (position >= 0 && position < stringArray.Length)
        {
            stringArray[position] = newValue;
            Console.WriteLine("\nUpdated array:");
            foreach (string item in stringArray)
            {
                Console.WriteLine(item);
            }
        }
        else
        {
            Console.WriteLine("Invalid position. Change failed.");
        }

        // c. Creating a stepped array of real numbers
        double[][] jaggedArray = new double[3][];
        jaggedArray[0] = new double[] { 1.1, 2.2 };
        jaggedArray[1] = new double[] { 3.3, 4.4, 5.5 };
        jaggedArray[2] = new double[] { 6.6, 7.7, 8.8, 9.9 };

        Console.WriteLine("\nStepped Array:");
        for (int i = 0; i < jaggedArray.Length; i++)
        {
            for (int j = 0; j < jaggedArray[i].Length; j++)
            {
                Console.Write(jaggedArray[i][j] + "\t");
            }
            Console.WriteLine();
        }

        // d. Creating Implicitly Typed Variables
        var implicitIntArray = new[] { 1, 2, 3 };
        var implicitString = "Implicitly typed string";

        Console.WriteLine("\nImplicitly typed array:");
        foreach (var item in implicitIntArray)
        {
            Console.Write(item + "\t");
        }
        Console.WriteLine("\nImplicitly typed string: " + implicitString);
    }
}