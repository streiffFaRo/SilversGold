using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Tutorial : MonoBehaviour
{
    public GameObject[] tutorialBoxes;
    public GameObject blur;
    public bool combatScene;

    private int currentBox;

    private void Start()
    {
        if (GameManager.instance.currentLevel == 1)
        {
            currentBox = 0;
            StartCoroutine(FireBaseTutorial());
        }
    }

    private IEnumerator FireBaseTutorial()
    {
        if (combatScene)
        {
            yield return new WaitForSeconds(4.5f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
        tutorialBoxes[0].SetActive(true);
        blur.SetActive(true);
    }

    public void HandleBoxClosure()
    {
        tutorialBoxes[currentBox].SetActive(false);
        VolumeManager.instance.GetComponent<AudioManager>().PlayPlatzHalterTeller();
        currentBox++;
        if (currentBox >= tutorialBoxes.Length)
        {
            blur.SetActive(false);
        }
        else
        {
            tutorialBoxes[currentBox].SetActive(true);
        }
    }
}
