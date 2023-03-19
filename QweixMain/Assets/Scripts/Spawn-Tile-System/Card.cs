/****************************************************************************
*
*  File              : Card.cs      
*  Date Created      : 2/16/23   
*  Description       : This script is designed to hold the attributes of each card. 
*
*  Programmer(s)     : Robert Mimms
*  Last Modification : 3/16/23
*  Additional Notes  : 
*  Flow Chart URL :
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Card : MonoBehaviour
{

    
    public PlaceByCard placeByCard;
    public GameObject cardObject;
    public SpriteRenderer cardRenderer;
    public string cardName;
    public int quiexCost;

  

    // This is called by the PlaceByCard script when the player clicks on the card in order for the script to access the various attributes of the card and spawn correctly.
    public void OnSpawn()
    {

        placeByCard.spawningCard = this.gameObject;
    }
}
