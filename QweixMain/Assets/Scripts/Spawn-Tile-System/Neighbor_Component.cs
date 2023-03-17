/****************************************************************************
*
*  File              : Neighbor_Component.cs      
*  Date Created      : 03/02/23   
*  Description       : This script is designed to find the neighbors of each tile and store them in an array. 
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

public class Neighbor_Component : MonoBehaviour
{

    public GridManager gridManager;
    public Transform[] neighbors;
    public List<Transform> neighborList = new List<Transform>();

    // Finds the gridManager
    private void Awake()
    {
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();

    }

    // Creates an array to hold the tiles, then with a for loop goes through the neighboring positions
    // of the tile and adds them to the array in clockwise order starting at the tile below.
    public void generateNeighbors()
    {
        // Debug.Log("Generating Neighbors.");
        Transform[,] tiles = gridManager.tiles;

        neighbors = new Transform[8];
        int x = GetComponent<Location_Component>().position[0];
        int y = GetComponent<Location_Component>().position[1];
        int width = GridManager.width;
        int height = GridManager.height;

        for (int i = 0; i < 8; i++)
        {

            switch (i)
            {
                case 0:
                    if (y - 1 >= 0)
                        neighbors[i] = gridManager.tiles[x, y - 1].transform;
                    break;
                case 1:
                    if (x - 1 >= 0 && y - 1 >= 0)
                        neighbors[i] = gridManager.tiles[x - 1, y - 1].transform;
                    break;

                case 2:
                    if (x - 1 >= 0)
                        neighbors[i] = gridManager.tiles[x - 1, y].transform;
                    break;

                case 3:
                    if (x - 1 >= 0 && y + 1 < height)
                        neighbors[i] = gridManager.tiles[x - 1, y + 1].transform;
                    break;

                case 4:
                    if (y + 1 < height)
                        neighbors[i] = gridManager.tiles[x, y + 1].transform;
                    break;
                case 5:
                    if (x + 1 < width && y + 1 < height)
                        neighbors[i] = gridManager.tiles[x + 1, y + 1].transform;
                    break;

                case 6:
                    if (x + 1 < width)
                        neighbors[i] = tiles[x + 1, y].transform;

                    break;
                case 7:
                    if (x + 1 < width && y - 1 >= 0)
                        neighbors[i] = gridManager.tiles[x + 1, y - 1].transform;
                    break;
                default:
                    break;
            }


        }

        for (int i = 0; i < neighbors.Length; i++)
        {
            neighborList.Add(neighbors[i]);
        }
    }
}
// Old script for the shunting mechaninc
    // public bool NeighborCheck(Transform newSpawn, GameObject following)
    // {
    //     bool forbidden = false;
    //     foreach (Transform transform in newSpawn.GetComponent<Neighbor_Component>().neighborList)
    //     {
    //
    //         if (transform.GetComponent<Tile>().isForbidden)
    //         {
    //             forbidden = true;
    //         }
    //     }
    //     return forbidden;
    //
    // }

