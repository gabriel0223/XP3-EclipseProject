using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatEM 
{
    public static bool Between(this float value, int left, int right)
    { 
        return value > left && value < right; 
    }
}
