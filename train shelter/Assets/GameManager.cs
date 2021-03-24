using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance { get; private set; }
    [SerializeField] private ItemDatabase itemDatabase;
    public ItemDatabase ItemDatabase { get => itemDatabase; }
    [SerializeField] private CarriageDatabase carriageDatabase;
    public CarriageDatabase CarriageDatabase { get => carriageDatabase; }

    private Train train;


    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);


        train = FindObjectOfType<Train>();
    }

    private void Start()
    {
        Inventory.Instance.AddItem(ItemType.Human, 10);
    }


}
