using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarController : MonoBehaviour
{
    [SerializeField] int toolBarSize = 10;
    int selectedTool;

    public Action<int> onChange;

    public Item GetItem
    {
        get {
            return GameManager.Instance.inventoryContainer.slots[selectedTool].item;
        }
    }
    internal void Set(int id)
    {
        selectedTool = id;
    }

    private void Update()
    {
        float delta = Input.mouseScrollDelta.y;
        if (delta != 0)
        {
            if(delta > 0)
            {
                selectedTool += 1;
                selectedTool = (selectedTool >= toolBarSize ? 0 : selectedTool);
            }
            else
            {
                selectedTool -= 1;
                selectedTool = (selectedTool <= 0 ? toolBarSize - 1 : selectedTool);
            }
            onChange?.Invoke(selectedTool);
        }
    }
}
