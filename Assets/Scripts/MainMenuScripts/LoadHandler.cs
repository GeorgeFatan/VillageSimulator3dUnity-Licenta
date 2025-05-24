using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadHandler : MonoBehaviour
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

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        LoadSavedSceneFromMenu();
    }

    void LoadSavedSceneFromMenu()
    {
        GameState gameState = GameManager.Instance.GetGameState();
        if(gameState == null )
        {
            Debug.LogWarning("Nu a fost gasit niciun stare a jocului pentru a fi incarcata");
            return;
        }

        // Starea jucatorului
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

        StartCoroutine(ReenableCharacterController());

        // money system save
        SellScript.instantaBuyTerrain.playerMoney = gameState.playerMoney;
        SellScript.instantaBuyTerrain.UpdateMoneyDisplay();

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
