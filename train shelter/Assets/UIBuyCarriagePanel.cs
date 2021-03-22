using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuyCarriagePanel : MonoBehaviour
{
    [SerializeField] private GameObject container;

    private Carriage carriage;

    //instantiate bug here
    public void Init(Carriage carriage)
    {
        this.carriage = carriage;

        var componentPrefab = Resources.Load<GameObject>("BuyComponent");
        var cost = carriage.BuyCost;
        cost.items.ForEach(slot =>
        {
            var go = Instantiate(componentPrefab) as GameObject;
            go.transform.SetParent(container.transform as RectTransform);
            var component = go.GetComponent<UIComponent>();
            component.ChangeAmount(slot.value);
        });
    }
}
