#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.Collections.Generic;

public class BoardGeneratorTool : EditorWindow
{
    private GameObject tilePrefab;
    private int numRow = 8;
    private int numColumn = 5;
    private float distanceRatio = 0.05f;

    private BaseBlock[] blocks;

    private string folderPath = "Assets/Prefabs/Color Block Jam/Blocks";

    [MenuItem("Saferio/Board Generator")]
    public static void ShowWindow()
    {
        GetWindow<BoardGeneratorTool>("Board Generator");
    }

    private void OnEnable()
    {
        GameObject[] prefabs = LoadAllPrefabsInFolder(folderPath);

        List<BaseBlock> blockList = new List<BaseBlock>();

        for (int i = 0; i < prefabs.Length; i++)
        {
            BaseBlock block = prefabs[i].GetComponent<BaseBlock>();

            if (block != null)
            {
                blockList.Add(block);
            }
        }

        blocks = blockList.ToArray();
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

        if (GUILayout.Button("Generate Blocks"))
        {
            GenerateBlocks();
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

                position.x = (-(numColumn - 1) / 2f + j) * (1 + distanceRatio) * tileSize.x;
                position.z = ((numRow - 1) / 2f - i) * (1 + distanceRatio) * tileSize.z;

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

    private void GenerateBlocks()
    {
        int totalTile = numRow * numColumn;
        int remainingTile = totalTile;
        float tileDistance = GetTileDistance();

        bool[] isTileFull = new bool[remainingTile];

        int tryTime = 0;
        int maxTryTime = 100;

        BaseBlock selectedPrefab;

        while (remainingTile > 0.2f * totalTile && tryTime < maxTryTime)
        {
            int random = Random.Range(0, blocks.Length);

            selectedPrefab = blocks[random];

            int sizeX = selectedPrefab.BlockProperty.NumTileX;
            int sizeZ = selectedPrefab.BlockProperty.NumTileZ;
            int numTile = sizeX * sizeZ;

            Vector2Int coordinator = new Vector2Int();
            Vector2Int fallbackCoordinator = new Vector2Int(-1, -1);

            bool isFound = false;

            for (int i = 0; i < numRow; i++)
            {
                for (int j = 0; j < numColumn; j++)
                {
                    bool isValid = IsValidArea(j, i, sizeX, sizeZ);

                    if (!isValid)
                    {
                        continue;
                    }

                    if (i - (sizeX - 1) < 0 || i + (sizeX - 1) >= numColumn)
                    {
                        isValid = false;

                        continue;
                    }

                    if (j - (sizeZ - 1) < 0 || j + (sizeZ - 1) >= numRow)
                    {
                        isValid = false;

                        continue;
                    }

                    if (isValid)
                    {
                        int randomIsPicked = Random.Range(0, 5);

                        if (randomIsPicked == 0)
                        {
                            coordinator = new Vector2Int(j, i);

                            isFound = true;

                            break;
                        }
                        else
                        {
                            fallbackCoordinator = new Vector2Int(j, i);
                        }
                    }
                }

                if (isFound)
                {
                    break;
                }
            }

            if (!isFound)
            {
                if (fallbackCoordinator.x >= 0)
                {
                    coordinator = fallbackCoordinator;

                    isFound = true;
                }
            }

            tryTime++;

            if (!isFound)
            {
                continue;
            }

            BaseBlock block = Instantiate(selectedPrefab, Selection.activeTransform);

            Vector3 placedPosition = new Vector3();
            Debug.Log(block.name + "/" + coordinator + "/" + remainingTile);
            placedPosition.x = (-(numColumn - 1) / 2f + (coordinator.x + (sizeX - 1) / 2f)) * tileDistance;
            placedPosition.y = 2;
            placedPosition.z = ((numRow - 1) / 2f - (coordinator.y + (sizeZ - 1) / 2f)) * tileDistance;

            block.transform.position = placedPosition;

            for (int i = coordinator.y; i <= coordinator.y + sizeZ - 1; i++)
            {
                for (int j = coordinator.x; j <= coordinator.x + sizeX - 1; j++)
                {
                    isTileFull[j + numColumn * i] = true;
                }
            }

            remainingTile -= numTile;
        }

        bool IsValidArea(int startX, int startZ, int sizeX, int sizeZ)
        {
            for (int i = startZ; i <= startZ + sizeZ - 1; i++)
            {
                if (i >= numRow)
                {
                    return false;
                }

                for (int j = startX; j <= startX + sizeX - 1; j++)
                {
                    if (j >= numColumn)
                    {
                        return false;
                    }

                    if (isTileFull[j + numColumn * i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    private float GetTileDistance()
    {
        return (1 + distanceRatio) * tilePrefab.GetComponent<MeshRenderer>().bounds.size.x;
    }

    private GameObject[] LoadAllPrefabsInFolder(string folder)
    {
        List<GameObject> prefabs = new List<GameObject>();

        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { folder });

        // if (prefabGUIDs.Length == 0)
        // {
        //     Debug.LogWarning("No prefabs found in folder: " + folder);
        //     return nu;
        // }

        foreach (string guid in prefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null)
            {
                prefabs.Add(prefab);
            }
        }

        return prefabs.ToArray();
    }
}
#endif
