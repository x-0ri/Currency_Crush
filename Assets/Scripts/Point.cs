using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public int x;
    public int y;
    public Point(int nx, int ny) {
        x = nx;
        y = ny;
    }
    public void Multiply_indirect(int m)
    {
        x *= m; 
        y *= m;
    }
    public void Add_indirect(Point p)
    {
        x += p.x;
        y += p.y;
    }
    public Vector2 ToVector()
    {
        return new Vector2(x, y);
    }
    public bool Equals(Point p) // funkcja sprawdzajaca rownosc
    {
        return (x == p.x && y == p.y); //zwraca true jezeli x = p.x i y = p.y
    }
    public static Point FromVector(Vector2 v)   
    {
        return new Point((int)v.x, (int)v.y);
    }
    public static Point FromVector(Vector3 v)
    {
        return new Point((int)v.x, (int)v.y);
    }
    public static Point Multiply(Point p, int m)
    {
        return new Point(p.x*m, p.y*m);
    }
    public static Point Add(Point p, Point o)
    {
        return new Point(p.x + o.x, p.y + o.y);
    }
    public static Point Clone(Point p)
    {
        return new Point(p.x, p.y);
    }
    public static Point Zero
    {
        get { return new Point(0, 0); }
    }
    public static Point One
    {
        get { return new Point(1, 1); }
    }
    public static Point Up
    {
        get { return new Point(0, 1); }
    }
    public static Point Down
    {
        get { return new Point(0, -1); }
    }
    public static Point Right
    {
        get { return new Point(1, 0); }
    }
    public static Point Left
    {
        get { return new Point(-1, 0); }
    }
}
