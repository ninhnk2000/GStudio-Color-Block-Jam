using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static GameEnum;

public class LevelLoader : MonoBehaviour
{
    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private LevelDataContainer levelDataContainer;

    private int _maxLevel;

    public static event Action startLevelEvent;
    public static event Action setLevelNumberEvent;
    public static event Action<int> setLevelScrewNumberEvent;
    public static event Action<int> setMultiPhaseLevelScrewNumberEvent;
    public static event Action showSceneTransitionEvent;
    public static event Action<BaseBlock[]> sendLevelBaseBlocksDataEvent;
    public static event Action<float, float, float, float> updateBoundEvent;
    public static event Action<float> setLevelCameraOrthographicSize;

    private void Awake()
    {
        PausePopup.replayLevelEvent += Replay;
        ReplayPopup.replayLevelEvent += Replay;
        LosePopup.replayLevelEvent += Replay;
        DebugPopup.toLevelEvent += GoLevel;
        DebugPopup.nextLevelEvent += NextLevel;
        DebugPopup.prevLevelEvent += PrevLevel;
        WinPopup.nextLevelEvent += NextLevel;
        WinPopup.goLevelEvent += GoLevel;

        currentLevel.Load();

        _maxLevel = levelDataContainer.MaxLevel;

        GoLevel(currentLevel.Value);
    }

    void OnDestroy()
    {
        PausePopup.replayLevelEvent -= Replay;
        ReplayPopup.replayLevelEvent -= Replay;
        LosePopup.replayLevelEvent -= Replay;
        DebugPopup.toLevelEvent -= GoLevel;
        DebugPopup.nextLevelEvent -= NextLevel;
        DebugPopup.prevLevelEvent -= PrevLevel;
        WinPopup.nextLevelEvent -= NextLevel;
        WinPopup.goLevelEvent -= GoLevel;
    }

    private async Task LoadLevel()
    {
        int nextLevel = 1;

        if (!UserData.IsLoopLevel)
        {
            if (currentLevel.Value <= _maxLevel)
            {
                nextLevel = currentLevel.Value;
            }
            else
            {
                nextLevel = UnityEngine.Random.Range(6, _maxLevel);

                await UserData.SetIsLoopLevel(true);
                await UserData.SetIsLastLevelBeforeLoop(_maxLevel);
            }
        }
        else
        {
            if (currentLevel.Value - UserData.NumLevelLoop >= _maxLevel)
            {
                nextLevel = UnityEngine.Random.Range(6, _maxLevel);

                // await UserData.SetNumLevelLoop(UserData.NumLevelLoop + 1);
            }
            else
            {
                nextLevel = currentLevel.Value - UserData.NumLevelLoop;
            }
        }

        nextLevel = Mathf.Clamp(nextLevel, 1, _maxLevel);

        setLevelNumberEvent?.Invoke();

        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>($"Level {nextLevel}");

        handle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject level = Instantiate(op.Result, transform);

                LevelCameraModifier levelCameraModifier = level.GetComponent<LevelCameraModifier>();

                if (levelCameraModifier != null)
                {
                    setLevelCameraOrthographicSize?.Invoke(levelCameraModifier.FieldOfView);
                }
                else
                {
                    setLevelCameraOrthographicSize?.Invoke(GameConstants.DEFAULT_CAMERA_FIELD_OF_VIEW);
                }

                ManageBaseBlocks(level.transform);
            }
        };

        showSceneTransitionEvent?.Invoke();
    }

    private async void GoLevel(int level)
    {
        await Task.Delay(500);

        if (gameObject.transform.childCount > 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }

        currentLevel.Value = level;

        currentLevel.Save();

        await currentLevel.SaveAsync();

        startLevelEvent?.Invoke();

        LoadLevel();
    }

    private void NextLevel()
    {
        GoLevel(currentLevel.Value + 1);
    }

    private void PrevLevel()
    {
        GoLevel(currentLevel.Value - 1);
    }

    private void Replay()
    {
        GoLevel(currentLevel.Value);
    }

    private void ManageBaseBlocks(Transform level)
    {
        BaseBlock[] blocks = TransformUtil.GetComponentsFromAllChildren<BaseBlock>(level).ToArray();

        sendLevelBaseBlocksDataEvent?.Invoke(blocks);

        BoardTile[] boardTiles = TransformUtil.GetComponentsFromAllChildren<BoardTile>(level).ToArray();
        BaseBarricade[] barricades = TransformUtil.GetComponentsFromAllChildren<BaseBarricade>(level).ToArray();

        float tileSize = boardTiles[0].GetComponent<MeshRenderer>().bounds.size.x;

        float rightBound = 0;
        float leftBound = float.MaxValue;
        float topBound = 0;
        float bottomBound = float.MaxValue;

        for (int i = 0; i < boardTiles.Length; i++)
        {
            Vector3 position = boardTiles[i].transform.position;

            if (position.x > rightBound)
            {
                rightBound = position.x;
            }

            if (position.x < leftBound)
            {
                leftBound = position.x;
            }

            if (position.z > topBound)
            {
                topBound = position.z;
            }

            if (position.z < bottomBound)
            {
                bottomBound = position.z;
            }
        }

        rightBound += 0.5f * tileSize;
        leftBound -= 0.5f * tileSize;
        topBound += 0.5f * tileSize;
        bottomBound -= 0.5f * tileSize;

        updateBoundEvent?.Invoke(rightBound, leftBound, topBound, bottomBound);

        for (int i = 0; i < barricades.Length; i++)
        {
            if (barricades[i].transform.position.x <= leftBound)
            {
                barricades[i].Direction = Direction.Left;

                barricades[i].transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else if (barricades[i].transform.position.x >= rightBound)
            {
                barricades[i].Direction = Direction.Right;

                barricades[i].transform.eulerAngles = new Vector3(0, 270, 0);
            }
            else if (barricades[i].transform.position.z >= topBound)
            {
                barricades[i].Direction = Direction.Up;

                barricades[i].transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (barricades[i].transform.position.z <= bottomBound)
            {
                barricades[i].Direction = Direction.Down;

                barricades[i].transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}
