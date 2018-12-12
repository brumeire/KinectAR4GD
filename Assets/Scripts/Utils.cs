using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static ushort Remap(ushort minOld, ushort maxOld, ushort minNew, ushort maxNew, ushort value)
    {
        return (ushort)(minNew + (value - minOld) * (maxNew - minNew) / (maxOld - minOld));
    }
}
