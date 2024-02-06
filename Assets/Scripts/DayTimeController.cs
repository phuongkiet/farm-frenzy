using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayTimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;
    const float phaseLength = 900f; // 15 minute chunk of time 

    [SerializeField] Color nightLightColor;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] Color dayLightColor = Color.white;

    float time;
    [SerializeField] float timeScale = 60f;
    [SerializeField] float startAtTime = 28800f; //in seconds
    [SerializeField] Text text;
    [SerializeField] Light2D globalLight;
    private int days;

    List<TimeAgent> timeAgent;

    private void Awake()
    {
        timeAgent = new List<TimeAgent>();
    }

    private void Start()
    {
        time = startAtTime;
    }

    public void Subscribe(TimeAgent agent)
    {
        timeAgent.Add(agent);
    }

    public void UnSubscribe(TimeAgent agent)
    {
        timeAgent.Remove(agent);
    }

    float Hours
    {
        get { return time / 3600f; }
    }

    float Minutes
    {
        get { return Mathf.Floor(time % 3600f / 300f) * 5; } // 300 seconds = 5 minutes
    }

    private void Update()
    {
        time += Time.deltaTime * timeScale;

        TimeValueCalculation();
        DayLight();

        if (time > secondsInDay)
        {
            NextDay();
        }

        TimeAgents();
    }

    int oldPhase = 0;
    private void TimeAgents()
    {
        int currentPhase = (int)(time / phaseLength);

        if (oldPhase != currentPhase)
        {
            oldPhase = currentPhase;
            for (int i = 0; i < timeAgent.Count; i++)
            {
                timeAgent[i].Invoke();
            }
        }
    }

    private void DayLight()
    {
        float v = nightTimeCurve.Evaluate(Hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        globalLight.color = c;
    }

    private void TimeValueCalculation()
    {
        int hh = (int)Mathf.Floor(Hours);
        int mm = (int)Minutes;
        text.text = hh.ToString("00") + ":" + mm.ToString("00");
    }

    private void NextDay()
    {
        time = 0;
        days += 1;
    }
}
