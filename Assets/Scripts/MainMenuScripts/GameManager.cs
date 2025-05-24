using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    private GameState gameState;
    private string savePath;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/saveGame.json";
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void LoadGame()
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            gameState = JsonUtility.FromJson<GameState>(json);

            SceneManager.LoadScene(1); // trecem la scena principala 
        }
    }
    
    public GameState GetGameState()
    {
        return gameState;
    }

}
