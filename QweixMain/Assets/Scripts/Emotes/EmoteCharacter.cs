using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Emotes/Emote Character", order = 1)]
[System.Serializable]
public class EmoteCharacter : ScriptableObject
{
    //public string characterName;
    public Sprite characterSprite;

    public enum Team
    {
        ICG,
        Necro
    }

    public Team team;


    public Sprite dissappointedSprite;
    public GameObject dissappointedPrefab;
    public Sprite frustratedSprite;
    public GameObject frustratedPrefab;
    public Sprite laughingSprite;
    public GameObject laughingPrefab;
    public Sprite smugSprite;
    public GameObject smugPrefab;
}
