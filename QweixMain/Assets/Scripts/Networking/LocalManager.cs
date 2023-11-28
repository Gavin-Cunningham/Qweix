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

    // Timer variable for testing purposes
    private float testTimer;

    // CardCores for testing purposes
    public CardCore[] testingCardCores = new CardCore[QwiexHand.HandSize];
    public bool useTestingCardCores;

    // Decks for testing purposes
    private QwiexDeck ironCreekGangTestDeck;
    private QwiexDeck necroMastersTestDeck;

    // Player for testing purposes
    private QwiexPlayer testPlayer;

    // Which deck to use for testing
    public CardTribe testTribe;

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

        ironCreekGangTestDeck = new QwiexDeck();
        necroMastersTestDeck = new QwiexDeck();

        for (int i = 1; i < 15; i++)
        {
            CardCore cardCore = cardCoreLibrary.GetCardCore(i);

            // Only grab cards that have an icon
            if (cardCore.cardPicture != null)
            {
                if (cardCore.cardTribe == CardTribe.IronCreekGang)
                {
                    ironCreekGangTestDeck.AddCard(cardCore);
                }
                else if (cardCore.cardTribe == CardTribe.NecroMasters)
                {
                    necroMastersTestDeck.AddCard(cardCore);
                }
            }
        }

        testPlayer = new QwiexPlayer();

        if(testTribe == CardTribe.IronCreekGang)
        {
            testPlayer.playerDeck = ironCreekGangTestDeck;
        }
        else if(testTribe == CardTribe.NecroMasters)
        {
            testPlayer.playerDeck = necroMastersTestDeck;
        }
        else
        {
            Debug.Log("No test deck found for " + testTribe.ToString());
        }

        testPlayer.playerDeck.ShuffleDeck();
        testPlayer.playerHand = new QwiexHand();

        if(useTestingCardCores)
        {
            // Create a new deck of the test cards
            QwiexDeck testCardDeck = new QwiexDeck();

            // Add test cards to hand UI
            foreach (CardCore cardCore in testingCardCores)
            {
                if (cardCore == null)
                {
                    Debug.Log("Testing CardCore missing");
                }
                else
                {
                    testCardDeck.AddCard(cardCore);
                    handUIController.AddCard(cardCore.cardID, cardCore.cardPicture, cardCore.qwiexCost);
                }
            }

            testPlayer.playerDeck = testCardDeck;
        }
        else
        {
            // Draw cards from deck
            for (int i = 0; i < QwiexHand.HandSize; i++)
            {
                CardCore cardCore = testPlayer.playerDeck.DrawCard();

                handUIController.AddCard(cardCore.cardID, cardCore.cardPicture, cardCore.qwiexCost);

                testPlayer.playerHand.AddCard(cardCore);
            }
        }

        /*
        // Add generic cards to the hand UI
        for (int i=1; i<HandUIController.numberOfCardSlots+1; i++)
        {
            handUIController.AddCard(i, cardCoreLibrary.GetCardCore(i).cardPicture, cardCoreLibrary.GetCardCore(i).qwiexCost);
        }
        */

        handUIController.SetNextCard(testPlayer.playerDeck.NextCard().cardPicture);

        testPlayer.Qwiex = 0f;

        testTimer = 120f;
    }

    // Update is called once per frame
    void Update()
    {
        if(testPlayer.Qwiex <= QwiexBarUIController.numberOfQuiexBars)
        {
            testPlayer.Qwiex += Time.deltaTime * 0.5f;
        }

        qwiexBarUIController.SetQwiexLevel(testPlayer.Qwiex);
        handUIController.UpdateCardAvailability(testPlayer.Qwiex);

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
        GameObject prefabToSpawn = cardCoreLibrary.GetCardCore(cardID).prefabToSpawn;

        if(prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, worldDropLocation, Quaternion.identity);
        }
        else
        {
            Debug.Log("No prefab found in CardCoreLibrary for " + cardCoreLibrary.GetCardCore(cardID).cardName);
        }

        // Spend the Qwiex
        testPlayer.Qwiex -= cardCoreLibrary.GetCardCore(cardID).qwiexCost;

        // Remove the card from hand
        testPlayer.playerHand.RemoveCard(cardID);
        testPlayer.playerDeck.AddCard(cardCoreLibrary.GetCardCore(cardID));
        handUIController.RemoveCardFromHand(cardID);

        // Draw a new card
        CardCore drawnCard = testPlayer.playerDeck.DrawCard();
        testPlayer.playerHand.AddCard(drawnCard);
        handUIController.AddCard(drawnCard.cardID, drawnCard.cardPicture, drawnCard.qwiexCost);

        // Set the next card image
        handUIController.SetNextCard(testPlayer.playerDeck.NextCard().cardPicture);
    }
}
