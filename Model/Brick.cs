using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


public class Brick
{


    public string Name;

    public override string ToString()
    {
        return Name;
    }

    public Vector3 Start;
    public Vector3 End;

    public List<Brick> supportedBy = new List<Brick>();
    public List<Brick> supports = new List<Brick>();


    public Brick (Vector3 start, Vector3 end)
    {
        Start = start;
        End = end;
    }

    public bool MoveDownOneClick()
    {
        if (Start.z == 1 || End.z == 1) return false;

        Start.z--; End.z--;
        return true;
    }

    public List<Vector3> GetZPlane(bool max)
    {
        List<Vector3> result = new List<Vector3>();
        long startX = Math.Min(Start.x, End.x);
        long EndX = Math.Max(Start.x, End.x);
        long startY = Math.Min(Start.y, End.y);
        long endY = Math.Max(Start.y, End.y);

        long RoofFloor = max ? Math.Max(Start.z, End.z) : Math.Min(Start.z, End.z);

        for(long i = startX; i <= EndX; i++)
        {
            for(long j = startY; j <= endY; j++)
            {
                result.Add(new Vector3(i, j, RoofFloor));
            }
        }

        return result;
    }

    public bool IsSupporting(Brick other)
    {
        List<Vector3> tp = GetZPlane(true);
        foreach(Vector3 op in other.GetZPlane(false))
        {
            if (tp.Contains(op + Vector3.Down))
                return true;
        }

        return false;
    }

    public long LowestCoord => Math.Min(Start.z, End.z);
}

