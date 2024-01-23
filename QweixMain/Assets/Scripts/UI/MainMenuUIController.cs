/****************************************************************************
*
*  File              : MainMenuUIController.cs
*  Date Created      : 1/15/2024 
*  Description       : Controller for Main Menu UI
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
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUIController : MonoBehaviour
{
    // Reference to UI Root
    private VisualElement uiRoot;

    // References to UI Panels
    private VisualElement qwiexLogoPanel;
    private VisualElement createGamePanel;
    private VisualElement joinGamePanel;
    private VisualElement inLobbyPanel;
    private VisualElement deckBuildingPanel;

    // List of lobbies
    private List<Lobby> lobbyList;

    // Lobby the user has joined
    private Lobby joinedLobby;

    void Awake()
    {
        // Set reference to the root UI element
        uiRoot = GetComponent<UIDocument>().rootVisualElement;

        // Find UI Panels
        qwiexLogoPanel = uiRoot.Q<VisualElement>("QwiexLogoPanel");
        createGamePanel = uiRoot.Q<VisualElement>("CreateGamePanel");
        joinGamePanel = uiRoot.Q<VisualElement>("JoinGamePanel");
        inLobbyPanel = uiRoot.Q<VisualElement>("InLobbyPanel");
        deckBuildingPanel = uiRoot.Q<VisualElement>("DeckBuildingPanel");

        // Add click handlers for buttons
        Button createGameNavBtn = uiRoot.Q<Button>("CreateGameButton");
        createGameNavBtn.RegisterCallback<ClickEvent>(CreateGameNavClick);

        Button joinGameNavBtn = uiRoot.Q<Button>("JoinGameButton");
        joinGameNavBtn.RegisterCallback<ClickEvent>(JoinGameNavClick);

        Button deckBuildingNavBtn = uiRoot.Q<Button>("DeckBuildingButton");
        deckBuildingNavBtn.RegisterCallback<ClickEvent>(DeckBuildingNavClick);

        Button createLobbyBtn = uiRoot.Q<Button>("CreateLobbyButton");
        createLobbyBtn.RegisterCallback<ClickEvent>(CreateLobbyClick);

        Button startGameBtn = uiRoot.Q<Button>("StartGameButton");
        startGameBtn.RegisterCallback<ClickEvent>(StartGameClick);

        Button leaveLobbyBtn = uiRoot.Q<Button>("LeaveLobbyButton");
        leaveLobbyBtn.RegisterCallback<ClickEvent>(LeaveLobbyClick);

        Button refreshBtn = uiRoot.Q<Button>("RefreshButton");
        refreshBtn.RegisterCallback<ClickEvent>(RefreshClick);
    }

    void Start()
    {
        lobbyList = new List<Lobby>();

        ShowPanel(MenuPanel.QwiexLogo);
    }

    void Update()
    {
        
    }

    // Shows a particular menu panel, hiding the rest
    private void ShowPanel(MenuPanel panel)
    {
        // Hide all panels by default
        qwiexLogoPanel.style.display = DisplayStyle.None;
        createGamePanel.style.display = DisplayStyle.None;
        joinGamePanel.style.display = DisplayStyle.None;
        inLobbyPanel.style.display = DisplayStyle.None;
        deckBuildingPanel.style.display = DisplayStyle.None;

        // Display the appropriate panel
        switch (panel)
        {
            case MenuPanel.QwiexLogo:
                qwiexLogoPanel.style.display = DisplayStyle.Flex;
                break;

            case MenuPanel.CreateGame:
                createGamePanel.style.display = DisplayStyle.Flex;
                break;

            case MenuPanel.JoinGame:
                RefreshJoinGame();
                joinGamePanel.style.display = DisplayStyle.Flex;
                break;

            case MenuPanel.InLobby:
                RefreshInLobby();
                inLobbyPanel.style.display = DisplayStyle.Flex;
                break;

            case MenuPanel.DeckBuilding:
                deckBuildingPanel.style.display = DisplayStyle.Flex;
                break;

        }
    }

    // Click event for Create Game navivation button
    private void CreateGameNavClick(ClickEvent evt)
    {
        ShowPanel(MenuPanel.CreateGame);
    }

    // Click event for Join Game navivation button
    private void JoinGameNavClick(ClickEvent evt)
    {
        ShowPanel(MenuPanel.JoinGame);
    }

    // Click event for Deck Building navivation button
    private void DeckBuildingNavClick(ClickEvent evt)
    {
        ShowPanel(MenuPanel.DeckBuilding);
    }

    // Click event for Create Lobby button
    private void CreateLobbyClick(ClickEvent evt)
    {
        TextField lobbyNameField = uiRoot.Q<TextField>("LobbyNameTextField");

        // Make sure the user has entered their name and a lobby name
        if (GetPlayerName() != "" && lobbyNameField.value != "")
        {
            // Create a new player object and set its values
            LobbyPlayer player = new LobbyPlayer();
            player.ready = false;

            player.lobbyPlayerName = GetPlayerName();

            DropdownField factionField = uiRoot.Q<DropdownField>("FactionChoiceDropdownField");
            player.lobbyPlayerFaction = factionField.value;

            // TEMPORARY: Player 1 is ICG by default
            player.lobbyPlayerFaction = "Iron Creek Gang";

            // Create a new lobby object and set its values
            Lobby lobby = new Lobby();
            lobby.player1 = player;

            lobby.lobbyName = lobbyNameField.value;

            DropdownField mapField = uiRoot.Q<DropdownField>("MapChoiceDropdownField");
            lobby.lobbyMap = mapField.value;

            // Add Set this lobby as the joined lobby and add it to the list
            joinedLobby = lobby;
            lobbyList.Add(lobby);

            // Add the lobby panel to the UI
            AddLobbyPanel(lobby);

            // Navigate to the InLobby menu panel
            ShowPanel(MenuPanel.InLobby);

            // Hide the navigation panel
            DisplayNavigationPanel(false);
        }
    }

    // Adds a lobby panel to the Join Game scrollview
    private void AddLobbyPanel(Lobby lobby)
    {
        // Load the lobby panel asset file and instantiate one
        VisualTreeAsset lobbyPanelAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/Templates/LobbyPanel.uxml");
        VisualElement lobbyPanel = lobbyPanelAsset.Instantiate();
        lobbyPanel.name = "LobbyPanel";

        // Set text values
        Label lobbyNameLabel = lobbyPanel.Q<Label>("LobbyNameLabel");
        lobbyNameLabel.text = lobby.lobbyName;

        Label lobbyMapLabel = lobbyPanel.Q<Label>("LobbyMapLabel");
        lobbyMapLabel.text = lobby.lobbyMap;

        // Add handler for Join Lobby button
        Button joinLobbyButton = lobbyPanel.Q<Button>("JoinLobbyButton");
        joinLobbyButton.RegisterCallback<ClickEvent>(JoinLobbyClick);

        Label lobbyPlayerCountLabel = lobbyPanel.Q<Label>("LobbyPlayerCountLabel");
        if(lobby.player1 == null || lobby.player2 == null)
        {
            lobbyPlayerCountLabel.text = "1/2";
            joinLobbyButton.style.visibility = Visibility.Visible;
        }
        else
        {
            lobbyPlayerCountLabel.text = "2/2";
            joinLobbyButton.style.visibility = Visibility.Hidden;
        }

        // Find the lobby scrollview and add the lobby panel to it
        VisualElement lobbyScrollView = uiRoot.Q<VisualElement>("LobbyScrollView");
        lobbyScrollView.Add(lobbyPanel);
    }

    // Joins a lobby
    private void JoinLobbyClick(ClickEvent evt)
    {
        // Get the button that was clicked
        Button joinLobbyButton = evt.currentTarget as Button;

        // Traverse the UI hierarchy to find the name of the associated lobby
        string lobbyName = joinLobbyButton.parent.parent.Q<Label>("LobbyNameLabel").text;

        // Find the lobby with the matching name in the list
        foreach(Lobby lobby in lobbyList)
        {
            if (lobby.lobbyName == lobbyName)
            {
                // If the player is not already in the lobby and Player 2 slot is not filled, add them as Player 2
                if (lobby.player1.lobbyPlayerName != GetPlayerName() && lobby.player2 == null)
                {
                    LobbyPlayer newPlayer = new LobbyPlayer();

                    newPlayer.lobbyPlayerName = GetPlayerName();

                    // TODO: Get faction from deck

                    // TEMPORARY: Player 2 is Necro Masters by default
                    newPlayer.lobbyPlayerFaction = "Necro Masters";
                    lobby.player2 = newPlayer;

                    joinedLobby = lobby;
                }

                ShowPanel(MenuPanel.InLobby);

                DisplayNavigationPanel(false);
            }
        }
    }

    // Refreshes the In Lobby page
    private void RefreshInLobby()
    {
        // If there is a joined Lobby to display
        if (joinedLobby != null)
        {
            // Set Lobby Title text
            uiRoot.Q<Label>("LobbyTitleLabel").text = joinedLobby.lobbyName;

            VisualElement player1Panel = uiRoot.Q<VisualElement>("LobbyPlayer1Panel");

            // If Player 1 has left the lobby, promote Player 2 to Player 1
            if (joinedLobby.player1 == null)
            {
                joinedLobby.player1 = joinedLobby.player2;

                // TEMPORARY: Player 1 is ICG by default
                joinedLobby.player1.lobbyPlayerFaction = "Iron Creek Gang";

                joinedLobby.player2 = null;
            }

            // Set Player 1 name and faction text
            player1Panel.Q<Label>("LobbyPlayerNameLabel").text = joinedLobby.player1.lobbyPlayerName;
            player1Panel.Q<Label>("LobbyPlayerFactionLabel").text = joinedLobby.player1.lobbyPlayerFaction;

            // Show or hide Player 1 ready flags
            if (joinedLobby.player1.ready == true)
            {
                player1Panel.Q<Label>("LobbyPlayerReadyLabel").style.visibility = Visibility.Visible;
            }
            else
            {
                player1Panel.Q<Label>("LobbyPlayerReadyLabel").style.visibility = Visibility.Hidden;
            }

            VisualElement player2Panel = uiRoot.Q<VisualElement>("LobbyPlayer2Panel");

            // If Player 2 is gone set the Player 2 text to default
            if (joinedLobby.player2 == null)
            {
                player2Panel.Q<Label>("LobbyPlayerNameLabel").text = "(waiting)";

                player2Panel.Q<Label>("LobbyPlayerFactionLabel").text = "";

                player2Panel.Q<Label>("LobbyPlayerReadyLabel").style.visibility = Visibility.Hidden;
            }
            else
            {
                // Set Player 2 name and faction text
                player2Panel.Q<Label>("LobbyPlayerNameLabel").text = joinedLobby.player2.lobbyPlayerName;

                player2Panel.Q<Label>("LobbyPlayerFactionLabel").text = joinedLobby.player2.lobbyPlayerFaction;

                // Show or hide Player 2 ready flags
                if (joinedLobby.player2.ready == true)
                {
                    player2Panel.Q<Label>("LobbyPlayerReadyLabel").style.visibility = Visibility.Visible;
                }
                else
                {
                    player2Panel.Q<Label>("LobbyPlayerReadyLabel").style.visibility = Visibility.Hidden;
                }
            }
        }
    }

    // Refreshes the Join Lobby page
    private void RefreshJoinGame()
    {
        VisualElement lobbyScrollView = uiRoot.Q<ScrollView>("LobbyScrollView");

        lobbyScrollView.Clear();

        foreach(Lobby lobby in lobbyList)
        {
            AddLobbyPanel(lobby);
        }
    }

    // Sets a LobbyPlayer's ready status
    // Starts the game if both players are ready
    private void StartGameClick(ClickEvent evt)
    {
        // Find out who clicked the button and set their ready flag, then refresh the In Lobby
        if(joinedLobby.player1.lobbyPlayerName == GetPlayerName())
        {
            joinedLobby.player1.ready = true;
        }
        else if(joinedLobby.player2.lobbyPlayerName == GetPlayerName())
        {
            joinedLobby.player2.ready = true;
        }

        RefreshInLobby();

        // If both players are ready
        if (joinedLobby.player1.ready && joinedLobby.player2 != null)
        {
            if (joinedLobby.player2.ready)
            {   
                // Remove the lobby from the list
                lobbyList.Remove(joinedLobby);

                // Go to the next scene and start the game

            }
        }
    }

    // Leaves the Lobby and refreshes the In Lobby page
    // Deletes lobby if no players are in it
    private void LeaveLobbyClick(ClickEvent evt)
    {
        // If Player 1 is leaving the lobby
        if (joinedLobby.player1.lobbyPlayerName == GetPlayerName())
        {
            // If there is a second player, promote them to Player 1
            if (joinedLobby.player2 != null)
            {
                joinedLobby.player1 = joinedLobby.player2;
                joinedLobby.player2 = null;

                // TEMPORARY: Player 1 is ICG by default
                joinedLobby.player1.lobbyPlayerFaction = "Iron Creek Gang";
            }
            // If there is no second player, remove the Lobby from the list
            else
            {
                lobbyList.Remove(joinedLobby);
                joinedLobby = null;
            }
        }
        // If player 2 is leaving the lobby, set Player 2 to null
        else if (joinedLobby.player2.lobbyPlayerName == GetPlayerName())
        {
            joinedLobby.player2 = null;
        }

        // If the lobby is still in the list, refresh the In Lobby page
        if(lobbyList.Contains(joinedLobby))
        {
            RefreshInLobby();
        };

        // Send the user back to the Qwiex Logo page
        DisplayNavigationPanel(true);

        ShowPanel(MenuPanel.QwiexLogo);
    }

    // Gets the player's name from the PlayerName textbox
    private string GetPlayerName()
    {
        return uiRoot.Q<TextField>("PlayerNameTextField").value;
    }

    //Displays or hides the navigation panel
    private void DisplayNavigationPanel(bool display)
    {
        VisualElement menuBarPanel = uiRoot.Q<VisualElement>("MenuBarPanel");

        if(display)
        {
            menuBarPanel.style.visibility = Visibility.Visible;
        }
        else
        {
            //menuBarPanel.style.visibility = Visibility.Hidden;
        }
    }

    // Refreshes the Lobbies
    private void RefreshClick(ClickEvent evt)
    {
        RefreshJoinGame();
        RefreshInLobby();
    }
}

// Different menu panels to display
public enum MenuPanel { QwiexLogo, CreateGame, JoinGame, InLobby, DeckBuilding };


public class Lobby
{
    public string lobbyName;
    public string lobbyMap;
    public LobbyPlayer player1;
    public LobbyPlayer player2;
}

public class LobbyPlayer
{
    public string lobbyPlayerName;
    public string lobbyPlayerFaction;
    public bool ready;
}