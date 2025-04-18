using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StBucket : MonoBehaviour
{
    public bool isFull = false;
    public int assignedSlot = -1; // inseamna ca default nu este asociat niciun slot din inventar

    public void UmplereGaleata()
    { 
        isFull = true;
        Debug.Log("Galeata este plina..");
    }

    public void GolireGaleata()
    {
        isFull=false;
        Debug.Log("Galeata a fost golita..");
    }

    public void ResetareAssignedSlot()
    {
        assignedSlot = -1;
        Debug.Log("Slot-ul pe care era galeata a fost resetat atunci cand am pus-o pe jos... -1");
    }
}
