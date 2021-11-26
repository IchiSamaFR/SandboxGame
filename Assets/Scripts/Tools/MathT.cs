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
}
