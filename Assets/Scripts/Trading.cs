using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trading : MonoBehaviour
{
    [SerializeField] GameObject storePanel;
    TalkInteract talkInteract;

    Currency currency;

    ItemStorePanel itemStorePanel;
    [SerializeField] ItemContainer playerItemContainer;
    [SerializeField] ItemPanel itemPanel;
    [SerializeField] GameObject toolBarPanel;   

    private void Awake()
    {
        currency = GetComponent<Currency>();
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
            currency.Add(moneyGain);
            itemToSell.Clear();
            GameManager.Instance.dragAndDropController.UpdateIcon();
        }
    }

    internal void BuyItem(int id)
    {
        Item itemToBuy = talkInteract.storeContent.slots[id].item;
        int totalPrice = itemToBuy.price;
        if(currency.Check(totalPrice) == true)
        {
            currency.Decrease(totalPrice);
            playerItemContainer.Add(itemToBuy);
            itemPanel.Show();
        }
    }
}
