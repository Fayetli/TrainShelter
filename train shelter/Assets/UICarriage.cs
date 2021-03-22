using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class UICarriage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lvlTmp;
    [SerializeField] private TextMeshProUGUI resourcePerTikTmp;
    [SerializeField] private TextMeshProUGUI workersTmp;

    public void ChangeLvl(int value) => lvlTmp.text = "Lv. " + value;
    public void ChangeResourcePerTik(BigInteger value) => resourcePerTikTmp.text = value + "/s";
    public void ChangeWorkersAmount(int amount) => workersTmp.text = amount.ToString();


}
