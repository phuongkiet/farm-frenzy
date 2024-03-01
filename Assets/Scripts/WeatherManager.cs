using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WeatherStates
{
    Clear,
    Rainy
}

public class WeatherManager : TimeAgent
{
    [Range(0f, 1f)][SerializeField] float chanceToChangeWeather = 0.02f;

    public WeatherStates currentState = WeatherStates.Clear;
    public CropsManager cropsManager;
    public SceneManager sceneManager;

    [SerializeField] ParticleSystem rainSystem;

    private void Start()
    {
        Init();
        onTimeTick += RandomWeatherChangeCheck;
        UpdateWeather();
    }

    public void RandomWeatherChangeCheck(DayTimeController dayTimeController)
    {
        if(UnityEngine.Random.value < chanceToChangeWeather)
        {
            RandomWeatherChange();
        }
    }

    private void RandomWeatherChange()
    {
        WeatherStates newWeatherState = (WeatherStates)UnityEngine.Random.Range(0, Enum.GetNames(typeof(WeatherStates)).Length);
        ChangeWeather(newWeatherState);
    }

    private void ChangeWeather(WeatherStates newWeatherState)
    {
        if(SceneManager.GetActiveScene().name == "MainScene")
        {
            currentState = newWeatherState;
            Debug.Log(currentState);
            UpdateWeather();

            if (currentState == WeatherStates.Rainy)
            {
                WaterAllCrops();
            }
        }
        else
        {
            rainSystem.Stop();
        }
    }

    private void UpdateWeather()
    {
        switch (currentState)
        {
            case WeatherStates.Clear:
                rainSystem.gameObject.SetActive(false); 
                break;
            case WeatherStates.Rainy:
                rainSystem.gameObject.SetActive(true);
                break;
        }
    }

    private void WaterAllCrops()
    {
        if (cropsManager != null)
        {
            foreach (var cropTile in cropsManager.GetAllCropTiles())
            {
                cropsManager.Water(cropTile.position);
            }
        }
    }
}
