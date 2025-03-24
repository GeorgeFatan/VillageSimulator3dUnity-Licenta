using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> inventorySlots; 
    public Texture cornTexture; 
    private int maxItemsPerSlot = 64;
    public void AddItemToSlot()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemTexture == cornTexture)
            {
                if (slot.itemCount < maxItemsPerSlot)
                {
                    slot.IncrementItemCount();
                    Debug.Log($"Items in slot: {slot.itemCount}");
                    return;
                }
            }
        }

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemTexture == null)
            {
                slot.SetItem(cornTexture, 1);
                Debug.Log("Item added to a new slot.");
                return;
            }
        }

        Debug.LogWarning("Inventory full!");
    }
}
