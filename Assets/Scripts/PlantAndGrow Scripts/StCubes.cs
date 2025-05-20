using System;
using System.Reflection;
using UnityEngine;

public class StCubes : MonoBehaviour
{
    public enum StareCuburi { Empty, Planted, ReadyToPlant, Weeds}
    public StareCuburi stareCurenta = StareCuburi.Weeds;

    public GameObject weedPrefab; //ref la prefabu buruiana uscata
    public GameObject currentWeebOnCube; // prefabu de buruiana care e pe un cub
    
    void Start()
    {
        stareCurenta = StareCuburi.Planted;
        if(weedPrefab != null)
        {
            currentWeebOnCube = Instantiate(weedPrefab,transform.position, Quaternion.identity);
            currentWeebOnCube.transform.SetParent(transform);
        }
    }

    public void ResetStare()
    {
        stareCurenta = StareCuburi.ReadyToPlant;
        Debug.Log($"Cubul {gameObject.name} a fost resetat la starea ReadyToPlant.");
    }

    public bool CanPlant()
    {
        return stareCurenta == StareCuburi.ReadyToPlant;
    }

    public void Planted()
    {
        stareCurenta = StareCuburi.Planted;
        Debug.Log($"Cubul {gameObject.name} este plantat.");
    }

}