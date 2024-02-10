using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceableObjectsManager : MonoBehaviour
{
    [SerializeField] PlaceableObjectsContainer container;
    [SerializeField] List<Tilemap> targetTilemaps;

    private void Start()
    {
        GameManager.Instance.GetComponent<PlaceableObjectReferenceManager>().placeableObjectsManager = this;
    }

    public void Place(Item item, Vector3Int positionGrid)
    {
        GameObject go = Instantiate(item.itemPrefabs);
        foreach(Tilemap tile in targetTilemaps)
        {
            Vector3 position = tile.CellToWorld(positionGrid) + tile.cellSize/2;
            go.transform.position = position;
            container.placeableObjects.Add(new PlaceableObject(item, go.transform, positionGrid));
        }
    }
}
 