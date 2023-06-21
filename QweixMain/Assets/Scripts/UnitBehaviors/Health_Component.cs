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
*  Programmer(s)     : Gabe Burch
*  Last Modification : 05/31/2023
*  Additional Notes  : 
*  External Documentation URL : https://trello.com/c/LVPox6UR/8-healthcomponent
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Component : MonoBehaviour    
{
    // Maximum health available
    public float maxHealth;
    private float currentHealth;

    // Reference to health bar UI
    public Image healthBar;

    private float damageTimer;

    // Start is called before the first frame update
    void Start()
    {
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

        damageTimer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        damageTimer -= Time.deltaTime;

        if(damageTimer < 0)
        {
            damageTimer = 1;
            TakeDamage(10);
        }
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
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    private void Die()
    {
        SpawnOnDeath_Component spawnOnDeath = GetComponentInChildren<SpawnOnDeath_Component>();

        if (spawnOnDeath != null)
        {
            spawnOnDeath.Spawn();
        }

        // Room for other *OnDeath script references here

        Destroy(gameObject);
    }
}
