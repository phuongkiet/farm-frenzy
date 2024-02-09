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

    public override void Interact(Character character)
    {
        if(opened == false)
        {
            opened = true;
            chestClosed.SetActive(false);
            chestOpened.SetActive(true);
            AudioManager.instance.Play(openChestAudio);
        }
        else
        {
            opened = false;
            chestClosed.SetActive(true);
            chestOpened.SetActive(false);
            AudioManager.instance.Play(closeChestAudio);
        }
    }
}
