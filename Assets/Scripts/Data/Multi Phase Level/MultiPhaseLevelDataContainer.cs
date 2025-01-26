using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/MultiPhaseLevelDataContainer")]
public class MultiPhaseLevelDataContainer : ScriptableObject
{
    [SerializeField] private MultiPhaseLevelData[] items;

    public MultiPhaseLevelData[] Items
    {
        get => items;
    }
}
