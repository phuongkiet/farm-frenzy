using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TilemapCropsManager : TimeAgent
{
    [SerializeField] TileBase plowed;
    [SerializeField] TileBase seeded;
    [SerializeField] TileBase watered;
    Tilemap targetTilemap;
    [SerializeField] CropsContainer container;
    [SerializeField] GameObject cropSpritePrefab;

    private void Start()
    {
        GameManager.Instance.GetComponent<CropsManager>().cropsManager = this;
        targetTilemap = GetComponent<Tilemap>();
        onTimeTick += Tick;
        Init();
        VisualizeMap();
    }

    private void VisualizeMap()
    {
       for(int i = 0; i < container.crops.Count; i++)
        {
            VisualizeTile(container.crops[i]);
        }
    }

    private void OnDestroy()
    {
        for(int i = 0; i < container.crops.Count; i++)
        {
            container.crops[i].spriteRenderer = null;
        }
    }

    public void Tick()
    {
        if (targetTilemap == null) { return; }

        foreach (CropTile cropTile in container.crops)
        {
            if (cropTile.crop == null) { continue; }

            cropTile.damage += 0.02f;

            if (cropTile.damage > 1f)
            {
                cropTile.Harvested();
                targetTilemap.SetTile(cropTile.position, plowed);
                continue;
            }

            if (!cropTile.isWatered)
            {
                Debug.Log("The crop at position " + cropTile.position + " is not watered!");
                continue;
            }

            if (cropTile.Complete)
            {
                Debug.Log("Im done growing");
                continue;
            }

            cropTile.growTimer += 1;

            if (cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
            {
                cropTile.spriteRenderer.gameObject.SetActive(true);
                cropTile.spriteRenderer.sprite = cropTile.crop.sprites[cropTile.growStage];

                cropTile.growStage += 1;
            }
        }
    }


    public bool Check(Vector3Int position)
    {
        return container.Get(position) != null;
    }

    public void Plow(Vector3Int position)
    {
        if(Check(position) == true) { return; }
        CreatePlowedTile(position);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        CropTile tile = container.Get(position);    
        if(tile == null) { return; }
        targetTilemap.SetTile(position, seeded);
        tile.crop = toSeed;
    }

    public void VisualizeTile(CropTile cropTile)
    {
        targetTilemap.SetTile(cropTile.position, cropTile.crop != null ? seeded : plowed);  
        
        if(cropTile.spriteRenderer == null)
        {
            GameObject go = Instantiate(cropSpritePrefab, transform);
            go.transform.position = targetTilemap.CellToWorld(cropTile.position);
            cropTile.spriteRenderer = go.GetComponent<SpriteRenderer>();
        }
        bool growing = cropTile.crop != null && cropTile.growTimer >= cropTile.crop.growthStageTime[0];
        cropTile.spriteRenderer.gameObject.SetActive(growing);
        if(growing == true)
        {
            cropTile.spriteRenderer.sprite = cropTile.crop.sprites[cropTile.growStage-1];
        }
    }
    private void CreatePlowedTile(Vector3Int position)
    {

        CropTile crop = new CropTile();
        container.Add(crop);

        crop.position = position;

        VisualizeTile(crop);

        targetTilemap.SetTile(position, plowed);
    }

    internal void PickUp(Vector3Int gridPosition)
    {
        Vector2Int position = (Vector2Int)gridPosition;
        CropTile tile = container.Get(gridPosition);
        if(tile == null) { return; }
        {
            
        }

        if (tile.Complete)
        {
            ItemSpawnManager.instance.SpawnItem(targetTilemap.CellToWorld(gridPosition), tile.crop.yeild, tile.crop.count);
            tile.Harvested();
            VisualizeTile(tile);
        }
    }

    public void Water(Vector3Int position)
    {
        CropTile cropTile = container.Get(position);
        if (cropTile != null)
        {
            cropTile.isWatered = true;

            // Check if the tile has been seeded
            if (cropTile.crop != null)
            {
                // If seeded, visualize both the crop tile and the watered tile
                targetTilemap.SetTile(position, watered); // Visualize watered tile
                VisualizeTile(cropTile); // Re-visualize crop tile
            }
            else
            {
                // If not seeded, just visualize the watered tile
                targetTilemap.SetTile(position, watered);
            }

            Debug.Log("Crop at position " + position + " watered!");
        }
    }
}
