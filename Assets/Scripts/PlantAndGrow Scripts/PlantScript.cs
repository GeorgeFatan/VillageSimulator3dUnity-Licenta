using System.Collections;
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
        // Verifică apăsarea butonului P pentru a planta pe următorul cub
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlantOnNextCube();
        }

        // Verifică dacă a trecut o zi
        if (timeController != null && timeController.daysPassed > lastDaysPassed)
        {
            lastDaysPassed = timeController.daysPassed;
            AdvanceGrowthStage(); // Treci la următorul stagiu de creștere
        }
    }

    void PlantOnNextCube()
    {
        // Verifică dacă mai există cuburi disponibile pentru plantare
        if (currentIndex < soilCubes.Count)
        {
            GameObject currentCube = soilCubes[currentIndex];
            Vector3 position = currentCube.transform.position;

            // Ajustează poziția pe Y pentru a plasa planta pe fața superioară a cubului
            position.y += currentCube.GetComponent<Renderer>().bounds.size.y / 2;

            // Instanțiază planta din stadiul 1 (CornSt1)
            GameObject plant = Instantiate(plantStages[0], position, Quaternion.identity);
            plantedPlants.Add(plant);

            // Crește indexul pentru a planta pe următorul cub la următoarea apăsare
            currentIndex++;
        }
        else
        {
            Debug.Log("Toate cuburile au fost plantate!");
        }
    }

    void AdvanceGrowthStage()
    {
        // Dacă toate plantele sunt deja în stadiul final, nu face nimic
        if (currentStage >= plantStages.Length - 1)
        {
            Debug.Log("Plantele sunt deja în stadiul final!");
            return;
        }

        currentStage++; // Treci la stagiul următor

        // Înlocuiește toate plantele curente cu cele din stagiul următor
        for (int i = 0; i < plantedPlants.Count; i++)
        {
            GameObject currentPlant = plantedPlants[i];
            Vector3 position = currentPlant.transform.position;
            Quaternion rotation = currentPlant.transform.rotation;

            Destroy(currentPlant); // Șterge planta curentă

            // Creează planta din stagiul următor
            GameObject newPlant = Instantiate(plantStages[currentStage], position, rotation);
            plantedPlants[i] = newPlant; // Actualizează lista plantelor
        }

        Debug.Log($"Plantele au avansat la stagiul {currentStage + 1}!");
    }
}
