using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CarriageType{
    Food,
    Electric,
    Wood,
    Stone
}

[CreateAssetMenu(order=51, fileName = "New Carriage Database", menuName = "Train/Carriage Database")]
public class CarriageDatabase : ScriptableObject
{
    public List<CarriageData> carriages;


    [System.Serializable]
    public class CarriageData
    {
        public CarriageType type;
        public int maxWorkersValue;
    }
}
