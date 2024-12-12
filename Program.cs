using System.Xml.Linq;

namespace Selection_Counter
{
    internal class Program
    {
        static string? FilePath;

        static void Main(string[] args)
        {
            if (args.Length == 0) Error("Не был передан SVG файл");

            FilePath = args[0];

            if (Path.GetExtension(FilePath).ToLower() != ".svg") Error("Переданный файл не является SVG");

            try
            {
                XDocument xml = XDocument.Load(FilePath);

                if (xml.Root == null || xml.Root.Name.LocalName != "svg") Error("Неподходящее содержимое файла");

                foreach (XElement element in xml.Root.Elements())
                {
                    if (element.Attribute("id").ToString().ToLower().Contains("selection"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(element.Attribute("id").ToString().Replace("id=", ""));
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($" {element.Elements().Count()} \n");
                    }
                }

                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Error(ex.ToString());
            }
        }

        static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ошибка: {message}");
            Console.ReadLine();
            return;
        }
    }
}
