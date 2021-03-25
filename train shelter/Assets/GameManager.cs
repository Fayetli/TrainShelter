using UnityEngine;
using System.Numerics;

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
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);


        train = FindObjectOfType<Train>();
    }

    private void Start()
    {
        Inventory.Instance.AddItem(ItemType.Human, 10);

    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.C))
        {
            Inventory.Instance.AddItem(ItemType.Human, 1000);
            Inventory.Instance.AddItem(ItemType.Electric, 1000);
            Inventory.Instance.AddItem(ItemType.Food, 1000);
            Inventory.Instance.AddItem(ItemType.Stones, 1000);
            Inventory.Instance.AddItem(ItemType.Woods, 1000);
        }
#endif
    }

}
