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


            SceneManager.LoadScene("ScenaLaptop");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1) // 1 = scena laptop
        {
            SaveSystem saveSystem = FindObjectOfType<SaveSystem>();
            if (saveSystem != null)
            {
                saveSystem.ApplyGameState(gameStateLoadMeniu);
                Debug.Log("Ai incarcat jocul din main menu cu succes");
            }
        }
    }




}
