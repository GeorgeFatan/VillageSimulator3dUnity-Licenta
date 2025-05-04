using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DiscardSeedsInventory();
        }
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
                    if (bucketScript.assignedSlot != -1)
                    {
                        Debug.Log($"Slot asociat galetii: {bucketScript.assignedSlot}");
                        Debug.Log($"Slotul selectat: {currentSlot}");

                        if (bucketScript.assignedSlot == currentSlot)
                        {
                            equippedTool.SetActive(true);
                            Debug.Log($"Galeata echipata este vizibila pe slotul {currentSlot + 1}.");
                        }
                        else
                        {
                            equippedTool.SetActive(false);
                            Debug.Log($"Galeata echipata nu este pe slotul {currentSlot + 1}. A fost ascunsa.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Scriptul StBucket lipseste pe tool-ul echipat!");
                    }
                }
                else
                {
                    Debug.Log("Nu exista niciun tool echipat.");
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
        for (int i = 0; i < inventorySlots.Count; i++)
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
                slot.SetItem(plantData.plantTexture, 1, plantData, null);
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

    // Remove dupa plantare a semintelor din inventar
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

    // metode pt inventaru de la sura

    public PlantData GetSelectedPlant()
    {
        InventorySlot selectedSlot = inventorySlots[currentSlot];
        if (selectedSlot != null && selectedSlot.plantData != null)
        {
            return selectedSlot.plantData;
        }
        return null;
    }


    // sura inventar
    public int GetSelectedPlantCount()
    {
        InventorySlot selectedSlot = inventorySlots[currentSlot];
        if (selectedSlot != null && selectedSlot.plantData != null)
        {
            return selectedSlot.itemCount;
        }
        return 0;
    }

    public void RemoveSelectedPlantSlot(int amount)
    {
        InventorySlot selectedSlot = inventorySlots[currentSlot];
        if (selectedSlot != null && selectedSlot.plantData != null)
        {
            for (int i = 0; i < amount; i++)
            {
                selectedSlot.DecrementItemCount();
            }
        }
    }

    public bool AddItemToSuraSLot(PlantData plantData, int amount)
    {

        if (plantData == null || plantData.plantTexture == null)
        {
            Debug.LogError("PlantData sau textura lipsesc!");
            return false;
        }

        // Adaugam la un slot deja ocupat cu acelasi item
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == plantData.plantTexture)
            {
                int space = maxItemsPerSlot - slot.itemCount;
                if (space > 0)
                {
                    int toAdd = Mathf.Min(amount, space);
                    for (int i = 0; i < toAdd; i++)
                    {
                        slot.IncrementItemCount();
                    }
                    amount -= toAdd;
                    if (amount <= 0)
                        return true;
                }
            }
        }

        // Cautam un slot liber pentru restul itemelor
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.slotImage.texture == null)
            {
                int toAdd = Mathf.Min(amount, maxItemsPerSlot);
                slot.SetItem(plantData.plantTexture, toAdd, plantData, null);
                amount -= toAdd;
                if (amount <= 0)
                    return true;
            }
        }

        if (amount > 0)
        {
            Debug.LogWarning("Nu exista destule sloturi libere in inventar pentru nr de iteme curent.");
            return false;
        }
        return true;
    }

    // Putem arunca itemele din inventar

    public void DiscardSeedsInventory()
    {
        InventorySlot slot = inventorySlots[currentSlot];
        if (slot != null && slot.slotImage.texture != null)
        {
            Debug.Log($"Itemele din slotul {currentSlot + 1} au fost aruncate.");
            slot.ClearSlot(); 
        }
        else
        {
            Debug.Log($"Slot-ul {currentSlot + 1} nu contine iteme.");
        }

    }

}