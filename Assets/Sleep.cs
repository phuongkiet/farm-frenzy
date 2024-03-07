using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour
{
    DisableControls disableControls;
    Character character;
    DayTimeController dayTimeController;
    private void Awake()
    {
        disableControls = GetComponent<DisableControls>();
        character = GetComponent<Character>();
        dayTimeController = GameManager.Instance.dayTimeController;
    }
    internal void DoSleep()
    {
        StartCoroutine(SleepRoutine());
    }

    IEnumerator SleepRoutine()
    {
        ScreenTint screenTint = GameManager.Instance.screenTint;
        disableControls.DisableControl();
        screenTint.Tint();
        yield return new WaitForSeconds(2f);
        character.FullRest();
        dayTimeController.SkipToMorning();
        screenTint.UnTint();
        yield return new WaitForSeconds(2f);
        disableControls.EnableControl();
        yield return null;
    }
}
