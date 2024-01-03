using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


public struct Vector2
{
    public static Vector2 Origo = new Vector2(0, 0);

    public static Vector2 North = new Vector2(-1, 0);
    public static Vector2 South = new Vector2(1, 0);
    public static Vector2 East = new Vector2(0, 1);
    public static Vector2 West = new Vector2(0, -1);

    public static Vector2 NW = North + West;
    public static Vector2 NE = North + East;
    public static Vector2 SW = South + West;
    public static Vector2 SE = South + East;

    public static List<Vector2> CardinalDirections = new List<Vector2>() { North, South, East, West };

    public static List<Vector2> CardinalDirectionsNull = new List<Vector2>() { Origo, North, South, East, West };

    public static List<Vector2> Directions = new List<Vector2>() { North, South, East, West, NW, NE, SW, SE };

    public static List<Vector2> DirectionsNull = new List<Vector2>() { Origo, North, South, East, West, NW, NE, SW, SE };

    public long x;
    public long y;

    public Vector2(long y, long x)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2(string unparsed, bool yFirst = true)
    {
        y = long.Parse(unparsed.Split(",")[yFirst ? 0 : 1]);
        x = long.Parse(unparsed.Split(",")[yFirst ? 1 : 0]);
    }

    public override string ToString()
    {
        return y + "," + x;
    }

    public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.y + b.y, a.x + b.x);

    public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.y - b.y, a.x - b.x);

    public static Vector2 operator *(Vector2 a, int mult) => new Vector2(a.y*mult, a.x*mult);

    public static bool operator ==(Vector2 a, Vector2 b)
    {
        return a.y == b.y && a.x == b.x;
    }

    public static bool operator !=(Vector2 a, Vector2 b)
    {
        return a.y != b.y || a.x != b.x;
    }

}

