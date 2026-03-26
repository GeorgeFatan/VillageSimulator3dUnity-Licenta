using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyTerrainScript : MonoBehaviour
{
    public GameObject terrainToBuy; // ref la terenu care vrem sa-l cumparam
    public int terrainPrice = 10;
    private bool playerInTrigger = false;
    public bool terrainUnlocked = false; // var pentru a verif daca terenu a fost deja cumparat
    public GameObject buyTrigger; // ref la ob cu triggerul pt cumparare

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

    public void LockTerrain()
    {
        terrainUnlocked = false;
        terrainToBuy.SetActive(false);
        buyTrigger.SetActive(true); 
    }

    public void UnlockTerrain()
    {
        terrainUnlocked = true;
        terrainToBuy.SetActive(true);
        buyTrigger.SetActive(false);
    }

    void BuyTerrain()
    {
        if(SellScript.instantaBuyTerrain.playerMoney >= terrainPrice)
        {
            SellScript.instantaBuyTerrain.playerMoney -= terrainPrice; // scadem banii
            UnlockTerrain(); // deblocam terenul
            SellScript.instantaBuyTerrain.UpdateMoneyUI(); // actualizam UI
            Debug.Log($"Teren achizitiona! Bani ramasi: {SellScript.instantaBuyTerrain.playerMoney}");
            gameObject.SetActive(false); // dezactivam triggerul dupa cumparare
            
        }
        else
        {
            Debug.LogWarning("Nu ai suficienti bani pentru a cumpara un teren nou..");
        }
    }

   public void ForceExitTrigger()
    {
        playerInTrigger = false; 
       
    }

}
