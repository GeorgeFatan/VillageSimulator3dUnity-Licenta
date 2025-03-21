using UnityEngine;

public class StCubes : MonoBehaviour
{
    public enum StareCuburi { Empty, Planted, ReadyToPlant }
    public StareCuburi stareCurenta = StareCuburi.ReadyToPlant;

    void Start()
    {
        // Cand pornim jocu sa cuburile sa fie in starea Ready
        stareCurenta = StareCuburi.ReadyToPlant;
        Debug.Log($"Cubul {gameObject.name} este pregătit pentru plantare.");
    }

    public void ResetStare()
    {
        stareCurenta = StareCuburi.ReadyToPlant;
        Debug.Log($"Cubul {gameObject.name} este acum pregătit pentru plantare.");
    }

    public bool CanPlant()
    {
        return stareCurenta == StareCuburi.ReadyToPlant;
    }

    public void Planted()
    {
        stareCurenta = StareCuburi.Planted;
        Debug.Log($"Cubul {gameObject.name} este plantat.");
    }
}