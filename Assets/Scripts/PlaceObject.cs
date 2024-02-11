using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/ToolAction/Place Object")]
public class PlaceObject : ToolAction  
{
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        if (tileMapReadController.placeablesManager.Check(gridPosition) == true)
        {
            return false;
        }
        tileMapReadController.placeablesManager.Place(item, gridPosition);
        return true;
    }

    public override void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        inventory.Remove(usedItem);
    }
}
