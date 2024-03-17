using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trading : MonoBehaviour
{
    [SerializeField] GameObject storePanel;
    TalkInteract talkInteract;


    ItemStorePanel itemStorePanel;
    [SerializeField] ItemContainer playerItemContainer;
    [SerializeField] ItemPanel itemPanel;
    [SerializeField] GameObject toolBarPanel;

    private void Start()
    {
        if (playerItemContainer == null)
        {
            InitInventory();
        }

    }

    private void InitInventory()
    {
        playerItemContainer = (ItemContainer)ScriptableObject.CreateInstance(typeof(ItemContainer));
        playerItemContainer.InitInventory();
    }

    private void Awake()
    {
        if (storePanel != null)
        {
            itemStorePanel = storePanel.GetComponent<ItemStorePanel>();
        }
        else
        {
            Debug.LogError("Store panel is not assigned to Trading script.");
        }
    }


    private void Update()
    {
        StopTrading();
    }

    public void BeginTrading(TalkInteract talkInteract)
    {
        this.talkInteract = talkInteract;
        Debug.Log("Begin Trading");
        /*itemStorePanel.SetInventory(talkInteract.storeContent);*/
        storePanel.SetActive(true);   
        toolBarPanel.gameObject.SetActive(false);
    }

    public void StopTrading()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            storePanel.SetActive(false);
            toolBarPanel.gameObject.SetActive(true);
        }
        
    }

    public void SellItem()
    {
        if(GameManager.Instance.dragAndDropController.CheckForSale() == true)
        {
            ItemSlot itemToSell = GameManager.Instance.dragAndDropController.itemSlot;
            int moneyGain = itemToSell.item.stackable == true ? itemToSell.item.price * itemToSell.count : itemToSell.item.price;
            GameManager.Instance.currencyReferenceManager.CurrencyManager.currency.Add(moneyGain);
            GameManager.Instance.playerDataManager.PlayerData.money.Add(moneyGain);
            itemToSell.Clear();
            GameManager.Instance.dragAndDropController.UpdateIcon();
            CurrencyDisplay.Instance.UpdateText();
        }
    }

    internal void BuyItem(int id)
    {
        Item itemToBuy = talkInteract.storeContent.slots[id].item;
        int totalPrice = itemToBuy.price;
        if(GameManager.Instance.currencyReferenceManager.CurrencyManager.currency.Check(totalPrice) == true)
        {
            GameManager.Instance.currencyReferenceManager.CurrencyManager.currency.Decrease(totalPrice);
            GameManager.Instance.playerDataManager.PlayerData.money.Decrease(totalPrice);
            playerItemContainer.Add(itemToBuy);
            itemPanel.Show();
            CurrencyDisplay.Instance.UpdateText();
        }
    }
}
