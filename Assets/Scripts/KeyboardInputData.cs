using System;
using UnityEngine;

[Serializable]
public struct KeyboardInputData
{
    public float Time;
    public float X;
    public float Y;

    public override string ToString()
    {
        return $"Time: {Time}, Input: {X} : {Y}";
    }
}

