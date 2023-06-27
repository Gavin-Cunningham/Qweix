/****************************************************************************
*
*  File              : CardCore.cs
*  Date Created      : 06/26/2023 
*  Description       : Storage class for all elements of a particular card
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

public class CardCore : MonoBehaviour
{
    [SerializeField]
    public int cardID;

    [SerializeField]
    string cardName;

    [SerializeField]
    Texture cardPicture;

    [SerializeField]
    int QwiexCost;

    [SerializeField]
    GameObject prefabToSpawn;

    [SerializeField]
    CardTribe cardTribe;

    [SerializeField]
    CardRarity cardRarity;

    [SerializeField]
    CardType cardType;
}

enum CardTribe { IronCreekGang, NecroMasters, Mercenary }
enum CardRarity { Common, Uncommon, Rare, Epic }
enum CardType { Troop, Building, Spell }