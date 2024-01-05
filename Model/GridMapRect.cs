using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


public class GridMapRect
{
    char[,] map;

    public GridMapRect(char[,] map)
    {
        this.map = map;
    }

    public GridMapRect(List<string> strings)
    {
        map = new char[strings.Count, strings[0].Length];

        for (int i = 0; i < strings.Count; i++)
            for(int j = 0; j < strings[i].Length; j++)
                map[i,j] = strings[i][j];
    }

    public GridMapRect(long rows, long cols, char init)
    {
        map = new char[rows, cols];
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                map[i,j] = init;
    }


    public char this[int y, int x]
    {
        get => map[y, x];
        set => map[y, x] = value;
    }

    public int Rows => map.GetLength(0);
    public int Cols => map.GetLength(1);

    public char this[Vector2 pair]
    {
        get
        {
            return map[pair.y, pair.x];
        }
        set
        {
            map[pair.y, pair.x] = value;
        }
    }

    public bool inBounds(Vector2 pair)
    {
        return pair.y >= 0 && pair.y < map.GetLength(0) && pair.x >= 0 && pair.x < map.GetLength(1);
    }

    public void Print()
    {
        for(int i = 0; i < map.GetLength(0); i++)
        {
            Console.WriteLine();
            for(int j = 0;j < map.GetLength(1); j++)
                Console.Write(map[i,j]);
        }

        Console.WriteLine();
    }

    public string FlattenMap()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
                sb.Append(map[i,j]);
        }

        return sb.ToString();

    }

}

