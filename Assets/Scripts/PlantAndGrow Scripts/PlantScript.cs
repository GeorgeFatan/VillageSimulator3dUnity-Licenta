using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public GameObject subPlanePrefab; // Prefabul pentru subdiviziuni
    public GameObject originalPlane;  // Plane-ul inițial de teren arat
    public int gridSize = 2; // Dimensiunea grilei de subdiviziuni (2 pentru două jumătăți)
    private int currentStep = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlantHalfPlane();
        }
    }

    void PlantHalfPlane()
    {
        Renderer renderer = originalPlane.GetComponent<Renderer>();
        Vector3 size = renderer.bounds.size;

        float halfPlaneWidth = size.x / 2; // Împarte plane-ul în două jumătăți
        float planeHeight = size.z;        // Înălțimea plane-ului rămâne aceeași

        // Determină jumătatea curentă care trebuie plantată
        if (currentStep == 0)
        {
            PlantSubPlanes(new Vector3(originalPlane.transform.position.x - size.x / 2, originalPlane.transform.position.y + 0.025f, originalPlane.transform.position.z), halfPlaneWidth, planeHeight);
            currentStep = 1;
        }
        else if (currentStep == 1)
        {
            PlantSubPlanes(new Vector3(originalPlane.transform.position.x, originalPlane.transform.position.y + 0.025f, originalPlane.transform.position.z), halfPlaneWidth, planeHeight);
            currentStep = 2;
        }
    }

    void PlantSubPlanes(Vector3 startPosition, float width, float height)
    {
        float subPlaneWidth = width / gridSize;
        float subPlaneHeight = height / gridSize;

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(startPosition.x + x * subPlaneWidth, startPosition.y, startPosition.z + z * subPlaneHeight);
                GameObject subPlane = Instantiate(subPlanePrefab, position, Quaternion.identity, originalPlane.transform);
                subPlane.transform.localScale = new Vector3(subPlaneWidth, 1, subPlaneHeight);
            }
        }
    }
}
