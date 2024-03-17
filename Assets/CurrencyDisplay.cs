using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : MonoBehaviour
{
    public static CurrencyDisplay Instance;
    public Text text;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Start()
    {
        UpdateText();
    }
    public void UpdateText()
    {
        text.text = CurrencyManager.Instance.currency.amount.ToString();
    }
}


