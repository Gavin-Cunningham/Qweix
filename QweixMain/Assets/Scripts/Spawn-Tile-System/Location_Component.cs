/****************************************************************************
*
*  File              : Location_Component.cs      
*  Date Created      : 3/02/23   
*  Description       : This script is designed to hold the location of the tile/unit/structure. 
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

public class Location_Component : MonoBehaviour
{

    // Uses an array of ints for the x and y coordinates of the unit/tile/structure
    public int[] position;

    // On awake it creates the array and calls the GetPosition method
    private void Awake()
    {
        position = new int[2];
        GetPosition();
    }

    // Calls get position in update so it can be used on moving units
    private void Update()
    {
        GetPosition();
    }
    
    // Just sets the correct array elements to the coordinates of the transform and returns it.
    public int[] GetPosition()
    {
        //Debug.Log(Mathf.RoundToInt(this.transform.position.x) + ", " + Mathf.RoundToInt(this.transform.position.y));

        position[0] = Mathf.RoundToInt(this.transform.position.x);
        position[1] = Mathf.RoundToInt(this.transform.position.y);


        return position;

    }

}
