using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Action<bool> OnPause;

    public int gameTurn;
    public int playerTurn;
    public bool gameStarted;
    public bool gamePaused = false;
    [SerializeField] private GameObject playerGroup;
    public GameObject PlayerGroup { set { playerGroup = value; } }
    [SerializeField] public CanvasScript canvas;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<PlayerScript> players = new();
    private Dictionary<PlayerColor, PlayerScript> playersByColor = new();
    [SerializeField] private List<int> diceValues = new();
    [SerializeField] private Color red, blue, green, pink;

    [SerializeField] private Tilemap map;
    public Tilemap Map { get { return map; } set { map = value; } }
    [SerializeField] private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;

    private bool someoneWon = false;

    [SerializeField] private List<SceneReference> levels;
    public int levelNumber = 0;
    public int deadPlayers = 0;

    protected new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        if (setToDestroy)
            return;

        players.Capacity = 4;
        dataFromTiles = new();

        foreach (TileData tileData in tileDatas)
        {
            foreach (TileBase tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }

        
        
    }

    private void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (gameStarted)
            return;

        foreach (GameObject dice in canvas.dices)
        {
            diceValues.Add(dice.GetComponent<DiceScript>().diceValue);
        }

        gameStarted = true;

        PassTurn();
    }

    public void RestartLevel()
    {
        ResetValues();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetValues()
    {
        someoneWon = false;
        gameStarted = false;
        gameTurn = 0;
        playerTurn = 0;
        deadPlayers = 0;
        diceValues.Clear();
    }

    public void PassTurn()
    {
        if (deadPlayers == players.Count)
            LoadNextLevel();
        if (someoneWon)
            return;

        PlayerScript currentPlayer = players[playerTurn];
        if (gameTurn < diceValues.Count)
            StartCoroutine(currentPlayer.Timer(diceValues[gameTurn]));
        else
            TickGameOver();

        gameTurn++;

        if (gameTurn < diceValues.Count)
        {
            do
            {
                playerTurn++;
                playerTurn %= players.Count;
            }
            while (!players[playerTurn].gameObject.activeSelf);
        }
    }

    public void SetPlayer(PlayerScript player)
    {
        playersByColor[player.Color] = player;
        switch (player.Color)
        {
            case PlayerColor.Red:
                players[0] = player;
                break;
            case PlayerColor.Blue:
                players[1] = player;
                break;
            case PlayerColor.Green:
                players[2] = player;
                break;
            case PlayerColor.Yellow:
                players[3] = player;
                break;
        }
    }

    public PlayerScript GetPlayer(PlayerColor color)
    {
        return playersByColor[color];
    }

    public void TickGameOver()
    {
        someoneWon = true;
    }

    public void SetPlayerGroup(GameObject playerGroup)
    {
        this.playerGroup = playerGroup;
        //Get all available players
        players = new List<PlayerScript>();

        for (int i = 0; i < this.playerGroup.transform.childCount; i++)
        {
            PlayerScript currentPlayer = this.playerGroup.transform.GetChild(i).GetComponent<PlayerScript>();

            if (currentPlayer.gameObject.activeSelf)
            {
                players.Add(currentPlayer);
            }
        }
    }

    public void LoadNextLevel()
    {
        ResetValues();
        SceneManager.LoadScene(levels[levelNumber]);
        levelNumber++;
    }

    public void LoadLevel(SceneReference scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadLevel(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            if (!gamePaused)
            {
                Time.timeScale = 0;
                if(MusicManager.Instance != null)
                    MusicManager.Instance.PauseMusic();
                gamePaused = true;
                OnPause?.Invoke(gamePaused);
            }
        }
        else
        {
            if (gamePaused)
            {
                Time.timeScale = 1;
                if (MusicManager.Instance != null)
                    MusicManager.Instance.UnpauseMusic();
                gamePaused = false;
                OnPause?.Invoke(gamePaused);
            }
        }
        
    }


}
