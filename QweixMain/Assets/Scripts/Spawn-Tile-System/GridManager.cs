/****************************************************************************
*
*  File              : Card.cs      
*  Date Created      : 01/16/23 
*  Description       : This script is designed to be the brains of the grid and the tilesystem 
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
using System.Dynamic;
using Mono.CSharp;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // constants for the height and width of the grid
    public const int width = 32;
    public const int height = 16;
    
    // References to the Tile script as well as the grid parent.
    [SerializeField] private Tile tile;
    public GameObject gridParent;
    public GameObject fogParent;
    
    // This is the array the tiles are stored in, made it serialized so we could see it.
    [SerializeField] public Transform[,] tiles;
    [SerializeField] public Transform[,] fogTiles;



    private void Start()
    {
        // Creates the grid and calls the generateNeighbors method for each tile.
        createGrid();
        generateNeighborArray();
        // generateFogOfWar();
    }

    
    // Uses nested loops to create a width by height grid. 
    // At each position it instantiates a tile, puts them in the array, and names them after their coordinates
    void createGrid()
    {
        Debug.Log("Creating Grid");

        tiles = new Transform[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tile, new Vector3(x, y), Quaternion.identity, gridParent.transform);
                    tiles[x, y] = spawnedTile.gameObject.transform;
                    spawnedTile.name = $"Tile {x},{y}";
            }

            
        }
    }

    public void removeTiles()
    {

        foreach (Transform tile in tiles)
        {
            
            if (tile.GetComponent<Tile>().isForbidden)
            {
                Destroy(tile.gameObject);
                
            }
            
        }

        // foreach (Transform fogTile in fogTiles)
        // {
        //
        //     if (fogTile.gameObject.GetComponent<Tile>().isForbidden)
        //     {
        //         Destroy(fogTile.gameObject);
        //     }
        //     
        // }
    }
    public void generateFogOfWar()
    {
        fogTiles = new Transform[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x > 15)
                {
                    var fogTile = Instantiate(tile, new Vector3(x, y, 5), Quaternion.identity,
                        fogParent.transform);
                    fogTiles[x, y] = fogTile.transform;
                    fogTile.name = $"FOGOFWAR Tile {x},{y}";
                    fogTile.GetComponent<Tile>().Renderer.color = fogTile.GetComponent<Tile>().FogOfWarColor;
                }
            }
        }
    }
    // For each tile in the grid it calls the generateNeighbors method in the Neighbor_Component
    void generateNeighborArray()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                tiles[i, j].gameObject.GetComponent<Neighbor_Component>().generateNeighbors();


            }
        }
    }
}
