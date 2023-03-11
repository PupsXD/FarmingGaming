using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string ItemName;
    public int CountPerStack = 1;
    public int ItemID
    {
        get { return ItemName.GetHashCode(); }
    }
    public Sprite Icon;

}