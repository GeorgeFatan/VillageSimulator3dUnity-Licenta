using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

public class PlantScript : MonoBehaviour
{
    public List<GameObject> soilCubes;
    private List<GameObject> plantedPlants = new List<GameObject>();
    private int currentStage = 0;

    [SerializeField] private TimeController timeController;
    private int lastDaysPassed = 0;

    public Animator playerAnimator;
    private bool isAnimating = false;
    public bool isPlayerInPlantingZone = false;

    private PlantData selectedPlantData;
    private InventorySystem inventorySystem;

    [SerializeField] private Transform playerTransform;


    [SerializeField] public ParticleSystem waterParticlesEffect;

    [SerializeField]
    private ToolData emptyGaleataToolData;


    private void Start()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
        if (inventorySystem == null)
        {
            Debug.LogError("Eroare! Inventory System nu a fost gasit in scena actuala...");
        }
        
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
                Debug.Log("PlayerTransform a fost gasit automat.");
            }
            else
            {
                Debug.LogError("Player-ul nu a fost gasit! Asigura-te ca are tag-ul 'Player'.");
            }
        }
        soilCubes.Clear();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("SoilCube") && !soilCubes.Contains(child.gameObject))
            {
                soilCubes.Add(child.gameObject);
            }
        }

        // init plantedPlants cu plantele existente
        plantedPlants.Clear();
        foreach (GameObject cube in soilCubes)
        {
            StCubes soilCube = cube.GetComponent<StCubes>();
            if (soilCube != null && soilCube.currentPlantInstance != null)
            {
                plantedPlants.Add(soilCube.currentPlantInstance);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isAnimating)
        {
            if (isPlayerInPlantingZone)
            {
                selectedPlantData = SeedSelector.selectedPlant;
                if (selectedPlantData == null)
                {
                    Debug.LogWarning("Nu a fost selectata nicio planta!");
                    return;
                }

                if (!inventorySystem.HasItemInSlot(selectedPlantData.seedTexture, 1))
                {
                    Debug.LogWarning($"Nu mai sunt seminte de {selectedPlantData.plantName} in inventar!");
                    return;
                }

                isAnimating = true;
                PlayPlantingAnimation();
                StartCoroutine(WaitForAnimation());
            }
            else
            {
                Debug.LogWarning("Nu te afli intr-o zona de plantare.");
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            WaterPlants();
            PlayWateringAnimation();

            
        }

        if (timeController != null && timeController.daysPassed > lastDaysPassed)
        {
            lastDaysPassed = timeController.daysPassed;
            AdvanceGrowthStage();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SapaCubes();
            PlayDiggingAnimation();
        }
    }

    IEnumerator WaitForAnimation()
    {
        float animationTime = playerAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationTime);
        PlantOnCube();
        inventorySystem.RemoveItemFromSlot(selectedPlantData.seedTexture);
        isAnimating = false;
    }

    void PlayPlantingAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Plant");
            Debug.Log("Animatia de plantare a fost declansata.");
        }
        else
        {
            Debug.LogWarning("Animatorul nu este setat!");
        }
    }

    void PlayWateringAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Watering");
             if(waterParticlesEffect != null)
            {
                waterParticlesEffect.Play();
            }
            Debug.Log("Animatia de udare a plantelor a fost declansata.");
        }
        else
        {
            Debug.LogWarning("Animatorul nu este setat!");
        }
    }

    void PlayDiggingAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Digging");
            Debug.Log("Animatia de sapat a fost declansata.");
        }
        else
        {
            Debug.LogWarning("Animatorul nu este setat!");
        }
    }

    void SapaCubes()
    {
        Transform equipHarletPoint = GameObject.Find("EquipHarletPoint")?.transform;
        if (equipHarletPoint == null)
        {
            Debug.LogError("EquipHarletPoint nu a fost gasit in ierarhie!");
            return;
        }

        EquippedHarlet equippedSpade = equipHarletPoint.childCount > 0 ? equipHarletPoint.GetChild(0).GetComponent<EquippedHarlet>() : null;

        if (equippedSpade == null || equippedSpade.toolData.toolName != "Spade")
        {
            Debug.LogWarning("Nu ai echipat un Harlet pentru a putea sapa cuburile!");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError("Transformul player-ului nu este setat!");
            return;
        }

        Vector3 playerXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        GameObject closestCube = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject currentCube in soilCubes)
        {
            Vector3 cubeXZ = new Vector3(currentCube.transform.position.x, 0, currentCube.transform.position.z);
            float distance = Vector3.Distance(playerXZ, cubeXZ);

            if (distance < 0.6f && distance < minDistance)
            {
                StCubes soilCube = currentCube.GetComponent<StCubes>();
                if (soilCube != null && (soilCube.stareCurenta == StCubes.StareCuburi.Planted || soilCube.stareCurenta == StCubes.StareCuburi.Weeds))
                {
                    closestCube = currentCube;
                    minDistance = distance;
                }
            }
        }

        if (closestCube != null)
        {
            StCubes soilCube = closestCube.GetComponent<StCubes>();
            if (soilCube.currentWeebOnCube != null)
            {
                Destroy(soilCube.currentWeebOnCube);
                soilCube.currentWeebOnCube = null;
            }
            soilCube.ResetStare();
            plantedPlants.Remove(soilCube.currentPlantInstance);
            Debug.Log($"Cubul {soilCube.gameObject.name} a fost sapat si este acum in starea ReadyToPlant.");
        }
        else
        {
            Debug.LogWarning("Nu exista niciun cub valid sub jucator pentru sapare.");
        }
    }

    void PlantOnCube()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Transformul jucatorului nu este setat!");
            return;
        }

        Vector3 playerXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        GameObject closestCube = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject currentCube in soilCubes)
        {
            Vector3 cubeXZ = new Vector3(currentCube.transform.position.x, 0, currentCube.transform.position.z);
            float distance = Vector3.Distance(playerXZ, cubeXZ);

            if (distance < 0.6f && distance < minDistance)
            {
                StCubes soilCube = currentCube.GetComponent<StCubes>();
                if (soilCube != null && soilCube.CanPlant())
                {
                    closestCube = currentCube;
                    minDistance = distance;
                }
            }
        }

        if (closestCube != null)
        {
            StCubes soilCube = closestCube.GetComponent<StCubes>();
            Vector3 position = closestCube.transform.position;
            position.y += closestCube.GetComponent<Renderer>().bounds.size.y / 2;

            GameObject plant = Instantiate(selectedPlantData.growthStages[0], position, Quaternion.identity);
            plant.transform.SetParent(closestCube.transform);
            plantedPlants.Add(plant);

            soilCube.Planted(selectedPlantData, 0, plant);
            currentStage = 0;

            BoxCollider boxCollider = plant.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            PickUpScript pickUpScript = plant.AddComponent<PickUpScript>();
            pickUpScript.associatedCube = soilCube;

            if (playerAnimator == null)
            {
                playerAnimator = FindObjectOfType<Animator>();
            }

            if (playerAnimator != null)
            {
                pickUpScript.playerAnimator = playerAnimator;
            }

            Debug.Log($"A fost plantat un/o {selectedPlantData.plantName}.");
        }
        else
        {
            Debug.LogWarning("Nu exista niciun cub valid sub jucator pentru plantare.");
        }
    }

    void AdvanceGrowthStage()
    {
        List<GameObject> newPlantedPlants = new List<GameObject>();

        foreach (GameObject cube in soilCubes)
        {
            StCubes soilCube = cube.GetComponent<StCubes>();
            if (soilCube != null && soilCube.stareCurenta == StCubes.StareCuburi.Planted && soilCube.currentPlantData != null)
            {
                PlantData plantData = soilCube.currentPlantData;
                int stage = soilCube.currentStage;

                // Verificăm dacă planta poate avansa
                if (stage >= plantData.growthStages.Length - 1)
                {
                    Debug.Log($"Planta {plantData.plantName} pe cubul {cube.name} este deja in stadiul final!");
                    newPlantedPlants.Add(soilCube.currentPlantInstance);
                    continue;
                }

                if (stage == 0 && !soilCube.isWatered)
                {
                    Debug.LogWarning($"Planta {plantData.plantName} pe cubul {cube.name} nu este udata.");
                    newPlantedPlants.Add(soilCube.currentPlantInstance);
                    continue;
                }

                // Avansăm stadiul
                stage++;
                Debug.Log($"Planta {plantData.plantName} pe cubul {cube.name} avanseaza la stadiul {stage}.");

                // Distrugem planta veche
                Vector3 position = soilCube.currentPlantInstance.transform.position;
                Quaternion rotation = soilCube.currentPlantInstance.transform.rotation;

                PickUpScript pickUpScript = soilCube.currentPlantInstance.GetComponent<PickUpScript>();
                Animator playerAnimatorRef = pickUpScript != null ? pickUpScript.playerAnimator : null;
                StCubes associatedCubeRef = pickUpScript != null ? pickUpScript.associatedCube : null;

                Destroy(soilCube.currentPlantInstance);

                // Instanțiem planta nouă în stadiul avansat
                GameObject newPlant = Instantiate(plantData.growthStages[stage], position, rotation);
                newPlant.transform.SetParent(soilCube.transform);
                newPlantedPlants.Add(newPlant);

                // Actualizăm StCubes
                soilCube.currentStage = stage;
                soilCube.currentPlantInstance = newPlant;
                soilCube.isWatered = false; // Resetăm starea udării după avansare

                // Adăugăm componentele necesare
                BoxCollider boxCollider = newPlant.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;

                PickUpScript newPickUpScript = newPlant.AddComponent<PickUpScript>();
                newPickUpScript.playerAnimator = playerAnimatorRef;
                newPickUpScript.associatedCube = associatedCubeRef;
            }
            else if (soilCube != null && soilCube.currentPlantInstance != null)
            {
                newPlantedPlants.Add(soilCube.currentPlantInstance);
            }
        }

        plantedPlants = newPlantedPlants;
    }

    public bool IsReadyToHarvest(GameObject plant)
    {
        StCubes associatedCube = plant.GetComponent<PickUpScript>()?.associatedCube;

        if (associatedCube == null || associatedCube.transform.parent != transform)
        {
            return false;
        }

        return associatedCube.currentStage == associatedCube.currentPlantData.growthStages.Length - 1;
    }

    void WaterPlants()
    {
        Transform equipPoint = GameObject.Find("EquipPoint")?.transform;
        if (equipPoint == null)
        {
            Debug.LogError("EquipPoint nu a fost gasit in ierarhie");
            return;
        }

        StBucket equippedBucket = equipPoint.childCount > 0 ? equipPoint.GetChild(0).GetComponent<StBucket>() : null;

        if (equippedBucket == null)
        {
            Debug.LogWarning("Nu ai o galeata echipata pentru a uda plantele..!!!");
            return;
        }

        if (!equippedBucket.isFull)
        {
            Debug.Log("Galeata este goala.. umple galeata inainte de a uda plantele...");
            return;
        }

        bool wateredAny = false;
        foreach (GameObject cube in soilCubes)
        {
            StCubes soilCube = cube.GetComponent<StCubes>();
            if (soilCube != null && soilCube.currentPlantData != null && !soilCube.isWatered)
            {
                soilCube.WaterPlant();
                wateredAny = true;
            }
        }

        if (wateredAny)
        {
            // golim galeata logic
            equippedBucket.GolireGaleata();
            Debug.Log($"Plantele {selectedPlantData?.plantName} au fost udate!");

            // inlocuim modelul plin cu modelul gol

            // salvam poz si rotatia galetii pline
            Transform bucketEquipPoint = equippedBucket.transform.parent; // punctul de echipare al galetii pline
            Vector3 position = equippedBucket.transform.position; // pozitia galetii pline
            Quaternion rotation = equippedBucket.transform.rotation; // rotatia galetii pline
            Vector3 scale = equippedBucket.transform.localScale; // scara galetii pline

            // distrugem galeata plina
            Destroy(equippedBucket.gameObject);

            // instantiem o galeata noua goala in locul celei pline
            GameObject emptyGaleata = Instantiate(emptyGaleataToolData.toolPrefab, position, rotation);

            // atasam la eq point
            emptyGaleata.transform.SetParent(bucketEquipPoint);
            emptyGaleata.transform.localPosition = new Vector3(0.062f, -0.081f, 0.004f); // pozitionam la 0,0,0 fata de punctul de echipare
            emptyGaleata.transform.localRotation = Quaternion.Euler(1.33f, -34.61f, 0f); // rotatie default fata de punctul de echipare
            emptyGaleata.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            inventorySystem.equippedTool = emptyGaleata;
        }
        else
        {
            Debug.Log("Toate plantele selectate sunt deja udate sau nu exista.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPlantingZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPlantingZone = false;
        }
    }
}