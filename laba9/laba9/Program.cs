using System;
using System.Text;
using System.Collections;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.ObjectModel;

public class Controller : IEnumerable<Students>
{
    private Queue<Students> studentQueue;
    public Controller()
    {
        studentQueue = new Queue<Students>();
    }
    public void EnqueueStudent(Students student)
    {
        studentQueue.Enqueue(student);
    }
    public Students DequeueStudent()
    {
        return studentQueue.Dequeue();
    }
    public IEnumerator<Students> GetEnumerator()
    {
        return studentQueue.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public void ClearController()
    {
        studentQueue.Clear();
    }
}
public class Students 
{
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public int[] Marks { get; set; }
    public string Faculty { get; set; }
    public Students(int id,string name,string surname,int age, int[] marks,string faculty) {
        Id = id;
        Name = name;
        Surname = surname;
        Age = age;
        Marks = marks;
        Faculty = faculty;
    }
    public Students()
    {
        Id = 0;
        Name = "";
        Surname = "";
        Age = 0;
        Marks = new int[] { 0,0,0,0};
        Faculty = "";
    }
    public void Display()
    {
        Type type = this.GetType();
        PropertyInfo[] properties = type.GetProperties();

        Console.WriteLine("Свойства класса Students:");
        foreach (var property in properties)
        {
            object value = property.GetValue(this);
            Console.WriteLine($"{property.Name}: {value}");
        }
    }

}
class Program
{
    static void Main()
    {
        Controller controller = new Controller();
        Students student1 = new Students(1, "Павел", "Лабко", 18,new int[] { 2, 2, 4, 2 },"ФИТ");
        Students student2 = new Students(1, "Павел", "Иванов", 19, new int[] {6, 6, 6, 6 }, "ФИТ");

        controller.EnqueueStudent(student1);
        controller.EnqueueStudent(student2);

        
        Students empty = new Students();
        controller.EnqueueStudent(empty);
        foreach (Students student in controller)
        {
            Console.WriteLine($"Студент: {student.Name} {student.Surname}, Возраст: {student.Age},Факультет: {student.Faculty}");
        }
        var temp = controller.DequeueStudent();
        temp.Display();
        Console.WriteLine(controller.Contains<Students>(temp));
        
        foreach (Students student in controller)
        {
            Console.WriteLine($"Студент: {student.Name} {student.Surname}, Возраст: {student.Age},Факультет: {student.Faculty}");
        }
        List<Students> secondCollection = new List<Students>(controller);
        controller.ClearController();
        foreach (Students student in secondCollection)
        {
            Console.WriteLine($"Студент: {student.Name} {student.Surname}, Возраст: {student.Age}, Факультет: {student.Faculty}");
        }
        Students foundStudent = secondCollection.Find(student => student.Surname == "Иванов");

        if (foundStudent != null)
        {
            Console.WriteLine($"Найден студент: {foundStudent.Name} {foundStudent.Surname}");
        }
        else
        {
            Console.WriteLine("Студент не найден.");
        }
        ObservableCollection<Students> observableCollection = new ObservableCollection<Students>();
        observableCollection.CollectionChanged += CollectionChangedHandler;
        Students student3 = new Students(3, "Настя", "Водчиц", 18, new int[] { 2, 4, 3, 5 }, "ТОВ");
        observableCollection.Add(student3);
        observableCollection.RemoveAt(0);
    }

    private static void CollectionChangedHandler(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Console.WriteLine("Коллекция изменилась!");
    }
}