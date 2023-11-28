using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class DemolitionistBomb_Component : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab = null;
    [NonSerialized] public GameObject currentTarget;
    [NonSerialized] public float countdownTime = 3.0f;
    private float countdownTimeTotal;
    [NonSerialized] public float bombDamage = 5.0f;

    private float warningCountdown;
    [SerializeField] private float warningBlinkLength = 1.0f;
    [SerializeField, Range(0.0f, 2.0f)] private float warnColorFadeRate = 1.0f;
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
            BlowUp();
        }
        countdownTime -= Time.deltaTime;
        if (countdownTime < 0)
        {
            BlowUp();
        }
        CountdownFlashing();
    }

    private void BlowUp()
    {
        if (currentTarget != null)
        {
            currentTarget.SendMessage("TakeDamage", bombDamage, SendMessageOptions.DontRequireReceiver);
        }
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, new Quaternion(0, 0, 0, 0));
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
            spriteRenderer.color = warnColor;
        }

        //Countdown the interval for warning color.
        warningCountdown -= Time.deltaTime;

        //Fade the warning color back to white.
        spriteRenderer.color = spriteRenderer.color + (new Color(0.01f, 0.01f, 0.01f, 1.0f) * (warnColorFadeRate / warningBlinkLength));
    }
}
