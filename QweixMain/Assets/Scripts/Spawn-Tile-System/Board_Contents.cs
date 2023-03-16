using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board_Contents : MonoBehaviour
{
    public GridManager gridManager;
    

    public List<Transform> boardContents = new List<Transform>();
    public List<Transform> occupiedTiles = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //if (placing)
        //{
        //    Highlight();

        //}
        //else
        //    unHighlight();

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
