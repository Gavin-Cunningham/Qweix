using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor_Component : MonoBehaviour
{

    public GridManager gridManager;
    public Transform[] neighbors;
    public List<Transform> neighborList = new List<Transform>();

    private void Awake()
    {
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


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

    public bool NeighborCheck(Transform newSpawn, GameObject following)
    {
        bool forbidden = false;
        foreach (Transform transform in newSpawn.GetComponent<Neighbor_Component>().neighborList)
        {

            if (transform.GetComponent<Tile>().isForbidden)
            {
                forbidden = true;
            }
        }
        return forbidden;

    }
}
