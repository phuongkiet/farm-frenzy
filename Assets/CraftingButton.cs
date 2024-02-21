using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image icon;
    [SerializeField] Text text;
    [SerializeField] Image highlight;

    [SerializeField] Text ingridientText;

    public ItemSlot item;

    int myIndex;

    public void SetIndex(int index)
    {
        myIndex = index;
    }

    public void Set(ItemSlot slot)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;

        if (slot.item.stackable == true)
        {
            text.gameObject.SetActive(true);
            text.text = slot.count.ToString();
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

    public void Clean()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CraftingItemPanel itemPanel = transform.parent.GetComponent<CraftingItemPanel>();
        itemPanel.OnClick(myIndex);

    }

    public void Highlight(bool b)
    {
        highlight.gameObject.SetActive(b);
    }

    public void ShowIngridient()
    {
        Show(item);
    }

    public void Show(ItemSlot slot)
    {
        ingridientText.text = slot.item.Name;
    }
}
