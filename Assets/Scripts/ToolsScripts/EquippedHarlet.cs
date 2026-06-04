using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedHarlet : MonoBehaviour
{
    public ToolData toolData;
    public Transform equipPoint;
    public Transform playerTransform;
    private InventorySystem inventory;
    private bool isPlayerNearby = false;
    private bool isEquipped = false;

    public void Initialize(ToolData data, Transform equip, Transform player, InventorySystem inventar)
    {
        toolData = data;
        equipPoint = equip;
        playerTransform = player;
        inventory = inventar;
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isEquipped)
        {
            EquipTool();
        }

        if (Input.GetKeyDown(KeyCode.Q) && isEquipped)
        {
            DropTool();
        }
    }

    void EquipTool()
    {
        transform.SetParent(equipPoint);
        transform.localPosition = new Vector3(-0.045f, 0.147f, 0.034f);
        transform.localRotation = Quaternion.Euler(24.534f, -45.742f, 180f);

        Debug.Log($"Harletul {toolData.toolName} a fost echipat.");
        isEquipped = true;
        isPlayerNearby = false; // Reset triggerul

       // add spade in inventar
        if (inventory != null)
        {
            inventory.AddToolToSlot(toolData);
            inventory.equippedTool = gameObject;
        }

        // dupa echipare dezacativam acel colider
        GetComponent<Collider>().enabled = false;
    }

    void DropTool()
    {
        transform.SetParent(null);
        transform.position = playerTransform.position + new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(90, 0, 0);

        Debug.Log($"Harletul {toolData.toolName} a fost pus pe jos.");
        isEquipped = false;
        isPlayerNearby = true; 

        // Delete harlet din inventar
        if (inventory != null)
        {
            inventory.RemoveToolFromSlot(toolData);
            inventory.equippedTool = null;
        }


        // reactivam colideru 
        GetComponent<Collider>().enabled = true;
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
}