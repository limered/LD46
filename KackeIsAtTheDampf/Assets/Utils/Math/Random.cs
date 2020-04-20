using System;
using System.Runtime.InteropServices;
using UnityEngine;

// ReSharper disable NonReadonlyMemberInGetHashCode

// ReSharper disable InconsistentNaming

namespace Utils.Math
{

    public static class RandomHelpers
    {
        public static int RandomIndex<T>(this T[] arr)
        {
            return (int)Mathf.Floor(UnityEngine.Random.Range(0, arr.Length - 1));
        }
    }
}