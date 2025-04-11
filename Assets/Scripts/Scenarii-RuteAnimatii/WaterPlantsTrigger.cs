using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlantsTrigger : MonoBehaviour
{
    public Animator playerAnimator; // ref la animator

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            playerAnimator.SetTrigger("Watering");
            Debug.Log("Animatia de plantare a fost declansata...");
        } 
        
    }
}
