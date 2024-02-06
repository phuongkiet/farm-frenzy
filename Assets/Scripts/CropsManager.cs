using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class CropTile
{
    public int growTimer;
    public int growStage;
    public Crop crop;
    public SpriteRenderer spriteRenderer;
    public float damage;
    public Vector3Int position;

    public bool Complete
    {
        get
        {
            if(crop == null) { return false; }
            return growTimer >= crop.timeToGrow;
        }
    }

    internal void Harvested()
    {
        growTimer = 0;
        growStage = 0;
        crop = null;
        damage = 0;
        spriteRenderer.gameObject.SetActive(false);
    }
}

public class CropsManager : TimeAgent
{
    [SerializeField] TileBase plowed;
    [SerializeField] TileBase seeded;
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] GameObject cropSpritePrefab;

    Dictionary<Vector2Int, CropTile> crops;

    private void Start()
    {
        crops = new Dictionary<Vector2Int, CropTile>();
        onTimeTick += Tick;
        Init();
    }

    public void Tick()
    {
        foreach(CropTile cropTile in crops.Values)
        {
            if(cropTile.crop == null) { continue; }

            cropTile.damage += 0.02f;

            if(cropTile.damage > 1f)
            {
                cropTile.Harvested();
                targetTilemap.SetTile(cropTile.position, plowed);
                continue;
            }

            if (cropTile.Complete)
            {
                Debug.Log("Im done growing");
                continue;
            }

            cropTile.growTimer += 1;

            if(cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
            {
                cropTile.spriteRenderer.gameObject.SetActive(true);
                cropTile.spriteRenderer.sprite = cropTile.crop.sprites[cropTile.growStage];

                cropTile.growStage += 1;
            }
        }
    }

    public bool Check(Vector3Int position)
    {
        return crops.ContainsKey((Vector2Int)position);
    }

    public void Plow(Vector3Int position)
    {
        if (crops.ContainsKey((Vector2Int)position))
        {
            return;
        }
        CreatePlowedTile(position);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        targetTilemap.SetTile(position, seeded);
        crops[(Vector2Int)position].crop = toSeed;
    }


    private void CreatePlowedTile(Vector3Int position)
    {
        CropTile crop = new CropTile();
        crops.Add((Vector2Int)position, crop);

        GameObject go = Instantiate(cropSpritePrefab);
        go.transform.position = targetTilemap.CellToWorld(position);
        go.SetActive(false);
        crop.spriteRenderer = go.GetComponent<SpriteRenderer>();

        crop.position = position;

        targetTilemap.SetTile(position, plowed);
    }

    internal void PickUp(Vector3Int gridPosition)
    {
        Vector2Int position = (Vector2Int)gridPosition;
        if(crops.ContainsKey(position) == false) { return; }

        CropTile cropTile = crops[position];
        if (cropTile.Complete)
        {
            ItemSpawnManager.instance.SpawnItem(targetTilemap.CellToWorld(gridPosition), cropTile.crop.yeild, cropTile.crop.count);
            targetTilemap.SetTile(gridPosition, plowed);
            cropTile.Harvested();
        }
    }
}
