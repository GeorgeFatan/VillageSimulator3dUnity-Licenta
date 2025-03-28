using UnityEngine;

public class StCubes : MonoBehaviour
{
    public enum StareCuburi { Empty, Planted, ReadyToPlant}
    public StareCuburi stareCurenta = StareCuburi.ReadyToPlant;

    void Start()
    {
        stareCurenta = StareCuburi.ReadyToPlant;
        Debug.Log($"Cubul {gameObject.name} este pregatit pentru plantare.");
    }

    public void ResetStare()
    {
        stareCurenta = StareCuburi.ReadyToPlant;
        Debug.Log($"Cubul {gameObject.name} a fost resetat la starea ReadyToPlant.");
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