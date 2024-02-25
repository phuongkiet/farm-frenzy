using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Stat
{
    public int maxVal;
    public int currVal;

    public Stat(int curr, int max)
    {
        maxVal = max;
        currVal = curr;
    }

    internal void Add(int amount)
    {
        currVal += amount;
    }

    internal void Subtract(int amount)
    {
        currVal -= amount;
    }

    internal void Heal(int amount)
    {
        currVal += amount;
        if (currVal > maxVal)
        {
            currVal = maxVal;
        }
    }

    internal void SetToMax()
    {
        currVal = maxVal;
    }
}
public class Character : MonoBehaviour
{
    public Stat hp;
    [SerializeField]  StatusBar hpBar; 
    public Stat stamina;
    [SerializeField]  StatusBar staminaBar;

    public bool isDead;
    public bool isExhausted;

    private void Start()
    {
        UpdateHPBar();
        UpdateStaminaBar();
    }
    public void TakeDamage(int amount)
    {
        hp.Subtract(amount);
        if (hp.currVal < 0)
        {
            isDead = true;
        }

        UpdateHPBar();

    }

    private void UpdateHPBar()
    {
        hpBar.Set(hp.currVal, hp.maxVal);

    }

    private void UpdateStaminaBar()
    {
        staminaBar.Set(stamina.currVal, stamina.maxVal);

    }

    public void Heal(int amout)
    {
        hp.Add(amout);
        UpdateHPBar();

    }

    public void FullHeal()
    {
        hp.SetToMax();
        UpdateHPBar();

    }

    public void GetTired(int amount)
    {
        stamina.Subtract(amount);
        if (stamina.currVal < 0)
        {
            isExhausted = true;
        }
        UpdateStaminaBar();

    }

    public void Reset(int amout)
    {
        stamina.Add(amout);
        UpdateStaminaBar();

    }

    public void FullReset()
    {
        stamina.SetToMax();
        UpdateStaminaBar();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Heal(10);
        } 
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GetTired(10);
        } 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Reset(10);
        }
    }
}
