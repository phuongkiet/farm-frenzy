using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorePanel : StorePanel
{
    [SerializeField] Trading trading;
    public override void OnClick(int id)
    {
        if(GameManager.Instance.dragAndDropController.itemSlot.item == null)
        {
            BuyItem(id);
        }
        else
        {
            SellItem();
        }
        Show();
    }

    private void BuyItem(int id)
    {
        trading.BuyItem(id);
    }

    private void SellItem()
    {
        trading.SellItem();
    }
}
