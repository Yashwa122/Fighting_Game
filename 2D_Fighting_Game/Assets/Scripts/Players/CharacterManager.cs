using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int numberOfUsers;
    public List<PlayerBase> players = new List<PlayerBase>();

    public List<CharacterBase> characterList = new List<CharacterBase>();

    public CharacterBase returnCharacterWithID(string id)
    {
        CharacterBase retVal = null;

        for (int i = 0; i < characterList.Count; i++)
        {
            if(string.Equals(characterList[i].charId,id))
            {
                retVal = characterList[i];
            }
        }

        return retVal;
    }

    public PlayerBase returnPlayerFromStates(StateManager states)
    {
        PlayerBase retVal = null;

        for (int i = 0; i < players.Count; i++)
        {
            if(players[i].playerStates == states)
            {
                retVal = players[i];
                break;
            }
        }

        return retVal;
    }

    public static CharacterManager instance;
    internal int numberOfPlayers;

    public static CharacterManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}

[System.Serializable]
public class CharacterBase
{
    public string charId;
    public GameObject prefab;
}

[System.Serializable]
public class PlayerBase
{
    public string playerId;
    public string inputId;
    public PlayerType playerType;
    public bool hasCharacter;
    public GameObject playerPrefab;
    public StateManager playerStates;
    public int score;

    public enum PlayerType
    {
        user,
        ai,
        simulation
    }
}
