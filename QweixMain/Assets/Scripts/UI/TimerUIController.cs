/****************************************************************************
*
*  File              : TimerUIController.cs
*  Date Created      : 11/18/2023 
*  Description       : Controller for Timer UI
*
*  Programmer(s)     : Gabe Burch
*  Last Modification : 
*  Additional Notes  : 
*  External Documentation URL : 
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerUIController : MonoBehaviour
{
    // Reference to local manager
    public LocalManager localManager;

    // References to various UI elements
    private VisualElement uiRoot;
    private Label timerLabel;

    // Start is called before the first frame update
    void Start()
    {
        if (localManager == null)
        {
            Debug.Log("LocalManager reference not set");
        }

        // Set reference to the root UI element
        uiRoot = GetComponent<UIDocument>().rootVisualElement;

        AddUIChildren();
    }

    private void AddUIChildren()
    {
        // Find the TimerPanel
        VisualElement timerPanel = uiRoot.Q<VisualElement>("TimerPanel");

        // Create TimerBackground UI element and add it to the TimerPanel
        VisualElement timerBackground = new VisualElement() { name = "TimerBackground" };
        timerBackground.AddToClassList("timerBackground");
        timerPanel.Add(timerBackground);

        // Create TimerLabel UI element and add it to the TimerBackground
        timerLabel = new Label() { name = "TimerBackground", text = "2:00" };
        timerLabel.AddToClassList("timerLabel");
        timerBackground.Add(timerLabel);
    }

    // Called by the local manager to set the displayed time remaining
    public void SetTimer(float inputSeconds)
    {
        int displayMinutes = Mathf.FloorToInt(inputSeconds) / 60;
        int displaySeconds = Mathf.FloorToInt(inputSeconds) % 60;

        string timerOutput = displayMinutes.ToString() + ":";

        if(displaySeconds < 10)
        {
            timerOutput += "0";
        }

        timerOutput += displaySeconds.ToString();

        timerLabel.text = timerOutput;
    }
}
