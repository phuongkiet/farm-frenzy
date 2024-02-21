using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(menuName = "Data/ToolAction/Watering")]
public class WateringTile : ToolAction
{
    [SerializeField] List<TileBase> canWater;
    [SerializeField] AudioClip plowAudio;
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        TileBase tileWater = tileMapReadController.GetTileBase(gridPosition);
        if (canWater.Contains(tileWater) == false)
        {
            return false;
        }

        tileMapReadController.cropsManager.Water(gridPosition);

        return true;
    }
}
