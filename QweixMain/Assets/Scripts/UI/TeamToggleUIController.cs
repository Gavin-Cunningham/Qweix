/****************************************************************************
*
*  File              : TeamToggleUIController.cs
*  Date Created      : 11/19/2023 
*  Description       : Controller for Team Toggle button
*
*  Programmer(s)     : Gabe Burch
*  Last Modification : 
*  Additional Notes  : 
*  External Documentation URL : 
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UIElements;

//public class TeamToggleUIController : MonoBehaviour
//{

//    // Reference to local manager
//    public LocalManager localManager;

//    // References to various UI elements
//    private VisualElement uiRoot;
//    private Button teamToggleButton;

//    // Start is called before the first frame update
//    void Start()
//    {
//        if (localManager == null)
//        {
//            Debug.Log("LocalManager reference not set");
//        }

//        // Set reference to the root UI element
//        uiRoot = GetComponent<UIDocument>().rootVisualElement;

//        VisualElement teamTogglePanel = uiRoot.Q<VisualElement>("TeamTogglePanel");
        
//        teamToggleButton = new Button { name = "TeamToggleButton" };
//        teamToggleButton.AddToClassList("teamToggleButton");
//        teamToggleButton.text = "Team " + localManager.currentTeam.ToString();
//        teamToggleButton.RegisterCallback<ClickEvent>(TeamToggleButtonClick);
//        teamTogglePanel.Add(teamToggleButton);
//    }

//    private void TeamToggleButtonClick(ClickEvent evt)
//    {
//        if(localManager.currentTeam == 1)
//        {
//            localManager.currentTeam = 2;
//        }
//        else
//        {
//            localManager.currentTeam = 1;
//        }

//        teamToggleButton.text = "Team " + localManager.currentTeam.ToString();
//    }
//}
