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
            Debug.Log($"Textura {texture.name} a fost setata in slot.");
        }
        else
        {
            Debug.LogError("Slot Image nu este configurat corect in Inspector!");
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
        itemTexture = null; // Sterge textura obiectului
        itemCount = 0; // Reset contorul
        if (slotImage != null)
        {
            slotImage.texture = null; // Sterge imaginea din UI
        }
        if (itemCountText != null)
        {
            itemCountText.text = ""; // Goleste nr de iteme (textu ala)
        }

        Debug.Log("Slot resetat!");

    }
}
    