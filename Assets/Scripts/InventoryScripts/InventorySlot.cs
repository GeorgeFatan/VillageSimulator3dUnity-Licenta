using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    public RawImage slotImage; 
    public Text itemCountText; 
    public Texture itemTexture;  
    public int itemCount = 0; 

    public void SetItem(Texture texture, int count)
    {
        itemTexture = texture;
        itemCount = count;

        if (slotImage != null)
        {
            slotImage.texture = texture; 
            Debug.Log($"Textura {texture.name} a fost setată în slot.");
        }
        else
        {
            Debug.LogError("Slot Image nu este configurat corect în Inspector!");
        }

        UpdateItemCountText(); 
    }

    public void IncrementItemCount()
    {
        itemCount++;
        if (slotImage != null && itemTexture != null)
        {
            slotImage.texture = itemTexture; 
        }

        UpdateItemCountText();
    }

    private void UpdateItemCountText()
    {
        itemCountText.text = itemCount > 0 ? itemCount.ToString() : "";
    }

    public void ClearSlot()
    {
        itemTexture = null; // Șterge textura obiectului
        itemCount = 0; // Resetează contorul
        if (slotImage != null)
        {
            slotImage.texture = null; // Șterge imaginea din UI
        }
        if (itemCountText != null)
        {
            itemCountText.text = ""; // Golește textul
        }

        Debug.Log("Slot resetat!");

    }
}
    