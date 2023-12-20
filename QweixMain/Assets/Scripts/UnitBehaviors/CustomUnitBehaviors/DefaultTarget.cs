using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultTarget : MonoBehaviour
{

    public static DefaultTarget instance { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
