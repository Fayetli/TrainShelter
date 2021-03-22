using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Train : MonoBehaviour
{
    List<ITrainPart> cars = new List<ITrainPart>();
    [SerializeField] private GameObject carriagePrefab;
    [SerializeField] private CarriageDatabase database;
    private void Awake()
    {
        cars = GetComponentsInChildren<ITrainPart>().ToList();

        for(int i = 0; i < 3; i++)
            AddNewCarriage(CarriageStatus.Active);
        AddNewCarriage(CarriageStatus.Inactive);
    }
    private int counter = 0;
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S))
            AddNewCarriage(CarriageStatus.Inactive);
#endif
    }

    private void FixedUpdate() {
        counter++;
        if(counter == 50){
            counter = 0;
            RealizeCarriages();
        }
    }

    private void RealizeCarriages(){
        cars.ForEach(c => 
        {
            var turple = c.Realize;
            if(turple.value != 0)
                Inventory.Instance.AddItem(turple.type, turple.value);
        });
    }

    public void OnTrainPartChanged(CarriageStatus status)
    {
        if(status == CarriageStatus.Active)
            if(database.carriages.Count > cars.Count)
                AddNewCarriage(CarriageStatus.Inactive);
    }

    private void AddNewCarriage(CarriageStatus status)
    {
        var carriage = Instantiate(carriagePrefab, transform as RectTransform).GetComponent<ITrainPart>();
        carriage.Init(cars.Count, status, database.carriages[cars.Count], this);
        cars.Add(carriage);
    }

    public void RealizeCarriageTap(Inventory.ItemSlot slot){
        if(slot.value == 0)
            return;
        Inventory.Instance.AddItemSlot(slot);
    }

}
