using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SuraInventoryScript : MonoBehaviour
{
    public int selectedSlotIndex = -1;

    [System.Serializable]
    public class SuraSlot
    {
        public RawImage slotImage; 
        public PlantData plantData;
        public int count;
        public Text itemCountText;
        
        public void SetPlant(PlantData plant, int amount)
        {
            plantData = plant;
            count = amount;
            slotImage.texture = plant.plantTexture;
            itemCountText.text = count.ToString();
        }

        public void ClearSlot()
        {
            plantData = null;
            count = 0;
            slotImage.texture = null;
            itemCountText.text = null;
        }
    }

    public SuraSlot[] suraSlots; // Direct legat de UI
    public int capacity = 8; // Nr de sloturi

  /*  private void Awake()
    {
        for (int i = 0; i < suraSlots.Length; i++)
        {
            suraSlots[i].ClearSlot(); // Resetăm UI la start
        }
    }*/

    public void SelectSlot(int index)
    {
        if(index >= 0 && index < suraSlots.Length && suraSlots[index].plantData != null)
        {
            selectedSlotIndex = index;
        }
        else
        {
            selectedSlotIndex = -1;
        }
    }

    public bool AddPlant(PlantData plant, int amount)
    {
        for (int i = 0; i < suraSlots.Length; i++)
        {
            if (suraSlots[i].plantData == null)
            {
                suraSlots[i].SetPlant(plant, amount);
                Debug.Log($"Leguma {plant.plantName} a fost adaugata în slotul {i} din Sura.");
                return true;
            }
        }
        Debug.LogWarning("Sura este plina!");
        return false;
    }

    public bool RemovePlantAt(int index)
    {
        if (index >= 0 && index < suraSlots.Length && suraSlots[index].plantData != null)
        {
            Debug.Log($"Leguma {suraSlots[index].plantData.plantName} a fost scoasa din slotul {index}.");
            suraSlots[index].ClearSlot();
            return true;
        }
        Debug.LogWarning("Nu s-a găsit leguma!");
        return false;
    }

    public int GetFirstPlantIndex()
    {
        for (int i = 0; i < suraSlots.Length; i++)
        {
            if (suraSlots[i].plantData != null)
            {
                return i; // Returneaza primul slot ocupat
            }
        }
        return -1; // Daca nu exista plante
    }
}