/****************************************************************************
*
*  File              : EmoteUIController.cs
*  Date Created      : 11/18/2023 
*  Description       : Controller for Emote UI
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
using UnityEngine;
using UnityEngine.UIElements;

public class EmoteUIController : MonoBehaviour
{
    // Is the Emote menu open
    private bool emoteMenuOpen;

    // Reference to local manager
    public LocalManager localManager;

    // References to various UI elements
    private VisualElement uiRoot;
    private VisualElement emoteChoices;

    // Emote texture list for testing purposes
    public List<Texture2D> testEmoteTextureList;

    // Start is called before the first frame update
    void Start()
    {
        if (localManager == null)
        {
            Debug.Log("LocalManager reference not set");
        }

        if (testEmoteTextureList == null)
        {
            Debug.Log("EmoteIconList references not set");
        }

        // Emote menu closed by default
        emoteMenuOpen = false;

        // Set reference to the root UI element
        uiRoot = GetComponent<UIDocument>().rootVisualElement;

        AddUIChildren();
    }

    private void AddUIChildren()
    {
        VisualElement emotePanel = uiRoot.Q<VisualElement>("EmotePanel");

        emotePanel.Clear();

        emoteChoices = new VisualElement { name = "EmoteChoices" };
        emoteChoices.AddToClassList("emoteChoices");
        emoteChoices.style.visibility = Visibility.Hidden;
        emotePanel.Add(emoteChoices);

        VisualElement emoteLaughButton = new VisualElement { name = "EmoteLaughButton" };
        emoteLaughButton.AddToClassList("emoteButton");
        emoteLaughButton.RegisterCallback<ClickEvent>(EmoteMenuClick);
        emoteLaughButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(testEmoteTextureList[0]));
        emoteChoices.Add(emoteLaughButton);

        VisualElement emoteSmugButton = new VisualElement { name = "EmoteSmugButton" };
        emoteSmugButton.AddToClassList("emoteButton");
        emoteSmugButton.RegisterCallback<ClickEvent>(EmoteMenuClick);
        emoteSmugButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(testEmoteTextureList[1]));
        emoteChoices.Add(emoteSmugButton);

        VisualElement emoteFrustratedButton = new VisualElement { name = "EmoteFrustratedButton" };
        emoteFrustratedButton.AddToClassList("emoteButton");
        emoteFrustratedButton.RegisterCallback<ClickEvent>(EmoteMenuClick);
        emoteFrustratedButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(testEmoteTextureList[2]));
        emoteChoices.Add(emoteFrustratedButton);

        VisualElement emoteDisappointedButton = new VisualElement { name = "EmoteDisappointedButton" };
        emoteDisappointedButton.AddToClassList("emoteButton");
        emoteDisappointedButton.RegisterCallback<ClickEvent>(EmoteMenuClick);
        emoteDisappointedButton.style.backgroundImage = new StyleBackground(Background.FromTexture2D(testEmoteTextureList[3]));
        emoteChoices.Add(emoteDisappointedButton);

        Button emoteMenuButton = new Button { name = "EmoteMenuButton" };
        emoteMenuButton.AddToClassList("emoteMenuButton");
        emoteMenuButton.RegisterCallback<ClickEvent>(EmoteMenuClick);
        emotePanel.Add(emoteMenuButton);

        VisualElement emoteMenuButtonIcon = new VisualElement { name = "EmoteMenuButtonIcon" };
        emoteMenuButtonIcon.AddToClassList("emoteMenuButtonIcon");
        emoteMenuButton.Add(emoteMenuButtonIcon);
    }

    // Opens or closes the emote menu
    private void EmoteMenuClick(ClickEvent evt)
    {
        if (evt.button == 0)
        {
            if (emoteMenuOpen)
            {
                emoteMenuOpen = false;
                emoteChoices.style.visibility = Visibility.Hidden;
            }
            else
            {
                emoteMenuOpen = true;
                emoteChoices.style.visibility = Visibility.Visible;
            }
        }
    }

    // Emote button was clicked
    private void EmoteButtonClick(ClickEvent evt)
    {
        
    }
}
