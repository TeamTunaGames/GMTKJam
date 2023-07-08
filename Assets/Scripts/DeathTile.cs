using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeathTile : VariableTile
{
    public PlayerColor playerColor;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Death Tile")]
    public static void CreateDeathTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Death Tile", "New Death Tile", "Asset", "Save Death Tile", "Assets");
        if (path == string.Empty)
            return;

        AssetDatabase.CreateAsset(CreateInstance<DeathTile>(), path);
    }
#endif

    public override void landed(PlayerScript playerScript)
    {
        if (playerScript.Color == playerColor)
        {
            playerScript.gameObject.SetActive(false);
        }
    }
}

