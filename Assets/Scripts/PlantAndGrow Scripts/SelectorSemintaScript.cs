using System.Collections.Generic;
using UnityEngine;

public class SeedSelector : MonoBehaviour
{
    public List<PlantData> availablePlants; // Lista plantelor disponibile
    private int currentIndex = 0; // Indexul curent al selectiei
    public static PlantData selectedPlant; // Planta selectata curent

    private bool isPlayerInRange = false;

    // Ref catre InventorySystem
    private InventorySystem inventorySystem;

    void Start()
    {
        // Gasim sistemul de inventar in scena
        inventorySystem = FindObjectOfType<InventorySystem>();
        if (inventorySystem == null)
        {
            Debug.LogError("InventorySystem nu a fost gasit in scena curenta!");
        }
    }

    void Update()
    {
        if (!isPlayerInRange) return;

        if (Input.GetKeyDown(KeyCode.Q)) // stanga
        {
            currentIndex = (currentIndex - 1 + availablePlants.Count) % availablePlants.Count;
            Debug.Log($"Selectat: {availablePlants[currentIndex].plantName}");
        }

        if (Input.GetKeyDown(KeyCode.E)) // dreapta
        {
            currentIndex = (currentIndex + 1) % availablePlants.Count;
            Debug.Log($"Selectat: {availablePlants[currentIndex].plantName}");
        }

        if (Input.GetKeyDown(KeyCode.Return)) // Confirm cu Enter
        {
            selectedPlant = availablePlants[currentIndex];
            Debug.Log($"Seminte selectate: {selectedPlant.seedName}");

            // Adaugam un stack de 12 seminte in inventar
            AddSeedsToInventory(selectedPlant);
        }
    }

    private void AddSeedsToInventory(PlantData plantData)
    {
        if (inventorySystem == null || plantData == null)
        {
            Debug.LogError("InventorySystem sau PlantData este null!");
            return;
        }

        foreach (InventorySlot slot in inventorySystem.inventorySlots)
        {
            if (slot.slotImage.texture == plantData.seedTexture)
            {
                slot.IncrementItemCount(12); //  nr de seminte 12
                Debug.Log($"Seminte de {plantData.plantName} adaugate intr-un slot existent. Total: {slot.itemCount}");
                return;
            }
            else if (slot.slotImage.texture == null)
            {
                slot.SetItem(plantData.seedTexture, 12); // Ad un stack de 12 seminte intr-un slot gol
                Debug.Log($"Seminte de {plantData.plantName} adaugate intr-un slot nou.");
                return;
            }
        }

        Debug.LogWarning("Inventarul este plin! Nu mai exista sloturi disponibile.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Intrat in zona de selectia a semintelor.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Ai iesit din zona de selectia a semintelor.");
        }
    }
}