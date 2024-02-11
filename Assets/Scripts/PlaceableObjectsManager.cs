using System;
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
        VisualizeMap();
    }

    private void OnDestroy()
    {
        for(int i = 0;  i < container.placeableObjects.Count; i++)
        {
            if (container.placeableObjects[i].targetObject == null) { continue; }

            IPersistant persistant = container.placeableObjects[i].targetObject.GetComponent<IPersistant>();
            if (persistant != null)
            {
                string jsonString = persistant.Read();
                container.placeableObjects[i].objectState = jsonString;
            }
            container.placeableObjects[i].targetObject = null;
        }
    }

    private void VisualizeMap()
    {
        for(int i = 0; i < container.placeableObjects.Count; i++)
        {
            VisualizeItem(container.placeableObjects[i]);
        }
    }

    private void VisualizeItem(PlaceableObject placeableObject)
    {
        GameObject go = Instantiate(placeableObject.item.itemPrefabs);
        go.transform.parent = transform; 

        foreach(Tilemap tile in targetTilemaps)
        {
            Vector3 position = tile.CellToWorld(placeableObject.positionOnGrid) + tile.cellSize / 2;
            go.transform.position = position; 
            
            IPersistant persistant = go.GetComponent<IPersistant>();
            if (persistant != null)
            {
                persistant.Load(placeableObject.objectState);
            }

            placeableObject.targetObject = go.transform;
        }

        
    }

    public bool Check(Vector3Int position)
    {
        return container.Get(position) != null;
    }

    public void Place(Item item, Vector3Int positionGrid)
    {
        if(Check(positionGrid) == true) { return; }
        PlaceableObject placeableObject = new PlaceableObject(item, positionGrid);
        VisualizeItem(placeableObject);
        container.placeableObjects.Add(placeableObject);
    }

    internal void PickUp(Vector3Int gridPosition)
    {
        PlaceableObject placeableObject = container.Get(gridPosition);
        if (placeableObject == null)
        {
            return;
        }
        foreach (Tilemap tile in targetTilemaps)
        {
            ItemSpawnManager.instance.SpawnItem(tile.CellToWorld(gridPosition), placeableObject.item, 1);
            Destroy(placeableObject.targetObject.gameObject);
            container.Remove(placeableObject);
        }
    }
}
