using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats : ScriptableObject
{
    public abstract int MaxHealth { get; }
    public abstract float MoveSpeed { get; }
}
