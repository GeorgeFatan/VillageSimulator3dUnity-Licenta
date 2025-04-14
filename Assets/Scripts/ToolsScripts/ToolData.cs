using UnityEngine;

[CreateAssetMenu(fileName = "NewToolData", menuName = "Farming/ToolData")]
public class ToolData : ScriptableObject
{
    public string toolName; 
    public Texture toolTexture; 
    public GameObject toolPrefab;
    public string toolDescription; 
}