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
       (c) Copyright 2022-2023 by Qweix - All Rights Reserved      
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
    [SerializeField] float TowerZone = 10.0f;
    
    public GameObject player1Camera;
    public GameObject player2Camera;

    [SerializeField] private GameObject kingTowerICG;
    [SerializeField] private GameObject kingTowerNecro;
    [SerializeField] private GameObject defTowerICG;
    [SerializeField] private GameObject defTowerNecro;

    [SerializeField] private GameObject teamOneKTSpawnPoint;
    [SerializeField] private GameObject teamTwoKTSpawnPoint;
    [SerializeField] private GameObject teamOneDefSpawnPoint1;
    [SerializeField] private GameObject teamOneDefSpawnPoint2;
    [SerializeField] private GameObject teamTwoDefSpawnPoint1;
    [SerializeField] private GameObject teamTwoDefSpawnPoint2;

    public GameObject player1KT { get; private set; }
    public GameObject player2KT { get; private set; }

    [SerializeField] private GameObject victoryPopUp;
    [SerializeField] private GameObject defeatPopUp;


    // Timer variable for testing purposes
    private NetworkVariable<float> matchTimer = new NetworkVariable<float>(120f);
    public NetworkVariable<bool> matchActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    // CardCores for testing purposes
    public CardCore[] testingCardCores = new CardCore[QwiexHand.HandSize];
    public bool useTestingCardCores;

    // Decks for testing purposes
    public QwiexDeck ironCreekGangTestDeck { get; private set; }
    public QwiexDeck necroMastersTestDeck { get; private set; }

    // Which deck to use for testing
    public CardTribe testTribe;

    [SerializeField] public int localPlayerTeam;

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

        for (int i = 1; i <= cardCoreLibrary.transform.childCount; i++)
        {
            CardCore cardCore = cardCoreLibrary.GetCardCore(i);

            // Only grab cards that have an icon
            if (cardCore.cardPicture != null && cardCore.enableInDeck != false)
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
        if (IsServer)
        {
            matchTimer.Value = 120f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (matchActive.Value == true)
        {
            if (players.Count >= 1)
            {
                if (IsServer)
                {
                    PlayerQwiexManagement();
                }

                qwiexBarUIController.SetQwiexLevel(players[(int)NetworkManager.Singleton.LocalClientId].Qwiex.Value);
                handUIController.UpdateCardAvailability(players[(int)NetworkManager.Singleton.LocalClientId].Qwiex.Value);
                if (IsServer)
                {
                    matchTimer.Value -= Time.deltaTime;
                }
                timerUIController.SetTimer(matchTimer.Value);
            }
        }
    }

    public void PlayerRegister(QwiexPlayer newPlayer)
    {
        if (newPlayer.teamNum.Value == ((int)NetworkManager.Singleton.LocalClientId + 1))
        {
            localPlayerTeam = newPlayer.teamNum.Value;
        }
        Debug.Log("player team number is " + newPlayer.teamNum.Value);
        players.Add(newPlayer);
        SetupPlayerTowers(newPlayer);
        if(newPlayer.teamNum.Value == 2)
        {
            matchActive.Value = true;
        }
    }

    private void PlayerQwiexManagement()
    {
        if (qwiexUpdateTimer > 2.0f)
        {
            foreach (QwiexPlayer player in players)
            {
                if (player.Qwiex.Value < QwiexBarUIController.numberOfQuiexBars)
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
        ServerRpcParams sendingClientID = new ServerRpcParams   {   };
        SpawnUnitServerRpc(cardID, worldDropLocation, sendingClientID);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnUnitServerRpc(int CCNumber, Vector3 clientSpawnLocation, ServerRpcParams serverParams)
    {

        ClientRpcParams currentClientParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new List<ulong> { serverParams.Receive.SenderClientId }
            }
        };

        int ownerId = (int)serverParams.Receive.SenderClientId + 1;

        if (SpawnPointChecker(clientSpawnLocation, ownerId) == true)
        {
            players[(int)serverParams.Receive.SenderClientId].Qwiex.Value -= cardCoreLibrary.GetCardCore(CCNumber).qwiexCost;
            SpawnUnitMethod(CCNumber, clientSpawnLocation, ownerId);
            SpawnResultClientRpc(true, CCNumber, currentClientParams);
        }
        else
        {
            SpawnResultClientRpc(false, CCNumber, currentClientParams);
        }
    }


    [ClientRpc]
    private void SpawnResultClientRpc(bool spawnSuccess, int cardID, ClientRpcParams currentClientID)
    {

        if (spawnSuccess)
        {
            // Remove the card from hand
            players[(int)NetworkManager.Singleton.LocalClientId].playerHand.RemoveCard(cardID);
            players[(int)NetworkManager.Singleton.LocalClientId].playerDeck.AddCard(cardCoreLibrary.GetCardCore(cardID));
            handUIController.RemoveCardFromHand(cardID);

            // Draw a new card
            CardCore drawnCard = players[(int)NetworkManager.Singleton.LocalClientId].playerDeck.DrawCard();
            players[(int)NetworkManager.Singleton.LocalClientId].playerHand.AddCard(drawnCard);
            handUIController.AddCard(drawnCard.cardID, drawnCard.cardPicture, drawnCard.dragSprite, drawnCard.dragSpriteScale, drawnCard.qwiexCost);

            // Set the next card image
            handUIController.SetNextCard(players[(int)NetworkManager.Singleton.LocalClientId].playerDeck.NextCard().cardPicture);

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
        if (spawnedUnit.TryGetComponent<Spell_BaseTriggered_Component>(out Spell_BaseTriggered_Component spell_Component))
        {
            spell_Component.teamCheck = team;
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

    private void SetupPlayerTowers(QwiexPlayer player)
    {
        if (IsServer)
        {
            GameObject KTSpawnPoint;
            GameObject KTPrefab;
            GameObject DFSpawnPoint1;
            GameObject DFSpawnPoint2;
            GameObject DFPrefab;

            if (player.teamNum.Value == 1)
            {
                KTSpawnPoint = teamOneKTSpawnPoint;
                KTPrefab = kingTowerICG;
                DFSpawnPoint1 = teamOneDefSpawnPoint1;
                DFSpawnPoint2 = teamOneDefSpawnPoint2;
                DFPrefab = defTowerICG;
            }
            else
            {
                KTSpawnPoint = teamTwoKTSpawnPoint;
                KTPrefab = kingTowerNecro;
                DFSpawnPoint1 = teamTwoDefSpawnPoint1;
                DFSpawnPoint2 = teamTwoDefSpawnPoint2;
                DFPrefab = defTowerNecro;
            }

            GameObject spawnedKT = SpawnTower(KTPrefab, KTSpawnPoint, player);
            if (player.teamNum.Value == 1)
            {
                player1KT = spawnedKT;
            }

            if (player.teamNum.Value == 2)
            {
                player2KT = spawnedKT;
            }
            SpawnTower(DFPrefab, DFSpawnPoint1, player);
            SpawnTower(DFPrefab, DFSpawnPoint2, player);

        }
    }

    private GameObject SpawnTower(GameObject prefab, GameObject spawnpoint, QwiexPlayer player)
    {
        GameObject spawnedTower = Instantiate(prefab, spawnpoint.transform.position, Quaternion.identity);
        spawnedTower.GetComponent<NetworkObject>().Spawn(true);

        if (spawnedTower.TryGetComponent<Targeting_Component>(out Targeting_Component targeting_Component))
        {
            targeting_Component.teamCheck = player.teamNum.Value;
        }
        return spawnedTower;
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

    private void OnEnable()
    {
        Health_Component.OnUnitDeath += UnitDeath;
    }

    private void OnDisable()
    {
        Health_Component.OnUnitDeath -= UnitDeath;
    }

    //This helps the unit realize its target is dead
    private void UnitDeath(GameObject deadUnit)
    {
        if (deadUnit == player1KT)
        {
            matchActive.Value = false;
            MatchResultClientRpc(2);
        }
        
        if (deadUnit == player2KT)
        {
            matchActive.Value = false;
            MatchResultClientRpc(1);
        }
        
    }


    [ClientRpc]
    private void MatchResultClientRpc(int gameWinner)
    {
        if (((int)NetworkManager.Singleton.LocalClientId + 1) == gameWinner)
        {
            victoryPopUp.SetActive(true);
        }
        else
        {
            defeatPopUp.SetActive(true);
        }
    }
}
