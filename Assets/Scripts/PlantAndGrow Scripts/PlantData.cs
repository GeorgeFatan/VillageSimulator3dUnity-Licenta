using UnityEngine;

[CreateAssetMenu(fileName = "NewPlantData", menuName = "Farming/PlantData")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public Texture plantTexture;
    public Texture seedTexture;
    public GameObject[] growthStages; // cele 3 stagii de crestere

    //semintele

    public string seedName;
    
}
