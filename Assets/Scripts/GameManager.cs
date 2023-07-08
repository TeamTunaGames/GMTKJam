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
    [SerializeField] private GameObject playerGroup;
    [SerializeField] private CanvasScript canvas;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<PlayerScript> players = new();
    [SerializeField] private List<int> diceValues = new();
    [SerializeField] private Color red, blue, green, pink;

    [SerializeField] private Tilemap map;
    public Tilemap Map { get { return map; } }
    [SerializeField] private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;

    protected new void Awake()
    {
        base.Awake();
        if (setToDestroy)
            return;

        dataFromTiles = new();

        foreach (TileData tileData in tileDatas)
        {
            foreach (TileBase tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }

        //Get all available players
        players = new List<PlayerScript>();

        for (int i = 0; i < playerGroup.transform.childCount; i++)
        {
            PlayerScript currentPlayer = playerGroup.transform.GetChild(i).GetComponent<PlayerScript>();

            if (currentPlayer.gameObject.activeSelf)
            {
                players.Add(currentPlayer);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PassTurn()
    {
        PlayerScript currentPlayer = players[playerTurn];
        if(gameTurn < diceValues.Count)
            StartCoroutine(currentPlayer.timer(diceValues[gameTurn]));

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
}
