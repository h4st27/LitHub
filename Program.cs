using System.Text.RegularExpressions;

class Program
{
    static string fileName = "NewFileTxt.txt";
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        do
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Word Count");
            Console.WriteLine("2. Calculator Expression");
            Console.WriteLine("0. Quit");

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
                        Console.WriteLine("Goodbye");
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
        Console.Write("Input math expression:");

        string? expression = Console.ReadLine();

        try
        {
            double result = Convert.ToDouble(table.Compute(expression, string.Empty));
            Console.WriteLine($"Result is: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something is went wrong: {ex.Message}");
        }
    }

    //2
    static void GetWordCount()
    {
        Console.Write("Write text here:");
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
