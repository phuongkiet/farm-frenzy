using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlaceableObject
{
    public Item item;
    public Transform targetObject;
    public Vector3Int positionOnGrid;

    //<summary>
    // serialized JSON string which contains the state of the object
    //</summary>
    public string objectState;

    public PlaceableObject(Item item, Vector3Int positionOnGrid)
    {
        this.item = item;
        this.positionOnGrid = positionOnGrid;
    }
}

[CreateAssetMenu(menuName ="Data/Placeable Objects Container")]
public class PlaceableObjectsContainer : ScriptableObject
{
    public List<PlaceableObject> placeableObjects;
    internal PlaceableObject Get(Vector3Int position)
    {
        return placeableObjects.Find(x => x.positionOnGrid == position);
    }

    internal void Remove(PlaceableObject placeableObject)
    {
        placeableObjects.Remove(placeableObject);
    }
}
