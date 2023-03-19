/****************************************************************************
*
*  File              : Card.cs      
*  Date Created      : 3/16/23   
*  Description       : This script is designed to hold and update a player's qweix count. 
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
using TMPro;

public class Player_Qweix_Component : MonoBehaviour
{

    // player's qweix count
    public int qweixCount = 0;

    // a textual reference to the count
    public TextMeshProUGUI textQweixtCount;

    // The update just keeps the on-screen counter updated on the current count
    private void Update()
    {
        textQweixtCount.text = qweixCount.ToString();
    }

    
    // Ups the qweix count and is called by the button on-screen
    public void AddQweix()
    {

        qweixCount++;

    }
    
    // Lowers the qweix count and is called by the button on-screen
    public void removeQweix()
    {

        qweixCount--;

    }


}
