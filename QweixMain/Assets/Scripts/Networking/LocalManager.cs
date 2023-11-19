/****************************************************************************
*
*  File              : LocalManager.cs
*  Date Created      : 11/08/2023 
*  Description       : Intermediary between local UI and network server
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

public class LocalManager : MonoBehaviour
{
    // Reference to CardCore library
    public CardCoreLibrary cardCoreLibrary;

    // References to various UI controllers
    public HandUIController handUIController;
    public QwiexBarUIController qwiexBarUIController;
    public TimerUIController timerUIController;
    public EmoteUIController emoteUIController;

    // Team variable for testing purposes
    public int currentTeam = 1;

    // Qwiex level variable for testing purposes
    private float testQwiexLevel;

    // Timer variable for testing purposes
    private float testTimer;

    // Card IDs for testing purposes
    public CardCore[] testingCardCores = new CardCore[HandUIController.numberOfCardSlots];


    void Start()
    {
        if (cardCoreLibrary == null)
        {
            Debug.Log("CardCoreLibrary reference not set");
        }

        if (handUIController == null)
        {
            Debug.Log("HandUIController reference not set");
        }

        if (qwiexBarUIController == null)
        {
            Debug.Log("QwiexBarUIController reference not set");
        }

        if (timerUIController == null)
        {
            Debug.Log("TimerUIController reference not set");
        }

        // Add test cards to hand UI
        foreach (CardCore cardCore in testingCardCores)
        {
            if (cardCore == null)
            {
                Debug.Log("Testing CardCore missing");
            }
            else
            {
                handUIController.AddCard(cardCore.cardID, cardCore.cardPicture, cardCore.qwiexCost);
            }
        }

        /*
        // Add generic cards to the hand UI
        for (int i=1; i<HandUIController.numberOfCardSlots+1; i++)
        {
            handUIController.AddCard(i, cardCoreLibrary.GetCardCore(i).cardPicture, cardCoreLibrary.GetCardCore(i).qwiexCost);
        }
        */

        handUIController.SetNextCard(cardCoreLibrary.GetCardCore(HandUIController.numberOfCardSlots + 1).cardPicture);

        testQwiexLevel = 0f;

        testTimer = 120f;
    }

    // Update is called once per frame
    void Update()
    {
        qwiexBarUIController.SetQwiexLevel(testQwiexLevel);

        if(testQwiexLevel <= QwiexBarUIController.numberOfQuiexBars)
        {
            testQwiexLevel += Time.deltaTime * 0.5f;
        }

        testTimer -= Time.deltaTime;

        timerUIController.SetTimer(testTimer);
    }

    // Called by the Hand UI controller after a drag-and-drop of a card
    public void PlayCard(int cardID, Vector2 mouseUpLocation)
    {
        // Raycast from the drag-and-drop location through a zero plane
        Ray ray = Camera.main.ScreenPointToRay(mouseUpLocation);
        Plane zeroPlane = new Plane(Vector3.forward, Vector3.zero);
        float distance;
        zeroPlane.Raycast(ray, out distance);

        // Convert the screen drag-and-drop location to world coordinates
        Vector2 dropLocation = ray.GetPoint(distance);
        Vector3 worldDropLocation = new Vector3(dropLocation.x, dropLocation.y, 0.25f);

        // Instantiate the prefab
        Instantiate(cardCoreLibrary.GetCardCore(cardID).prefabToSpawn, worldDropLocation, Quaternion.identity);
    }
}
