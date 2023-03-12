using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemConsumer
{
    public bool CanConsume(Item item);
    public void Consume(int sourceInventorySlot);
}
