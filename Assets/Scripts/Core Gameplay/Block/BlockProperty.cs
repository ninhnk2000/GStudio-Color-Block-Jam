using System;
using UnityEngine;

[Serializable]
public class BlockProperty
{
    [SerializeField] private int numTileX;
    [SerializeField] private int numTileZ;

    private bool _isDisintegrating;

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
