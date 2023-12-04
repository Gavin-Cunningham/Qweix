/****************************************************************************
*
*  File              : QwiexBarUIController.cs
*  Date Created      : 11/18/2023 
*  Description       : Controller for Qwiex Bar UI
*
*  Programmer(s)     : Gabe Burch
*  Last Modification : 
*  Additional Notes  : 
*  External Documentation URL : 
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QwiexBarUIController : MonoBehaviour
{
    // Number of Qwiex bars to display
    public const int numberOfQuiexBars = 10;

    // Reference to local manager
    public LocalManager localManager;

    // References to various UI elements
    private VisualElement uiRoot;
    private Label qwiexTotalLabel;

    // List of the full Qwiex bars
    private List<VisualElement> qwiexFullBarList;


    private void Awake()
    {
        if (localManager == null)
        {
            Debug.Log("LocalManager reference not set");
        }

        // Set reference to the root UI element
        uiRoot = GetComponent<UIDocument>().rootVisualElement;

        qwiexFullBarList = new List<VisualElement>();

        AddUIChildren();

        SetQwiexLevel(0f);
    }

    // Adds Qwiex Bar UI elements
    private void AddUIChildren()
    {
        // Find the QwiexPanel
        VisualElement qwiexPanel = uiRoot.Q<VisualElement>("QwiexPanel");

        // Create QwiexMeter UI element and add it to the QwiexPanel
        VisualElement qwiexMeter = new VisualElement() { name = "QwiexMeter" };
        qwiexMeter.AddToClassList("qwiexMeter");
        qwiexPanel.Add(qwiexMeter);

        // For each of the available Qwiex levels
        for(int i=0;i<numberOfQuiexBars;i++)
        {
            // Create an empty Qwiex bar and add it to the QwiexMeter
            VisualElement qwiexEmptyBar = new VisualElement() { name = "QwiexEmptyBar" };
            qwiexMeter.Add(qwiexEmptyBar);

            // Classify the Qwiex Bar as Left, Inner, or Right styled
            if (i == 0)
            {
                qwiexEmptyBar.AddToClassList("qwiexEmptyBarLeftEnd");
            }
            else if (i < numberOfQuiexBars - 1)
            {
                qwiexEmptyBar.AddToClassList("qwiexEmptyBarInner");
            }
            else if (i == numberOfQuiexBars - 1)
            {
                qwiexEmptyBar.AddToClassList("qwiexEmptyBarRightEnd");
            }

            // Create a full QwiexBar and add it to the empty bar and to the list of full bars
            VisualElement qwiexFullBar = new VisualElement() { name = "QwiexFullBar" };
            qwiexFullBar.AddToClassList("qwiexFullBar");
            qwiexEmptyBar.Add(qwiexFullBar);
            qwiexFullBarList.Add(qwiexFullBar);
        }

        // Create the QwiexTotalOrb and add it to the QwiexPanel
        VisualElement qwiexTotalOrb = new VisualElement() { name = "QwiexTotalOrb" };
        qwiexTotalOrb.AddToClassList("qwiexTotalOrb");
        qwiexPanel.Add(qwiexTotalOrb);

        // Create the QwiexTotalLabel and add it to the QwiexTotalOrb
        qwiexTotalLabel = new Label() { name = "QwiexTotalLabel", text = "10" };
        qwiexTotalLabel.AddToClassList("qwiexTotalLabel");
        qwiexTotalOrb.Add(qwiexTotalLabel);
    }

    // Called by local manager to set the current Qwiex level
    public void SetQwiexLevel(float qwiexLevel)
    {
        // Iterate through the QwiexFullBars
        foreach(VisualElement qwiexFullBar in qwiexFullBarList)
        {
            // If the index is less than Qwiex level, set the width to full
            if(qwiexFullBarList.IndexOf(qwiexFullBar) < qwiexLevel - 1)
            {
                qwiexFullBar.style.width = Length.Percent(100);
            }
            // If the index is over the Qwiex level by less than one, set the width to the percentage
            else if(qwiexFullBarList.IndexOf(qwiexFullBar) < qwiexLevel)
            {
                qwiexFullBar.style.width = Length.Percent((qwiexLevel % 1f) * 100);
            }
            // Otherwise set the width to zero
            else
            {
                qwiexFullBar.style.width = 0f;
            }
        }

        qwiexTotalLabel.text = Mathf.Floor(qwiexLevel).ToString();
    }
}
