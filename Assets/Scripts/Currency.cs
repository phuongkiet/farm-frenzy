using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    [SerializeField] int amount;
    [SerializeField] Text text;

    private void Start()
    {
        amount = 500;
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = amount.ToString();
    }
}
