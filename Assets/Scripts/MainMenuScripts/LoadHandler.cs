using System.Collections;
using UnityEngine;

public class LoadHandler : MonoBehaviour
{
    private void Start()
    {
        // Wait one frame to ensure everything is initialized
        StartCoroutine(InitializeAfterLoad());
    }

    private IEnumerator InitializeAfterLoad()
    {
        yield return null; // Wait one frame

        GameState gameState = GameManager.Instance.GetGameState();
        if (gameState != null)
        {
            SaveSystem saveSystem = FindObjectOfType<SaveSystem>();
            if (saveSystem != null)
            {
                saveSystem.ApplyGameState(gameState);
            }
        }
    }
}