using System;
using UnityEngine;
using static GameEnum;

[Serializable]
public class BarricadeProperty
{
    [SerializeField] private GameFaction faction;
    [SerializeField] private int numTileX;
    [SerializeField] private int numTileZ;

    public GameFaction Faction
    {
        get => faction;
        set => faction = value;
    }

    public int NumTileX
    {
        get => numTileX;
    }

    public int NumTileZ
    {
        get => numTileZ;
    }
}
