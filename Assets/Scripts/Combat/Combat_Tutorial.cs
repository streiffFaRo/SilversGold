using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Tutorial : MonoBehaviour
{
    //Verantwortlich dem Spieler das Spiel anhand von Infoboxen zu erklären
    
    //TODO Tutorial noch mehr aufsplitten für bessere Verträglichkeit und keine Überlastung
    
    [Header("General")]
    public GameObject[] tutorialBoxes1;
    public GameObject[] tutorialBoxes2;
    public GameObject blur;
    public bool combatScene; //Im Inspektor gesetzt

    //Private Variablen
    private int currentBox;

    private void Start()
    {
        if (GameManager.instance.currentLevel == 1)
        {
            currentBox = 0;
            StartCoroutine(FireBaseTutorial());
        }
    }
    

    private IEnumerator FireBaseTutorial() //Startet das Tutorial
    {
        if (!combatScene)
        {
            yield return new WaitForSeconds(1f);
            tutorialBoxes1[0].SetActive(true);
            blur.SetActive(true);
        }
        else if (combatScene && !GameManager.instance.tutorialDone)
        {
            yield return new WaitForSeconds(4.5f);
            tutorialBoxes1[0].SetActive(true);
            blur.SetActive(true);
        }
    }

    public void HandleBoxClosure()
    {
        tutorialBoxes1[currentBox].SetActive(false);
        VolumeManager.instance.GetComponent<AudioManager>().PlayPlatzHalterTeller();
        currentBox++;
        if (currentBox >= tutorialBoxes1.Length)
        {
            blur.SetActive(false);
        }
        else
        {
            tutorialBoxes1[currentBox].SetActive(true);
        }
    }
    
    public void Tutorial2()
    {
        currentBox = 0;
        tutorialBoxes2[0].SetActive(true);
        blur.SetActive(true);
    }

    public void HandleBoxClosure2()
    {
        tutorialBoxes2[currentBox].SetActive(false);
        VolumeManager.instance.GetComponent<AudioManager>().PlayPlatzHalterTeller();
        currentBox++;
        if (currentBox >= tutorialBoxes2.Length)
        {
            blur.SetActive(false);
            if (combatScene)
            {
                GameManager.instance.tutorialDone = true;
            }
            Destroy(gameObject);
        }
        else
        {
            tutorialBoxes2[currentBox].SetActive(true);
        }
    }
}
