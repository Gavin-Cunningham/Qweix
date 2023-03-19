/****************************************************************************
*
*  File              : Health_Component.cs      
*  Date Created      : 3/16/23   
*  Description       : This script is designed to represent the unit/structure's health stat. 
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

public class Health_Component : MonoBehaviour
{
    public Board_Contents boardContents;
    public int health;
    public int startingHealth;
    
    
    // Starts by setting the health of the object to it's maximum or starting health
    // and sets the board_contents by finding the Grid Manager
    void Start()
    {
        health = startingHealth;
        boardContents = GameObject.Find("GridManager").GetComponent<Board_Contents>();
    }

    // calls the RemoveObject method in BoardContents to remove it from the array
    // and destroys the object
    void Update()
    {
        
        if (health <= 0)
        {
            boardContents.RemoveObject(this.gameObject.transform);
            Destroy(gameObject);
        }
    }
}
