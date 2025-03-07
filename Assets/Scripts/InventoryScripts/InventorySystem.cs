using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<RawImage> inventorySlots; // Lista tuturor sloturilor
    public Texture cornTexture; // Textura asociată porumbului

    public void AddItemToSlot()
    {
        foreach (RawImage slot in inventorySlots)
        {
            if (slot.texture == null) // Verifică dacă slotul este gol
            {
                slot.texture = cornTexture; // Adaugă textura porumbului
                Debug.Log("Porumbul a fost adăugat în inventar!");
                return;
            }
        }
        Debug.Log("Inventarul este plin!"); // Mesaj de debug dacă toate sloturile sunt ocupate
    }
}
