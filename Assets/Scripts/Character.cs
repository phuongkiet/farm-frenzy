using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public int maxVal;
    public int curVal;
    public Stat(int maxVal, int curVal)
    {
        this.maxVal = maxVal;
        this.curVal = curVal;
    }

    public void Subtract(int amount)
    {
        curVal -= amount;
    }

    public void Add(int amount)
    {
        curVal += amount;
        if(curVal > maxVal)
        {
            curVal = maxVal;
        }
    }

    public void SetToMax()
    {
        curVal = maxVal;
    }
}

public class Character : MonoBehaviour
{
    public Stat stamina;
    public bool isExhausted;
    [SerializeField] Status status;
    DisableControls disableControls;

    private void Awake()
    {
        disableControls = GetComponent<DisableControls>();
    }

    private void Start()
    {
        UpdateStamina();
    }

    private void UpdateStamina()
    {
        status.Set(stamina.curVal, stamina.maxVal);
    }

    public void GetTired(int amount)
    {
        stamina.Subtract(amount);
        if(stamina.curVal < 0)
        {
            isExhausted = true;
        }
        UpdateStamina();
    }

    public void Rest(int amount)
    {
        stamina.Add(amount);
        UpdateStamina();
    }

    public void FullRest()
    {
        stamina.SetToMax();
        UpdateStamina();
    }
}
