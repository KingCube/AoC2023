using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;


class Program
{

    public static long[,,,] memory;

    static void Main(string[] args)
    {
        Dictionary<string, TreeNodeAAA> Nodes = new Dictionary<string, TreeNodeAAA>();

        long sum = 0;
        List<string> map = new List<string>();

        StreamReader sr = new StreamReader("C:/Users/Seth/source/repos/AoC2023/Input/day12.txt");

        int rowNr = 0;

        while (!sr.EndOfStream)
        {
            string input = sr.ReadLine();

            string row = input.Split(" ")[0];
            string strNums = input.Split(" ")[1];

            bool repeat = true;

            string oldRow = row;
            string oldNums = strNums;

            if (repeat)
            {
                for (int i = 0; i < 4; i++)
                {
                    row +="?" + oldRow;
                    strNums += "," + oldNums;
                }
            }
            
            row = row + ".";



            string[] nums = strNums.Split(",");

            List<int> limits = new List<int>();

            for (int i = 0; i < nums.Length; i++)
                limits.Add(int.Parse(nums[i]));

            List<int> qs = new List<int>();
            for (int i = 0; i < row.Length; i++)
                if (row[i] == '?')
                    qs.Add(i);


            memory = new long[row.Length, 100, 40,2];

            long ans = Flipper(row, limits, 0, 0, 0);

            Console.WriteLine(ans);
            sum += ans;
        }

     

        Console.WriteLine(sum);
        Console.WriteLine("EoP");
        Console.ReadKey();
    }

    public static long Flipper(string input, List<int> limits, int pos, int currentLen, int finishedGroups)
    {
        long ans = 0;


        if (input[pos] == '?')
        {
            char[] chars = input.ToCharArray();

            chars[pos] = '#';
            ans += Flipper(new string(chars), limits, pos, currentLen, finishedGroups);

            chars[pos] = '.';
            ans += Flipper(new string(chars), limits, pos, currentLen, finishedGroups);
        }
        else {

            int newCurrentLen = input[pos] == '.' ? 0 : currentLen + 1;
            int newFinishedGroup = finishedGroups + (input[pos] == '.' && currentLen > 0 ? 1 : 0);

            //Console.WriteLine("from memory:" + memory[pos, newFinishedGroup, newCurrentLen]);

            int isSpring = input[pos] == '#' ? 1 : 0;

            if (memory[pos, finishedGroups, currentLen, isSpring] == -1)
                return 0;
            else if (memory[pos, finishedGroups, currentLen, isSpring] > 0) 
               return memory[pos, finishedGroups, currentLen, isSpring];

            if (input[pos] == '.')
            {
                List<int> newLimits = new List<int>(limits);
                if (currentLen > 0)
                {
                    if (limits[0] != currentLen)
                    {
                        memory[pos, finishedGroups, currentLen, isSpring] = -1;
                        return 0;
                    }

                    newLimits.RemoveAt(0);
                }


                if (pos == input.Length - 1)
                {
                    return (newLimits.Count == 0) ? 1 : 0;
                }

                ans += Flipper(input, newLimits, pos + 1, newCurrentLen, newFinishedGroup);
            }
            else if (input[pos] == '#')
            {
                if (limits.Count == 0 || newCurrentLen > limits[0])
                {
                    memory[pos, finishedGroups, currentLen, isSpring] = -1;
                    return 0;
                }

                ans += Flipper(input, limits, pos + 1, newCurrentLen, newFinishedGroup);
            }

            if(ans == 0)
                memory[pos, finishedGroups, currentLen, isSpring] = -1;
            else
                memory[pos, finishedGroups, currentLen, isSpring] = ans;

        }
        

        return ans;
    }






    public static List<int> GetDiffList(List<int> nums) {
        List<int> diffs = new List<int>();

        for(int i = 1; i < nums.Count; i++)
            diffs.Add(nums[i] - nums[i-1]);

        if(diffs.Count(x=> x == 0) == diffs.Count())
        {
            diffs.Add(0);
            return diffs;
        }
        else
        {
            List<int> subDiffs = GetDiffList(diffs);
            if(subDiffs.Count == 0)
            {
                diffs.Add(0);
            }
            else
            {
                diffs.Add(subDiffs[diffs.Count() - 1] + diffs[diffs.Count-1]);
            }

            return diffs;
        }

    }


    public static void PrintMap(char[,] map, int startRow = 0, int EndRow = -1)
    {
        int finalEndRow = EndRow == -1 ? map.GetLength(0) : EndRow;
        for (int i = startRow; i < finalEndRow; i++)
        {
            for(int j = 0; j < map.GetLength(1); j++)
                Console.Write(map[i, j] + " ");

            Console.WriteLine();
        }
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

