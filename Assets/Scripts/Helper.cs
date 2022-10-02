using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse2D
{
    public static class Helper
    {
        // public static T InitializeJaggedArray<T>(params int[] lengths)
        // {
        //     Type elementType = typeof(T).GetElementType();
        //     Array arr = Array.CreateInstance(elementType,lengths.Length);
        //     elementType = elementType.GetElementType();
        //     for (int i = 0; i < arr.Length; i++)
        //     {
        //         arr.SetValue(Array.CreateInstance(elementType,lengths[i]),i);
        //     }
        //     return (T)((object)arr);
        // }
        public static T CreateJaggedArray<T>(params int[] lengths)
        {
            return (T)InitializeJaggedArray(typeof(T).GetElementType(), 0, lengths);
        }

        static object InitializeJaggedArray(Type type, int index, int[] lengths)
        {
            Array array = Array.CreateInstance(type, lengths[index]);
            Type elementType = type.GetElementType();

            if (elementType != null)
            {
                for (int i = 0; i < lengths[index]; i++)
                {
                    array.SetValue(
                        InitializeJaggedArray(elementType, index + 1, lengths), i);
                }
            }

            return array;
        }

        public static bool CheckJaggedArray2IfIndexIsValid<T>(this T[][] array, int x, int y)
        {
            if (array == null) //|| x >= array.Length || array[0] == null || y >= array[0].Length || x < 0 || y < 0
            {
                return false;
            }
            return ValidateCoordinates(x, y, array[0].Length, array.Length);
            
        }

        public static bool ValidateCoordinates(int x, int y, int width, int height)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return false;
            return true;
        }
    }
}
