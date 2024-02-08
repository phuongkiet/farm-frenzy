using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] List<Tilemap> targetTilemap;
    [SerializeField] List<TileBase> tile;
    public List<Vector3Int> markedCellPosition;
    List<Vector3Int> oldCellPosition;
    bool show;

    private void Update()
    {
        if (show == false) { return; }
        foreach (Tilemap tilemap in targetTilemap)
        {
            if (oldCellPosition != null)
            {
                foreach (Vector3Int position in oldCellPosition)
                {
                    tilemap.SetTile(position, null);
                }
            }

            if (markedCellPosition != null)
            {
                foreach (Vector3Int position in markedCellPosition)
                {
                    tilemap.SetTile(position, tile[0]);
                }
            }
        }
        oldCellPosition = markedCellPosition;
    }

    internal void Show(bool selectable)
    {
        show = selectable;

        foreach (Tilemap tilemap in targetTilemap)
        {
            tilemap.gameObject.SetActive(show);
        }
    }
}
