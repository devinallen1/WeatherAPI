using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using DigitalRuby.RainMaker;

public class WeatherAPI_Script : MonoBehaviour
{
    public string city;
    public string country;
    public string weatherDescription;
    public int weatherID;
    public string icon;
    public float temperature;
    public float minTemperature;
    public float maxTemperature;
    public float rain;
    public float wind;
    public float clouds;
    public string searchingCity;
    public string searchingCountry;
    public TMP_InputField input_SearchCity;
    public TMP_InputField input_SearchCountry;
    public GameObject rainPartical;


    private string apiUrl = "http://api.openweathermap.org/data/2.5/weather?q=";// ogden,us&appid=70c538965901887a28a187b206c95c28";
    private string appiKey = "&appid=70c538965901887a28a187b206c95c28";
    private string appiKey2 = "&appid=a724efed060b5ce0e20338f6c363409f";
    private UpdateWeatherConditions updateConditions;
    

    // Start is called before the first frame update
    void Start()
    {
        updateConditions = UpdateWeatherConditions.FindObjectOfType<UpdateWeatherConditions>();
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Button_Search();
        }
    }

    IEnumerator SendRequest()
    {
        UnityWebRequest www = UnityWebRequest.Get(apiUrl + searchingCity + "," + searchingCountry + appiKey2);
        yield return www.SendWebRequest(); ;
        if (www.error == null || www.error == "")
        {
            Debug.Log(www.downloadHandler.text);
            SetWeatherAtributes(www.downloadHandler.text);
            //updateConditions.hasWeather = true;
            //RainMaker();
            //Debug.Log("Response: " + www.ToString());
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    }


    void SetWeatherAtributes(string jsonString)
    {
        var weatherJson = JSON.Parse(jsonString);
        city = weatherJson["name"].Value;
        country = weatherJson["sys"]["country"].Value;
        weatherDescription = weatherJson["weather"][0]["description"].Value;
        weatherID = weatherJson["weather"][0]["id"].AsInt;
        temperature = weatherJson["main"]["temp"].AsFloat;
        minTemperature = weatherJson["main"]["temp_min"].AsFloat;
        maxTemperature = weatherJson["main"]["temp_max"].AsFloat;
        rain = weatherJson["rain"]["3h"].AsFloat;
        clouds = weatherJson["clouds"]["all"].AsInt;
        wind = weatherJson["wind"]["speed"].AsFloat;
        icon = weatherJson["weather"][0]["icon"].Value;
        updateConditions.hasWeather = true;
    }

    public void Button_Search()
    {
        if (input_SearchCity.text != "" && input_SearchCountry.text != "")
        {
            searchingCity = input_SearchCity.text;
            searchingCountry = input_SearchCountry.text;
            StartCoroutine(SendRequest()); 
            updateConditions.newSearch = true;
            //updateConditions.needWeather = true;
        }
    }

    

}
