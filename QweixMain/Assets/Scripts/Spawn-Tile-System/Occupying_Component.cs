/****************************************************************************
*
*  File              : Card.cs      
*  Date Created      : 03/02/23   
*  Description       : This script is designed to create an array containing the tiles that a unit/structure is occupying. 
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

public class Occupying_Component : MonoBehaviour
{

    // OverlapBoxAll returns an array of Collider2Ds, this is here to hold that.
    public Collider2D[] occupyingTiles;

    // The box relies on Vector2s for the location and the size, this 
    private float localX;
    private float localY;

    // This ensures the Gizmo is only drawn in Play Mode, can be easily removed if that's not wanted.
    bool started;


    // We need the location of the object in order to determine the location of the OverlapBoxAll.
    [SerializeField]
    private Location_Component location;
    

    // Takes the local scale of x and y, divides them by 2 in order to get the distance from the center point 
    // in each direction. Also throws an error if the location component can't be found. It flips the bool that
    // denotes the game has started, so the gizmos aren't drawn in the scene view.
    void Start()
    {
        localX = transform.localScale.x / 2;
        localY = transform.localScale.y / 2;
        if (location == null)
            Debug.Log("Location Component not found");
        started = true;
    }

    // Uses an OverlapBoxAll to return all the colliders being touched by the object and puts them in an array.
    void Update()
    {

        occupyingTiles = Physics2D.OverlapBoxAll(new Vector2(GetComponent<Location_Component>().position[0], GetComponent<Location_Component>().position[1]), new Vector2(Mathf.RoundToInt(localX), Mathf.RoundToInt(localY)), 0);
       
        
    }

    // Draws the overlap box in the scene in case the highlight isn't working.
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
