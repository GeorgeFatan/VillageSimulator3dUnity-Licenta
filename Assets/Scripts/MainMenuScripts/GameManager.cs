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
    public bool HasSavedGame()
    {
        return File.Exists(savePath);
    }


    public void IncarcaJoc()
    {
        if (File.Exists(savePath))
        {
            // incarca save file-ul 
            string json = File.ReadAllText(savePath);
            gameState = JsonUtility.FromJson<GameState>(json);

            // incarca scena principala
            SceneManager.LoadScene("ScenaLaptop");
        }
    }
    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetLoadedGameState(GameState loadedState)
    {
        gameState = loadedState;
    }

    private IEnumerator LoadGameDataAfterSceneLoad()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 1);

        // Aici apelezi toate funcțiile de încărcare din SaveSystem
        SaveSystem saveSystem = FindObjectOfType<SaveSystem>();
        if (saveSystem != null)
        {
            saveSystem.LoadGame(); // Încarcă datele efective
        }
        else
        {
            Debug.LogError("SaveSystem nu a fost găsit în scena principală!");
        }
    }

}
