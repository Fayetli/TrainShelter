using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

[RequireComponent(typeof(Carriage))]
public class UICarriage : MonoBehaviour
{
    private Carriage carriage;
    [SerializeField] private TextMeshProUGUI lvlTmp;
    [SerializeField] private TextMeshProUGUI resourcePerTikTmp;
    [SerializeField] private TextMeshProUGUI workersTmp;
    [SerializeField] private Image image;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject hud;
    [SerializeField] private UICarriageInfoPanel infoPanel;
    public void ChangeLvl(int value) => lvlTmp.text = "Lv. " + value;
    public void ChangeResourcePerTik(BigInteger value) => resourcePerTikTmp.text = value.ParseToString() + "/s";
    public void ChangeWorkersAmount(int amount) => workersTmp.text = amount.ToString();
    [SerializeField] private Button upgradeButton;

    private void Awake()
    {
        carriage = GetComponent<Carriage>();
    }

    private void Start() {
        CheckButtonInteractable();
        Inventory.Instance.OnInventoryChanged.AddListener(CheckButtonInteractable);
    }

    public void Init(Sprite sprite, ItemType type)
    {
        image.sprite = sprite;
        itemImage.sprite = GameManager.Instance.ItemDatabase.GetSprite(type);
    }
    public void InstantiateBuyPanel()
    {
        var buyPanel = Resources.Load<GameObject>("BuyCarriagePanel");
        var buyyer = Instantiate(buyPanel, transform as RectTransform).GetComponent<UIBuyCarriagePanel>();
        buyyer.Init(carriage);
    }

    public void ChangeImageByStatus(CarriageStatus status)
    {
        switch (status)
        {
            case CarriageStatus.Inactive:
                image.color = new Color32(255, 255, 255, 75);
                InstantiateBuyPanel();
                break;
            case CarriageStatus.Active:
                image.color = new Color32(255, 255, 255, 255);
                HUDSetActive(true);
                break;
        }
    }

    public void HUDSetActive(bool active) => hud.SetActive(active);
    public void CheckButtonInteractable() => upgradeButton.interactable = Inventory.Instance.HasCost(carriage.UpgradeCost);
    public void UpgradeCarriage()
    {
        if (!Inventory.Instance.HasCost(carriage.UpgradeCost))
            Debug.LogError("Hasn`t upgrade cost");
        carriage.UpLvl();

        if(infoPanel.gameObject.activeInHierarchy)
            infoPanel.ShowUpgradeComponent();
    }
    public void ActiveInfoPanel() => infoPanel.gameObject.SetActive(!infoPanel.gameObject.activeInHierarchy);

}
