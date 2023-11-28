/****************************************************************************
*
*  File              : QwiexDeck.cs
*  Date Created      : 11/21/2023 
*  Description       : A player's deck of CardCore objects, from which they
*                      will draw their Cards
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
using System.Linq;
using UnityEngine;

public class QwiexDeck
{
    // How many cards are in a full Deck
    public const int DeckSize = 8;

	// The deck of cards
    public List<CardCore> playingDeck;

	public QwiexDeck()
    {
		playingDeck = new List<CardCore>();
    }

	// Returns and removes the top card of the deck
	public CardCore DrawCard()
	{
		// Return and remove CardCore of the top of the deck
		CardCore cardDrawn = playingDeck[0];
		playingDeck.Remove(cardDrawn);

		return cardDrawn;
	}

	// Adds a card to the bottom of the deck
	public void AddCard(CardCore cardCore)
	{
		playingDeck.Add(cardCore);
	}

	// Shuffle the deck using Fisher-Yates method
	public void ShuffleDeck()
	{
		System.Random rng = new System.Random();

		int n = playingDeck.Count;

		while(n > 1)
        {
			int k = rng.Next(n);
			n--;
			CardCore temp = playingDeck[k];
			playingDeck[k] = playingDeck[n];
			playingDeck[n] = temp;
        }
	}

	// Returns the top card of the deck
	public CardCore NextCard()
	{
		return playingDeck[0];
	}

}
