using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootContainerInteract : Interactable
{
    [SerializeField] GameObject chestClosed;
    [SerializeField] GameObject chestOpened;
    [SerializeField] bool opened;
    public override void Interact(Character character)
    {
        if(opened == false)
        {
            opened = true;
            chestClosed.SetActive(false);
            chestOpened.SetActive(true);
        }
    }
}
