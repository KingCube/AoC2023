using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Utils
{
    public static List<int> ParseLineToInts(string line, string separator = ",")
    {
        List<int> retList = new List<int>();

        foreach (string s in line.Split(separator))
        {
            retList.Add(int.Parse(s));
        }

        return retList;
    }
}

