using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Items SubLib")]
public class ItemsSubLib : ScriptableObject
{
    public Item[] Items;

    public Item this[int index]
    {
        get { return Items[index]; }
    }
    public Item GetByID(int ID)
    {
        return Items.First((i) => i.ItemID == ID);
    }
}
