using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyReferenceManger : MonoBehaviour
{
    public CurrencyManager CurrencyManager;

    public void UpdateCurrencyAmount(int amount)
    {
        if(CurrencyManager == null)
        {
            Debug.Log("");
        }
        CurrencyManager.UpdateCurrencyAmount(amount);
    }
}
