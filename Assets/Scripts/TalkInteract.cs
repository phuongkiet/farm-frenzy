using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkInteract : Interactable
{
    [SerializeField] DialogueContainer _dialogueContainer;
    public override void Interact(Character character)
    {
        GameManager.Instance.dialogueSystem.Initialize(_dialogueContainer);
    }
}
