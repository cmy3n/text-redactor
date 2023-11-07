using System;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;

public class Figure
{
    public string Name { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }

    public Figure() { }

    public Figure(string name, double width, double height)
    {
        Name = name;
        Width = width;
        Height = height;
    }
}

public static class FileConverter
{
    public static void PathFile(out string filePath)
    {
        Console.WriteLine("Введите путь к файлу: ");
        filePath = Console.ReadLine();
    }

    public static void LoadFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".txt":
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
                break;
            case ".json":
                string json = File.ReadAllText(filePath);
                Figure figureJson = JsonConvert.DeserializeObject<Figure>(json);
                Console.WriteLine($"Name: {figureJson.Name}, Width: {figureJson.Width}, Height: {figureJson.Height}");
                break;
            case ".xml":
                XmlSerializer serializer = new XmlSerializer(typeof(Figure));
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    Figure figureXml = (Figure)serializer.Deserialize(fileStream);
                    Console.WriteLine($"Name: {figureXml.Name}, Width: {figureXml.Width}, Height: {figureXml.Height}");
                }
                break;
            default:
                Console.WriteLine("Неподдерживаемый формат файла.");
                break;
        }
    }

    public static void SaveFile(Figure figure, string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        string newExtension;
        switch (extension)
        {
            case ".txt":
                newExtension = ".json";
                string json = JsonConvert.SerializeObject(figure);
                File.WriteAllText(Path.ChangeExtension(filePath, newExtension), json);
                Console.WriteLine($"Файл сохранен в формате JSON: {Path.ChangeExtension(filePath, newExtension)}");
                break;
            case ".json":
                newExtension = ".xml";
                XmlSerializer serializer = new XmlSerializer(typeof(Figure));
                using (FileStream fileStream = new FileStream(Path.ChangeExtension(filePath, newExtension), FileMode.Create))
                {
                    serializer.Serialize(fileStream, figure);
                }
                Console.WriteLine($"Файл сохранен в формате XML: {Path.ChangeExtension(filePath, newExtension)}");
                break;
            case ".xml":
                newExtension = ".txt";
                File.WriteAllText(Path.ChangeExtension(filePath, newExtension), $"{figure.Name}\n{figure.Width}\n{figure.Height}");
                Console.WriteLine($"Файл сохранен в формате TXT: {Path.ChangeExtension(filePath, newExtension)}");
                break;
            default:
                Console.WriteLine("Неподдерживаемый формат файла.");
                break;
        }
    }
}

public class Program
{
    public static void Main()
    {
        string filePath;
        FileConverter.PathFile(out filePath);
        FileConverter.LoadFile(filePath);

        Console.WriteLine("Нажмите F1 для сохранения или Escape для завершения.");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.F1)
            {
                Console.WriteLine("\nСохранение файла...");
                Figure newFigure = new Figure("Прямоугольник", 10, 5);
                FileConverter.SaveFile(newFigure, filePath);
                Console.WriteLine("Файл сохранен.");
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("Выход из программы.");
                break;
            }
        }
    }
}