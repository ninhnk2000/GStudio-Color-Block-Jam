using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Saferio/ScrewAway/LevelDataContainer")]
public class LevelDataContainer : ScriptableObject
{
    [SerializeField] private int maxLevel;
    [SerializeField] private LevelData[] levelsData;
    [SerializeField] private LevelDifficultyConfiguration[] templateLevelDifficultyConfiguration;
    [SerializeField] private LevelDifficultyConfiguration fallbackLevelDifficultyConfiguration;

    public int MaxLevel
    {
        get => maxLevel;
    }

    public LevelData[] LevelsData
    {
        get => levelsData;
    }

    // public LevelDifficultyConfiguration GetLevelDifficultyConfiguration(int level)
    // {
    //     int levelIndex = level - 1;

    //     if (levelIndex < levelsData.Length)
    //     {
    //         if (levelsData[levelIndex].LevelDifficultyConfiguration != null)
    //         {
    //             return levelsData[levelIndex].LevelDifficultyConfiguration;
    //         }
    //     }

    //     return fallbackLevelDifficultyConfiguration;
    // }

    public LevelDifficultyConfiguration GetLevelDifficultyConfiguration(int level)
    {
        int modulus = (level - 1) % 5;

        return templateLevelDifficultyConfiguration[modulus];
    }

    public void SetLevelDifficultyConfiguration(int level, LevelPhase[] levelPhases)
    {
        int levelIndex = level - 1;

        if (levelIndex < levelsData.Length)
        {
            if (levelsData[levelIndex].LevelDifficultyConfiguration != null)
            {
                levelsData[levelIndex].LevelDifficultyConfiguration.LevelPhases = levelPhases;
            }
        }
    }
}
