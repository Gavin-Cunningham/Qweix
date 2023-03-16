using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public const int width = 32;
    public const int height = 18;
    
    [SerializeField] private Tile tile;
    [SerializeField] private Transform cam;
    public GameObject gridParent;
    [SerializeField] public Transform[,] tiles;



    private void Start()
    {

        createGrid();
        generateNeighborArray(); 
    }

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


                //if (x > 13)
                //{

                //    tile.Renderer.color = new Color(.65f, .65f, .65f, .65f);

                //}
            }
        }


    }


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
