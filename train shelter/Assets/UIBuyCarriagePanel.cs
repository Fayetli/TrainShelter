using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBuyCarriagePanel : MonoBehaviour
{
    [SerializeField] private GameObject container;
    public GameObject Container { get => container; }
    [SerializeField] private Button button;
    private Carriage carriage;

    public void Init(Carriage carriage)
    {
        this.carriage = carriage;

        var componentPrefab = Resources.Load<GameObject>("Component");
        var cost = carriage.BuyCost;
        cost.items.ForEach(slot =>
        {
            var component = Instantiate(componentPrefab, container.transform as RectTransform).GetComponent<UIComponent>();
            component.Init(slot.value, GameManager.Instance.ItemDatabase.GetSprite(slot.type));
        });

        CheckButtonInteractable();

        Inventory.Instance.OnInventoryChanged.AddListener(CheckButtonInteractable);
    }

    public void BuyClick()
    {
        carriage.Buy();
        Destroy(gameObject);
    }
    public void CheckButtonInteractable() => button.interactable = Inventory.Instance.HasCost(carriage.BuyCost);

}
