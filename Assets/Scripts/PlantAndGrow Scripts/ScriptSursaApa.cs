using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSursaApa : MonoBehaviour
{
    private bool isPlayerNeaby = false;

    [SerializeField]
    private GameObject uiTextMeshPro;

    private void Update()
    {
           if(isPlayerNeaby && Input.GetKeyDown(KeyCode.F))
        {
            StBucket equippedBucket = FindObjectOfType<StBucket>();
            if(equippedBucket != null)
            {
                equippedBucket.UmplereGaleata();
                uiTextMeshPro.SetActive(true);
                Debug.Log("Galeata a fost umpluta cu apa.");
            }
            else
            {
                Debug.Log("Nu exista nicio galeata goala echipata...");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNeaby = true;
            Debug.Log("Apasa pe tasta F pentru a umple galeata..");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerNeaby = false;
        uiTextMeshPro.SetActive(false);
    }
}
