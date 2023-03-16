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
    


    // Start is called before the first frame update
    void Start()
    {
        localX = transform.localScale.x / 2;
        localY = transform.localScale.y / 2;
        //Debug.Log(Mathf.RoundToInt(localX));
        if (location == null)
            Debug.Log("Location Component not found");
        started = true;
    }

    // Update is called once per frame
    void Update()
    {

        occupyingTiles = Physics2D.OverlapBoxAll(new Vector2(GetComponent<Location_Component>().position[0], GetComponent<Location_Component>().position[1]), new Vector2(Mathf.RoundToInt(localX), Mathf.RoundToInt(localY)), 0);
       
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
