



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{


    public SpriteRenderer Renderer;

    public Color color;


    public TextMeshProUGUI unitText;

    [SerializeField]
    private Location_Component location;



    private void Start()
    {
     
        Renderer = GetComponent<SpriteRenderer>();
        color = Renderer.color;
        

    }
   
   
}
