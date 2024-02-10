using UnityEngine.Tilemaps;
using UnityEngine;

public class TileMapReadController : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    public CropsManager cropsManager;
    public PlaceableObjectReferenceManager placeablesManager;

    public Vector3Int GetGridPosition(Vector2 position, bool mousePosition)
    {
        if (tilemap == null)
        {
            tilemap = GameObject.Find("DirtTilemap").GetComponent<Tilemap>();
        }

        if (tilemap == null) { return Vector3Int.zero; }

        Vector3 worldPosition;
        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        Vector3Int gridPositions = tilemap.WorldToCell(worldPosition);

        return gridPositions;
    }

    public TileBase GetTileBase(Vector3Int gridPosition)
    {
        if (tilemap == null)
        {
            tilemap = GameObject.Find("DirtTilemap").GetComponent<Tilemap>();
        }

        if (tilemap == null) { return null; }

        TileBase tile = tilemap.GetTile(gridPosition);

        return tile;
    }
}
