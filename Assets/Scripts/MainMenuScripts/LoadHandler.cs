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
        SaveSystem saveSystem = null;
        while (saveSystem == null)
        {
            saveSystem = FindObjectOfType<SaveSystem>();
            yield return null; // 1 frame wait
        }

        // mai asteptam 1 frame sa ne asiguram ca toate obietele sunt initializate
        yield return null;

        GameState gameState = GameManager.Instance.GetGameState();  // preluam starea curenta a jocului din GameManager

        if (gameState != null)
        {
            saveSystem.ApplyGameState(gameState);
        }
    }
}