using System;
using UnityEngine;
using static GameEnum;

[Serializable]
public class ScrewData
{
    [SerializeField] private int _screwId;
    [SerializeField] private GameFaction _faction;
    [SerializeField] private bool _isDone;
    [SerializeField] private bool _isInScrewPort;
    [SerializeField] private bool _isDestroyed;

    public int ScrewId
    {
        get => _screwId;
        set => _screwId = value;
    }

    public GameFaction Faction
    {
        get => _faction;
        set => _faction = value;
    }

    public bool IsDone
    {
        get => _isDone;
        set => _isDone = value;
    }

    public bool IsInScrewPort
    {
        get => _isInScrewPort;
        set => _isInScrewPort = value;
    }

    public bool IsDestroyed
    {
        get => _isDestroyed;
        set => _isDestroyed = value;
    }
}
