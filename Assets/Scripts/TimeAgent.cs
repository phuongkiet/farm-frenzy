using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    public Action<DayTimeController> onTimeTick;
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        GameManager.Instance.dayTimeController.Subscribe(this);
    }

    public void Invoke(DayTimeController dayTimeController)
    {
        onTimeTick?.Invoke(dayTimeController);
    }

    private void OnDestroy()
    {
        GameManager.Instance.dayTimeController.UnSubscribe(this);
    }
}
