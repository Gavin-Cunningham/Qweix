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
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCore : MonoBehaviour
{
    [SerializeField]
    public int cardID;

    [SerializeField]
    public string cardName;

    [SerializeField]
    public Texture2D cardPicture;

    [SerializeField]
    public int qwiexCost;

    [SerializeField]
    public GameObject prefabToSpawn;

    [SerializeField]
    public CardTribe cardTribe;

    [SerializeField]
    public CardRarity cardRarity;

    [SerializeField]
    public CardType cardType;
}

public enum CardTribe { IronCreekGang, NecroMasters, Mercenary }
public enum CardRarity { Common, Uncommon, Rare, Epic }
public enum CardType { Troop, Building, Spell }