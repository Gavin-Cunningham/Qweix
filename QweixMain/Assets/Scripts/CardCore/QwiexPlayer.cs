/****************************************************************************
*
*  File              : QwiexPlayer.cs
*  Date Created      : 11/24/2023 
*  Description       : A player object for holding Qwiex, Hand, Deck, etc
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

public class QwiexPlayer
{
    // The player's deck
    public QwiexDeck playerDeck;

    // The player's current hand
    public QwiexHand playerHand;

    // The player's current amount of Qwiex
    public float Qwiex;
}
