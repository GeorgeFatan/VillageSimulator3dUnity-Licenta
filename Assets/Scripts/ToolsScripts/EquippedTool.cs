using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedTool : MonoBehaviour
{
    public ToolData toolData;
    public Transform equipPoint;
    public Transform playerTransform;
    private InventorySystem inventory;
    private bool isPlayerNearby = false;


    public void Initialize(ToolData data, Transform equip, Transform player, InventorySystem inventar)
    {
        toolData = data;
        equipPoint = equip;
        playerTransform = player;
        inventory = inventar;
    }

    private void Update()
    {
        if(isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            EquipTool(); // equip // = // 
        }

        if(Input.GetKeyDown(KeyCode.Q) && transform.parent == equipPoint)
        {
            DropTool(); // drop la unealta care e practic o instanta a uneltei principale
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    void EquipTool()
    {
        transform.SetParent(equipPoint); // Atasam obiectul la punctul de echipare
        transform.position = equipPoint.position;
        transform.rotation = equipPoint.rotation;

        Debug.Log($"Unealta {toolData.toolName} a fost echipata.");

        // Asociem un slot din inventar
        StBucket bucketScript = GetComponent<StBucket>();
        if (bucketScript != null)
        {
            int availableSlot = inventory.GetFirstAvailbleSlot(); // Gasim primul slot liber
            if (availableSlot != -1) // Verif daca exista sloturi disponibile
            {
                bucketScript.assignedSlot = availableSlot; // Setam slotul asociat
                Debug.Log($"Tool-ul {toolData.toolName} a fost asociat cu slotul {availableSlot + 1}.");
            }
            else
            {
                Debug.LogWarning($"Inventarul este plin! Nu s-a putut asocia un slot pentru {toolData.toolName}.");
                bucketScript.ResetareAssignedSlot(); // ramane resetat -1
            }
        }

        // Adaugam tool-ul in inventar
        if (inventory != null)
        {
            inventory.AddToolToSlot(toolData);
            Debug.Log($"Unealta {toolData.toolName} a fost adaugata intr-un slot liber in inventar.");
        }

        // Dezactivam collider-ul pentru a preveni probleme la alte functionalitati care folosesc aceleasi taste.
        GetComponent<Collider>().enabled = false;

        // Setan tool-ul echipat
        inventory.equippedTool = gameObject;
    }

    void DropTool()
    {
        transform.SetParent(null); // Eliminam obiectul de la EquipPoint
        transform.position = playerTransform.position; // Plasam obiectul pe jos
        transform.rotation = Quaternion.Euler(0, 0, 0); // Setan obiectu sa fie pus pe rotatia 0 0 0 Adica in picioare (inca nu merge cum vreau)


        Debug.Log($"Unealta {toolData.toolName} a fost pusa pe jos.");

        // Resetam assignedSlot 
        StBucket bucketScript = GetComponent<StBucket>();
        if (bucketScript != null)
        {
            bucketScript.ResetareAssignedSlot();
            Debug.Log($"Slotul unealtei {toolData.toolName} resetat..");
        }

        // Scoatem tool-ul din inventar
        if (inventory != null)
        {
            inventory.RemoveToolFromSlot(toolData);
            Debug.Log($"Unealta {toolData.toolName} a fost eliminata din inventar.");
        }

        // Eliminam referinta  din InventorySystem
        if (inventory != null && inventory.equippedTool == gameObject)
        {
            inventory.equippedTool = null;
            Debug.Log("Referinta la tool-ul echipat a fost eliminata.");
        }

        // Activam collider-ul pentru a putea ridica din nou obiectu de pe jos.
        GetComponent<Collider>().enabled = true;
    }
}
