using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public List<GameObject> soilCubes; // lista cuburilor disponibile pentru plantare
    private List<GameObject> plantedPlants = new List<GameObject>(); // plante curente
    private int currentStage = 0; // stadiu curent de crestere al plantei

    [SerializeField]
    private TimeController timeController; // referinta la sistemul de timp
    private int lastDaysPassed = 0;

    public Animator playerAnimator; // referinta la Animator-ul jucatorului

    public bool isPlayerInPlantingZone = false;

    private PlantData selectedPlantData;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // plantați cu P
        {
            if (isPlayerInPlantingZone)
            {
                selectedPlantData = SeedSelector.selectedPlant;

                if (selectedPlantData == null)
                {
                    Debug.LogWarning("Nu a fost selectată nicio plantă!");
                    return;
                }

                PlayPlantingAnimation();
                PlantOnCube();
            }
            else
            {
                Debug.LogWarning("Nu te afli într-o zonă de plantare.");
            }
        }

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
            Debug.Log("Animația de plantare a fost declanșată.");
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

                GameObject plant = Instantiate(selectedPlantData.growthStages[0], position, Quaternion.identity);
                plantedPlants.Add(plant);

                soilCube.Planted();
                currentStage = 0;

                BoxCollider boxCollider = plant.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                plant.AddComponent<PickUpScript>();

                Debug.Log($"A fost plantat un/o {selectedPlantData.plantName}.");
                return;
            }
        }

        Debug.LogWarning("Nu există locuri disponibile pentru plantare.");
    }

    void AdvanceGrowthStage()
    {
        if (selectedPlantData == null)
        {
            Debug.LogWarning("Nu a fost selectată nicio plantă pentru creștere.");
            return;
        }

        if (currentStage >= selectedPlantData.growthStages.Length - 1)
        {
            Debug.Log("Plantele sunt deja în stadiul final!");
            return;
        }

        currentStage++;
        Debug.Log($"Toate plantele avansează la stadiul {currentStage}.");

        List<GameObject> newPlantedPlants = new List<GameObject>();

        for (int i = 0; i < plantedPlants.Count; i++)
        {
            GameObject currentPlant = plantedPlants[i];

            if (currentPlant != null)
            {
                Vector3 position = currentPlant.transform.position;
                Quaternion rotation = currentPlant.transform.rotation;

                Destroy(currentPlant);

                GameObject newPlant = Instantiate(selectedPlantData.growthStages[currentStage], position, rotation);
                newPlantedPlants.Add(newPlant);

                BoxCollider boxCollider = newPlant.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                newPlant.AddComponent<PickUpScript>();

                Debug.Log($"Planta de la poziția {i} a crescut la stadiul {currentStage}.");
            }
            else
            {
                Debug.LogWarning($"Planta de la poziția {i} lipsește sau a fost distrusă.");
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
            Debug.Log("Jucătorul a intrat în zona de plantare.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPlantingZone = false;
            Debug.Log("Jucătorul a ieșit din zona de plantare.");
        }
    }
}
