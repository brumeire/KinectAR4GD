using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static ushort Remap(ushort minOld, ushort maxOld, ushort minNew, ushort maxNew, ushort value)
    {
        return (ushort) (minNew + (value - minOld) * (maxNew - minNew) / (maxOld - minOld));
    }

    public static float Remap(float minOld, float maxOld, float minNew, float maxNew, float value)
    {
        return (float)(minNew + (value - minOld) * (maxNew - minNew) / (maxOld - minOld));
    }

    /*public static int Remap(int minOld, int maxOld, int minNew, int maxNew, int value)
    {
        return (int)(minNew + (value - minOld) * (maxNew - minNew) / (maxOld - minOld));
    }*/
}
