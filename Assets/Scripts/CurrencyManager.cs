using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public Currency currency;
    public bool isUserDataLoaded = false; 

    private void Start()
    {
        GameManager.Instance.GetComponent<CurrencyReferenceManger>().CurrencyManager = this;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdateCurrencyAmount(int newAmount)
    {
        if (currency.amount != newAmount)
        {
            currency.amount = newAmount;
            Debug.Log($"Money: {newAmount}");
            GameManager.Instance.GetComponent<CurrencyReferenceManger>().CurrencyManager.UpdateCurrencyAmount(newAmount);
            Debug.Log(GetCurrencyMoney());
            CurrencyDisplay.Instance.UpdateText();
        }
    }

    public int GetCurrencyMoney()
    {
        return currency.amount;
    }
}

