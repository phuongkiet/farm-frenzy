using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    [SerializeField] Text amount;
    [SerializeField] Slider statusBar;

    public void Set(int curVal, int maxVal)
    {
        statusBar.maxValue = maxVal;
        statusBar.value = curVal;

        amount.text = curVal.ToString();
    }
}
