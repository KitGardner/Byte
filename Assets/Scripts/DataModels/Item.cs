using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Item : MonoBehaviour {

    public Guid ItemId { get; set; }
    public string ItemName { get; set; }
    public int BuyValue { get; set; }
    public int SellValue { get; set; }
    public string ItemDescription { get; set; }
    public string Notes { get; set; }
    public Category Category { get; set; }
    public string AffectedStat { get; set; }
    public int RestorativeAmount { get; set; }

    public int Count { get; set; }


    public const int MaxCount = 99;


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

public enum Category
{
    Consumable = 10,
    Quest = 20,
    Collectable = 30,
}
