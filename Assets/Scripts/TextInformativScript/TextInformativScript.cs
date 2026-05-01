using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextInformativScript : MonoBehaviour
{
    
    public GameObject tutorialPanel;

    private void Start()
    {
        tutorialPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //verificam daca jucatorul intra sau iese din zona colider-ului
        {
            tutorialPanel.SetActive(true);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            tutorialPanel.SetActive(false);
        }
    }

    public void HideTextInformativLaLoad()
    {
        tutorialPanel.SetActive(false); // la load, ascudem textul informativ
        // asa rezolvam bugu cand dam save si dupa mergem la un obiect care are tutorial, apare textu
        // pe urma ne facem treaba, si daca dam load apare textu informativ desi nu suntem in triggeru de tutorial
    }

}
