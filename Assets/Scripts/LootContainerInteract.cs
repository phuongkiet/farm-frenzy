using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootContainerInteract : Interactable, IPersistant
{
    [SerializeField] GameObject chestClosed;
    [SerializeField] GameObject chestOpened;
    [SerializeField] bool opened;
    [SerializeField] AudioClip openChestAudio;
    [SerializeField] AudioClip closeChestAudio;
    [SerializeField] ItemContainer itemContainer;

    private void Start()
    {
        if (itemContainer == null)
        {
            Init();
        }
    }

    private void Init()
    {
        itemContainer = (ItemContainer)ScriptableObject.CreateInstance(typeof(ItemContainer));
        itemContainer.Init();
    }

    public override void Interact(Character character)
    {
        if (opened == false)
        {
            Open(character);
        }
        else
        {
            Close(character);
        }
    }

    public void Open(Character character)
    {
        opened = true;
        chestClosed.SetActive(false);
        chestOpened.SetActive(true);
        AudioManager.instance.Play(openChestAudio);

        character.GetComponent<ItemContainerInteractController>().Open(itemContainer, transform);
    }

    public void Close(Character character)
    {
        opened = false;
        chestClosed.SetActive(true);
        chestOpened.SetActive(false);
        AudioManager.instance.Play(closeChestAudio);

        character.GetComponent<ItemContainerInteractController>().Close();
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
        for (int i = 0; i < itemContainer.slots.Count; i++)
        {
            if (itemContainer.slots[i].item == null)
            {
                toSave.saveLootItemData.Add(new SaveLootItemData(-1, 0));
            }
            else
            {
                toSave.saveLootItemData.Add(new SaveLootItemData(itemContainer.slots[i].item.id, itemContainer.slots[i].count));
            }
        }
        return JsonUtility.ToJson(toSave);
    }

    public void Load(string jsonString)
    {
        if (jsonString == "" || jsonString == "{}" || jsonString == null) { return; }
        if (itemContainer == null)
        {
            Init();
        }
        ToSave toLoad = JsonUtility.FromJson<ToSave>(jsonString);
        for (int i = 0; i < toLoad.saveLootItemData.Count; i++)
        {
            if (toLoad.saveLootItemData[i].itemId == -1)
            {
                itemContainer.slots[i].Clear();
            }
            else
            {
                itemContainer.slots[i].item = GameManager.Instance.itemDB.items[toLoad.saveLootItemData[i].itemId];
                itemContainer.slots[i].count = toLoad.saveLootItemData[i].count;
            }
        }
    }
}
