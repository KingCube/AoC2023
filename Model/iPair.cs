using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


public struct iPair
{
    public static iPair Origo = new iPair(0, 0);

    public static iPair North = new iPair(0, -1);
    public static iPair South = new iPair(0, 1);
    public static iPair East = new iPair(1, 0);
    public static iPair West = new iPair(-1, 0);

    public static iPair NW = North + West;
    public static iPair NE = North + East;
    public static iPair SW = South + West;
    public static iPair SE = South + East;

    public static List<iPair> CardinalDirections = new List<iPair>()
    {
        North, South, East, West
    };

    public static List<iPair> CardinalDirectionsNull = new List<iPair>()
    {
        Origo, North, South, East, West
    };

    public static List<iPair> Directions = new List<iPair>()
    {
        North, South, East, West, NW, NE, SW,SE
    };

    public static List<iPair> CardinalValues = new List<iPair>()
    {
        Origo, North, South, East, West, NW, NE, SW,SE
    };


    public int x;
    public int y;

    public iPair(int y, int x)
    {
        this.x = x;
        this.y = y;
    }

    public static iPair operator +(iPair a, iPair b)
    {
        return new iPair(a.y + b.y, a.x + b.x);
    }

    public static iPair operator -(iPair a, iPair b)
    {
        return new iPair(a.y - b.y, a.x - b.x);
    }

}

