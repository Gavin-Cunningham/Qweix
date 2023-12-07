/****************************************************************************
*
*  File              : LocalManager.cs
*  Date Created      : 11/08/2023 
*  Description       : Intermediary between local UI and network server
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
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class LocalManager : NetworkBehaviour
{
    public static LocalManager instance { get; private set; }
    // Reference to CardCore library
    public CardCoreLibrary cardCoreLibrary;

    // References to various UI controllers
    public HandUIController handUIController;
    public QwiexBarUIController qwiexBarUIController;
    public TimerUIController timerUIController;
    public EmoteUIController emoteUIController;
    public float qwiexUpdateTimer;


    private GameObject spawnedUnit = null;
    [SerializeField] float TowerZone = 5.0f;
    
    public GameObject player1Camera;
    public GameObject player2Camera;

    // Timer variable for testing purposes
    private float testTimer;

    // CardCores for testing purposes
    public CardCore[] testingCardCores = new CardCore[QwiexHand.HandSize];
    public bool useTestingCardCores;

    // Decks for testing purposes
    public QwiexDeck ironCreekGangTestDeck { get; private set; }
    public QwiexDeck necroMastersTestDeck { get; private set; }

    // Which deck to use for testing
    public CardTribe testTribe;

    private List<QwiexPlayer> players = new List<QwiexPlayer>();


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        

        if (cardCoreLibrary == null)
        {
            Debug.Log("CardCoreLibrary reference not set");
        }

        if (handUIController == null)
        {
            Debug.Log("HandUIController reference not set");
        }

        if (qwiexBarUIController == null)
        {
            Debug.Log("QwiexBarUIController reference not set");
        }

        if (timerUIController == null)
        {
            Debug.Log("TimerUIController reference not set");
        }

        ironCreekGangTestDeck = new QwiexDeck();
        necroMastersTestDeck = new QwiexDeck();

        for (int i = 1; i < 15; i++)
        {
            CardCore cardCore = cardCoreLibrary.GetCardCore(i);

            // Only grab cards that have an icon
            if (cardCore.cardPicture != null)
            {
                if (cardCore.cardTribe == CardTribe.IronCreekGang)
                {
                    ironCreekGangTestDeck.AddCard(cardCore);
                }
                else if (cardCore.cardTribe == CardTribe.NecroMasters)
                {
                    necroMastersTestDeck.AddCard(cardCore);
                }
            }
        }

        testTimer = 120f;
    }

    // Update is called once per frame
    void Update()
    {
        if (players.Count >= 1)
        {
            if (IsServer)
            {
                PlayerQwiexManagement();
            }
            
            qwiexBarUIController.SetQwiexLevel(players[(int)OwnerClientId].Qwiex.Value);
            handUIController.UpdateCardAvailability(players[(int)OwnerClientId].Qwiex.Value);

            testTimer -= Time.deltaTime;

            timerUIController.SetTimer(testTimer);
        }
    }

    public void PlayerRegister(QwiexPlayer newPlayer)
    {
        players.Add(newPlayer);
    }

    private void PlayerQwiexManagement()
    {
        if (qwiexUpdateTimer > 2.0f)
        {
            foreach (QwiexPlayer player in players)
            {
                if (player.Qwiex.Value <= QwiexBarUIController.numberOfQuiexBars)
                {
                        player.Qwiex.Value += 1.0f;
                }
            }
            qwiexUpdateTimer = 0.0f;
        }

        qwiexUpdateTimer += Time.deltaTime;
    }

    //Have playcard return a bool for whether the card is played or not
    public void PlayCard(int cardID, Vector2 mouseUpLocation)
    {
        if (IsClient)
        {
        // Raycast from the drag-and-drop location through a zero plane
        Ray ray = Camera.main.ScreenPointToRay(mouseUpLocation);
        Plane zeroPlane = new Plane(Vector3.forward, Vector3.zero);
        float distance;
        zeroPlane.Raycast(ray, out distance);

        // Convert the screen drag-and-drop location to world coordinates
        Vector2 dropLocation = ray.GetPoint(distance);
        Vector3 worldDropLocation = new Vector3(dropLocation.x, dropLocation.y, 0.0f);

        // Sends information to the server to spawn the given Unit
        SpawnUnitServerRpc(cardID, worldDropLocation);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnUnitServerRpc(int CCNumber, Vector3 clientSpawnLocation)
    {

        ClientRpcParams currentClientParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new List<ulong> { OwnerClientId }
            }
        };

        int ownerId = (int)OwnerClientId + 1;

        if (SpawnPointChecker(clientSpawnLocation, ownerId) == true)
        {
            SpawnUnitMethod(CCNumber, clientSpawnLocation, ownerId);
            SpawnResultClientRpc(true, CCNumber, currentClientParams);
            players[(int)OwnerClientId].Qwiex.Value -= cardCoreLibrary.GetCardCore(CCNumber).qwiexCost;
            Debug.Log("do the thing" + " " + ownerId);
        }
        else
        {
            SpawnResultClientRpc(false, CCNumber, currentClientParams);
            Debug.Log("dont do the thing" + " " + ownerId);
        }
    }


    [ClientRpc]
    private void SpawnResultClientRpc(bool spawnSuccess, int cardID, ClientRpcParams currentClientID)
    {

        if (spawnSuccess)
        {
            // Remove the card from hand
            players[(int)OwnerClientId].playerHand.RemoveCard(cardID);
            players[(int)OwnerClientId].playerDeck.AddCard(cardCoreLibrary.GetCardCore(cardID));
            handUIController.RemoveCardFromHand(cardID);

            // Draw a new card
            CardCore drawnCard = players[(int)OwnerClientId].playerDeck.DrawCard();
            players[(int)OwnerClientId].playerHand.AddCard(drawnCard);
            handUIController.AddCard(drawnCard.cardID, drawnCard.cardPicture, drawnCard.dragSprite, drawnCard.dragSpriteScale, drawnCard.qwiexCost);

            // Set the next card image
            handUIController.SetNextCard(players[(int)OwnerClientId].playerDeck.NextCard().cardPicture);

        }
        // Allows the card slot in the players hand to be used again
        handUIController.EnableOriginalCardSlot();
    }

    private void SpawnUnitMethod(int unitCCNumber, Vector3 spawnLocation, int team)
    {
        GameObject unitPrefab = cardCoreLibrary.GetCardCore(unitCCNumber).prefabToSpawn;

        spawnedUnit = Instantiate(unitPrefab, spawnLocation, Quaternion.identity);
        //THIS SHOULD END UP ON SERVERSIDE
        spawnedUnit.GetComponent<NetworkObject>().Spawn(true);
        //spawnedUnit.GetComponent<Targeting_Component>().teamCheck = currentTeam;
        if (spawnedUnit.TryGetComponent<Targeting_Component>(out Targeting_Component targeting_Component))
        {
            targeting_Component.teamCheck = team;
        }
    }


    bool SpawnPointChecker(Vector3 spawnPoint, int team)
    {
        GameObject[] Towers;
        bool canSpawn = false;

        if (NavMesh.SamplePosition(spawnPoint, out NavMeshHit hit, .1f, NavMesh.AllAreas))
        {
            //the mouse location was on navmesh
            canSpawn = true;
        }
        else
        {
            //the mouse location was not on navmesh
            canSpawn = false;
            return canSpawn;
        }

        //gather an array of objects with the tags Towers and KingTower
        Towers = FindGameObjectsWithTags(new string[] { "Tower", "KingTower" });

        //loops to check to see if the spawn location is to close to enemy towers
        foreach (GameObject tower in Towers)
        {
            if (team != tower.GetComponent<Targeting_Component>().teamCheck)
            {
                float distance = Vector3.Distance(tower.transform.position, spawnPoint);
                if (distance < TowerZone)
                {
                    canSpawn = false;
                    return canSpawn;
                }
            }
        }

        return canSpawn;
    }


    GameObject[] FindGameObjectsWithTags(params string[] tags)
    {
        var allTowers = new List<GameObject>();

        foreach (string tag in tags)
        {
            allTowers.AddRange(GameObject.FindGameObjectsWithTag(tag).ToList());
        }

        return allTowers.ToArray();
    }
}
