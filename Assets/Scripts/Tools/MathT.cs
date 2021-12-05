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
}
