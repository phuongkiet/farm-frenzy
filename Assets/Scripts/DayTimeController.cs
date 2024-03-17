using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public enum DayOfWeek
{
    Monday = 0,
    Tuesday = 1,
    Wednesday = 2,
    Thursday = 3,
    Friday = 4,
    Saturday = 5,
    Sunday = 6,
}

public enum Season
{
    Spring = 0,
    Summer = 1,
    Fall = 2,
    Winter = 3
}

public class DayTimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;
    const float phaseLength = 900f; // 15 minute chunk of time 
    const float phaseInDay = 96f;

    [SerializeField] Color nightLightColor;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] Color dayLightColor = Color.white;

    float time;
    [SerializeField] float timeScale = 60f;
    [SerializeField] float startAtTime = 28800f; //in seconds
    [SerializeField] Text text;
    [SerializeField] Text dayOfWeekText;
    [SerializeField] Text season;
    [SerializeField] Text date;
    [SerializeField] Light2D globalLight;
    private int days = 1;
    bool nightMusicStarted = false;
    bool dayMusicStarted = false;

    DayOfWeek dayOfWeek;
    Season currentSeason;
    const int seasonLength = 20;

    List<TimeAgent> timeAgent;

    // Reference to the MusicManager
    [SerializeField] MusicManager musicManager;

    private void Awake()
    {
        timeAgent = new List<TimeAgent>();
    }

    private void Start()
    {
        time = startAtTime;
        UpdateDateText();
        UpdateSeason();
        UpdateDate();
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

        if (IsNightTime() && !nightMusicStarted)
        {
            musicManager.SwitchToNightMusic();
            nightMusicStarted = true;
        }
        else if (!IsNightTime())
        {
            nightMusicStarted = false;
        }

        if(IsDayTime() && !dayMusicStarted)
        {
            musicManager.SwitchToDayMusic();
            dayMusicStarted = true;
        }
        else if (!IsDayTime())
        {
            dayMusicStarted = false;
        }
    }

    private bool IsNightTime()
    {
        return Hours >= 20f;
    }

    private bool IsDayTime()
    {
        return Hours >= 5f;
    }

    int oldPhase = -1;
    private void TimeAgents()
    {
        if(oldPhase == -1)
        {
            oldPhase = CalculatePhase();
        }
        int currentPhase = CalculatePhase();
        while(oldPhase < currentPhase)
        {
            oldPhase += 1;
            for (int i = 0; i < timeAgent.Count; i++)
            {
                timeAgent[i].Invoke(this);
            }
        }
    }

    private int CalculatePhase()
    {
        return (int)(time / phaseLength) + (int)(days * phaseInDay);
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
        time -= secondsInDay;
        days += 1;
        UpdateDate();
        int dayNum = (int)dayOfWeek;
        dayNum += 1;
        if (dayNum >= 7)
        {
            dayNum = 0;
        }
        dayOfWeek = (DayOfWeek)dayNum;
        UpdateDateText();

        ResetCropWateredStatus();

        if (days >= seasonLength)
        {
            NextSeason();
        }
    }

    private void ResetCropWateredStatus()
    {
        TilemapCropsManager[] cropManagers = FindObjectsOfType<TilemapCropsManager>();
        foreach (TilemapCropsManager manager in cropManagers)
        {
            manager.ResetWateredStatus();
        }
    }

    private void UpdateDate()
    {
        date.text = days.ToString();
    }

    private void NextSeason()
    {
        days = 1;
        int seasonNum = (int)currentSeason;
        seasonNum += 1;
        if(seasonNum >= 4)
        {
            seasonNum = 0;
        }

        currentSeason = (Season)seasonNum;
        UpdateSeason();
    }

    private void UpdateSeason()
    {
        season.text = currentSeason.ToString();
    }

    private void UpdateDateText()
    {
        dayOfWeekText.text = dayOfWeek.ToString();
    }

    public void SkipTime(float second = 0, float minute = 0, float hour = 0) 
    {
        float timeToSkip = second;
        timeToSkip += minute * 60f;
        timeToSkip += hour * 3600f;

        time += timeToSkip;
    }

    public void SkipToMorning()
    {
        float secondToSkip = 0f;

        if(time > startAtTime)
        {
            secondToSkip += secondsInDay - time + startAtTime;
        }
        else
        {
            secondToSkip += startAtTime - time;
        }

        SkipTime(secondToSkip);
    }
}
