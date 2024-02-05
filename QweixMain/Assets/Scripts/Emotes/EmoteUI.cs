using Mono.CSharp;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class EmoteUI : NetworkBehaviour
{
    public static EmoteUI instance { get; private set; }

    [SerializeField] public List<EmoteCharacter> characters = new List<EmoteCharacter>();
    public List<int> LocalPlayerEmotes = null;

    [SerializeField] private Button emoteMenuButton;
    [SerializeField] private Button emoteReturnButton;
    [SerializeField] private Button emoteButton1;
    [SerializeField] private Button emoteButton2;
    [SerializeField] private Button emoteButton3;
    [SerializeField] private Button emoteButton4;

    public EmoteCharacter selectedCharacter = null;
    public bool EmoteMenuOpen = false;
    public bool emoteUIInitialized = false;
    public int localPlayerTeam = 0;
    //public Vector3 localPlayerKTLoc;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        emoteMenuButton.onClick.AddListener(() =>
        {
            EmoteMenuButtonClick();
        });
        emoteReturnButton.onClick.AddListener(() =>
        {
            EmoteReturnButtonClick();
        });
        emoteButton1.onClick.AddListener(() =>
        {
            EmoteButtonSelect(0);
        });
        emoteButton2.onClick.AddListener(() =>
        {
            EmoteButtonSelect(1);
        });
        emoteButton3.onClick.AddListener(() =>
        {
            EmoteButtonSelect(2);
        });
        emoteButton4.onClick.AddListener(() =>
        {
            EmoteButtonSelect(3);
        });
    }

    private void Start()
    {
        ShowHideButtons(false);
    }

    private void Update()
    {
        if (!emoteUIInitialized)
        {
            if (LocalManager.instance.matchActive.Value == true)
            {
                InitializeEmoteUI();
                //SetLocalKingTower();
            }
        }
    }

    private void SortEmotesByTeam()
    {
        foreach (EmoteCharacter character in characters)
        {
            if (character.team == EmoteCharacter.Team.ICG && localPlayerTeam == 1)
            {
                LocalPlayerEmotes.Add(characters.FindIndex(c => c.characterSprite == character.characterSprite));
            }
            else if (character.team == EmoteCharacter.Team.Necro && localPlayerTeam == 2)
            {
                LocalPlayerEmotes.Add(characters.FindIndex(c => c.characterSprite == character.characterSprite));
            }
        }
    }
    
    private void SetButtonSprite(Button button, Sprite sprite)
    {
        button.GetComponent<Image>().sprite = sprite;
    }

    private void SetAllButtonSprites()
    {
        if (selectedCharacter != null)
        {
            SetButtonSprite(emoteButton1, selectedCharacter.dissappointedSprite);
            SetButtonSprite(emoteButton2, selectedCharacter.frustratedSprite);
            SetButtonSprite(emoteButton3, selectedCharacter.laughingSprite);
            SetButtonSprite(emoteButton4, selectedCharacter.smugSprite);
        }
        else if (selectedCharacter == null)
        {
            SetButtonSprite(emoteButton1, characters[LocalPlayerEmotes[0]].characterSprite);
            SetButtonSprite(emoteButton2, characters[LocalPlayerEmotes[1]].characterSprite);
            SetButtonSprite(emoteButton3, characters[LocalPlayerEmotes[2]].characterSprite);
            if (LocalPlayerEmotes.Count >= 4)
            {
                SetButtonSprite(emoteButton4, characters[LocalPlayerEmotes[3]].characterSprite);
            }

        }
    }

    private void EmoteButtonSelect(int index)
    {
        if (selectedCharacter == null)
        {
            selectedCharacter = characters[LocalPlayerEmotes[index]];
            ShowHideButtons(true);
            SetAllButtonSprites();
        }
        else
        {
            ServerRpcParams sendingClientID = new ServerRpcParams { };
            switch (index)
            {
                case 0:
                    SpawnEmoteServerRPC(characters.FindIndex(c => c.characterSprite == selectedCharacter.characterSprite), 0, sendingClientID);
                    break;
                case 1:
                    SpawnEmoteServerRPC(characters.FindIndex(c => c.characterSprite == selectedCharacter.characterSprite), 1, sendingClientID);
                    break;
                case 2:
                    SpawnEmoteServerRPC(characters.FindIndex(c => c.characterSprite == selectedCharacter.characterSprite), 2, sendingClientID);
                    break;
                case 3:
                    SpawnEmoteServerRPC(characters.FindIndex(c => c.characterSprite == selectedCharacter.characterSprite), 3, sendingClientID);
                    break;
            }
            selectedCharacter = null;
            ShowHideButtons(false);
            EmoteMenuOpen = false;
        }
    }

    private void EmoteMenuButtonClick()
    {
        selectedCharacter = null;

        if (EmoteMenuOpen == false)
        {
            EmoteMenuOpen = true;
            ShowHideButtons(true, FourthCharacter());
            SetAllButtonSprites();
        }
        else
        {
            EmoteMenuOpen = false;
            ShowHideButtons(false);
        }
    }

    private void InitializeEmoteUI()
    {
        if (localPlayerTeam == 0)
        {
            Debug.Log("Emotes: Local Player was: " + localPlayerTeam);
            localPlayerTeam = LocalManager.instance.localPlayerTeam;
            Debug.Log("Emotes: Local Player is now: " + localPlayerTeam);
        }

        if (localPlayerTeam == 0) { return; }

        Debug.Log("Emotes: Local Player is no longer 0");

        emoteUIInitialized = true;

        SortEmotesByTeam();
    }

    private void EmoteReturnButtonClick()
    {
        if (selectedCharacter != null)
        {
            selectedCharacter = null;
            SetAllButtonSprites();
        }
        else if (selectedCharacter == null)
        {
            ShowHideButtons(false);
            EmoteMenuOpen = false;
        }
    }

    private void ShowHideButtons(bool state)
    {
        emoteReturnButton.gameObject.SetActive(state);
        emoteButton1.gameObject.SetActive(state);
        emoteButton2.gameObject.SetActive(state);
        emoteButton3.gameObject.SetActive(state);
        emoteButton4.gameObject.SetActive(state);
    }

    private void ShowHideButtons(bool state, bool showFourthButton)
    {
        emoteReturnButton.gameObject.SetActive(state);
        emoteButton1.gameObject.SetActive(state);
        emoteButton2.gameObject.SetActive(state);
        emoteButton3.gameObject.SetActive(state);
        if(showFourthButton) 
        {
            emoteButton4.gameObject.SetActive(state);
        }
        else
        {
           emoteButton4.gameObject.SetActive(false);
        }
    }

    private bool FourthCharacter()
    {
        if (LocalPlayerEmotes.Count >= 4) { return true; }
        else { return false; }
    }

    //private void SetLocalKingTower()
    //{
    //    if (localPlayerTeam == 1)
    //    {
    //        localPlayerKTLoc = LocalManager.instance.player1KT.transform.position;
    //    }
    //    else if (localPlayerTeam == 2)
    //    {
    //        localPlayerKTLoc = LocalManager.instance.player2KT.transform.position;
    //    }
    //}

    //private void SpawnEmoteSprite(GameObject emotePrefab)
    //{

    //}

    [ServerRpc(RequireOwnership = false)]
    private void SpawnEmoteServerRPC(int emoteIndex, int emoteType, ServerRpcParams serverParams)
    {
        Vector3 playerEmoteKtLoc = new Vector3(0f, 0f, 0f);

        int senderClientId = (int)serverParams.Receive.SenderClientId;

        if (senderClientId == 0)
        {
            playerEmoteKtLoc = LocalManager.instance.player1KT.transform.position;
        }
        else if (senderClientId == 1)
        {
            playerEmoteKtLoc = LocalManager.instance.player2KT.transform.position;
        }

        GameObject emotePrefab = null;

        switch (emoteType)
        {
            case 0:
                {
                    emotePrefab = characters[emoteIndex].dissappointedPrefab;
                    break;
                }
            case 1:
                {
                    emotePrefab = characters[emoteIndex].frustratedPrefab;
                    break;
                }
            case 2:
                {
                    emotePrefab = characters[emoteIndex].laughingPrefab;
                    break;
                }
            case 3:
                {
                    emotePrefab = characters[emoteIndex].smugPrefab;
                    break;
                }
        }

        GameObject emote = Instantiate(emotePrefab, new Vector3(playerEmoteKtLoc.x, playerEmoteKtLoc.y + 4, 0.0f), new Quaternion(0, 0, 0, 0));
        emote.GetComponent<NetworkObject>().Spawn(true);
    }
}
