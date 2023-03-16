using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Component : MonoBehaviour
{
    public Board_Contents boardContents;

    public int health;
    public int startingHealth;
    // Start is called before the first frame update
    void Start()
    {
        health = startingHealth;
        boardContents = GameObject.Find("GridManager").GetComponent<Board_Contents>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (health <= 0)
        {
            boardContents.SendMessage("RemoveObject", this.gameObject.transform);
            Destroy(gameObject);
        }
    }
}
