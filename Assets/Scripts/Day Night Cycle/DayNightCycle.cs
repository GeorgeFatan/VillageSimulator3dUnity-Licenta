using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time")]
    [Tooltip("Day Length in Minutes")]
    [SerializeField]
    private float _targetDayLength = 0.5f; // lungimea unei zile
    public float targetDayLength => _targetDayLength;

    [SerializeField]
    [Range(0f, 1f)]
    private float _timeOfDay = 7f / 24f; // Inițiere la ora 7 dimineața (7/24)
    public float timeOfDay => _timeOfDay;

    [SerializeField]
    private int _dayNumber = 0;
    public int dayNumber => _dayNumber;

    [SerializeField]
    private int _yearNumber = 0;
    public int yearNumber => _yearNumber;

    private float _timeScale = 100f;

    [SerializeField]
    private int _yearLength = 100;
    public int yearLength => _yearLength;

    public bool pause = false;

    [Header("Sun Light")]
    [SerializeField]
    private Transform dailyRotation;
    [SerializeField]
    private Light sun;
    [SerializeField]
    private Light moon; // Adăugăm lumina lunii
    private float intensity;
    [SerializeField]
    private float sunBaseIntensity = 1f;
    [SerializeField]
    private float sunVariation = 1.5f;
    [SerializeField]
    private Gradient sunColor;
    [SerializeField]
    private Gradient moonColor; // Adăugăm gradientul pentru culoarea lunii
    [SerializeField]
    private AnimationCurve sunIntensityCurve; // Adăugăm curba de intensitate a soarelui

    private void Update()
    {
        if (!pause)
        {
            UpdateTimeScale();
            UpdateTime();
        }
        AdjustSunRotation();
        AdjustMoonRotation();
        SunIntensity();
        AdjustSunColor();
        AdjustMoonColor();
    }

    private void UpdateTimeScale()
    {
        _timeScale = 24 / (_targetDayLength / 60);
    }

    private void UpdateTime()
    {
        _timeOfDay += Time.deltaTime * _timeScale / 86400; // secunde într-o zi
        if (_timeOfDay > 1) // o nouă zi
        {
            _dayNumber++;
            _timeOfDay -= 1;

            if (_dayNumber > _yearLength) // an nou 
            {
                _yearNumber++;
                _dayNumber = 0;
            }
        }
    }

    private void AdjustSunRotation()
    {
        float sunAngle = _timeOfDay * 360f - 90f; // Offset pentru a începe la ora 7
        dailyRotation.transform.localRotation = Quaternion.Euler(new Vector3(sunAngle, 0f, 0f));
    }

    private void AdjustMoonRotation()
    {
        float moonAngle = _timeOfDay * 360f + 90f; // Luna este opusă soarelui
        moon.transform.localRotation = Quaternion.Euler(new Vector3(moonAngle, 0f, 0f));
    }

    private void SunIntensity()
    {
        intensity = Vector3.Dot(sun.transform.forward, Vector3.down);
        intensity = Mathf.Clamp01(intensity);
        sun.intensity = sunBaseIntensity * sunIntensityCurve.Evaluate(_timeOfDay);

        // Gestionăm intensitatea lunii
        float moonIntensity = 1 - intensity;
        moon.intensity = moonIntensity * sunVariation * sunBaseIntensity;
    }

    private void AdjustSunColor()
    {
        sun.color = sunColor.Evaluate(intensity);
    }

    private void AdjustMoonColor()
    {
        float moonIntensity = 1 - intensity;
        moon.color = moonColor.Evaluate(moonIntensity);
    }
}
