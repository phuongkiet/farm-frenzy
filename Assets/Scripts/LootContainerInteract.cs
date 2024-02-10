using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootContainerInteract : Interactable
{
    [SerializeField] GameObject chestClosed;
    [SerializeField] GameObject chestOpened;
    [SerializeField] bool opened;
    [SerializeField] AudioClip openChestAudio;
    [SerializeField] AudioClip closeChestAudio;
    [SerializeField] ItemContainer itemContainer;

    public override void Interact(Character character)
    {
        if(opened == false)
        {
            Open(character);
        }
        else
        {
            Close(character);
        }
    }

    public void Open(Character character)
    {
        opened = true;
        chestClosed.SetActive(false);
        chestOpened.SetActive(true);
        AudioManager.instance.Play(openChestAudio);

        character.GetComponent<ItemContainerInteractController>().Open(itemContainer, transform);
    }

    public void Close(Character character)
    {
        opened = false;
        chestClosed.SetActive(true);
        chestOpened.SetActive(false);
        AudioManager.instance.Play(closeChestAudio);

        character.GetComponent<ItemContainerInteractController>().Close();
    }
}
