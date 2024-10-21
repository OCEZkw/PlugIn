using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherControlUI : MonoBehaviour
{
    public DynamicWeatherPlugin weatherPlugin;
    public Button sunnyButton;
    public Button cloudyButton;
    public Button rainyButton;
    public Button stormyButton;

    void Start()
    {
        sunnyButton.onClick.AddListener(() => SetWeather(WeatherState.Sunny));
        cloudyButton.onClick.AddListener(() => SetWeather(WeatherState.Cloudy));
        rainyButton.onClick.AddListener(() => SetWeather(WeatherState.Rainy));
        stormyButton.onClick.AddListener(() => SetWeather(WeatherState.Stormy));
    }

    void SetWeather(WeatherState state)
    {
        weatherPlugin.SetWeather(state);
    }
}