using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PlantScript plantScript = FindObjectOfType<PlantScript>(); 

            if (plantScript != null)
            {
                if (plantScript.IsReadyToHarvest(gameObject)) 
                {
                    InventorySystem inventory = FindObjectOfType<InventorySystem>();
                    if (inventory != null)
                    {
                        inventory.AddItemToSlot();
                        Destroy(gameObject); // dupa ce recoltam planta, distrugem obiectu de pe jos
                        Debug.Log("Planta a fost recoltată!");

                        // Practic dam reset la starea cubului asociat
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit))
                        {
                            StCubes soilCube = hit.collider.GetComponent<StCubes>();
                            if (soilCube != null)
                            {
                                soilCube.ResetStare();
                                Debug.Log($"Cubul {soilCube.gameObject.name} a fost resetat.");
                            }
                            else
                            {
                                Debug.LogWarning("Cubul asociat nu a fost gasit!");
                            }
                        }
                    }
                    else
                    { //debuguri 
                        Debug.LogError("InventorySystem nu a fost gasit!");
                    }
                }
                else
                {
                    Debug.LogWarning("Planta nu este gata pentru recoltare! (Stadiul 3 necesar)");
                }
            }
            else
            {
                Debug.LogError("PlantScript nu a fost gasit pe această planta!");
            }
        }
    }
}