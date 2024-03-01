using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerInteractController : MonoBehaviour
{
    ItemContainer targetItemContainer;
    [SerializeField] ItemContainerPanel chestPanel;
    [SerializeField] GameObject toolBarPanel;
    Transform openedChest;
    [SerializeField] float maxDistance = 2.5f;

    private void Update()
    {
        if(openedChest != null)
        {
            float distance = Vector2.Distance(openedChest.position, transform.position);
            if(distance > maxDistance)
            {
                openedChest.GetComponent<LootContainerInteract>().Close(GetComponent<Character>());
            }
        }
    }

    public void Open(ItemContainer itemContainer, Transform _openedChest)
    {
        targetItemContainer = itemContainer;
        chestPanel.inventory = targetItemContainer;
        chestPanel.transform.parent.gameObject.SetActive(true);
        chestPanel.gameObject.SetActive(true);
        toolBarPanel.SetActive(false);
        openedChest = _openedChest;
    }

    public void Close()
    {
        chestPanel.transform.parent.gameObject.SetActive(false);
        chestPanel.gameObject.SetActive(false);
        toolBarPanel.SetActive(true);
        openedChest = null;
    }
}
