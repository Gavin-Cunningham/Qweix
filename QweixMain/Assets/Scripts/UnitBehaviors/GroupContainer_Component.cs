using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupContainer_Component : MonoBehaviour
{
    void Start()
    {
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
