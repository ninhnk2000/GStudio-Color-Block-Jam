using System;
using UnityEngine;
using static GameEnum;

[Serializable]
public class BlockProperty
{
    [SerializeField] private GameFaction faction;
    [SerializeField] private int numTileX;
    [SerializeField] private int numTileZ;
    [SerializeField] private int[] emptyTileIndexes;

    // FOR LEVEL 10
    [SerializeField] public bool isCheckDisintegrationBothRightLeft;

    private bool _isMoving;
    private bool _isDisintegrating;
    private bool _isRotating;
    private bool _isPreventDisintegrating;
    private bool _isReadyTriggerDisintegrateFx;
    private bool _isDone;

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

    public bool IsRotating
    {
        get => _isRotating;
        set => _isRotating = value;
    }

    public bool IsPreventDisintegrating
    {
        get => _isPreventDisintegrating;
        set => _isPreventDisintegrating = value;
    }

    public bool IsReadyTriggerDisintegrateFx
    {
        get => _isReadyTriggerDisintegrateFx;
        set => _isReadyTriggerDisintegrateFx = value;
    }

    public bool IsDone
    {
        get => _isDone;
        set => _isDone = value;
    }

    public int NumTileX
    {
        get => numTileX;
    }

    public int NumTileZ
    {
        get => numTileZ;
    }

    public int[] EmptyTileIndexes
    {
        get => emptyTileIndexes;
    }

    public virtual bool IsMovable()
    {
        if (_isDisintegrating)
        {
            return false;
        }

        return true;
    }
}
