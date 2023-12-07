using System.Diagnostics.Metrics;
using System.Linq.Expressions;


class Program
{
    static void Main(string[] args)
    {
        long sum = 0;
        List<string> map = new List<string>();

        StreamReader sr = new StreamReader("C:/Users/Seth/source/repos/AoC2023/Input/day7.txt");


        long rowNr = 0;

        List<(string, int)> inputs = new List<(string, int)>();


        while (!sr.EndOfStream)
        {
            string input = sr.ReadLine();
            inputs.Add(new(input.Split(" ")[0], int.Parse(input.Split(" ")[1])));
        }

        List<(string, int)> orderedInputs = inputs.OrderBy(x => ProcessHand(x.Item1)).ToList();

        for (int i = 0; i < orderedInputs.Count; i++)
            sum += orderedInputs[i].Item2 * (i+1);


        Console.WriteLine(sum);
        Console.WriteLine("EoP");
        Console.ReadKey();
    }


    public static long ProcessHand(string hand)
    {
        char[] cards = new char[] {'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

        int[] counts = new int[cards.Length];
        int jokers = 0;

        for(int i = 0; i < hand.Length; i++)
        {
            counts[Array.IndexOf(cards, hand[i])]++;
            if (hand[i] == 'J') jokers++;
        }

        counts[0] = 0;

        List<int> orderedCounts = counts.OrderByDescending(x => x).ToList();

        int typeScore = 0;
        
        if (orderedCounts[0] == 5 || orderedCounts[0] + jokers == 5)
            typeScore = 7;
        else if (orderedCounts[0] == 4 || orderedCounts[0] + jokers == 4)
            typeScore = 6;
        else if ((orderedCounts[0] == 3 || orderedCounts[0] + jokers == 3) && orderedCounts[1] == 2)
            typeScore = 5;
        else if (orderedCounts[0] == 3 || orderedCounts[0] + jokers == 3)
            typeScore = 4;
        else if ((orderedCounts[0] == 2 || orderedCounts[0] + jokers == 2) && orderedCounts[1] == 2)
            typeScore = 3;
        else if (orderedCounts[0] == 2 || orderedCounts[0] + jokers == 2)
            typeScore = 2;
        

        long score = typeScore * IntPow(10, hand.Length*2+1);

        for (int i = 0; i < hand.Length; i++)
            score += IntPow(10, 2 * hand.Length - 2 * i) + (Array.IndexOf(cards, hand[i]) * IntPow(10, 2 * hand.Length - 2 * i - 1));


        Console.WriteLine(hand + ": " + score);

        return score;
    }

    static long IntPow(long x, long pow)
    {
        long ret = 1;
        while (pow != 0)
        {
            if ((pow & 1) == 1)
                ret *= x;
            x *= x;
            pow >>= 1;
        }
        return ret;
    }

    public static long lookupValueMap(List<(long dest, long source, long len)> map, long x)
    {
        foreach((long d, long s, long l) lookup in map)
        {
            if (x >= lookup.s && x <= lookup.s + lookup.l - 1)
                return lookup.d + (x - lookup.s);
        }

        return x;
    }

    public static int isTwoNumsClose(List<string> map, int i, int j)
    {
        List<int> nums = new List<int>();

        Console.WriteLine("foundnum start and scanning around " + i + "," + j);

        if (i - 1 >= 0)
        {
            for(int r = j -1; r <= j + 1 && r < map[i].Length; r++)
            {
                if (r < 0) continue;

                if (map[i - 1][r].ToString().IsNumeric())
                {
                    int StartPos = findStartPos(map, i - 1, r);
                    int EndPos = findEndPos(map, i -1 , r);

                    nums.Add(int.Parse(map[i - 1].Substring(StartPos, EndPos - StartPos + 1)));
                    r = EndPos;

                    Console.WriteLine("foundnum " + nums[nums.Count - 1]);
                }

            }
        }

        if (i + 1 <= map.Count)
        {
            for (int r = j - 1; r <= j +1 && r < map[i].Length; r++)
            {
                if (r < 0) continue;

                if (map[i + 1][r].ToString().IsNumeric())
                {
                    int StartPos = findStartPos(map, i + 1, r);
                    int EndPos = findEndPos(map, i + 1, r);

                    nums.Add(int.Parse(map[i + 1].Substring(StartPos, EndPos - StartPos + 1)));
                    r = EndPos;

                    Console.WriteLine("foundnum " + nums[nums.Count - 1]);
                }
            }
        }

        if(j -1 >= 0 && map[i][j - 1].ToString().IsNumeric())
        {
            int StartPos = findStartPos(map, i, j-1);
            int EndPos = findEndPos(map, i, j - 1);

            nums.Add(int.Parse(map[i].Substring(StartPos, EndPos - StartPos + 1)));
            Console.WriteLine("foundnum " + nums[nums.Count - 1]);
        }

        if (j + 1 <= map.Count && map[i][j + 1].ToString().IsNumeric())
        {
            int StartPos = findStartPos(map, i, j + 1);
            int EndPos = findEndPos(map, i, j + 1);

            nums.Add(int.Parse(map[i].Substring(StartPos, EndPos - StartPos + 1)));
            Console.WriteLine("foundnum " + nums[nums.Count - 1]);
        }


        if (nums.Count != 2) return 0;
        return nums[0] * nums[1];
    }

    public static int findStartPos(List<string> map, int i, int j)
    {
        int retPos = j;

        while (true)
        {
            j--;
            if (j < 0) return retPos;
            if (!map[i][j].ToString().IsNumeric()) return retPos;
            retPos = j;
        }
    }

    public static int findEndPos(List<string> map, int i, int j)
    {
        int retPos = j;

        while (true)
        {
            j++;
            if (j >= map[i].Length) return retPos;
            if (!map[i][j].ToString().IsNumeric()) return retPos;
            retPos = j;
        }
    }



    public static bool isSymbolClose(List<string> map, int startRow,  int startPos, int endPos)
    {

        for (int j = startPos - 1; j < endPos + 1; j++)
        {
            if (j < 0) continue;
            if (j >= map[startRow].Length) continue;
            
            if(startRow-1 >= 0)
                if (!map[startRow-1][j].ToString().IsNumeric() && !map[startRow-1][j].ToString().Equals("."))
                    return true;

            if (startRow + 1 < map.Count)
                if (!map[startRow + 1][j].ToString().IsNumeric() && !map[startRow + 1][j].ToString().Equals("."))
                    return true;

            if (!map[startRow][j].ToString().IsNumeric() && !map[startRow][j].ToString().Equals("."))
                return true;
        }

        return false;
    }


    public static List<int> AllIndexesOf(string str, string value)
    {
        if (String.IsNullOrEmpty(value))
            throw new ArgumentException("the string to find may not be empty", nameof(value));

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

