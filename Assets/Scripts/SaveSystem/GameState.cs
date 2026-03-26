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
    public int playerMoney; // banii 
    public List<SoilCubeData> soilCubesData;

    // npc armature
    public Vector3 npcPositionM; // pt Brian NPC
    public Quaternion npcRotationM;
    public Vector3 npcPositionF; // pt Megan NPC
    public Quaternion npcRotationF;

    // terrain2 state
    public bool terrain2Unlocked; 

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


[System.Serializable]
public class SavedCubeState
{
    public string cubeName; // numele cubului
    public string currentState; // Starea curenta a cuburilor
}


[System.Serializable]
public class SoilCubeData
{
    public string cubeName;
    public string currentState;
    public bool hasWeed;
    public string plantName;
    public int plantStage;
    public bool isWatered;
}

