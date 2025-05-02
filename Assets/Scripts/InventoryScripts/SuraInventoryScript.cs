using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuraInventoryScript : MonoBehaviour
{
    [System.Serializable]
    public class SuraSlot
    {
        public PlantData plantData;
        public int count;

        public SuraSlot()
        {
            plantData = null;
            count = 0;
        }
    }

    public int capacity = 10; // nr de sloturi
    public SuraSlot[] suraSlots;

    private void Awake()
    {
        suraSlots = new SuraSlot[capacity];
        for (int i = 0; i < capacity; i++)
        {
            suraSlots[i] = new SuraSlot();
        }
    }

    public bool AddPlant(PlantData plant, int amount = 12)
    {
        for (int i = 0; i < suraSlots.Length; i++)
        {
            // Ver daca slotul este liber 
            if (suraSlots[i].plantData == null)
            {
                suraSlots[i].plantData = plant; // Bagam leguma in slot
                suraSlots[i].count = amount;    // Setam nr de bucati
                Debug.Log($"Leguma {plant.plantName} a fost adaugata în slotul {i} din sura cu {amount} bucati.");
                return true;
            }
        }
        Debug.LogWarning("Sura este plina!");
        return false;
    }

    // Scoatem un slot intreg din sura in inventaru jucatorului
    public bool RemovePlantAt(int index)
    {
        if (index >= 0 && index < suraSlots.Length && suraSlots[index].plantData != null)
        {
            Debug.Log($"Leguma {suraSlots[index].plantData.plantName} din slotul {index} a fost scoasa din sura (stocul de {suraSlots[index].count} bucati este eliminat).");
            suraSlots[index].plantData = null;
            suraSlots[index].count = 0;
            return true;
        }
        Debug.LogWarning("Nu s-a gasit leguma in slotul specificat sau indexul este invalid!");
        return false;
    }

    // Returnam din SuraSlots primul slot care nu e null.
    public int GetFirstPlantIndex()
    {
        for (int i = 0; i < suraSlots.Length; i++)
        {
            if (suraSlots[i].plantData != null)
            {
                return i;
            }
        }
        return -1;
    }
}