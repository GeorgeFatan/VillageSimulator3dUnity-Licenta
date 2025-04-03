using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepScript : MonoBehaviour
{
    public TimeController timeController; // referinta la tController
    private bool isPlayerInTrigger = false; // verificare daca suntem in trigger

    // trebuie sa apasam pe tasta F ca sa dormim

    private void Update()
    {
        if(isPlayerInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            if(timeController != null)
            {
                timeController.SkipLaZiuaUrmatoare();
            }
            else
            {
                Debug.LogWarning("EROAREEE");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            Debug.Log("Player-ul este la usa, si este pregatit sa termina ziua de munca!");
        }
        
    }

     private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            Debug.Log("Player-ul a iesit din trigger-ul de sleep");
        }
        
    }


}
