using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public struct Vector3
{
    public static Vector3 Down = new Vector3(0, 0,- 1);


    public long x;
    public long y;
    public long z;

    public Vector3(long x, long y, long z)
    {
        this.y = y; this.x = x; this.z = z;
    }

    public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.y + b.y, a.x + b.x, a.z + b.z);

    public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.y - b.y, a.x - b.x, a.z - b.z);

    public static bool operator ==(Vector3 a, Vector3 b)
    {
        return a.y == b.y && a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(Vector3 a, Vector3 b)
    {
        return a.y != b.y || a.x != b.x || a.z != b.z;
    }
}

