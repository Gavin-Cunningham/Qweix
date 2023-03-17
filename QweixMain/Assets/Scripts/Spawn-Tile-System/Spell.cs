/****************************************************************************
*
*  File              : Spell.cs      
*  Date Created      : 03/14/23   
*  Description       : This script is designed to be the spell equivalent of the Unit script and will eventually contain the spells' effects. 
*
*  Programmer(s)     : Robert Mimms
*  Last Modification : 3/16/23
*  Additional Notes  : 
*  Flow Chart URL :
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    public float spellTimer = 5;

    public bool wasCast;


    // Sets a bool to keep track of when this was cast to true, and destroys the object after 5 seconds.
    // This is fully a placeholder script to make sure the spells were functioning and spawning correctly.
    public void OnCast()
    {
       
        wasCast = true;
       Destroy(gameObject, 5f);

    }
}
