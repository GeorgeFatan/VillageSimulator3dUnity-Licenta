using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RutaAutomataUdareTeren : MonoBehaviour
{
    public NavMeshAgent navMeshAgent; // Ref la componenta NavMeshAgent
    public Animator playerAnimator; // Ref la Animator
    public List<Transform> fieldPositions; // Lista cu cuburile de pe teren

    private int currentCubeIndex = 0; // Indexul cubului curent
    private bool isMoving = false; // Indicator pentru a verifica dacă player-ul se mișcă


    private void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.U) && !isMoving)
        {
            isMoving = true; 
            MoveToNextCube();
        }

     
        if (isMoving && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            TriggerWateringAnim();
        }
    }

    void MoveToNextCube()
    {
        if (currentCubeIndex < fieldPositions.Count)
        {
            Transform cube = fieldPositions[currentCubeIndex];
            navMeshAgent.SetDestination(cube.position); 

            Debug.Log($"Player-ul se deplaseaza catre cubul {currentCubeIndex + 1} la pozitia {cube.position}");
        }
        else
        {
            isMoving = false;
            Debug.Log("Player-ul a udat toate plantele. :)");
        }
    }

    void TriggerWateringAnim()
    {
        playerAnimator.SetTrigger("Watering"); 
        Debug.Log($"Player-ul uda planta de pe cubul {currentCubeIndex + 1}");

        currentCubeIndex++; // Trecem la urm cub
        Invoke("MoveToNextCube", 2f); // Asteptam pentru a finaliza anim inainte de a trece la urmatorul cub
    }
}