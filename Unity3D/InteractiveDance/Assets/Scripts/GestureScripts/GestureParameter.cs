using UnityEngine;
using System.Collections;
using System;

public class GestureParameter 
{

    private readonly float _min;
    private readonly float _max;
    private readonly float _unit;

    public GestureParameter(float min, float max)
    {
        _min = min;
        _max = max;
        _unit = (max - min) / 100;
    }
    public GestureParameter(int min, float max) : this((float)min, max){}
    public GestureParameter(float min, int max) : this(min, (float)max) { }
    public GestureParameter(int min, int max) : this((float)min, (float)max) { }

    public float GetUnit()
    {
        return _unit;
    }

    public float Clamp(float value)
    {
        return Mathf.Clamp(value, _min, _max);
    }
    public int Clamp(int value)
    {
        return Mathf.Clamp(value, (int)_min, (int)_max);
    }
}
