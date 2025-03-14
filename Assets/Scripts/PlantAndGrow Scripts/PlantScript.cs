using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public GameObject[] plantStages; // Array cu prefabs pentru cele 3 stagii ale plantei (CornSt1, CornSt2, CornSt3)
    public List<GameObject> soilCubes; // Lista cuburilor reprezentând terenul arabil

    private List<GameObject> plantedPlants = new List<GameObject>(); // Lista plantelor curente
    private int currentStage = 0; // Stagiul curent al plantelor
    private int currentIndex = 0; // Index pentru următorul cub

    [SerializeField]
    private TimeController timeController; // Referință la TimeController

    private int lastDaysPassed = 0; // Ultima valoare a zilelor trecute

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Plantează pe următorul cub
        {
            PlantOnNextCube();
        }

        if (timeController != null && timeController.daysPassed > lastDaysPassed) // Verifică dacă a trecut o zi
        {
            lastDaysPassed = timeController.daysPassed;
            AdvanceGrowthStage(); // Treci la următorul stagiu de creștere
        }
    }

    void PlantOnNextCube()
    {
        if (currentIndex < soilCubes.Count)
        {
            GameObject currentCube = soilCubes[currentIndex];
            Vector3 position = currentCube.transform.position;

            position.y += currentCube.GetComponent<Renderer>().bounds.size.y / 2;

            GameObject plant = Instantiate(plantStages[0], position, Quaternion.identity);
            plantedPlants.Add(plant);

            BoxCollider boxCollider = plant.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            plant.AddComponent<PickUpScript>();

            currentIndex++;
        }
        else
        {
            Debug.Log("Toate cuburile au fost plantate!");
        }
    }

    void AdvanceGrowthStage()
    {
        if (currentStage >= plantStages.Length - 1)
        {
            Debug.Log("Plantele sunt deja în stadiul final!");
            return;
        }

        currentStage++;

        for (int i = 0; i < plantedPlants.Count; i++)
        {
            GameObject currentPlant = plantedPlants[i];
            Vector3 position = currentPlant.transform.position;
            Quaternion rotation = currentPlant.transform.rotation;

            Destroy(currentPlant);

            GameObject newPlant = Instantiate(plantStages[currentStage], position, rotation);
            plantedPlants[i] = newPlant;

            BoxCollider boxCollider = newPlant.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            newPlant.AddComponent<PickUpScript>();
        }

        Debug.Log($"Plantele au avansat la stagiul {currentStage + 1}!");
    }
}
