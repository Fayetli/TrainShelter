using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using TMPro;

public class UICarriageInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject upgradeCostContainer;
    [SerializeField] private Carriage carriage;
    [SerializeField] private TextMeshProUGUI resPerUnit;
    private List<UIComponent> components;
    private Camera camera;

    private void Awake()
    {
        components = new List<UIComponent>();
        camera = Camera.main;
    }

    private void OnEnable()
    {
        ShowUpgradeComponent();
        UpdateResourcePerUnit(carriage.ResourcePerUnit);
        Inventory.Instance.OnInventoryChanged.AddListener(ShowUpgradeComponent);
        carriage.OnResourcePreTikChanged.AddListener(UpdateResourcePerUnit);
    }

    public void ShowUpgradeComponent()
    {
        ClearComponentsList();

        var componentPrefab = Resources.Load<GameObject>("Component");
        var cost = carriage.UpgradeCost;

        cost.items.ForEach(slot =>
        {
            var component = Instantiate(componentPrefab, upgradeCostContainer.transform as RectTransform).GetComponent<UIComponent>();
            component.Init(slot.value, GameManager.Instance.ItemDatabase.GetSprite(slot.type));
            components.Add(component);
        });
    }


    private void OnDisable()
    {
        ClearComponentsList();
        Inventory.Instance.OnInventoryChanged.RemoveListener(ShowUpgradeComponent);
        carriage.OnResourcePreTikChanged.RemoveListener(UpdateResourcePerUnit);
    }

    private void ClearComponentsList()
    {
        components.ForEach(c => Destroy(c.gameObject));
        components.Clear();
    }
    
    private void Update() {
        if(!(transform as RectTransform).IsVisibleFrom(camera))
            gameObject.SetActive(false);
    }
    //Bone
    private void UpdateResourcePerUnit(BigInteger value) => resPerUnit.text = carriage.ResourcePerUnit.ParseToString();

}
