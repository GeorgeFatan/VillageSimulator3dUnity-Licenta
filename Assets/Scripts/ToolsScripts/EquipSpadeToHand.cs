using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSpadeToHand : MonoBehaviour
{
    public ToolData toolData; // ref catre obiectu harlet/sapa
    public Transform equipHarletPoint; //pct de echipare de pe armature
    public Transform playerTransform;
    private InventorySystem inventory; // ref la inventar
    private List<GameObject> spadesOnTerrain = new List<GameObject>(); // lista cu harletele puse pe jos, dupa ce luam din trigger

    private bool isPlayerNearby = false;

    private void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
    }

    private void Update()
    {
        if(isPlayerNearby && Input.GetKeyDown(KeyCode.E)) 
        {
            EquipHarlet();
           
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            DropHarlet();
            Debug.Log($"Unealta de tip {toolData.toolName} a fost pusa pe jos");
        }
    }

    void EquipHarlet()
    {
        GameObject newHarlet = Instantiate(toolData.toolPrefab, equipHarletPoint.position,equipHarletPoint.rotation);
        newHarlet.transform.SetParent(equipHarletPoint); // attach la EquipHarletPoint din armature

        newHarlet.transform.localPosition = new Vector3(-0.045f, 0.147f, 0.034f);
        newHarlet.transform.localRotation = Quaternion.Euler(24.534f, -45.742f, 180f);

        Debug.Log($"Unealta de tip {toolData.toolName} a fost echipata intr-un slot liber");

        // adaugam scriptul EquippedTool, care ne permite sa utilizam in continuare unealta dupa ce o lasam pe jos
        EquippedHarlet handlerEquippedTool = newHarlet.AddComponent<EquippedHarlet>();
        handlerEquippedTool.Initialize(toolData, equipHarletPoint, playerTransform, inventory);

        // adaugam collider pt game objectu harlet creat din trigger de  sapdes.
        BoxCollider boxColliderHarlet = newHarlet.AddComponent<BoxCollider>();
        boxColliderHarlet.isTrigger = true;
        boxColliderHarlet.size = new Vector3(0.5f, 1f, 0.5f);

        // adaugam unealta in inventar
        if(inventory != null)
        {
            inventory.AddToolToSlot(toolData);
        }

        // adaugam harletul intr-un slot din inventar liber
        if(inventory != null)
        {
            inventory.equippedTool = newHarlet;
        }

    }

    void DropHarlet()
    {
        // verificam daca exista un harlet echipat
        if(equipHarletPoint.childCount > 0)
        {
            GameObject harletToDrop = equipHarletPoint.GetChild(0).gameObject; // unealta echipata curent
            harletToDrop.transform.SetParent(null); // eliminam harletu din mana
            harletToDrop.transform.position = new Vector3(
                playerTransform.position.x,
                playerTransform.position.y + 0.5f,
                playerTransform.position.z
                );

            harletToDrop.transform.rotation = Quaternion.Euler(90, 0, 0);

            // eliminam harletu din inventar
            if(inventory != null)
            {
                inventory.RemoveToolFromSlot(toolData);
            }

            Destroy(harletToDrop);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Apasa pe E pentru a echipa unealta...");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }

    }
}
