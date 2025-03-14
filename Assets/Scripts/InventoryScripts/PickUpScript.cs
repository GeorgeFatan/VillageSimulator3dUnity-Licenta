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
                Debug.Log("Se apelează metoda AddItemToSlot...");
                inventory.AddItemToSlot(); // Adaugă planta în inventar
                Destroy(gameObject); // Distruge planta recoltată
                Debug.Log("Planta a fost recoltată!");
            }
            else
            {
                Debug.LogError("InventorySystem nu a fost găsit!");
            }
        }
    }
}
