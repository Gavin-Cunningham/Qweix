/****************************************************************************
*
*  File              : Card.cs      
*  Date Created      : 2/16/23   
*  Description       : This script is designed to hold the attributes of each card. 
*
*  Programmer(s)     : Robert Mimms
*  Last Modification : 3/21/23
*  Additional Notes  : 
*  Flow Chart URL :
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class Card : MonoBehaviour
{
    public PlaceByCard placeByCard;
    public GameObject cardObject;
    public SpriteRenderer cardRenderer;
    public string cardName;
    public int qwiexCost;
    public bool inHand;
    
    // Index referring to the place in the deck the prefab occupies.
    public int cardIndex;
    private void Start()
    {
        placeByCard = GameObject.Find("GridManager").GetComponent<PlaceByCard>();
    }

    // This is called by the PlaceByCard script when the player clicks on the card in order for the script to access the various attributes of the card and spawn correctly.
    public void OnSpawn()
    {
        placeByCard.spawningCard = this.gameObject;
    }
}