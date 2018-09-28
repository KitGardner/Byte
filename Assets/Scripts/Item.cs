using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public string ItemName { get; set; }
    public int ItemId { get; set; }
    public int Count { get; set; }
    public int MaxCount { get; set; }
    public string Description { get; set; }
    public ItemType ItemType;
    public int InventoryId { get; set; }


    public bool IncrementCount()
    {
        int modifiedCount = Count + 1;
        if(modifiedCount > MaxCount)
        {
            return false;
        }

        Count = modifiedCount;
        return true;

    }
}

public enum ItemType
{
    Recovery = 10,
    Quest = 20,
    Collectable = 30,
    Weapon = 40
}
