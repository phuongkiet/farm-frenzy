using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolCharacterController : MonoBehaviour
{
    CharacterController2D character;
    Rigidbody2D rigidbody;
    ToolBarController toolBarController;
    Animator animator;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] List<TileMapReadController> controllers; // Change this to a list of TileMapReadControllers
    [SerializeField] float maxDistance = 1.5f;
    [SerializeField] ToolAction onTilePickUp;

    Vector3Int selectedTilePosition;
    bool selectable;

    private void Awake()
    {
        character = GetComponent<CharacterController2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        toolBarController = GetComponent<ToolBarController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SelectTile();
        CanSelectCheck();
        Marker();
        if (Input.GetMouseButtonDown(0))
        {
            if (UseToolWorld() == true)
            {
                return;
            }
            UseToolGrid();
        }
    }

    private void SelectTile()
    {
        foreach (TileMapReadController controller in controllers) // Iterate over each TileMapReadController in the list
        {
            selectedTilePosition = controller.GetGridPosition(Input.mousePosition, true);
            if (selectedTilePosition != null)
            {
                break;
            }
        }
    }

    private void Marker()
    {
        markerManager.markedCellPostion = new List<Vector3Int> { selectedTilePosition };
    }

    void CanSelectCheck()
    {
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
    }

    private bool UseToolWorld()
    {
        Vector2 position = rigidbody.position + character.lastMotionVector * offsetDistance;

        Item item = toolBarController.GetItem;
        if(item == null)
        {
            return false;
        }
        if(item.onAction == null)
        {
            return false;
        }
        animator.SetTrigger("act");
        bool complete = item.onAction.OnApply(position);

        if (complete == true)
        {
            if (item.onItemUsed != null)
            {
                item.onItemUsed.OnItemUsed(item, GameManager.Instance.inventoryContainer);
            }
        }

        return complete;
    }

    private void UseToolGrid()
    {
        if (selectable == true)
        {
            Item item = toolBarController.GetItem;
            if(item == null) 
            {
                PickUpTile();
                return; 
            }
            if(item.onTileMapAction == null) { return; }

            animator.SetTrigger("act");
            foreach(TileMapReadController controller in controllers)
            {
                bool complete = item.onTileMapAction.OnApplyToTileMap(selectedTilePosition, controller, item);
                if(complete == true)
                {
                    if(item.onItemUsed != null)
                    {
                        item.onItemUsed.OnItemUsed(item, GameManager.Instance.inventoryContainer);
                    }
                }
            }
        }
    }

    private void PickUpTile()
    {
        if(onTilePickUp == null) { return; }
        foreach (TileMapReadController controller in controllers)
        {
            onTilePickUp.OnApplyToTileMap(selectedTilePosition, controller, null);       
        }
    }
}
