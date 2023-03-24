using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DataClasses;

public class UIControl_Battlefield : MonoBehaviour
{
    #region Constants

    [SerializeField]
    private DummyData dummyData;

    [SerializeField]
    private UIDocument UIControlBattlefield;

    private Label timeLabel;

    private Label playerNameLabel;

    private Label opponentNameLabel;

    private VisualElement cardFaceNext;
    private VisualElement cardFace1;
    private VisualElement cardFace2;
    private VisualElement cardFace3;
    private VisualElement cardFace4;

    private Label cardQwiexCostNext;
    private Label cardQwiexCost1;
    private Label cardQwiexCost2;
    private Label cardQwiexCost3;
    private Label cardQwiexCost4;

    // Cards in hand display
    private CardDisplay cardDisplayNext;
    private CardDisplay cardDisplay1;
    private CardDisplay cardDisplay2;
    private CardDisplay cardDisplay3;
    private CardDisplay cardDisplay4;

    private Label qwiexLabel;
    private List<VisualElement> qwiexBar;

    #endregion Constants

    private void Awake()
    {
        var UIRoot = UIControlBattlefield.rootVisualElement;

        timeLabel = UIRoot.Q<Label>("lbl_TimeLeftDisplay");

        playerNameLabel = UIRoot.Q<Label>("lbl_PlayerName");

        opponentNameLabel = UIRoot.Q<Label>("lbl_OpponentName");

        cardQwiexCost1 = UIRoot.Q<Label>("lbl_Card1Cost");
        cardQwiexCost2 = UIRoot.Q<Label>("lbl_Card2Cost");
        cardQwiexCost3 = UIRoot.Q<Label>("lbl_Card3Cost");
        cardQwiexCost4 = UIRoot.Q<Label>("lbl_Card4Cost");

        cardFace1 = UIRoot.Q<VisualElement>("ve_Card1Face");
        cardFace2 = UIRoot.Q<VisualElement>("ve_Card2Face");
        cardFace3 = UIRoot.Q<VisualElement>("ve_Card3Face");
        cardFace4 = UIRoot.Q<VisualElement>("ve_Card4Face");
        cardFaceNext = UIRoot.Q<VisualElement>("ve_CardNextFace");

        qwiexBar = new List<VisualElement>();

        qwiexLabel = UIRoot.Q<Label>("lbl_QwiexTotal");
        qwiexBar.Add(UIRoot.Q<VisualElement>("ve_QwiexBar1"));
        qwiexBar.Add(UIRoot.Q<VisualElement>("ve_QwiexBar2"));
        qwiexBar.Add(UIRoot.Q<VisualElement>("ve_QwiexBar3"));
        qwiexBar.Add(UIRoot.Q<VisualElement>("ve_QwiexBar4"));
        qwiexBar.Add(UIRoot.Q<VisualElement>("ve_QwiexBar5"));
        qwiexBar.Add(UIRoot.Q<VisualElement>("ve_QwiexBar6"));
        qwiexBar.Add(UIRoot.Q<VisualElement>("ve_QwiexBar7"));
        qwiexBar.Add(UIRoot.Q<VisualElement>("ve_QwiexBar8"));
        qwiexBar.Add(UIRoot.Q<VisualElement>("ve_QwiexBar9"));
        qwiexBar.Add(UIRoot.Q<VisualElement>("ve_QwiexBar10"));
    }

    // Should be updated every frame
    public void UpdateTimerDisplay(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timeLabel.text = "" + minutes + ":" + seconds;
    }

    // Should be updated every frame
    public void UpdateQwiexDisplay(float currentQwiex)
    {
        // Iterate through the VisualElements in the Qwiex Bar
        foreach(VisualElement element in qwiexBar)
        {
            // If the current Qwiex level exceeds the index of the current display segment
            if (currentQwiex >= qwiexBar.IndexOf(element) + 1)
            {
                // Set the width of the display segment to full
                element.style.width = Length.Percent(100);
            }
            // If the current Qwiex level is between the index of the last and current
            else if(Mathf.Floor(currentQwiex) == qwiexBar.IndexOf(element))
            {
                // Set the width of the display segment to the proper percentage
                element.style.width = Length.Percent((currentQwiex - Mathf.Floor(currentQwiex)) * 100);
            }
            else
            {
                // Otherwise set the width of the display segment to zero
                element.style.width = Length.Percent(0);
            }
        }

        // Update the Qwiex label
        qwiexLabel.text = Math.Floor(currentQwiex).ToString();
    }

    public void SetPlayerInfo(PlayerInfo user, PlayerInfo opponent)
    {
        playerNameLabel.text = user.playerName;

        opponentNameLabel.text = opponent.playerName;
    }

    // Set card info into card slot
    //     0 = Next
    //     1 = Top
    //     2 = Top-Middle
    //     3 = Bottom-Middle
    //     4 = Bottom
    public void SetCardInfo(CardInfo info, int slot)
    {
        switch(slot)
        {
            case 0:
                cardDisplayNext.displayQwiexCost = info.cardQwiexCost;
                cardDisplayNext.displayTexture = info.cardTexture;
                cardDisplayNext.info = info;
                cardFaceNext.style.backgroundImage = new StyleBackground(Sprite.Create(info.cardTexture, new Rect(0.0f, 0.0f, info.cardTexture.width, info.cardTexture.height), new Vector2(0.5f, 0.5f), 100.0f));
                
                break;

            case 1:
                cardDisplay1.displayQwiexCost = info.cardQwiexCost;
                cardDisplay1.displayTexture = info.cardTexture;
                cardDisplay1.info = info;
                cardFace1.style.backgroundImage = new StyleBackground(Sprite.Create(info.cardTexture, new Rect(0.0f, 0.0f, info.cardTexture.width, info.cardTexture.height), new Vector2(0.5f, 0.5f), 100.0f));
                cardQwiexCost1.text = info.cardQwiexCost.ToString();

                break;

            case 2:
                cardDisplay2.displayQwiexCost = info.cardQwiexCost;
                cardDisplay2.displayTexture = info.cardTexture;
                cardDisplay2.info = info;
                cardFace2.style.backgroundImage = new StyleBackground(Sprite.Create(info.cardTexture, new Rect(0.0f, 0.0f, info.cardTexture.width, info.cardTexture.height), new Vector2(0.5f, 0.5f), 100.0f));
                cardQwiexCost2.text = info.cardQwiexCost.ToString();

                break;

            case 3:
                cardDisplay3.displayQwiexCost = info.cardQwiexCost;
                cardDisplay3.displayTexture = info.cardTexture;
                cardDisplay3.info = info;
                cardFace3.style.backgroundImage = new StyleBackground(Sprite.Create(info.cardTexture, new Rect(0.0f, 0.0f, info.cardTexture.width, info.cardTexture.height), new Vector2(0.5f, 0.5f), 100.0f));
                cardQwiexCost3.text = info.cardQwiexCost.ToString();

                break;

            case 4:
                cardDisplay4.displayQwiexCost = info.cardQwiexCost;
                cardDisplay4.displayTexture = info.cardTexture;
                cardDisplay4.info = info;
                cardFace4.style.backgroundImage = new StyleBackground(Sprite.Create(info.cardTexture, new Rect(0.0f, 0.0f, info.cardTexture.width, info.cardTexture.height), new Vector2(0.5f, 0.5f), 100.0f));
                cardQwiexCost4.text = info.cardQwiexCost.ToString();

                break;

        }
    }

}

// Struct that will be derived from card objects
public struct CardDisplay
{
    public CardInfo info;
    public int displayQwiexCost;
    public Texture displayTexture;
}