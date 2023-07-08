using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private string playerName;
    [SerializeField] private PlayerColor color;
    public PlayerColor Color { get { return color; } }
    [SerializeField] private TileCheck nextTile = TileCheck.Right;

    private void Start()
    {
        //StartCoroutine(timer());
    }

    public IEnumerator timer(int tileMoves)
    {
        for (int i = 1; i <= tileMoves; i++)
        {
            yield return new WaitForSeconds(0.25f);
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

        Vector3Int pos2 = new((int)transform.position.x, (int)transform.position.y);
        GameManager.Instance.Map.RefreshTile(pos2);

        VariableTile tile2 = GameManager.Instance.Map.GetTile<VariableTile>(pos2);
        tile2.landed(this);

        Vector3Int getDir(TileCheck check)
        {
            return check switch
            {
                TileCheck.Left => Vector3Int.left,
                TileCheck.Right => Vector3Int.right,
                TileCheck.Up => Vector3Int.up,
                TileCheck.Down => Vector3Int.down,
                _ => Vector3Int.down
            };
        }


        yield return new WaitForSeconds(0.25f);
        GameManager.Instance.PassTurn();
    }

    public void SetNewDirection(TileCheck dir)
    {
        nextTile = dir;
    }
}

public enum PlayerColor
{
    Red,
    Blue,
    Green,
    Yellow
}