using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextInformativScript : MonoBehaviour
{
    public string mesajTutorial;
    public TextMeshProUGUI mesajTutorialUI;

     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //verificam daca jucatorul intra sau iese din zona colider-ului
        {
            mesajTutorialUI.text = mesajTutorial; // afisam
            mesajTutorialUI.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            mesajTutorialUI.gameObject.SetActive(false); //daca iesim din trigger, dispare mesaju de pe ecran
        }
    }
}
