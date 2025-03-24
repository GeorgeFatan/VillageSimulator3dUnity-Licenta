using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public GameObject[] plantStages; // Stagiile plantei
    public List<GameObject> soilCubes; // Lista cuburilor

    private List<GameObject> plantedPlants = new List<GameObject>(); // Plantele curente
    private int currentStage = 0; // Stagiul curent al creșterii

    [SerializeField]
    private TimeController timeController; // Referință la timpul zilelor

    private int lastDaysPassed = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Plantare
        {
            PlantOnCube();
        }

        if (timeController != null && timeController.daysPassed > lastDaysPassed) // Creștere
        {
            lastDaysPassed = timeController.daysPassed;
            AdvanceGrowthStage();
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

                GameObject plant = Instantiate(plantStages[0], position, Quaternion.identity);
                plantedPlants.Add(plant); 

                soilCube.Planted(); 
                currentStage = 0; // resetam stagiu de crestere dupa ce plantam

                
                BoxCollider boxCollider = plant.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true; 
                boxCollider.size = new Vector3(1.3f, 2.9f, 1.3f); 
                plant.AddComponent<PickUpScript>();

                Debug.Log("Planta a fost replantată!");
                return;
            }
        }

        Debug.Log("Niciun cub nu este pregătit pentru plantare!");
    }


    void AdvanceGrowthStage()
    {
        if (currentStage >= plantStages.Length - 1)
        {
            Debug.Log("Plantele sunt deja în stadiul final!");
            return;
        }

        currentStage++;
        Debug.Log($"Avansăm la stagiul {currentStage}. Total plante: {plantedPlants.Count}");

        // Creăm o listă temporară pentru actualizarea plantelor
        List<GameObject> newPlantedPlants = new List<GameObject>();

        for (int i = 0; i < plantedPlants.Count; i++)
        {
            GameObject currentPlant = plantedPlants[i];

            if (currentPlant != null)
            {
                Vector3 position = currentPlant.transform.position;
                Quaternion rotation = currentPlant.transform.rotation;

                Destroy(currentPlant); // Distruge planta curentă

                GameObject newPlant = Instantiate(plantStages[currentStage], position, rotation); // Creează noua plantă
                newPlantedPlants.Add(newPlant); // Adaugă planta în lista temporară

                BoxCollider boxCollider = newPlant.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                newPlant.AddComponent<PickUpScript>();

                Debug.Log($"Planta din poziția {i} a avansat la stagiul {currentStage}.");
            }
            else
            {
                Debug.LogWarning($"Planta de pe poziția {i} lipsește sau a fost deja distrusă!");
            }
        }

        // Înlocuim lista originală cu lista actualizată
        plantedPlants = newPlantedPlants;
    }
}