using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataClasses;
using UnityEngine.UIElements;

public class DummyData : MonoBehaviour
{
    [SerializeField]
    private UIControl_Battlefield UIController;

    [SerializeField]
    private PlayerInfo dummyPlayerInfo;

    [SerializeField]
    private PlayerInfo dummyOpponentInfo;

    [SerializeField]
    private List<CardInfo> dummyCardInfo;

    [SerializeField]
    // Length of the timer in minutes
    private float timerMinutes = 2f;
    private float secondsRemaining;
    private bool isTimerRunning;

    [SerializeField]
    // Speed of Qwiex accrual
    private float QwiexPerSecond = 0.5f;

    // Player's current Qwiex level
    private float currentQwiex = 0f;

    // Start is called before the first frame update
    void Start()
    {
        UIController.SetPlayerInfo(dummyPlayerInfo, dummyOpponentInfo);

        /*
        for(int i=0;i<5;i++)
        {
            UIController.SetCardInfo(dummyCardInfo[i], 4 - i);
        }
        */

        isTimerRunning = false;
        SetTimer(timerMinutes);
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (secondsRemaining > 0 && isTimerRunning)
        {
            secondsRemaining -= Time.deltaTime;
            UIController.UpdateTimerDisplay(secondsRemaining);
        }

        if (currentQwiex < 10f)
        {
            currentQwiex += Time.deltaTime * QwiexPerSecond;

            if (currentQwiex > 10f)
            {
                currentQwiex = 10f;
            }

            UIController.UpdateQwiexDisplay(currentQwiex);
        }
    }

    private void SetTimer(float minutes)
    {
        secondsRemaining = timerMinutes * 60;
    }

    private void StartTimer()
    {
        isTimerRunning = true;
    }

    private void PauseTimer()
    {
        isTimerRunning = false;
    }
}
