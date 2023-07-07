using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int boardPosition;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public IEnumerator AdvanceTiles(int number)
    {
        //Move by one tile
        number--;

        TileScript currentTile = gameManager.tiles[boardPosition - number];
        transform.position = currentTile.gameObject.transform.position;

        //Move upwards if standing on another player
        if (currentTile.playerOnTile != null && currentTile.playerOnTile != GetComponent<PlayerScript>())
        {
            transform.position += new Vector3(0, 1);
        }

        //Start next move
        if (number > 0)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(AdvanceTiles(number));
        }
    }
}
