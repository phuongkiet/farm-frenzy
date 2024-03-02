using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableControls : MonoBehaviour
{
    CharacterController2D characterController;
    ToolCharacterController toolCharacterController;
    ToolBarController toolBarController;
    ItemContainerInteractController itemContainerInteractController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        toolCharacterController = GetComponent<ToolCharacterController>();
        toolBarController = GetComponent<ToolBarController>();
        itemContainerInteractController = GetComponent<ItemContainerInteractController>();
    }

    public void DisableControl()
    {
        characterController.enabled = false;
        toolCharacterController.enabled = false;
        toolBarController.enabled = false;
        itemContainerInteractController.enabled = false;
    }

    public void EnableControl()
    {
        characterController.enabled = true;
        toolCharacterController.enabled = true;
        toolBarController.enabled = true;
        itemContainerInteractController.enabled = true;
        toolBarController.enabled = true;
    }
}
