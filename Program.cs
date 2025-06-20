using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace Selection_Counter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) Error("Не был передан SVG файл");

            string filePath = args[0];

            if (Path.GetExtension(filePath).ToLower() != ".svg") Error("Переданный файл не является SVG");

            try
            {
                XDocument xml = XDocument.Load(filePath);

                if (xml.Root == null || xml.Root.Name.LocalName != "svg") Error("Неподходящее содержимое файла");

                foreach (XElement element in xml.Root.Elements())
                {
                    XAttribute? id = element.Attribute("id");

                    if (id != null)
                    {
                        if (id.ToString().ToLower().Contains("selection"))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(id.ToString().Replace("id=", ""));
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write($" {element.Elements().Count()} \n");
                        }
                    }
                }

                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Error(ex.ToString());
            }
        }

        [DoesNotReturn]
        private static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ошибка: {message}");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
