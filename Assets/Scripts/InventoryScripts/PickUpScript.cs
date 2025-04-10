using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    private PlantData plantData; // Ref catre datele plantei
    public Animator playerAnimator; // Animator-ul jucătorului

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

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) // Rec cu E
        {
            PlantScript plantScript = FindObjectOfType<PlantScript>();

            // Ver daca planta este gata de recoltare
            if (plantScript != null && plantScript.IsReadyToHarvest(gameObject))
            {
                TriggerHarvestAnimation();

                if (plantData != null)
                {
                    InventorySystem inventory = FindObjectOfType<InventorySystem>();
                    if (inventory != null)
                    {
                        // Adăugăm planta recoltată în inventar
                        inventory.AddItemToSlot(plantData); // Se foloseste plantData.plantTexture
                        Destroy(gameObject); // Distrugem planta recoltata
                        Debug.Log($"{plantData.plantName} a fost recoltata si adaugata în inventar!");
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