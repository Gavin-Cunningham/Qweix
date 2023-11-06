using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
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

        //Its mutation time
        if (mutationRemainingTime <= 0.0f)
        {
            //Animator.Play;
        }

    }

    //The Lycan starts off with a slow speed and low damage.
    //Both a bar counts down and he flashes red faster to indicate that he is about to change. 
    //Then he stops, does his transformation animation and then instantiates the new character.
}
