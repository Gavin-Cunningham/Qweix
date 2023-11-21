/****************************************************************************
*
*  File              : LycanTransformation_Component.cs
*  Date Created      : 11/08/2023 
*  Description       : This script handles all the custom behaviors for the Lycan_Human form
*  and its transformation into the Lycan_Beast.
*
*  Programmer(s)     : Gavin Cunningham
*  Last Modification : 11/08/2023 
*  Additional Notes  : 
*  External Documentation URL :
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LycanTransformation_Component : MonoBehaviour
{
    [SerializeField] private GameObject beastFormPrefab;
    [SerializeField] private float mutationTime;
    private float mutationRemainingTime;
    private float mutationWarningPause = 1.0f;
    private float mutationWarningCountdown;
    [SerializeField] private Image mutationBar;
    [SerializeField] private Color mutationWarnColor = new Color(255, 255, 255, 255);
    [SerializeField] private Color warnColorFade = new Color(10, 10, 10, 255);
    private SpriteRenderer spriteRenderer;
    bool transformEventCalled = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mutationRemainingTime = mutationTime;
    }

    private void Update()
    {
        //Countdown time till mutation.
        mutationRemainingTime -= Time.deltaTime;

        //Make the Character blink faster the closer it is to mutation time.
        mutationWarningPause -= Time.deltaTime / mutationTime;

        //Make the character the warning color if the interval has passed.
        if (mutationWarningCountdown <= 0.0f)
        {
            mutationWarningCountdown = mutationWarningPause;
            spriteRenderer.color = mutationWarnColor;
        }

        //Countdown the interval for warning color.
        mutationWarningCountdown -= Time.deltaTime;

        //Fade the warning color back to white.
        spriteRenderer.color = spriteRenderer.color + warnColorFade;

        //Make the mutation bar drop as the time gets less.
        mutationBar.fillAmount = mutationRemainingTime / mutationTime;

        //Its mutation time!
        if (mutationRemainingTime <= 0.0f)
        {

            GetComponent<Animation_Component>().enabled = false;
            GetComponent<Movement_Component>().enabled = false;
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<Animator>().Play("Transform");
        }
    }

    public void TransformEvent()
    {
        if (!transformEventCalled)
        {
            transformEventCalled = true;

            //Spawn in Beast Form
            GameObject beastForm = Instantiate(beastFormPrefab, transform.position, new Quaternion(0, 0, 0, 0));
            //Copy team to Beast Form
            beastForm.GetComponent<Targeting_Component>().teamCheck = GetComponent<Targeting_Component>().teamCheck;

            //Copy over Health percentage from human to beast.
            /*Health_Component beastHealth = beastForm.GetComponent<Health_Component>();
            Health_Component humanHealth = GetComponent<Health_Component>();
            float damageAmount = beastHealth.maxHealth * (1 - (humanHealth.currentHealth / humanHealth.maxHealth));
            beastHealth.TakeDamage(damageAmount);*/

            //Destroy human form so that the beast may reign.
            Destroy(gameObject);
        }

    }

    //The Lycan starts off with a slow speed and low damage.
    //Both a bar counts down and he flashes red faster to indicate that he is about to change. 
    //Then he stops, does his transformation animation and then instantiates the new character.
}
