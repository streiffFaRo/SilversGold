using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Tutorial : MonoBehaviour
{
    public GameObject[] tutorialBoxes;
    public GameObject blur;

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
        yield return new WaitForSeconds(4.5f);
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
