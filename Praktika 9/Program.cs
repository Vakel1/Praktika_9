using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public override string ToString()
    {
        return $"{Id}: {FirstName} {LastName}, {Email}";
    }
}

class StudentManager
{
    private List<Student> students = new List<Student>();

    public void AddStudent(Student student)
    {
        student.Id = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1;
        students.Add(student);
    }

    public void RemoveStudent(int studentId)
    {
        Student studentToRemove = students.FirstOrDefault(s => s.Id == studentId);
        if (studentToRemove != null)
        {
            students.Remove(studentToRemove);
        }
    }

    public void UpdateStudent(int studentId, Student updatedStudent)
    {
        Student existingStudent = students.FirstOrDefault(s => s.Id == studentId);
        if (existingStudent != null)
        {
            existingStudent.FirstName = updatedStudent.FirstName;
            existingStudent.LastName = updatedStudent.LastName;
            existingStudent.Email = updatedStudent.Email;
        }
    }

    public List<Student> GetAllStudents()
    {
        return students;
    }

    public Student GetStudentById(int studentId)
    {
        return students.FirstOrDefault(s => s.Id == studentId);
    }

    public List<Student> SearchStudents(string keyword)
    {
        return students.Where(s =>
            s.FirstName.ToLower().Contains(keyword.ToLower()) ||
            s.LastName.ToLower().Contains(keyword.ToLower()) ||
            s.Email.ToLower().Contains(keyword.ToLower())).ToList();
    }

    public List<Student> SortStudentsByLastName()
    {
        return students.OrderBy(s => s.LastName).ToList();
    }

    public List<Student> SortStudentsByEmail()
    {
        return students.OrderBy(s => s.Email).ToList();
    }
}

class Program
{
    private static StudentManager studentManager = new StudentManager();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Student Management System");
            Console.WriteLine("1. Добавить Студента");
            Console.WriteLine("2. Удолить Студента");
            Console.WriteLine("3. Редактировать Студента");
            Console.WriteLine("4. Список Студентов");
            Console.WriteLine("5. Найти Студента");
            Console.WriteLine("6.Сортировать Студентов по Фамилии");
            Console.WriteLine("7. Сортировать по почте");
            Console.WriteLine("8. Сохранить Студентов");
            Console.WriteLine("9. Загрузить Студентов из файла");
            Console.WriteLine("0. Выйти");
            Console.Write("Выберите пункт: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        AddStudent();
                        break;
                    case 2:
                        RemoveStudent();
                        break;
                    case 3:
                        UpdateStudent();
                        break;
                    case 4:
                        ListAllStudents();
                        break;
                    case 5:
                        SearchStudents();
                        break;
                    case 6:
                        SortStudentsByLastName();
                        break;
                    case 7:
                        SortStudentsByEmail();
                        break;
                    case 8:
                        SaveStudentsToFile();
                        break;
                    case 9:
                        LoadStudentsFromFile();
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод.");
            }
        }
    }

    private static void AddStudent()
    {
        Student student = new Student();
        Console.Write("Введите имя: ");
        student.FirstName = Console.ReadLine();
        Console.Write("Ввеите фамилию: ");
        student.LastName = Console.ReadLine();
        Console.Write("Введите почту: ");
        student.Email = Console.ReadLine();
        studentManager.AddStudent(student);
        Console.WriteLine("Студен добавлен.");
    }

    private static void RemoveStudent()
    {
        Console.Write("Введите ID Студента: ");
        if (int.TryParse(Console.ReadLine(), out int studentId))
        {
            Student student = studentManager.GetStudentById(studentId);
            if (student != null)
            {
                studentManager.RemoveStudent(studentId);
                Console.WriteLine("Студент удалён.");
            }
            else
            {
                Console.WriteLine("Студент не найден.");
            }
        }
        else
        {
            Console.WriteLine("Неверный ввод.");
        }
    }

    private static void UpdateStudent()
    {
        Console.Write("Введите Id Студента: ");
        if (int.TryParse(Console.ReadLine(), out int studentId))
        {
            Student existingStudent = studentManager.GetStudentById(studentId);
            if (existingStudent != null)
            {
                Student updatedStudent = new Student();
                Console.Write("Введите имя: ");
                updatedStudent.FirstName = Console.ReadLine();
                Console.Write("Ввеите фамилию: ");
                updatedStudent.LastName = Console.ReadLine();
                Console.Write("Введите почту: ");
                updatedStudent.Email = Console.ReadLine();
                studentManager.UpdateStudent(studentId, updatedStudent);
                Console.WriteLine("Студент отредактирован.");
            }
            else
            {
                Console.WriteLine("Студент не найден.");
            }
        }
        else
        {
            Console.WriteLine("Неверный Id.");
        }
    }

    private static void ListAllStudents()
    {
        List<Student> students = studentManager.GetAllStudents();
        Console.WriteLine("Список всех студентов:");
        foreach (Student student in students)
        {
            Console.WriteLine(student);
        }
    }

    private static void SearchStudents()
    {
        Console.Write("Введите для поиска: ");
        string keyword = Console.ReadLine();
        List<Student> searchResults = studentManager.SearchStudents(keyword);
        Console.WriteLine("Найти результаты:");
        foreach (Student student in searchResults)
        {
            Console.WriteLine(student);
        }
    }

    private static void SortStudentsByLastName()
    {
        List<Student> sortedStudents = studentManager.SortStudentsByLastName();
        Console.WriteLine("Отсортировано по фамилии:");
        foreach (Student student in sortedStudents)
        {
            Console.WriteLine(student);
        }
    }

    private static void SortStudentsByEmail()
    {
        List<Student> sortedStudents = studentManager.SortStudentsByEmail();
        Console.WriteLine("Отсартировано по почте:");
        foreach (Student student in sortedStudents)
        {
            Console.WriteLine(student);
        }
    }

    private static void SaveStudentsToFile()
    {
        try
        {
            List<Student> students = studentManager.GetAllStudents();
            using (StreamWriter writer = new StreamWriter("students.txt"))
            {
                foreach (Student student in students)
                {
                    writer.WriteLine($"{student.Id},{student.FirstName},{student.LastName},{student.Email}");
                }
            }
            Console.WriteLine("Студент сохранён.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving students to file: {ex.Message}");
        }
    }

    private static void LoadStudentsFromFile()
    {
        try
        {
            List<Student> students = new List<Student>();
            using (StreamReader reader = new StreamReader("students.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        Student student = new Student
                        {
                            Id = int.Parse(parts[0]),
                            FirstName = parts[1],
                            LastName = parts[2],
                            Email = parts[3]
                        };
                        students.Add(student);
                    }
                }
            }
            studentManager = new StudentManager();
            studentManager.GetAllStudents().AddRange(students);
            Console.WriteLine("Students loaded from file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading students from file: {ex.Message}");
        }
    }
}
