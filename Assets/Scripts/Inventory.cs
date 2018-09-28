using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] items { get; set; }

    public int InventoryId { get; set; }

    public int GameSaveId { get; set; }
}
