using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    public RawImage slotImage; // Imaginea obiectului din slot
    public Text itemCountText; // Textul pentru numărul de obiecte
    public Texture itemTexture; // Textura obiectului din slot
    public int itemCount = 0; // Numărul de obiecte din slot

    public void SetItem(Texture texture, int count)
    {
        itemTexture = texture;
        itemCount = count;

        if (slotImage != null)
        {
            slotImage.texture = texture; // Aplică textura obiectului în RawImage
            Debug.Log($"Textura {texture.name} a fost setată în slot.");
        }
        else
        {
            Debug.LogError("Slot Image nu este configurat corect în Inspector!");
        }

        UpdateItemCountText(); // Actualizează numărul de obiecte
    }

    public void IncrementItemCount()
    {
        itemCount++;
        if (slotImage != null && itemTexture != null)
        {
            slotImage.texture = itemTexture; // Forțează aplicarea texturii
        }

        UpdateItemCountText();
    }

    private void UpdateItemCountText()
    {
        itemCountText.text = itemCount > 1 ? itemCount.ToString() : ""; // Ascunde textul dacă numărul e 1
    }
}
    