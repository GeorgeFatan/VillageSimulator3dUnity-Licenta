using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;

public class SaveSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject playerArmature; // Ref PlayerArmature
    [SerializeField]
    private GameObject npcArmatureBrian; // ref la npc armature brian
    [SerializeField]
    private GameObject npcArmatureMegan; // ref la npc armature megan
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
    [SerializeField]
    private BuyTerrainScript buyTerrainSCript; // ref la scriptu de buy terrain 
    [SerializeField]
    private TextInformativScript textInformativScript; // ref la scriptu de text informativ

    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/saveGame.json";
        Debug.Log("Jocul a fost salvat la ruta: " + savePath);

        characterController = playerArmature.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
           
            LoadGame();
        }
    }

    public void SaveGame()
    {
        if (playerArmature == null || timeController == null || sunLight == null || moonLight == null || inventorySystem == null ||
            suraInventoryScript == null || npcArmatureBrian == null || npcArmatureMegan == null)
        {
            Debug.LogError("Una sau mai multe referinte is null.");
            return;
        }

        // inventar jucator

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

        // sura slots

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
                Debug.Log($"Saving sura slot {i}: plant '{savedSuraSlot.itemName}' with count {savedSuraSlot.itemCount}");
            }
            else
            {
                savedSuraSlot.itemType = "none";
                savedSuraSlot.itemName = "";
                savedSuraSlot.itemCount = 0;
                Debug.Log($"Saving sura slot {i}: empty");
            }
            savedSuraSlots.Add(savedSuraSlot);
        }


        // cuburi terenuri 

        List<SoilCubeData> soilCubesData = new List<SoilCubeData>();
        StCubes[] allSoilCubes = FindObjectsOfType<StCubes>();
        foreach (StCubes cube in allSoilCubes)
        {
            SoilCubeData cubeData = new SoilCubeData
            {
                cubeName = cube.gameObject.name,
                currentState = cube.stareCurenta.ToString(),
                hasWeed = cube.currentWeebOnCube != null,
                plantName = cube.currentPlantData != null ? cube.currentPlantData.plantName : "",
                plantStage = cube.currentStage,
                isWatered = cube.isWatered
            };
            soilCubesData.Add(cubeData);
            Debug.Log($"Saving cube {cubeData.cubeName}: state {cubeData.currentState}, hasWeed {cubeData.hasWeed}, plant {cubeData.plantName}, stage {cubeData.plantStage}, isWatered {cubeData.isWatered}");
        }



        GameState gameState = new GameState
        {
            playerPosition = playerArmature.transform.position, // Player Armature
            playerRotation = playerArmature.transform.rotation,
            npcPositionM = npcArmatureBrian.transform.position, // Brian Armature
            npcRotationM = npcArmatureBrian.transform.rotation,
            npcPositionF = npcArmatureMegan.transform.position, // Megan armature
            npcRotationF = npcArmatureMegan.transform.rotation,
            daysPassed = timeController.daysPassed,
            currentHour = (float)timeController.currentTime.Hour + (float)timeController.currentTime.Minute / 60.0f,
            sunPosition = sunLight.transform.position,
            sunRotation = sunLight.transform.rotation,
            moonPosition = moonLight.transform.position,
            moonRotation = moonLight.transform.rotation,
            inventorySlots = savedSlots,
            currentSlot = inventorySystem.currentSlot,
            suraInventorySlots = savedSuraSlots,
            playerMoney = SellScript.instantaBuyTerrain.playerMoney,
            soilCubesData = soilCubesData,
            terrain2Unlocked = buyTerrainSCript.terrainUnlocked
        };

        string json = JsonUtility.ToJson(gameState, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Joc Salvat! Pozitia Jucatorului este: " + gameState.playerPosition + ", Zile trecute: " + gameState.daysPassed +
            ", Ora: " + gameState.currentHour + ", Player Inventory slots saved: " + gameState.inventorySlots.Count + ", Sura Slots: " +
            gameState.suraInventorySlots.Count + ", Player money saved: " + gameState.playerMoney + ", Soil cubes saved: " + gameState.soilCubesData.Count + ", pozitia lui Brian este: " 
            + gameState.npcPositionM + ", pozitia lui Megan este: " + gameState.npcPositionF);
    }



    public void ApplyGameState(GameState gameState)
    {
        if (playerArmature == null || timeController == null || sunLight == null || moonLight == null || inventorySystem == null ||
            suraInventoryScript == null)
        {
            Debug.LogError("cannot load, erroare");
            return;
        }

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // player position
        playerArmature.transform.position = gameState.playerPosition;
        playerArmature.transform.rotation = gameState.playerRotation;

        // npc Brian Position 
        npcArmatureBrian.transform.position = gameState.npcPositionM;
        npcArmatureBrian.transform.rotation = gameState.npcRotationM;

        // npc Megan position 
        npcArmatureMegan.transform.position = gameState.npcPositionF;
        npcArmatureMegan.transform.rotation = gameState.npcRotationF;

        // day night cycle 
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

        // inventar hud jucator
        for (int i = 0; i < inventorySystem.inventorySlots.Count && i < gameState.inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySystem.inventorySlots[i];
            SavedInventorySlot savedSlot = gameState.inventorySlots[i];
            slot.ClearSlot();
            if (savedSlot.itemType == "plant")
            {
                PlantData plantData = FindPlantByName(savedSlot.itemName);
                if (plantData != null)
                {
                    slot.SetItem(plantData.plantTexture, savedSlot.itemCount, plantData, null);
                }
            }
            else if (savedSlot.itemType == "tool")
            {
                ToolData toolData = FindToolByName(savedSlot.itemName);
                if (toolData != null)
                {
                    slot.SetItem(toolData.toolTexture, savedSlot.itemCount, null, toolData);
                }
            }
            else
            {
                Debug.Log($"Slot {i} este gol");
            }
        }

        inventorySystem.currentSlot = gameState.currentSlot;
        inventorySystem.SelectSlot(gameState.currentSlot);

        // sura inventar

        for (int i = 0; i < suraInventoryScript.suraSlots.Length && i < gameState.suraInventorySlots.Count; i++)
        {
            SuraInventoryScript.SuraSlot slot = suraInventoryScript.suraSlots[i];
            SavedSuraSlots savedSuraSlot = gameState.suraInventorySlots[i];
            slot.ClearSlot();
            if (savedSuraSlot.itemType == "plant")
            {
                PlantData plantData = FindPlantByName(savedSuraSlot.itemName);
                if (plantData != null)
                {
                    slot.SetPlant(plantData, savedSuraSlot.itemCount);
                }
            }
            else
            {
                Debug.Log($"Sura slot {i} golit.");
            }
        }

        // cubes si plante data

        StCubes[] allSoilCubes = FindObjectsOfType<StCubes>();
        foreach (SoilCubeData cubeData in gameState.soilCubesData)
        {
            StCubes cube = Array.Find(allSoilCubes, c => c.gameObject.name == cubeData.cubeName);
            if (cube != null)
            {
                cube.stareCurenta = (StCubes.StareCuburi)Enum.Parse(typeof(StCubes.StareCuburi), cubeData.currentState);
                if (cubeData.hasWeed && cube.weedPrefab != null)
                {
                    if (cube.currentWeebOnCube != null) Destroy(cube.currentWeebOnCube);
                    cube.currentWeebOnCube = Instantiate(cube.weedPrefab, cube.transform.position, Quaternion.identity);
                    cube.currentWeebOnCube.transform.SetParent(cube.transform);
                }
                else if (!cubeData.hasWeed && cube.currentWeebOnCube != null)
                {
                    Destroy(cube.currentWeebOnCube);
                    cube.currentWeebOnCube = null;
                }

                if (cubeData.plantName != "")
                {
                    PlantData plantData = FindPlantByName(cubeData.plantName);
                    if (plantData != null && cubeData.plantStage >= 0 && cubeData.plantStage < plantData.growthStages.Length)
                    {
                        if (cube.currentPlantInstance != null) Destroy(cube.currentPlantInstance);
                        Vector3 position = cube.transform.position;
                        position.y += cube.GetComponent<Renderer>().bounds.size.y / 2;
                        GameObject plantInstance = Instantiate(plantData.growthStages[cubeData.plantStage], position, Quaternion.identity);
                        plantInstance.transform.SetParent(cube.transform);
                        cube.Planted(plantData, cubeData.plantStage, plantInstance);
                        cube.isWatered = cubeData.isWatered;

                        BoxCollider boxCollider = plantInstance.AddComponent<BoxCollider>();
                        boxCollider.isTrigger = true;

                        PickUpScript pickUpScript = plantInstance.AddComponent<PickUpScript>();
                        pickUpScript.associatedCube = cube;
                        Animator playerAnimator = FindObjectOfType<Animator>();
                        if (playerAnimator != null)
                        {
                            pickUpScript.playerAnimator = playerAnimator;
                        }
                    }
                }
                else
                {
                    cube.currentPlantData = null;
                    cube.currentStage = 0;
                    cube.isWatered = false;
                    if (cube.currentPlantInstance != null)
                    {
                        Destroy(cube.currentPlantInstance);
                        cube.currentPlantInstance = null;
                    }
                }

                Debug.Log($"Cub loaded {cubeData.cubeName}: status: {cubeData.currentState}, hasWeed: {cubeData.hasWeed}, plant: {cubeData.plantName}, plantStage: {cubeData.plantStage}, isWatered {cubeData.isWatered}");
            }
            else
            {
                Debug.LogWarning($"Cube {cubeData.cubeName} nu a fost gasit pe parcursul load.");
            }
        }

        // terrain 2
        if(gameState.terrain2Unlocked)
        {
            buyTerrainSCript.UnlockTerrain();
        }
        else
        {
            buyTerrainSCript.LockTerrain(); 
        }

        // money system save
        SellScript.instantaBuyTerrain.playerMoney = gameState.playerMoney;
        SellScript.instantaBuyTerrain.UpdateMoneyUI();

        StartCoroutine(ReenableCharacterController());


        Debug.Log("Joc Incarcat cu succes! PlayerPosition: " + gameState.playerPosition + ", Current Position: " + playerArmature.transform.position +
                  ", Zile trecute: " + timeController.daysPassed + ", Ora: " + timeController.currentTime.ToString("HH:mm") +
                  ", Soare Position: " + sunLight.transform.position + ", inventar jucator: " + gameState.inventorySlots.Count +
                  ", inventarul surii dupa load: " + gameState.suraInventorySlots.Count + ", Player Money acum: " + gameState.playerMoney + ", Soil Cubes loaded:" +
                  gameState.soilCubesData + ", Pozitia lui NPC Brian: " + gameState.npcPositionM + ", Pozitia lui NPC Megan: "
                  + gameState.npcPositionM);

    }

    private void ResetTerrainsToDefault()
    {
        buyTerrainSCript.LockTerrain();
        buyTerrainSCript.ForceExitTrigger();
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            ResetTerrainsToDefault();
           

            string json = File.ReadAllText(savePath);
            GameState gameState = JsonUtility.FromJson<GameState>(json);
            ApplyGameState(gameState);

            textInformativScript.HideTutorial(); 


        }
    }

    private IEnumerator ReenableCharacterController()
    {
        yield return new WaitForEndOfFrame();
        if (characterController != null)
        {
            characterController.enabled = true;
        }
    }

  

    private PlantData FindPlantByName(string plantName)
    {
        foreach (var plant in availablePlants)
        {
            if (plant != null && plant.plantName == plantName)
            {
                Debug.Log($"Am gasit PlantData: {plantName}");
                return plant;
            }
        }
        Debug.LogWarning($"PlantData pentru {plantName} nu a fost gasit in availablePlants.");
        return null;
    }

    private ToolData FindToolByName(string toolName)
    {
        foreach (var tool in availableTools)
        {
            if (tool != null && tool.toolName == toolName)
            {
                Debug.Log($"Am gasit ToolData: {toolName}");
                return tool;
            }
        }
        Debug.LogWarning($"ToolData pentru {toolName} nu a fost gasit in availableTools.");
        return null;
    }
}