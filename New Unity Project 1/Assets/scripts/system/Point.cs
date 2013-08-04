using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Point
{
    public int x;
    public int y;

    public static readonly Point zero = new Point();

    public Point()
    {
        x = 0;
        y = 0;
    }

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    } 
}
