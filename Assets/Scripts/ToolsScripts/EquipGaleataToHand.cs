using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class EquipGaleataToHand : MonoBehaviour
{
    public ToolData toolData; // Referinta la ToolData
    public Transform equipPoint; // Punctul de echipare
    public Transform playerTransform; // Referinta la pozitia playerului
    private InventorySystem inventory; // Referinta la sistemul de inventar

    private List<GameObject> toolsOnTerrain = new List<GameObject>(); // Lista pentru galetile puse jos
    private bool isPlayerNearby = false; // Trigger pentru interactiune


    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>(); // Gasim inventarul automat
    }

    void Update()
    {
        // Interactam cu galeata principala pentru a genera o clona
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            EquipTool();
         
        }

        // Punem jos ultima galeata echipata daca apasam Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropTool();
            

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Playerul intra in triggerul galetii principale
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Apasa pe E pentru a lua o galeata...");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Playerul iese din trigger
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    void EquipTool()
    {
        // Generam o clona noua din galeata principala
        GameObject newTool = Instantiate(toolData.toolPrefab, equipPoint.position, equipPoint.rotation);
        newTool.transform.SetParent(equipPoint); // Atasam galeata la punctul de echipare
        newTool.transform.localPosition = new Vector3(0.017f, -0.081f, -0.003f);
        newTool.transform.localRotation = Quaternion.Euler(0f, 58.18f, 0f);

        Debug.Log($"Galeata clonata de tip {toolData.toolName} a fost echipata!");

        // Adaugam scriptul EquippedTool pe unealta clonata
        EquippedGaleata handlerEquippedTool = newTool.AddComponent<EquippedGaleata>();
        handlerEquippedTool.Initialize(toolData, equipPoint, playerTransform, inventory);

        // Script pentru starea galetii goala sau plina
        StBucket bucketScript = newTool.AddComponent<StBucket>();
        Debug.Log("Scriptul Bucket a fost atasat automat galetii clonate.");

        // Adaugam un collider pentru detectarea obiectului clonat
        BoxCollider boxCollider = newTool.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(1.0f, 1.0f, 1.0f);

        Debug.Log("Collider-ul trigger a fost configurat pentru galeata clonata.");

        // Adaugam tool-ul in inventar
        if (inventory != null)
        {
            inventory.AddToolToSlot(toolData);
            Debug.Log($"Unealta de tipul {toolData.toolName} a fost adaugata in inventar.");
        }

        //adaugam tool-ul la slot-ul din inventar. (practic la EquippedTool ca sa putem dezactiva activa cand apasam pe slot-uri diferite)
        if(inventory != null)
        {
            inventory.equippedTool = newTool;
        }
    }

    void DropTool()
    {
        // Verificam daca exista o galeata echipata
        if (equipPoint.childCount > 0)
        {
            GameObject toolToDrop = equipPoint.GetChild(0).gameObject; // Ultima galeata echipata
            toolToDrop.transform.SetParent(null); // Eliminam parent-ul
            toolToDrop.transform.position = playerTransform.position; // Mutam pe jos la pozitia playerului

            // setam sa punem tool-ul in picioare (rotatie = 0 0 0 )
            transform.rotation = Quaternion.Euler(0, 0, 0);

            Debug.Log($"Galeata de tip {toolData.toolName} a fost plasata pe jos la pozitia {playerTransform.position}.");

            // Eliminam galeata din inventar
            if (inventory != null)
            {
                inventory.RemoveToolFromSlot(toolData);
                Debug.Log($"Galeata {toolData.toolName} a fost eliminata din inventar.");
            }

            Destroy(toolToDrop);
        }
       
    }
}