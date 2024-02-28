using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObjectReferenceManager : MonoBehaviour
{
    public PlaceableObjectsManager placeableObjectsManager;

    public void Place(Item item, Vector3Int pos)
    {
        if(placeableObjectsManager == null)
        {
            Debug.Log("No placeableObjectsManager reference detected");
            return;
        }

        placeableObjectsManager.Place(item, pos);
    }

    public bool Check(Vector3Int pos)
    {
        if(placeableObjectsManager == null)
        {
            Debug.Log("No placeableObjectManager reference detected");
            return false;
        }

        return placeableObjectsManager.Check(pos);
        
    }

    internal void PickUp(Vector3Int gridPosition)
    {
        if (placeableObjectsManager == null)
        {
            Debug.Log("No placeableObjectManager reference detected");
            return;
        }

        placeableObjectsManager.PickUp(gridPosition);
    }
}
