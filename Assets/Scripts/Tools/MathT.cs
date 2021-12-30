using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathT
{
    public static bool IntBetween(int value, int a, int b)
    {
        if(value >= a && value < b)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool FloatBetween(float value, float a, float b)
    {
        if (value >= a && value < b)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static int DistanceCost(Vector3 a, Vector3 b)
    {
        int x = (int)(a.x - b.x >= 0 ? a.x - b.x : b.x - a.x);
        int z = (int)(a.z - b.z >= 0 ? a.z - b.z : b.z - a.z);

        int val = 0;
        if (x - z >= 0)
        {
            val += x * 10;
            val += (x - (x - z)) * 4;
        }
        else
        {
            val += z * 10;
            val += (z - (z - x)) * 4;
        }

        return val;
    }
}
