using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CompressedNode
{
    public Dictionary<Vector2, int> costs = new Dictionary<Vector2, int>();
    public List<Vector2> connected  = new List<Vector2>();

    public  Vector2 id;

    public override string ToString()
    {
        return id.ToString();
    }

    public void CalculateConnections(GridMapRect gMap, Dictionary<Vector2,CompressedNode> allNodes)
    {
        foreach (Vector2 dir in GetDirections(gMap, id, Vector2.Origo))
        {
            Vector2 newPos = id + dir;
            Vector2 lastDir = dir;
            int cost = 1;

            while (true)
            {
                List<Vector2> newDirs = GetDirections(gMap, newPos, lastDir);
                if(newDirs.Count != 1)
                {
                    connected.Add(newPos);
                    costs[newPos] = cost;
                    break;
                }
                else if(newDirs.Count == 1) 
                {
                    newPos = newPos + newDirs[0];
                    cost++;
                    lastDir = newDirs[0];
                }
            }
        }
    }

    public static List<Vector2> GetDirections (GridMapRect gMap, Vector2 pos, Vector2 originDir)
    {
        List<Vector2> dirs = new List<Vector2>(Vector2.CardinalDirections);
        List<Vector2> retList = new List<Vector2>();

        dirs.Remove(originDir * -1);

        foreach (Vector2 dir in dirs)
        {
            Vector2 cand = pos + dir;
            if (!gMap.inBounds(cand)) continue;
            if (gMap[cand] == '#') continue;

            retList.Add(dir);
        }

        return retList;
    }
}

