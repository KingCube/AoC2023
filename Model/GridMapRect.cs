using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

    public char this[int y, int x] => map[y, x];

    public char this[iPair pair] => map[pair.y, pair.x];   

}

