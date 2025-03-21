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
                inventory.AddItemToSlot(); // Adaugă planta în inventar
                Destroy(gameObject); // Distruge planta recoltată
                Debug.Log("Planta a fost recoltată!");

                // Găsește cubul pe baza poziției plantei
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit))
                {
                    StCubes soilCube = hit.collider.GetComponent<StCubes>();
                    if (soilCube != null)
                    {
                        soilCube.ResetStare(); // Resetează cubul
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