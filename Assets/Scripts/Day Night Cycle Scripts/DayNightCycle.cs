using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TimeController : MonoBehaviour
{
    [SerializeField]
    private float timeMultiplier; 
    [SerializeField]
    private float startHour;
    [SerializeField]
    public TextMeshProUGUI timeText;
    [SerializeField]
    private Light luminaSoare;
    [SerializeField]
    private float sunriseHour;
    [SerializeField]
    private float sunsetHour;
    [SerializeField]
    private Color dayAmbientLight;
    [SerializeField]
    private Color nightAmbientLight;
    [SerializeField]
    private AnimationCurve lightChangeCurve;
    [SerializeField]
    private float maxIntensitateLuminaSoare;
    [SerializeField]
    private Light luminaLuna;
    [SerializeField]
    private float maxIntensitateLuminaLuna;
    public DateTime currentTime;
    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;
    [SerializeField]
    public  int daysPassed; 
    private DateTime previousTime;  

    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);

        daysPassed = 0; 
        previousTime = currentTime; // previos time devine timpul din frame-ul curent
    }

   
    void Update()
    {
        UpdateTime();
        RotateSoare();
        UpdateLightSettings();
    }

    private void UpdateTime()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
        

       
        if (previousTime.Day != currentTime.Day)
        {
            daysPassed++;
            Debug.Log("Days Passed: " + daysPassed);
        }

        previousTime = currentTime; 
    }

    private void RotateSoare()
    {
        float rotatieSoare;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan durataDeLaRasaritLaApus = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan rasaritDurataTrecuta = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double procent = rasaritDurataTrecuta.TotalMinutes / durataDeLaRasaritLaApus.TotalMinutes;

            rotatieSoare = Mathf.Lerp(0, 180, (float)procent);
        }
        else
        {
            TimeSpan durataDeLaApusLaRasarit = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan apusDurataTrecuta = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double procent = apusDurataTrecuta.TotalMinutes / durataDeLaApusLaRasarit.TotalMinutes;

            rotatieSoare = Mathf.Lerp(180, 360, (float)procent);
        }

        luminaSoare.transform.rotation = Quaternion.AngleAxis(rotatieSoare, Vector3.right);
    }


    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(luminaSoare.transform.forward, Vector3.down);

        luminaSoare.intensity = Mathf.Lerp(0, maxIntensitateLuminaSoare, lightChangeCurve.Evaluate(dotProduct));

        luminaLuna.intensity = Mathf.Lerp(maxIntensitateLuminaLuna, 0, 1 - lightChangeCurve.Evaluate(dotProduct));

        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }

    //script pt Day Skip (somn)
    public void SkipLaZiuaUrmatoare()
    {
        daysPassed++;   
        Debug.Log($"Ziua curenta este acum: {daysPassed}");


        // ne trezim mereu la 8 dimi => reset ceas la ora 8
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(8.0f);
        Debug.Log($"Ora este acum: {currentTime.ToString("HH:mm")}");

        if(timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
        RotateSoare();
        UpdateLightSettings();
    }
}
