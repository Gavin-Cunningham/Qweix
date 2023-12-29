/****************************************************************************
*
*  File              : Health_Component.cs
*  Date Created      : 05/31/2023 
*  Description       : Receives damage and subtracts the amount of damage received from current health
*
*                      Updates health bar UI display when damage is taken
*
*                      If health is zero or less after taking damage, perform death actions then destroy self
*                      
*  Requirements      : A Health Bar on the game object with the UI Image referenced as the healthBar variable
*
*  Programmer(s)     : Gabe Burch, Gavin Cunningham
*  Last Modification : 12/03/2023
*  Additional Notes  : -(08/18/2023) [Gavin] Added the OnUnitDeath event
*                      -(10/04/2023) [Gavin] Added hiding of the health bar if the unit is at full health.
*                      -This now requires that the Healthbar Hierarchy remains HealthbarBorder>HealthBarBackground>HealthBar in the prefab
*                      -(11/20/2023) [Gavin] Made currentHealth public get, private set.
*                      -(12/03/2023) [Gavin] Made Start() virtual and healthBarBorder protected to allow inheritors to edit (MoveableBar_Health_Component)
*  External Documentation URL : https://trello.com/c/LVPox6UR/8-healthcomponent
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.Netcode;

public class Health_Component : NetworkBehaviour    
{
    // Maximum health available
    public float maxHealth;
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>();

    public static event Action<GameObject> OnUnitDeath;
    private GameObject thisUnit;

    // Reference to health bar UI
    public Image healthBar;
    private Image healthBarBackground;
    protected Image healthBarBorder;

    [SerializeField] private bool leaveDebris = false;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        //if (!IsServer) { return; }

        if (maxHealth <= 0)
        {
            Debug.Log("Max Health value not set");
        }
        else
        {
            currentHealth.Value = maxHealth;
        }

        if (healthBar == null)
        {
            Debug.Log("Health Bar reference not set");
        }

        thisUnit = gameObject;

        //This autopulls the two parent components from the health bar for the purposes of hiding. 
        //If the Health bar heirarchy is to change this will need to be altered or changed to inspector set references
        //Have to get through transform because GetComponentInParent doesn't work straight from the Image component???
        healthBarBackground = healthBar.transform.parent.GetComponentInParent<Image>(true);
        healthBarBorder = healthBarBackground.transform.parent.GetComponentInParent<Image>(true);

        //Hides Health bar initially
        healthBar.enabled = false;
        healthBarBackground.enabled = false;
        healthBarBorder.enabled = false;

        currentHealth.OnValueChanged += OnHealthValueChange;

    }

    //public override void OnNetworkSpawn()
    //{
    //    currentHealth.OnValueChanged += OnHealthValueChange;
    //}

    public void OnHealthValueChange(float previous, float current)
    {
        UpdateHealthBar();
    }

    public void TakeDamage(float damageAmount)
    {
        if(!IsServer){ return; }
        currentHealth.Value -= damageAmount;

        UpdateHealthBar();

        if (currentHealth.Value <= 0)
        {
            DeathAnimation();
        }
    }

    private void UpdateHealthBar()
    {

        //Unhide Healthbar is unit has taken damage
        if (currentHealth.Value < maxHealth)
        {
            healthBar.enabled = true;
            healthBarBackground.enabled = true;
            healthBarBorder.enabled = true;
        }
        else
        {
            healthBar.enabled = false;
            healthBarBackground.enabled = false;
            healthBarBorder.enabled = false;
        }

        //Updates Healthbar fill amount
        healthBar.fillAmount = currentHealth.Value / maxHealth;
    }

    private void DeathAnimation()
    {
        GetComponent<Animator>().Play("Death");
    }

    //Now called by "death" animation event
    private void Die()
    {
        SpawnOnDeath_Component spawnOnDeath = GetComponentInChildren<SpawnOnDeath_Component>();

        if (spawnOnDeath != null)
        {
            spawnOnDeath.Spawn();
        }

        OnUnitDeath?.Invoke(thisUnit);

        // Room for other *OnDeath script references here
        if (!leaveDebris)
        {
            GetComponent<NetworkObject>().Despawn(true);
        }
        //Destroy(gameObject);
    }
}
