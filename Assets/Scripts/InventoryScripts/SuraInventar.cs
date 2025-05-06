using UnityEngine;

public class SuraInventar : MonoBehaviour
{
    public SuraInventoryScript suraInventar; // Ref la SuraInventoryScript 
    public InventorySystem inventorySystem;  // Ref la sistemul de inventar 

    private bool isPlayerInTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            Debug.Log("Esti in fata usii surii. Apasă F pentru a depune leguma si Enter pentru a retrage legumele dintr-un slot ocupat.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private void Update()
    {
        if (!isPlayerInTrigger) return;

        // Depozitarea slotului selectat din inventaru jucatorului intr-un slot liber din sura
        if (Input.GetKeyDown(KeyCode.F)) // Când apăsăm F pentru depozitare
        {
            PlantData selectedPlant = inventorySystem.GetSelectedPlant();
            int amount = inventorySystem.GetSelectedPlantCount(); // Obținem numărul de iteme exact

            if (selectedPlant != null && amount > 0)
            {
                if (suraInventar.AddPlant(selectedPlant, amount)) // Transferăm itemele în Sură
                {
                    inventorySystem.RemoveSelectedPlantSlot(amount); // Ștergem itemele din inventarul jucătorului
                    Debug.Log($"Leguma {selectedPlant.plantName} ({amount} bucăți) a fost depozitată în Sură.");
                }
                else
                {
                    Debug.Log("Inventarul Surii este plin!");
                }
            }
            else
            {
                Debug.Log("Nu ai legume selectate în inventar.");
            }
        }

        // Retragerea unui slot din sura in inventarul playerului
        if (Input.GetKeyDown(KeyCode.Return))
        {
            int index = suraInventar.GetFirstPlantIndex();
            if (index != -1)
            {
                SuraInventoryScript.SuraSlot slot = suraInventar.suraSlots[index];
                if (slot.plantData != null)
                {
                    int currentSlotAmount = slot.count; 
                    bool success = inventorySystem.AddItemToSuraSLot(slot.plantData, currentSlotAmount);
                    if (success)
                    {
                        suraInventar.RemovePlantAt(index);
                        Debug.Log($"Leguma a fost scoasa din inventarul surii in inventarul jucatorului (nr de {currentSlotAmount} bucati).");
                    }
                    else
                    {
                        Debug.Log("Nu exsita destul spatiu liber in inventar pentru cantitatea din slotul luat din inventarul surii.");
                    }
                }
            }
            else
            {
                Debug.Log("inventarul surii este goala.");
            }
        }
    }
}