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

            if (soilCube != null)
            {
                Debug.Log($"Cubul verificat: {currentCube.name} are starea: {soilCube.stareCurenta}");

                if (soilCube.CanPlant())
                {
                    Vector3 position = currentCube.transform.position;

                    // Plasează planta
                    position.y += currentCube.GetComponent<Renderer>().bounds.size.y / 2;

                    GameObject plant = Instantiate(plantStages[0], position, Quaternion.identity);
                    plantedPlants.Add(plant);

                    soilCube.Planted();
                    Debug.Log($"Planta a fost plantată pe cubul: {currentCube.name}");
                    return;
                }
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