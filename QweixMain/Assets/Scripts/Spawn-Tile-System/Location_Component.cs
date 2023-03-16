using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location_Component : MonoBehaviour
{

    public int[] position;


    private void Awake()
    {
        position = new int[2];
        GetPosition();
    }

    private void Update()
    {
        GetPosition();
    }
    public int[] GetPosition()
    {
        //Debug.Log(Mathf.RoundToInt(this.transform.position.x) + ", " + Mathf.RoundToInt(this.transform.position.y));

        position[0] = Mathf.RoundToInt(this.transform.position.x);
        position[1] = Mathf.RoundToInt(this.transform.position.y);


        return position;

    }

}
