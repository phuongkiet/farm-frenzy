using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : MonoBehaviour
{
    public ItemContainer inventory;
    public List<InventoryButton> buttons;
    private void Start()
    {
        if (inventory == null)
        {
            InitInventory();
        }
        Init();
        
    }

    public void Init()
    {
        SetIndex();
        Show();
    }
    private void InitInventory()
    {
        inventory = (ItemContainer)ScriptableObject.CreateInstance(typeof(ItemContainer));
        inventory.InitInventory();
    }

    private void OnEnable()
    {
        /*Clear();*/
        Show();
    }

    private void LateUpdate()
    {
        if (inventory == null) { return; }
        if (inventory.isDirty)
        {
            Show();
            inventory.isDirty = false;
        }
    }

    private void SetIndex()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetIndex(i);
        }
    }

    public virtual void Show()
    {
        if (inventory == null) { return; }
        for (int i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
        {
            if (inventory.slots[i].item == null)
            {
                buttons[i].Clean();
            }
            else
            {
                buttons[i].Set(inventory.slots[i]);
            }
        }
    }
    /*public void Clear()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].Clean();
        }
    }*/

    public virtual void OnClick(int id)
    {

    }
}
