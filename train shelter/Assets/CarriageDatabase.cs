using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 51, fileName = "New Carriage Database", menuName = "Train/Carriage Database")]
public class CarriageDatabase : ScriptableObject
{
    public List<CarriageData> carriages;


    [System.Serializable]
    public class CarriageData
    {
        public Sprite sprite;
        public ItemType type;
        public int defaultValue;
        public int maxWorkersValue;
        public float addMultyplier;
        public Inventory.Cost cost;
    }
}
