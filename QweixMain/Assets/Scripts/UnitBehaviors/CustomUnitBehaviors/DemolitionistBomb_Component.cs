/****************************************************************************
*
*  File              : DemolitionistBomb_Component.cs
*  Date Created      : 11/22/2023 
*  Description       : This goes on the bomb prefab that the demolitionist plants.
*  It hands receiving the targeting & damage info and issuing damage. & blinking.
*
*  Programmer(s)     : Gavin Cunningham
*  Last Modification : 
*  Additional Notes  : 

*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemolitionistBomb_Component : MonoBehaviour
{
    [Tooltip("Optional Prefab for an explosion effect to play after the bomb blows up.")]
    [SerializeField] private GameObject[] effectsList;

    [NonSerialized] public GameObject currentTarget;
    [NonSerialized] public float countdownTime = 3.0f;
    private float countdownTimeTotal;
    [NonSerialized] public float bombDamage = 5.0f;

    private float warningCountdown;
    [Tooltip("How often should the bomb blink. Speed up over time.")]
    [SerializeField] private float warningBlinkLength = 1.0f;
    [Tooltip("How fast after blinking should the bomb return to its normal color.")]
    [SerializeField, Range(0.0f, 2.0f)] private float warnColorFadeRate = 1.0f;
    [Tooltip("What color should the bomb blink. Default is red")]
    [SerializeField] private Color warnColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        countdownTimeTotal = countdownTime;
    }

    void Update()
    {
        if (currentTarget == null)
        {
            //If our target is already destroyed, go ahead and blow up to clear the field.
            BlowUp();
        }

        //When timer is up, blow up and deal damage.
        countdownTime -= Time.deltaTime;
        if (countdownTime < 0)
        {
            BlowUp();
        }

        //Flash to show that we are getting closer to blowing up.
        CountdownFlashing();
    }

    private void BlowUp()
    {
        if (currentTarget != null)
        {
            currentTarget.SendMessage("TakeDamage", bombDamage, SendMessageOptions.DontRequireReceiver);
        }
        if (effectsList != null)
        {
            foreach(GameObject effect in effectsList)
            {
                Instantiate(effect, transform.position, new Quaternion(0, 0, 0, 0));
            }
        }
        Destroy(gameObject);
    }

    private void CountdownFlashing()
    {
        //Make the Character blink faster the closer it is to mutation time.
        warningBlinkLength -= Time.deltaTime / countdownTimeTotal;

        //Make the character the warning color if the interval has passed.
        if (warningCountdown <= 0.0f)
        {
            warningCountdown = warningBlinkLength;
            spriteRenderer.material.color = warnColor;
        }

        //Countdown the interval for warning color.
        warningCountdown -= Time.deltaTime;

        //Fade the warning color back to white.
        spriteRenderer.material.color = spriteRenderer.material.color + (new Color(0.01f, 0.01f, 0.01f, 1.0f) * (warnColorFadeRate / warningBlinkLength));
    }
}
