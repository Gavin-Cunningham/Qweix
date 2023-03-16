using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //declares variables for the mouseover highlight
    public SpriteRenderer Renderer;
    public Color MouseOverColor = new Color(255 / 255f, 237 / 255f, 156 / 255f, .85f);
    public Color OriginalColor;
    public Color HighlightColor;
    public Color ForbiddenColor;

    public bool isForbidden;
    public bool isHighlighted;
 


    public GridManager gridManager;
   
    public PlaceByCard placeByCard;


    public int[] tilePosition;

    // Creates the array for the Tile Position, and finds the GridManager and PlceByCard scripts
    private void Awake()
    {
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        placeByCard = GameObject.Find("GridManager").GetComponent<PlaceByCard>();

    }

    void Start()
    {
        tilePosition = GetComponent<Location_Component>().position;
        //gets the renderer and sets the OG color for later reference
        Renderer = GetComponent<SpriteRenderer>();
        OriginalColor = Renderer.color;
        if (tilePosition[0] > 13 && tilePosition[0] < 18)
        {
            isForbidden = true;
            if (tilePosition[1] < 16 && tilePosition[1] > 12)
            {
                isForbidden = false;
            }
            else if (tilePosition[1] > 1 && tilePosition[1] < 5)
            {
                isForbidden = false;
            }
        }
    }

    void OnMouseOver()
    {
        //As the mouse touches a tile it highlights, simple as that
        if (!placeByCard.placing)
        {
            Renderer.color = MouseOverColor;
            isHighlighted = true;
        }
    }

    void OnMouseExit()
    {
        isHighlighted = false;
        // if the tile is not its original color and belives it's highlighted, as the mouse leaves it, it reverts to its original color
        if (Renderer.color != OriginalColor && !isHighlighted)
        {
            Renderer.color = OriginalColor;
            Unhighlight();

        }
    }

    private void Update()
    {
        // If there is a card being placed, the script checks if this tile should highlight. If it should, it does. If the card is hovering over the gap or another structure/unit the tiles highlight red. 
        if (placeByCard.following != null)
        {
            if (shouldHighlight())
            {
                Renderer.color = HighlightColor;
                isHighlighted = true;

                foreach (Collider2D col in placeByCard.following.GetComponent<Occupying_Component>().occupyingTiles)
                {
                    if (isForbidden)
                    {
                            Renderer.color = ForbiddenColor;
                            isHighlighted = true;
                           
                           
                                foreach (Collider2D loc in placeByCard.following.GetComponent<Occupying_Component>().occupyingTiles)
                                {
                                    Renderer.color = ForbiddenColor;
                                    isHighlighted = true;
                                    isForbidden = true;
                                }
                            }
                        }

                        foreach (Transform transform in placeByCard.GetComponent<Board_Contents>().occupiedTiles)
                {

                    if (transform.gameObject == gameObject)
                    {

                        Renderer.color = ForbiddenColor;
                        isForbidden = true;
                        isHighlighted = true;

                        if (isForbidden)
                        {
                            foreach (Collider2D col in placeByCard.following.GetComponent<Occupying_Component>().occupyingTiles)
                            {
                                Renderer.color = ForbiddenColor;
                                isHighlighted = true;
                                isForbidden = true;
                            }
                        }
                    }
                }
            }
            else
                Unhighlight();
        }

        //if (placeByCard.following != null)
        //{

        //    Debug.Log(placeByCard.following.name);
        //}

    }

    // Called by PlaceByCard during placing. 
    public void Highlight()
    {
        
        foreach (Transform transform in placeByCard.GetComponent<Board_Contents>().occupiedTiles)
        {

            if (transform.gameObject == gameObject)
            {

                Renderer.color = ForbiddenColor;
                isHighlighted = true;

            }

        }
        placeByCard.highlightedTiles.Clear();

    }

    public bool shouldHighlight()
    {
        if (placeByCard.following != null)
        {
            for (int i = 0; i < placeByCard.following.GetComponent<Occupying_Component>().occupyingTiles.Length; i++)
            {
                if (placeByCard.following.GetComponent<Occupying_Component>().occupyingTiles[i].transform == gameObject.transform)
                {
                    return true;

                }

            }
            return false;
        }
            return false;
    }
    public void Unhighlight()
    {

        Renderer.color = OriginalColor;
        isHighlighted = false;


    }

}
