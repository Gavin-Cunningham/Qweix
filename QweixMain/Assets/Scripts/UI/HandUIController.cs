/****************************************************************************
*
*  File              : HandUIController.cs
*  Date Created      : 11/08/2023 
*  Description       : Controller for Hand UI
*
*  Programmer(s)     : Gabe Burch
*  Last Modification : 
*  Additional Notes  : 
*  External Documentation URL : 
*****************************************************************************
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
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

    // The Camera that the player is using
    [SerializeField] private Camera localPlayerCamera;
    private Vector2 playerCameraOffset;

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

        //retrieve offset of the Camera
        playerCameraOffset = new Vector2 (localPlayerCamera.transform.position.x, localPlayerCamera.transform.position.y);
    }

    private void Update()
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

    // Called by a PointerDown event on a CardSlot UI element
    public static void StartDrag(Vector2 position, CardSlot originalSlot)
    {
        // Set dragging flag and reference to original card slot
        isDragging = true;

        originalCardSlot = originalSlot;

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

            // Enable the original card slot and remove the reference
            originalCardSlot.EnableSlot();
            originalCardSlot = null;

            // Hide the GhostIcon
            ghostIcon.style.visibility = Visibility.Hidden;
        }
    }

    // Called by the local manager to add cards to the UI
    public bool AddCard(int cardID, Texture2D cardTexture, int qwiexCost)
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
