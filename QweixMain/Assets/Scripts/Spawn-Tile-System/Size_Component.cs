/****************************************************************************
*
*  File              : Size_Component.cs      
*  Date Created      : 03/02/23   
*  Description       : This script is designed to hold a unit/structure's size. 
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

public class Size_Component : MonoBehaviour
{
    public Vector3 size;


    // Sets the vector3 size to the localScale of the object
    void Start()
    {
        size = transform.localScale;
        
    }

}
