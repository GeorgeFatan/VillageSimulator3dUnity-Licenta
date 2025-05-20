using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState
{
    // player armature
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    //day night cycle 
    public int daysPassed;
    public float currentHour;
    public Vector3 sunPosition;
    public Quaternion sunRotation;
    public Vector3 moonPosition;
    public Quaternion moonRotation;
    public List<SavedInventorySlot> inventorySlots; // lista sloturilor
    public int currentSlot;
    public List<SavedSuraSlots> suraInventorySlots; // lista cu sloturile surii


}

[System.Serializable]
public class SavedInventorySlot
{
    public string itemType; // plant sau tool
    public string itemName;
    public int itemCount;
}

[System.Serializable]
public class SavedSuraSlots
{
    public string itemType; 
    public string itemName;
    public int itemCount;
}
 

