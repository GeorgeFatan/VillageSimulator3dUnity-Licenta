using System;
using System.Reflection;
using UnityEngine;

public class StCubes : MonoBehaviour
{
    public enum StareCuburi { Empty, Planted, ReadyToPlant, Weeds}
    public StareCuburi stareCurenta = StareCuburi.Weeds;

    public GameObject weedPrefab; //ref la prefabu buruiana uscata
    public GameObject currentWeebOnCube; // prefabu de buruiana care e pe un cub
    public PlantData currentPlantData; // planta curenta pe cub
    public int currentStage; // stagiu curent de crestere 
    public GameObject currentPlantInstance; // ref la instanta plantei 
    public bool isWatered; // starea udarii pentru leguma
    
    void Start()
    {
        if(SaveSystem.gameIsLoaded)
        {
            return;
        }

        stareCurenta = StareCuburi.Weeds;
        if(weedPrefab != null && stareCurenta == StareCuburi.Weeds)
        {
            currentWeebOnCube = Instantiate(weedPrefab,transform.position, Quaternion.identity);
            currentWeebOnCube.transform.SetParent(transform);

            currentWeebOnCube.transform.localPosition = new Vector3(0, 0.5f, 0);
            currentWeebOnCube.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            
        }
    }

    public void ResetStare()
    {
        stareCurenta = StareCuburi.ReadyToPlant;
        currentPlantData = null;
        currentStage = 0;
        isWatered = false;
        if(currentPlantData != null)
        {
            Destroy(currentPlantInstance);
            currentPlantInstance = null;
        }
        Debug.Log($"Cubul {gameObject.name} a fost resetat la starea ReadyToPlant.");
    }

    public bool CanPlant()
    {
        return stareCurenta == StareCuburi.ReadyToPlant;
    }

    public void Planted(PlantData plantData, int stage, GameObject plantInstance)
    {
        stareCurenta = StareCuburi.Planted;
        currentPlantData = plantData;
        currentStage = stage;
        currentPlantInstance = plantInstance;
        isWatered = false; // initial planta nu e udata.
        Debug.Log($"Cubul {gameObject.name} a fost plantat.");
    }

    public void WaterPlant()
    {
        isWatered = true;
    }

}