using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemDragAndDropController dragAndDropController;
    public DayTimeController dayTimeController;
    public DialogueSystem dialogueSystem;
    public PlaceableObjectReferenceManager placeableObjectReferenceManager;
    public ItemList itemDB;
    public ScreenTint screenTint;
    public CurrencyReferenceManger currencyReferenceManager;
    public FirebaseManager firebaseManager;
    public PlayerDataManager playerDataManager;

    private void Start()
    {
        if (inventoryContainer == null)
        {
            InitInventory();
        }
    }

    private void InitInventory()
    {
        inventoryContainer = (ItemContainer)ScriptableObject.CreateInstance(typeof(ItemContainer));
        inventoryContainer.InitInventory();
    }

    private void Awake()
    {
        Instance = this;
    }

    [Serializable]
    public class SaveLootItemData
    {
        public int itemId;
        public int count;

        public SaveLootItemData(int id, int c)
        {
            itemId = id;
            count = c;
        }
    }

    [Serializable]
    public class ToSave
    {
        public List<SaveLootItemData> saveLootItemData;
        public ToSave()
        {
            saveLootItemData = new List<SaveLootItemData>();
        }
    }
    public string Read()
    {
        ToSave toSave = new ToSave();
        for (int i = 0; i < inventoryContainer.slots.Count; i++)
        {
            if (inventoryContainer.slots[i].item == null)
            {
                toSave.saveLootItemData.Add(new SaveLootItemData(-1, 0));
            }
            else
            {
                toSave.saveLootItemData.Add(new SaveLootItemData(inventoryContainer.slots[i].item.id, inventoryContainer.slots[i].count));
            }
        }
        return JsonUtility.ToJson(toSave);
    }

    public void Load(string jsonString)
    {
        if (jsonString == "" || jsonString == "{}" || jsonString == null) { return; }
        if (inventoryContainer == null)
        {
            InitInventory();
        }
        ToSave toLoad = JsonUtility.FromJson<ToSave>(jsonString);
        for (int i = 0; i < toLoad.saveLootItemData.Count; i++)
        {
            if (toLoad.saveLootItemData[i].itemId == -1)
            {
                inventoryContainer.slots[i].Clear();
            }
            else
            {
                inventoryContainer.slots[i].item = GameManager.Instance.itemDB.items[toLoad.saveLootItemData[i].itemId];
                inventoryContainer.slots[i].count = toLoad.saveLootItemData[i].count;
            }
        }
    }
}
