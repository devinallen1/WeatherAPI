using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DigitalRuby.RainMaker;

public class UpdateWeatherConditions : MonoBehaviour
{
    //public bool needWeather;
    public bool newSearch;
    public bool hasWeather;
    public TextMeshProUGUI City;
    public TextMeshProUGUI Temperature;
    public TextMeshProUGUI MinMaxTemperature;
    public TextMeshProUGUI WindSpeed;
    public GameObject rainParticle;
    public GameObject clouds;
    public GameObject rainClouds;
    public GameObject snowClouds;
    public GameObject snow;
    public GameObject sun;

    private WeatherAPI_Script weather;
    private BaseRainScript rainScript;
    // Start is called before the first frame update
    void Start()
    {
        //needWeather = false;
        newSearch = false;
        hasWeather = false;
        weather = WeatherAPI_Script.FindObjectOfType<WeatherAPI_Script>();
        rainScript = BaseRainScript.FindObjectOfType<BaseRainScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (newSearch && hasWeather)
        {
            City.text = weather.city + ", " + weather.country;
            Temperature.text = CalculateDegreeFehrenheit(weather.temperature).ToString("F0") + "°F";
            MinMaxTemperature.text = "Min: " + CalculateDegreeFehrenheit(weather.minTemperature).ToString("F0") +
                                     "°F - Max: " + CalculateDegreeFehrenheit(weather.maxTemperature).ToString("F0") + "°F";
            WindSpeed.text = "Wind Speed: " + weather.wind.ToString();
            RainMaker();
            CloudMaker();
            SnowMaker();
            SunMaker();
            newSearch = false;
            hasWeather = false;
        }

    }

    private float CalculateDegreeFehrenheit(float kelvin)
    {
        float fehrenheit = (kelvin * 1.8f) - 459.67f;
        return fehrenheit;
    }

    void RainMaker()
    {
        if (weather.weatherID >= 300 && weather.weatherID <= 321)
        {
            rainScript.RainIntensity = .5f;
            rainClouds.SetActive(true);
        }
        else if (weather.weatherID >= 500 && weather.weatherID <= 531)
        {
            rainScript.RainIntensity = 1.0f;
            rainClouds.SetActive(true);
        }
        else
        {
            rainScript.RainIntensity = 0f;
            rainClouds.SetActive(false);
        }
    }

    void CloudMaker()
    {
        if (weather.weatherID >= 801 && weather.weatherID <= 804)
        {
            clouds.SetActive(true);
        }
        else
        {
            clouds.SetActive(false);
        }
    }

    void SnowMaker()
    {
        if (weather.weatherID >= 600 && weather.weatherID <= 622)
        {
            snowClouds.SetActive(true);
            snow.SetActive(true);
        }
        else
        {
            snowClouds.SetActive(false);
            snow.SetActive(false);
        }
    }

    void SunMaker()
    {
        string tempString = weather.icon;
        string lastCharacter = tempString.Substring(tempString.Length - 1, 1);

        switch (lastCharacter)
        {
            case "d":
                sun.GetComponent<Light>().enabled = true;
                sun.transform.rotation = Quaternion.Euler(153f, 0f, 0f);
                break;
            case "n":
                sun.transform.rotation = Quaternion.Euler(190.5f, 0f, 0f);
                if (weather.weatherID >= 300 && weather.weatherID <= 321 || weather.weatherID >= 500 && weather.weatherID <= 531)
                {
                    rainScript.RainIntensity = .5f;
                    rainClouds.SetActive(true);
                    sun.GetComponent<Light>().enabled = true;
                }
                else
                {
                    sun.GetComponent<Light>().enabled = false;
                }
                break;
            default:
                break;
        }
    }
}
