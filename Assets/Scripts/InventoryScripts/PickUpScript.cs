using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    private PlantData plantData; // Ref catre datele plantei
    public Animator playerAnimator; // Animator-ul jucătorului
    public StCubes associatedCube; //ref catre cub

    void Start()
    {
        // ref la PlantData prin PlantInfo
        PlantInfo plantInfo = GetComponent<PlantInfo>();
        if (plantInfo != null)
        {
            plantData = plantInfo.plantData; // Setam PlantData din PlantInfo
        }
        else
        {
            Debug.LogError("PlantInfo nu este atasat acestui obiect!");
        }
    }

    // animatia de recoltare
    void TriggerHarvestAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Recoltare"); // animatia
            Debug.Log("Animatia de recoltare a fost declansata.");
        }
        else
        {
            Debug.LogWarning("Animator-ul jucatorului nu este setat!");
        }
    }

    void ResetSoilCube()
    {
        // Cautam componenta StCubes in GameObject-ul parent
        if (associatedCube != null)
        {
            associatedCube.ResetStare();
            Debug.Log($"Cubul {associatedCube.gameObject.name} a fost resetat la starea ReadyToPlant..");
        }
        else
        {
            Debug.LogError("Cubul asociat nu a fost setat!");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) // Recoltam cu  tasta E
        {
            PlantScript plantScript = FindObjectOfType<PlantScript>();

            // Verif daca planta este gata de recoltare
            if (plantScript != null && plantScript.IsReadyToHarvest(gameObject))
            {
                TriggerHarvestAnimation();

                if (plantData != null)
                {
                    InventorySystem inventory = FindObjectOfType<InventorySystem>();
                    if (inventory != null)
                    {
                        inventory.AddItemToSlot(plantData); // Adaugam leguma intr-un slot din inventar

                        ResetSoilCube();
                        Destroy(gameObject);

                        Debug.Log($"{plantData.plantName} a fost recoltata si cubul a fost resetat!");
                    }
                    else
                    {
                        Debug.LogError("InventorySystem nu a fost gasit!");
                    }
                }
                else
                {
                    Debug.LogError("PlantData nu este setat pentru acest obiect!");
                }
            }
            else
            {
                Debug.LogWarning("Planta nu este gata pentru recoltare!");
            }
        }
    }
}