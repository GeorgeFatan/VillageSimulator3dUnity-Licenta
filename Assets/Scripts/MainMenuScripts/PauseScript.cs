using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                EscPause();
            }
            else
            {
                EscResume();
            }
        }
    }

    void EscPause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        StarterAssetsInputs playerInputs = FindObjectOfType<StarterAssetsInputs>();
        if(playerInputs != null)
        {
            playerInputs.cursorInputForLook = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }

    public void EscResume()
    {

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;

        StarterAssetsInputs playerInputs = FindObjectOfType<StarterAssetsInputs>();
        if (playerInputs != null)
        {
            playerInputs.cursorInputForLook = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);

    }


    public void SaveJocFromPauseMenu()
    {
        SaveSystem saveSystem = FindAnyObjectByType<SaveSystem>();
        if(saveSystem != null)
        {
            saveSystem.SaveGame(); 
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }



}
