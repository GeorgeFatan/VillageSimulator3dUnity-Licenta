using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> inventorySlots; // Lista tuturor sloturilor de inventar
    public Texture cornTexture; // Textura asociată porumbului
    private int maxItemsPerSlot = 64; // Numărul maxim de obiecte pe slot

    public void AddItemToSlot()
    {
        Debug.Log("AddItemToSlot a fost apelată."); // Confirmare că metoda este chemată

        foreach (InventorySlot slot in inventorySlots)
        {
            Debug.Log("Se verifică un slot...");

            if (slot.itemTexture == cornTexture) // Dacă slotul conține deja acest obiect
            {
                Debug.Log($"Slotul este ocupat cu textura: {slot.itemTexture.name}.");
                if (slot.itemCount < maxItemsPerSlot) // Dacă nu s-a atins limita
                {
                    slot.IncrementItemCount();
                    Debug.Log($"Numărul actual de porumbi: {slot.itemCount}");
                }
                else
                {
                    Debug.LogWarning("Inventarul este plin pentru acest slot!");
                }
                return;
            }

            if (slot.itemTexture == null) // Dacă slotul este gol
            {
                Debug.Log("Slot gol găsit. Adăugăm textura porumbului...");
                slot.SetItem(cornTexture, 1); // Adaugă textura și inițializează contorul
                Debug.Log("Porumbul a fost adăugat în slot!");
                return;
            }
        }

        Debug.LogWarning("Inventarul este plin! Nu mai există sloturi disponibile.");
    }
}
