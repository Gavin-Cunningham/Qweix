using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Size_Component : MonoBehaviour
{
    public Vector3 size;


    // Start is called before the first frame update
    void Start()
    {
        size = transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
