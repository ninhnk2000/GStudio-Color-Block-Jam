#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.Collections.Generic;
using static GameEnum;
using Dreamteck.Utilities;
using UnityEngine.SceneManagement;

public class BoardGeneratorTool : EditorWindow
{
    private GameObject tilePrefab;
    private int numRow = 8;
    private int numColumn = 5;
    private float distanceRatio = 0f;

    private BaseBlock[] blocks;
    private BaseBarricade[] barricades;
    private Transform blockContainer;

    private string folderPath = "Assets/Prefabs/Color Block Jam/Blocks";
    private string barricadeFolderPath = "Assets/Prefabs/Color Block Jam/Barricades";

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


        LoadBarricadePrefabs();
    }

    private void LoadBarricadePrefabs()
    {
        GameObject[] prefabs = LoadAllPrefabsInFolder(barricadeFolderPath);

        List<BaseBarricade> barricadeList = new List<BaseBarricade>();

        for (int i = 0; i < prefabs.Length; i++)
        {
            BaseBarricade barricade = prefabs[i].GetComponent<BaseBarricade>();

            if (barricade != null)
            {
                barricadeList.Add(barricade);
            }
        }

        barricades = barricadeList.ToArray();
    }

    private void OnGUI()
    {
        GUILayout.Label("Board Generator Settings", EditorStyles.boldLabel);

        tilePrefab = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", tilePrefab, typeof(GameObject), false);

        numRow = EditorGUILayout.IntField("Number of Rows", numRow);
        numColumn = EditorGUILayout.IntField("Number of Columns", numColumn);
        distanceRatio = EditorGUILayout.FloatField("Distance Ratio", distanceRatio);

        blockContainer = (Transform)EditorGUILayout.ObjectField("Block Container", blockContainer, typeof(Transform));

        if (GUILayout.Button("Snap"))
        {
            SnapUsingRaycast(Selection.activeTransform);
        }

        if (GUILayout.Button("Snap All"))
        {
            SnapAll();
        }

        if (GUILayout.Button("Generate Board"))
        {
            GenerateBoard(Selection.activeTransform);
        }

        if (GUILayout.Button("Generate Blocks"))
        {
            GenerateBlocks(Selection.activeTransform);
        }

        if (GUILayout.Button("Generate Barricades"))
        {
            GenerateBarricades(Selection.activeTransform, blockContainer);
        }

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 16;
        buttonStyle.padding = new RectOffset(20, 20, 20, 20);
        buttonStyle.fixedHeight = 60;

        if (GUILayout.Button("Generate Full Level", buttonStyle))
        {
            GenerateFullLevel();
        }
    }

    private void GenerateFullLevel()
    {
        Transform level = Selection.activeTransform;

        tilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Color Block Jam/Board/Tile.prefab");

        GameObject TileContainerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Color Block Jam/Containers/Tile Container.prefab");
        GameObject BlockContainerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Color Block Jam/Containers/Block Container.prefab");
        GameObject BarricadeContainerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Color Block Jam/Containers/Barricade Container.prefab");

        // Remove all existing blocks
        List<GameObject> toRemoveList = new List<GameObject>();

        for (int i = 0; i < level.childCount; i++)
        {
            toRemoveList.Add(level.GetChild(i).gameObject);
        }

        foreach (var item in toRemoveList)
        {
            DestroyImmediate(item);
        }

        Transform tileContainer = ((GameObject)PrefabUtility.InstantiatePrefab(TileContainerPrefab, level)).transform;
        Transform blockContainer = ((GameObject)PrefabUtility.InstantiatePrefab(BlockContainerPrefab, level)).transform;
        Transform barricadeContainer = ((GameObject)PrefabUtility.InstantiatePrefab(BarricadeContainerPrefab, level)).transform;

        GenerateBoard(tileContainer);
        GenerateBlocks(blockContainer);
        GenerateBarricades(barricadeContainer, blockContainer);
    }

    private void GenerateBoard(Transform container)
    {
        int numTile = numRow * numColumn;
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;

        GameObject[] tiles = new GameObject[numTile];

        for (int i = 0; i < numTile; i++)
        {
            tiles[i] = (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab, container);
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

        EditorUtility.SetDirty(container);
    }

    private void SnapAll()
    {
        BaseBlock[] blocks = TransformUtil.GetComponentsFromAllChildren<BaseBlock>(Selection.activeTransform).ToArray();

        for (int i = 0; i < blocks.Length; i++)
        {
            BaseBlock parentBlock = blocks[i].transform.parent.GetComponent<BaseBlock>();

            if (parentBlock != null)
            {
                continue;
            }

            SnapUsingRaycast(blocks[i].transform);
        }
    }

    private void SnapUsingRaycast(Transform target)
    {
        float tileDistance = GetTileDistance();

        BaseBlock block = target.GetComponent<BaseBlock>();

        SceneManager.GetActiveScene().GetPhysicsScene().Raycast(target.position, Vector3.down, out RaycastHit hit, 5);

        if (hit.collider != null)
        {
            Vector3 _snapPosition = hit.collider.transform.position;

            if (_snapPosition.x > target.position.x)
            {
                _snapPosition.x -= (block.BlockProperty.NumTileX - 1) / 2f * tileDistance;
            }
            else
            {
                _snapPosition.x += (block.BlockProperty.NumTileX - 1) / 2f * tileDistance;
            }

            if (_snapPosition.z > target.position.z)
            {
                _snapPosition.z -= (block.BlockProperty.NumTileZ - 1) / 2f * tileDistance;
            }
            else
            {
                _snapPosition.z += (block.BlockProperty.NumTileZ - 1) / 2f * tileDistance;
            }

            if (block.BlockProperty.NumTileX % 2 == 1)
            {
                _snapPosition.x = hit.collider.transform.position.x;
            }

            if (block.BlockProperty.NumTileZ % 2 == 1)
            {
                _snapPosition.z = hit.collider.transform.position.z;
            }

            _snapPosition.y = target.transform.position.y;

            target.transform.position = _snapPosition;
        }
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

    private void GenerateBlocks(Transform container)
    {
        int totalTile = numRow * numColumn;
        int remainingTile = totalTile;
        float tileDistance = GetTileDistance();

        bool[] isTileFull = new bool[remainingTile];

        int tryTime = 0;
        int maxTryTime = 100;

        BaseBlock selectedPrefab;

        // Remove all existing blocks
        List<GameObject> toRemoveList = new List<GameObject>();

        for (int i = 0; i < container.childCount; i++)
        {
            toRemoveList.Add(container.GetChild(i).gameObject);
        }

        foreach (var item in toRemoveList)
        {
            DestroyImmediate(item);
        }

        while (remainingTile > 0.3f * totalTile && tryTime < maxTryTime)
        {
            int random = Random.Range(0, blocks.Length);

            selectedPrefab = blocks[random];

            int sizeX = selectedPrefab.BlockProperty.NumTileX;
            int sizeZ = selectedPrefab.BlockProperty.NumTileZ;
            int numTile = sizeX * sizeZ;

            Vector2Int coordinator = new Vector2Int();
            Vector2Int fallbackCoordinator = new Vector2Int(-1, -1);

            bool isFound = false;

            int randomStartZ = Random.Range(0, numRow - 1);
            int randomStartX = Random.Range(0, numColumn - 1);

            for (int i = randomStartZ; i < numRow; i++)
            {
                for (int j = randomStartX; j < numColumn; j++)
                {
                    bool isValid = IsValidArea(isTileFull, j, i, sizeX, sizeZ);

                    if (!isValid)
                    {
                        continue;
                    }

                    if (i - (sizeX - 1) < 0 || i + (sizeX - 1) >= numRow)
                    {
                        isValid = false;

                        continue;
                    }

                    if (j - (sizeZ - 1) < 0 || j + (sizeZ - 1) >= numColumn)
                    {
                        isValid = false;

                        continue;
                    }

                    if (isValid)
                    {
                        int randomIsPicked = Random.Range(0, 10);

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

            BaseBlock block = PrefabUtility.InstantiatePrefab(selectedPrefab, container).GetComponent<BaseBlock>();

            Vector3 placedPosition = new Vector3();

            placedPosition.x = (-(numColumn - 1) / 2f + (coordinator.x + (sizeX - 1) / 2f)) * tileDistance;
            placedPosition.y = 2;
            placedPosition.z = ((numRow - 1) / 2f - (coordinator.y + (sizeZ - 1) / 2f)) * tileDistance;

            block.transform.position = placedPosition;

            for (int i = coordinator.y; i <= coordinator.y + sizeZ - 1; i++)
            {
                for (int j = coordinator.x; j <= coordinator.x + sizeX - 1; j++)
                {
                    int indexInBlockSpace = (j - coordinator.x) + (i - coordinator.y) * numColumn;

                    if (!IsBlockTileEmpty(indexInBlockSpace, block))
                    {
                        isTileFull[j + numColumn * i] = true;
                    }
                }
            }

            block.BlockProperty.Faction = (GameEnum.GameFaction)Random.Range(0, GameConstants.SCREW_FACTION.Length);
            block.BlockServiceLocator.blockMaterialPropertyBlock.SetFaction();

            remainingTile -= numTile;
        }

        EditorUtility.SetDirty(Selection.activeGameObject);

        bool IsBlockTileEmpty(int index, BaseBlock block)
        {
            for (int i = 0; i < block.BlockProperty.EmptyTileIndexes.Length; i++)
            {
                if (index == block.BlockProperty.EmptyTileIndexes[i])
                {
                    return true;
                }
            }

            return false;
        }
    }

    private void GenerateBarricades(Transform container, Transform blockContainer)
    {
        int totalBarricadeTiles = 2 * (numRow + numColumn);
        int remainingBarricadeTiles = totalBarricadeTiles;

        int expandedNumRow = numRow + 2;
        int expandedNumColumn = numColumn + 2;

        bool[] isTileFull = new bool[expandedNumRow * expandedNumColumn];

        for (int i = 0; i < expandedNumRow; i++)
        {
            for (int j = 0; j < expandedNumColumn; j++)
            {
                int index = j + i * expandedNumColumn;

                if (j > 0 && j < expandedNumColumn - 1 && i > 0 && i < expandedNumRow - 1)
                {
                    isTileFull[index] = true;

                    continue;
                }

                if (i == 0 && j == 0)
                {
                    isTileFull[index] = true;

                    continue;
                }

                if (i == 0 && j == expandedNumColumn - 1)
                {
                    isTileFull[index] = true;

                    continue;
                }

                if (i == expandedNumRow - 1 && j == 0)
                {
                    isTileFull[index] = true;

                    continue;
                }

                if (i == expandedNumRow - 1 && j == expandedNumColumn - 1)
                {
                    isTileFull[index] = true;

                    continue;
                }

                isTileFull[index] = false;
            }
        }

        // Remove all existing blocks
        List<GameObject> toRemoveList = new List<GameObject>();

        for (int i = 0; i < container.childCount; i++)
        {
            toRemoveList.Add(container.GetChild(i).gameObject);
        }

        foreach (var item in toRemoveList)
        {
            DestroyImmediate(item);
        }

        // Start
        Vector2Int coordinator = new Vector2Int();

        int sizeX = 0;
        int sizeZ = 0;

        bool isValid = false;
        int tryTime = 0;
        int maxTryTime = 200;

        List<BaseBarricade> generatedBarricades = new List<BaseBarricade>();

        while (remainingBarricadeTiles > 0 && tryTime < maxTryTime)
        {
            tryTime++;

            for (int i = 0; i < expandedNumRow; i++)
            {
                for (int j = 0; j < expandedNumColumn; j++)
                {
                    if (j > 0 && j < expandedNumColumn - 1 && i > 0 && i < expandedNumRow - 1)
                    {
                        continue;
                    }

                    int random = Random.Range(0, barricades.Length);

                    BaseBarricade selectedPrefab = barricades[random];

                    sizeX = selectedPrefab.BarricadeServiceLocator.BarricadeProperty.NumTileX;
                    sizeZ = selectedPrefab.BarricadeServiceLocator.BarricadeProperty.NumTileZ;

                    isValid = IsValidArea(isTileFull, j, i, sizeX, sizeZ, expandedNumRow, expandedNumColumn);

                    if (!isValid)
                    {
                        continue;
                    }

                    if (j - (sizeX - 1) < 0 || j + (sizeX - 1) >= expandedNumColumn)
                    {
                        isValid = false;

                        continue;
                    }

                    if (i - (sizeZ - 1) < 0 || i + (sizeZ - 1) >= expandedNumRow)
                    {
                        isValid = false;

                        continue;
                    }

                    if (isValid)
                    {
                        coordinator = new Vector2Int(j, i);

                        Vector2Int barricadeCoordinator = coordinator;

                        if (sizeZ > sizeX)
                        {
                            if (coordinator.x >= expandedNumColumn / 2f)
                            {
                                barricadeCoordinator.x++;
                            }
                            else
                            {
                                barricadeCoordinator.x--;
                            }
                        }
                        else
                        {
                            if (coordinator.y >= expandedNumRow / 2f)
                            {
                                barricadeCoordinator.y++;
                            }
                            else
                            {
                                barricadeCoordinator.y--;
                            }
                        }

                        BaseBarricade barricade = PrefabUtility.InstantiatePrefab(selectedPrefab, container).GetComponent<BaseBarricade>(); ;

                        generatedBarricades.Add(barricade);

                        Vector3 placedPosition = new Vector3();

                        placedPosition.x = (-(expandedNumColumn - 1) / 2f + (coordinator.x + (sizeX - 1) / 2f)) * GetTileDistance();
                        placedPosition.y = -1.5f;
                        placedPosition.z = ((expandedNumRow - 1) / 2f - (coordinator.y + (sizeZ - 1) / 2f)) * GetTileDistance();

                        barricade.transform.position = placedPosition;

                        if (barricade.BarricadeProperty.NumTileX == 1 && barricade.BarricadeProperty.NumTileZ == 1)
                        {
                            if (coordinator.x == 0 || coordinator.x == expandedNumColumn - 1)
                            {
                                barricade.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                            }
                        }

                        if (coordinator.x == 0)
                        {
                            barricade.Direction = Direction.Left;

                            barricade.transform.position += new Vector3(1.1f, 0, 0);
                        }
                        else if (coordinator.x == expandedNumColumn - 1)
                        {
                            barricade.Direction = Direction.Right;

                            barricade.transform.position -= new Vector3(1.1f, 0, 0);
                        }
                        else if (coordinator.y == 0)
                        {
                            barricade.Direction = Direction.Up;

                            barricade.transform.position -= new Vector3(0, 0, 1.1f);
                        }
                        else if (coordinator.y == expandedNumRow - 1)
                        {
                            barricade.Direction = Direction.Down;

                            barricade.transform.position += new Vector3(0, 0, 1.1f);
                        }

                        remainingBarricadeTiles -= sizeX * sizeZ;

                        break;
                    }
                }

                if (isValid)
                {
                    break;
                }
            }

            if (isValid)
            {
                for (int i = coordinator.y; i <= coordinator.y + sizeZ - 1; i++)
                {
                    for (int j = coordinator.x; j <= coordinator.x + sizeX - 1; j++)
                    {
                        isTileFull[j + expandedNumColumn * i] = true;
                    }
                }
            }
        }

        GenerateBarricadeFactions(generatedBarricades, blockContainer);

        EditorUtility.SetDirty(container);
    }

    private void GenerateBarricadeFactions(List<BaseBarricade> barricades, Transform blockContainer)
    {
        Dictionary<GameFaction, int> factionsWithMaxSize = GetFactionFromAllBlocks(blockContainer);

        int barricadeIndex = 0;

        bool isRandomDisabled;
        int random;

        // List<GameFaction> factionForAllBarricades = new List<GameFaction>();

        // factionForAllBarricades.AddRange(factions);

        List<BaseBarricade> remainingBarricades = barricades;

        foreach (var item in factionsWithMaxSize)
        {
            for (int j = 0; j < barricades.Count; j++)
            {
                BarricadeProperty barricadeProperty = barricades[j].BarricadeProperty;

                int barricadeMaxSize = Mathf.Max(barricadeProperty.NumTileX, barricadeProperty.NumTileZ);

                if (barricadeMaxSize >= item.Value)
                {
                    barricades[j].BarricadeServiceLocator.barricadeFaction.SetFaction(item.Key);

                    remainingBarricades.Remove(barricades[j]);

                    break;
                }
            }
        }

        for (int i = 0; i < remainingBarricades.Count; i++)
        {
            remainingBarricades[i].BarricadeServiceLocator.barricadeFaction.SetFaction(GameFaction.Disabled);
        }

        // factionForAllBarricades.Shuffle();

        // for (int i = barricadeIndex; i < barricades.Count; i++)
        // {
        //     barricades[i].BarricadeServiceLocator.barricadeFaction.SetFaction(factionForAllBarricades[i]);
        // }
    }

    private Dictionary<GameFaction, int> GetFactionFromAllBlocks(Transform blockContainer)
    {
        List<BaseBlock> blockList = TransformUtil.GetComponentsFromAllChildren<BaseBlock>(blockContainer);

        Dictionary<GameFaction, int> factionsWithMaxSize = new Dictionary<GameFaction, int>();

        foreach (var block in blockList)
        {
            if (!factionsWithMaxSize.ContainsKey(block.Faction))
            {
                int maxSize = Mathf.Max(block.BlockProperty.NumTileX, block.BlockProperty.NumTileZ);

                factionsWithMaxSize.Add(block.Faction, maxSize);
            }
            else
            {
                int maxSize = Mathf.Max(block.BlockProperty.NumTileX, block.BlockProperty.NumTileZ);

                if (maxSize > factionsWithMaxSize[block.Faction])
                {
                    factionsWithMaxSize[block.Faction] = maxSize;
                }
            }
        }

        return factionsWithMaxSize;
    }

    #region UTIL
    bool IsValidArea(bool[] isTileFull, int startX, int startZ, int sizeX, int sizeZ)
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

    bool IsValidArea(bool[] isTileFull, int startX, int startZ, int sizeX, int sizeZ, int numRow, int numColumn)
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
    #endregion
}
#endif
