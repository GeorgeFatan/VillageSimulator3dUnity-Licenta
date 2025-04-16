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
            Debug.Log($"Apasa pe tasta e pentru a echipa unealta de tip {toolData.name}");
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
        // mutam unealta la equip point .. ca la EquipToHand

        transform.SetParent(equipPoint);
        transform.position = equipPoint.position;
        transform.rotation = equipPoint.rotation;

      

        Debug.Log($"Unealta de tip {toolData.toolName} a fost echipata");

        // adaugam intr-un slot din inventar
        if(inventory != null)
        {
            inventory.AddToolToSlot(toolData);
            Debug.Log($"Unealta/Tool-ul de tipul {toolData.toolName} a fost adaugat intr-un slot liber in inventar");
        }
        //dezactivam colideru ???
        GetComponent<Collider>().enabled = false;
    }

    void DropTool()
    {
        // o punem pe jos
        transform.parent = (null);
        transform.position = playerTransform.position; // o punem efectiv la pozitia jucatorului ((coordonatele sale))
                                                       
        // setam sa punem tool-ul in picioare (rotatie = 0 0 0 )
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Debug.Log($"Unealta/Tool-ul de tip {toolData.toolName} a fost pusa pe jos");

        //delete din inventar

        if(inventory != null)
        {
            inventory.RemoveToolFromSlot(toolData);
            Debug.Log($"Unealta/Tool-ul de tipul {toolData.toolName} a fost pusa pe jos, implicit slot-ul din inventar s-a eliberat");
        }
        // activam colideru 
        GetComponent<Collider>().enabled = true;
    }
}
