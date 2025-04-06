using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSelector : MonoBehaviour
{
    public List<PlantData> availablePlants;
    private int currentIndex = 0;
    public static PlantData selectedPlant;

    private bool isPlayerInRange = false;

    void Update()
    {
        if (!isPlayerInRange) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentIndex = (currentIndex - 1 + availablePlants.Count) % availablePlants.Count;
            Debug.Log($"Selectat: {availablePlants[currentIndex].plantName}");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentIndex = (currentIndex + 1) % availablePlants.Count;
            Debug.Log($"Selectat: {availablePlants[currentIndex].plantName}");
        }

        if (Input.GetKeyDown(KeyCode.Return)) // ENTER = confirmare
        {
            selectedPlant = availablePlants[currentIndex];
            Debug.Log($"Planta selectată: {selectedPlant.plantName}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Intrat în zona de selecție a plantei.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Ieșit din zona de selecție.");
        }
    }
}
