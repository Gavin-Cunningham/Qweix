using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    public float spellTimer = 5;

    public bool wasCast;


    // 
    public void OnCast()
    {
       
        wasCast = true;
       Destroy(gameObject, 5f);

    }
}
