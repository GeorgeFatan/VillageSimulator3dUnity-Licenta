using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> inventorySlots; 
    public Texture cornTexture; 
    private int maxItemsPerSlot = 12; 

    public void AddItemToSlot()
    {
       
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemTexture == cornTexture && slot.itemCount < maxItemsPerSlot)
            {
                slot.IncrementItemCount(); // Crește contorul
                Debug.Log($"Item adaugat în slot. Total obiecte în acest slot: {slot.itemCount}");
                return; 
            }
        }

       // daca slotu e gol atunci:
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemTexture == null) 
            {
                slot.SetItem(cornTexture, 1); 
                Debug.Log("Item adaugat intr-un slot gol.");
                return; 
            }
        }

       
        Debug.LogWarning("Inventarul este plin! Nu mai exista sloturi disponibile.");
    }
}