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

    void  Awake()
    {
        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        charManager = CharacterManager.GetInstance();
        numberOfPlayers = charManager.numberOfPlayers;

        charGrid = new PotraitInfo[maxX, maxY];

        int x = 0;
        int y = 0;

        potraitPrefabs = potraitCanvas.GetComponentsInChildren<PotraitInfo>();

        for (int i = 0; i < potraitPrefabs.Length; i++)
        {
            potraitPrefabs[i].posX += x;
            potraitPrefabs[i].posY += y;

            charGrid[x, y] = potraitPrefabs[i];

            if (x < maxX - 1)
            {
                x++;
            }
            else
            {
                x = 0;
                y++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!loadLevel)
        {
            for (int i = 0; i < plInterfaces.Count; i++)
            {
                if (i < numberOfPlayers)
                {
                    if(Input.GetButtonUp("Fire2" + charManager.players[i].inputId))
                    {
                        plInterfaces[i].playerBase.hasCharacter = false;
                    }

                    if (!charManager.players[i].hasCharacter)
                    {
                        plInterfaces[i].playerBase = charManager.players[i];

                        HandleSelectorPosition(plInterfaces[i]);
                        HandleSelectScreenInput(plInterfaces[i], charManager.players[i].inputId);
                        HandleCharacterPreview(plInterfaces[i]);
                    }
                }
                else
                {
                    charManager.players[i].hasCharacter = true;
                }
            }
        }

        if (bothPlayersSelected)
        {
            Debug.Log("loading");
            StartCoroutine("LoadLevel");
            loadLevel = true;
        }
        else
        {
            if(charManager.players[0].hasCharacter && charManager.players[1].hasCharacter)
            {
                bothPlayersSelected = true;
            }
        }
    }

    void HandleSelectScreenInput(PlayerInterfaces Pl, string playerId)
    {
        #region Grid Navigation

        float vertical = Input.GetAxis("vertical" + playerId);

        if (vertical != 0)
        {
            if (!Pl.hitInputOnce)
            {
                if (vertical > 0)
                {
                    Pl.activeY = (Pl.activeY > 0) ? Pl.activeY - 1 : maxY -1;
                }
                else
                {
                    Pl.activeY = (Pl.activeY < maxY - 1) ? Pl.activeY + 1 : 0;
                }

                Pl.hitInputOnce = true;
            }
        }

        float horizontal = Input.GetAxis("Horizontal" + playerId);

        if (!Pl.hitInputOnce)
        {
            if (horizontal > 0)
            {
                Pl.activeX = (Pl.activeX > 0) ? Pl.activeX - 1 : maxX - 1;
            }
            else
            {
                Pl.activeX = (Pl.activeX < maxX - 1) ? Pl.activeX + 1 : 0;
            }

            Pl.timerToReset = 0;
            Pl.hitInputOnce = true;
        }

        if(vertical == 0 && horizontal == 0)
        {
            Pl.hitInputOnce = false;
        }

        if (Pl.hitInputOnce)
        {
            Pl.timerToReset += Time.deltaTime;

            if(Pl.timerToReset > 0.8f)
            {
                Pl.hitInputOnce = false;
                Pl.timerToReset = 0;
            }
        }

        #endregion

        if (Input.GetButtonUp("Fire1" + playerId))
        {
            Pl.createdCharacter.GetComponentInChildren<Animator>().Play("Kick");

            Pl.playerBase.playerPrefab = charManager.returnCharacterWithID(Pl.activePotrait.characterId).prefab;

            Pl.playerBase.hasCharacter = true;
        }
    }

    void HandleSelectorPosition(PlayerInterfaces Pl)
    {
        Pl.selector.SetActive(true);

        Pl.activePotrait = charGrid[Pl.activeX, Pl.activeY];

        Vector2 selectorPosition = Pl.activePotrait.transform.localPosition;
        selectorPosition = selectorPosition + new Vector2(potraitCanvas.transform.localPosition.x, potraitCanvas.transform.localPosition.y);

        Pl.selector.transform.localPosition = selectorPosition;
    }

    IEnumerator LoadLevel()
    {
        for (int i = 0; i < charManager.players.Count; i++)
        {
            if(charManager.players[i].playerType == PlayerBase.PlayerType.ai)
            {
                if(charManager.players[i].playerPrefab == null)
                {
                    int ranValue = Random.Range(0, potraitPrefabs.Length);

                    charManager.players[i].playerPrefab = charManager.returnCharacterWithID(potraitPrefabs[ranValue].characterId).prefab;

                    Debug.Log(potraitPrefabs[ranValue].characterId);
                }
            }
        }

        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("level", LoadSceneMode.Single);
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