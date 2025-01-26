using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    [SerializeField] private LevelDifficultyConfiguration levelDifficultyConfiguration;

    public LevelDifficultyConfiguration LevelDifficultyConfiguration
    {
        get => levelDifficultyConfiguration;
    }
}
