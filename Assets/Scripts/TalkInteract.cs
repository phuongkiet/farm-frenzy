using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkInteract : Interactable
{
    [SerializeField] DialogueContainer _dialogueContainer;
    public bool isDoneTalk = false;
    private Character currentCharacter;
    public ItemContainer storeContent;

    public override void Interact(Character character)
    {
        currentCharacter = character; 
        GameManager.Instance.dialogueSystem.Initialize(_dialogueContainer);
        GameManager.Instance.dialogueSystem.OnDialogueConclude += StartTrading;
    }

    private void StartTrading()
    {
        isDoneTalk = true;

        if (isDoneTalk == true)
        {
            Trading trading = currentCharacter.GetComponent<Trading>(); 

            if (trading != null)
            {
                trading.BeginTrading(this);
            }
            else
            {
                Debug.LogError("Trading component is not attached or not initialized");
            }
        }

        GameManager.Instance.dialogueSystem.OnDialogueConclude -= StartTrading;
    }
}

