/****************************************************************************
*
*  File              : CardSlot.cs
*  Date Created      : 11/08/2023 
*  Description       : Custom VisualElement for Card drag-and-drop UI
*
*  Programmer(s)     : Gabe Burch
*  Last Modification : 
*  Additional Notes  : 
*  External Documentation URL : 
*****************************************************************************
       (c) Copyright 2022-2023 by MPoweredGames - All Rights Reserved      
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardSlot : VisualElement
{
    public int cardID;
    private VisualElement cardFace;
    private Label cardQwiexCost;
    public bool enabled;
    
    public CardSlot()
    {
        cardID = -1;
        enabled = false;
        AddToClassList("cardSlot");
        RegisterCallback<PointerDownEvent>(OnPointerDown);

        AddUIChildren();
    }

    private void AddUIChildren()
    {
        cardFace = new VisualElement();
        cardFace.name = "CardFace";
        cardFace.AddToClassList("cardFace");
        this.Add(cardFace);

        VisualElement cardBanner = new VisualElement() { name = "CardBanner" };
        cardBanner.AddToClassList("cardBanner");
        cardFace.Add(cardBanner);

        VisualElement cardHalo = new VisualElement() { name = "CardHalo" };
        cardHalo.AddToClassList("cardHalo");
        cardBanner.Add(cardHalo);

        VisualElement cardQwiexCostOrb = new VisualElement() { name = "cardQwiexCostOrb" };
        cardQwiexCostOrb.AddToClassList("cardQwiexCostOrb");
        cardHalo.Add(cardQwiexCostOrb);

        cardQwiexCost = new Label { name = "cardQwiexCost" };
        cardQwiexCost.AddToClassList("cardQwiexCost");
        cardQwiexCostOrb.Add(cardQwiexCost);
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        if(evt.button == 0)
        {
            HandUIController.StartDrag(evt.position, this);

            DisableSlot();
        }
    }

    public void SetCardImage(Texture2D cardTexture)
    {
        cardFace.style.backgroundImage = new StyleBackground(Background.FromTexture2D(cardTexture));
    }

    public void SetQwiexCost(int qwiexCost)
    {
        cardQwiexCost.text = qwiexCost.ToString();
    }

    public void DisableSlot()
    {
        cardFace.style.opacity = 0.5f;
        enabled = false;
    }

    public void EnableSlot()
    {
        cardFace.style.opacity = 1.0f;
        enabled = true;
    }
}
