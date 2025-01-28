using System;
using UnityEngine;
using static GameEnum;

[Serializable]
public class BlockProperty
{
    [SerializeField] private GameFaction faction;
    [SerializeField] private int numTileX;
    [SerializeField] private int numTileZ;

    private bool _isMoving;
    private bool _isDisintegrating;

    public GameFaction Faction
    {
        get => faction;
        set => faction = value;
    }

    public bool IsMoving
    {
        get => _isMoving;
        set => _isMoving = value;
    }

    public bool IsDisintegrating
    {
        get => _isDisintegrating;
        set => _isDisintegrating = value;
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
