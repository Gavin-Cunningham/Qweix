/****************************************************************************
*
*  File              : Card.cs      
*  Date Created      : 03/02/23   
*  Description       : This script is designed to hold the unit attributes. 
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
using TMPro;

public class Unit : MonoBehaviour
{


    // Currently contains reference to the text on the unit and the location.
    
    public TextMeshProUGUI unitText;
    [SerializeField]
    private Location_Component location;
    
   
   
}
