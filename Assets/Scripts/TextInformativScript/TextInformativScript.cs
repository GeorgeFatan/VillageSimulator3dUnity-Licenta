using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextInformativScript : MonoBehaviour
{
    public string mesajTutorial;
    public TextMeshProUGUI mesajTutorialUI;
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
            mesajTutorialUI.text = mesajTutorial; 
           
        }
    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            tutorialPanel.SetActive(false);
        }
    }
}
