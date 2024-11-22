using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Tutorial : MonoBehaviour
{
    //Verantwortlich dem Spieler das Spiel anhand von Infoboxen zu erklären
    
    //TODO Tutorial noch mehr aufsplitten für bessere Verträglichkeit und keine Überlastung
    
    [Header("General")]
    public GameObject[] tutorialBoxes;
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
            tutorialBoxes[0].SetActive(true);
            blur.SetActive(true);
        }
        else if (combatScene && !GameManager.instance.tutorialDone)
        {
            yield return new WaitForSeconds(4.5f);
            tutorialBoxes[0].SetActive(true);
            blur.SetActive(true);
        }
    }

    public void HandleBoxClosure()
    {
        tutorialBoxes[currentBox].SetActive(false);
        VolumeManager.instance.GetComponent<AudioManager>().PlayPlatzHalterTeller();
        currentBox++;
        if (currentBox >= tutorialBoxes.Length)
        {
            blur.SetActive(false);
            if (combatScene)
            {
                GameManager.instance.tutorialDone = true;
            }
        }
        else
        {
            tutorialBoxes[currentBox].SetActive(true);
        }
    }
}
