using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SellScript : MonoBehaviour
{
    public InventorySystem inventorySystem; 
    public int pretPerItem = 10; 
    private int playerMoney = 0; 
    public TextMeshProUGUI moneyText; 
    public int selectedSlotIndex = 0;  // deocamdata nu merge

    private void Update()
    {
        //HandleSlotSelection(); 
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
                int totalItems = selectedSlot.itemCount;
                int baniCastigati = totalItems * pretPerItem;

               
                playerMoney += baniCastigati;

                // reset slot 
                selectedSlot.ClearSlot();

                // apar bannii sus
                UpdateMoneyDisplay();
                Debug.Log($"Ai vândut {totalItems} legume pentru {baniCastigati} bani! Total bani: {playerMoney}");
            }
            else
            {
                Debug.Log("Nu există legume în slotul selectat pentru a fi vândute!");
            }
        }
        else
        {
            Debug.LogWarning("Slot-ul selectat este invalid!");
        }
    }

    void UpdateMoneyDisplay()
    {
        if (moneyText != null)
        {
            moneyText.text = $"Bani: {playerMoney}"; 
        }
    }

 /*   void HandleSlotSelection()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectSlot(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectSlot(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectSlot(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectSlot(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { SelectSlot(4); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { SelectSlot(5); }
    }*/

    void SelectSlot(int index)
    {
        if (index >= 0 && index < inventorySystem.inventorySlots.Count)
        {
            selectedSlotIndex = index; // update slot-ul selectat
            Debug.Log($"Slot-ul {index + 1} a fost selectat.");
        }
        else
        {
            Debug.LogWarning("Indexul slot-ului selectat este în afara limitelor!");
        }
    }
}