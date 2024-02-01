using Mono.CSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteUI : MonoBehaviour
{
    public static EmoteUI instance { get; private set; }

    [SerializeField] public List<EmoteCharacter> characters = new List<EmoteCharacter>();
    public List<EmoteCharacter> LocalPlayerEmotes = null;

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

    private void SortEmotesByTeam()
    {
        foreach (EmoteCharacter character in characters)
        {
            if (character.team == EmoteCharacter.Team.ICG && localPlayerTeam == 1)
            {
                LocalPlayerEmotes.Add(character);
            }
            else if (character.team == EmoteCharacter.Team.Necro && localPlayerTeam == 2)
            {
                LocalPlayerEmotes.Add(character);
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
            SetButtonSprite(emoteButton1, LocalPlayerEmotes[0].characterSprite);
            SetButtonSprite(emoteButton2, LocalPlayerEmotes[1].characterSprite);
            SetButtonSprite(emoteButton3, LocalPlayerEmotes[2].characterSprite);
            if (LocalPlayerEmotes.Count >= 4)
            {
                SetButtonSprite(emoteButton4, LocalPlayerEmotes[3].characterSprite);
            }

        }
    }

    private void EmoteButtonSelect(int index)
    {
        if (selectedCharacter == null)
        {
            selectedCharacter = LocalPlayerEmotes[index];
        }
        else
        {
            switch (index)
            {
                case 0:
                    SpawnEmoteSprite(selectedCharacter.dissappointedPrefab);
                    break;
                case 1:
                    SpawnEmoteSprite(selectedCharacter.frustratedPrefab);
                    break;
                case 2:
                    SpawnEmoteSprite(selectedCharacter.laughingPrefab);
                    break;
                case 3:
                    SpawnEmoteSprite(selectedCharacter.smugPrefab);
                    break;
            }
            selectedCharacter = null;
            ShowHideButtons(false);
        }
    }

    private void EmoteMenuButtonClick()
    {
        InitializeEmoteUI();

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
        if (emoteUIInitialized)
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

    private void SpawnEmoteSprite(GameObject emotePrefab)
    {

    }
}
