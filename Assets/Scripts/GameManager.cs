using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<PlayerScript> players = new();
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
            foreach(TileBase tile in tileData.tiles)
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
    }
}
