using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CarriageStatus{
    Inactive, Active
}

public interface ITrainPart{
    CarriageStatus GetStatus();
    void Init(int index, CarriageStatus status, CarriageDatabase.CarriageData data);
}

[RequireComponent(typeof(Image))]
public class Carriage : MonoBehaviour, ITrainPart, IPointerDownHandler 
{
    private Image image;
    private CarriageStatus status;
    [SerializeField] private int index;

    private CarriageType type;
    private int maxWorkersValue;
    public delegate void StatusChangeDelegate(CarriageStatus status);
    StatusChangeDelegate OnStatusChanged;

    public void Init(int index, CarriageStatus status, CarriageDatabase.CarriageData data){
        this.index = index;
        this.status = status;
        this.type = data.type;
        this.maxWorkersValue = data.maxWorkersValue;

        if(image == null)
            image = GetComponent<Image>();

        OnStatusChanged += transform.parent.GetComponent<Train>().OnTrainPartChanged;

        ChangeSpriteStatus(false);
    }

    void ChangeSpriteStatus(bool throwEvent){
        switch(status){
            case CarriageStatus.Inactive:
            image.color = new Color32(255, 255, 255, 75);
            break;
            case CarriageStatus.Active:
            image.color = new Color32(255, 255, 255, 255);
            break;
        }
        
        if(throwEvent)
            OnStatusChanged?.Invoke(status);
    }

    public  CarriageStatus GetStatus() => status;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(type);
        switch(status){
            case CarriageStatus.Inactive:
            status = CarriageStatus.Active;
            ChangeSpriteStatus(true);
            break;
        }
    }
}
