using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> inventorySlots; // Lista sloturilor de inventar
    public int maxItemsPerSlot = 12; // Nr maxim de iteme per slot

    // adaugam un item în inventar
    public void AddItemToSlot(PlantData plantData)
    {
        if (plantData == null || plantData.plantTexture == null)
        {
            Debug.LogError("PlantData sau textura lipsesc!");
            return;
        }

        // gasim un slot care contine deja acelasi item
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == plantData.plantTexture && slot.itemCount < maxItemsPerSlot)
            {
                slot.IncrementItemCount(); // Creștem numărul de iteme
                Debug.Log($"Item {plantData.plantName} adaugat in slot existent. Total: {slot.itemCount}");
                return;
            }
        }

        // Gasim un slot gol pentru o planta noua
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == null)
            {
                slot.SetItem(plantData.plantTexture, 1); // Setan textura si numarul de iteme
                Debug.Log($"Item {plantData.plantName} adaugat intr-un slot nou.");
                return;
            }
        }

        Debug.LogWarning("Inventarul este plin! Nu mai exista sloturi disponibile.");
    }

    // Verificam daca exista suficiente seminte in slot-ul inventarului
    public bool HasItemInSlot(Texture texture, int count)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == texture && slot.itemCount >= count)
            {
                return true; 
            }
        }
        return false; 
    }

    // Stergem seeds din inventar
    public void RemoveItemFromSlot(Texture texture)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == texture && slot.itemCount > 0)
            {
                slot.DecrementItemCount(); // Scădem numărul de semințe
                Debug.Log($"O samanta din slotul cu textura {texture.name} a fost plantata.");
                return;
            }
        }
        Debug.LogWarning($"Nu exista suficiente seminte pentru planta {texture.name}!");
    }
}