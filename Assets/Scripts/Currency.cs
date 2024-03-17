using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Currency
{
    public int amount;

    public Currency(int startingAmount = 0)
    {
        amount = startingAmount;
    }

    public void Add(int moneyGain)
    {
        amount += moneyGain;
    }

    public bool Check(int totalPrice)
    {
        return amount >= totalPrice;
    }

    public void Decrease(int totalPrice)
    {
        amount -= totalPrice;
        if (amount < 0) { amount = 0; }
    }
}
