using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;


public class MainMenuLoadScript : MonoBehaviour
{
    private GameState gameStateLoadMeniu;
    private string savePath; 


    private void Awake()
    {
        savePath = Application.persistentDataPath + "/saveGame.json";
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    public void IncarcaJocMainMenu()
    { 
        if (File.Exists(savePath))
        { 
            string json = File.ReadAllText(savePath);
            gameStateLoadMeniu = JsonUtility.FromJson<GameState>(json);
            GameManager.Instance.SetLoadedGameState(gameStateLoadMeniu);

            SceneManager.LoadScene("ScenaLaptop");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 1)
        {
            StartCoroutine(ApplyAfterOneFrame());
        }
        
    }

    private IEnumerator ApplyAfterOneFrame()
    {
        yield return null; // asteptam 1 frame

        SaveSystem saveSystem = FindObjectOfType<SaveSystem>();

        if (saveSystem != null)
        {
            // Citește JSON-ul din fișier ACUM, nu cel vechi
            string json = File.ReadAllText(savePath);
            GameState freshState = JsonUtility.FromJson<GameState>(json);

            saveSystem.ApplyGameState(freshState);

            Debug.Log("Game state applied fresh from JSON.");
        }
        else
        {
            Debug.LogError("SaveSystem nu a fost găsit în scena principală!"); 
        }
    }




}
