/****************************************************************************
*
*  File              : Card.cs      
*  Date Created      : 03/02/23   
*  Description       : This script is designed to keep track of what is on the board.
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

public class Board_Contents : MonoBehaviour
{
    public GridManager gridManager;
    public List<Transform> boardContents = new List<Transform>();
    public List<Transform> occupiedTiles = new List<Transform>();
    
    // This is in fixed update because it worked when I made it fixed.
    // This keeps a count of the things on the board, and the occupied tiles.
    private void FixedUpdate()
    {
        if (boardContents.Count > 0)
        {
            for (int i = 0; i < boardContents.Count; i++)
            {
                Transform unit = boardContents[i];

                int unitX = unit.GetComponent<Location_Component>().position[0];
                int unitY = unit.GetComponent<Location_Component>().position[1];

                Transform underUnit = gridManager.tiles[unitX, unitY];
                if (!occupiedTiles.Contains(underUnit))
                {
                    occupiedTiles.Add(underUnit);
                }
                for (int j = 0; j < boardContents[i].GetComponent<Occupying_Component>().occupyingTiles.Length; j++)
                {
                    
                        if (!occupiedTiles.Contains(boardContents[i].GetComponent<Occupying_Component>().occupyingTiles[j].transform))
                        {
                            occupiedTiles.Add(boardContents[i].GetComponent<Occupying_Component>().occupyingTiles[j].transform);
                        }
                    

                }

            }
        }
    }

    
    // This is called by the health component to remove the destroyed object from the array.
    public void RemoveObject(Transform transform)
    {
        Debug.Log("This was called when " + transform + " was destroyed");
        for(int i = 0; i<boardContents.Count;i++)
        {

            if (transform == boardContents[i])
            {
                boardContents.RemoveAt(i);
            }

        }

    }
    
}
