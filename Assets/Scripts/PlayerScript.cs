using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private string playerName;
    [SerializeField] private PlayerColor color;

    [SerializeField] private TileCheck nextTile = TileCheck.Right;

    private void Start()
    {
        StartCoroutine(timer());
    }

    private IEnumerator timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Vector3Int pos = new((int)transform.position.x, (int)transform.position.y);
            GameManager.Instance.Map.RefreshTile(pos);

            VariableTile tile = GameManager.Instance.Map.GetTile<VariableTile>(pos);

            if (tile.HasAdjacentNeighbor(nextTile))
            {
                transform.position = pos + getDir(nextTile);
            }
            else
            {
                switch (nextTile)
                {
                    case TileCheck.Up:
                        if (tile.HasAdjacentNeighbor(TileCheck.Right) || tile.HasAdjacentNeighbor(TileCheck.Left))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Right) ? TileCheck.Right : TileCheck.Left;  
                        else
                            nextTile = TileCheck.Down;
                        break;
                    case TileCheck.Left:
                        if (tile.HasAdjacentNeighbor(TileCheck.Up) || tile.HasAdjacentNeighbor(TileCheck.Down))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Up) ? TileCheck.Up : TileCheck.Down;
                        else
                            nextTile = TileCheck.Right;
                        break;
                    case TileCheck.Down:
                        if (tile.HasAdjacentNeighbor(TileCheck.Right) || tile.HasAdjacentNeighbor(TileCheck.Left))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Right) ? TileCheck.Right : TileCheck.Left;
                        else
                            nextTile = TileCheck.Up;
                        break;
                    case TileCheck.Right:
                        if (tile.HasAdjacentNeighbor(TileCheck.Up) || tile.HasAdjacentNeighbor(TileCheck.Down))
                            nextTile = tile.HasAdjacentNeighbor(TileCheck.Up) ? TileCheck.Up : TileCheck.Down;
                        else
                            nextTile = TileCheck.Left;
                        break;

                }

                transform.position = pos + getDir(nextTile);
            }
        }

        Vector3Int getDir(TileCheck check)
        {
            switch (check)
            {
                case TileCheck.Left:
                    return Vector3Int.left;
                case TileCheck.Right:
                    return Vector3Int.right;
                case TileCheck.Up:
                    return Vector3Int.up;
                case TileCheck.Down:
                    return Vector3Int.down;
                default:
                    return Vector3Int.down;
            }
        }
    }
}

public enum PlayerColor
{
    Red,
    Blue,
    Green,
    Pink
}