using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableTimeSpan
{
    [SerializeField] private int _hours;
    [SerializeField] private int _minutes;
    [SerializeField] private int _seconds;
    public int Hours { get { return _hours; } }
    public int Minutes { get { return _minutes;} }
    public int Seconds { get { return _seconds; }}

    public int TotalSeconds
    {
        get { return _seconds + _minutes * 60 + _hours * 3600; }
    }

    public TimeSpan AsTimeSpan
    {
        get { return new  TimeSpan(Hours, Minutes, Seconds);}
    }
}
