using System.Diagnostics.Metrics;

class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, int> replacer = new Dictionary<string, int>();
        replacer.Add("one", 1);
        replacer.Add("two", 2);
        replacer.Add("three", 3);
        replacer.Add("four", 4);
        replacer.Add("five", 5);
        replacer.Add("six", 6);
        replacer.Add("seven", 7);
        replacer.Add("eight", 8);
        replacer.Add("nine", 9);

        // See https://aka.ms/new-console-template for more information
        int sum = 0;

        StreamReader sr = new StreamReader("C:/Users/Seth/source/repos/AoC2023/Input/day1x.txt");

        while (!sr.EndOfStream)
        {
            int first = 0;
            int last = 0;

            string input = sr.ReadLine();
            int[] numsfound = new int[input.Length];

            foreach (string key in replacer.Keys)
            {
                List<int> allFound = AllIndexesOf(input, key);
                foreach (int x in allFound)
                    numsfound[x] = replacer[key];
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].ToString().IsNumeric())
                {
                    numsfound[i] = int.Parse(input[i].ToString());
                }
            }

            for (int i = 0; i < numsfound.Length; i++)
            {
                if (numsfound[i] == 0) continue;

                if (first == 0)
                    first = numsfound[i];

                last = numsfound[i];
            }

            Console.WriteLine(first + "," + last);

            sum += int.Parse(String.Concat(first, last));
        }

        Console.WriteLine(sum);
        Console.WriteLine("EoP");
        Console.ReadKey();
    }


    public static List<int> AllIndexesOf(string str, string value)
    {
        if (String.IsNullOrEmpty(value))
            throw new ArgumentException("the string to find may not be empty", "value");

        List<int> indexes = new List<int>();

        for (int index = 0; ; index += value.Length)
        {
            index = str.IndexOf(value, index);

            if (index == -1)
                return indexes;

            indexes.Add(index);
        }
    }
}





public static class StringExt
{
    public static bool IsNumeric(this string text) => double.TryParse(text, out _);
}

