using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolCharacterController : MonoBehaviour
{
    CharacterController2D characterController2D;
    Character character;
    Rigidbody2D rigidbody;
    ToolBarController toolBarController;
    Animator animator;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController controller; 
    [SerializeField] float maxDistance = 1.5f;
    [SerializeField] ToolAction onTilePickUp;
    [SerializeField] IconHighlight iconHighlight;

    Vector3Int selectedTilePosition;
    bool selectable;

    private void Awake()
    {
        character = GetComponent<Character>();
        characterController2D = GetComponent<CharacterController2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        toolBarController = GetComponent<ToolBarController>();
        animator = GetComponent<Animator>();
    }
    private void EnergyCost(int energyCost)
    {
        character.GetTired(energyCost);
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
        if (Input.GetMouseButtonDown(1))
        {
            UseToolWater();
        }
    }

    private void SelectTile()
    {
        selectedTilePosition = controller.GetGridPosition(Input.mousePosition, true);
    }

    private void Marker()
    {
        markerManager.markedCellPosition = new List<Vector3Int> { selectedTilePosition };
        iconHighlight.cellPosition = selectedTilePosition;
    }

    void CanSelectCheck()
    {
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
        iconHighlight.CanSelect = selectable;
    }

    private bool UseToolWorld()
    {
        Vector2 position = rigidbody.position + characterController2D.lastMotionVector * offsetDistance;

        Item item = toolBarController.GetItem;
        if(item == null)
        {
            return false;
        }
        if(item.onAction == null)
        {
            return false;
        }

        EnergyCost(item.onAction.energyCost);

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
            if (item == null)
            {
                PickUpTile();
                return;
            }
            if (item.onTileMapAction == null) { return; }
            EnergyCost(item.onAction.energyCost);
            animator.SetTrigger("plow act");
            bool complete = item.onTileMapAction.OnApplyToTileMap(selectedTilePosition, controller, item);
            if (complete == true)
            {
                if (item.onItemUsed != null)
                {
                    item.onItemUsed.OnItemUsed(item, GameManager.Instance.inventoryContainer);
                }
            }
        }
    }

    private void UseToolWater()
    {
        if (selectable == true)
        {
            Item item = toolBarController.GetItem;
            if (item == null)
            { 
                return;
            }
            if (item.onTileMapAction == null) { return; }
            EnergyCost(item.onAction.energyCost);
            animator.SetTrigger("water act");
            bool complete = item.onTileMapAction.OnApplyToTileMap(selectedTilePosition, controller, item);
            if (complete == true)
            {
                if (item.onItemUsed != null)
                {
                    item.onItemUsed.OnItemUsed(item, GameManager.Instance.inventoryContainer);
                }
            }
        }
    }

    private void PickUpTile()
    {
        if (onTilePickUp == null) { return; }
        onTilePickUp.OnApplyToTileMap(selectedTilePosition, controller, null);
    }
}
