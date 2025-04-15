using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipObjectToHand : MonoBehaviour
{
    public ToolData toolData; // ref la toolData
    public Transform equipPoint;
    public GameObject equippedTool; // ref la unealta echipata
    private bool isPlayerNearby = false;
    private InventorySystem inventory; // ref la inventory system


    // sper sa mearga
    private bool coolDownTaste = false;
    private float coolDownTasteTime = 1.0f;


    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
    }

    void Update()
    {
       
            if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !coolDownTaste)
            {
                EquipTool();
                coolDownTaste = true;
                Invoke(nameof(ResetCooldown), coolDownTasteTime);
            }

            if (equippedTool != null && Input.GetKeyDown(KeyCode.E) && !coolDownTaste)
            {
                // Daca pun tasta E , intra in DropTool. daca las orice alta tasta.. NU
                DropTool();
            }
        
    }

    void ResetCooldown()
    {
        coolDownTaste = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Apasa pe E pentru a lua galeata de jos..");
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
        if (equippedTool == null)
        {
            equippedTool = Instantiate(toolData.toolPrefab, equipPoint.position, equipPoint.rotation);
            equippedTool.transform.SetParent(equipPoint); // Atașăm unealta la punctul de echipare
            Debug.Log($"Unealta de tip {toolData.toolName} a fost echipată!");

            if (inventory != null)
            {
                inventory.AddToolToSlot(toolData); // Adăugăm unealta în inventar
                Debug.Log($"Unealta {toolData.toolName} a fost adăugată în inventar.");
            }

            // Actualizează ToolData pentru noua instanță (dacă e necesar)
            toolData.toolPrefab = equippedTool; // Asociem clona ca prefab activ

            gameObject.SetActive(false); // Dezactivăm obiectul original
        }
        else
        {
            Debug.Log("Unealta este deja echipată.");
        }
    }

    void DropTool()
    {
        if (equippedTool != null)
        {
            // Determinăm poziția de drop
            Vector3 dropPosition = transform.position;
            dropPosition.y -= 2f;

            // Instanțiază unealta pe jos
            Instantiate(equippedTool, dropPosition, Quaternion.identity);
            Debug.Log($"Unealta {toolData.toolName} a fost pusă jos!");

            // Eliminăm unealta din inventar
            if (inventory != null)
            {
                inventory.RemoveToolFromSlot(toolData);
                Debug.Log($"Unealta {toolData.toolName} a fost eliminată din inventar.");
            }

            Destroy(equippedTool); // Elimină instanța echipată
            equippedTool = null;  // Resetăm referința
        }
        else
        {
            Debug.LogWarning("Nu ai nicio unealtă echipată pentru a o da jos!");
        }
    }
}
