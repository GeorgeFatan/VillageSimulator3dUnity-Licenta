using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    private PlantData plantData;

    void Start()
    {
        // Obtine referinta la PlantData prin PlantInfo
        PlantInfo plantInfo = GetComponent<PlantInfo>();
        if (plantInfo != null)
        {
            plantData = plantInfo.plantData;
        }
        else
        {
            Debug.LogError("PlantInfo nu este atasat acestui obiect!");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PlantScript plantScript = FindObjectOfType<PlantScript>();

            if (plantScript != null && plantScript.IsReadyToHarvest(gameObject))
            {
                if (plantData != null)
                {
                    InventorySystem inventory = FindObjectOfType<InventorySystem>();
                    if (inventory != null)
                    {
                        inventory.AddItemToSlot(plantData); // Trimite PlantData
                        Destroy(gameObject); // Distruge planta recoltata
                        Debug.Log($"{plantData.plantName} a fost recoltata si adaugata in inventar!");
                    }
                    else
                    {
                        Debug.LogError("InventorySystem nu a fost gasit!");
                    }
                }
                else
                {
                    Debug.LogError("PlantData nu este setat!");
                }
            }
            else
            {
                Debug.LogWarning("Planta nu este gata de recoltare!");
            }
        }
    }
}