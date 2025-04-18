using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StBucket : MonoBehaviour
{
    public bool isFull = false;
    public int assignedSlot;
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
}
