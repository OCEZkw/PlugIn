using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeatherState
{
    Sunny,
    Cloudy,
    Rainy,
    Stormy
}

public class DynamicWeatherPlugin : MonoBehaviour
{
    [System.Serializable]
    public class WeatherSettings
    {
        public Color skyColor;
        public float fogDensity;
        public float rainIntensity;
        public float windIntensity;
        public AudioClip ambientSound;
    }

    public WeatherSettings sunnySettings;
    public WeatherSettings cloudySettings;
    public WeatherSettings rainySettings;
    public WeatherSettings stormySettings;

    public ParticleSystem rainParticleSystem;
    public Light directionalLight;
    public AudioSource ambientAudioSource;

    private WeatherState currentWeather = WeatherState.Sunny;
    private Coroutine transitionCoroutine;

    public float transitionDuration = 3f;

    void Start()
    {
        SetWeather(currentWeather);
    }

    public void SetWeather(WeatherState newWeather)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        transitionCoroutine = StartCoroutine(TransitionWeather(newWeather));
    }

    private IEnumerator TransitionWeather(WeatherState targetWeather)
    {
        WeatherSettings currentSettings = GetWeatherSettings(currentWeather);
        WeatherSettings targetSettings = GetWeatherSettings(targetWeather);

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;

            // Transition sky color
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(currentSettings.skyColor, targetSettings.skyColor, t));

            // Transition fog
            RenderSettings.fogDensity = Mathf.Lerp(currentSettings.fogDensity, targetSettings.fogDensity, t);

            // Transition rain
            var emission = rainParticleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(currentSettings.rainIntensity, targetSettings.rainIntensity, t);

            // Transition wind
            UpdateWindForce(Vector3.right * Mathf.Lerp(currentSettings.windIntensity, targetSettings.windIntensity, t));

            // Transition lighting
            directionalLight.intensity = Mathf.Lerp(1f, 0.5f, t); // Dim light for cloudy/rainy weather

            // Transition sound
            if (elapsedTime == 0f)
            {
                ambientAudioSource.clip = targetSettings.ambientSound;
                ambientAudioSource.Play();
            }
            ambientAudioSource.volume = t;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentWeather = targetWeather;
    }

    private void UpdateWindForce(Vector3 force)
    {
        var forceOverLifetime = rainParticleSystem.forceOverLifetime;
        forceOverLifetime.x = new ParticleSystem.MinMaxCurve(force.x);
        forceOverLifetime.y = new ParticleSystem.MinMaxCurve(force.y);
        forceOverLifetime.z = new ParticleSystem.MinMaxCurve(force.z);
    }

    private WeatherSettings GetWeatherSettings(WeatherState state)
    {
        switch (state)
        {
            case WeatherState.Sunny:
                return sunnySettings;
            case WeatherState.Cloudy:
                return cloudySettings;
            case WeatherState.Rainy:
                return rainySettings;
            case WeatherState.Stormy:
                return stormySettings;
            default:
                return sunnySettings;
        }
    }

    // Method to be called by other scripts to get current weather state
    public WeatherState GetCurrentWeather()
    {
        return currentWeather;
    }
}