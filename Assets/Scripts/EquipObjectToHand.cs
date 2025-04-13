using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipObjectToHand : MonoBehaviour
{
    public GameObject galeataPrefab;
    public Transform equipPoint;

    private bool isPlayerNearby = false;
    private GameObject equippedGaleata;


    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) // Apăsăm pe E pentru echipare
        {
            EquipGaleata();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Apasa pe E pentru a lua galeata de jos..");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            EquipGaleata();
        }
    }

    void EquipGaleata()
    {
        if(equippedGaleata == null)
        {
            // instantiem galeata la pct definit de noi equip point

            equippedGaleata = Instantiate(galeataPrefab, equipPoint.position, equipPoint.rotation);
            equippedGaleata.transform.SetParent(equipPoint);
            Debug.Log("Galeata a fost echipata...");

            gameObject.SetActive(false);
        }

        else
        {
            Debug.Log("Galeata este deja echipata");
        }
    }


}
