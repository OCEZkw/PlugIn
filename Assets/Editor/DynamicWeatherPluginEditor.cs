using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicWeatherPlugin))]
public class DynamicWeatherPluginEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DynamicWeatherPlugin weatherPlugin = (DynamicWeatherPlugin)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Weather Control", EditorStyles.boldLabel);

        if (GUILayout.Button("Set Sunny"))
        {
            weatherPlugin.SetWeather(WeatherState.Sunny);
        }

        if (GUILayout.Button("Set Cloudy"))
        {
            weatherPlugin.SetWeather(WeatherState.Cloudy);
        }

        if (GUILayout.Button("Set Rainy"))
        {
            weatherPlugin.SetWeather(WeatherState.Rainy);
        }

        if (GUILayout.Button("Set Stormy"))
        {
            weatherPlugin.SetWeather(WeatherState.Stormy);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Current Weather", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(weatherPlugin.GetCurrentWeather().ToString());
    }
}