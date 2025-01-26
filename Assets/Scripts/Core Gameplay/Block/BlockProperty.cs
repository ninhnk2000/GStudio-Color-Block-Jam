using System;
using UnityEngine;

[Serializable]
public class BlockProperty
{
    [SerializeField] private int numTileX;
    [SerializeField] private int numTileZ;

    public int NumTileX
    {
        get => numTileX;
    }

    public int NumTileZ
    {
        get => numTileZ;
    }
}
