using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Train train;

    private void Awake() {
        train = FindObjectOfType<Train>();

        InstantiateInventory();
    }


    private void InstantiateInventory(){
        Inventory.Instance.AddItem(ItemType.Human, 5);
        Inventory.Instance.AddItem(ItemType.Food, 5);
        Inventory.Instance.AddItem(ItemType.Woods, 5);
        Inventory.Instance.AddItem(ItemType.Stones, 5);
        Inventory.Instance.AddItem(ItemType.Electric, 5);
    }

}
