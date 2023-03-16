/****************************************************************************
*
*  File              : Highlight_Component.cs
*  Date Created      : 3/2/23
*  Description       : This script is designed to create an array of tiles that need to be highlighted with an overlapbox
*
*  Programmer(s)     : Robert Mimms
*  Last Modification : 3/9/23
*  Additional Notes  : 
*  Flow Chart URL :
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight_Component : MonoBehaviour
{
    // OverlapBoxAll returns an array of Collider2Ds, this is here to hold that.
    public Collider2D[] highlightedTiles;

    // We need the location of the object in order to determine the location of the OverlapBoxAll.
    [SerializeField]
    private Location_Component location;



    // Start is called before the first frame update
    void Start()
    {
        if (location == null)
            Debug.Log("Location Component not found");

    }

    // Update is called once per frame
    void Update()
    {
        // This creates an array of Collider2Ds that comprise the objects below the spawning object. The PlaceByCard script then reads through the array, ensures the objects are tiles, and calls their highlight method in Tile.
        highlightedTiles = Physics2D.OverlapBoxAll(new Vector2(GetComponent<Location_Component>().position[0], GetComponent<Location_Component>().position[1]), new Vector2(GetComponent<Size_Component>().size.x / 2, GetComponent<Size_Component>().size.y / 2), 0);


    }
}
