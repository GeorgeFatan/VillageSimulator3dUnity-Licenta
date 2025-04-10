using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    public RawImage slotImage; // Imaginea din UI
    public Text itemCountText; // Textul care afiseaza numarul de obiecte
    private Texture itemTexture; 
    public int itemCount = 0; // Nr curent de obiecte în slot

    // Metodata pentru setarea unui item
    public void SetItem(Texture texture, int count)
    {
        itemTexture = texture;
        itemCount = count;

        if (slotImage != null)
        {
            slotImage.texture = texture; // Update imaginea in UI
            Debug.Log($"Textura {texture.name} a fost setata in slot.");
        }
        else
        {
            Debug.LogError("Slot Image nu este configurat corect in Inspector!");
        }

        UpdateItemCountText();
    }

    public void IncrementItemCount(int amount = 1)
    {
        itemCount += amount; // Crestem nr de iteme cu valoarea specif.
        UpdateItemCountText();
    }

    public void DecrementItemCount()
    {
        if (itemCount > 0) {
            itemCount--; //scadem nr de iteme (in cazu nostru de seminte)
            UpdateItemCountText();
        }
        if (itemCount == 0)
        {
            ClearSlot(); // golim slotu
            Debug.Log("Slotul este gol!!!! Nu mai sunt seminte (TEST)");
        }
    }
    private void UpdateItemCountText()
    {
        itemCountText.text = itemCount > 0 ? itemCount.ToString() : ""; // afiseaza nr de iteme
    }

    public void ClearSlot()
    {
        itemTexture = null; // Reset textura
        itemCount = 0; // Reset nr de iteme

        if (slotImage != null)
        {
            slotImage.texture = null; // Delete imaginea din UI
        }

        if (itemCountText != null)
        {
            itemCountText.text = ""; // Goleste textul
        }

        Debug.Log("Slot resetat!");
    }
}