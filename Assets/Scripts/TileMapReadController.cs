using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapReadController : MonoBehaviour
{
    [SerializeField] List<Tilemap> tilemaps; // Change this to a list of Tilemaps
    public CropsManager cropsManager;

    public Vector3Int GetGridPosition(Vector2 position, bool mousePosition)
    {
        Vector3 worldPosition;
        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        Vector3Int gridPositions = tilemaps[0].WorldToCell(worldPosition); 

        return gridPositions;
    }

    public TileBase GetTileBase(Vector3Int gridPosition)
    {
        foreach (Tilemap tilemap in tilemaps) 
        {
            TileBase tile = tilemap.GetTile(gridPosition);
            if (tile != null)
            {
                return tile;
            }
        }

        return null;
    }

}
