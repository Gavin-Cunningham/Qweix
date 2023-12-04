/****************************************************************************
*
*  File              : GroupContainer_Component
*  Date Created      : 10/26/2023 
*  Description       : This should go on an empty object which is tied to the card
*  for groups of units that spawn in together.
*
*  Programmer(s)     : 
*  Last Modification : 
*  Additional Notes  : 

*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupContainer_Component : MonoBehaviour
{

    void Start()
    {
        AssignChildrenTeams();
        transform.DetachChildren();
        Destroy(gameObject);
    }

    private void AssignChildrenTeams()
    {
        int groupTeam = GetComponent<Targeting_Component>().teamCheck;
        Targeting_Component[] childrenTCs = GetComponentsInChildren<Targeting_Component>();

        foreach (Targeting_Component tC in childrenTCs)
        {
            tC.teamCheck = groupTeam;
        }
    }
}
