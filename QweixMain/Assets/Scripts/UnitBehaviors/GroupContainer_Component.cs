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
