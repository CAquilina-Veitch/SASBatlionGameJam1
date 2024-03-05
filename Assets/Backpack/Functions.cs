using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Functions
{
    public static Vector2 ToPos(this Vector2Int coord)
    {
        return new Vector2(coord.x - 4, coord.y - 4);
    }
    public static Vector2Int ToCoord(this Vector2 pos)
    {
        return new Vector2Int((int)pos.x + 4, (int)pos.y + 4);
    }
    public static Color toHalf(this Color clr)
    {
        return new Color(clr.r, clr.g, clr.b, 0.25f);
    }
    public static Color toFull(this Color clr)
    {
        return new Color(clr.r, clr.g, clr.b, 1);
    }
    public static List<Vector2Int> Dirs
    {
        get
        {
            return new List<Vector2Int>(new Vector2Int[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left });
        }
    }
    public static Vector2Int Total(this List<Vector2Int> dirs)
    {
        Vector2Int total = Vector2Int.zero;
        foreach(Vector2Int dir in dirs)
        {
            total += dir;
        }
        return total;
    }
    public static Card Rand(this List<Card> a)
    {
        return a[Random.Range(0,a.Count)];
    }
}