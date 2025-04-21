using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    public RawImage slotImage; // Imaginea din UI
    public Text itemCountText; // Textul care afiseaza numarul de obiecte
    private Texture itemTexture; 
    public int itemCount = 0; // Nr curent de obiecte în slot
    public PlantData plantData; // ref la PlantData
    public ToolData toolData; // ref la ToolData
    public void SetItem(Texture texture, int count, PlantData refPlant = null, ToolData refTool = null)
    {
        itemTexture = texture;
        itemCount = count;
        if (refPlant != null)
        {
            plantData = refPlant;
            toolData = null;
        }
        else if(refTool != null)
        {
            toolData = refTool;
            plantData = null;  
        }
        if(slotImage != null)
        {
            slotImage.texture = texture; // Update la imagine din slot
            if(refPlant != null)
            {
                Debug.Log($"Leguma recoltata de tipul {refPlant.plantName} a fost pusa intr-un slot liber ");
            }
            else if(refTool != null)
            {
                Debug.Log($"Unealta de tipul {refTool.toolName} a fost pusa intr-un slot liber..");
            }
            else
            {
                Debug.LogError("Slot image nu este configurat corect in Inspector..");
            }
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
        plantData = null;
        toolData = null;

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