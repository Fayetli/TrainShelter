using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Train train;

    private void Awake() {
        train = FindObjectOfType<Train>();

    }

    private void Start() {
        Inventory.Instance.AddItem(ItemType.Human, 10);
    }


}
