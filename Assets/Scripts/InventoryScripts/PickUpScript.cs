using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    private PlantData plantData; // Referinta catre PlantData
    public Animator playerAnimator; // Animatorul pentru player

    void Start()
    {
        // Cauta PlantData prin PlantInfo
        PlantInfo plantInfo = GetComponent<PlantInfo>();
        if (plantInfo != null)
        {
            plantData = plantInfo.plantData;
        }
        else
        {
            Debug.LogError("PlantInfo nu este atasat acestui obiect!");
        }

        // Cauta Animatorul automat daca nu este setat in Inspector
        if (playerAnimator == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerAnimator = player.GetComponent<Animator>();
                if (playerAnimator != null)
                {
                    Debug.Log("Animatorul a fost setat automat pentru player.");
                }
                else
                {
                    Debug.LogWarning("Animatorul nu a fost gasit pe Player!");
                }
            }
            else
            {
                Debug.LogError("Obiectul cu tag-ul 'Player' nu a fost gasit in scena!");
            }
        }
    }

    void TriggerHarvestAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Recoltare"); // Declansam animatia
            Debug.Log("Animatia de recoltare a fost declansata.");
        }
        else
        {
            Debug.LogWarning("Animatorul player-ului nu este setat!");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PlantScript plantScript = FindObjectOfType<PlantScript>();

            if (plantScript != null && plantScript.IsReadyToHarvest(gameObject))
            {
                TriggerHarvestAnimation();

                if (plantData != null)
                {
                    InventorySystem inventory = FindObjectOfType<InventorySystem>();
                    if (inventory != null)
                    {
                        inventory.AddItemToSlot(plantData); // Adaugam PlantData in inventar
                        Destroy(gameObject); // Distrugem planta recoltata
                        Debug.Log($"{plantData.plantName} a fost recoltata si adaugata in inventar!");
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