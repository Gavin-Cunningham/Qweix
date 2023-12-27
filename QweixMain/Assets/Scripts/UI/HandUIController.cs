/****************************************************************************
*
*  File              : HandUIController.cs
*  Date Created      : 11/08/2023 
*  Description       : Controller for Hand UI
*
*  Programmer(s)     : Gabe Burch
*  Last Modification : 
*  Additional Notes  : -(12/06/23) [Gavin] Added dragSpriteScale
*  External Documentation URL : 
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.UIElements;

public class HandUIController : MonoBehaviour
{
    // List of all card slots
    private List<CardSlot> cardSlots = new List<CardSlot>();

    // References to various UI elements
    private VisualElement uiRoot;
    private static VisualElement ghostIcon;
    private VisualElement nextCardFace;

    // Are we currently dragging a card?
    private static bool isDragging;

    // Reference to the slot that has been dragged from
    private static CardSlot originalCardSlot;

    // Reference to local manager
    public LocalManager localManager;

    private void Awake()
    {
        if(localManager == null)
        {
            Debug.Log("LocalManager reference not set");
        }
        
        // Set reference to the root UI element
        uiRoot = GetComponent<UIDocument>().rootVisualElement;

        // Query the root UI element to find the HandPanel
        VisualElement handPanel = uiRoot.Q<VisualElement>("HandPanel");

        // Create CardSlot UI elements and add them to the list and the HandPanel
        for(int i=0;i<QwiexHand.HandSize;i++)
        {
            CardSlot slot = new CardSlot();

            cardSlots.Add(slot);

            handPanel.Add(slot);
        }

        // Add the NextCard UI elements
        VisualElement nextCardPanel = new VisualElement() { name = "NextCardPanel" };
        nextCardPanel.AddToClassList("nextCardPanel");
        handPanel.Add(nextCardPanel);

        Label nextLabel = new Label() { name = "NextCardPanel", text = "NEXT" };
        nextLabel.AddToClassList("nextLabel");
        nextCardPanel.Add(nextLabel);

        nextCardFace = new VisualElement() { name = "NextCardFace" };
        nextCardFace.AddToClassList("nextCardFace");
        nextCardPanel.Add(nextCardFace);

        // Add the GhostIcon and set its event handlers
        ghostIcon = uiRoot.Q<VisualElement>("GhostIcon");
        ghostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        ghostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    private void Update()
    {
        if (localManager.matchActive.Value == true)
        {
            // IF the mouse button is down and we are dragging
            if (Input.GetMouseButton(0))
            {
                if (isDragging)
                {
                    // Move GhostIcon to cursor position
                    ghostIcon.style.top = (Screen.height - Input.mousePosition.y) - ghostIcon.layout.height / 2;
                    ghostIcon.style.left = Input.mousePosition.x - ghostIcon.layout.width / 2;
                }
            }
        }
        else
        {
            isDragging = false;
        }
    }

    // Called by a PointerDown event on a CardSlot UI element
    public static void StartDrag(Vector2 position, CardSlot originalSlot)
    {
        // Set dragging flag and reference to original card slot
        isDragging = true;

        originalCardSlot = originalSlot;

        Sprite dragSprite = originalCardSlot.dragSprite;
        if (dragSprite != null)
        {
            // Set GhostIcon drag sprite
            ghostIcon.style.backgroundImage = new StyleBackground(Background.FromSprite(originalCardSlot.dragSprite));
            ghostIcon.style.backgroundColor = new Color(1, 1, 1, 0);
            ghostIcon.style.opacity = 0.5f;
            ghostIcon.style.width = (((originalCardSlot.dragSprite.rect.width * (Screen.height / 1080.0f)) / 1.69f) * originalCardSlot.dragSpriteScale);
            ghostIcon.style.height = (((originalCardSlot.dragSprite.rect.height * (Screen.height / 1080.0f)) / 1.69f) * originalCardSlot.dragSpriteScale);
        }
        else
        {
            ghostIcon.style.backgroundImage = null;
            ghostIcon.style.width = 20;
            ghostIcon.style.height = 20;
        }

        // Move GhostIcon to drag position and make it visible
        ghostIcon.style.top = position.y - ghostIcon.layout.height / 2;
        ghostIcon.style.left = position.x - ghostIcon.layout.width / 2;
        ghostIcon.style.visibility = Visibility.Visible;
    }

    // Event handler for PointerMoveEvent
    private void OnPointerMove(PointerMoveEvent evt)
    {
        // If we're dragging, move GhostIcon to drag location
        if(isDragging)
        {
            ghostIcon.style.top = evt.position.y - ghostIcon.layout.height / 2;
            ghostIcon.style.left = evt.position.x - ghostIcon.layout.width / 2;
        }
    }

    // Event handler for PointerUpEvent
    private void OnPointerUp(PointerUpEvent evt)
    {
        // If we were dragging
        if(isDragging)
        {
            // Turn off dragging flag
            isDragging = false;
            
            // Tell the local manager to play the card at the designated location
            localManager.PlayCard(originalCardSlot.cardID, new Vector2(evt.position.x, Screen.height - evt.position.y));

        }
    }

    public void EnableOriginalCardSlot()
    {
        // Enable the original card slot and remove the reference
        originalCardSlot.EnableSlot();
        originalCardSlot = null;

        // Hide the GhostIcon
        ghostIcon.style.visibility = Visibility.Hidden;
    }


    // Called by the local manager to add cards to the UI
    public bool AddCard(int cardID, Texture2D cardTexture, Sprite dragSprite, float dragSpriteScale, int qwiexCost)
    {
        // Have we found a place for the card?
        bool cardPlaced = false;

        // Go through the card slots, looking for an open one
        foreach(CardSlot slot in cardSlots)
        {
            // If we find an open slot and have not yet placed the card
            if(slot.cardID == -1 && !cardPlaced)
            {
                // Place the card
                slot.cardID = cardID;
                slot.SetCardImage(cardTexture);
                slot.SetDragSprite(dragSprite, dragSpriteScale);
                slot.SetQwiexCost(qwiexCost);
                cardPlaced = true;
            }
        }

        return cardPlaced;
    }

    // Called by the local manager to set the Next card image
    public void SetNextCard(Texture2D cardTexture)
    {
        nextCardFace.style.backgroundImage = new StyleBackground(Background.FromTexture2D(cardTexture));
    }

    // Enables/disables each card slot depending on the Qwiex level
    public void UpdateCardAvailability(float qwiexLevel)
    {
        foreach(CardSlot cardSlot in cardSlots)
        {
            if(qwiexLevel < cardSlot.cardQwiexCost)
            {
                cardSlot.DisableSlot();
            }
            else
            {
                cardSlot.EnableSlot();
            }
        }
    }

    // Removes a card from the player's hand UI
    public void RemoveCardFromHand(int cardID)
    {
        foreach(CardSlot cardSlot in cardSlots)
        {
            if(cardSlot.cardID == cardID)
            {
                cardSlot.cardID = -1;
            }
        }
    }

}
