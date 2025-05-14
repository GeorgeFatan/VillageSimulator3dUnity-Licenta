using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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
        // initializam lista cu cuburi
        foreach (Transform child in transform)
        {
            if (child.CompareTag("SoilCube") && !soilCubes.Contains(child.gameObject)) 
            {
                soilCubes.Add(child.gameObject); // practic pentru fiecare teren cu plant script, avem cuburile copil ale sale.
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
                /* PlantOnCube();
                 inventorySystem.RemoveItemFromSlot(selectedPlantData.seedTexture);*/
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

    // Corutina = multitasking = permite suspendarea si reluarea executiei.
    IEnumerator WaitForAnimation()
    {
        float animationTime = playerAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationTime); // asteptam sa se termina animatia care este in desfasurare
        PlantOnCube(); // Plasam prefabu dupa terminarea animatiei
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

        // Verif daca exista un tool de tip Harlet echipat
        EquippedHarlet equippedSpade = equipHarletPoint.childCount > 0 ? equipHarletPoint.GetChild(0).GetComponent<EquippedHarlet>() : null;

        if (equippedSpade == null || equippedSpade.toolData.toolName != "Spade")
        {
            Debug.LogWarning("Nu ai echipat un Harlet pentru a putea sapa cuburile!");
            return;
        }

        // cel mai apropiat cub valid sub jucător
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
                if (soilCube != null && soilCube.stareCurenta == StCubes.StareCuburi.Planted)
                {
                    closestCube = currentCube;
                    minDistance = distance;
                }
            }
        }
        
        // efectuam saparea daca am gasit un cub valid 
        if (closestCube != null)
        {
            StCubes soilCube = closestCube.GetComponent<StCubes>();

            // Distrugem buruienile existente 
            if (soilCube.currentWeebOnCube != null)
            {
                Destroy(soilCube.currentWeebOnCube);
                soilCube.currentWeebOnCube = null;
            }

            // Reset la starea de ReadyToPlant
            soilCube.ResetStare();
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

            selectedPlantData.isWatered = false;
            soilCube.Planted();
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
        if (selectedPlantData == null)
        {
            Debug.LogWarning("Nu a fost selectata nicio planta.");
            return;
        }

        if (currentStage >= selectedPlantData.growthStages.Length - 1)
        {
            Debug.Log("Plantele sunt deja in stadiul final!");
            return;
        }

        if (currentStage == 0 && !selectedPlantData.isWatered)
        {
            Debug.LogWarning($"Planta {selectedPlantData.plantName} nu este udata.");
            return;
        }

        currentStage++;
        Debug.Log($"Planta {selectedPlantData.plantName} avanseaza la stadiul {currentStage}.");

        List<GameObject> newPlantedPlants = new List<GameObject>();

        for (int i = 0; i < plantedPlants.Count; i++)
        {
            GameObject currentPlant = plantedPlants[i];

            if (currentPlant != null)
            {
                Vector3 position = currentPlant.transform.position;
                Quaternion rotation = currentPlant.transform.rotation;

                PickUpScript pickUpScript = currentPlant.GetComponent<PickUpScript>();
                Animator playerAnimatorRef = pickUpScript != null ? pickUpScript.playerAnimator : null;
                StCubes associatedCubeRef = pickUpScript != null ? pickUpScript.associatedCube : null;

                Destroy(currentPlant);

                GameObject newPlant = Instantiate(selectedPlantData.growthStages[currentStage], position, rotation);
                newPlantedPlants.Add(newPlant);

                if (associatedCubeRef != null)
                {
                    newPlant.transform.SetParent(associatedCubeRef.transform);
                }

                BoxCollider boxCollider = newPlant.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;

                PickUpScript newPickUpScript = newPlant.AddComponent<PickUpScript>();
                newPickUpScript.playerAnimator = playerAnimatorRef;
                newPickUpScript.associatedCube = associatedCubeRef;
            }
        }

        plantedPlants = newPlantedPlants;
    }

    public bool IsReadyToHarvest(GameObject plant)
    {
        StCubes associatedCube = plant.GetComponent<PickUpScript>()?.associatedCube;

        bool isCorrectCube = associatedCube != null && associatedCube.transform.parent == transform;
        bool isFinalStage = selectedPlantData != null && currentStage == selectedPlantData.growthStages.Length - 1;

        return isCorrectCube && isFinalStage;
    }

    void WaterPlants()
    {
        Transform equipPoint = GameObject.Find("EquipPoint")?.transform;
        if(equipPoint == null)
        {
            Debug.LogError("EquipPoint nu a fost gasit in iarahie");
            return;
        }

        // Căutăm scriptul StBucket pe găleata echipată
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

        if (selectedPlantData != null && !selectedPlantData.isWatered)
        {
            selectedPlantData.isWatered = true; // Udăm planta
            equippedBucket.GolireGaleata(); // Golim găleata după udare
            Debug.Log($"Planta {selectedPlantData.plantName} a fost udata!");
        }
        else
        {
            Debug.Log("Planta este deja udata sau nu exista.");
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
