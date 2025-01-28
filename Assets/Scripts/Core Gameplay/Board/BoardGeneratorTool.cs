#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class BoardGeneratorTool : EditorWindow
{
    private GameObject tilePrefab;
    private int numRow = 5;
    private int numColumn = 5;
    private float distanceRatio = 1.0f;

    [MenuItem("Saferio/Board Generator")]
    public static void ShowWindow()
    {
        GetWindow<BoardGeneratorTool>("Board Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Board Generator Settings", EditorStyles.boldLabel);

        tilePrefab = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", tilePrefab, typeof(GameObject), false);

        numRow = EditorGUILayout.IntField("Number of Rows", numRow);
        numColumn = EditorGUILayout.IntField("Number of Columns", numColumn);
        distanceRatio = EditorGUILayout.FloatField("Distance Ratio", distanceRatio);

        if (GUILayout.Button("Generate Board"))
        {
            Generate();
        }

        if (GUILayout.Button("Snap"))
        {
            Snap();
        }
    }

    private void Generate()
    {
        int numTile = numRow * numColumn;
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;

        GameObject[] tiles = new GameObject[numTile];

        for (int i = 0; i < numTile; i++)
        {
            tiles[i] = Instantiate(tilePrefab, Selection.activeTransform);
        }

        Vector3 position = new Vector3();

        for (int i = 0; i < numRow; i++)
        {
            for (int j = 0; j < numColumn; j++)
            {
                int tileIndex = j + i * numColumn;

                position.x = (-(numColumn - 1) / 2 + j) * (1 + distanceRatio) * tileSize.x;
                position.z = ((numRow - 1) / 2 - i) * (1 + distanceRatio) * tileSize.z;

                tiles[tileIndex].transform.position = position;
            }
        }

        EditorUtility.SetDirty(Selection.activeTransform);
    }

    private void Snap()
    {
        BaseBlock block = Selection.activeObject.GetComponent<BaseBlock>();

        float tileDistance = GetTileDistance();

        Vector3 position = block.transform.position;

        Vector2 coordinator;

        // covert to bottom-right position
        Vector3 bottomRightPosition = new Vector3();

        bottomRightPosition.x = block.transform.position.x + (block.BlockProperty.NumTileX - 1) / 2f * tileDistance;
        bottomRightPosition.z = block.transform.position.z - (block.BlockProperty.NumTileZ - 1) / 2f * tileDistance;

        coordinator.x = Mathf.Round(bottomRightPosition.x / tileDistance);
        coordinator.y = Mathf.Round(bottomRightPosition.z / tileDistance);

        Vector3 finalPosition = new Vector3(0, 2, 0);

        finalPosition.x = coordinator.x * tileDistance - (block.BlockProperty.NumTileX - 1) / 2f * tileDistance;
        finalPosition.z = coordinator.y * tileDistance + (block.BlockProperty.NumTileZ - 1) / 2f * tileDistance;

        block.transform.position = finalPosition;
    }

    private float GetTileDistance()
    {
        return (1 + distanceRatio) * tilePrefab.GetComponent<MeshRenderer>().bounds.size.x;
    }
}
#endif
