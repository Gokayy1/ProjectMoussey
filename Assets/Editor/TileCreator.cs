using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class TileCreator
{
    [MenuItem("Assets/Create/2D/Tile (Manual)")]
    public static void CreateSimpleTile()
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        AssetDatabase.CreateAsset(tile, "Assets/NewSimpleTile.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = tile;
    }
}
