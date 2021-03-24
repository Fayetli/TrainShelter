using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Numerics;
using System.Collections;

public enum CarriageStatus
{
    Inactive, Active
}

public interface ITrainPart
{
    public CarriageStatus Status { get; }
    void Init(int index, CarriageStatus status, CarriageDatabase.CarriageData data, Train train, int lvl = 1);
    (ItemType type, BigInteger value) Realize { get; }
}

[RequireComponent(typeof(UICarriage))]
public class Carriage : MonoBehaviour, ITrainPart
{
    #region Init
    private Train train;
    [SerializeField] private int index;
    public ItemType ItemType { get; private set; }
    public CarriageStatus Status { get; private set; }
    public Inventory.Cost BuyCost { get; private set; }
    private int maxWorkersValue;
    private int defaultValue;
    [SerializeField] private UICarriage ui;
    #endregion

    #region Realizing
    public int workers;
    public float addMultyplier = 0;
    public int lvl { get; private set; }
    public (ItemType type, BigInteger value) Realize
    {
        get => (ItemType, (BigInteger)((1 + addMultyplier * lvl) * workers * defaultValue));
    }

    #endregion

    #region Delegates
    public delegate void StatusChangeDelegate(CarriageStatus status);
    StatusChangeDelegate OnStatusChanged;

    public IntUnityEvent OnWorkersCountChanged;
    public IntUnityEvent OnLvlValueChanged;
    public BigIntUnityEvent OnResourcePreTikChanged;
    [System.Serializable]
    public class IntUnityEvent : UnityEvent<int> { }
    [System.Serializable]
    public class BigIntUnityEvent : UnityEvent<BigInteger> { }
    #endregion

    private void Awake()
    {
        if (OnWorkersCountChanged == null)
            OnWorkersCountChanged = new IntUnityEvent();
        if (OnLvlValueChanged == null)
            OnLvlValueChanged = new IntUnityEvent();
        if (OnResourcePreTikChanged == null)
            OnResourcePreTikChanged = new BigIntUnityEvent();

    }
    public void Init(int index, CarriageStatus status, CarriageDatabase.CarriageData data, Train train, int lvl = 1)
    {
        this.index = index;
        this.Status = status;
        this.ItemType = data.type;
        this.maxWorkersValue = data.maxWorkersValue;
        this.defaultValue = data.defaultValue;
        this.addMultyplier = data.addMultyplier;
        this.BuyCost = data.cost;
        this.lvl = lvl;
        this.train = train;
        ui.Init(data.sprite, ItemType);

        OnStatusChanged += transform.parent.GetComponent<Train>().OnTrainPartChanged;

        GenerateCost();
        ChangeSpriteStatus(false);
    }

    void ChangeSpriteStatus(bool throwEvent)
    {
        ui.ChangeImageByStatus(Status);

        if (throwEvent)
            OnStatusChanged?.Invoke(Status);
    }

    private void GenerateCost()
    {
        BuyCost.items.ForEach(slot =>
        {
            slot.value = defaultValue * 100;
        });
    }

    public void OnTap()
    {
        if (Status == CarriageStatus.Active)
        {
            var slot = new Inventory.ItemSlot(ItemType, (BigInteger)((1 + addMultyplier * lvl) * defaultValue));
            train.RealizeCarriageTap(slot);
        }
    }

    public void Buy()
    {
        if (Status == CarriageStatus.Inactive)
        {
            Status = CarriageStatus.Active;
            ChangeSpriteStatus(true);
            Inventory.Instance.RemoveCost(BuyCost);
        }
    }

    public void UpLvl()
    {
        lvl++;
        OnLvlValueChanged?.Invoke(lvl);
        ThrowResourceEvent();
    }

    public void UpWorkers()
    {
        if (!Inventory.Instance.HasItem(ItemType.Human))
            return;

        workers++;
        Inventory.Instance.RemoveItem(ItemType.Human);

        OnWorkersCountChanged?.Invoke(workers);
        ThrowResourceEvent();
    }

    public void DownWorkers()
    {
        if (workers == 0)
            return;

        workers--;
        Inventory.Instance.AddItem(ItemType.Human);

        OnWorkersCountChanged?.Invoke(workers);
        ThrowResourceEvent();
    }

    public void ThrowResourceEvent()
    {
        OnResourcePreTikChanged?.Invoke(Realize.value);
    }
}
