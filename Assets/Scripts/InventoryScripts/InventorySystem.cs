using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> inventorySlots; // Lista sloturilor
    public int maxItemsPerSlot = 12; // Nr maxim de iteme per slot

    public void AddItemToSlot(PlantData plantData)
    {
        if (plantData == null || plantData.plantTexture == null)
        {
            Debug.LogError("PlantData sau textura lipsesc!");
            return;
        }

        // Gaseste un slot care deja contine planta selectata
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == plantData.plantTexture && slot.itemCount < maxItemsPerSlot)
            {
                slot.IncrementItemCount(); // Crestere numarul de iteme
                Debug.Log($"Item {plantData.plantName} adăugat în slot existent. Total: {slot.itemCount}");
                return;
            }
        }

        // Găsește un slot gol pentru plante noi
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == null)
            {
                slot.SetItem(plantData.plantTexture, 1); // Set textura automat
                Debug.Log($"Item {plantData.plantName} adăugat într-un slot nou.");
                return;
            }
        }

        // Mesaj de avertizare pentru inventar plin
        Debug.LogWarning("Inventarul este plin! Nu mai există sloturi disponibile.");
    }
}