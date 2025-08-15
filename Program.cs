using System.Xml.Linq;

namespace Selection_Counter
{
    internal class Program
    {
        static void Main(string[] files)
        {
            if (files.Length == 0) Error("Не был передан SVG файл");
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            char result;
            List<int> selections = [];

            foreach (string file in files)
            {
                if (Path.GetExtension(file).ToLower() != ".svg")
                {
                    Error("Переданный файл не является SVG");
                    result = '❌';
                    goto Verdict;
                }

                try
                {
                    XDocument xml = XDocument.Load(file);

                    if (xml.Root == null || xml.Root.Name.LocalName != "svg")
                    {
                        Error("Неподходящее содержимое файла");
                        result = '❌';
                        goto Verdict;
                    }

                    foreach (XElement element in xml.Root.Elements())
                    {
                        XAttribute? id = element.Attribute("id");

                        if (id != null)
                        {
                            if (id.ToString().ToLower().Contains("selection"))
                            {
                                int count = element.Elements().Count();

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(id.ToString().Replace("id=", ""));
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write($" {count}\n");
                                
                                selections.Add(count);
                            }
                        }
                    }

                    if (selections.All(count => count == selections[0]))
                    {
                        result = '✅';
                    }
                    else
                    {
                        result = '❗';
                    }

                    selections = [];
                }
                catch (Exception ex)
                {
                    Error(ex.ToString());
                    result = '❌';
                }

                Verdict:

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{file.Split('\\')[^1]}:{result}\n");
            }

            Console.ReadLine();
        }

        private static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ошибка: {message}\n");
        }
    }
}
