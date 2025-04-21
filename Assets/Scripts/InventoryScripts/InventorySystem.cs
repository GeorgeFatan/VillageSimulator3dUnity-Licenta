using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> inventorySlots; // Lista sloturilor de inventar
    public int maxItemsPerSlot = 12; // Nr maxim de iteme per slot
    public int currentSlot = 0;
    public GameObject equippedTool;
    public Transform equipPoint;
    public PlantData plantData;

    void Update()
    {
        HandleSlotSelection(); // Gestionăm schimbarea sloturilor
    }



    void HandleSlotSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectSlot(0); } // Slot 1
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectSlot(1); } // Slot 2
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectSlot(2); } // Slot 3
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectSlot(3); } // Slot 4
    }


    void SelectSlot(int index)
    {
        if (index >= 0 && index < inventorySlots.Count)
        {
            currentSlot = index; // Actualizam slotul 
            Debug.Log($"Slot-ul {index + 1} a fost selectat.");

            // Verificam daca exista un tool echipat in inventar
            if (equippedTool != null)   
            {
                StBucket bucketScript = equippedTool.GetComponent<StBucket>();
                if (bucketScript != null)
                {
                    if(bucketScript.assignedSlot != -1)
                    {
                        Debug.Log($"Slot asociat galetii: {bucketScript.assignedSlot}");
                        Debug.Log($"Slotul selectat: {currentSlot}");
                        // Activăm sau dezactivăm găleata echipată
                        if (bucketScript.assignedSlot == currentSlot)
                        {
                            equippedTool.SetActive(true); // Activăm găleata echipată
                            Debug.Log($"Galeata echipată este activată pe slotul {currentSlot + 1}.");
                        }
                        else
                        {
                            equippedTool.SetActive(false); // Dezactivăm găleata echipată
                            Debug.Log($"Galeata echipată nu este pe slotul {currentSlot + 1}. A fost dezactivată.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Scriptul StBucket lipsește pe tool-ul echipat!");
                    }
                }
                else
                {
                    Debug.Log("Nu există niciun tool echipat. Obiectele din scenă nu sunt afectate.");
                }
            }

            else
            {
                Debug.LogWarning($"Indexul slotului selectat ({index}) este invalid!");
            }
        }
           
    }

    public int GetFirstAvailbleSlot()
    {
        for(int i = 0; i<inventorySlots.Count; i++)
        {
            if (inventorySlots[i].slotImage.texture == null)
            {
                return i; // returnam index pt primu slot liber
            }
        }
        Debug.LogWarning("Nu exista slot-uri libere.. inventarul este plin..");
        return -1;
    }

    public void AddItemToSlot(PlantData plantData)
    {
        if (plantData == null || plantData.plantTexture == null)
        {
            Debug.LogError("PlantData sau textura lipsesc!");
            return;
        }

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == plantData.plantTexture && slot.itemCount < maxItemsPerSlot)
            {
                slot.IncrementItemCount();
                Debug.Log($"Item {plantData.plantName} adaugat in slot existent. Total: {slot.itemCount}");
                return;
            }
        }

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == null)
            {
                slot.SetItem(plantData.plantTexture, 1, plantData,null);
                Debug.Log($"Item {plantData.plantName} adaugat intr-un slot nou.");
                return;
            }
        }

        Debug.LogWarning("Inventarul este plin! Nu mai exista sloturi disponibile.");
    }

   // adaugam tool/unelte in inventar // bug la drop (Q) 
    public void AddToolToSlot(ToolData toolData)
    {
        if (toolData == null || toolData.toolTexture == null)
        {
            Debug.LogError("ToolData sau textura lipsesc!");
            return;
        }

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == toolData.toolTexture)
            {
                Debug.Log($"Unealta {toolData.toolName} este deja în inventar.");
                return;
            }
        }

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == null)
            {
                slot.SetItem(toolData.toolTexture, 1, null, toolData);
                Debug.Log($"Unealta {toolData.toolName} a fost adaugata in inventar.");
                return;
            }
        }

        Debug.LogWarning("Inventarul este plin! Nu mai exista sloturi disponibile.");
    }

   // Remove seminte/Legume din inventar
    public void RemoveItemFromSlot(Texture texture)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == texture && slot.itemCount > 0)
            {
                slot.DecrementItemCount();
                Debug.Log($"O samanta din slotul cu textura {texture.name} a fost plantata.");
                return;
            }
        }

        Debug.LogWarning($"Nu exista suficiente seminte pentru planta {texture.name}!");
    }

    // remove Tools/Unelte din inventar
    public void RemoveToolFromSlot(ToolData toolData)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == toolData.toolTexture)
            {
                slot.ClearSlot();
                Debug.Log($"Unealta {toolData.toolName} a fost eliminata din inventar.");
                return;
            }
        }

        Debug.LogWarning($"Unealta {toolData.toolName} nu exista in inventar!");
    }

    public bool HasItemInSlot(Texture texture, int count)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == texture && slot.itemCount >= count)
            {
                return true;
            }
        }
        return false;
    }

   /* public bool ContainsTool(ToolData toolData)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            // Verificam daca textura din slot este aceeasi cu textura uneltei
            if (slot.slotImage.texture == toolData.toolTexture)
            {
                return true; // Unealta exista in inventar
            }
        }
        return false; // Unealta nu exista in inventar
    }*/
}