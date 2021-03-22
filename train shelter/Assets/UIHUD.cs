using System.Numerics;
using UnityEngine;
using TMPro;

public class UIHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI humanAmount;
    [SerializeField] private TextMeshProUGUI foodAmount;
    [SerializeField] private TextMeshProUGUI electricAmount;
    [SerializeField] private TextMeshProUGUI woodAmount;
    [SerializeField] private TextMeshProUGUI stoneAmount;

    private void Start() {
        Inventory.Instance.OnItemAmountChanged += ChangeText;
    }
    public void ChangeText(ItemType item, BigInteger value)
    {
        switch (item)
        {
            case ItemType.Human:
                humanAmount.text = value.ToString();
                break;
            case ItemType.Food:
                foodAmount.text = value.ToString();
                break;
            case ItemType.Electric:
                electricAmount.text = value.ToString();
                break;
            case ItemType.Woods:
                woodAmount.text = value.ToString();
                break;
            case ItemType.Stones:
                stoneAmount.text = value.ToString();
                break;
            default:
                Debug.LogError("Error item type");
                break;
        }
    }

    private void OnDisable() {
        Inventory.Instance.OnItemAmountChanged -= ChangeText;
    }
}
