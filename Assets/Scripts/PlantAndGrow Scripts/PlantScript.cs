using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public List<GameObject> soilCubes; // Lista cuburilor disponibile pentru plantare
    private List<GameObject> plantedPlants = new List<GameObject>(); // Plante curente
    private int currentStage = 0; // Stadiu curent de creștere al plantei

    [SerializeField]
    private TimeController timeController; // Ref la sistemul de timp
    private int lastDaysPassed = 0;

    public Animator playerAnimator; // Ref la Animator-ul player-ului

    public bool isPlayerInPlantingZone = false; // Indicator daca jucatorul se afla intr-o zona de plantare

    private PlantData selectedPlantData; // Datele plantei selectate
    private InventorySystem inventorySystem; // Ref la sistemul de inventar al jucatorului

    private void Start()
    {
        // Gasim inventarul in scena
        inventorySystem = FindObjectOfType<InventorySystem>();
        if (inventorySystem == null)
        {
            Debug.LogError("Eroare! Inventory System nu a fost gasit in scena actuala...");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Plant cu P
        {
            if (isPlayerInPlantingZone)
            {
                selectedPlantData = SeedSelector.selectedPlant;

                if (selectedPlantData == null)
                {
                    Debug.LogWarning("Nu a fost sel nicio planta!");
                    return;
                }

                // Ver daca exista suficiente seminte in inventar
                if (!inventorySystem.HasItemInSlot(selectedPlantData.seedTexture, 1))
                {
                    Debug.LogWarning($"Nu mai sunt seminte de {selectedPlantData.plantName} in inventar!");
                    return;
                }

                PlayPlantingAnimation();
                PlantOnCube();
                inventorySystem.RemoveItemFromSlot(selectedPlantData.seedTexture); // Reducem seeds
            }
            else
            {
                Debug.LogWarning("Nu te afli intr-o zona de plantare.");
            }
        }

        // avansam stadiul de crestere în functie de timp
        if (timeController != null && timeController.daysPassed > lastDaysPassed)
        {
            lastDaysPassed = timeController.daysPassed;
            AdvanceGrowthStage();
        }
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

    void PlantOnCube()
    {
        foreach (GameObject currentCube in soilCubes)
        {
            StCubes soilCube = currentCube.GetComponent<StCubes>();
            if (soilCube != null && soilCube.CanPlant())
            {
                Vector3 position = currentCube.transform.position;
                position.y += currentCube.GetComponent<Renderer>().bounds.size.y / 2;

                // Instanțiem planta
                GameObject plant = Instantiate(selectedPlantData.growthStages[0], position, Quaternion.identity);
                plantedPlants.Add(plant);

                // Setăm cubul ca plantat
                soilCube.Planted();
                currentStage = 0;

                // Adăugăm BoxCollider și PickUpScript
                BoxCollider boxCollider = plant.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;

                PickUpScript pickUpScript = plant.AddComponent<PickUpScript>();

                // Verificăm și setăm Animator-ul jucătorului
                if (playerAnimator == null)
                {
                    // Încercăm să găsim automat Animator-ul în scenă
                    playerAnimator = FindObjectOfType<Animator>(); // Găsește orice Animator disponibil
                    if (playerAnimator != null)
                    {
                        Debug.Log("PlayerAnimator a fost găsit automat și setat.");
                    }
                    else
                    {
                        Debug.LogError("PlayerAnimator nu este configurat și nu a fost găsit în scenă!");
                    }
                }

                // Setăm Animator-ul pentru PickUpScript
                if (playerAnimator != null)
                {
                    pickUpScript.playerAnimator = playerAnimator; // Asignăm referința
                    Debug.Log("Animator-ul Player a fost setat pentru plantă.");
                }
                else
                {
                    Debug.LogWarning("PickUpScript nu a primit Animator-ul Player.");
                }

                Debug.Log($"A fost plantat un/o {selectedPlantData.plantName}. Semințele din inventar au fost decrementate.");
                return;
            }
        }

        Debug.LogWarning("Nu există locuri disponibile pentru plantare.");
    }

    void AdvanceGrowthStage()
    {
        if (selectedPlantData == null)
        {
            Debug.LogWarning("Nu a fost selectata nicio planta pentru avansare a stadiului de crestere.");
            return;
        }

        if (currentStage >= selectedPlantData.growthStages.Length - 1)
        {
            Debug.Log("Plantele sunt deja in stadiul final!");
            return;
        }

        currentStage++;
        Debug.Log($"Toate plantele avanseaza la stadiul {currentStage}.");

        List<GameObject> newPlantedPlants = new List<GameObject>();

        for (int i = 0; i < plantedPlants.Count; i++)
        {
            GameObject currentPlant = plantedPlants[i];

            if (currentPlant != null)
            {
                Vector3 position = currentPlant.transform.position;
                Quaternion rotation = currentPlant.transform.rotation;
                
                // Trebuie sa salvam PlayerAnimator ca sa poata fi transferat catre toate stagiile de crestere
                PickUpScript PickUpScript1 = currentPlant.GetComponent<PickUpScript>();
                Animator playerAnimatorRef = PickUpScript1 != null ? PickUpScript1.playerAnimator : null;

                // Distrugerea stagiului curent
                Destroy(currentPlant);

                // Instantiem stadiu urmator 
                GameObject newPlant = Instantiate(selectedPlantData.growthStages[currentStage], position, rotation);
                newPlantedPlants.Add(newPlant);

                // Box colider pt stagii
                BoxCollider boxCollider = newPlant.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;

                PickUpScript newPickUpScript = newPlant.AddComponent<PickUpScript>();

                if(playerAnimatorRef != null)
                {
                    newPickUpScript.playerAnimator = playerAnimatorRef;
                    Debug.Log("PlayerAnimator a fost adaugat catre plantele din stadiu urmator....");
                }
                else
                {
                    Debug.LogWarning("PlayerAnimator nu a fost gasit in stadiu urmator...");
                }

                Debug.Log($"Planta de la pozitia {i} a crescut la stadiul {currentStage}.");
            }
            else
            {
                Debug.LogWarning($"Planta de la pozitia {i} lipseste sau a fost distrusa.");
            }
        }

        plantedPlants = newPlantedPlants;
    }

    public bool IsReadyToHarvest(GameObject plant)
    {
        return selectedPlantData != null && currentStage == selectedPlantData.growthStages.Length - 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPlantingZone = true;
            Debug.Log("Jucatorul a intrat in zona de plantare a terenului.... XXXX.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPlantingZone = false;
            Debug.Log("Jucatorul a iesit din zona de plantare a terenului ..... xxxx.");
        }
    }
}