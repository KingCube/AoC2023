using AoC2023.Model._2021;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO.Pipes;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;


public class Program
{
    static void Main(string[] args)
    {
        long sum = 0;

        StreamReader sr = new StreamReader("..\\..\\..\\Input\\day21_18x.txt");


        while (!sr.EndOfStream)
        {
            string input = sr.ReadLine();
        }

        Vector2 LowLims = new Vector2(-248, 29);
        Vector2 MaxLims = new Vector2(-194, 73);
        //Vector2 LowLims = new Vector2(-10, 20);
        //Vector2 MaxLims = new Vector2(-5, 30);

        for (long i = LowLims.y; i < -LowLims.y; i++)
        {
            for(int j = 0; j <= MaxLims.x; j++)
            {
                int vY = (int)i;
                int vX = (int)j;
                int y = 0;
                int x = 0;
                while(y > LowLims.y && x < MaxLims.x)
                {
                    y += vY--;
                    x += Math.Max(vX--, 0);

                    if (x >= LowLims.x && x <= MaxLims.x && y >= LowLims.y && y <= MaxLims.y)
                    {
                        sum++;
                        break;
                    }
                }
            }
        }

        Console.WriteLine(sum);
        Console.ReadKey();
    }

    public static int FindPathsCaves(List<CaveNode> visitedSmall, CaveNode cur, bool usedUpBonus)
    {
        if (cur.id == "end") return 1;

        int paths = 0;
        foreach(CaveNode conn in cur.conns) 
        {
            bool newUsedUpBonus = usedUpBonus;
            if (visitedSmall.Contains(conn))
            {
                if (conn.id == "start") continue;
                if (usedUpBonus) continue;
                newUsedUpBonus = true;
            }

            List<CaveNode> newVisitedSmall = new List<CaveNode>(visitedSmall);
            if(!conn.isLarge) newVisitedSmall.Add(conn);

            paths += FindPathsCaves(newVisitedSmall, conn, newUsedUpBonus);
        }

        return paths;
    }

    public static int BinaryToInt(string data)
    {
        Console.WriteLine("Calculating binary on:" + data);
        int sum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            int c = data.Length - i - 1;
            sum += int.Parse(data[c].ToString())*(int)Math.Pow(2, i);
        }
        return sum;
    }


    public static int DivideGraph(List<(string, string)> allEdges, List<string> selected, List<(string, string)> outEdges)
    {
        if (outEdges.Count == 3) return selected.Count;

        List<string> orderedOut = outEdges.Select(x => x.Item1).Where(x=> !selected.Contains(x)).ToList();
        orderedOut.AddRange(outEdges.Select(x => x.Item2).Where(x => !selected.Contains(x) && !orderedOut.Contains(x)).ToList());

        List<string> orderedOut2 = orderedOut.OrderByDescending(x => outEdges.Count(o => o.Item1 == x) + outEdges.Count(o => o.Item2 == x)).ToList();

        Console.WriteLine(selected.Count);
        for(int i = 0; i < orderedOut2.Count; i++)
        {
            List<string> newSelected = new List<string>(selected);
            string newNode = orderedOut2[i];
            
            newSelected.Add(newNode);

            List<(string, string)> newOutEdges = new List<(string, string)>();

            // remove intra connections
            foreach((string, string) e in outEdges)
            {
                if (newSelected.Contains(e.Item1) && newSelected.Contains(e.Item2)) continue;
                newOutEdges.Add(e);
            }

            // add new out
            foreach((string, string) e in allEdges)
            {
                if(
                    (e.Item1 == newNode && !newSelected.Contains(e.Item2)) 
                    ||
                    (e.Item2 == newNode && !newSelected.Contains(e.Item1))) 
                        newOutEdges.Add(e); 
            }
            

            int ans = DivideGraph(allEdges, newSelected,  newOutEdges);
            if(ans != - 1) return ans;
        }

        return -1;
    }



    public static List<string> Connected(List<(string, string)> edges, string root)
    {
        List<string> retList = new List<string>();

        List<string> frontier = new List<string>();
        frontier.Add(root);
        retList.Add(root);

        while(frontier.Count != 0)
        {
            List<string> newFront = new List<string>();

            foreach(string s in frontier)
            {
                foreach(string l in edges.Where(x=> x.Item1 == s).Select(x => x.Item2).ToList())
                {
                    if (retList.Contains(l) || newFront.Contains(l)) continue;
                    newFront.Add(l);
                }

                foreach (string l in edges.Where(x => x.Item2 == s).Select(x => x.Item1).ToList())
                {
                    if (retList.Contains(l) || newFront.Contains(l)) continue;
                    newFront.Add(l);
                }
            }

            retList.AddRange(newFront);
            frontier = newFront;
        }

        return retList;
    }


    public static bool isForwardTime(Vector3 pos, Vector3 vel, double xVal, double yVal)
    {
        if (vel.x >= 0 && xVal < pos.x) return false;
        if (vel.x < 0 && xVal > pos.x) return false;
        if (vel.y >= 0 && yVal < pos.y) return false;
        if (vel.y < 0 && yVal > pos.y) return false;

        return true;

    }










    public static bool OneWayOnly(GridMapRect gMap, Vector2 pos, Vector2 originDir, out Vector2 onlyWay, List<Vector2> visited)
    {
        List<Vector2> dirs = new List<Vector2>(Vector2.CardinalDirections);
        dirs.Remove(originDir*-1);

        onlyWay = Vector2.Origo;

        int possibleWays = 0;

        foreach(Vector2 dir in dirs)
        {
            Vector2 newPos = pos + dir;
            if (!gMap.inBounds(newPos)) continue;
            if (gMap[newPos] == '#') continue;
            if(visited.Contains(newPos)) continue;
            possibleWays++;
            onlyWay = dir; 
        }

        if (possibleWays != 1) onlyWay = Vector2.Origo;

        return possibleWays == 1;
    }


    public static int GetEnergized(GridMapRect gMap, (Vector2 pos, Vector2 dir) origo)
    {
        HashSet<Vector2> energized = new HashSet<Vector2>();
        HashSet<(Vector2, Vector2)> visited = new HashSet<(Vector2 pos, Vector2 dir)>();

        List<(Vector2, Vector2)> frontier = new List<(Vector2 pos, Vector2 dir)>();
        frontier.Add(origo);
        

        while (frontier.Count() != 0)
        {
            List<(Vector2, Vector2)> newFrontier = new List<(Vector2 pos, Vector2 dir)>();

            foreach ((Vector2 pos, Vector2 dir) cur in frontier)
            {
                if (visited.Contains(cur)) continue;
                visited.Add(cur);

                Vector2 newPos = cur.pos + cur.dir;
                if (!gMap.inBounds(newPos)) continue;

                char c = gMap[newPos];
                if (c == '.')
                    newFrontier.Add((newPos, cur.dir));
                else if (c == '/')
                {
                    if (cur.dir == Vector2.East)
                        newFrontier.Add((newPos, Vector2.North));
                    else if (cur.dir == Vector2.South)
                        newFrontier.Add((newPos, Vector2.West));
                    else if (cur.dir == Vector2.West)
                        newFrontier.Add((newPos, Vector2.South));
                    else if (cur.dir == Vector2.North)
                        newFrontier.Add((newPos, Vector2.East));
                }
                else if (c == '\\')
                {
                    if (cur.dir == Vector2.East)
                        newFrontier.Add((newPos, Vector2.South));
                    else if (cur.dir == Vector2.South)
                        newFrontier.Add((newPos, Vector2.East));
                    else if (cur.dir == Vector2.West)
                        newFrontier.Add((newPos, Vector2.North));
                    else if (cur.dir == Vector2.North)
                        newFrontier.Add((newPos, Vector2.West));
                }
                else if (c == '-')
                {
                    if (cur.dir == Vector2.East || cur.dir == Vector2.West)
                        newFrontier.Add((newPos, cur.dir));
                    else
                    {
                        newFrontier.Add((newPos, Vector2.West));
                        newFrontier.Add((newPos, Vector2.East));
                    }
                }
                else if (c == '|')
                {
                    if (cur.dir == Vector2.North || cur.dir == Vector2.South)
                        newFrontier.Add((newPos, cur.dir));
                    else
                    {
                        newFrontier.Add((newPos, Vector2.North));
                        newFrontier.Add((newPos, Vector2.South));
                    }
                }
            }


            foreach ((Vector2, Vector2) c in newFrontier)
                energized.Add(c.Item1);

            frontier = newFrontier;
        }

        return energized.Count();

    }


    public static void RollNorth(GridMapRect gMap)
    {
        for (int i = 0; i < gMap.Rows; i++)
        {
            for (int j = 0; j < gMap.Cols; j++)
            {
                if (gMap[i, j] == 'O')
                {
                    Vector2 cursor = new Vector2(i, j);
                    while (true)
                    {
                        Vector2 oldCursor = cursor;
                        cursor += Vector2.North;
                        if (!gMap.inBounds(cursor) || gMap[cursor] != '.') break;

                        gMap[cursor] = 'O';
                        gMap[oldCursor] = '.';
                    }
                }
            }
        }
    }

    public static void RollSouth(GridMapRect gMap)
    {
        for (int i = gMap.Rows -1 ; i >= 0; i--)
        {
            for (int j = 0; j < gMap.Cols; j++)
            {
                if (gMap[i, j] == 'O')
                {
                    Vector2 cursor = new Vector2(i, j);
                    while (true)
                    {
                        Vector2 oldCursor = cursor;
                        cursor += Vector2.South;
                        if (!gMap.inBounds(cursor) || gMap[cursor] != '.') break;

                        gMap[cursor] = 'O';
                        gMap[oldCursor] = '.';
                    }
                }
            }
        }
    }

    public static void RollEast(GridMapRect gMap)
    {
        for (int i = gMap.Cols -1 ; i >= 0; i--)
        {
            for (int j = 0; j < gMap.Rows; j++)
            {
                if (gMap[j, i] == 'O')
                {
                    Vector2 cursor = new Vector2(j, i);
                    while (true)
                    {
                        Vector2 oldCursor = cursor;
                        cursor += Vector2.East;
                        if (!gMap.inBounds(cursor) || gMap[cursor] != '.') break;

                        gMap[cursor] = 'O';
                        gMap[oldCursor] = '.';
                    }
                }
            }
        }
    }

    public static void RollWest(GridMapRect gMap)
    {
        for (int i = 1; i < gMap.Cols; i++)
        {
            for (int j = 0; j < gMap.Rows; j++)
            {
                if (gMap[j, i] == 'O')
                {
                    Vector2 cursor = new Vector2(j, i);
                    while (true)
                    {
                        Vector2 oldCursor = cursor;
                        cursor += Vector2.West;
                        if (!gMap.inBounds(cursor) || gMap[cursor] != '.') break;

                        gMap[cursor] = 'O';
                        gMap[oldCursor] = '.';
                    }
                }
            }
        }
    }

    public static int FindMirror(List<string> map)
    {
        // start horizontal
        for (int i = 1; i < map.Count; i++)
        {
            int bRow = i - 1;
            int uRow = i;

            int diffs = 0;

            while (bRow >= 0 && uRow < map.Count)
            {
                for(int r = 0; r < map[0].Length; r++)
                {
                    if (map[bRow][r] != map[uRow][r])
                    {
                        diffs++;
                    }
                }

                bRow--;
                uRow++;
            }

            if(diffs == 1)
            {
                return 100 * i;
            }
        }

        // vertical
        for(int j = 1; j < map[0].Length; j++)
        {
            int bCol = j - 1;
            int uCol = j;

            int diffs = 0;

            while (bCol >= 0 && uCol < map[0].Length)
            {
                for(int c = 0; c < map.Count; c++)
                {
                    if (map[c][bCol] != map[c][uCol])
                    {
                        diffs++;
                    }
                }

                bCol--;
                uCol++;
            }

            if (diffs == 1) return j;
        }

        return 0;
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

