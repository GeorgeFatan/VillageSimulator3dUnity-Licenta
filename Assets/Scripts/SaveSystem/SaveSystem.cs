using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject playerArmature; // Ref PlayerArmature
    [SerializeField]
    private TimeController timeController;
    [SerializeField]
    private Light sunLight; // ref la soare
    [SerializeField]
    private Light moonLight; // ref la luna
    [SerializeField]
    private InventorySystem inventorySystem; // ref la inventarul jucatorului
    [SerializeField]
    private SuraInventoryScript suraInventoryScript; // ref la inventaru surii
    [SerializeField]
    private List<PlantData> availablePlants; // Lista de PlantData
    [SerializeField]
    private List<ToolData> availableTools;   // Lista de ToolData

    private CharacterController characterController; // Ref la CharacterController
    private string savePath;

    private void Awake()
    {
        // Verificam daca player armature a fost adaugata in inspector
        if (playerArmature == null)
        {
            Debug.LogError("PlayerArmature nu a fost asignata");
            return;
        }

        // CharacterController-ul de pe PlayerArmature
        characterController = playerArmature.GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogWarning("CharacterController nu a fost gasit.");
        }

        // time controller
        if (timeController == null)
        {
            Debug.LogError("TimeController not assigned! Please drag the TimeController GameObject (with TimeController script) into the Inspector.");
            return;
        }

        if (sunLight == null)
        {
            Debug.LogError("SunLight not assigned! Please drag the Sun Light into the Inspector.");
            return;
        }

        if (moonLight == null)
        {
            Debug.LogError("MoonLight not assigned! Please drag the Moon Light into the Inspector.");
            return;
        }

        // inventory system save
        if (inventorySystem == null)
        {
            Debug.LogError("InventorySystem not assigned! Please drag the InventorySystem GameObject into the Inspector.");
            return;
        }

        // sura inventory system
        if (suraInventoryScript == null)
        {
            Debug.LogError("SuraInventorySystem not assigend! erroare.");
        }

        // Setem calea fisierului json
        savePath = Application.persistentDataPath + "/saveGame.json";
        Debug.Log("Jocul a fost salvat la ruta: " + savePath);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("Save key (0) pressed.");
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("Load key (9) pressed.");
            LoadGame();
        }
    }

    public void SaveGame()
    {
        if (playerArmature == null || timeController == null || sunLight == null || moonLight == null || inventorySystem == null ||
            suraInventoryScript == null)
        {
            Debug.LogError("Una sau mai multe referinte is null.");
            return;
        }

        // cream lista cu sloturile salvate
        List<SavedInventorySlot> savedSlots = new List<SavedInventorySlot>();
        for (int i = 0; i < inventorySystem.inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySystem.inventorySlots[i];
            SavedInventorySlot savedSlot = new SavedInventorySlot();

            if (slot.plantData != null)
            {
                savedSlot.itemType = "plant";
                savedSlot.itemName = slot.plantData.plantName;
                savedSlot.itemCount = slot.itemCount;
                Debug.Log($"Saving slot {i}: plant '{savedSlot.itemName}' with count {savedSlot.itemCount}");
            }
            else if (slot.toolData != null)
            {
                savedSlot.itemType = "tool";
                savedSlot.itemName = slot.toolData.toolName;
                savedSlot.itemCount = slot.itemCount;
                Debug.Log($"Saving slot {i}: tool '{savedSlot.itemName}' with count {savedSlot.itemCount}");
            }
            else
            {
                savedSlot.itemType = "none";
                savedSlot.itemName = "";
                savedSlot.itemCount = 0;
                Debug.Log($"Saving slot {i}: empty");
            }
            savedSlots.Add(savedSlot);
        }

        // cream lista cu sloturile salvate
        List<SavedSuraSlots> savedSuraSlots = new List<SavedSuraSlots>();
        for (int i = 0; i < suraInventoryScript.suraSlots.Length; i++)
        {
            SuraInventoryScript.SuraSlot slot = suraInventoryScript.suraSlots[i];
            SavedSuraSlots savedSuraSlot = new SavedSuraSlots();

            if (slot.plantData != null)
            {
                savedSuraSlot.itemType = "plant";
                savedSuraSlot.itemName = slot.plantData.plantName;
                savedSuraSlot.itemCount = slot.count;
                Debug.Log($"Saving slot {i}: plant '{savedSuraSlot.itemName}' with count {savedSuraSlot.itemCount}");
            }
            else
            {
                savedSuraSlot.itemType = "none";
                savedSuraSlot.itemName = "";
                savedSuraSlot.itemCount = 0;
                Debug.Log($"Saving slot {i}: empty");
            }
            savedSuraSlots.Add(savedSuraSlot);
        }


        // Creem un obiect de tip gameState cu pozitia si rotatia lui PlayerArmature
        GameState gameState = new GameState
        {
            playerPosition = playerArmature.transform.position,
            playerRotation = playerArmature.transform.rotation,
            daysPassed = timeController.daysPassed,
            currentHour = (float)timeController.currentTime.Hour + (float)timeController.currentTime.Minute / 60.0f,
            sunPosition = sunLight.transform.position,
            sunRotation = sunLight.transform.rotation,
            moonPosition = moonLight.transform.position,
            moonRotation = moonLight.transform.rotation,
            inventorySlots = savedSlots,
            currentSlot = inventorySystem.currentSlot,
            suraInventorySlots = savedSuraSlots
        };

        string json = JsonUtility.ToJson(gameState, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Joc Salvat! Pozitia Jucatorului este: " + gameState.playerPosition + ", Zile trecute: " + gameState.daysPassed +
            ", Ora: " + gameState.currentHour + ", Player Inventory slots saved: " + gameState.inventorySlots.Count + ", Sura Slots: " + 
            gameState.suraInventorySlots.Count);
    }

    public void LoadGame()
    {
        if (playerArmature == null || timeController == null || sunLight == null || moonLight == null || inventorySystem == null
            || suraInventoryScript == null)
        {
            Debug.LogError("Cannot load: one or more references are null.");
            return;
        }

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameState gameState = JsonUtility.FromJson<GameState>(json);

            if (characterController != null)
            {
                characterController.enabled = false;
            }
            else
            {
                Debug.LogWarning("CharacterController is null during load, forcing position.");
            }

            playerArmature.transform.position = gameState.playerPosition;
            playerArmature.transform.rotation = gameState.playerRotation;
            timeController.daysPassed = gameState.daysPassed;
            timeController.currentTime = DateTime.Today.AddHours((double)gameState.currentHour);
            if (timeController.timeText != null)
            {
                timeController.timeText.text = timeController.currentTime.ToString("HH:mm");
            }
            sunLight.transform.position = gameState.sunPosition;
            sunLight.transform.rotation = gameState.sunRotation;
            moonLight.transform.position = gameState.moonPosition;
            moonLight.transform.rotation = gameState.moonRotation;

            // restauram inventarul jucatorului
            for (int i = 0; i < inventorySystem.inventorySlots.Count && i < gameState.inventorySlots.Count; i++)
            {
                InventorySlot slot = inventorySystem.inventorySlots[i];
                SavedInventorySlot savedSlot = gameState.inventorySlots[i];

                // golim slotul curent
                slot.ClearSlot();

                if (savedSlot.itemType == "plant")
                {
                    Debug.Log($"Attempting to load plant '{savedSlot.itemName}' into slot {i}");
                    PlantData plantData = FindPlantByName(savedSlot.itemName);
                    if (plantData != null)
                    {
                        slot.SetItem(plantData.plantTexture, savedSlot.itemCount, plantData, null);
                        Debug.Log($"Loaded plant {savedSlot.itemName} with count {savedSlot.itemCount} into slot {i}.");
                    }
                    else
                    {
                        Debug.LogWarning($"PlantData for {savedSlot.itemName} not found in availablePlants.");
                    }
                }
                else if (savedSlot.itemType == "tool")
                {
                    Debug.Log($"Attempting to load tool '{savedSlot.itemName}' into slot {i}");
                    ToolData toolData = FindToolByName(savedSlot.itemName);
                    if (toolData != null)
                    {
                        slot.SetItem(toolData.toolTexture, savedSlot.itemCount, null, toolData);
                        Debug.Log($"Loaded tool {savedSlot.itemName} with count {savedSlot.itemCount} into slot {i}.");
                    }
                    else
                    {
                        Debug.LogWarning($"ToolData for {savedSlot.itemName} not found in availableTools.");
                    }
                }
                else
                {
                    Debug.Log($"Slot {i} is empty as expected.");
                }
            }

            // restauram slotul curent
            inventorySystem.currentSlot = gameState.currentSlot;
            inventorySystem.SelectSlot(gameState.currentSlot);

            if (characterController != null)
            {
                characterController.enabled = true;
            }

            // restauram inventarul surii 
            for(int i = 0; i < suraInventoryScript.suraSlots.Length && i < gameState.suraInventorySlots.Count; i++)
            {
                SuraInventoryScript.SuraSlot slot = suraInventoryScript.suraSlots[i];
                SavedSuraSlots savedSuraSlot = gameState.suraInventorySlots[i];

                // golim sloturile inainte
                slot.ClearSlot();

                if(savedSuraSlot.itemType == "plant")
                {
                    PlantData plantData = FindPlantByName(savedSuraSlot.itemName);
                    if(plantData != null)
                    {
                        slot.SetPlant(plantData, savedSuraSlot.itemCount);
                    }
                }
                else
                {
                    Debug.Log($"Sloturile inventarului surii {i} sunt goale.");
                }
            }

            Debug.Log("Game Loaded! PlayerPosition: " + gameState.playerPosition + ", Current Position: " + playerArmature.transform.position +
                      ", Zile trecute: " + timeController.daysPassed + ", Ora: " + timeController.currentTime.ToString("HH:mm") +
                      ", Soare Position: " + sunLight.transform.position + ", Luna Position: " + moonLight.transform.position +
                      ", inventar jucator: " + gameState.inventorySlots.Count + ", inventarul surii dupa load: " + gameState.suraInventorySlots.Count);
        }
        else
        {
            Debug.LogWarning("No save file found..");
        }
    }

    private PlantData FindPlantByName(string plantName)
    {
        Debug.Log($"Searching for PlantData: {plantName} in {availablePlants.Count} available plants");
        foreach (var plant in availablePlants)
        {
            Debug.Log($"Checking PlantData: {plant.plantName}");
            if (plant.plantName == plantName)
            {
                Debug.Log($"Found PlantData: {plantName}");
                return plant;
            }
        }
        Debug.LogWarning($"PlantData for {plantName} not found in availablePlants.");
        return null;
    }

    private ToolData FindToolByName(string toolName)
    {
        Debug.Log($"Searching for ToolData: {toolName} in {availableTools.Count} available tools");
        foreach (var tool in availableTools)
        {
            Debug.Log($"Checking ToolData: {tool.toolName}");
            if (tool.toolName == toolName)
            {
                Debug.Log($"Found ToolData: {toolName}");
                return tool;
            }
        }
        Debug.LogWarning($"ToolData for {toolName} not found in availableTools.");
        return null;
    }
}