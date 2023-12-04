/****************************************************************************
*
*  File              : QwiexHand.cs
*  Date Created      : 11/21/2023 
*  Description       : A player's hand of CardCore objects
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

public class QwiexHand
{
    // How many cards are in a full Hand
    public const int HandSize = 4;

    // Cards in the player's hand
    public CardCore[] cardsInHand = new CardCore[QwiexHand.HandSize];

    // Add a card to the player's hand
    // Returns false if the hand was full
    public bool AddCard(CardCore card)
    {
        bool cardAdded = false;

        for(int i=0;i<HandSize;i++)
        {
            if (cardsInHand[i] == null && cardAdded == false)
            {
                cardsInHand[i] = card;
                cardAdded = true;
            }
        }

        return cardAdded;
    }

    // Removes a card from the player's hand
    public void RemoveCard(int cardID)
    {
        for (int i = 0; i < HandSize; i++)
        {
            if (cardsInHand[i].cardID == cardID)
            {
                cardsInHand[i] = null;
            }
        }
    }
}
