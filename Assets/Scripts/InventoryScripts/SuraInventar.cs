using JetBrains.Annotations;
using UnityEngine;

public class SuraInventar : MonoBehaviour
{
    public SuraInventoryScript suraInventar; // Ref la SuraInventoryScript 
    public InventorySystem inventorySystem;  // Ref la sistemul de inventar 
    private bool isPlayerInTrigger = false;
    public GameObject inventarSuraUI;
    public bool isSuraUIActive = false;

    private void Start()
    {
        inventarSuraUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            isSuraUIActive = true;
            inventarSuraUI.SetActive(true);
            Debug.Log("Esti in fata usii surii. Apasă F pentru a depune leguma si Enter pentru a retrage legumele dintr-un slot ocupat.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            isSuraUIActive = false;
            inventarSuraUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isPlayerInTrigger) return;

        if (isSuraUIActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectSuraSlot(0); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectSuraSlot(1); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectSuraSlot(2); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectSuraSlot(3); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { SelectSuraSlot(4); }
            if (Input.GetKeyDown(KeyCode.Alpha6)) { SelectSuraSlot(5); }
            if (Input.GetKeyDown(KeyCode.Alpha7)) { SelectSuraSlot(6); }
            if (Input.GetKeyDown(KeyCode.Alpha8)) { SelectSuraSlot(7); }
        }

 
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
            int selectedSlot = suraInventar.selectedSlotIndex;
            if (selectedSlot != -1)
            {
                SuraInventoryScript.SuraSlot slot = suraInventar.suraSlots[selectedSlot];
                if (slot.plantData != null)
                {
                    int currentSlotAmount = slot.count; 
                    bool success = inventorySystem.AddItemToSuraSLot(slot.plantData, currentSlotAmount);
                    if (success)
                    {
                        suraInventar.RemovePlantAt(selectedSlot);
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

    public void SelectSuraSlot(int index)
    {
        if(index >= 0 && index < suraInventar.suraSlots.Length)
        {
            suraInventar.selectedSlotIndex = index;
            Debug.Log($"Slotul {index + 1} din Sura a fost selectat..");
        }
    }

}