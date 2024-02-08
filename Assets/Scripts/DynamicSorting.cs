using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    public const string LAYER_NAME = "Foreground";
    public int sortingOrder = 0;
    private SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (sprite)
        {
            sprite.sortingOrder = sortingOrder;
            sprite.sortingLayerName = LAYER_NAME;

        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Tree"))
        {
            sprite.sortingOrder = 6;
        }

        else if (col.CompareTag("Hill"))
        {
            sprite.sortingOrder = 3;
        }
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Tree"))
        {
            sprite.sortingOrder = 6;
        }
        else if (col.CompareTag("Hill"))
        {
            sprite.sortingOrder = 3;
        }

    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Tree"))
        {
            sprite.sortingOrder = 8;
        }
        else if (col.CompareTag("Hill"))
        {
            sprite.sortingOrder = 6;
        }
    }
}
