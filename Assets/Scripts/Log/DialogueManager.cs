using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //Verantwortlich für das Logbuch und die Benutzung
    
    private static DialogueManager instance;
    
    [Header("Ink Files")] 
    [SerializeField] private TextAsset[] allLogFiles;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI titleUI;
    [SerializeField] private TextMeshProUGUI textUI;
    
    [Header("GameObjects")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    public static event Action<string> InkEvent;
    
    //Private Variablen
    private Story currentStory;
    private Coroutine displayLineCorutine;
    private bool canContinueToNextLine = false;
    private bool breakLineFormationChain = false;
    
    public GameState gameState;
    

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one DialogueManager in the scene");
        }

        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }
    
    void Start()
    {
        
        SelectCurrentStory();
        
        GameManager.instance.SetUpForNextLevel();
        
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        BindExternalFunctions();
        
        ContinueStory();
    }

    private void OnEnable()
    {
        PlayerInputManager.onConfirmEvent += PlayerContinues;
    }

    private void OnDisable()
    {
        PlayerInputManager.onConfirmEvent -= PlayerContinues;
        
        currentStory.UnbindExternalFunction("Unity_Event");
        currentStory.UnbindExternalFunction("Get_State");
        currentStory.UnbindExternalFunction("Add_State");
    }

    public void SelectCurrentStory() //Wählt Inkfile für das entsprechende Level
    {
        currentStory = new Story(allLogFiles[GameManager.instance.currentLevel].text);
    }

    public void PlayerContinues()
    {
        if (canContinueToNextLine)
        {
            ContinueStory();
        }
        else
        {
            breakLineFormationChain = true;
        }
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCorutine != null)
            {
                StopCoroutine(displayLineCorutine);
            }
            displayLineCorutine = StartCoroutine(DisplayLine(currentStory.Continue()));
        }
        else
        {
            Debug.LogWarning("Kein Text zu präsentieren!");
        }
    }

    private IEnumerator DisplayLine(string line) //Gibt Text in bestimmter Geschwindigkeit wieder
    {
        textUI.text = line;
        continueButton.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        for (int i = 0; i < line.Length; i++)
        {
            if (breakLineFormationChain)
            {
                textUI.maxVisibleCharacters = line.Length;
                break;
            }
            textUI.maxVisibleCharacters = i+1;
            yield return new WaitForSeconds(GameManager.instance.typingSpeed);
        }
        
        DisplayChoices();
        canContinueToNextLine = true;
        breakLineFormationChain = false;
    }

    private void DisplayChoices() //Zeigt Auswahlmöglichkeiten an
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        
        if (currentChoices.Count > choices.Length)
        {
            Debug.Log("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int j = index; j < choices.Length; j++)
        {
            choices[j].gameObject.SetActive(false);
        }
        
        if (index >= 2)
        {
            continueButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
        }
    }
    
    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }
    
    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }
    
    //Ink Event zusatz

    private void BindExternalFunctions()
    {
        currentStory.BindExternalFunction<string>("Unity_Event", Unity_Event);
        currentStory.BindExternalFunction<string>("Get_State", Get_State, true);
        currentStory.BindExternalFunction<string, int>("Add_State", Add_State);

        //CustomExternalFunction
        
        /*currentStory.BindExternalFunction("poker", (string betAmount) =>
        {
            poker.BetAmount(betAmount);
        }); */
        
    }
    
    private void Unity_Event(string eventName)
    {
        InkEvent?.Invoke(eventName);
    }
    
    private object Get_State(String id)
    {
        State state = gameState.Get(id);
        return state != null ? state.amount : 0;
    }

    private void Add_State(string id, int amount)
    {
        gameState.Add(id, amount);
    }
}
