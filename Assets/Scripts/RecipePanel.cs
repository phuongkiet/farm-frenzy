using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipePanel : CraftingItemPanel
{
    [SerializeField] RecipeList list;
    [SerializeField] Crafting crafting;

    public override void Show()
    {
        for(int i = 0; i < buttons.Count && i < list.recipe.Count; i++)
        {
            buttons[i].Set(list.recipe[i].output);
        }
    }

    public override void OnClick(int id)
    {
        if(id >= list.recipe.Count) { return; }

        crafting.Craft(list.recipe[id]);    
    }
}
