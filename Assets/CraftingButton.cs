using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image icon;
    [SerializeField] Text text;
    [SerializeField] Image highlight;

    [SerializeField] Text ingridientText;

    public ItemSlot item;
    public CraftingRecipe currentRecipe;

    int myIndex;

    public void SetIndex(int index)
    {
        myIndex = index;
    }

    public void SetRecipe(CraftingRecipe recipe)
    {
        currentRecipe = recipe;
    }

    public void Set(ItemSlot slot)
    {
        item = slot;
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

    public void ShowIngredient(string ingredientInfo)
    {
        if (!string.IsNullOrEmpty(ingredientInfo))
        {
            ingridientText.text = ingredientInfo;
        }
        else
        {
            ingridientText.text = "N/A";
        }
    }

    public void UnShowIngridient()
    {
        ingridientText.text = null;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnShowIngridient();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentRecipe != null && myIndex >= 0)
        {
            Debug.Log("currentRecipe.elements.Count: " + currentRecipe.elements.Count);

            // Initialize an empty string to store ingredient information
            string ingredientsInfo = "";

            // Loop through each element in the recipe
            foreach (var element in currentRecipe.elements)
            {
                if (element.item != null)
                {
                    // Concatenate ingredient information
                    ingredientsInfo += "x " + element.count + " " + element.item.Name + "\n";
                }
                else
                {
                    ingredientsInfo += "N/A\n";
                }
            }

            // Show the concatenated ingredient information
            ShowIngredient(ingredientsInfo);
        }
        else
        {
            Debug.LogError("Invalid index or recipe. Recipe: " + (currentRecipe != null ? "not null" : "null") + ", myIndex: " + myIndex + ", elements count: " + (currentRecipe != null ? currentRecipe.elements.Count.ToString() : "N/A"));
        }
    }
}
