using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 52, fileName = "New Items Database", menuName = "Items/Items Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;
    [System.Serializable]
    public class ItemData{
        public ItemType type;
        public Sprite sprite;
    }
    public Sprite GetSprite(ItemType type) => items.Find(i => i.type == type).sprite;
    
}
