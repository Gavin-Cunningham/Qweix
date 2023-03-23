/****************************************************************************
*
*  File              : Card.cs      
*  Date Created      : 3/21/23   
*  Description       : This script is designed to hold all the logic dedicated to the player's hand of cards. 
*
*  Programmer(s)     : Robert Mimms
*  Last Modification : 3/21/23
*  Additional Notes  : 
*  Flow Chart URL :
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using QFSW.QC;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Hand : MonoBehaviour
{
    // using a const to define the starting hand size, just in case we want to change the player's hand size somehow one day
    private const int StartingHandSize = 4;
    public int handSize;

    // The player's deck is copied into a list for the game, so to leave it alone and unchanged.
    // The inPlayDeck is mutable to allow for cards to go to the bottom
    public Player_Deck PlayerDeck;
    public List<GameObject> inPlayDeck;
    
    // The hand represents clones of the cards in the players hand
    public List<GameObject> currentHand;
    
    // An array of the spawn points so cards go to the right spot when drawn.
    public Transform[] cardSpawnPoints;

    // Keeps track of when cards are drawn
    public int deckIndex;

    // Start sets the decklist, hand size, shuffles the deck and draws the starting hand of 4 cards
    void Start()
    {
        inPlayDeck = PlayerDeck.masterDeck.ToList();
        handSize = StartingHandSize;
        Shuffle();
        DrawStartingHand();
    }

    // Goes through the deck list and swaps each object with a random object in the list.
    void Shuffle()
    {
        for (int i = 0; i < inPlayDeck.Count; i++)
        {
            int rnd = Random.Range(0, inPlayDeck.Count);
            (inPlayDeck[rnd], inPlayDeck[i]) = (inPlayDeck[i], inPlayDeck[rnd]);
        }
        getCardIndex();
    }

    // Goes through the decklist and ensures the card is correctly indexed when cards are played and added to the bottom.
    void getCardIndex()
    {
        
        for (int i = 0; i < inPlayDeck.Count; i++)
        {
            inPlayDeck[i].GetComponent<Card>().cardIndex = i;
        }
    }
    
    // Draws a card for each spawn point
    void DrawStartingHand()
    {
        for (int i = 0; i < handSize; i++)
        {
            DrawCard(cardSpawnPoints[i], i);
        }
    }

    // Gets passed in the card that was played, finds the card, inserts it at the bottom of the list and removes the old one
    // Then draws a new card in the appropriate spot, destroys the cloned card, and reindexes the cards in the deck. 
    public void RemoveFromHand(GameObject discarded)
    {
        for (int i = 0; i < handSize; i++)
        {
            if (currentHand[i] == discarded)
            {
                Transform cardSlot = cardSpawnPoints[i];
               
                inPlayDeck.Insert(8, inPlayDeck[discarded.GetComponent<Card>().cardIndex]);
                inPlayDeck.Remove(inPlayDeck[discarded.GetComponent<Card>().cardIndex]);
                //currentHand[i] = null;
                DrawCard(cardSlot, i);
                Destroy(discarded);
                getCardIndex();

            }
        }
    }


    // Instantiates the next card in the deck into the appropriate card slot, then sets the hand array slot to the new card.
    // Then it tells the card it's in the hand, increments deckIndex, and checks to see if deckIndex needs to be reset.
    public void DrawCard(Transform cardSlot, int arraySlot)
    {
        Debug.Log("DrawCards was called");

        Debug.Log("deckIndex: " + deckIndex);
        Debug.Log(inPlayDeck[deckIndex].name);
        Debug.Log(cardSlot.transform);
        var drawnCard = Instantiate(inPlayDeck[deckIndex], cardSlot.transform);
        currentHand[arraySlot] = drawnCard;
        drawnCard.GetComponent<Card>().inHand = true;
        deckIndex++;

        if (deckIndex == 8)
        {
            deckIndex = 0;
        }
    }
}