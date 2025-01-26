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
    [SerializeField] private LevelBoosterObserver levelBoosterObserver;

    private LevelDifficultyConfiguration _currentLevelDifficultyConfiguration;
    private int _maxLevel;

    public static event Action startLevelEvent;
    public static event Action setLevelNumberEvent;
    public static event Action<int> setLevelScrewNumberEvent;
    public static event Action<int> setMultiPhaseLevelScrewNumberEvent;
    public static event Action showSceneTransitionEvent;

    private void Awake()
    {
        PausePopup.replayLevelEvent += Replay;
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

                _currentLevelDifficultyConfiguration = LevelDifficultyCalculatorUltility.GenerateScrewBoxDifficultyTemplate(currentLevel.Value);

                AutoAssignScrewFaction(level);
                AutoAssignScrewFactionMultiPhaseLevel(level);
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

        levelBoosterObserver.Reset();

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

    private void AutoAssignScrewFactionMultiPhaseLevel(GameObject level)
    {
        MultiPhaseScrew[] screws = TransformUtil.GetComponentsFromAllChildren<MultiPhaseScrew>(level.transform).ToArray();

        Dictionary<int, List<GameFaction>> remainingFactionForScrewsByPhase = new Dictionary<int, List<GameFaction>>();

        Dictionary<int, int> currentFactionByPhase = new Dictionary<int, int>();

        for (int i = 0; i < screws.Length; i++)
        {
            int phase = screws[i].Phase;

            if (!remainingFactionForScrewsByPhase.ContainsKey(phase))
            {
                currentFactionByPhase.Add(phase, 0);

                remainingFactionForScrewsByPhase.Add(phase, new List<GameFaction>() { GameConstants.SCREW_FACTION[currentFactionByPhase[phase]] });
            }
            else
            {
                remainingFactionForScrewsByPhase[phase].Add(GameConstants.SCREW_FACTION[currentFactionByPhase[phase]]);

                if (i > 0 && (i + 1) % 3 == 0)
                {
                    currentFactionByPhase[phase]++;

                    if (currentFactionByPhase[phase] >= GameConstants.SCREW_FACTION.Length)
                    {
                        currentFactionByPhase[phase] = 0;
                    }
                }
            }
        }

        for (int i = 0; i < screws.Length; i++)
        {
            int phase = screws[i].Phase;

            int randomIndex = UnityEngine.Random.Range(0, remainingFactionForScrewsByPhase[phase].Count);

            screws[i].ScrewId = i;
            screws[i].Faction = remainingFactionForScrewsByPhase[phase][randomIndex];

            screws[i].ScrewServiceLocator.screwFaction.SetColorByFaction();

            remainingFactionForScrewsByPhase[phase].RemoveAt(randomIndex);
        }

        setMultiPhaseLevelScrewNumberEvent?.Invoke(screws.Length);
    }

    private void AutoAssignScrewFaction(GameObject level)
    {
        BaseScrew[] screws = TransformUtil.GetComponentsFromAllChildren<BaseScrew>(level.transform).ToArray();

        int numScrew = screws.Length;

        List<BaseScrew> noBlockedScrews = new List<BaseScrew>();
        List<BaseScrew> blockedScrews = new List<BaseScrew>();

        for (int i = 0; i < screws.Length; i++)
        {
            int numBlockedObject = screws[i].CountBlockingObjects(isIncludeHiddenScrew: true);

            if (numBlockedObject == 0)
            {
                noBlockedScrews.Add(screws[i]);
            }
            else
            {
                blockedScrews.Add(screws[i]);
            }
        }

        // SHUFFLE SCREW LISTS
        ShuffleList(noBlockedScrews);
        ShuffleList(blockedScrews);

        List<BaseScrew> shuffledScrews = new List<BaseScrew>();

        float ratioGeneratingSameFactionForTripleNoBlockedScrewStartGame = _currentLevelDifficultyConfiguration.RatioGeneratingSameFactionForTripleNoBlockedScrewStartGame;

        // float ratioGeneratingSameFactionForTripleNoBlockedScrewStartGame =
        //     levelDataContainer.LevelsData[currentLevel.Value].LevelDifficultyConfiguration.RatioGeneratingSameFactionForTripleNoBlockedScrewStartGame;

        int numNoBlockedScrewsToGetSameFaction = (int)(ratioGeneratingSameFactionForTripleNoBlockedScrewStartGame * noBlockedScrews.Count);
        int numRemainingScrews = numScrew - numNoBlockedScrewsToGetSameFaction;

        // EASY SCREWS
        for (int i = 0; i < numNoBlockedScrewsToGetSameFaction; i++)
        {
            BaseScrew nextScrew = noBlockedScrews.First();

            shuffledScrews.Add(nextScrew);

            noBlockedScrews.Remove(nextScrew);
        }

        // REMAINING SCREWS
        int currentIndex = 0;

        for (int i = 0; i < numRemainingScrews; i++)
        {
            BaseScrew nextScrew;

            if (currentIndex % (GameConstants.NUMBER_SLOT_PER_SCREW_BOX - 1) == 0)
            {
                if (noBlockedScrews.Count > 0)
                {
                    nextScrew = noBlockedScrews.First();

                    noBlockedScrews.Remove(nextScrew);
                }
                else
                {
                    nextScrew = blockedScrews.First();

                    blockedScrews.Remove(nextScrew);
                }
            }
            else
            {
                if (blockedScrews.Count > 0)
                {
                    nextScrew = blockedScrews.First();

                    blockedScrews.Remove(nextScrew);
                }
                else
                {
                    nextScrew = noBlockedScrews.First();

                    noBlockedScrews.Remove(nextScrew);
                }
            }

            shuffledScrews.Add(nextScrew);

            currentIndex++;
        }

        // // NO BLOCKED SCREW
        // int currentFaction = 0;
        // bool isGeneratingSameFactionForTripleScrews = false;

        // List<GameFaction> remainingFactionForNoBlockedScrews = new List<GameFaction>();

        // for (int i = 0; i < noBlockedScrews.Count; i++)
        // {
        //     if (i % GameConstants.NUMBER_SLOT_PER_SCREW_BOX == 0)
        //     {
        //         float random = UnityEngine.Random.Range(0f, 1f);

        //         if (random < levelDataContainer.LevelsData[currentLevel.Value].LevelDifficultyConfiguration.RatioGeneratingSameFactionForTripleNoBlockedScrewStartGame)
        //         {
        //             isGeneratingSameFactionForTripleScrews = true;
        //         }
        //         else
        //         {
        //             isGeneratingSameFactionForTripleScrews = false;
        //         }
        //     }

        //     if (isGeneratingSameFactionForTripleScrews)
        //     {
        //         remainingFactionForNoBlockedScrews.Add(GameConstants.SCREW_FACTION[currentFaction]);
        //     }
        //     else
        //     {
        //         int randomFactionIndex = UnityEngine.Random.Range(0, GameConstants.SCREW_FACTION.Length);

        //         remainingFactionForNoBlockedScrews.Add(GameConstants.SCREW_FACTION[randomFactionIndex]);
        //     }

        //     if (i > 0 && (i + 1) % 3 == 0)
        //     {
        //         currentFaction++;

        //         if (currentFaction >= GameConstants.SCREW_FACTION.Length)
        //         {
        //             currentFaction = 0;
        //         }
        //     }
        // }

        // int currentFaction = UnityEngine.Random.Range(0, GameConstants.SCREW_FACTION.Length - 1);
        int currentFaction = 0;

        List<GameFaction> remainingFactionForBlockedScrews = new List<GameFaction>();

        for (int i = 0; i < shuffledScrews.Count; i++)
        {
            remainingFactionForBlockedScrews.Add(GameConstants.SCREW_FACTION[currentFaction]);

            if (i > 0 && (i + 1) % 3 == 0)
            {
                currentFaction++;

                if (currentFaction >= GameConstants.SCREW_FACTION.Length)
                {
                    currentFaction = 0;
                }
            }
        }

        for (int i = 0; i < shuffledScrews.Count; i++)
        {
            GameFaction nextFaction = remainingFactionForBlockedScrews.First();

            shuffledScrews[i].ScrewId = i;
            shuffledScrews[i].Faction = nextFaction;

            shuffledScrews[i].ScrewServiceLocator.screwFaction.SetColorByFaction();

            remainingFactionForBlockedScrews.Remove(nextFaction);
        }

        // RAMDOMIZE
        // for (int i = 0; i < shuffledScrews.Count; i++)
        // {
        //     int randomIndex = UnityEngine.Random.Range(0, remainingFactionForBlockedScrews.Count);

        //     shuffledScrews[i].ScrewId = i;
        //     shuffledScrews[i].Faction = remainingFactionForBlockedScrews[randomIndex];

        //     shuffledScrews[i].ScrewServiceLocator.screwFaction.SetColorByFaction();

        //     remainingFactionForBlockedScrews.RemoveAt(randomIndex);
        // }

        setLevelScrewNumberEvent?.Invoke(shuffledScrews.Count);
    }

    void ShuffleList<T>(List<T> list)
    {
        System.Random rand = new System.Random();
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
