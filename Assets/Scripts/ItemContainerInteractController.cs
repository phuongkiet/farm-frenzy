using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerInteractController : MonoBehaviour
{
    ItemContainer targetItemContainer;
    [SerializeField] GameObject chestPanel;
    Transform openedChest;
    [SerializeField] float maxDistance = 2.5f;

    private void Update()
    {
        if(openedChest != null)
        {
            float distance = Vector2.Distance(openedChest.position, transform.position);
            if(distance < maxDistance)
            {
                openedChest.GetComponent<LootContainerInteract>().Close(GetComponent<Character>());
            }
        }
    }

    public void Open(ItemContainer itemContainer, Transform _openedChest)
    {
        targetItemContainer = itemContainer;
        chestPanel.SetActive(true);
        openedChest = _openedChest;
    }

    public void Close()
    {
        chestPanel.SetActive(false);
        openedChest = null;
    }
}
