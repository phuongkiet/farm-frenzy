using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    [SerializeField] ItemContainer inventory;

    private void Start()
    {
        if (inventory == null)
        {
            InitInventory();
        }
        
    }

    private void InitInventory()
    {
        inventory = (ItemContainer)ScriptableObject.CreateInstance(typeof(ItemContainer));
        inventory.InitInventory();
    }

    public void Craft(CraftingRecipe recipe)
    {
        
        if (inventory.CheckFreeSpace() == false)
        {
            Debug.Log("Not enough space to fit the item after crafting");
            return;
        }
        for(int i = 0; i < recipe.elements.Count; i++)
        {
            if (inventory.CheckItem(recipe.elements[i]) == false)
            {
                Debug.Log("Crafting recipe elements are not present in the inventory");
                return;
            }
        }

        for(int i = 0; i < recipe.elements.Count; i++)
        {
            inventory.Remove(recipe.elements[i].item, recipe.elements[i].count);
        }
        inventory.Add(recipe.output.item, recipe.output.count);
    }
}
