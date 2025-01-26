using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/MultiPhaseLevelData")]
public class MultiPhaseLevelData : ScriptableObject
{
    [SerializeField] private int level;
    [SerializeField] private PhaseData[] phasesData;

    public int Level
    {
        get => level;
    }

    public PhaseData[] PhasesData
    {
        get => phasesData;
    }
}

[Serializable]
public class PhaseData
{
    public int phase;
    public float cameraOrthographicSize;
}
