using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyTerrainScript : MonoBehaviour
{
    public GameObject terrainToBuy; // ref la terenu care vrem sa-l cumparam
    public int terrainPrice = 10;
    private bool playerInTrigger = false; 
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            Debug.Log("Daca ai destui bani in cont, apasa pe tata ENTER pentru a putea achizitiona un nou teren arabil..");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            Debug.Log("Ai iesit din trigger-ul pentru achizitionare de teren");
        }
        
    }
    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Return))
        {
            BuyTerrain();
        }
        
    }

    void BuyTerrain()
    {
        if(SellScript.instantaBuyTerrain.playerMoney >= terrainPrice)
        {
            SellScript.instantaBuyTerrain.playerMoney -= terrainPrice; // scadem banii
            terrainToBuy.SetActive(true);
            SellScript.instantaBuyTerrain.UpdateMoneyDisplay(); // actualizam UI
            Debug.Log($"Teren achizitiona! Bani ramasi: {SellScript.instantaBuyTerrain.playerMoney}");
            Destroy(gameObject); // dupa ce achizitionam terenul, distrugem obiectu care reprezinta triggeru de cumparare
        }
        else
        {
            Debug.LogWarning("Nu ai suficienti bani pentru a cumpara un teren nou..");
        }
    }

}
