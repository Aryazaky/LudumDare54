using System;
using UnityEngine;

namespace Gespell.Utilities
{
    public static class Extensions
    {
        public static float ToFloat(this int value)
        {
            return value;
        }

        public static int FloorToInt(this float value)
        {
            return Mathf.FloorToInt(value);
        }

        public static int MaxIndex(this Array array)
        {
            if(array.Length == 0) Debug.LogWarning("Array is empty");
            return array.Length - 1;
        }
    }
}