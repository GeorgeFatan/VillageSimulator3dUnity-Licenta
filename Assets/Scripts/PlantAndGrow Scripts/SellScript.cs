using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SellScript : MonoBehaviour
{
    public InventorySystem inventorySystem; 
    public int playerMoney = 0; 
    public TextMeshProUGUI baniJucatorText; 
    public int selectedSlotIndex = 0;
    public static SellScript instantaBuyTerrain;

    private void Awake()
    {
        if(instantaBuyTerrain == null)
        {
            instantaBuyTerrain = this;  // instanta globala (pentru a putea fi accesat in scriptu de buyTerrain)
        }    
    
    }

    private void Update()
    {
        HandleSlotSelection(); 
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.O))
        {
            SellItems(); 
        }
    }

    void SellItems()
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < inventorySystem.inventorySlots.Count)
        {
            InventorySlot selectedSlot = inventorySystem.inventorySlots[selectedSlotIndex];

            if (selectedSlot != null && selectedSlot.itemCount > 0)
            {
               
                if (selectedSlot.plantData != null)
                {
                    int totalItems = selectedSlot.itemCount;
                    int pretPerItem = selectedSlot.plantData.plantPrice; // luam pretul din PlantData
                    int baniCastigati = totalItems * pretPerItem;

                    playerMoney += baniCastigati;

                    // clear slot, daca am vandut legumele din el
                    selectedSlot.ClearSlot();

                    UpdateMoneyUI();
                    Debug.Log($"Ai vandut {totalItems} {selectedSlot.plantData.plantName} pentru {baniCastigati} bani! Total bani: {playerMoney}");
                }
                else
                {
                    Debug.LogWarning("Nu poti vinde acest obiect! Doar legumele sunt acceptate.");
                }
            }
        }
    }

   public void UpdateMoneyUI()
    {
        if (baniJucatorText != null)
        {
            baniJucatorText.text = $"Bani: {playerMoney}"; 
        }
    }

   

   void HandleSlotSelection()
    {
          // apasam pe 1 , 2 ,3 ,4 ,5 ,6 ,7 , 8 pt a selecta slotu   
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectSlot(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectSlot(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectSlot(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectSlot(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { SelectSlot(4); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { SelectSlot(5); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { SelectSlot(6); }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { SelectSlot(7); }
    }

    void SelectSlot(int index)
    {
        if (index >= 0 && index < inventorySystem.inventorySlots.Count)
        {
            selectedSlotIndex = index; // update slot-ul selectat
            Debug.Log($"Slot-ul {index + 1} a fost selectat.");
        }
    }
}