using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Numerics;

public enum CarriageStatus
{
    Inactive, Active
}

public interface ITrainPart
{
    public CarriageStatus Status { get; }
    void Init(int index, CarriageStatus status, CarriageDatabase.CarriageData data, Train train);
    (ItemType type, BigInteger value) Realize { get; }
}

[RequireComponent(typeof(Image))]
public class Carriage : MonoBehaviour, ITrainPart
{
    #region Init
    private Image image;
    [SerializeField] private int index;
    public ItemType ItemType { get; private set; }
    public CarriageStatus Status { get; private set; }
    public Inventory.Cost BuyCost { get; private set; }
    private int maxWorkersValue;
    private int defaultValue;
    [SerializeField] private GameObject carriageHUD;
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
    public ItemSlotUnityEvent OnCarriageTap;

    [System.Serializable]
    public class IntUnityEvent : UnityEvent<int> { }
    [System.Serializable]
    public class BigIntUnityEvent : UnityEvent<BigInteger> { }
    [System.Serializable]
    public class ItemSlotUnityEvent : UnityEvent<Inventory.ItemSlot> { }
    #endregion

    private void Awake()
    {
        if (OnWorkersCountChanged == null)
            OnWorkersCountChanged = new IntUnityEvent();
        if (OnLvlValueChanged == null)
            OnLvlValueChanged = new IntUnityEvent();
        if (OnResourcePreTikChanged == null)
            OnResourcePreTikChanged = new BigIntUnityEvent();
        if (OnCarriageTap == null)
            OnCarriageTap = new ItemSlotUnityEvent();

    }
    public void Init(int index, CarriageStatus status, CarriageDatabase.CarriageData data, Train train)
    {
        this.index = index;
        this.Status = status;
        this.ItemType = data.type;
        this.maxWorkersValue = data.maxWorkersValue;
        this.defaultValue = data.defaultValue;
        this.addMultyplier = data.addMultyplier;
        this.BuyCost = data.cost;
        this.lvl = 1;

        if (image == null)
            image = GetComponent<Image>();

        OnStatusChanged += transform.parent.GetComponent<Train>().OnTrainPartChanged;
        OnCarriageTap.AddListener(train.RealizeCarriageTap);

        ChangeSpriteStatus(false);

    }

    void ChangeSpriteStatus(bool throwEvent)
    {
        switch (Status)
        {
            case CarriageStatus.Inactive:
                image.color = new Color32(255, 255, 255, 75);
                SpawnBuyPanel();
                break;
            case CarriageStatus.Active:
                image.color = new Color32(255, 255, 255, 255);
                carriageHUD.SetActive(true);
                break;
        }

        if (throwEvent)
            OnStatusChanged?.Invoke(Status);
    }

    private void SpawnBuyPanel()
    {
        var buyPanel = Resources.Load<GameObject>("BuyCarriagePanel");
        Instantiate(buyPanel, transform as RectTransform);
        var buyyer = buyPanel.GetComponent<UIBuyCarriagePanel>();
        buyyer.Init(this);
    }

    public void OnTap()
    {
        switch (Status)
        {
            case CarriageStatus.Inactive:
                Status = CarriageStatus.Active;
                ChangeSpriteStatus(true);
                break;
            case CarriageStatus.Active:
                OnCarriageTap?.Invoke(new Inventory.ItemSlot(ItemType, (BigInteger)((1 + addMultyplier * lvl) * defaultValue)));
                break;
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
