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

    public PlaceableObject(Item item, Transform targetObject, Vector3Int positionOnGrid)
    {
        this.item = item;
        this.targetObject = targetObject;
        this.positionOnGrid = positionOnGrid;
    }
}

[CreateAssetMenu(menuName ="Data/Placeable Objects Container")]
public class PlaceableObjectsContainer : ScriptableObject
{
    public List<PlaceableObject> placeableObjects;
}
