using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMathUtils
{
    private static System.Random rd = new System.Random();

    public static float horizontalDistance(Vector3 v1, Vector3 v2)
    {
        Vector3 d = new Vector3(v1.x - v2.x, 0, v1.z - v2.z);
        return d.magnitude;
    }

    public static float verticalDistance(Vector3 v1, Vector3 v2)
    {
        return Mathf.Abs(v1.y - v2.y);
    }

    public static int IntRandom(int min, int max)
    {
        return rd.Next(min, max);
    }
}
