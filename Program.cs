using System.Text.RegularExpressions;

class Program
{
    static string fileName = "LoremIpsum.txt";
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        do
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Word Count");
            Console.WriteLine("2. Calc Expression");
            Console.WriteLine("0. Exit");

            Console.Write("Input:");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        GetWordCount();
                        break;

                    case 2:
                        EvaluateMathExpression();
                        break;

                    case 0:
                        Console.WriteLine("Have a nice day. Bye!");
                        return;

                    default:
                        Console.WriteLine("Wrong input.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Wrong input.");
            }

            Console.WriteLine();
        } while (true);
    }

    //1
    static void EvaluateMathExpression()
    {
        System.Data.DataTable table = new System.Data.DataTable();
        Console.Write("Input math expr:");

        string? expression = Console.ReadLine();

        try
        {
            double result = Convert.ToDouble(table.Compute(expression, string.Empty));
            Console.WriteLine($"Result: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something is wrong: {ex.Message}");
        }
    }

    //2
    static void GetWordCount()
    {
        Console.Write("Write text:");
        string? text = Console.ReadLine();

        MatchCollection matches = Regex.Matches(text, "[a-zA-Z0-9]");
        string[] words;

        if (matches.Count() > 0) {
            words = text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"WC:{words.Length}");
            return;
        }

        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, fileName);
        string content = File.ReadAllText(filePath);
        words = content.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine($"Default content: {content}");
        Console.WriteLine($"WC:{words.Length}");
    }
}
