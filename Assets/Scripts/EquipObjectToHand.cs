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

    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
    }


    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) // Apasam pe E pentru echipare
        {
            EquipTool();
        }
        if(equippedTool != null && Input.GetKeyDown(KeyCode.Q)) // Apasam pe Q pentru a lasa jos tool-ul 
        {
            DropTool();

        }
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
        if(equippedTool == null)
        {
            // instantiem galeata la pct definit de noi equip point

            equippedTool = Instantiate(toolData.toolPrefab, equipPoint.position, equipPoint.rotation);
            equippedTool.transform.SetParent(equipPoint);
            Debug.Log($"Unealta de tip {toolData.toolName} a fost echipata...");

            if(inventory != null)
            {
                inventory.AddToolToSlot(toolData);
            }

            gameObject.SetActive(false);
        }

        else
        {
            Debug.Log("Galeata este deja echipata");
        }
    }

    void DropTool()
    {
        if(equippedTool != null)
        {
            equippedTool.transform.SetParent(null);

            if(inventory != null)
            {
                inventory.RemoveToolFromSlot(toolData);
            }

            Instantiate(toolData.toolPrefab, equipPoint.position, Quaternion.identity);
            Debug.Log($"{toolData.toolName} a fost pusa jos!");

            equippedTool = null;
        }
    }

}
