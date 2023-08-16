/****************************************************************************
*
*  File              : CardCoreLibrary.cs
*  Date Created      : 06/26/2023 
*  Description       : Singleton class for holding all CardCore classes
*  
*                      Call GetCardCore(int id) to get the CardCore class associated
*                      with that id number
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

public class CardCoreLibrary : MonoBehaviour
{
    private List<CardCore> cardCoreList;

    private static CardCoreLibrary instance;

    private void Awake()
    {
        // Singleton check
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        foreach(CardCore cardCore in GetComponentsInChildren<CardCore>())
        {
            cardCoreList.Add(cardCore);
        }
    }

    public CardCore GetCardCore(int id)
    {
        CardCore returnCardCore = null;

        foreach(CardCore cardCore in cardCoreList)
        {
            if(cardCore.cardID == id)
            {
                returnCardCore = cardCore;
            }
        }

        if(returnCardCore == null)
        {
            Debug.Log("CardCoreLibrary.GetCardCore() could not find card with id " + id);
        }

        return returnCardCore;
    }
}
