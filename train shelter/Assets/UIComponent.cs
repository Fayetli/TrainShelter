using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
public class UIComponent : MonoBehaviour
{
    [SerializeField] private Image image;
    public Image Image { get => image; }
    [SerializeField] private TextMeshProUGUI count;
    public TextMeshProUGUI Count { get => count; }

    public void Init(BigInteger amount, Sprite sprite){
        ChangeSprite(sprite);
        ChangeAmount(amount);
    }
    public void ChangeSprite(Sprite sprite) => image.sprite = sprite;
    public void ChangeAmount(BigInteger amount) => count.text = amount.ParseToString();
    
}
