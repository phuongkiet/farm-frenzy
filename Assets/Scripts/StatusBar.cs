using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Slider slideBar;

    public void Set(int curr, int max)
    {
        slideBar.maxValue = max;
        slideBar.value = curr;

        text.text = curr.ToString();
    }
}
