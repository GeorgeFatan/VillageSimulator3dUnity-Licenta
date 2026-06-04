using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedSelector : MonoBehaviour
{
    public List<PlantData> availablePlants; // Lista plantelor disponibile
    private int currentIndex = 0; // Indexul curent al selectiei
    public static PlantData selectedPlant; // Planta selectata curent

    private bool isPlayerInRange = false;

    // Ref catre InventorySystem
    private InventorySystem inventorySystem;

    // UI 
    public GameObject semintaSelectionUI; // ref catre canvasu pentru UI de selector seminte
    public TextMeshProUGUI seedNameText;
 


    void Start()
    {
        // Gasim sistemul de inventar in scena
        inventorySystem = FindObjectOfType<InventorySystem>();
        if (inventorySystem == null)
        {
            Debug.LogError("InventorySystem nu a fost gasit in scena curenta!");
        }

        // ne asiguram ca acest Canvas UI este ascuns la inceputul rularii

        if(semintaSelectionUI != null)
        {
            semintaSelectionUI.SetActive(false);
        }

        ActualizeazaUI();
            
    }

    void Update()
    {
        if (!isPlayerInRange) return;

        if (Input.GetKeyDown(KeyCode.Q)) // stanga
        {
            currentIndex = (currentIndex - 1 + availablePlants.Count) % availablePlants.Count;
            Debug.Log($"Selectat: {availablePlants[currentIndex].plantName}");
            ActualizeazaUI();
        }

        if (Input.GetKeyDown(KeyCode.E)) // dreapta
        {
            currentIndex = (currentIndex + 1) % availablePlants.Count;
            Debug.Log($"Selectat: {availablePlants[currentIndex].plantName}");
            ActualizeazaUI();
        }

        if (Input.GetKeyDown(KeyCode.Return)) // Confirm cu Enter
        {
            selectedPlant = availablePlants[currentIndex];
            Debug.Log($"Seminte selectate: {selectedPlant.seedName}");

            // Adaugam un stack de 6 seminte in inventar
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
                slot.IncrementItemCount(6); //  nr de seminte 6
                Debug.Log($"Seminte de {plantData.plantName} adaugate intr-un slot existent. Total: {slot.itemCount}");
                return;
            }
            else if (slot.slotImage.texture == null)
            {
                slot.SetItem(plantData.seedTexture, 6); // Ad un stack de 6 seminte intr-un slot gol
                Debug.Log($"Seminte de {plantData.plantName} adaugate intr-un slot nou.");
                return;
            }
        }

        Debug.LogWarning("Inventarul este plin! Nu mai exista sloturi disponibile.");
    }
    
    private void ActualizeazaUI()
    {
        if(availablePlants == null || availablePlants.Count == 0)
        {
            return;
        }

        // updatam textul cu numele semintei
        if(seedNameText != null)
        {
            seedNameText.text = availablePlants[currentIndex].seedName;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Intrat in zona de selectia a semintelor.");
        }

        if (semintaSelectionUI != null)
        {
            semintaSelectionUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Ai iesit din zona de selectia a semintelor.");
        }


        if (semintaSelectionUI != null)
        {
            semintaSelectionUI.SetActive(false);
        }
    }
}