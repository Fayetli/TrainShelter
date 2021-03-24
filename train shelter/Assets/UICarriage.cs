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
    public void ChangeLvl(int value) => lvlTmp.text = "Lv. " + value;
    public void ChangeResourcePerTik(BigInteger value) => resourcePerTikTmp.text = value + "/s";
    public void ChangeWorkersAmount(int amount) => workersTmp.text = amount.ToString();

    private void Awake() {
        carriage = GetComponent<Carriage>();
    }

    public void Init(Sprite sprite, ItemType type){
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
}
