using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolBarPanel : ItemPanel
{
    [SerializeField] ToolBarController controller;
    [SerializeField] Text toolName;

    private void Start()
    {
        Init();
        controller.onChange += Highlight;
        Highlight(0);
    }
    public override void OnClick(int id)
    {
        controller.Set(id);
        Highlight(id);
    }

    int currentSelectedTool;

    public void Highlight(int id)
    {
        buttons[currentSelectedTool].Highlight(false);
        currentSelectedTool = id;
        buttons[currentSelectedTool].Highlight(true);
        Item selectedItem = controller.GetItem;

        if (selectedItem != null)
        {
            toolName.text = selectedItem.Name; 
        }
        else
        {
            toolName.text = "";
        }
    }
}
