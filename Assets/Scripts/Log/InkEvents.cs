using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InkEvents : MonoBehaviour
{
    #region Inspector

    [SerializeField] private List<InkEvent> inkEvents;
    
    #endregion

    #region Unity Event Functions

    private void OnEnable()
    {
        DialogueManager.InkEvent += TryInvokedEvent;
    }

    private void OnDisable()
    {
        DialogueManager.InkEvent -= TryInvokedEvent;
    }

    #endregion

    private void TryInvokedEvent(string eventName)
    {
        foreach (InkEvent inkEvent in inkEvents)
        {
            if (inkEvent.name == eventName)
            {
                inkEvent.onEvent.Invoke();
                return;
            }
        }
    }
}
[Serializable]
public struct InkEvent
{
    [Tooltip("Name des ink Events.")]
    public string name;
    
    public UnityEvent onEvent;
}
