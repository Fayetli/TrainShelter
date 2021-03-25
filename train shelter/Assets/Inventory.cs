using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ItemType
{
    Human,
    Food,
    Electric,
    Woods,
    Stones
}

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class Cost
    {
        public List<ItemSlot> items;

        public Cost Copy()
        {
            var cost = new Cost();
            cost.items = new List<ItemSlot>();
            this.items.ForEach(s =>
            {
                var tempSlot = new ItemSlot(s.type, s.value);
                cost.items.Add(tempSlot);
            });
            return cost;   
        }
    }

    [System.Serializable]
    public class ItemSlot
    {
        public ItemType type;
        public BigInteger value;
        public ItemSlot() { }
        public ItemSlot(ItemType type, BigInteger value)
        {
            this.type = type;
            this.value = value;
        }
    }
    public static Inventory Instance { get; private set; }
    public Dictionary<ItemType, BigInteger> items { get; private set; } = new Dictionary<ItemType, BigInteger>();

    public delegate void ItemDelegate(ItemType item, BigInteger amount);
    public ItemDelegate OnItemAmountChanged;
    public UnityEvent OnInventoryChanged;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (OnInventoryChanged == null)
            OnInventoryChanged = new UnityEvent();
    }


    public void AddItem(ItemType item, BigInteger value)
    {
        if (!items.ContainsKey(item))
            items.Add(item, value);
        else
            items[item] += value;

        OnItemAmountChanged?.Invoke(item, items[item]);
        OnInventoryChanged?.Invoke();
    }
    public void AddItem(ItemType item) => AddItem(item, 1);
    public void AddItemSlot(ItemSlot slot) => AddItem(slot.type, slot.value);

    public bool HasItem(ItemType item, BigInteger amount)
    {
        if (!items.ContainsKey(item))
            return false;
        return items[item] >= amount;
    }
    public bool HasItem(ItemType item) => HasItem(item, 1);
    public bool HasItemSlot(ItemSlot slot) => HasItem(slot.type, slot.value);
    public bool HasCost(Cost cost) => cost.items.TrueForAll(slot => HasItemSlot(slot));

    public void RemoveItem(ItemType item, BigInteger value)
    {
        if (!items.ContainsKey(item) || !HasItem(item, value))
            Debug.LogError("Contains item error");
        items[item] -= value;

        OnItemAmountChanged?.Invoke(item, items[item]);
        OnInventoryChanged?.Invoke();
    }
    public void RemoveItem(ItemType item) => RemoveItem(item, 1);
    public void RemoveItemSlot(ItemSlot slot) => RemoveItem(slot.type, slot.value);
    public void RemoveCost(Cost cost) => cost.items.ForEach(slot => RemoveItemSlot(slot));
}


