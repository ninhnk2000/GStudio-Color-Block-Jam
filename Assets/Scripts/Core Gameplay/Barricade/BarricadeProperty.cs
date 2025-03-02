using System;
using UnityEngine;
using static GameEnum;

[Serializable]
public class BarricadeProperty
{
    [SerializeField] private GameFaction faction;
    [SerializeField] private Direction direction;
    [SerializeField] private int numTileX;
    [SerializeField] private int numTileZ;
    [SerializeField] private float additionalPositionX;

    public GameFaction Faction
    {
        get => faction;
        set => faction = value;
    }

    public Direction Direction
    {
        get => direction;
        set => direction = value;
    }

    public int NumTileX
    {
        get => numTileX;
    }

    public int NumTileZ
    {
        get => numTileZ;
    }

    public float AdditionalPositionX
    {
        get => additionalPositionX;
    }
}
