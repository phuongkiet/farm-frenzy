using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Crop")]
public class Crop : ScriptableObject
{
    public enum CropType
    {
        Wheat,
        Tomato
    }

    public CropType cropType;
    public int timeToGrow = 10;
    public Item yeild;
    public int count = 1;

    public List<Sprite> sprites;
    public List<int> growthStageTime;
    public TileBase cropTileBase; 
}
