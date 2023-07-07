using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<TileScript> tiles;
    [HideInInspector] public List<PlayerScript> players;
    [HideInInspector] public List<DiceScript> dices;
    public int[] diceValues;
    private int gameTurn;
    private int playerTurn;
    private int tileAdvance;
    private CanvasScript canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<CanvasScript>();
        //Get all tiles
        GameObject tileGroup = GameObject.Find("Tiles");

        for (int i = 0; i < tileGroup.transform.childCount; i++)
        {
            tiles.Add(tileGroup.transform.GetChild(i).GetComponent<TileScript>());
        }

        //Get all players
        GameObject playerGroup = GameObject.Find("Players");

        for (int i = 0; i < playerGroup.transform.childCount; i++)
        {
            players.Add(playerGroup.transform.GetChild(i).GetComponent<PlayerScript>());
            players[i].transform.position = tiles[0].gameObject.transform.position;
        }

        //Get all dices
        GameObject diceGroup = GameObject.Find("Canvas").transform.Find("Dices").gameObject;

        for (int i = 0; i < diceGroup.transform.childCount; i++)
        {
            dices.Add(diceGroup.transform.GetChild(i).GetComponent<DiceScript>());

            DiceScript currentDice = dices[i];
            currentDice.diceValue = diceValues[i];
            currentDice.image.sprite = canvas.diceSprites[currentDice.diceValue - 1];
        }
    }


    void Update()
    {
        //Start next turn
        if (gameTurn != diceValues.Length)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PassTurn();
            }
        }
    }

    private void PassTurn()
    {
        //Move the player and update tiles
        PlayerScript currentPlayer = players[playerTurn];
        tiles[currentPlayer.boardPosition].playerOnTile = null;
        tileAdvance = diceValues[gameTurn];

        while (tiles[currentPlayer.boardPosition+tileAdvance].playerOnTile != null)
        {
            tileAdvance++;
        }

        currentPlayer.boardPosition += tileAdvance;
        tiles[currentPlayer.boardPosition].playerOnTile = currentPlayer;
        
        //Player move animation
        StartCoroutine(currentPlayer.AdvanceTiles(tileAdvance));

        //End turn
        gameTurn++;
        playerTurn++;

        if (playerTurn > players.Count - 1)
        {
            playerTurn = 0;
        }
    }
}
