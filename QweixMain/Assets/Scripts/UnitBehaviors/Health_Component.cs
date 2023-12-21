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
*  Last Modification : 11/20/2023
*  Additional Notes  : -(08/18/2023) [Gavin] Added the OnUnitDeath event
*                      -(10/04/2023) [Gavin] Added hiding of the health bar if the unit is at full health.
*                      -This now requires that the Healthbar Hierarchy remains HealthbarBorder>HealthBarBackground>HealthBar in the prefab
*                      -(11/20/2023) [Gavin] Made currentHealth public get, private set.
*  External Documentation URL : https://trello.com/c/LVPox6UR/8-healthcomponent
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
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
    public float currentHealth { get; private set; }

    public static event Action<GameObject> OnUnitDeath;
    private GameObject thisUnit;

    // Reference to health bar UI
    public Image healthBar;
    private Image healthBarBackground;
    private Image healthBarBorder;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsServer) { return; }

        if (maxHealth <= 0)
        {
            Debug.Log("Max Health value not set");
        }
        else
        {
            currentHealth = maxHealth;
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
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {

        //Unhide Healthbar is unit has taken damage
        if (currentHealth < maxHealth)
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
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    private void Die()
    {
        SpawnOnDeath_Component spawnOnDeath = GetComponentInChildren<SpawnOnDeath_Component>();

        if (spawnOnDeath != null)
        {
            spawnOnDeath.Spawn();
        }

        OnUnitDeath?.Invoke(thisUnit);

        // Room for other *OnDeath script references here
        GetComponent<NetworkObject>().Despawn(true);
        //Destroy(gameObject);
    }
}
