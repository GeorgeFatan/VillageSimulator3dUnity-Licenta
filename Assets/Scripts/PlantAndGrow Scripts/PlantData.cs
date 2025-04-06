// PlantData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlantData", menuName = "Farming/PlantData")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public GameObject[] growthStages; // cele 3 stadii ale plantei
}
