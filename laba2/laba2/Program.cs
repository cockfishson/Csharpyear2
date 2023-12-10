using System;
using System.Linq;
namespace laba2
{
    public partial class Abiturient
    {
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
        private int[] grades; // Массив оценок

        // Статическое поле для хранения количества созданных объектов
        private static int objectCount = 0;

        // Конструктор без параметров
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

        // Конструктор с параметрами
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

        // Статический конструктор (конструктор типа)
        static Abiturient()
        {
            Console.WriteLine("Static constructor called.");
        }

        // Закрытый конструктор
        private Abiturient(int id, string lastName)
        {
            Id = id;
            LastName = lastName;
        }

        // Поле только для чтения ID
        public int GetId()
        {
            return Id;
        }

        // Поле-константа
        public const string UniversityName = "My University";

        // Свойство для оценок с проверкой корректности через set
        public int[] Grades
        {
            get { return grades; }
            set { SetGrades(value); }
        }
        public int MaxGrades()
        {
            return Grades.Max();
        }

        // Метод для установки оценок с проверкой корректности
        private void SetGrades(int[] newGrades)
        {
            if (newGrades == null || newGrades.Length == 0 || newGrades.Any(grade => grade < 1 || grade > MaxGrade))
                if (newGrades == null || newGrades.Length == 0 || newGrades.Any(grade => grade < 1 || grade > MaxGrade))
                {
                    throw new ArgumentException("Invalid grades. Grades should be between 1 and 5.");
                }
            grades = newGrades;
        }

        // Метод для расчета среднего балла
        public double AverageGrade()
        {
            return Grades.Average();
        }

        // Метод для поиска максимального балла


        // Метод для поиска минимального балла
        public int MinGrade()
        {
            return Grades.Min();
        }

        // Статический метод для вывода информации о классе
        public static void PrintClassInfo()
        {
            Console.WriteLine($"Class Info: Object Count = {objectCount}");
        }

        // Переопределение метода Equals для сравнения объектов
        public override bool Equals(object obj)
        {
            if (obj is Abiturient other)
            {
                return Id == other.Id;
            }
            return false;
        }

        // Переопределение метода GetHashCode для вычисления хэш-кода
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // Переопределение метода ToString для вывода информации об объекте
        public override string ToString()
        {
            return $"Abiturient #{Id}: {lastName} {firstName} {patronymic}, Address: {address}, Phone: {phoneNumber}, Average Grade: {AverageGrade()}";
        }
    }

    class Program
    {
        static void Main()
        {
            Abiturient[] Abiturients = new Abiturient[]
            {
            new Abiturient("Сацюк", "Вован", "Пинскович", "ул. Грибоедова, 28", "+375-29-367-5585", new int[] {4, 5, 7, 7, 9}),
            new Abiturient("Петров", "Петр", "Петрович", "ул. Ленина, 2", "+375-44-654-3210", new int[] {4, 9, 3, 4, 4}),
            new Abiturient("Сидоров", "Сидор", "Сидорович", "ул. Гагарина, 3", "+375-26-555-5555", new int[] {5, 5, 5, 5, 5}),

            };
            // a) Вывод списка абитуриентов с неудовлетворительными оценками
            Console.WriteLine("Список абитуриентов с неудовлетворительными оценками:");
            var AbiturientsWithUnsatisfactoryGrades = Abiturients.Where(a => a.Grades.Any(grade => grade < 3));
            foreach (var Abiturient in AbiturientsWithUnsatisfactoryGrades)
            {
                Console.WriteLine(Abiturient);
            }

            // b) Вывод списка абитуриентов с суммой баллов выше заданной
            int minimumTotalGrade = 30; // Минимальная сумма баллов
            Console.WriteLine($"\nСписок абитуриентов с суммой баллов выше {minimumTotalGrade}:");
            var AbiturientsWithHighTotalGrade = Abiturients.Where(a => a.Grades.Sum() > minimumTotalGrade);
            foreach (var Abiturient in AbiturientsWithHighTotalGrade)
            {
                Console.WriteLine(Abiturient);
            }

            // 2) Вызов конструкторов, свойств и методов
            Abiturient newAbiturient = new Abiturient("Новый", "Абитуриент", "Петрович", "ул. Новая, 4", "+375-25-222-3333", new int[] { 4, 5, 4, 3, 5 });
            Console.WriteLine($"\nНовый абитуриент: {newAbiturient}");
            Console.WriteLine($"Средний балл нового абитуриента: {newAbiturient.AverageGrade()}");
            Console.WriteLine($"Максимальный балл нового абитуриента: {newAbiturient.MaxGrades()}");
            Console.WriteLine($"Минимальный балл нового абитуриента: {newAbiturient.MinGrade()}");
            // 3) Создание массива объектов вашего типа
            Abiturient[] moreAbiturients = new Abiturient[]
            {
            new Abiturient("Аксеневич", "Ирина", "Александрович", "ул. Свердлова, 5", "+375-29-888-7777", new int[] { 3, 4, 3, 2, 4 }),
            new Abiturient("Михнюк", "Никита", "Михайлович", "ул. Ульянова, 6", "+375-29-666-5555", new int[] { 5, 4, 5, 3, 4 }),
            };

            Console.WriteLine("\nСписок всех абитуриентов:");
            foreach (var Abiturient in Abiturients.Concat(moreAbiturients))
            {
                Console.WriteLine(Abiturient);
            }

            // 4) Создание и вывод анонимного типа
            var anonymousAbiturient = new
            {
                FirstName = "Павел",
                LastName = "Иванов",
                AverageGrade = 4.5
            };

            Console.WriteLine($"\nАнонимный абитуриент: {anonymousAbiturient.FirstName} {anonymousAbiturient.LastName}, Средний балл: {anonymousAbiturient.AverageGrade}");
        }
    }
}