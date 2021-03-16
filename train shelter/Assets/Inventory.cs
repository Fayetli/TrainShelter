using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    Human,
    Food,
    Electric,
    Woods,
    Stones
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance{ 
        get
        {
            if(inventory == null)
                inventory = new Inventory();
            return inventory;
        }
    }

    private static Inventory inventory;

    public Dictionary<ItemType, int> items {get; private set;} = new Dictionary<ItemType, int>();

    public void AddItem(ItemType item, int value){
        items.Add(item, value);
        Debug.Log(items[item]);
    }
}
