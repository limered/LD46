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
            if (arr == null) throw new System.Exception("cannot get random index of null array");
            if (arr.Length == 0) throw new System.Exception("cannot get random index of empty array");
            if (arr.Length == 1) return 0;
            return (int)Mathf.Floor(UnityEngine.Random.Range(0, arr.Length - 1));
        }
    }

    [Serializable]
    public struct RandomFloatRange
    {
        public float Min;
        public float Max;


        public RandomFloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Value
        {
            get
            {
                return UnityEngine.Random.Range(Min, Max);
            }
        }
    }
}