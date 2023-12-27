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
using Unity.Netcode;

public class QwiexPlayer : NetworkBehaviour
{
    // The player's deck
    public QwiexDeck playerDeck;

    // The player's current hand
    public QwiexHand playerHand;

    // The player's current amount of Qwiex
    public NetworkVariable <float> Qwiex = new NetworkVariable<float>(0);
    public NetworkVariable <int> teamNum = new NetworkVariable<int>(0);



    private void Start()
    {
        if (IsServer)
        {
            teamNum.Value = (int)OwnerClientId + 1;
            Debug.Log("clientID is " + ((int)OwnerClientId + 1));
        }


        if(!IsServer)
        {
            LocalManager.instance.player1Camera.SetActive(false);

            LocalManager.instance.player2Camera.SetActive(true);
            if(LocalManager.instance.player2Camera.activeSelf == true)
            {
                Debug.Log("player2 has connected");
            }
            
        }

        LocalManager.instance.PlayerRegister(this);

        if ((int)OwnerClientId == 0)
        {
            playerDeck = LocalManager.instance.ironCreekGangTestDeck;
        }
        else if ((int)OwnerClientId == 1)
        {
            playerDeck = LocalManager.instance.necroMastersTestDeck;
        }
        else
        {
            Debug.Log("No test deck found for " + OwnerClientId.ToString());
        }

        if (IsOwner)
        {
            playerDeck.ShuffleDeck();
            playerHand = new QwiexHand();

            for (int i = 0; i < QwiexHand.HandSize; i++)
            {
                CardCore cardCore = playerDeck.DrawCard();

                LocalManager.instance.handUIController.AddCard(cardCore.cardID, cardCore.cardPicture, cardCore.dragSprite, cardCore.dragSpriteScale, cardCore.qwiexCost);

                playerHand.AddCard(cardCore);
            }

            LocalManager.instance.handUIController.SetNextCard(this.playerDeck.NextCard().cardPicture);
        }
    }

}
