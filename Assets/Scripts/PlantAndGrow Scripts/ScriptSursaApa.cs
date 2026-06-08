using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSursaApa : MonoBehaviour
{
    private bool isPlayerNeaby = false;

    [SerializeField]
    private GameObject uiTextMeshPro;

    [SerializeField]
    private ToolData fullGaleataApa;

    private void Update()
    {
           if(isPlayerNeaby && Input.GetKeyDown(KeyCode.F))
        {
            StBucket equippedBucket = FindObjectOfType<StBucket>();

            if(equippedBucket != null)
            {
                // salvam referintele galetii echipate goale 
                Transform equipPoint = equippedBucket.transform.parent; // punctul de echipare al galetii goale
                Vector3 position = equippedBucket.transform.position; // pozitia galetii goale
                Quaternion rotation = equippedBucket.transform.rotation; // rotatia galetii goale

                // distrugem galeata goala
                Destroy(equippedBucket.gameObject);

                // instantiem o galeata noua plina cu apa in locul celei goale
                GameObject fullGaleata = Instantiate(fullGaleataApa.toolPrefab, position, rotation);

                // atasam la punctul de echipare al galetii goale
                fullGaleata.transform.SetParent(equipPoint);
                fullGaleata.transform.localPosition = new Vector3(0.062f, -0.081f, 0.004f); // pozitionam la 0,0,0 fata de punctul de echipare
                fullGaleata.transform.localRotation = Quaternion.Euler(1.33f, -34.61f, 0f); // rotatie default fata de punctul de echipareeee

                fullGaleata.transform.localScale = new Vector3(0.09f, 0.09f, 0.09f); // ajustam scara pentru a se potrivi cu mana


                // scriptul st bucket
                StBucket newBucketScript = fullGaleata.AddComponent<StBucket>();
                newBucketScript.isFull = true; // setam starea galetii ca fiind plina


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
