using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuraShowHideUI : MonoBehaviour
{
    public GameObject inventorySuraUI;

    private void Start()
    {
        inventorySuraUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventorySuraUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventorySuraUI.SetActive(false);
        }
    }
}
