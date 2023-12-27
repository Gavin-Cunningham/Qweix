using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DestroySelf_Component : NetworkBehaviour
{
    [SerializeField] float timeToDestroy = 3.0f;

    private void Update()
    {
        if (!IsHost) { return; }
        if (timeToDestroy != 0.0f)
        {
            timeToDestroy -= Time.deltaTime;
        }
        else
        {
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}
