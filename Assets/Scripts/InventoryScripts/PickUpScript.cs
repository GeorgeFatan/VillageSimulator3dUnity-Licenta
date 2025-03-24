using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            InventorySystem inventory = FindObjectOfType<InventorySystem>();
            if (inventory != null)
            {
                inventory.AddItemToSlot(); 
                Destroy(gameObject); //distrugem planta care o recoltam
                Debug.Log("Planta a fost recoltată!");

                // gasim cubu asociat si ii resetam starea 
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit))
                {
                    StCubes soilCube = hit.collider.GetComponent<StCubes>();
                    if (soilCube != null)
                    {
                        soilCube.ResetStare(); // reset cube stare initial
                        Debug.Log($"Cubul {soilCube.gameObject.name} a fost resetat.");
                    }
                    else
                    {
                        Debug.LogWarning("Cubul asociat nu a fost găsit!");
                    }
                }
            }
            else
            {
                Debug.LogError("InventorySystem nu a fost găsit!");
            }
        }
    }
}