using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public GameObject[] plantStages; // prefabs pentru fiecare stadiu al plantei
    public List<GameObject> soilCubes; // lista cuburilor disponibile pentru plantare
    private List<GameObject> plantedPlants = new List<GameObject>(); // plante curente
    private int currentStage = 0; // stadiu curent de crestere al plantei

    [SerializeField]
    private TimeController timeController; // ref la nr de zile
    private int lastDaysPassed = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // pe p plantam
        {
            PlantOnCube();
        }

        if (timeController != null && timeController.daysPassed > lastDaysPassed)
        {
            lastDaysPassed = timeController.daysPassed;
            AdvanceGrowthStage();
        }
    }

    // planting pe cuburi disp
    void PlantOnCube()
    {
        foreach (GameObject currentCube in soilCubes)
        {
            StCubes soilCube = currentCube.GetComponent<StCubes>();
            if (soilCube != null && soilCube.CanPlant())
            {
                Vector3 position = currentCube.transform.position;
                position.y += currentCube.GetComponent<Renderer>().bounds.size.y / 2;

                GameObject plant = Instantiate(plantStages[0], position, Quaternion.identity); // instantiem planta la primu stadiu de crestere
                plantedPlants.Add(plant);

                soilCube.Planted();
                currentStage = 0; // resetam stadiu dupa ce plantam 

                BoxCollider boxCollider = plant.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                plant.AddComponent<PickUpScript>(); 

                Debug.Log("A fost plantata o nouă leguma!");
                return;
            }
        }

        Debug.LogWarning("Nu exista locuri disponibile pentru plantare!");
    }

    // avansare la stadiul urmator 
    void AdvanceGrowthStage()
    {
        if (currentStage >= plantStages.Length - 1)
        {
            Debug.Log("Plantele sunt deja în stadiul final!");
            return;
        }

        currentStage++;
        Debug.Log($"Avansam la stadiul {currentStage}.");

        List<GameObject> newPlantedPlants = new List<GameObject>();

        for (int i = 0; i < plantedPlants.Count; i++)
        {
            GameObject currentPlant = plantedPlants[i];

            if (currentPlant != null)
            {
                Vector3 position = currentPlant.transform.position;
                Quaternion rotation = currentPlant.transform.rotation;

                Destroy(currentPlant);

                GameObject newPlant = Instantiate(plantStages[currentStage], position, rotation);
                newPlantedPlants.Add(newPlant);

                BoxCollider boxCollider = newPlant.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                newPlant.AddComponent<PickUpScript>();

                Debug.Log($"Planta la pozitia {i} a avansat la stadiul {currentStage}.");
            }
            else
            {
                Debug.LogWarning($"Planta de la pozitia {i} lipseste sau a fost distrusa/recoltata deja!");
            }
        }

        plantedPlants = newPlantedPlants;
    }

    public bool IsReadyToHarvest(GameObject plant)
    {
        return currentStage == plantStages.Length - 1; 
    }
}