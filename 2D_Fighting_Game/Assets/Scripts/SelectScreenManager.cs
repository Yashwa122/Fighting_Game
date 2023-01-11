using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectScreenManager : MonoBehaviour
{
    public int numberOfPlayers = 1;
    public List<PlayerInterfaces> plInterfaces = new List<PlayerInterfaces>();
    public PotraitInfo[] potraitPrefabs;
    public int maxX;
    public int maxY;
    PotraitInfo[,] charGrid;

    public GameObject potraitCanvas;

    bool loadLevel;
    public bool bothPlayersSelected;

    CharacterManager charManager;

    #region Singleton
    public static SelectScreenManager instance;
    public static SelectScreenManager GetInstance()
    {
        return instance;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleCharacterPreview(PlayerInterfaces Pl)
    {
        if (Pl.previewPotrait != Pl.activePotrait)
        {
            if (Pl.createdCharacter != null)
            {
                Destroy(Pl.createdCharacter);
            }

            GameObject go = Instantiate(
                CharacterManager.GetInstance().returnCharacterWithID(Pl.activePotrait.characterId).prefab,
                Pl.charVisPos.position,
                Quaternion.identity) as GameObject;

            Pl.createdCharacter = go;

            Pl.previewPotrait = Pl.activePotrait;

            if(!string.Equals(Pl.playerBase.playerId, charManager.players[0].playerId))
            {
                Pl.createdCharacter.GetComponent<StateManager>().lookRight = false;
            }
        }
    }

    [System.Serializable]
    public class PlayerInterfaces
    {
        public PotraitInfo activePotrait;
        public PotraitInfo previewPotrait;
        public GameObject selector;
        public Transform charVisPos;
        public GameObject createdCharacter;

        public int activeX;
        public int activeY;

        public bool hitInputOnce;
        public float timerToReset;

        public PlayerBase playerBase;
    }
}
