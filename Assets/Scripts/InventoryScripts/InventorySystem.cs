using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> inventorySlots; // Lista sloturilor de inventar
    public int maxItemsPerSlot = 12; // Nr maxim de iteme per slot

  
    public void AddItemToSlot(PlantData plantData)
    {
        if (plantData == null || plantData.plantTexture == null)
        {
            Debug.LogError("PlantData sau textura lipsesc!");
            return;
        }

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == plantData.plantTexture && slot.itemCount < maxItemsPerSlot)
            {
                slot.IncrementItemCount();
                Debug.Log($"Item {plantData.plantName} adaugat in slot existent. Total: {slot.itemCount}");
                return;
            }
        }

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == null)
            {
                slot.SetItem(plantData.plantTexture, 1);
                Debug.Log($"Item {plantData.plantName} adaugat intr-un slot nou.");
                return;
            }
        }

        Debug.LogWarning("Inventarul este plin! Nu mai exista sloturi disponibile.");
    }

   // adaugam tool/unelte in inventar // bug la drop (Q) 
    public void AddToolToSlot(ToolData toolData)
    {
        if (toolData == null || toolData.toolTexture == null)
        {
            Debug.LogError("ToolData sau textura lipsesc!");
            return;
        }

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == toolData.toolTexture)
            {
                Debug.Log($"Unealta {toolData.toolName} este deja în inventar.");
                return;
            }
        }

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == null)
            {
                slot.SetItem(toolData.toolTexture, 1);
                Debug.Log($"Unealta {toolData.toolName} a fost adaugata in inventar.");
                return;
            }
        }

        Debug.LogWarning("Inventarul este plin! Nu mai exista sloturi disponibile.");
    }

   // Remove seminte/Legume din inventar
    public void RemoveItemFromSlot(Texture texture)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == texture && slot.itemCount > 0)
            {
                slot.DecrementItemCount();
                Debug.Log($"O samanta din slotul cu textura {texture.name} a fost plantata.");
                return;
            }
        }

        Debug.LogWarning($"Nu exista suficiente seminte pentru planta {texture.name}!");
    }

    // remove Tools/Unelte din inventar
    public void RemoveToolFromSlot(ToolData toolData)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == toolData.toolTexture)
            {
                slot.ClearSlot();
                Debug.Log($"Unealta {toolData.toolName} a fost eliminata din inventar.");
                return;
            }
        }

        Debug.LogWarning($"Unealta {toolData.toolName} nu exista in inventar!");
    }

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
}