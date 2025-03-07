using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            InventorySystem inventory = FindObjectOfType<InventorySystem>();
            inventory.AddItemToSlot(); // Adaugă imaginea asociată în inventar
            Destroy(gameObject); // Elimină obiectul porumb din scenă
        }
    }
}


