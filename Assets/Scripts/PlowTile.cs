using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Data/ToolAction/Plow")]
public class PlowAction : ToolAction
{
    [SerializeField] List<TileBase> canPlow;
    [SerializeField] AudioClip plowAudio;
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        TileBase tilePlow = tileMapReadController.GetTileBase(gridPosition);
        if (canPlow.Contains(tilePlow) == false)
        {
            return false;
        }

        tileMapReadController.cropsManager.Plow(gridPosition);
        AudioManager.instance.Play(plowAudio);

        return true;
    }
}
