using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int gameTurn;
    public int playerTurn;
    public bool gameStarted;
    public bool gameEnded;
    [SerializeField] private GameObject playerGroup;
    public GameObject PlayerGroup { set { playerGroup = value; } }
    [SerializeField] public CanvasScript canvas;
    [SerializeField] private GameObject playerPrefab;
    private Dictionary<PlayerColor, PlayerScript> playersByColor = new();
    [SerializeField] private List<PlayerScript> players = new();
    [SerializeField] private List<int> diceValues = new();
    [SerializeField] private Color red, blue, green, pink;

    [SerializeField] private Tilemap map;
    public Tilemap Map { get { return map; } set { map = value; } }
    [SerializeField] private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;

    private bool someoneWon = false;

    protected new void Awake()
    {
        base.Awake();
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            VariableTile clickedTile = map.GetTile<VariableTile>(gridPosition);

            map.RefreshTile(gridPosition);
            if (clickedTile != null)
                clickedTile.PrintData();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (CanvasScript.Instance.paused)
            {
                CanvasScript.Instance.Unpause();
            }
            else
            {
                CanvasScript.Instance.Pause();
            }
        }

        if (CheckWinCondition() && !gameEnded)
        {
            gameEnded = true;
            StartCoroutine(CanvasScript.Instance.WinAnimation());
        }
    }

    public void StartGame()
    {
        foreach (GameObject dice in canvas.dices)
        {
            diceValues.Add(dice.GetComponent<DiceScript>().diceValue);
        }

        gameStarted = true;

        PassTurn();
    }

    public void RestartLevel()
    {
        someoneWon = false;
        gameTurn = 0;
        playerTurn = 0;
        diceValues.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PassTurn()
    {
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

    private bool CheckWinCondition()
    {
        foreach (PlayerScript player in players)
        {
            if (player.gameObject.activeSelf)
            {
                return false;
            }
        }
        return true;
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
}
