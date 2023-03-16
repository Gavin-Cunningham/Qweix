/****************************************************************************
*
*File              : Card.cs
* Date Created      : 2 / 16 / 23
* Description       : This script is designed to allow the player to click on a card, drag the unit onto the board, have the board highlight appropriately, and spawn the correct object.
* 
*
*  Programmer(s)     : Robert Mimms
*  Last Modification : 3 / 16 / 23
* Additional Notes:
*Flow Chart URL :
*****************************************************************************
       (c)Copyright 2022 - 2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/



using System.Collections.Generic;
using UnityEngine;

public class PlaceByCard : MonoBehaviour
{
    // Variables for the ghosts that follow the mouse position as the player is placing a unit
    public GameObject following;
    public GameObject followParent;
    public float followSize;

    // Used for determining the tile the unit will spawn at when dropped
    public GameObject placeTile;
    public bool placing = false;

    // Represents the card being dragged onto the field and the unit that card will spawn.
    public GameObject spawningCard;
    public GameObject spawningUnit;

    //Reference to the parent of the tile grid.
    public GameObject gridParent;

    // A list of the currently highlighted tiles in the grid.
    public List<Transform> highlightedTiles = new List<Transform>();

    // A reference to the GridManager script used to access the array of tiles.
    public GridManager gridManager;
    public Tile tile;

    // Update is called once per frame
    void Update()
    {
        // This sets the followParent to the position of the mouse. Then creates a raycast from followParent position in order to correctly understand which card is clicked.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        followParent.transform.position = mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(followParent.transform.position, Vector2.zero);


        if (hit)
        {
            // Checks if the collider hit by the raycast is a card. If it is and it's clicked on
            // it checks the qweix cost against the current qweix count and creates a follower if there is enough.
            if (hit.transform.GetComponent<Card>() != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (QweixCheck(hit.transform.GetComponent<Card>()))
                    {
                        CreateFollower(hit.transform.gameObject.GetComponent<Card>());
                    }

                    else
                    {
                        Debug.Log("You don't have enough Qweix for that.");
                        //popUp.gameObject.SetActive(true);
                        //if(Input.GetMouseButtonDown(0))
                        //{
                        //    popUp.gameObject.SetActive(false);
                        //}

                    }
                }
            }

            // If the mouse is released and it has something following it will spawn the object associated with the card clicked.
            if (Input.GetMouseButtonUp(0) && following != null)
            {
                SpawnCardObject(spawningCard.GetComponent<Card>());
            }

            // Since the following objects are on an IgnoreRaycast layer, the raycast sees the tiles and
            // tells them to highlight accordingly if they're within the area of the object.
            if (hit.transform.GetComponent<Tile>() != null)
            {
                placeTile = hit.transform.gameObject;

                if (placing)
                {
                    if (placeTile != null)
                    {
                        if (following.GetComponent<Occupying_Component>().occupyingTiles.Length > 0)
                        {
                            for (int i = 0; i < following.GetComponent<Occupying_Component>().occupyingTiles.Length; i++)
                            {

                                Transform tile = following.GetComponent<Occupying_Component>().occupyingTiles[i].transform;
                                if (tile.GetComponent<Tile>() != null)
                                {
                                    tile.GetComponent<Tile>().Highlight();

                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // This reads in a card, sets spawned card to it for later, instantiates an object to follow the mouse position, sets the followsize to the bounds
    // of the object, and sets the text floating on the placeholder object to reflect the name of the card.  

    public void CreateFollower(Card card)
    {
        card.GetComponent<Card>().OnSpawn();

        placing = true;

        following = Instantiate(card.cardObject, new Vector3(followParent.transform.position.x, followParent.transform.position.y, 0), followParent.transform.rotation, followParent.transform);

        followSize = following.GetComponent<Size_Component>().size.x;

        if (following.GetComponent<Unit>() != null)
        {
            following.GetComponent<Unit>().unitText.text = card.GetComponent<Card>().cardName;
        }
    }

    // This calls spawncheck to make sure the object can be placed where the player is trying to,
    // then unhighlights the tiles underneath, destroys the follower, and instantiates the card object.
    // It checks if the card represented a unit or a spell. If a unit it adds it to the board contents array, 
    // and changes the text. if it's a spell it calls onCast to enact the spells effect.
    // Then it removes the qweix from the player's qweix total, clears the highlighted tiles array, and turns placing off.
    public void SpawnCardObject(Card card)
    {

        if (SpawnCheck())
        {
            foreach (Collider2D col in following.GetComponent<Occupying_Component>().occupyingTiles)
            {

                col.GetComponent<Tile>().Unhighlight();

            }
            Destroy(following);
            following = null;
            placing = false;

            var spawnedCard = Instantiate(card.gameObject.GetComponent<Card>().cardObject, new Vector3(placeTile.transform.position.x, placeTile.transform.position.y), Quaternion.identity);


            if (spawnedCard.GetComponent<Unit>() != null)
            {
                GetComponent<Board_Contents>().boardContents.Add(spawnedCard.transform);
                spawnedCard.GetComponent<Unit>().unitText.text = card.GetComponent<Card>().cardName;
            }
            if (spawnedCard.GetComponent<Spell>() != null)
            {
                spawnedCard.GetComponent<Spell>().OnCast();

            }
            gridManager.GetComponent<Player_Qweix_Component>().qweixCount -= card.GetComponent<Card>().quiexCost;

            highlightedTiles.Clear();

            placing = false;

        }
        // if it cannot be placed, it simply deletes the follower and unhighlights. 
        else
        {
            foreach (Collider2D col in following.GetComponent<Occupying_Component>().occupyingTiles)
            {
                if (col.GetComponent<Tile>() != null)
                    col.GetComponent<Tile>().Unhighlight();

            }
            Destroy(following);
            following = null;
            placing = false;

        }


    }

    // Checks the qweix count vs the cost and returns the result
    public bool QweixCheck(Card card)
    {

        if (gridManager.GetComponent<Player_Qweix_Component>().qweixCount >= card.GetComponent<Card>().quiexCost)
        {
            return true;
        }
        else
            return false;

    }

    // Each Tile knows if it's forbidden to be placed on or not, this checks all the tiles under the follower to check if it's an open space or not. Returns the results. 
    public bool SpawnCheck()
    {

        if (placeTile != null)
        {
            foreach (Collider2D col in following.GetComponent<Occupying_Component>().occupyingTiles)
            {

                if (col.GetComponent<Tile>().isForbidden)
                {
                    return false;
                }

            }
            return true;

        }
        else
            return false;


    }


}

// unnecessary old function 
//void unHighlight()
//{
//    if (placeTile != null)
//    {
//        for (int x = 0; x < GridManager.width; x++)
//        {
//            for (int y = 0; y < GridManager.height; y++)
//            {
//                int tileX = gridManager.tiles[x, y].GetComponent<Location_Component>().position[0];
//                int tileY = gridManager.tiles[x, y].GetComponent<Location_Component>().position[1];
//                if (tileX >= placeTile.GetComponent<Location_Component>().position[0] + followSize / 2 || tileY >= placeTile.GetComponent<Location_Component>().position[1] + followSize / 2)
//                {
//                    gridManager.tiles[x, y].GetComponent<Tile>().Renderer.color = tile.GetComponent<Tile>().OriginalColor;
//                    tile.GetComponent<Tile>().isHighlighted = false;

//                }
//                if (tileX <= placeTile.GetComponent<Location_Component>().position[0] - followSize / 2 || tileY <= placeTile.GetComponent<Location_Component>().position[1] - followSize / 2)
//                {
//                    gridManager.tiles[x, y].GetComponent<Tile>().Renderer.color = tile.GetComponent<Tile>().OriginalColor;
//                    tile.GetComponent<Tile>().isHighlighted = false;

//                }

//            }

//        }
//        highlightedTiles.Clear();
//    }
//}


// Prototype shunt mechanic...disregard...for now.
//public void FindOpenTile(Transform oldSpawn)
//{

//    int newX = oldSpawn.GetComponent<Location_Component>().position[0];
//    int newY = oldSpawn.GetComponent<Location_Component>().position[1];
//    int modifier = Mathf.RoundToInt(following.GetComponent<Size_Component>().unitSize.x / 2);
//    Transform newSpawn;

//    if (!gridManager.tiles[newX - modifier, newY].GetComponent<Tile>().isForbidden)
//    {
//        newSpawn = gridManager.tiles[newX - modifier, newY];
//        Debug.Log(gridManager.tiles[newX - modifier, newY].name);
//        if (!newSpawn.GetComponent<Neighbor_Component>().NeighborCheck(newSpawn, following))
//        {
//            shuntTile = newSpawn;

//        }

//    }
//    else if (!gridManager.tiles[newX, newY - modifier].GetComponent<Tile>().isForbidden)
//    {
//        newSpawn = gridManager.tiles[newX, newY - modifier];
//        Debug.Log(gridManager.tiles[newX, newY - modifier].name);
//        if (!newSpawn.GetComponent<Neighbor_Component>().NeighborCheck(newSpawn, following))
//        {
//            shuntTile = newSpawn;

//        }
//    }

//    else if (!gridManager.tiles[newX, newY + modifier].GetComponent<Tile>().isForbidden)
//    {
//        newSpawn = gridManager.tiles[newX, newY + modifier];
//        Debug.Log(gridManager.tiles[newX, newY + modifier].name);
//        if (!newSpawn.GetComponent<Neighbor_Component>().NeighborCheck(newSpawn, following))
//        {
//            shuntTile = newSpawn;
//        }
//    }
//    else if (!gridManager.tiles[newX + modifier, newY].GetComponent<Tile>().isForbidden)
//    {
//        newSpawn = gridManager.tiles[newX + modifier, newY];
//        Debug.Log(gridManager.tiles[newX + modifier, newY].name);
//        if (!newSpawn.GetComponent<Neighbor_Component>().NeighborCheck(newSpawn, following))
//        {
//            shuntTile = newSpawn;
//        }
//    }
//    else
//    {
//        Debug.Log("TOTES BROKE BRO");
//        shuntTile = gridManager.tiles[24, 10];
//    }
//}