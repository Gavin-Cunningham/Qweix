using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DestroySelf_Component : NetworkBehaviour
{
    [SerializeField] float timeToDestroy = 3.0f;
    [SerializeField] private bool fadeOut = false;
    private float fadeOutTime = 1.0f;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        
        if (fadeOut)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

    }

    private void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if (fadeOut)
        {
            spriteRenderer.color = new (1, 1, 1, (timeToDestroy / fadeOutTime));
        }

        if (!IsServer) { return; }

        if (timeToDestroy <= 0f)
        {
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}
